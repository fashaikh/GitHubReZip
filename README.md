# GitHubReZipper
This proxy  allows you to zip deploy your functions/function app/appservice without tying it to the github repo. 
This allows the other user to make changes on the function offline, and use the rich capabilites of the functions portal  
to tweak it.

This uses msdeploy to deploy the .zip file that GitHub /zip api. However the folder structure in the /zip api is one level too deep for function apps. This ReZip proxy does an in-memory re-zip  and outputs a zip stream, with a functions friendly folder structure. 


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
