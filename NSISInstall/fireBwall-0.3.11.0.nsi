; fireBwall-0.3.11.0.nsi
!include LogicLib.nsh
!include ZipDLL.nsh
; Macro Section
!macro IfKeyExists ROOT MAIN_KEY KEY
push $R0
push $R1
 
!define Index 'Line${__LINE__}'
 
StrCpy $R1 "0"
 
"${Index}-Loop:"
; Check for Key
EnumRegKey $R0 ${ROOT} "${MAIN_KEY}" "$R1"
StrCmp $R0 "" "${Index}-False"
  IntOp $R1 $R1 + 1
  StrCmp $R0 "${KEY}" "${Index}-True" "${Index}-Loop"
 
"${Index}-True:"
;Return 1 if found
push "1"
goto "${Index}-End"
 
"${Index}-False:"
;Return 0 if not found
push "0"
goto "${Index}-End"
 
"${Index}-End:"
!undef Index
exch 2
pop $R0
pop $R1
!macroend
;

;--------------------------------

!define VERSION "0.3.11.0"

; The name of the installer
Name "firebwall ${VERSION}"

; The file to write
OutFile "firebwall-${VERSION}.exe"

; The default installation directory
InstallDir $PROGRAMFILES\fireBwall

; Registry key to check for directory (so if you install again, it will 
; overwrite the old one automatically)
InstallDirRegKey HKLM "Software\fireBwall" "Install_Dir"

; Request application privileges
RequestExecutionLevel admin

;--------------------------------

; Pages

Page components
Page directory
Page instfiles

UninstPage uninstConfirm
UninstPage instfiles

;--------------------------------

Function InstallWinpkfilter
	!insertmacro IfKeyExists "HKLM" "SYSTEM\CurrentControlSet\services" "ndisrd"
	Pop $R0
	${If} $R0 != 0
		nsisdl::download /TIMEOUT=30000 "http://www.ntkernel.com/downloads/winpkflt_rtl.zip" "$INSTDIR\winpkflt_rtl.zip"
		!insertmacro ZIPDLL_EXTRACT "$INSTDIR\winpkflt_rtl.zip" "$INSTDIR" "winpkflt_rtl.exe"
		ExecWait "$INSTDIR\winpkflt_rtl.exe" 
	${EndIf}
FunctionEnd

Function RunOnStartup
	WriteRegExpandStr HKCU "SOFTWARE\Microsoft\Windows\CurrentVersion\Run" "firebwall" "$INSTDIR\fireBwall.exe"
FunctionEnd

Section "fireBwall ${VERSION} (required)"

  SectionIn RO
  
  ; Set output path to the installation directory.
  SetOutPath $INSTDIR
  
  ; Put file there
  File "fireBwall.exe"
  File "FirewallModule.dll"
  File "ndisapi.dll"
  File "winpkflt_rtl.exe"
  Call InstallWinpkfilter
  Call RunOnStartup
  CreateDirectory "$APPDATA\firebwall"
  CreateDirectory "$APPDATA\firebwall\modules"
  
  ; Write the installation path into the registry
  WriteRegStr HKLM SOFTWARE\fireBwall "Install_Dir" "$INSTDIR"
  
  ; Write the uninstall keys for Windows
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\fireBwall" "DisplayName" "fireBwall ${VERSION}"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\fireBwall" "UninstallString" "$INSTDIR\uninstall.exe"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\fireBwall" "Publisher" "bwall"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\fireBwall" "DisplayIcon" "$INSTDIR\fireBwall.exe"
  WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\fireBwall" "NoModify" 1
  WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\fireBwall" "NoRepair" 1
  WriteUninstaller "uninstall.exe"
  
SectionEnd

SectionGroup "Modules"

Section "Basic Firewall"
	SetOutPath $APPDATA\fireBwall\modules
	File "BasicFirewall.dll"
SectionEnd

Section "ARP Poisoning Protection"
	SetOutPath $APPDATA\fireBwall\modules
	File "ARPPoisoningProtection.dll"
SectionEnd

Section "Denial of Service Protection"
	SetOutPath $APPDATA\fireBwall\modules
	File "DDoS.dll"
SectionEnd

Section "ICMP Filter"
	SetOutPath $APPDATA\fireBwall\modules
	File "ICMPFilter.dll"
SectionEnd

Section "IP Monitor"
	SetOutPath $APPDATA\fireBwall\modules
	File "IPMonitor.dll"
SectionEnd

Section "MAC Address Filter"
	SetOutPath $APPDATA\fireBwall\modules
	File "MacFilter.dll"
SectionEnd

Section "Save Flash Video"
	SetOutPath $APPDATA\fireBwall\modules
	File "SaveFlashVideo.dll"
SectionEnd

Section "IP Guard"
	SetOutPath $APPDATA\fireBwall\modules
	File "IPGuard.dll"
SectionEnd

SectionGroupEnd

; Optional section (can be disabled by the user)
Section "Start Menu Shortcuts"

  CreateDirectory "$SMPROGRAMS\fireBwall"
  CreateShortCut "$SMPROGRAMS\fireBwall\Uninstall.lnk" "$INSTDIR\uninstall.exe" "" "$INSTDIR\uninstall.exe" 0
  CreateShortCut "$SMPROGRAMS\fireBwall\fireBwall.lnk" "$INSTDIR\fireBwall.exe" "" "$INSTDIR\fireBwall.exe" 0
  
SectionEnd

;--------------------------------

; Uninstaller

Section "Uninstall"
  
  ; Remove registry keys
  DeleteRegKey HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\fireBwall"
  DeleteRegKey HKLM SOFTWARE\fireBwall

  ; Remove files and uninstaller
  Delete "$INSTDIR\*.*"

  ; Remove shortcuts, if any
  Delete "$SMPROGRAMS\fireBwall\*.*"

  ; Remove directories used
  RMDir "$SMPROGRAMS\fireBwall"
  RMDir "$INSTDIR"

SectionEnd
