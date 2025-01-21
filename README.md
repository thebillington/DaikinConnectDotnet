# Daikin Connect

This is a simple .NET MVC project which allows you to connect to and fetch data from the Daikin API.

If you're new to dotnet you can use [this tutorial](https://learn.microsoft.com/en-us/aspnet/core/tutorials/first-mvc-app/start-mvc?view=aspnetcore-9.0&tabs=visual-studio) to learn the basics.

## Setting Up

### Pre-requisites

1. Git installed on your machine (or knowledge of how to download files from git)
2. dotnet sdk - `built on 9.0.102`
3. Visual Studio Code (or an alternative IDE)
4. Ability to do basic Terminal/Command Line navigation

### Project setup

1. Pull this repository (`git clone https://github.com/DaikinConnectDotnet/`)
2. Open the repository in Visual Studio Code and run it

### Setting up ngrok

1. Head to the [ngrok](https://ngrok.com/) website and register for a free account
2. Follow the instructions and install `ngrok` for your system
3. Register for a static domain
4. Take the domain name (e.g. `super-grit-lightning.ngrok-free.app`) and set it as the `RedirectUri` in `appsettings.json`

### Creating a Daikin API App

1. Head to the [Daikin API](https://developer.cloud.daikineurope.com/) and register for an account
2. Once registered, click your email in the top right and click `My Apps`
3. Click `New App`
4. Choose a good app name (I named mine `daikin-connect`) and set the redirect URI to the one you got from `ngrok` (e.g. `super-grit-lightning.ngrok-free.app`)
5. Once created, grab your `client_id` and add it as `DaikinClientID` in `appsettings.json`
6. Grab your `client_secret` and add it as an environment variable (e.g. in `~/.zshenv` or `~/.bash_profile` on your local machine, or in the `Properties/launchSettings.json` file (just don't commit it!)) (note; if you don't get your secret before closing the window, you may need to delete and recreate your app)

## Running the project

Once you have setup `ngrok` on your machine and setup the client ID, secret and redirect URI you can run the app.

To run the app successfully, you need to do 2 things:

1. Run the app locally using via your IDE of choice (or using command line `dotnet run`)
2. Create a tunnel via `ngrok` from your local machine to the `RedirectURI` you setup

This is because the Daikin API will not allow you to set `localhost` as the redirect URI after successfully authenticating a user via SSO.

### Running the app

1. Open a new Terminal and navigate to the directory where your code is (e.g. `cd ~/Downloads/DaikinConnectDotnet`)
2. Run the code via your IDE or using `dotnet run`
3. Go to your browser and navigate to `localhost:3000`

If you can see the running app, you should be good to go!

### Creating the `ngrok` tunnel

Now that the app is running, the last step is to create a tunnel from `ngrok` (publicly accessible) to your local instance (port `3000`).

`ngrok http --url=super-grit-lightning.ngrok-free.app 3000`