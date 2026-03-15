; Copyright (C) 2010-2013 by Laurentiu-Ghoerghe Crisan
[Setup]
; NOTE: The value of AppId uniquely identifies this application.
; Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{17366DE1-0984-4F60-94BC-7546A59E5289}}
AppName=MeaProcess Realtime
AppVerName=MeaProcess Realtime (PARIS)
AppPublisher=Atesion GmbH
AppPublisherURL=http://www.atesion.de
AppSupportURL=http://www.atesion.de
AppUpdatesURL=http://www.atesion.de
DefaultDirName={pf}\Atesion\MeaProcess Realtime (PARIS)\
DefaultGroupName=MeaProcess Realtime (PARIS)
DisableProgramGroupPage=yes
OutputBaseFilename=MeaProcess Realtime (PARIS) - Setup
SetupIconFile=.\Atesion.ico
Compression=lzma
SolidCompression=yes
ChangesAssociations=yes
ArchitecturesInstallIn64BitMode=x64


[Code]

function InitializeSetup(): Boolean;
var
    NetFrameWorkInstalled : Boolean;
    Result1 : Boolean;
begin

      NetFrameWorkInstalled := RegKeyExists(HKLM,'SOFTWARE\Microsoft\.NETFramework\policy\v4.0');
      if NetFrameWorkInstalled =true then
      begin
            Result := true;
      end;

      if NetFrameWorkInstalled = false then
      begin
            NetFrameWorkInstalled := RegKeyExists(HKLM,'SOFTWARE\Microsoft\.NETFramework\policy\v4.0');
            if NetFrameWorkInstalled = true then
            begin

                  Result := true;
            end;

            if NetFrameWorkInstalled =false then
                  begin
                        Result1 := MsgBox('This setup requires the .NET Framework 4.5. Please download and install the .NET Framework and run this setup again.',
                                    mbConfirmation, MB_OK) = idYes;
                        if Result1 =false then
                        begin
                              Result:=false;
                        end
                        else
                        begin
                              Result:=false;
                              //Run the .NET redistributable here using shellexec.
          end;
      end;
      end;
end;

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"
Name: "german"; MessagesFile: "compiler:Languages\German.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked
Name: "quicklaunchicon"; Description: "{cm:CreateQuickLaunchIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
;Scheme
Source: ".\Deploy\mea\*"; DestDir: "{app}\"; Flags: ignoreversion
Source: ".\Deploy\mea\de-DE\*"; DestDir: "{app}\de-DE"; Flags: ignoreversion

;Runtime
Source: ".\Deploy\mea\rt\*"; DestDir: "{app}\rt"; Flags: ignoreversion
Source: ".\Deploy\mea\rt\de-DE\*"; DestDir: "{app}\rt\de-DE"; Flags: ignoreversion
Source: ".\Deploy\mea\rt\Mp.Runtime.Scripting.dll"; DestDir: {app}\rt\;  StrongAssemblyName: "Mp.Runtime.Scripting, Version=1.3.0.25, Culture=neutral, ProcessorArchitecture=MSIL"; Flags: ignoreversion gacinstall
Source: ".\Deploy\mea\rt\Mp.XmlRpc.dll"; DestDir: {app}\rt\;  StrongAssemblyName: "Mp.XmlRpc, Version=1.0.0.0, Culture=neutral, ProcessorArchitecture=MSIL"; Flags: ignoreversion gacinstall
Source: ".\Deploy\mea\rt\ICSharpCode.SharpZipLib.dll"; DestDir: {app}\rt\;  StrongAssemblyName: "ICSharpCode.SharpZipLib, Version=1.0.0.0, Culture=neutral, ProcessorArchitecture=MSIL"; Flags: ignoreversion gacinstall


[Icons]
Name: "{group}\Scheme Editor (PARIS)"; Filename: "{app}\Mp.Scheme.App.exe"
Name: "{group}\MPAL Editor (PARIS)"; Filename: "{app}\Mpal.Editor.exe"
Name: "{commondesktop}\MeaProcess Realtime (PARIS)"; Filename: "{app}\Mp.Scheme.App.exe"; Tasks: desktopicon
Name: "{userappdata}\Microsoft\Internet Explorer\Quick Launch\MeaProcess Realtime (PARIS)"; Filename: "{app}\Mp.Scheme.App.exe"; Tasks: quicklaunchicon


[Run]
Filename: "{app}\Mp.Scheme.App.exe"; Description: "{cm:LaunchProgram,MeaProcess Realtime (PARIS)}"; Flags: nowait postinstall skipifsilent

[Registry]

;.mpw file association (Windows)
Root: HKCR; Subkey: ".mpw"; ValueType: string; ValueName: ""; ValueData: "Mp.Scheme.App.exe"; Flags: uninsdeletevalue
Root: HKCR; Subkey: "Mp.Scheme.App.exe"; ValueType: string; ValueName: ""; ValueData: "MeaProcess- Scheme Editor"; Flags: uninsdeletekey
Root: HKCR; Subkey: "Mp.Scheme.App.exe\DefaultIcon"; ValueType: string; ValueName: ""; ValueData: "{app}\MeaProcessFile.ico"
Root: HKCR; Subkey: "Mp.Scheme.App.exe\shell\open\command"; ValueType: string; ValueName: ""; ValueData: """{app}\Mp.Scheme.App.exe"" ""%1"""

;.mpal file association
Root: HKCR; Subkey: ".mpal"; ValueType: string; ValueName: ""; ValueData: "Mpal.Editor.exe"; Flags: uninsdeletevalue
Root: HKCR; Subkey: "Mpal.Editor.exe"; ValueType: string; ValueName: ""; ValueData: "MPAL Editor"; Flags: uninsdeletekey
Root: HKCR; Subkey: "Mpal.Editor.exe\DefaultIcon"; ValueType: string; ValueName: ""; ValueData: "{app}\MeaProcessFile.ico"
Root: HKCR; Subkey: "Mpal.Editor.exe\shell\open\command"; ValueType: string; ValueName: ""; ValueData: """{app}\Mpal.Editor.exe"" ""%1"""

;MeaProcess registry keys
Root: HKCU; Subkey: "Software\Atesion"; ValueType: string; ValueName: ""; ValueData: ""; Flags: uninsdeletekeyifempty
Root: HKCU; Subkey: "Software\Atesion\MeaProcess Realtime (PARIS)"; ValueType: string; ValueName: ""; ValueData: "";  Flags: uninsdeletekey
Root: HKCU; Subkey: "Software\Atesion\MPAL Editor"; ValueType: string; ValueName: ""; ValueData: "";  Flags: uninsdeletekey
Root: HKCU; Subkey: "Software\Atesion\MeaProcess Realtime (PARIS)\Runtime"; ValueType: string; ValueName: ""; ValueData: "";  Flags: uninsdeletekey
Root: HKCU; Subkey: "Software\Atesion\MeaProcess Realtime (PARIS)\Runtime"; ValueType: string; ValueName: "Path"; ValueData: "{app}\rt\";  Flags: uninsdeletekey
Root: HKCU; Subkey: "Software\Atesion\MeaProcess Realtime (PARIS)\Scheme"; ValueType: string; ValueName: ""; ValueData: "";  Flags: uninsdeletekey
Root: HKCU; Subkey: "Software\Atesion\MeaProcess Realtime (PARIS)\Scheme"; ValueType: string; ValueName: "Path"; ValueData: "{app}\";  Flags: uninsdeletekey
