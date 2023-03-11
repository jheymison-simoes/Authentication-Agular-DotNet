# Authentication Project with Email, Password, and Google using .NET and Angular

This project was developed to demonstrate how to create a simple authentication using email and password, and allowing the user to log in using their Google account. The application is divided into two parts: a RESTful API with .NET and a front-end with Angular.

## Prerequisites

Before you start, you'll need the following tools installed on your machine:

- Visual Studio or Visual Studio Code (for the .NET project)
- .NET Core 3.1 or higher
- Node.js
- Angular CLI

## How to run the project

To run the project, follow the steps below:

1. Clone the repository to your machine:
```bash 
git clone https://github.com/your-username/your-project.git
```
2. Open the project in Visual Studio or Visual Studio Code.
3. Open a terminal in the `Auth.API` folder and run the following commands:
```bash
dotnet restore
dotnet run
```
4. Open another terminal in the `Auth-App` folder and run the following commands:
```bash
npm install
ng serve
```
5. Access the application in your browser at `http://localhost:4200`.

## Project Configuration

Before running the project, you need to configure a few things. Below are some of the configurations you need to make:

### Google Configuration

To allow users to log in with their Google accounts, you need to configure a project in the [Google Developers Console](https://console.developers.google.com/). After creating the project, you need to create OAuth credentials for the Google API. Copy the Google API key to the `appsettings.json` file in the .NET project and to the `environment.ts` file in the Angular project.

### Database Configuration

The project uses Entity Framework Core to access the database. You need to configure the connection to the database in `appsettings.json` in the .NET project. Make sure your database is already created.

## Final Considerations

This project is just a simple example of how to create authentication using email and password, and allowing login using Google. You can modify and adapt it according to your needs. Feel free to contribute to this project or create a fork for your own implementation. If you have any questions or issues, feel free to create an issue in this repository.
