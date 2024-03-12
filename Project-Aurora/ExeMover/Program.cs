using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Management.Automation;

const int csidlCommonStartmenu = 0x16; // All Users\Start Menu
const int csidlStartmenu = 0xb;

var globalStartup = new StringBuilder(260);
SHGetSpecialFolderPath(IntPtr.Zero, globalStartup, csidlCommonStartmenu, false);

var userStartup = new StringBuilder(260);
SHGetSpecialFolderPath(IntPtr.Zero, userStartup, csidlStartmenu, false);

var globalStartPath = Path.Combine(globalStartup.ToString(), "Programs");
var userStartPath = Path.Combine(userStartup.ToString(), "Programs");

var currentPath = Path.GetDirectoryName(Environment.ProcessPath);
if (currentPath == null)
{
    Process.Start("AuroraRgb.exe");
    return;
}

var script = """
             param (
                 [Parameter(Mandatory = $true)]
                 [string]$TargetPath,
             
                 [Parameter(Mandatory = $true)]
                 [string[]]$LnkFiles
             )

             foreach ($lnkFile in $LnkFiles) {
                 # Create a Shell object
                 $shell = New-Object -ComObject WScript.Shell
             
                 # Check if the file exists
                 if (Test-Path $lnkFile) {
                     # Get the shortcut object
                     $shortcut = $shell.CreateShortcut($lnkFile)
             
                     # Change the target path
                     $shortcut.TargetPath = $TargetPath
             
                     # Save the changes
                     $shortcut.Save()
             
                     Write-Output "Changed target for $($lnkFile) to $TargetPath"
                 } else {
                     Write-Output "File $($lnkFile) does not exist."
                 }
             }

             Write-Output "All .lnk files have been updated to target $TargetPath"
             """;

var newPath = Path.Combine(currentPath, "AuroraRgb.exe");
string[] auroraShortcutPaths =
[
    Path.Combine(globalStartPath, "Aurora.lnk"),
    Path.Combine(userStartPath, "Aurora.lnk"),
    Path.Combine(userStartPath, "Aurora - Shortcut.lnk"),
    Path.Combine(userStartPath, "Aurora.exe - Shortcut.lnk"),
];
var lnkFiles = string.Join(", ", auroraShortcutPaths.Select(s => '"' + s + '"'));

var powershell = PowerShell.Create().AddScript(script);
powershell.AddParameter("TargetPath", newPath);
powershell.AddParameter("LnkFiles", lnkFiles);
powershell.Invoke();


Process.Start("AuroraRgb.exe");
return;

[DllImport("shell32.dll")]
static extern bool SHGetSpecialFolderPath(IntPtr hndOwner,
    [Out] StringBuilder lpszPath, int nFolder, bool fCreate);
