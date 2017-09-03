## CF Workshop Demo

### Introduction

This is a demo app for CF workshops, written in ASP.NET Core.
It is intended to demonstrate some of the day-in-the-life features for developers in CF

 * Push / bind / scale
 * Accessing CF environment variables
 * Accessing CF service variables
 * Scaling, dynamic routing and load balancing
 * Health management and application restart
 * Binding services

### Building, Packaging, and Deploying

#### To get the source code and build the project


    git clone https://github.com/bjimerson-pivotal/CfWorkshopDotNetCore

    dotnet restore

    dotnet build


#### To run the application

By default the application uses an embedded SLQlite database.

Run the application locally as an ASP.NET Core app for testing:

    cd CfWorkshopDotNetCore

    dotnet run

Running in CF is as usual too:

    cf push cf-workshop-dotnet-core
