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
BrandingText "NSA 4 Dummies"

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


#Section - "WinPcap"
Section "WinPcap" SEC01

        MessageBox MB_YESNOCANCEL "                                                   Install WinPcap? $\r$\n$\r$\nNOTE: In order for NSA4Dummies to work properly, WinPcap is required." /SD IDYES IDNO endWinPcap IDCANCEL WinPcap_QUIT
					
		SetOutPath $INSTDIR		
          File /r "..\Installer\WinPcap.exe"
          ExecWait "$INSTDIR\WinPcap.exe"
          Goto endWinPcap
		  
		WinPcap_QUIT:
		QUIT 
		  
	endWinPcap:	

SectionEnd	
#Section - "NSA4Dummies
Section "NSA4Dummies" SEC02
		
		SetOutPath $INSTDIR
		  File /r "..\Softwareprojekt2015\bin\Release\*"
		  WriteUninstaller $INSTDIR\uninstall.exe

SectionEnd

#Uninstall section
Section "Uninstall"
  RMDir /r $INSTDIR
SectionEnd