# Powershell-WebAccess

![](https://i.ibb.co/0cfnMzG/Untitled-4.png)

Powershell Core remote control in web browser for Windows.

# Prerequisites
1. .NET Core 3.1
2. pwsh.exe installed (preview 7.0)


# Run (manual)
1. Download [the latest release](https://github.com/romanov/Powershell-WebAccess/releases) and unpack
2. Run it `dotnet PowershellWeb.dll` or `PowershellWeb.exe`
3. Copy secret key and paste to the browser (click on a shield) and paste the first line of console output

# Install (automatic)
```
$directoryPath = (Get-Item -Path ".\").FullName
$url = "https://github.com/romanov/Powershell-WebAccess/releases/download/v1/PowershellWeb1.zip"
$installPath = "$directoryPath\powershellweb.zip"
Invoke-WebRequest -Uri $url -OutFile $installPath
Expand-Archive $installPath -DestinationPath "$directoryPath\powershell-web"
#Start-Job -Name "powershell-web" -ScriptBlock { dotnet run "$directoryPath/powershell-web" }
Set-Location -Path "$directoryPath\powershell-web"
dotnet .\PowershellWeb.dll
```

# Future
- multiline commands
- long-polling => signalr
