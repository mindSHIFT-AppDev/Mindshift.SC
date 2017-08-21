# Mindshift.SC
A solution for creating Sitecore modules. 

At this point, most of the modules are incomplete or even just ideas. I will lay that out below.

Now that I've moved the code into Github, I will most likely find a way to separate the projects out here while keeping the dependency in the solution itself.

## Projects
Most modules contain the following project types:
1. TDS Projects - these contain the Sitecore items needed for the module. They are separated by the Sitecore database.
2. Web Projects - This contains all the files that are needed for the module. They are Web Site projects, not Web Applications.
3. The Libraries - These have no suffix in their name like the TDS and Web Projects. They contain all the classes needed as well as any Web API Controllers.

While the repository shows all projects on the same level the Solution is separated into different Solution Folders based on the specific Module.

## Modules
This repository contains a number of different modules. Here is a short description of each of them. Please see the README.md under each project for more information.

### Common
These projects contain items, classes and files that are shared between all modules. Including the following:
1. An Angular 1 framework for creating dialogs.
2. A Generic Web API route for all modules.
3. Base classes for Web API Controllers, Mappings, Pipelines.
4. Custom Field Types, Enumerations, Dialogs.

### AutoPublish
Status: Complete and working.

Description: This module gives you the ability to schedule publish times via configuration items under System/Modules.

See version 1.0 in the [Releases](/Releases) folder.

### AdoLogging
Status: Complete but haven't tested it in a while.

Description: Logs log4net data into a custom database. Uses the ASR to report on that data.

### Dynamic Placeholders
Status: Needs to be verified

Description: Most of the the dynamic placeholder code itself came from somewhere else. This project is actually concerned with making a custom dialog that can be used as a replacement for the Presentation Details dialog in Sitecore, but supports the Dynamic Placeholders. It shows the placeholders and renderings in a tree format! The original code is repeated here as a dependency for testing.

### TfsPackageDesigner
Status: Idea only

Description: The idea is to create a Sitecore package based on a TFS checkin.

### ZipLogging
Status: Idea only

Description: The idea is to allow log4net to log directly into a zip file. This will increase space as well as increase write speed.



## Building

Building the .TDS.Master project will create the update package for installing a module. This is only setup for the working modules above so far. When building during development you can also use the TDS Sync to Sitecore.





