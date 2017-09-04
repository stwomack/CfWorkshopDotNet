# Lab Instructions
## Introduction
This hands-on lab will show you the basics of Pivotal Cloud Foundry and Steeltoe, and how they drastically silify your life as a developer.

_NOTE: These instructions use the CF CLI for all of the steps, since the CF CLI can execute all of the commands for the lab.  Alternatively, you can use the Apps Manager to accomplish some of the steps if you want to.  However, the Apps Manager can't do things like `cf push` or `cf restage`._

## Prerequisites
Before beginning this lab, make sure you have the following set up:

1. The latest version of the CF CLI installed: https://github.com/cloudfoundry/cli/releases
1. The latest git client installed: https://git-scm.com/downloads
1. Clone this repository: `git clone https://github.com/bjimerson-pivotal/CfWorkshopDotNet`
1. Open a Command Prompt window, and change your working directory to the CS project: `cd <git-repo-root>\CfWorkshopDotNet`

## Step 1 - Setup
Target your Pivotal Cloud Foundry instance with the CF CLI, and log in:

```
cf api https://api.<your-cf-url> --skip-ssl-validation
cf login

#Verify that you are logged in properly
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
Click on the Notes the link in your web site.  It will take a while, but it should throw an error eventually.  This is because Notes are Entity Framework entities and we don't have a database configured yet.  Let's create a database for our application to use:

```
cf marketplace

service                       plans                                                           description
p-mysql                       100mb-dev, 2000mb-prod, 100mb                                   MySQL databases on demand
p-rabbitmq                    standard                                                        RabbitMQ is a robust and scalable high-performance multi-protocol messaging broker.
p-redis                       shared-vm, dedicated-vm                                         Redis service to provide pre-provisioned instances configured as a datastore, running on a shared or dedicated VM.
p-service-registry            standard                                                        Service Registry for Spring Cloud Applications
```

Cool, there's a MySQL service setup by our platform team for us to use.  Let's create an instance and bind to it:

```
cf create-service p-mysql 100mb-dev cf-workshop-mysql
cf bind-service <my-app-name> cf-workshop-mysql

cf restage <my-app-name>
```

Now if you navigate to the Notes page in your app you should be able to add a new Note and save it.

###What just happened?
Our platform team has installed a bunch of backing services that we can use for our applications, including databases.  We browsed the PCF marketplace for available services, and created an instance of MySQL to use.  This was all done through something called a Service Broker.  A Service Broker is installed by the platform operators to let us discover, create, and bind to platform services.  Service Brokers can be for any backing services, like Oracle or MSSQL, NoSQL databases, SSO products, and service registries.

When we bound to our newly created MySQL database, the Steeltoe Connector library automatically configured our connection string to use the bound MySQL database.  We didn't need to reconfigure anything in `web.config` or the like; we simply used the Steeltoe Connector library and restaged our application.

## Step 5 - Scale your application out

## Step 6 - Kill your application (really!)

## Other things you can try