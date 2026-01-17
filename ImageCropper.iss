#define MyAppLong "ImageCropper"
#define MyAppName "ImageCropper"
#define MyAppVersion "1.0.1"

[Setup]
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppVerName={#MyAppLong} {#MyAppVersion}
VersionInfoVersion={#MyAppVersion}
ArchitecturesAllowed=x64os
ArchitecturesInstallIn64BitMode=x64os
PrivilegesRequired=none
PrivilegesRequiredOverridesAllowed=dialog
; UsePreviousPrivileges (standardmäßig yes) sorgt dafür, dass für ein Update dieselben Rechte wie bei der Erstinstallation angefordert werden.
DefaultDirName={autopf}\{#MyAppName}
AppPublisher=Wilhelm Happe
VersionInfoCopyright=(C) 2026, W. Happe
AppPublisherURL=https://www.netradio.info/
AppSupportURL=https://www.netradio.info/
AppUpdatesURL=https://www.netradio.info/
LicenseFile=Lizenzvereinbarung.txt
DisableWelcomePage=yes
DisableDirPage=no
DisableReadyPage=yes
CloseApplications=yes
WizardStyle=modern
WizardSizePercent=100
SetupIconFile=image.ico
UninstallDisplayIcon={app}\ImageCropper.exe
DefaultGroupName=ImageCropper
AppId=ImageCropper
TimeStampsInUTC=yes
OutputDir=.
OutputBaseFilename={#MyAppName}Setup
Compression=lzma2/max
SolidCompression=yes
DirExistsWarning=no
MinVersion=0,10.0
ChangesAssociations=yes

[Languages]
Name: "German"; MessagesFile: "compiler:Languages\German.isl"

[Files]
Source: "bin\x64\Release\net8.0-windows7.0\{#MyAppName}.exe"; DestDir: "{app}"; Flags: ignoreversion; Check: IsAdminInstallMode
Source: "bin\x64\Release\net8.0-windows7.0\{#MyAppName}.dll"; DestDir: "{app}"; Flags: ignoreversion; Check: IsAdminInstallMode
Source: "bin\x64\Release\net8.0-windows7.0\{#MyAppName}.runtimeconfig.json"; DestDir: "{app}"; Flags: ignoreversion; Check: IsAdminInstallMode
Source: "Lizenzvereinbarung.txt"; DestDir: "{app}"; Permissions: users-modify;
Source: "ImageCropper_Hilfe.pdf"; DestDir: "{app}"; Permissions: users-modify;
Source: "bin\x64\Release\net8.0-windows7.0\runtimes\win-x64\native\Magick.Native-Q8-x64.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "bin\x64\Release\net8.0-windows7.0\Magick.NET-Q8-x64.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "bin\x64\Release\net8.0-windows7.0\Magick.NET.Core.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "bin\x64\Release\net8.0-windows7.0\Magick.NET.SystemDrawing.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "bin\x64\Release\net8.0-windows7.0\System.Drawing.Common.dll"; DestDir: "{app}"; Flags: ignoreversion
; Source: "bin\x64\Release\net8.0-windows7.0\MediaDevices.dll"; DestDir: "{app}"; Flags: ignoreversion
; Self-Contained (Developer-PowerShell): dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=false
; Cave: Kopiert nach bin\Release\net8.0-windows7.0\win-x64\publish\
Source: "bin\Release\net8.0-windows7.0\win-x64\publish\*"; DestDir: "{app}"; Check: not IsAdminInstallMode 

[Icons]
Name: "{autodesktop}\{#MyAppLong}"; Filename: "{app}\{#MyAppName}.exe"; Tasks: desktopicon
Name: "{autoprograms}\{#MyAppName}"; Filename: "{app}\{#MyAppName}.exe"

[Tasks]
Name: desktopicon; Description: {cm:CreateDesktopIcon}

[Run]
Filename: "{app}\{#MyAppName}.exe"; Description: "Starte ImageCropper"; Flags: postinstall nowait skipifsilent shellexec

[Messages]
BeveledLabel=
WinVersionTooLowError=Das Programm erfordert eine höhere Windowsversion.
ConfirmUninstall=Möchten Sie '%1' von Ihrem PC entfernen? Eine Deinstallation ist vor einem Update nicht erforderlich.

[CustomMessages]
RemoveSettings=Möchten Sie die Einstellungsdatei ebenfalls entfernen?
Description=ImageCropper

[Code]
const
  SetupMutexName = 'ImageCropperSetupMutex';

function InitializeSetup(): Boolean; // only one instance of Inno Setup without prompting
begin
  Result := True;
  if CheckForMutexes(SetupMutexName) then
  begin
    Result := False; // Mutex exists, setup is running already, silently aborting
  end
    else
  begin
    CreateMutex(SetupMutexName); 
  end;
end;

procedure CurUninstallStepChanged (CurUninstallStep: TUninstallStep);
var
  mres : integer;
begin
  case CurUninstallStep of                   
    usPostUninstall:
      begin
        mres := MsgBox(CustomMessage('RemoveSettings'), mbConfirmation, MB_YESNO or MB_DEFBUTTON2);
        if mres = IDYES then
        begin
          DelTree(ExpandConstant('{userappdata}\{#MyAppName}'), True, True, True);
          RegDeleteKeyIncludingSubkeys(HKEY_CURRENT_USER, 'Software\{#MyAppName}');
        end;
      end;
  end;
end; 


