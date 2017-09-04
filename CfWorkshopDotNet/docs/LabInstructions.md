# Lab Instructions
## Introduction
This hands-on lab will show you the basics of Pivotal Cloud Foundry and Steeltoe, and how they drastically silify your life as a developer.

* NOTE: These instructions use the CF CLI for all of the steps, since the CF CLI can execute all of the commands for the lab.  Alternatively, you can use the Apps Manager to accomplish some of the steps if you want to.  However, the Apps Manager can't do things like `cf push` or `cf restage`.*

## Prerequisites
Before beginning this lab, make sure you have the following set up:

1. The latest version of the CF CLI installed: [https://github.com/cloudfoundry/cli/releases]
1. Clone this repository: `git clone https://github.com/bjimerson-pivotal/CfWorkshopDotNet`

## Step 1 - Setup
Target your Pivotal Cloud Foundry instance with the CF CLI, and log in:

```
cf api https://api.<your-cf-url> --skip-ssl-validation
cf login

#Verify that you are logged in properly
cf target

```