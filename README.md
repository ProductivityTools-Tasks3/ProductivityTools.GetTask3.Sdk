<!--Category:PowerShell--> 
 <p align="right">
    <a href="https://www.nuget.org/packages/ProductivityTools.ConnectionStringLight/"><img   src="Images/Header/Nuget_border_40px.png" /></a>
    <a href="http://productivitytools.tech/get-servicedescription/"><img src="Images/Header/ProductivityTools_green_40px_2.png" /><a> 
    <a href="https://github.com/ProductivityTools-Tasks3/ProductivityTools.GetTask3.Sdk"><img src="Images/Header/Github_border_40px.png" /></a>
</p>
<p align="center">
    <a href="http://http://productivitytools.tech/">
        <img src="Images/Header/LogoTitle_green_500px.png" />
    </a>
</p>

# GetTask3.Sdk

Nuget allows to access to the GetTask3 API.

To initialize we need to provide: 
- API address
- Firebase application key 

![](Images/2022-06-20-08-01-20.png)

```c#
var taskClient = new TaskClient("http://localhost:5513/api/",webapikey , (x) => { System.Console.WriteLine(x); });
```

Currently SDK exposes methods:
- GetStructure
- Start


 ## Authorization Code Grant