# Powershell-WebAccess

![](https://i.ibb.co/0cfnMzG/Untitled-4.png)

Powershell Core remote control in web browser for Windows.

# Prerequisites
1. .NET Core 3.1
2. pwsh.exe installed (preview 7.0)


# Run (manual)
1. Download [the latest release](https://github.com/romanov/Powershell-WebAccess/releases) and unpack
2. Run it `dotnet PowershellWeb.dll --key your-secret-password` or `PowershellWeb.exe --key your-secret-password`
3. Copy secret key and paste to the browser (click on a shield) and paste your secret key

# Install (automatic)
```
$directoryPath = (Get-Item -Path ".\").FullName
$url = "https://github.com/romanov/Powershell-WebAccess/releases/download/v2/version2.zip"
$installPath = "$directoryPath\powershellweb.zip"
Invoke-WebRequest -Uri $url -OutFile $installPath
Expand-Archive $installPath -DestinationPath "$directoryPath\powershell-web" -Force
Set-Location -Path "$directoryPath\powershell-web"
Start-Process dotnet -NoNewWindow -ArgumentList ".\PowershellWeb.dll --key your-secret-password"
```

# Port
The default port for now is `5000`.
To open port on window type the following command:
`netsh advfirewall firewall add rule name="Powershell WebAccess" dir=in action=allow protocol=TCP localport=5000`

# Shutdown
Command `Stop-WebAccess` will shutdown the app.

# Future
- multiline commands
- long-polling => signalr
- ssl
- terminal history
