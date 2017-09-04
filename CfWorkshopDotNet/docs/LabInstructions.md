# Lab Instructions
## Introduction
This hands-on lab will show you the basics of Pivotal Cloud Foundry and Steeltoe, and how they drastically simplify your life as a developer.  In this lab, you will learn how Pivotal Cloud Foundry and Steeltoe help with the basics of an application's lifecycle, like:
* Deploying and running an application
* Configuring your application in cloud environments
* Discovering, creating, and connecting to backing services such as databases
* Viewing application and system logs
* Scaling your application to meet demand and provide availability
* Monitoring and managing an application's health

_NOTE: These instructions use the CF CLI for all of the steps, since the CF CLI can execute all of the commands for the lab.  Alternatively, you can use the Apps Manager to accomplish some of the steps if you want to.  However, the Apps Manager can't do things like `cf push` or `cf restage`._

## Prerequisites
Before beginning this lab, make sure you have the following set up:

1. The latest version of the CF CLI installed: https://github.com/cloudfoundry/cli/releases
1. The latest git client installed: https://git-scm.com/downloads
1. Clone this repository: `git clone https://github.com/bjimerson-pivotal/CfWorkshopDotNet`
1. Open a Command Prompt window, and change your working directory to the CS project: `cd <git-repo-root>\CfWorkshopDotNet`

## Step 1 - Setup
Target your Pivotal Cloud Foundry (PCF) instance with the CF CLI, and log in:

```
cf api https://api.<your-cf-url> --skip-ssl-validation
cf login
```

And verify that you are logged in properly:

```
cf target
```

## Step 2 - Deploy your app
From the CS project directory root, deploy your app to PCF:

```
cf push
```

Once your app is deployed, you should see a URL for your application in the output.  Go ahead and open that in a browser to make sure everything is OK.

### What just happened?
A whole bunch of stuff happened, but PCF gives it all to you for free:
* Your application was uploaded, and installed along with an embedded IIS server (HWC) and shared libraries
* A route (ala DNS entry) was created for your application
* SSL termination was set up
* A load balancing configuration was created for your application
* Your application was started 
* Logging subsystems were created for your application
* Health monitoring and management was set up for your application

## Step 3 - View environment variables
On the home page (/Environment), you will see a number of environment properties displayed (the italicized properties are all environment variables).  These properties, and many more, are provided by PCF.  The Steeltoe Configuration library is used to retrieve these properties via .NET Configuration.  You can also set your own environment variables with the CF CLI or deployment manifest:

```
cf set-env <app-name> <variable-name> <variable-value>
```

### Why this matters
In 12-factor applications, environment variables should be used for configuration instead of code-based things like the `web.config` file.  PCF exposes all of the platform configuration to your application through environment variables, including bound services, which we will see later.  Instead of using files like `web.config`, make sure your application uses Steeltoe Configuration to set and retrieve configuration through environment variables.

## Step 4 - Create a database and bind to it
Click on the Notes link in your web site.  It will take a while, but it should throw an error eventually.  This is because Notes are Entity Framework entities and we don't have a database configured yet.  Let's create a database for our application to use:

```
cf marketplace

service                       plans                                                           description
p-mysql                       100mb-dev, 2000mb-prod, 100mb                                   MySQL databases on demand
p-rabbitmq                    standard                                                        RabbitMQ is a robust and scalable high-performance multi-protocol messaging broker.
p-redis                       shared-vm, dedicated-vm                                         Redis service to provide pre-provisioned instances configured as a datastore, running on a shared or dedicated VM.
p-service-registry            standard                                                        Service Registry for Spring Cloud Applications
```

Cool, there's a MySQL service set up by our platform team for us to use.  Let's create an instance and bind to it:

```
cf create-service p-mysql 100mb-dev cf-workshop-mysql
cf bind-service <my-app-name> cf-workshop-mysql

cf restage <my-app-name>
```

Now if you navigate to the Notes page in your app you should be able to add a new Note and save it.

