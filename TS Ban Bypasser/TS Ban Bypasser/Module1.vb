Imports System.IO
Imports System.Text
Imports System.Threading
Imports Microsoft.Win32
Module Module1

    Private Const MF_BYCOMMAND As Integer = &H0
    Public Const SC_CLOSE As Integer = &HF060
    Public Const SC_MINIMIZE As Integer = &HF020
    Public Const SC_MAXIMIZE As Integer = &HF030
    Public Const SC_SIZE As Integer = &HF000

    Friend Declare Function DeleteMenu Lib "user32.dll" (ByVal hMenu As IntPtr, ByVal nPosition As Integer, ByVal wFlags As Integer) As Integer
    Friend Declare Function GetSystemMenu Lib "user32.dll" (hWnd As IntPtr, bRevert As Boolean) As IntPtr

    Sub Write(ByRef strings As IEnumerable(Of String), ByRef colors As IEnumerable(Of ConsoleColor))

        'https://stackoverflow.com/questions/19625043/color-in-console-application
        'Credits to: HighTechProgramming15

        Dim i As Integer = 0
        For Each s In strings
            Console.ForegroundColor = colors(i)
            Console.Write(s)
            i += 1
        Next
    End Sub

    Sub WriteLine(ByRef strings As IEnumerable(Of String), ByRef colors As IEnumerable(Of ConsoleColor))

        'https://stackoverflow.com/questions/19625043/color-in-console-application
        'Credits to: HighTechProgramming15

        Dim i As Integer = 0
        For Each s In strings
            Console.ForegroundColor = colors(i)
            Console.WriteLine(s)
            i += 1
        Next
    End Sub

    Sub Main()

        Dim handle As IntPtr
        handle = Process.GetCurrentProcess.MainWindowHandle

        Dim sysMenu As IntPtr
        sysMenu = GetSystemMenu(handle, False)

        If handle <> IntPtr.Zero Then
            DeleteMenu(sysMenu, SC_MAXIMIZE, MF_BYCOMMAND)
            DeleteMenu(sysMenu, SC_SIZE, MF_BYCOMMAND)
            DeleteMenu(sysMenu, SC_CLOSE, MF_BYCOMMAND)
        End If

        Dim origWidth, width As Integer
        Dim origHeight, height As Integer
        origWidth = Console.WindowWidth
        origHeight = Console.WindowHeight
        width = origWidth
        height = origHeight
        Console.SetWindowSize(width, height)
        Console.SetBufferSize(width, height)


        Dim path As String = "C:\ProgramData\TSBypass\default.txt"
        Dim defaultid As String = My.Computer.Registry.GetValue("HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\", "ProductID", "")
        If Directory.Exists("C:\ProgramData\TSBypass") = True Then
        Else
            Directory.CreateDirectory("C:\ProgramData\TSBypass")
            Dim fs As FileStream = File.Create(Path)
            Dim info As Byte() = New UTF8Encoding(True).GetBytes(defaultid)
            fs.Write(info, 0, info.Length)
            fs.Close()

        End If

label:

        Console.Title = "Teamspeak ban bypasser"
        Console.Write("")
        Write({"
                ______                    _____                  __      ____                             
               /_  __/__  ____ _____ ___ / ___/____  ___  ____ _/ /__   / __ )__  ______  ____ ___________
                / / / _ \/ __ `/ __ `__ \\__ \/ __ \/ _ \/ __ `/ //_/  / __  / / / / __ \/ __ `/ ___/ ___/
               / / /  __/ /_/ / / / / / /__/ / /_/ /  __/ /_/ / ,<    / /_/ / /_/ / /_/ / /_/ (__  |__  ) 
              /_/  \___/\__,_/_/ /_/ /_/____/ .___/\___/\__,_/_/|_|  /_____/\__, / .___/\__,_/____/____/  
                                           /_/                             /____/_/                       "}, {ConsoleColor.Red})
        Console.WriteLine()
        Console.WriteLine()


        Console.WriteLine()
        Write({" -Type ", "bypass", " to start the bypassing process "}, {ConsoleColor.White, ConsoleColor.Red, ConsoleColor.White})
        Console.WriteLine()
        Console.WriteLine()
        Write({" -Type ", "restore", " to undo the bypassing process "}, {ConsoleColor.White, ConsoleColor.Green, ConsoleColor.White})
        Console.WriteLine()
        Console.WriteLine()
        Write({" -Type ", "Help", " To get more info on how to work this program "}, {ConsoleColor.White, ConsoleColor.DarkGreen, ConsoleColor.White})
        Console.WriteLine()
        Console.WriteLine()
        Write({" -Type ", "Close", " To exit the program "}, {ConsoleColor.White, ConsoleColor.DarkRed, ConsoleColor.White})

        Console.WriteLine()
        Console.WriteLine()

        Dim Input As String = Console.ReadLine

        If Input.Contains("bypass") Then
            Dim content As String = ""
            Using textReader As New System.IO.StreamReader("C:\ProgramData\TSBypass\default.txt")
                content = textReader.ReadToEnd
            End Using
            If My.Computer.Registry.GetValue("HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\", "ProductID", "").Equals(content) Then

                Dim p() As Process
                Dim nameProcess() As System.Diagnostics.Process = System.Diagnostics.Process.GetProcesses

                p = Process.GetProcessesByName("ts3client_win64")
                If p.Count > 0 Then
                    For i = 0 To nameProcess.Length - 1
                        If nameProcess(i).ProcessName.StartsWith("ts3client_win64") Then
                            Dim exeName As String = nameProcess(i).ProcessName
                            Dim proc() As Process = Process.GetProcessesByName(exeName)
                            For Each temp As Process In proc
                                temp.Kill()
                            Next
                        End If
                    Next
                    Threading.Thread.Sleep(500)
                End If

                My.Computer.Registry.SetValue("HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\", "ProductID", "")
                Console.WriteLine("Success")
                Threading.Thread.Sleep(2000)
                Console.Clear()

            Else

                Console.WriteLine("Already Bypassed")
                Threading.Thread.Sleep(2000)
                Console.Clear()
            End If




        End If

        If Input.Contains("restore") Then

            Dim content As String = ""
            Using textReader As New System.IO.StreamReader("C:\ProgramData\TSBypass\default.txt")
                content = textReader.ReadToEnd
            End Using

            If My.Computer.Registry.GetValue("HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\", "ProductID", "").Equals(content) Then
                Console.WriteLine("Nothing to revert")
                Threading.Thread.Sleep(2000)
                Console.Clear()

            Else

                'Directory.Delete("C:\ProgramData\TSBypass", True)
                My.Computer.Registry.SetValue("HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\", "ProductID", "" & content & "")
                Console.WriteLine("restored back to default")

                Threading.Thread.Sleep(2000)
                Console.Clear()

            End If
        End If

        If Input.Contains("help") Then
            Console.Clear()
            Console.WriteLine("Step-1")
            Console.WriteLine("Once you're done reading this you type: 'bypass'")
            Console.WriteLine("")
            Console.WriteLine("Step-2")
            Console.WriteLine("Your TS will close, once that happens you connnect to a vpn.")
            Console.WriteLine("")
            Console.WriteLine("Step-3")
            Console.WriteLine("Once you're connected to the vpn, open TS")
            Console.WriteLine("")
            Console.WriteLine("Step-4")
            Console.WriteLine("Press tool > identity (Ctrl+i) and make a new identity.")
            Console.WriteLine("")
            Console.WriteLine("Step-5")
            Console.WriteLine("Press connections > Connect (Ctrl+S). In the bottom left you see a button that says 'more'
once you click that a lot of things appear. You want to press 'identity' and select the one you made in step 4.")
            Console.WriteLine("")
            Console.WriteLine("Step-6")
            Console.WriteLine("Connnect to the server you are banned from with the new identity")
            Console.WriteLine("")
            Console.WriteLine("")
            Console.WriteLine("Press anykey to return the start.")
            Console.ReadKey()
            Console.Clear()
        End If

        If Input.Contains("close") Then
            Console.Clear()
            Console.WriteLine("Teamspeak Ban Bypasser")
            Write({" -Made By ", "LevensLes#0001 "}, {ConsoleColor.White, ConsoleColor.Magenta})
            Thread.Sleep(2000)
            Process.GetCurrentProcess.Kill()
        End If

        If Input.Contains("restore") Or Input.Contains("bypass") Or Input.Contains("help") Or Input.Contains("close") Then

        Else

            Console.WriteLine("the command you typed is incorrect")
            Threading.Thread.Sleep(2000)
            Console.Clear()

        End If

        GoTo label
        Console.Read()

    End Sub
End Module
