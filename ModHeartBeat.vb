Imports System.IO
Imports System.Threading

Module ModHeartBeat
    Public Function MonitorHeartBeat() As String
        'check if processes are running and write according textfile in webserver location
        Dim startExecution As Date
        Dim executionTime As TimeSpan

        MonitorHeartBeat = "OK"
        Try
            startExecution = DateTime.UtcNow()
            LogSessionEntry("DEBUG", "  MonitorHeartBeat...", "", "MonitorHeartBeat", "PROGRAM")

            'write file to webserver
            'Dim file As System.IO.StreamWriter

            'check and create folder
            If Directory.Exists(My.Settings.sLogLocation) = False Then
                Directory.CreateDirectory(My.Settings.sLogLocation)
            End If

            'file = My.Computer.FileSystem.OpenTextFileWriter(My.Settings.sLogLocation + "\Heartbeat.txt", vbFalse)
            'file.WriteLine(DateTime.UtcNow)
            'file.Close()

            Dim NumberOfRetries As Integer = 3
            Dim DelayOnRetry As Integer = 500
            Dim i As Integer = 0

            Do While i < NumberOfRetries
                Try
                    My.Computer.FileSystem.WriteAllText(My.Settings.sLogLocation + "Heartbeat.txt", DateTime.UtcNow.ToString, False)
                    Thread.Sleep(DelayOnRetry)
                    Exit Do
                Catch ex As Exception
                    'do nothing
                    i += 1
                End Try
            Loop
            executionTime = DateTime.UtcNow() - startExecution
            LogSessionEntry("DEBUG", "  MonitorHeartbeat: " + executionTime.ToString, "", "MonitorHeartbeat", "PROGRAM")

        Catch ex As Exception
            LogSessionEntry("ERROR", "MonitorHeartbeat: " + ex.ToString, "", "MonitorHeartbeat", "PROGRAM")
            MonitorHeartBeat = "MonitorHeartbeat: " + ex.ToString
        End Try
    End Function
End Module
