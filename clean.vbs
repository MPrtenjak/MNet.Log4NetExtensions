Option Explicit

Dim fso
Set fso = CreateObject("Scripting.FileSystemObject")

DirWalk(fso.GetAbsolutePathName(".")) 

Sub DirWalk(parmPath)
	Dim oSubDir, oSubFolder, oFile, n

	On Error Resume Next

	Set oSubFolder = fso.getfolder(parmPath)

	For Each oSubDir In oSubFolder.Subfolders
		if (oSubDir.name = "bin" or oSubDir.name = "obj") then
			Wscript.echo oSubDir
			fso.DeleteFolder oSubDir, true
		end if

		DirWalk oSubDir.Path
	Next

	On Error Goto 0
End Sub