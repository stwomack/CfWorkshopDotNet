# Lab Instructions
## Introduction
This hands-on lab will show you the basics of Pivotal Cloud Foundry and Steeltoe, and how they drastically silify your life as a developer.

<<<<<<< HEAD
_NOTE: These instructions use the CF CLI for all of the steps, since the CF CLI can execute all of the commands for the lab.  Alternatively, you can use the Apps Manager to accomplish some of the steps if you want to.  However, the Apps Manager can't do things like `cf push` or `cf restage`._
=======
__NOTE: These instructions use the CF CLI for all of the steps, since the CF CLI can execute all of the commands for the lab.  Alternatively, you can use the Apps Manager to accomplish some of the steps if you want to.  However, the Apps Manager can't do things like `cf push` or `cf restage`.__
>>>>>>> 5a838824c04f89b22339e72a4e9d108b8fb4367c

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