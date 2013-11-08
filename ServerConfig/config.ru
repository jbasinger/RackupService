
def log(msg)
	$stdout.puts msg
	$stdout.flush
end

cur_dir = File.expand_path( '..', __FILE__)

apps = []

dir_list = Dir.entries(cur_dir).select do |entry|
	File.directory? File.join(cur_dir,entry) and !(entry =='.' || entry == '..') 
end

dir_list.each do |d|
	
	if File.exists?(File.join(d,'app.rb')) then
		apps << d
	end
	
end

class AllMyAppsAdapter
	def initialize(apps)
		@apps = apps
	end
  def call(env)
		
		content = "
		<html>
			<head>
				<title>My ThinServer Apps</title>
			</head>
			<body>
				<h1>Here are my apps:</h1>
				<ul>"
		
		@apps.each do |app|
			
			content += "<li><a href='/#{app}'>#{app}</a></li>"
			
		end
		
		content += "</ul></body></html>"
		
    body = [content]
    [
      200,
      { 'Content-Type' => 'text/html' },
      body
    ]
  end
end

module AppBuffer
end

pwd = Dir.pwd

bad_apps = []

apps.each do |app|
	begin
		puts "Loading app #{app}"
		Dir.chdir(File.join(cur_dir,app))
		AppBuffer.class_eval(File.open(File.join(cur_dir,app,'app.rb')).read)
	rescue => e
		puts "Error loading app #{app}. Is there a class in there?\n#{e}"
		bad_apps << app
	end
end

bad_apps.each do |app|
	apps.delete(app)
end

#run Rack::URLMap.new("/" => App.new, "/api" => Api.new)
map = {"/apps" => AllMyAppsAdapter.new(apps)}

#Now that we have good apps, lets load them into the global scope
AppBuffer.constants.each do |app|
	
	app_name = app.to_s.downcase
	
	require File.join(cur_dir,app_name,'app.rb')
	
	map["/#{app_name}"] = Object.const_get(app.to_s).new
	
end

run Rack::URLMap.new(map)
