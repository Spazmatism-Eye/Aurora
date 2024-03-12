using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

// It is unfortunate but we have to set it to Unknown first.
Thread.CurrentThread.SetApartmentState(ApartmentState.Unknown);
Thread.CurrentThread.SetApartmentState(ApartmentState.STA);

const int csidlCommonStartmenu = 0x16;  // All Users\Start Menu
const int csidlStartmenu = 0xb;
var shell = new Shell32.Shell();

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

string[] auroraShortcutPaths = [
    Path.Combine(globalStartPath, "Aurora.lnk"),
    Path.Combine(userStartPath, "Aurora.lnk"),
    Path.Combine(userStartPath, "Aurora - Shortcut.lnk"),
    Path.Combine(userStartPath, "Aurora.exe - Shortcut.lnk"),
];

foreach (var auroraShortcutPath in auroraShortcutPaths)
{
    ChangeToNewPath(auroraShortcutPath);
}

Process.Start("AuroraRgb.exe");
return;

[DllImport("shell32.dll")]
static extern bool SHGetSpecialFolderPath(IntPtr hndOwner,
    [Out] StringBuilder lpszPath, int nFolder, bool fCreate);

void ChangeToNewPath(string shortcutPath)
{
    var newPath = Path.Combine(currentPath, "AuroraRgb.exe");
    if (!File.Exists(shortcutPath))
    {
        return;
    }
    ChangeLinkTarget(shortcutPath, newPath);
}
    
void ChangeLinkTarget(string shortcutFullPath, string newTarget)
{
    // Load the shortcut.
    var folder = shell.NameSpace(Path.GetDirectoryName(shortcutFullPath));
    var folderItem = folder.Items().Item(Path.GetFileName(shortcutFullPath));
    var currentLink = (Shell32.ShellLinkObject)folderItem.GetLink;

    // Assign the new path here. This value is not read-only.
    currentLink.Path = newTarget;

    // Save the link to commit the changes.
    currentLink.Save();
}