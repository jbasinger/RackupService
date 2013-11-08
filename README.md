RackupService
=============

A windows service that helps you easily deploy your rack applications locally.

Initially, I tried making it so I could just dump Sinatra sites into IIS and run them.
It was a terrible experience and I couldn't get it working without jumping through hoops.

So, I wrote a service that just calls rackup in a specified directory and loads all the sub directories
as applications. You just put in a folder and in that folder put an app.rb file that has a rackup-able
application and it'll attempt to load it up.

Setup
=====

1. Set your base directory where you want your apps to live in the app.config file for RackupService.
2. Copy config.ru from ServerConfig to your base directory
3. Move your apps into the base directory
3. Build rackup service.
4. Go to the directory with the binary in it with a command line prompt as an admin and type "installutil rackupservice.exe"

Notes
=====

When you add a new application, the service needs to be restarted.

Default port is 8080

Go to /apps to see the list of apps that were successfully installed.

An example directory structure would look like this

/sites
|
+ - config.ru
|
+ - /app1
| |
| + - app.rb
|
+ - /app2
  |
  + - app.rb
  
If you're having issues, let me know. I'll see if I can help.
