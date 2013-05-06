
from google.appengine.ext import db


class Player(db.Model):
    """Models an individual Player, with username, email, and password"""
    username = db.StringProperty()
    password = db.StringProperty()
    
    joined = db.DateTimeProperty(auto_now_add=True)


class MatchHistory(db.Model):
    """Models a match played"""
    players = db.ListProperty(db.ReferenceProperty(Player, collection_name='matches'))
    scores = db.ListProperty(db.IntegerProperty)
    elo_delta = db.IntegerProperty()
    duration = db.IntegerProperty()
    
    played = db.DateTimeProperty(auto_now_add=True)
    