#!/usr/bin/env python


import webapp2, json, jinja2, os, hashlib

from google.appengine.ext import db

from model import Player, Server, MatchHistory
from ranking import calculate_elos, PLAYER_A, PLAYER_B, DEFAULT_ELO

jinja_environment = jinja2.Environment(
    loader=jinja2.FileSystemLoader(os.path.dirname(__file__)))


def hash_digest(x):
    hasher = hashlib.new('SHA512')
    hasher.update(x)
    return hasher.hexdigest()


class MainHandler(webapp2.RequestHandler):
    def get(self):
        template = jinja_environment.get_template('index.html')
        self.response.out.write(template.render({}))

class JsonAPIHandler(webapp2.RequestHandler):
    def post(self):
        self.get()
    def get(self):
        resp = self.handle()
        self.response.write(json.dumps(resp))

class RegisterHandler(JsonAPIHandler):
    def handle(self):
        username = self.request.get("u")
        password = self.request.get("p")
        
        if not username or not password:
            return {"success": False , "error": "format"}
        
        same_name = Player.all().filter('username =', username)
        if same_name.get():
            return {"success": False , "error": "username"}
        
        
        player = Player(
            username=username, 
            password=hash_digest(password),
            counter = 0,
            elo = DEFAULT_ELO,
            searching=False, 
            match_server=None)
        player.put()
        return {"success": True}

class LoginHandler(JsonAPIHandler):
    def handle(self):
        username = self.request.get("u")
        password = self.request.get("p")
        
        if not username or not password:
            return {"success": False , "error": "format"}
        
        player = Player.all().filter('username =', username).filter('password', hash_digest(password)).get()
        if not player:
            return {"success": False , "error": "failed"}
        if not player.counter:
            player.counter = 0
        player.counter += 1
        player.put()
        return {"success": True}
        
class SearchHandler(JsonAPIHandler):
    def handle(self):
        username = self.request.get("u")
        if not username:
            return {"success": False, "error": "format"}
        player = Player.all().filter('username = ', username).get()
        if not player:
            return {"success": False, "error": "nonexisting"}
        if player.searching:
            return {"success": False, "error": "already searching"}
        
        server = Server.get_free()
        if not server:
            return {"success": False, "error": "no free servers"}
        
        other = Player.all().filter('searching = ', True).filter('username !=', username).get()
        if other:
            
            other.searching = False
            other.match_server = server
            player.match_server = server
            server.free = False
            
            other.put()
            player.put()
            server.put()
            
            return {"success": True}
        
        player.searching = True
        player.put()
        
        return {"success": True}

class CheckHandler(JsonAPIHandler):
    def handle(self):
        username = self.request.get("u")
        if not username:
            return {"success": False, "error": "format"}

        player = Player.all().filter('username = ', username).get()
        if not player:
            return {"success": False, "error": "nonexisting"}
        
        if not player.match_server:
            return {"success": True, "host": None}
        
        server_host = player.match_server.host
        
        player.match_server = None
        player.put()
        
        return {"success": True, "host": server_host}
    
class ResetHandler(JsonAPIHandler):
    def handle(self):
        host = self.request.get("h")
        players = self.request.get("p")
        scores = self.request.get("s")
        duration = self.request.get("d")
        
        # preconditions
        try:
            players = players.split(",")
            playera, playerb = tuple(players)
            scorea, scoreb = tuple([int(x) for x in scores.split(",")])
            scores = [scorea, scoreb]
        except Exception:
            players, scores = None, None
        try:
            duration = long(float(duration))
        except ValueError:
            duration = None
        if not host or not players or not scores or not duration:
            return {"success": False, "error": "format"}

        server = Server.all().filter('host = ', host).get()
        if not server:
            return {"success": False, "error": "nonexisting"}
        
        
        # actual processing
        server.free = True
        
        playera = Player.all().filter('username = ', playera).get()
        playerb = Player.all().filter('username = ', playerb).get()
        
        
        (pa_new_elo, pb_new_elo) = calculate_elos(playera.elo, playerb.elo, PLAYER_A if scorea > scoreb else PLAYER_B)
        
        mh = MatchHistory(
            players = players,
            scores = scores,
            elo_delta = playera.elo - playerb.elo,
            duration = duration)    
        
        playera.elo = int(pa_new_elo)
        playerb.elo = int(pb_new_elo)
        
        server.put()
        playera.put()
        playerb.put()
        mh.put()
        
        return {"success": True}
    
class AddServerHandler(JsonAPIHandler):
    def handle(self):
        host = self.request.get("h")
        if not host:
            return {"success": False, "error": "format"}

        server = Server.all().filter('host = ', host).get()
        if server:
            return {"success": False, "error": "existing"}
        
        server = Server(host=host, free=True)
        server.put()
        
        return {"success": True}


app = webapp2.WSGIApplication([
    ('/', MainHandler),
    ('/api/register', RegisterHandler),
    ('/api/login', LoginHandler),
    ('/api/search', SearchHandler),
    ('/api/check', CheckHandler),
    ('/api/reset', ResetHandler),
    ('/api/server', AddServerHandler)
    
], debug=False)

