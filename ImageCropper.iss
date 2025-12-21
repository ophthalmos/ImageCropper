#define MyAppLong "ImageCropper"
#define MyAppName "ImageCropper"
#define MyAppVersion "1.0.0"

[Setup]
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppVerName={#MyAppLong} {#MyAppVersion}
VersionInfoVersion={#MyAppVersion}
ArchitecturesAllowed=x64os
ArchitecturesInstallIn64BitMode=x64os
PrivilegesRequired=admin
AppPublisher=Wilhelm Happe
VersionInfoCopyright=(C) 2025, W. Happe
AppPublisherURL=https://www.netradio.info/
AppSupportURL=https://www.netradio.info/
AppUpdatesURL=https://www.netradio.info/
DefaultDirName={autopf}\{#MyAppName}
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
Source: "bin\x64\Release\net8.0-windows7.0\{#MyAppName}.exe"; DestDir: "{app}"; Permissions: users-modify; Flags: ignoreversion
Source: "bin\x64\Release\net8.0-windows7.0\{#MyAppName}.dll"; DestDir: "{app}"; Permissions: users-modify; Flags: ignoreversion
Source: "bin\x64\Release\net8.0-windows7.0\{#MyAppName}.runtimeconfig.json"; DestDir: "{app}"; Permissions: users-modify; Flags: ignoreversion
Source: "Lizenzvereinbarung.txt"; DestDir: "{app}"; Permissions: users-modify;
Source: "bin\x64\Release\net8.0-windows7.0\Magick.Native-Q8-x64.dll"; DestDir: "{app}"; Permissions: users-modify; Flags: ignoreversion
Source: "bin\x64\Release\net8.0-windows7.0\Magick.NET-Q8-x64.dll"; DestDir: "{app}"; Permissions: users-modify; Flags: ignoreversion
Source: "bin\x64\Release\net8.0-windows7.0\Magick.NET.Core.dll"; DestDir: "{app}"; Permissions: users-modify; Flags: ignoreversion
Source: "bin\x64\Release\net8.0-windows7.0\Magick.NET.SystemDrawing.dll"; DestDir: "{app}"; Permissions: users-modify; Flags: ignoreversion
Source: "bin\x64\Release\net8.0-windows7.0\System.Drawing.Common.dll"; DestDir: "{app}"; Permissions: users-modify; Flags: ignoreversion

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
        mres := MsgBox(CustomMessage('RemoveSettings'), mbConfirmation, MB_YESNO or MB_DEFBUTTON2)
        if mres = IDYES then
          begin
          DelTree(ExpandConstant('{userappdata}\{#MyAppName}'), True, True, True);
          RegDeleteKeyIncludingSubkeys(HKEY_CURRENT_USER, 'Software\{#MyAppName}');
          end;
      end;
  end;
end; 

procedure DeinitializeSetup();
var
  FilePath: string;
  BatchPath: string;
  S: TArrayOfString;
  ResultCode: Integer;
begin
  if ExpandConstant('{param:deleteSetup|false}') = 'true' then
  begin
    FilePath := ExpandConstant('{srcexe}');
    begin
      BatchPath := ExpandConstant('{%TEMP}\') + 'delete_' + ExtractFileName(ExpandConstant('{tmp}')) + '.bat';
      SetArrayLength(S, 7);
      S[0] := ':loop';
      S[1] := 'del "' + FilePath + '"';
      S[2] := 'if not exist "' + FilePath + '" goto end';
      S[3] := 'goto loop';
      S[4] := ':end';
      S[5] := 'rd "' + ExpandConstant('{tmp}') + '"';
      S[6] := 'del "' + BatchPath + '"';
      if SaveStringsToFile(BatchPath, S, True) then
      begin
        Exec(BatchPath, '', '', SW_HIDE, ewNoWait, ResultCode)
      end;
    end;
  end;
end;

