# C# Import Delimited Files

### Disclaimer
I am no professional C# developer. I can write C# and definitely understand the concepts and programming in general however C# is not my first language it's more like my 18th.
I have thrown this app together more as a proof of concept with some error checking and some validation. However it's NOT BULLET PROOF and code is a little sloppy and could use 
quite a bit of refactoring. 

### Future life of app / Goal
I hope this app encourages people to write their own Apps in C# and or any other language. I hope that people continue to improve their skill sets. I encourage people to submit
any bugs they find / feature requests they may want to see in the app and I will try to fix or implement them as time permits. I could see this app as like a little swiss army 
knife for any SQL dev that works with ETL or other data activities. Keep building more and more add-ons to make it a robust tool

# About the APP

### Batch Import
This repo contains a Windows Form application wrote in C# (.NET 5.0) with 4 NuGet Packages to allow users to read flat files (delimited) and write in bulk multiple files. 
It also allows users to change the datatypes upon import for each individual file (like SQL Import Util). However, unlike SQL Server import util, this utility also allows for 
multiple files as well as using VARCHAR(MAX) as SQL import util only allows varchar(8000) and TEXT. 

### Bulk Convert
This application also has a tool in it to allow users to bulk convet out 
excel files. It will go through a folders top higherarchy and grab any excel file (.xls or .xlsx) and convert them to a tab delimited file in a destination of their choosing. 
Once done they can use that foler to import.

### Constant import
The utility allows you to do increemental table imports up to (999) tables (can be increased with simple modification) along with a completely unioned table per load if the schemas 
are similar enough this is super helpful for files that are close to the same every time you get them. (Maybe different populations of some category of data).

### New Branch for Table Names = File Names