Imports System.Net
Imports System.Net.NetworkInformation

Module ModInternet
    'Public pIsInternetConnected As Boolean
    Public pLastKnownInternetConnected As DateTime


    Public Function IsInternetConnected() As String
        Dim startExecution As Date
        Dim executionTime As TimeSpan

        IsInternetConnected = "OK"
        Try
            startExecution = DateTime.UtcNow()

            If My.Settings.sDebugKillInternet = False Then
                'Dim IPHost As IPHostEntry = Dns.GetHostEntry("www.google.com")
                Dim IPAddress() As IPAddress = Dns.GetHostAddresses("www.google.com")
                pLastKnownInternetConnected = DateTime.UtcNow
            Else
                IsInternetConnected = "INTERNET IS DEAD"
                LogSessionEntry("FULL", "isInternetConnected: Internet is DEAD", "", "isInternetConnected", "INTERNET")
                Exit Function
            End If
            executionTime = DateTime.UtcNow() - startExecution
            LogSessionEntry("DEBUG", "  Internetis connected: " + executionTime.ToString, "", "isInternetConnected", "INTERNET")

        Catch ex As Exception
            IsInternetConnected = "isInternetConnected: " + ex.Message
            LogSessionEntry("FULL", "isInternetConnected: " + ex.Message, "", "isInternetConnected", "INTERNET")
        End Try
    End Function

    'Function IsInternetConnected() As Boolean
    '
    'Return New Ping().Send("www.google.com").Status = IPStatus.Success
    '
    'End Function



    Public Function CheckTimeoutInternet() As String
        'wrapper that takes into account a delay before the alarm is raised
        Dim returnvalue As String
        Dim i As Long
        Dim startExecution As Date
        Dim executionTime As TimeSpan

        CheckTimeoutInternet = "OK"
        Try
            startExecution = DateTime.UtcNow()
            returnvalue = IsInternetConnected()
            If returnvalue <> "OK" Then
                CheckTimeoutInternet = "OK"
                'DO NOT EXIT FUNCTION BUT CONTINUE TO SET STATUS CORRECTLY 
                'VALUE REMAINS OK UNTIL TIMEOUT IS REACHED
            End If

            i = DateDiff(DateInterval.Second, pLastKnownInternetConnected, DateTime.UtcNow)

            'log file is not changing
            If i >= My.Settings.sInternetTimeout Then
                FrmMain.LblInternet.BackColor = Color.Red
                FrmMain.LblInternet.Text = "NO WWW"
                LogSessionEntry("ERROR", "Internet not connected!", "", "CheckTimeoutInternet", "INTERNET")
                CheckTimeoutInternet = "NOK"
            Else
                FrmMain.LblInternet.BackColor = Color.Green
                FrmMain.LblInternet.Text = "INTERNET"
                CheckTimeoutInternet = "OK"
                LogSessionEntry("DEBUG", "  Internet OK!", "", "CheckTimeoutInternet", "INTERNET")
            End If
            executionTime = DateTime.UtcNow() - startExecution
            LogSessionEntry("DEBUG", "  CheckTimeoutInternet: " + executionTime.ToString, "", "CheckTimeoutInternet", "INTERNET")

        Catch ex As Exception
            CheckTimeoutInternet = "CheckTimeoutInternet: " + ex.Message
            LogSessionEntry("ERROR", "CheckTimeoutInternet: " + ex.Message, "", "CheckTimeoutInternet", "INTERNET")
        End Try
    End Function
End Module
