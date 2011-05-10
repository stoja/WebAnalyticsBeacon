About

The host based analytics project allows automated collection and analysis of web analytics data for quality assurance purposes. For more informaton on this project, please see: 

http://stdev.wordpress.com/2010/01/15/selenium-automation-webanalytics/

This code is a component of the host based analytics testing project and should be used in conjunction with the analytics viewer which can also be found here.

Prerequisites 

Update Database Connection Details
In order to build and run this project, please update web.config and *.dbml files with the correct connection details. Search for "changeme" for all parameters needing update. 

Enterprise Library Dependencies
Please ensure that you make Microsoft.Practices.EnterpriseLibrary.Common and Microsoft.Practices.EnterpriseLibrary.Logging assemblies available to this project.

Database / Stored Procedures

Please run scripts available in "BeaconDataSetup.sql" to build and prepare the analytics collection database.