
from google.appengine.ext import db


class Player(db.Model):
    """Models an individual Player, with username, email, and password"""
    username = db.StringProperty()
    password = db.StringProperty()
    
    joined = db.DateTimeProperty(auto_now_add=True)
