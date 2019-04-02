<h1>TrustSource plugin for Visual Studio and .NET framework and Core</h1>
This repository contains TrustSource solution for .NET Framework, .NET Core console projects and Visual Studio plugin for Windows. <br /><br />Plugin is capable of running both .NET Core and .NET Framework scans from directly inside Visual studio. Solution contains separate console projects for .NET Core console to execute .NET Core based cross platform applications. And .NET Framework project which can scan both .NET core and .NET based applications on Windows framework. Usage of these projects are as follows.

<h2>Visual Studio Integration</h2>
The Visual Studio Plugin can be found in the <a href="https://marketplace.visualstudio.com/items?itemName=TrustSource.vsp4dotnetcore">Visual Studio Marketplace</a>.<br />
Just install plugin and go to Tools -> TrustSource and fill Credentials. Then open solution and select TrustSource -> Execute Scan from Top Menu.

<h2>TrustSource Console applications</h2>
To make use of both Console applications, please follow these steps:

Install .NET Framework and .NET Core in desired operating system. Then locate working directory and run help command. This will browse list of options you could supply to this console application.
<br /><br /><b>Note: You can use current directory for scanning path, or provide path explicitly using [ -p | --path option is optional ]</b>
<br />

<br />Following is the command which will list all options which could be supplied to scanner:
<pre>
$ dotnet TS-NetCore-Scanner.dll <span class="hljs-operator">--help</span>
TS-NetCore-Scanner is in progress...
Version 1.0.0
Usage: TS-NetCore-Scanner [options]

Options:
  -?|-h|--help                    Show help information
  -v|--version                    Show version information
  -p|--path <optionvalue>         .Net Core Project Path
  -user|--username <optionvalue>  TrustSource Username
  -key|--ApiKey <optionvalue>     TrustSource Api Key
Scan .NET Core project dependency graph and send to TrustSource<br/>
Depending on your OS, you may need to execute the application as TS-NetCore-Scanner.exe or 'dotnet TS-NetCore-Scanne.dll'.
</pre>
<h3>Example</h3>
<pre>$ dotnet TS-NetCore-Scanner.dll -user "username@domain.com" -key "TrustSource key" -p "C:\Users\user\source\repos\solution"</pre>

In case of current working directory you can ignore path:
<pre>$ dotnet TS-NetCore-Scanner.dll -user "user@domain.com" -key "TrustSource Key"</pre>

<h2>Configuration files</h2>
Parameters could also be supplied into configuration files in their specific projects. In .Net Framework configurations are stored in App.config like:
<br />
<pre>
  &lt;appSettings>
    &lt;add key="TS-Username" value="API User Name" />
    &lt;add key="TS-ApiKey" value="API User Key" />
    &lt;add key="TS-ApiUrl" value="" /> <!--// Provide url for testing destinaton or leave empty-->
    &lt;add key="ProjectPath" value="Project Path to Scan" />
  &lt;/appSettings>
</pre>
<br />
And in .NET Core These settings are stored in AppSettings.json like:
<pre>
{
  "TrustSourceAPI": { // TrustSource Credentials
    "Username": "user",
    "ApiKey": "api",
    "ApiUrl": "" // Provide test API url or leave empty for live scan
  },
  "ProjectPath": "project"
}
</pre>


<h1>Contact & Questions</h1>
Feel free contacting us for more details and questions. We are eager to learn more about your demand. Send an email to support (at) trustsource.io.


<h1>Developed by</h1>
<a href="mailto:nabeel@relliks.com">Muhammad Nabeel</a> [ inabeel@live.com ]
<br />Technical Lead - <a href="https://relliks.com">Relliks Systems</a>
