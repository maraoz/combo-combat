#!/usr/bin/env python


import webapp2, json, jinja2, os, hashlib

from google.appengine.ext import db

from model import Player

jinja_environment = jinja2.Environment(
    loader=jinja2.FileSystemLoader(os.path.dirname(__file__)))


def hash_digest(x):
    hasher = hashlib.new('ripemd160')
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
        
        
        player = Player(username=username, password=hash_digest(password))
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
        return {"success": True}
        
class SearchHandler(JsonAPIHandler):
    def handle(self):
        username = self.request.get("u")
        if not username:
            return {"success": False, "error": "format"}
        player = Player.all().filter('username = ', username).get()
        if not player:
            return {"success": False, "error": "nonexisting"}
        
        other = Player.all().filter('searching = ', True).get()
        if other:
            
            server = Server.get_free()
            
            other.searching = False
            player.searching = False
            other.match_server = server
            player.match_server = server
            server.free = False
            return {}
        
        player.searching = True
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
        
        return {"success": True, "host": player.match_server.host}
    
class ResetHandler(JsonAPIHandler):
    def handle(self):
        host = self.request.get("h")
        if not host:
            return {"success": False, "error": "format"}

        server = Server.all().filter('host = ', host).get()
        if not server:
            return {"success": False, "error": "nonexisting"}
        server.free = True
        return {"success": True}



app = webapp2.WSGIApplication([
    ('/', MainHandler),
    ('/api/register', RegisterHandler),
    ('/api/login', LoginHandler),
    ('/api/search', SearchHandler),
    ('/api/check', CheckHandler),
    ('/api/reset', ResetHandler)
    
], debug=False)

