# monit-windows

## A minimal monit-compatible implementation to report basic system info to m/monit

The installer will install two binaries:

* `monit-configure` (or a shortcut named `Configure monit` in your start menu) which allows you to configure the M/Monit server to which it should report along with the user and password to authenticate.  The settings are stored in the registry under `HKLM\SOFTWARE\monit\`.  The `instanceId` is required to be unique and is generated automatically by `monit-configure`, you should leave it alone.
* a `monit` service which runs and reports basic system statistics (version, memory, CPU information) to the configured M/Monit server every 60 seconds.

It doesn't do any fancy service monitoring, just the basic system statistics.  I only wanted basic uptime monitoring of my Windows systems in M/Monit and this accomplishes that.

The Visual Studio solution was build with VS 2019 and requires [Visual Studio Installer Projects](https://marketplace.visualstudio.com/items?itemName=visualstudioclient.MicrosoftVisualStudio2017InstallerProjects) to build the installer.