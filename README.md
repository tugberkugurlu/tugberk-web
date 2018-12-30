# Tugberk.Web [![Build Status](https://travis-ci.com/tugberkugurlu/tugberk-web.svg?token=bSbyNzszQnYLxQxRyPu1&branch=master)](https://travis-ci.com/tugberkugurlu/tugberk-web)

This is yet another blog engine. I mainly use this as an excuse to try out new technologies by improving my blog platform.

## Development Experience

You can develop the software on Visual Studio, VS Code or JetBrainds Rider. If you would like to get the software up and running on a dev machibe, you first need to set up some environment variables:

```bash
export TUGBERKWEB_ConnectionStrings__DefaultConnection="<CONNECTION-STRING-HERE>"
export TUGBERKWEB_GoogleReCaptcha__Key="<GOOGLE-RECAPTCHA-KEY-HERE>"
export TUGBERKWEB_GoogleReCaptcha__Secret="<GOOGLE-RECAPTCHA-KEY-SECRET>"
```

Optionally, you can configure the following settings:

```bash
export TUGBERKWEB_GoogleAnalytics__TrackingId="<GOOGLE-ANALYTICS-TRACKINGID-HERE>"
```

As the next step, you need to manually install the npm dependencies:

```bash
cd ./src/Tugberk.Web/wwwroot
npm install
```

Then, you can run `dotnet run` under `./src/Tugberk.Web/` folder. The software will be available on `http://localhost:5000`.

### Building and Running the Docker Image

You can build the docker image to run the software locally inside a docker container.

```bash
docker build --tag tugberk/tugberk-web:v0.0.0 --file docker-tugberk-web.dockerfile --build-arg BUILDCONFIG=RELEASE .
docker run -p 5000:80 --env TUGBERKWEB_ConnectionStrings__DefaultConnection="<CONNECTION-STRING-HERE>" --env TUGBERKWEB_GoogleReCaptcha__Key="<GOOGLE-RECAPTCHA-KEY-HERE>" --env TUGBERKWEB_GoogleReCaptcha__Secret="<GOOGLE-RECAPTCHA-KEY-SECRET>" tugberk/tugberk-web:v0.0.0
```

After that, the software will be available on `http://localhost:5000` on the host machine.

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
 - [x] Create persistance interfaces
 - [ ] Create in-memory persistance implementation
 - [x] Create SQL Server persistance implementation based on EF
 - [ ] Handle google analytics on prod
 - [ ] Decide on prod logging
 - [x] Twitter cards
 - [x] RSS Feed for main
 - [ ] RSS Feed for tags
 - [x] Captcha for login page

### Home Page

 - [x] List last 5 blog posts on the home page
 - [x] Pagination on the home page
 - [ ] Ads

### Blog Post Page

 - [x] Create blog post page
 - [ ] Ads
 - [ ] Sort out relative images which came from old blog (e.g. `../Content/Images/BlogUploadedPics/tourism-heighlights-2010-edition-pdf-donwload-free.png`)

### Sidebar

 - [x] Replicate the current one
 - [x] Add youtube channel link
 - [ ] Ads

### Other Pages

 - [x] Create about page (static for now)
 - [x] Create speaker page (basic info for now)

### Hosting

 - [ ] Host on Azure App Service
 - [ ] HTTPS by default, so buy a cert

### Migration

 - [ ] Migrate comments to disqus
 - [x] Migrate blog posts to Azure SQL Database based on the SQL Structure

Now at this point, release!

## v2 TODOs

Theme here is to provide the management portal.

### Portal (Authentication, Admin, etc.)

 - [ ] Enable authentication through Twitter
 - [ ] Lock management part with authorization
 - [ ] Provide a page to add new blog post in HTML format
 - [ ] Provide a page to edit the content of a blog post: HTML content, title, abstract and tags
 - [ ] Provide a facility to approve and disaprove a blog post.
 - [ ] Provide a facility to allow and disallow comments on a blog post.
 - [ ] Upload images to Azure Blob Storage

## Further TODOs

 - Would be nice to be to tell how many times a blog post has been read by looking into Google Analytics

## Help Resources

 - [How to add reCAPTCHA to your .NET Core MVC project](https://retifrav.github.io/blog/2017/08/23/dotnet-core-mvc-recaptcha/)
 - [HttpClientFactory in ASP.NET Core 2.1 (Part 1)](https://www.stevejgordon.co.uk/introduction-to-httpclientfactory-aspnetcore)
