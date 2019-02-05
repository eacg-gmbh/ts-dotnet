# TrustSource nuget integration

This repo contains several plugins. Two for Visual Studio, one for Teamfoundation Server/Services. Two have been designed to support .NET-framework, one has been designed to support .Net-Core.

## .Net-Core & Visual Studio Integration
Go to [/TrustSource](https://github.com/eacg-gmbh/ecs-nuget/tree/master/TrustSource) folder and see the ReadMe there.

## .Net-Framework & TFS Integration

To make use of the TFS plugin, please follow these steps:

1.	Install Node.js
If not yet available on the server, download nodeJS at https://nodejs.org/en/download/ and perform installation. The installation may differ depending on your OS. Then make sure your environment has the correct path settings (to make npm command available)

2.	Install tfs-cli
Go to https://github.com/Microsoft/tfs-cli to install the TFS command line interface.

3.	Configure basic authorization
See https://github.com/Microsoft/tfs-cli/blob/master/docs/configureBasicAuth.md on how to configure your authentication.

4.	Create temporary files folder 
Provide a folder to secure temporary files, e.g. c:\temp\buildtasks

5.	Execute at command prompt with administrator rights

  `tfx login --auth-type basic`
  for a Service URL use a string such as 'http://studio:8080/tfs/DefaultCollection'  
  All the other parameters are optional

  `tfx build tasks create `
  Use “CheckLicenses” as a first parameter name

6.	Open file CheckLicenses/task.json and copy the value of the “id” field

7.	Open task.json file and paste the copied “id” value there.

8.	Execute at command prompt with administrator rights:

 ` tfx build tasks upload --task-path CheckLicenses `
 
 or if the task already on the TFS and you want update it:
 
 `tfx build tasks upload --task-path CheckLicenses --overwrite`
    
9.	Change to TFS and configure your build using the new custom task (should be available)

## .Net-framework & Visual Studio Integration

tbc.

## Contact & Questions

Feel free contacting us for more details and questions. We are eager to learn more about your demand. Send an email to support (at) trustsource.io. 

