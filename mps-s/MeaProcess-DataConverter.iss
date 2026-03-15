; Copyright (C) 2010-2013 by Laurentiu-Ghoerghe Crisan
[Setup]
; NOTE: The value of AppId uniquely identifies this application.
; Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{28B03C3E-4213-41F8-BACC-9B7503498B23}}
AppName=MeaProcess DataConverter
AppVerName=MeaProcess DataConverter (PARIS)
AppPublisher=Atesion GmbH
AppPublisherURL=http://www.atesion.de
AppSupportURL=http://www.atesion.de
AppUpdatesURL=http://www.atesion.de
DefaultDirName={pf}\Atesion\MeaProcess Realtime (PARIS)\
DefaultGroupName=MeaProcess Realtime (PARIS)
DisableProgramGroupPage=yes
OutputBaseFilename=MeaProcess (PARIS) - DataConverter
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

      NetFrameWorkInstalled := RegKeyExists(HKLM,'SOFTWARE\Microsoft\.NETFramework\policy\v2.0');
      if NetFrameWorkInstalled =true then
      begin
            Result := true;
      end;

      if NetFrameWorkInstalled = false then
      begin
            NetFrameWorkInstalled := RegKeyExists(HKLM,'SOFTWARE\Microsoft\.NETFramework\policy\v2.0');
            if NetFrameWorkInstalled = true then
            begin

                  Result := true;
            end;

            if NetFrameWorkInstalled =false then
                  begin
                        Result1 := MsgBox('This setup requires the .NET Framework 2.0. Please download and install the .NET Framework and run this setup again.',
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

[Files]
Source: ".\Deploy\converter\*"; DestDir: "{app}\"; Flags: ignoreversion
Source: ".\Deploy\converter\de-DE\*"; DestDir: "{app}\de-DE"; Flags: ignoreversion


[Icons]
Name: "{group}\DataConverter (PARIS)"; Filename: "{app}\Mp.Conv.Gui.exe"


[Run]

[Registry]
