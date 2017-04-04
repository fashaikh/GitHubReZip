# GitHubReZipper

This proxy allows you to zip deploy your functions/function app/appservice without tying it to the github repo. 
This allows the deployer to make changes on the function offline, and use the rich capabilites of the functions portal  
to tweak it. 

Plus you can mix and match functions from different repositories into a single function app,a s logn as the names are different.


# How does this work:

You use an ARM template to do webdeploy to a new or existing Function App
For the package URL, use the API below, which creates a zip file with the right folder structure

Basically to deploy from `master` branch of  https://github.com/Azure-Samples/functions-dotnet-sas-token/, 
you point msdeploy in your ARM template to
https://http://maws-zip.trafficmanager.net/Azure-Samples/functions-dotnet-sas-token/master

This uses msdeploy to deploy the .zip file that GitHub /zip api spits out. 
However the folder structure in the /zip api is one level too deep for function apps. 
The ReZip proxy does an in-memory re-zip  and outputs a zip stream, with a functions friendly folder structure. 


# Example
Sample function app github repo:
https://github.com/Azure-Samples/functions-dotnet-sas-token

Sample ARM deploy template : 
https://github.com/fashaikh/azure-quickstart-templates/blob/master/101-function-app-create-dynamic/azuredeploy.json


The directory structure for the functionsapp github repo should be the same as under the wwwroot folder 
For more details look here: https://github.com/Microsoft/azure-docs/blob/master/includes/functions-folder-structure.md


```
wwwroot
 | - host.json
 | - mynodefunction
 | | - function.json
 | | - index.js
 | | - node_modules
 | | | - ... packages ...
 | | - package.json
 | - mycsharpfunction
 | | - function.json
 | | - run.csx
```
