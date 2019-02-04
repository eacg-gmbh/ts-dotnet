<h1>TrustSource .NET Core console app and plugin for VS Windows</h1>
This repository contains TrustSource .NET Core plugin project for Visual Studio for Windows, and .NET Core console application for Cross Platform operations. 

<h2>TS-NetCore-Scanner Console application</h2>
To make use of the Console application, please follow these steps:

Install .NET Core in desired operation system. Then locate working directory and run help command. This will browse list of options you could supply to this console application.
<br /><br /><b>Note: You can use current directory for scanning path, or provide path explicitely using [ -p | --path option is optional ]</b>

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
Scan .NET Core project dependency graph and send to TrustSource
Depending on your OS, you may need to execute the application as TS-NetCore-Scanner.exe or 'dotnet TS-NetCore-Scanne.dll'
</pre>
<h3>Example</h3>
<pre>$ dotnet TS-NetCore-Scanner.dll -user "username@domain.com" -key "TrustSource key" -p "C:\Users\user\source\repos\solution"</pre>

In case of current working directory you can ignore path:
<pre>$ dotnet TS-NetCore-Scanner.dll -user "user@domain.com" -key "TrustSource Key"</pre>

<h1>Contact & Questions</h1>
Feel free contacting us for more details and questions. We are eager to learn more about your demand. Send an email to support (at) trustsource.io.


<h1>Developed by</h1>
<a href="mailto:nabeel@relliks.com">Muhammad Nabeel</a> [ inabeel@live.com ]
<br />Technical Lead - <a href="https://relliks.com">Relliks Systems</a>
