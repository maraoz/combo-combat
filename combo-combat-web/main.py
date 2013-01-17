#!/usr/bin/env python


import webapp2, json, jinja2, os, urllib, datetime, hashlib
from datetime import timedelta

import aetycoon

from google.appengine.ext import db
from google.appengine.ext import blobstore
from google.appengine.ext.webapp import blobstore_handlers

HTTP_DATE_FMT = "%a, %d %b %Y %H:%M:%S %Z"

jinja_environment = jinja2.Environment(
    loader=jinja2.FileSystemLoader(os.path.dirname(__file__)))





class GameClient(db.Model):
  version = db.StringProperty(required=True)
  blob_key = blobstore.BlobReferenceProperty(required=True)
  stamp = db.DateTimeProperty()

  




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
        return username
                

"""
Accept-Ranges:bytes
Cache-Control:max-age=300
Connection:keep-alive
Content-Length:21565931
Content-Type:text/plain; charset=UTF-8
Date:Wed, 16 Jan 2013 17:54:27 GMT
ETag:"14911eb-4d161ecfed840"
Expires:Wed, 16 Jan 2013 17:59:27 GMT
Last-Modified:Fri, 21 Dec 2012 19:33:45 GMT
Server:Footprint Distributor V4.8
"""



class LatestClientHandler(blobstore_handlers.BlobstoreDownloadHandler):
  def get(self):
    latest = GameClient.all().order('-stamp').get()
    serve = True
    if 'If-Modified-Since' in self.request.headers:
      last_seen = datetime.datetime.strptime(
                self.request.headers['If-Modified-Since'],
                HTTP_DATE_FMT)
      if last_seen >= latest.stamp.replace(microsecond=0):
        serve = False
    if 'If-None-Match' in self.request.headers:
      etags = [x.strip('" ')
           for x in self.request.headers['If-None-Match'].split(',')]
      if latest.version in etags:
        serve = False
    self.output_content(latest, serve)
      
  def output_content(self, game_client, serve=True):
    if serve:
      blob_info = game_client.blob_key
      self.send_blob(blob_info)
    else:
      self.response.set_status(304)
    self.response.headers['Cache-Control'] = 'max-age=60, must-revalidate'
    exp = datetime.datetime.now() + timedelta(days=60)
    self.response.headers['Expires'] = exp.strftime(HTTP_DATE_FMT)
    self.response.headers['Content-Type'] = "application/vnd.unity"
    last_modified = (game_client.stamp- timedelta(days=60)).strftime(HTTP_DATE_FMT)
    self.response.headers['Last-Modified'] = last_modified
    self.response.headers['ETag'] = '"%s"' % (str(game_client.version),)
        





class ClientUpdateHandler(webapp2.RequestHandler):
  def get(self):
    upload_url = blobstore.create_upload_url('/upload')
    self.response.out.write('<html><body>')
    self.response.out.write('<form action="%s" method="POST" enctype="multipart/form-data">' % upload_url)
    self.response.out.write("""Upload File: <input type="file" name="file"><br>
        <input name="version"></input>
        <input type="submit" name="submit" value="Submit"> </form></body></html>""")

class UploadHandler(blobstore_handlers.BlobstoreUploadHandler):
  def post(self):
    version = self.request.get('version')
    upload_files = self.get_uploads('file')  # 'file' is file upload field in the form
    blob_info = upload_files[0]
    
    gc = GameClient(version = version,
             blob_key = blob_info.key())
    gc.stamp = datetime.datetime.now()
    gc.put()
    
    self.redirect('/')




app = webapp2.WSGIApplication([
    ('/', MainHandler), 
    ('/api/register', RegisterHandler),
    ('/admin/client', ClientUpdateHandler),
    ('/upload', UploadHandler),
    ('/latest.unity3d', LatestClientHandler),
    ('/test', LatestClientHandler)
], debug=False)