### What just happened?
Our platform team has installed a bunch of backing services that we can use for our applications, including databases.  We browsed the PCF marketplace for available services, and created an instance of MySQL to use.  This was all done through something called a Service Broker.  A Service Broker is installed by the platform operators to let us discover, create, and bind to platform services.  Service Brokers can be for any backing services, like Oracle or MSSQL, NoSQL databases, SSO products, and service registries.

When we bound to our newly created MySQL database, the Steeltoe Connector library automatically configured our connection string to use the bound MySQL database.  We didn't need to reconfigure anything in `web.config` or the like; we simply used the Steeltoe Connector library and restaged our application.

## Step 5 - View your application logs
Now that you have a database to connect to and everything is working properly, let's view the application and system logs:

```
cf logs <my-app-name>
```

Refresh your app in the browser a few times and observe the log output.  You can hit CTRL+C to stop viewing the logs.

### Why is this important?
A la 12-factor apps, application logs should be treated as streams, and not written to files or databases.  PCF automatically aggregates application logs with system component logs, like the Router and the Cloud Controller, and exposes them as a stream that can be sent to external outputs.  All you have to do is write your logs to standard out and standard error, and PCF automatically intercepts and streams them.  Most customers use an external syslog server to store and analyze application logs in the event that something happens.

## Step 6 - Scale your application
Our application is going to need a number of instances to handle load and provide resiliency.  Right now we only have 1 instance running, so let's scale it to 3 instances:

```
cf scale <my-app-name> -i 3

cf app <my-app-name>

     state     since                  cpu    memory         disk          details
#0   running   2017-09-03T23:31:42Z   0.0%   292.8M of 1G   77.6M of 1G
#1   running   2017-09-04T16:04:38Z   0.0%   22.4M of 1G    61.9M of 1G
#2   running   2017-09-04T16:04:38Z   0.0%   0 of 1G        0 of 1G
```
That was easy!  We now have 3 instances of our application running.  Go to the Environment page of your app, and click the Refresh button a few times; you should see the Instance Index property change between 0, 1, and 2.  This is because when we scaled our application, the load balancer was automatically updated to include the new instances.

### Why is this important?
Scaling to meet demand and availability is an important aspect of applications.  PCF makes scaling very easy; by using the `cf scale` command, application instances are automatically created and added to the load balancer.  PCF also has an Autoscaler service to scale your applications based on CPU, throughput, latency, or on scheduled times.  Just bind an Autoscaler service instance to your application, configure the parameters, and you're all set!

## Step 7 - Kill your application (really!)
So what happens when your application doesn't behave the way it's supposed to?  We get it, it happens.  PCF will automatically recreate app instances if they crash for some reason.  PCF always reconciles desired app state with actual app state.

Go ahead and click the Kill button on the Environment page.  This will kill the running process of that app instance.  If you go back to the home page (change the URL in the address bar), you will see that your application is still available.  You can also view the state of your applications through the CF CLI:

```
cf app <my-app-name>

     state     since                  cpu    memory         disk          details
#0   running   2017-09-03T23:31:42Z   0.0%   292.8M of 1G   77.6M of 1G
#1   running   2017-09-04T16:04:38Z   0.0%   22.4M of 1G    61.9M of 1G
#2   starting  2017-09-04T16:04:38Z   0.0%   0 of 1G        0 of 1G
```

### What just happened?
PCF automatically adds health monitoring and management to your application.  When you killed the application instance, PCF did a number of things:
* The instance that died was removed from the load balancer, so that traffic wasn't sent to the dead instance
* The dead instance and associated container was destroyed
* A new container and app instance was created
* The new app instance was started
* The new app instance was added to the load balancer

PCF will always try to make sure that your application is healthy and in the desired state for you.

## Other things you can try (extra credit)
* Bind an Autoscaler service instance to your application and use a load testing tool to view your application automatically scale out and in.
* Use Steeltoe Security libraries and the PCF SSO service to add authentication to your application.
* Set space-scoped environment variables for your application, to provide things like feature flags.
* Add a Service Registry instance, and use the Steeltoe Discovery Client to register your application.