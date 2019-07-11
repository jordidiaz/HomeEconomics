SET path=%~dp0HomeEconomics.exe
sc stop HomeEconomics
sc create HomeEconomics binPath=%path%
sc start HomeEconomics
pause