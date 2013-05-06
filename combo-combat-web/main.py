#!/usr/bin/env python


import webapp2, json, jinja2, os, hashlib

from google.appengine.ext import db

from model import Player

jinja_environment = jinja2.Environment(
    loader=jinja2.FileSystemLoader(os.path.dirname(__file__)))


def hash(x):
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
            return {"registered": False , "error": "format"}
        
        same_name = Player.all().filter('username =', username)
        if same_name.get():
            return {"registered": False , "error": "username"}
        
        
        player = Player(username=username, password=hash(password))
        player.put()
        return {"registered": True}

class LoginHandler(JsonAPIHandler):
    def handle(self):
        username = self.request.get("u")
        password = self.request.get("p")
        
        if not username or not password:
            return {"login": False , "error": "format"}
        
        player = Player.all().filter('username =', username).filter('password', hash(password)).get()
        if not player:
            return {"login": False , "error": "failed"}
        return {"login": True}
        
        

app = webapp2.WSGIApplication([
    ('/', MainHandler),
    ('/api/register', RegisterHandler),
    ('/api/login', LoginHandler)
], debug=False)

