#
#Config
#
#The Name of the program + install Name
Name "NSA 4 Dummies"
#The file that shall be created
OutFile "nsa_v0.3.2_installer.exe"
#The installation path
InstallDir $PROGRAMFILES\NSA4Dummies
#The file that contains the license info
LicenseData "license.txt"
#File icon
Icon "img/icon.ico"
#The Text that is shown on the bottom
BrandingText "NSA4Dummies"

#
#Installer-Pages
#
Page license
Page directory
Page instfiles

#
#Uninstaller-Pages
#
UninstPage uninstConfirm
UninstPage instfiles

#Section - "WinPcap" SEC01
Section ""
        SetOutPath $INSTDIR
        MessageBox MB_YESNOCANCEL "                                                   Install WinPcap? $\r$\n$\r$\nNOTE: In order for NSA4Dummies to work properly, WinPcap is required" /SD IDYES IDNO endWinPcap
        #relative to path on github
          File /r "..\Softwareprojekt2015\bin\Release\*"
          ExecWait "$INSTDIR\Softwareprojekt2015\bin\Release\WinPcap.exe"
          Goto endWinPcap
        endWinPcap:
SectionEnd

#Section "NSA4Dummies" SEC02
Section ""
	SetOutPath $INSTDIR
	# relative to path on github
	File /r "..\Softwareprojekt2015\bin\Release\*"
	WriteUninstaller $INSTDIR\uninstall.exe
SectionEnd

#Uninstall section
Section "Uninstall"
  RMDir /r $INSTDIR
SectionEnd