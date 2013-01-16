#!/usr/bin/env python


import webapp2, json, jinja2, os, urllib, datetime

from google.appengine.ext import db
from google.appengine.ext import blobstore
from google.appengine.ext.webapp import blobstore_handlers

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
                



class LatestClientHandler(blobstore_handlers.BlobstoreDownloadHandler):
    def get(self):
        latest = GameClient.all().order('-stamp').get()

        blob_info = latest.blob_key
    
        self.send_blob(blob_info, save_as=blob_info.filename)


    

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
    ('/latest', LatestClientHandler)
], debug=True)

