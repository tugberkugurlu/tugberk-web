# Tugberk.Web [![Build Status](https://travis-ci.com/tugberkugurlu/tugberk-web.svg?token=bSbyNzszQnYLxQxRyPu1&branch=master)](https://travis-ci.com/tugberkugurlu/tugberk-web)

This is yet another blog engine. I mainly use this as an excuse to try out new technologies by improving my blog platform.

## Main Focus of Improvment

Idea is to replicate what I current have and then build on top of that. A few important improvments I am looking for after this:

 - I cannot publish easily today because AtomPub being very restricted on supported publishing tools. That's the main thing to improve
 - I am super unhappy with RavenDB. Let's get rid of that and make the peristance layer agnostic from the database engine so that I can swap it anytime I want.
 - Deployment is currently hard :s Let's strive for hosting this on Azure App Service and get rid of the server I have up and running for a while.
 - I want to have ads in the middle of posts. That would be amazing if I can get this done as well.
 - About page should be based on LinkedIn

## TODOs

 - [x] Bring down Bootstrap 4.0
 - [x] Create home page strcuture
 - [x] Create domain layer
 - [ ] Create persistance interfaces
 - [ ] Create in-memory persistance implementation
 - [ ] Create SQL Server persistance implementation based on EF
 - [ ] Handle google analytics on prod
 - [ ] Decide on prod logging
 - [ ] Twitter cards
 - [ ] RSS Feed

### Home Page

 - [ ] List last 5 blog posts on the home page
 - [ ] Pagination on the home page
 - [ ] Ads

### Blog Post Page

 - [x] Create blog post page
 - [ ] Ads

### Sidebar

 - [x] Replicate the current one
 - [ ] Add youtube channel link
 - [ ] Ads

### Other Pages

 - [x] Create about page (static for now)
 - [ ] Create speaker page (basic info for now)

### Hosting

 - [ ] Host on Azure App Service
 - [ ] HTTPS by default, so buy a cert

### Migration

 - [ ] Migrate comments to disqus
 - [ ] Migrate blog posts to Azure SQL Database based on the SQL Structure
 - [ ] 

Now at this point, release!

## v2 TODOs

Theme here is to provide the management portal.

### Authentication

 - [ ] Enable authentication through Twitter
 - [ ] Lock management part with authorization
 - [ ] Provide a page to add new blog post in HTML format
 - [ ] Provide a page to edit the content of a blog post: HTML content, title, abstract and tags
 - [ ] Provide a facility to approve and disaprove a blog post.
 - [ ] Provide a facility to allow and disallow comments on a blog post.

## Further TODOs

 - Would be nice to be to tell how many times a blog post has been read by looking into Google Analytics