Requisites to Run application:

Microsoft Windows 10
Microsoft SQL Server 2019
Visual Studio 2019

Configuration steps:

Running from visual Studio

-Open solution in visual Studio
-Locate file BlogEngineConfig.json and edit the Connection String to set proper values if needed.
-Ensure your current user has privileges to create and drop database configured in BlogEngineConfig.json, but don´t create database yourself, application will do it for you.
-Build Solution
-Run BlogEngine.GUI (when you run application application creates database and all structure and sample data).
-Done !!

Running from IIS

-Install IIS Features
-Install Microsoft .NET Core Bundle 5 (Download from https://dotnet.microsoft.com/download/dotnet/thank-you/runtime-aspnetcore-5.0.10-windows-x64-installer)
-Restart PC after installing

Go to IIS and follow this steps:
-Create a Pool called BlogEngine (Pipeline Integrated and Managed Code: No managed)
-Create an Application in IIS called BlogEngine.
-Assign Pool BlogEngine to Application BlogEngine.
-In Autentication settings from your Website Configuration (at BlogEngine application level) only enable windows autentication.
-Copy contents of directory BlogEngine\BlogEngine.GUI\bin\Release\net5.0\publish to c:\inetpub\wwwroot\BlogEngine
-Locate file BlogEngineConfig.json and edit the Connection String to set proper values if needed.
-Done, you can go to your application by clicking the "Browse" button at your BlogEngine application level.


Time to build project: 18 hours