#
#Config
#
#The Name of the program + install Name
Name "NSA 4 Dummies"
#The file that shall be created
OutFile "nsa_v0.3.1_installer.exe"
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

#Main section
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