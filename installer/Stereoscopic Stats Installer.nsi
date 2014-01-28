; Stereoscopic Stats Installer.nsi
;
;--------------------------------

; The name of the installer
Name "Stereoscopic Stats Installer"

; The file to write
OutFile "Stereoscopic Stats Installer.exe"

; The default installation directory
InstallDir $PROGRAMFILES\Stereoscopic_Stats

; The text to prompt the user to enter a directory
DirText "This will install Stereoscopic Stats on your computer. Please choose a directory."

; Registry key to check for directory (so if you install again, it will 
; overwrite the old one automatically)
InstallDirRegKey HKLM "Software\NSIS_Stereoscopic_Stats" "Install_Dir"

; Request application privileges for Windows Vista
RequestExecutionLevel admin

;--------------------------------

; Pages

Page directory
Page instfiles

UninstPage uninstConfirm
UninstPage instfiles

;--------------------------------

; The stuff to install
Section "Stereoscopic Stats (required)"

  SectionIn RO
  
  ; Set output path to the installation directory.
  SetOutPath $INSTDIR
  
  ; Copy files
  File "Stereoscopic Stats.exe"
  File /r "Stereoscopic Stats_Data"
  
  ; Write the installation path into the registry
  WriteRegStr HKLM SOFTWARE\NSIS_Stereoscopic_Stats "Install_Dir" "$INSTDIR"
  
  ; Write the uninstall keys for Windows
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Stereoscopic_Stats" "DisplayName" "Stereoscopic Stats"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Stereoscopic_Stats" "UninstallString" '"$INSTDIR\uninstall.exe"'
  WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Stereoscopic_Stats" "NoModify" 1
  WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Stereoscopic_Stats" "NoRepair" 1
  WriteUninstaller "uninstall.exe"
  
SectionEnd

; Optional section (can be disabled by the user)
Section "Start Menu Shortcuts"

  CreateDirectory "$SMPROGRAMS\Stereoscopic Stats"
  CreateShortCut "$SMPROGRAMS\Stereoscopic Stats\Uninstall.lnk" "$INSTDIR\uninstall.exe" "" "$INSTDIR\uninstall.exe" 0
  CreateShortCut "$SMPROGRAMS\Stereoscopic Stats\Stereoscopic Stats.lnk" "$INSTDIR\Stereoscopic Stats.exe" "" "$INSTDIR\Stereoscopic Stats.exe" 0
  
SectionEnd

;--------------------------------

; Uninstaller

Section "Uninstall"
  
  ; Remove registry keys
  DeleteRegKey HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Stereoscopic_Stats"
  DeleteRegKey HKLM SOFTWARE\Stereoscopic_Stats

  ; Remove files and uninstaller
  Delete "$INSTDIR\Stereoscopic Stats Installer.nsi"
  Delete $INSTDIR\uninstall.exe

  ; Remove shortcuts, if any
  Delete "$SMPROGRAMS\Stereoscopic_Stats\*.*"

  ; Remove directories used
  RMDir "$SMPROGRAMS\Stereoscopic_Stats"
  RMDir "$INSTDIR"

SectionEnd
