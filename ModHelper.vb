Imports System.ComponentModel
Imports System.IO
Imports System.IO.Compression
Imports System.Text.RegularExpressions
Imports System.Threading
Imports System.Web
Imports ASCOM.Astrometry

Module ModHelper
    Public pAudioPlaying As Boolean

    Public pLogFileName, pLogFileNameRTF, pLogFileNameFull As String
    Public pWeatherLogFileName As String
    Public pLogFile, pLogFileFull, pLogFileRTF As System.IO.StreamWriter
    Public pWeatherFile As System.IO.StreamWriter

    Public pOldProgramMessage(4) As String  'Old program message, prevents double lines in log
    Public pOldWeatherMessage(4) As String  'Old weather message, prevents double lines in log
    Public pOldWeatherStatusMessage(4) As String  'Old weather message, prevents double lines in log
    Public pOldSequenceMessage(4) As String 'Old sequence message, prevents double lines in log
    Public pOldTSXMessage(4) As String      'Old TSX message, prevents double lines in log
    Public pOldInternetMessage(4) As String 'Old internet message, prevents double lines in log
    Public pOldRoofMessage(4) As String     'Old roof message, prevents double lines in log
    Public pOldCoverMessage(4) As String  'Old cover message, prevents double lines in log
    Public pOldSwitchMessage(4) As String   'Old switch message, prevents double lines in log
    Public pOldMountMessage(4) As String    'Old mount message, prevents double lines in log
    Public pOldUPSMessage(4) As String      'Old UPS message, prevents double lines in log
    Public pOldSmartMessage(4) As String      'Old UPS message, prevents double lines in log

    Public pOldProgramTime(4) As Date   'Contains last date a message was processed
    Public pOldWeatherTime(4) As Date   'Contains last date a message was processed
    Public pOldWeatherStatusTime(4) As Date   'Contains last date a message was processed
    Public pOldSequenceTime(4) As Date  'Contains last date a message was processed
    Public pOldTSXTime(4) As Date       'Contains last date a message was processed
    Public pOldInternetTime(4) As Date  'Contains last date a message was processed
    Public pOldRoofTime(4) As Date      'Contains last date a message was processed
    Public pOldCoverTime(4) As Date   'Contains last date a message was processed
    Public pOldSwitchTime(4) As Date    'Contains last date a message was processed
    Public pOldMountTime(4) As Date     'Contains last date a message was processed
    Public pOldUPSTime(4) As Date       'Contains last date a message was processed
    Public pOldSmartTime(4) As Date     'Contains last date a message was processed

    Public pOldProgramTimeSMART As Date    'Contains the last date a mount/sequence/TSX: monitors if program hangs for SMART reporting
    Public pOldSequenceTimeSMART As Date    'Contains the last date a sequence message was logged: used for SMART error reporting

    Public pOldRunStatus As String

    Private WithEvents Bgw As BackgroundWorker
    Private ReadOnly BgwError As String

    Private WithEvents Bgw7 As BackgroundWorker
    Private ReadOnly Bgw7Error As String

    Private PZipfolder As String
    Private PZipFile As String
    Private PZipSynologyLocation As String

    Public pMessageBoxTitle As String
    Public pMessageBoxText As String
    Public pMessageBoxType As String 'CRITICAL, OKONLY, YESNO, OKCANCEL


    Public Function ShowMessage(vMessage As String, vType As String, vTitle As String) As DialogResult
        Dim svalue As DialogResult
        pMessageBoxTitle = vTitle
        pMessageBoxText = vMessage
        pMessageBoxType = vType
        svalue = FrmMessageBox.ShowDialog()
        Return svalue
    End Function

    '--------------------------------------------------------------------------------------------------------------------------------------------
    ' SOUND
    '--------------------------------------------------------------------------------------------------------------------------------------------
    Public Function PlaySound(vSound As String, vLoop As Boolean, vMessage As String, vTitle As String, vBox As Boolean) As String
        PlaySound = "OK"
        Try
            If pAudioPlaying = False Then
                If vLoop = True Then
                    My.Computer.Audio.Play(vSound, AudioPlayMode.BackgroundLoop)
                    pAudioPlaying = True
                Else
                    My.Computer.Audio.Play(vSound, AudioPlayMode.WaitToComplete)
                End If

                If vBox = True Then
                    If ShowMessage(vMessage, "CRITICAL", vTitle) = CType("OK", MsgBoxResult) Then
                        StopSound()
                    End If
                End If
            End If
        Catch ex As Exception
            LogSessionEntry("ERROR", "PlayError: " + ex.Message, "", "PlayError", "PROGRAM")
            PlaySound = ex.Message
        End Try

    End Function

    Public Sub StopSound()
        Try
            My.Computer.Audio.Stop()
            pAudioPlaying = False
        Catch ex As Exception
            LogSessionEntry("ERROR", "PlayError: " + ex.Message, "", "PlayError", "PROGRAM")
        End Try
    End Sub

    Public Sub WaitSeconds(vS As Double, vEvents As Boolean, vShowMessages As Boolean)
        Dim i As Integer
        Dim j As Double

        i = 0
        j = 2 * vS

        If vShowMessages = True Then
            LogSessionEntry("FULL", "Wait for " + Format(vS) + " seconds.", "", "WaitSeconds", "PROGRAM")
        End If

        Try
            While i < j 'OPEN
                Thread.Sleep(500)
                If vEvents = True Then
                    My.Application.DoEvents()
                End If
                i += 1
            End While

            If vShowMessages = True Then
                LogSessionEntry("FULL", "Wait completed.", "", "WaitSeconds", "PROGRAM")
            End If

            Application.DoEvents()
        Catch ex As Exception
            LogSessionEntry("ERROR", "WaitSeconds: " + ex.Message, "", "WaitSeconds", "PROGRAM")
        End Try
    End Sub

    '------------------------------------------------------------------------------------------------
    ' HELPER FUNCTIONS HIGH LEVEL
    '------------------------------------------------------------------------------------------------
    Public Function CheckIfRunning(vProcessName As String) As String
        Try
            Dim listProc() As System.Diagnostics.Process
            listProc = System.Diagnostics.Process.GetProcessesByName(vProcessName)
            If listProc.Length > 0 Then
                ' Process is running
                CheckIfRunning = "OK"
            Else
                ' Process is not running
                CheckIfRunning = "Not running"
            End If

        Catch ex As Exception
            CheckIfRunning = "CheckIfRunning: " + ex.Message
            LogSessionEntry("ERROR", "CheckIfRunning: " + ex.Message, "", "CheckIfRunning", "PROGRAM")
        End Try

    End Function

    Public Function StartProcess(vProcessName As String) As String
        StartProcess = "OK"
        Try
            'first check if already running
            If CheckIfRunning(vProcessName) = "Not running" Then
                'start the process
                Process.Start(vProcessName)
            End If

        Catch ex As Exception
            StartProcess = "StartProcess: " + ex.Message
            LogSessionEntry("ERROR", "StartProcess: " + ex.Message, "", "StartProcess", "PROGRAM")
        End Try
    End Function

    Public Function GetNthIndex(searchString As String, charToFind As Char, n As Integer) As Integer
        Dim charIndexPair = searchString.Select(Function(c, i) New With {.Character = c, .Index = i}) _
                                    .Where(Function(x) x.Character = charToFind) _
                                    .ElementAtOrDefault(n - 1)
        Return If(charIndexPair IsNot Nothing, charIndexPair.Index, -1)
    End Function


    '--------------------------------------------------------------------------------------------------------------------------------------------
    ' LOGGING
    '--------------------------------------------------------------------------------------------------------------------------------------------

    Public Sub LogInitializeArray()
        Try
            pOldProgramTime(0) = CDate("01/01/1977")
            pOldProgramTime(1) = CDate("01/01/1977")
            pOldProgramTime(2) = CDate("01/01/1977")
            pOldProgramTime(3) = CDate("01/01/1977")
            pOldProgramTime(4) = CDate("01/01/1977")

            pOldWeatherTime(0) = CDate("01/01/1977")
            pOldWeatherTime(1) = CDate("01/01/1977")
            pOldWeatherTime(2) = CDate("01/01/1977")
            pOldWeatherTime(3) = CDate("01/01/1977")
            pOldWeatherTime(4) = CDate("01/01/1977")

            pOldWeatherStatusTime(0) = CDate("01/01/1977")
            pOldWeatherStatusTime(1) = CDate("01/01/1977")
            pOldWeatherStatusTime(2) = CDate("01/01/1977")
            pOldWeatherStatusTime(3) = CDate("01/01/1977")
            pOldWeatherStatusTime(4) = CDate("01/01/1977")

            pOldSequenceTime(0) = CDate("01/01/1977")
            pOldSequenceTime(1) = CDate("01/01/1977")
            pOldSequenceTime(2) = CDate("01/01/1977")
            pOldSequenceTime(3) = CDate("01/01/1977")
            pOldSequenceTime(4) = CDate("01/01/1977")

            pOldTSXTime(0) = CDate("01/01/1977")
            pOldTSXTime(1) = CDate("01/01/1977")
            pOldTSXTime(2) = CDate("01/01/1977")
            pOldTSXTime(3) = CDate("01/01/1977")
            pOldTSXTime(4) = CDate("01/01/1977")

            pOldInternetTime(0) = CDate("01/01/1977")
            pOldInternetTime(1) = CDate("01/01/1977")
            pOldInternetTime(2) = CDate("01/01/1977")
            pOldInternetTime(3) = CDate("01/01/1977")
            pOldInternetTime(4) = CDate("01/01/1977")

            pOldRoofTime(0) = CDate("01/01/1977")
            pOldRoofTime(1) = CDate("01/01/1977")
            pOldRoofTime(2) = CDate("01/01/1977")
            pOldRoofTime(3) = CDate("01/01/1977")
            pOldRoofTime(4) = CDate("01/01/1977")

            pOldCoverTime(0) = CDate("01/01/1977")
            pOldCoverTime(1) = CDate("01/01/1977")
            pOldCoverTime(2) = CDate("01/01/1977")
            pOldCoverTime(3) = CDate("01/01/1977")
            pOldCoverTime(4) = CDate("01/01/1977")

            pOldSwitchTime(0) = CDate("01/01/1977")
            pOldSwitchTime(1) = CDate("01/01/1977")
            pOldSwitchTime(2) = CDate("01/01/1977")
            pOldSwitchTime(3) = CDate("01/01/1977")
            pOldSwitchTime(4) = CDate("01/01/1977")

            pOldMountTime(0) = CDate("01/01/1977")
            pOldMountTime(1) = CDate("01/01/1977")
            pOldMountTime(2) = CDate("01/01/1977")
            pOldMountTime(3) = CDate("01/01/1977")
            pOldMountTime(4) = CDate("01/01/1977")

            pOldUPSTime(0) = CDate("01/01/1977")
            pOldUPSTime(1) = CDate("01/01/1977")
            pOldUPSTime(2) = CDate("01/01/1977")
            pOldUPSTime(3) = CDate("01/01/1977")
            pOldUPSTime(4) = CDate("01/01/1977")

            pOldSmartTime(0) = CDate("01/01/1977")
            pOldSmartTime(1) = CDate("01/01/1977")
            pOldSmartTime(2) = CDate("01/01/1977")
            pOldSmartTime(3) = CDate("01/01/1977")
            pOldSmartTime(4) = CDate("01/01/1977")

            pOldProgramMessage(0) = ""
            pOldProgramMessage(1) = ""
            pOldProgramMessage(2) = ""
            pOldProgramMessage(3) = ""
            pOldProgramMessage(4) = ""

            pOldWeatherMessage(0) = ""
            pOldWeatherMessage(1) = ""
            pOldWeatherMessage(2) = ""
            pOldWeatherMessage(3) = ""
            pOldWeatherMessage(4) = ""

            'pOldWeatherStatusMessage(0) = ""
            'pOldWeatherStatusMessage(1) = ""
            'pOldWeatherStatusMessage(2) = ""
            'pOldWeatherStatusMessage(3) = ""
            'pOldWeatherStatusMessage(4) = ""

            pOldSequenceMessage(0) = ""
            pOldSequenceMessage(1) = ""
            pOldSequenceMessage(2) = ""
            pOldSequenceMessage(3) = ""
            pOldSequenceMessage(4) = ""

            pOldTSXMessage(0) = ""
            pOldTSXMessage(2) = ""
            pOldTSXMessage(2) = ""
            pOldTSXMessage(3) = ""
            pOldTSXMessage(4) = ""

            pOldInternetMessage(0) = ""
            pOldInternetMessage(1) = ""
            pOldInternetMessage(2) = ""
            pOldInternetMessage(3) = ""
            pOldInternetMessage(4) = ""

            pOldRoofMessage(0) = ""
            pOldRoofMessage(1) = ""
            pOldRoofMessage(2) = ""
            pOldRoofMessage(3) = ""
            pOldRoofMessage(4) = ""

            pOldCoverMessage(0) = ""
            pOldCoverMessage(1) = ""
            pOldCoverMessage(2) = ""
            pOldCoverMessage(3) = ""
            pOldCoverMessage(4) = ""

            pOldSwitchMessage(0) = ""
            pOldSwitchMessage(1) = ""
            pOldSwitchMessage(2) = ""
            pOldSwitchMessage(3) = ""
            pOldSwitchMessage(4) = ""

            pOldMountMessage(0) = ""
            pOldMountMessage(1) = ""
            pOldMountMessage(2) = ""
            pOldMountMessage(3) = ""
            pOldMountMessage(4) = ""

            pOldUPSMessage(0) = ""
            pOldUPSMessage(1) = ""
            pOldUPSMessage(2) = ""
            pOldUPSMessage(3) = ""
            pOldUPSMessage(4) = ""

            pOldSmartMessage(0) = ""
            pOldSmartMessage(1) = ""
            pOldSmartMessage(2) = ""
            pOldSmartMessage(3) = ""
            pOldSmartMessage(4) = ""

        Catch ex As Exception
            LogSessionEntry("ERROR", "LogInitializeArray: " + ex.Message, "", "LogInitializeArray", "PROGRAM")
        End Try
    End Sub

    Public Function LogSessionEntry(vLogType As String, vLogText As String, vLogTextVar As String, vLogProcedure As String, vLogArea As String) As String
        Dim aTime As DateTime = DateTime.UtcNow
        Dim aFormat As String = "dd/MM/yyyy HH:mm:ss"
        Dim LogMessage As Integer

        'ERROR: will sound alarm and send to Telegram error group
        'ESSENTIAL: will NOT sound alarm but is send to error box
        'BRIEF : message in textbox and send to Telegram
        'FULL : message only in textbox
        'DEBUG: lowest level, will generate huge logs!
        LogSessionEntry = "OK"
        Try
            'if not WEATHER_DATA, needs to be logged separatly
            If vLogArea = "WEATHER_DATA" Then
                'log the WEATHER_DATA
                'write logfile
                'vFile = My.Computer.FileSystem.OpenTextFileWriter(pWeatherLogFile, True)
                pWeatherFile.WriteLine(aTime.ToString(aFormat) + " " + vLogText.Replace(vbCrLf, ""))
                pWeatherFile.Flush()
                'vFile.Close()
            Else
                If vLogArea = "PROGRAM" Then
                    Select Case vLogType
                        Case "ERROR"
                            If pOldProgramMessage(0) <> vLogText Then
                                LogMessage = 1
                                pOldProgramMessage(0) = vLogText
                                pOldProgramTime(0) = DateTime.UtcNow
                            ElseIf pOldProgramMessage(0) = vLogText Then
                                If DateDiff(DateInterval.Second, pOldProgramTime(0), DateTime.UtcNow) > My.Settings.sAlarmRepeat Then
                                    LogMessage = 1
                                    pOldProgramTime(0) = DateTime.UtcNow
                                End If
                            End If
                        Case "ESSENTIAL"
                            If pOldProgramMessage(1) <> vLogText Then
                                LogMessage = 1
                                pOldProgramMessage(1) = vLogText
                                pOldProgramTime(1) = DateTime.UtcNow
                            ElseIf pOldProgramMessage(1) = vLogText Then
                                If DateDiff(DateInterval.Second, pOldProgramTime(1), DateTime.UtcNow) > My.Settings.sAlarmRepeat Then
                                    LogMessage = 1
                                    pOldProgramTime(1) = DateTime.UtcNow
                                End If
                            End If
                        Case "BRIEF"
                            If pOldProgramMessage(2) <> vLogText Then
                                LogMessage = 1
                                pOldProgramMessage(2) = vLogText
                                pOldProgramTime(2) = DateTime.UtcNow
                            ElseIf pOldProgramMessage(2) = vLogText Then
                                If DateDiff(DateInterval.Second, pOldProgramTime(2), DateTime.UtcNow) > My.Settings.sAlarmRepeat Then
                                    LogMessage = 1
                                    pOldProgramTime(2) = DateTime.UtcNow
                                End If
                            End If
                        Case "FULL"
                            If pOldProgramMessage(3) <> vLogText Then
                                LogMessage = 1
                                pOldProgramMessage(3) = vLogText
                                pOldProgramTime(3) = DateTime.UtcNow
                            ElseIf pOldProgramMessage(3) = vLogText Then
                                If DateDiff(DateInterval.Second, pOldProgramTime(3), DateTime.UtcNow) > My.Settings.sAlarmRepeat Then
                                    LogMessage = 1
                                    pOldProgramTime(3) = DateTime.UtcNow
                                End If
                            End If
                        Case "DEBUG"
                            If pOldProgramMessage(4) <> vLogText Then
                                LogMessage = 1
                                pOldProgramMessage(4) = vLogText
                                pOldProgramTime(4) = DateTime.UtcNow
                            ElseIf pOldProgramMessage(4) = vLogText Then
                                If DateDiff(DateInterval.Second, pOldProgramTime(4), DateTime.UtcNow) > My.Settings.sAlarmRepeat Then
                                    LogMessage = 1
                                    pOldProgramTime(4) = DateTime.UtcNow
                                End If
                            End If
                    End Select
                ElseIf vLogArea = "INTERNET" Then
                    Select Case vLogType
                        Case "ERROR"
                            If pOldInternetMessage(0) <> vLogText Then
                                LogMessage = 1
                                pOldInternetMessage(0) = vLogText
                                pOldInternetTime(0) = DateTime.UtcNow
                            ElseIf pOldInternetMessage(0) = vLogText Then
                                If DateDiff(DateInterval.Second, pOldInternetTime(0), DateTime.UtcNow) > My.Settings.sAlarmRepeat Then
                                    LogMessage = 1
                                    pOldInternetTime(0) = DateTime.UtcNow
                                End If
                            End If
                        Case "ESSENTIAL"
                            If pOldInternetMessage(1) <> vLogText Then
                                LogMessage = 1
                                pOldInternetMessage(1) = vLogText
                                pOldInternetTime(1) = DateTime.UtcNow
                            ElseIf pOldInternetMessage(1) = vLogText Then
                                If DateDiff(DateInterval.Second, pOldInternetTime(1), DateTime.UtcNow) > My.Settings.sAlarmRepeat Then
                                    LogMessage = 1
                                    pOldInternetTime(1) = DateTime.UtcNow
                                End If
                            End If
                        Case "BRIEF"
                            If pOldInternetMessage(2) <> vLogText Then
                                LogMessage = 1
                                pOldInternetMessage(2) = vLogText
                                pOldInternetTime(2) = DateTime.UtcNow
                            ElseIf pOldInternetMessage(2) = vLogText Then
                                If DateDiff(DateInterval.Second, pOldInternetTime(2), DateTime.UtcNow) > My.Settings.sAlarmRepeat Then
                                    LogMessage = 1
                                    pOldInternetTime(2) = DateTime.UtcNow
                                End If
                            End If
                        Case "FULL"
                            If pOldInternetMessage(3) <> vLogText Then
                                LogMessage = 1
                                pOldInternetMessage(3) = vLogText
                                pOldInternetTime(3) = DateTime.UtcNow
                            ElseIf pOldInternetMessage(3) = vLogText Then
                                If DateDiff(DateInterval.Second, pOldInternetTime(3), DateTime.UtcNow) > My.Settings.sAlarmRepeat Then
                                    LogMessage = 1
                                    pOldInternetTime(3) = DateTime.UtcNow
                                End If
                            End If
                        Case "DEBUG"
                            If pOldInternetMessage(4) <> vLogText Then
                                LogMessage = 1
                                pOldInternetMessage(4) = vLogText
                                pOldInternetTime(4) = DateTime.UtcNow
                            ElseIf pOldInternetMessage(4) = vLogText Then
                                If DateDiff(DateInterval.Second, pOldInternetTime(4), DateTime.UtcNow) > My.Settings.sAlarmRepeat Then
                                    LogMessage = 1
                                    pOldInternetTime(4) = DateTime.UtcNow
                                End If
                            End If
                    End Select
                ElseIf vLogArea = "ROOF" Then
                    Select Case vLogType
                        Case "ERROR"
                            If pOldRoofMessage(0) <> vLogText Then
                                LogMessage = 1
                                pOldRoofMessage(0) = vLogText
                                pOldRoofTime(0) = DateTime.UtcNow
                            ElseIf pOldRoofMessage(0) = vLogText Then
                                If DateDiff(DateInterval.Second, pOldRoofTime(0), DateTime.UtcNow) > My.Settings.sAlarmRepeat Then
                                    LogMessage = 1
                                    pOldRoofTime(0) = DateTime.UtcNow
                                End If
                            End If
                            pOldProgramTimeSMART = DateTime.UtcNow
                        Case "ESSENTIAL"
                            If pOldRoofMessage(1) <> vLogText Then
                                LogMessage = 1
                                pOldRoofMessage(1) = vLogText
                                pOldRoofTime(1) = DateTime.UtcNow
                            ElseIf pOldRoofMessage(1) = vLogText Then
                                If DateDiff(DateInterval.Second, pOldRoofTime(1), DateTime.UtcNow) > My.Settings.sAlarmRepeat Then
                                    LogMessage = 1
                                    pOldRoofTime(1) = DateTime.UtcNow
                                End If
                            End If
                            pOldProgramTimeSMART = DateTime.UtcNow
                        Case "BRIEF"
                            If pOldRoofMessage(2) <> vLogText Then
                                LogMessage = 1
                                pOldRoofMessage(2) = vLogText
                                pOldRoofTime(2) = DateTime.UtcNow
                            ElseIf pOldRoofMessage(2) = vLogText Then
                                If DateDiff(DateInterval.Second, pOldRoofTime(2), DateTime.UtcNow) > My.Settings.sAlarmRepeat Then
                                    LogMessage = 1
                                    pOldRoofTime(2) = DateTime.UtcNow
                                End If
                            End If
                            pOldProgramTimeSMART = DateTime.UtcNow
                        Case "FULL"
                            If pOldRoofMessage(3) <> vLogText Then
                                LogMessage = 1
                                pOldRoofMessage(3) = vLogText
                                pOldRoofTime(3) = DateTime.UtcNow
                            ElseIf pOldRoofMessage(3) = vLogText Then
                                If DateDiff(DateInterval.Second, pOldRoofTime(3), DateTime.UtcNow) > My.Settings.sAlarmRepeat Then
                                    LogMessage = 1
                                    pOldRoofTime(3) = DateTime.UtcNow
                                End If
                            End If
                            pOldProgramTimeSMART = DateTime.UtcNow
                        Case "DEBUG"
                            If pOldRoofMessage(4) <> vLogText Then
                                LogMessage = 1
                                pOldRoofMessage(4) = vLogText
                                pOldRoofTime(4) = DateTime.UtcNow
                            ElseIf pOldRoofMessage(4) = vLogText Then
                                If DateDiff(DateInterval.Second, pOldRoofTime(4), DateTime.UtcNow) > My.Settings.sAlarmRepeat Then
                                    LogMessage = 1
                                    pOldRoofTime(4) = DateTime.UtcNow
                                End If
                            End If
                    End Select
                ElseIf vLogArea = "COVER" Then
                    Select Case vLogType
                        Case "ERROR"
                            If pOldCoverMessage(0) <> vLogText Then
                                LogMessage = 1
                                pOldCoverMessage(0) = vLogText
                                pOldCoverTime(0) = DateTime.UtcNow
                            ElseIf pOldCoverMessage(0) = vLogText Then
                                If DateDiff(DateInterval.Second, pOldCoverTime(0), DateTime.UtcNow) > My.Settings.sAlarmRepeat Then
                                    LogMessage = 1
                                    pOldCoverTime(0) = DateTime.UtcNow
                                End If
                            End If
                            pOldProgramTimeSMART = DateTime.UtcNow
                        Case "ESSENTIAL"
                            If pOldCoverMessage(1) <> vLogText Then
                                LogMessage = 1
                                pOldCoverMessage(1) = vLogText
                                pOldCoverTime(1) = DateTime.UtcNow
                            ElseIf pOldCoverMessage(1) = vLogText Then
                                If DateDiff(DateInterval.Second, pOldCoverTime(1), DateTime.UtcNow) > My.Settings.sAlarmRepeat Then
                                    LogMessage = 1
                                    pOldCoverTime(1) = DateTime.UtcNow
                                End If
                            End If
                            pOldProgramTimeSMART = DateTime.UtcNow
                        Case "BRIEF"
                            If pOldCoverMessage(2) <> vLogText Then
                                LogMessage = 1
                                pOldCoverMessage(2) = vLogText
                                pOldCoverTime(2) = DateTime.UtcNow
                            ElseIf pOldCoverMessage(2) = vLogText Then
                                If DateDiff(DateInterval.Second, pOldCoverTime(2), DateTime.UtcNow) > My.Settings.sAlarmRepeat Then
                                    LogMessage = 1
                                    pOldCoverTime(2) = DateTime.UtcNow
                                End If
                            End If
                            pOldProgramTimeSMART = DateTime.UtcNow
                        Case "FULL"
                            If pOldCoverMessage(3) <> vLogText Then
                                LogMessage = 1
                                pOldCoverMessage(3) = vLogText
                                pOldCoverTime(3) = DateTime.UtcNow
                            ElseIf pOldCoverMessage(3) = vLogText Then
                                If DateDiff(DateInterval.Second, pOldCoverTime(3), DateTime.UtcNow) > My.Settings.sAlarmRepeat Then
                                    LogMessage = 1
                                    pOldCoverTime(3) = DateTime.UtcNow
                                End If
                            End If
                            pOldProgramTimeSMART = DateTime.UtcNow
                        Case "DEBUG"
                            If pOldCoverMessage(4) <> vLogText Then
                                LogMessage = 1
                                pOldCoverMessage(4) = vLogText
                                pOldCoverTime(4) = DateTime.UtcNow
                            ElseIf pOldCoverMessage(4) = vLogText Then
                                If DateDiff(DateInterval.Second, pOldCoverTime(4), DateTime.UtcNow) > My.Settings.sAlarmRepeat Then
                                    LogMessage = 1
                                    pOldCoverTime(4) = DateTime.UtcNow
                                End If
                            End If
                    End Select
                ElseIf vLogArea = "SWITCH" Then
                    Select Case vLogType
                        Case "ERROR"
                            If pOldSwitchMessage(0) <> vLogText Then
                                LogMessage = 1
                                pOldSwitchMessage(0) = vLogText
                                pOldSwitchTime(0) = DateTime.UtcNow
                            ElseIf pOldSwitchMessage(0) = vLogText Then
                                If DateDiff(DateInterval.Second, pOldSwitchTime(0), DateTime.UtcNow) > My.Settings.sAlarmRepeat Then
                                    LogMessage = 1
                                    pOldSwitchTime(0) = DateTime.UtcNow
                                End If
                            End If
                            pOldProgramTimeSMART = DateTime.UtcNow
                        Case "ESSENTIAL"
                            If pOldSwitchMessage(1) <> vLogText Then
                                LogMessage = 1
                                pOldSwitchMessage(1) = vLogText
                                pOldSwitchTime(1) = DateTime.UtcNow
                            ElseIf pOldSwitchMessage(1) = vLogText Then
                                If DateDiff(DateInterval.Second, pOldSwitchTime(1), DateTime.UtcNow) > My.Settings.sAlarmRepeat Then
                                    LogMessage = 1
                                    pOldSwitchTime(1) = DateTime.UtcNow
                                End If
                            End If
                            pOldProgramTimeSMART = DateTime.UtcNow
                        Case "BRIEF"
                            If pOldSwitchMessage(2) <> vLogText Then
                                LogMessage = 1
                                pOldSwitchMessage(2) = vLogText
                                pOldSwitchTime(2) = DateTime.UtcNow
                            ElseIf pOldSwitchMessage(2) = vLogText Then
                                If DateDiff(DateInterval.Second, pOldSwitchTime(2), DateTime.UtcNow) > My.Settings.sAlarmRepeat Then
                                    LogMessage = 1
                                    pOldSwitchTime(2) = DateTime.UtcNow
                                End If
                            End If
                            pOldProgramTimeSMART = DateTime.UtcNow
                        Case "FULL"
                            If pOldSwitchMessage(3) <> vLogText Then
                                LogMessage = 1
                                pOldSwitchMessage(3) = vLogText
                                pOldSwitchTime(3) = DateTime.UtcNow
                            ElseIf pOldSwitchMessage(3) = vLogText Then
                                If DateDiff(DateInterval.Second, pOldSwitchTime(3), DateTime.UtcNow) > My.Settings.sAlarmRepeat Then
                                    LogMessage = 1
                                    pOldSwitchTime(3) = DateTime.UtcNow
                                End If
                            End If
                        Case "DEBUG"
                            If pOldSwitchMessage(4) <> vLogText Then
                                LogMessage = 1
                                pOldSwitchMessage(4) = vLogText
                                pOldSwitchTime(4) = DateTime.UtcNow
                            ElseIf pOldSwitchMessage(4) = vLogText Then
                                If DateDiff(DateInterval.Second, pOldSwitchTime(4), DateTime.UtcNow) > My.Settings.sAlarmRepeat Then
                                    LogMessage = 1
                                    pOldSwitchTime(4) = DateTime.UtcNow
                                End If
                            End If
                    End Select
                ElseIf vLogArea = "TSX" Then
                    Select Case vLogType
                        Case "ERROR"
                            LogMessage = 1
                            pOldTSXMessage(0) = vLogText
                            pOldTSXTime(0) = DateTime.UtcNow
                            pOldProgramTimeSMART = DateTime.UtcNow
                        Case "ESSENTIAL"
                            LogMessage = 1
                            pOldTSXMessage(1) = vLogText
                            pOldTSXTime(1) = DateTime.UtcNow
                            pOldProgramTimeSMART = DateTime.UtcNow
                        Case "BRIEF"
                            If pOldTSXMessage(2) <> vLogText Then
                                LogMessage = 1
                                pOldTSXMessage(2) = vLogText
                                pOldTSXTime(2) = DateTime.UtcNow
                            ElseIf pOldTSXMessage(2) = vLogText Then
                                If DateDiff(DateInterval.Second, pOldTSXTime(2), DateTime.UtcNow) > My.Settings.sAlarmRepeat Then
                                    LogMessage = 1
                                    pOldTSXTime(2) = DateTime.UtcNow
                                End If
                            End If
                            pOldProgramTimeSMART = DateTime.UtcNow
                        Case "FULL"
                            If pOldTSXMessage(3) <> vLogText Then
                                LogMessage = 1
                                pOldTSXMessage(3) = vLogText
                                pOldTSXTime(3) = DateTime.UtcNow
                            ElseIf pOldTSXMessage(3) = vLogText Then
                                If DateDiff(DateInterval.Second, pOldTSXTime(3), DateTime.UtcNow) > My.Settings.sAlarmRepeat Then
                                    LogMessage = 1
                                    pOldTSXTime(3) = DateTime.UtcNow
                                End If
                            End If
                            pOldProgramTimeSMART = DateTime.UtcNow
                        Case "DEBUG"
                            If pOldTSXMessage(4) <> vLogText Then
                                LogMessage = 1
                                pOldTSXMessage(4) = vLogText
                                pOldTSXTime(4) = DateTime.UtcNow
                            ElseIf pOldTSXMessage(4) = vLogText Then
                                If DateDiff(DateInterval.Second, pOldTSXTime(4), DateTime.UtcNow) > My.Settings.sAlarmRepeat Then
                                    LogMessage = 1
                                    pOldTSXTime(4) = DateTime.UtcNow
                                End If
                            End If
                    End Select
                ElseIf vLogArea = "UPS" Then
                    Select Case vLogType
                        Case "ERROR"
                            If pOldUPSMessage(0) <> vLogText Then
                                LogMessage = 1
                                pOldUPSMessage(0) = vLogText
                                pOldUPSTime(0) = DateTime.UtcNow
                            ElseIf pOldUPSMessage(0) = vLogText Then
                                If DateDiff(DateInterval.Second, pOldUPSTime(0), DateTime.UtcNow) > My.Settings.sAlarmRepeat Then
                                    LogMessage = 1
                                    pOldUPSTime(0) = DateTime.UtcNow
                                End If
                            End If
                        Case "ESSENTIAL"
                            If pOldUPSMessage(1) <> vLogText Then
                                LogMessage = 1
                                pOldUPSMessage(1) = vLogText
                                pOldUPSTime(1) = DateTime.UtcNow
                            ElseIf pOldUPSMessage(1) = vLogText Then
                                If DateDiff(DateInterval.Second, pOldUPSTime(1), DateTime.UtcNow) > My.Settings.sAlarmRepeat Then
                                    LogMessage = 1
                                    pOldUPSTime(1) = DateTime.UtcNow
                                End If
                            End If
                        Case "BRIEF"
                            If pOldUPSMessage(2) <> vLogText Then
                                LogMessage = 1
                                pOldUPSMessage(2) = vLogText
                                pOldUPSTime(2) = DateTime.UtcNow
                            ElseIf pOldUPSMessage(2) = vLogText Then
                                If DateDiff(DateInterval.Second, pOldUPSTime(2), DateTime.UtcNow) > My.Settings.sAlarmRepeat Then
                                    LogMessage = 1
                                    pOldUPSTime(2) = DateTime.UtcNow
                                End If
                            End If
                        Case "FULL"
                            If pOldUPSMessage(3) <> vLogText Then
                                LogMessage = 1
                                pOldUPSMessage(3) = vLogText
                                pOldUPSTime(3) = DateTime.UtcNow
                            ElseIf pOldUPSMessage(3) = vLogText Then
                                If DateDiff(DateInterval.Second, pOldUPSTime(3), DateTime.UtcNow) > My.Settings.sAlarmRepeat Then
                                    LogMessage = 1
                                    pOldUPSTime(3) = DateTime.UtcNow
                                End If
                            End If
                        Case "DEBUG"
                            If pOldUPSMessage(4) <> vLogText Then
                                LogMessage = 1
                                pOldUPSMessage(4) = vLogText
                                pOldUPSTime(4) = DateTime.UtcNow
                            ElseIf pOldUPSMessage(4) = vLogText Then
                                If DateDiff(DateInterval.Second, pOldUPSTime(4), DateTime.UtcNow) > My.Settings.sAlarmRepeat Then
                                    LogMessage = 1
                                    pOldUPSTime(4) = DateTime.UtcNow
                                End If
                            End If
                    End Select
                ElseIf vLogArea = "MOUNT" Then
                    Select Case vLogType
                        Case "ERROR"
                            If pOldMountMessage(0) <> vLogText Then
                                LogMessage = 1
                                pOldMountMessage(0) = vLogText
                                pOldMountTime(0) = DateTime.UtcNow
                            ElseIf pOldMountMessage(0) = vLogText Then
                                If DateDiff(DateInterval.Second, pOldMountTime(0), DateTime.UtcNow) > My.Settings.sAlarmRepeat Then
                                    LogMessage = 1
                                    pOldMountTime(0) = DateTime.UtcNow
                                End If
                            End If
                            pOldProgramTimeSMART = DateTime.UtcNow
                        Case "ESSENTIAL"
                            If pOldMountMessage(1) <> vLogText Then
                                LogMessage = 1
                                pOldMountMessage(1) = vLogText
                                pOldMountTime(1) = DateTime.UtcNow
                            ElseIf pOldMountMessage(1) = vLogText Then
                                If DateDiff(DateInterval.Second, pOldMountTime(1), DateTime.UtcNow) > My.Settings.sAlarmRepeat Then
                                    LogMessage = 1
                                    pOldMountTime(1) = DateTime.UtcNow
                                End If
                            End If
                            pOldProgramTimeSMART = DateTime.UtcNow
                        Case "BRIEF"
                            If pOldMountMessage(2) <> vLogText Then
                                LogMessage = 1
                                pOldMountMessage(2) = vLogText
                                pOldMountTime(2) = DateTime.UtcNow
                            ElseIf pOldMountMessage(2) = vLogText Then
                                If DateDiff(DateInterval.Second, pOldMountTime(2), DateTime.UtcNow) > My.Settings.sAlarmRepeat Then
                                    LogMessage = 1
                                    pOldMountTime(2) = DateTime.UtcNow
                                End If
                            End If
                            pOldProgramTimeSMART = DateTime.UtcNow
                        Case "FULL"
                            If pOldMountMessage(3) <> vLogText Then
                                LogMessage = 1
                                pOldMountMessage(3) = vLogText
                                pOldMountTime(3) = DateTime.UtcNow
                            ElseIf pOldMountMessage(3) = vLogText Then
                                If DateDiff(DateInterval.Second, pOldMountTime(3), DateTime.UtcNow) > My.Settings.sAlarmRepeat Then
                                    LogMessage = 1
                                    pOldMountTime(3) = DateTime.UtcNow
                                End If
                            End If
                            pOldProgramTimeSMART = DateTime.UtcNow
                        Case "DEBUG"
                            If pOldMountMessage(4) <> vLogText Then
                                LogMessage = 1
                                pOldMountMessage(4) = vLogText
                                pOldMountTime(4) = DateTime.UtcNow
                            ElseIf pOldMountMessage(4) = vLogText Then
                                If DateDiff(DateInterval.Second, pOldMountTime(4), DateTime.UtcNow) > My.Settings.sAlarmRepeat Then
                                    LogMessage = 1
                                    pOldMountTime(4) = DateTime.UtcNow
                                End If
                            End If
                            pOldProgramTimeSMART = DateTime.UtcNow
                    End Select
                ElseIf vLogArea = "WEATHER" Then
                    Select Case vLogType
                        Case "ERROR"
                            If pOldWeatherMessage(0) <> vLogText Then
                                LogMessage = 1
                                pOldWeatherMessage(0) = vLogText
                                pOldWeatherTime(0) = DateTime.UtcNow
                            ElseIf pOldWeatherMessage(0) = vLogText Then
                                If DateDiff(DateInterval.Second, pOldWeatherTime(0), DateTime.UtcNow) > My.Settings.sAlarmRepeat Then
                                    LogMessage = 1
                                    pOldWeatherTime(0) = DateTime.UtcNow
                                End If
                            End If
                            pOldProgramTimeSMART = DateTime.UtcNow
                        Case "ESSENTIAL"
                            If pOldWeatherMessage(1) <> vLogText Then
                                LogMessage = 1
                                pOldWeatherMessage(1) = vLogText
                                pOldWeatherTime(1) = DateTime.UtcNow
                            ElseIf pOldWeatherMessage(1) = vLogText Then
                                If DateDiff(DateInterval.Second, pOldWeatherTime(1), DateTime.UtcNow) > My.Settings.sAlarmRepeat Then
                                    LogMessage = 1
                                    pOldWeatherTime(1) = DateTime.UtcNow
                                End If
                            End If
                            pOldProgramTimeSMART = DateTime.UtcNow
                        Case "BRIEF"
                            If pOldWeatherMessage(2) <> vLogText Then
                                LogMessage = 1
                                pOldWeatherMessage(2) = vLogText
                                pOldWeatherTime(2) = DateTime.UtcNow
                            ElseIf pOldWeatherMessage(2) = vLogText Then
                                If DateDiff(DateInterval.Second, pOldWeatherTime(2), DateTime.UtcNow) > My.Settings.sAlarmRepeat Then
                                    LogMessage = 1
                                    pOldWeatherTime(2) = DateTime.UtcNow
                                End If
                            End If
                            pOldProgramTimeSMART = DateTime.UtcNow
                        Case "FULL"
                            If pOldWeatherMessage(3) <> vLogText Then
                                LogMessage = 1
                                pOldWeatherMessage(3) = vLogText
                                pOldWeatherTime(3) = DateTime.UtcNow
                            ElseIf pOldWeatherMessage(3) = vLogText Then
                                If DateDiff(DateInterval.Second, pOldWeatherTime(3), DateTime.UtcNow) > My.Settings.sAlarmRepeat Then
                                    LogMessage = 1
                                    pOldWeatherTime(3) = DateTime.UtcNow
                                End If
                            End If
                            pOldProgramTimeSMART = DateTime.UtcNow
                        Case "DEBUG"
                            If pOldWeatherMessage(4) <> vLogText Then
                                LogMessage = 1
                                pOldWeatherMessage(4) = vLogText
                                pOldWeatherTime(4) = DateTime.UtcNow
                            ElseIf pOldWeatherMessage(4) = vLogText Then
                                If DateDiff(DateInterval.Second, pOldWeatherTime(4), DateTime.UtcNow) > My.Settings.sAlarmRepeat Then
                                    LogMessage = 1
                                    pOldWeatherTime(4) = DateTime.UtcNow
                                End If
                            End If
                    End Select
                ElseIf vLogArea = "WEATHER_STATUS" Then
                    Select Case vLogType
                        Case "ERROR"
                            If pOldWeatherStatusMessage(0) <> vLogText Then
                                LogMessage = 1
                                pOldWeatherStatusMessage(0) = vLogText
                                pOldWeatherStatusTime(0) = DateTime.UtcNow
                            End If
                            pOldProgramTimeSMART = DateTime.UtcNow
                        Case "ESSENTIAL"
                            If pOldWeatherStatusMessage(1) <> vLogText Then
                                LogMessage = 1
                                pOldWeatherStatusMessage(1) = vLogText
                                pOldWeatherStatusTime(1) = DateTime.UtcNow
                            End If
                            pOldProgramTimeSMART = DateTime.UtcNow
                        Case "BRIEF"
                            If pOldWeatherStatusMessage(2) <> vLogText Then
                                LogMessage = 1
                                pOldWeatherStatusMessage(2) = vLogText
                                pOldWeatherStatusTime(2) = DateTime.UtcNow
                            End If
                            pOldProgramTimeSMART = DateTime.UtcNow
                        Case "FULL"
                            If pOldWeatherStatusMessage(3) <> vLogText Then
                                LogMessage = 1
                                pOldWeatherStatusMessage(3) = vLogText
                                pOldWeatherStatusTime(3) = DateTime.UtcNow
                            End If
                            pOldProgramTimeSMART = DateTime.UtcNow
                        Case "DEBUG"
                            If pOldWeatherStatusMessage(4) <> vLogText Then
                                LogMessage = 1
                                pOldWeatherStatusMessage(4) = vLogText
                                pOldWeatherStatusTime(4) = DateTime.UtcNow
                            End If
                    End Select
                ElseIf vLogArea = "SEQUENCE" Then
                    Select Case vLogType
                        Case "ERROR"
                            If pOldSequenceMessage(0) <> vLogText Then
                                LogMessage = 1
                                pOldSequenceMessage(0) = vLogText
                                pOldSequenceTime(0) = DateTime.UtcNow
                            ElseIf pOldSequenceMessage(0) = vLogText Then
                                If DateDiff(DateInterval.Second, pOldSequenceTime(0), DateTime.UtcNow) > My.Settings.sAlarmRepeat Then
                                    LogMessage = 1
                                    pOldSequenceTime(0) = DateTime.UtcNow
                                End If
                            End If
                            pOldProgramTimeSMART = DateTime.UtcNow
                        Case "ESSENTIAL"
                            If pOldSequenceMessage(1) <> vLogText Then
                                LogMessage = 1
                                pOldSequenceMessage(1) = vLogText
                                pOldSequenceTime(1) = DateTime.UtcNow
                            End If
                            pOldProgramTimeSMART = DateTime.UtcNow
                        Case "BRIEF"
                            If pOldSequenceMessage(2) <> vLogText Then
                                LogMessage = 1
                                pOldSequenceMessage(2) = vLogText
                                pOldSequenceTime(2) = DateTime.UtcNow
                            End If
                            pOldProgramTimeSMART = DateTime.UtcNow
                        Case "FULL"
                            If pOldSequenceMessage(3) <> vLogText Then
                                LogMessage = 1
                                pOldSequenceMessage(3) = vLogText
                                pOldSequenceTime(3) = DateTime.UtcNow
                            End If
                            pOldProgramTimeSMART = DateTime.UtcNow
                        Case "DEBUG"
                            If pOldSequenceMessage(4) <> vLogText Then
                                LogMessage = 1
                                pOldSequenceMessage(4) = vLogText
                                pOldSequenceTime(4) = DateTime.UtcNow
                            ElseIf pOldSequenceMessage(4) = vLogText Then
                                If DateDiff(DateInterval.Second, pOldSequenceTime(4), DateTime.UtcNow) > My.Settings.sAlarmRepeat Then
                                    LogMessage = 1
                                    pOldSequenceTime(4) = DateTime.UtcNow
                                End If
                            End If
                    End Select
                    '----------------------------------------------------------
                ElseIf vLogArea = "SMART" Then
                    Select Case vLogType
                        Case "ERROR"
                            If pOldSmartMessage(0) <> vLogText Then
                                LogMessage = 1
                                pOldSmartMessage(0) = vLogText
                                pOldSmartTime(0) = DateTime.UtcNow
                            ElseIf pOldSmartMessage(0) = vLogText Then
                                If DateDiff(DateInterval.Second, pOldSmartTime(0), DateTime.UtcNow) > My.Settings.sAlarmRepeat Then
                                    LogMessage = 1
                                    pOldSmartTime(0) = DateTime.UtcNow
                                End If
                            End If
                        Case "ESSENTIAL"
                            If pOldSmartMessage(1) <> vLogText Then
                                LogMessage = 1
                                pOldSmartMessage(1) = vLogText
                                pOldSmartTime(1) = DateTime.UtcNow
                            ElseIf pOldSmartMessage(1) = vLogText Then
                                If DateDiff(DateInterval.Second, pOldSmartTime(1), DateTime.UtcNow) > My.Settings.sAlarmRepeat Then
                                    LogMessage = 1
                                    pOldSmartTime(1) = DateTime.UtcNow
                                End If
                            End If
                        Case "BRIEF"
                            If pOldSmartMessage(2) <> vLogText Then
                                LogMessage = 1
                                pOldSmartMessage(2) = vLogText
                                pOldSmartTime(2) = DateTime.UtcNow
                            ElseIf pOldSmartMessage(2) = vLogText Then
                                If DateDiff(DateInterval.Second, pOldSmartTime(2), DateTime.UtcNow) > My.Settings.sAlarmRepeat Then
                                    LogMessage = 1
                                    pOldSmartTime(2) = DateTime.UtcNow
                                End If
                            End If
                        Case "FULL"
                            If pOldSmartMessage(3) <> vLogText Then
                                LogMessage = 1
                                pOldSmartMessage(3) = vLogText
                                pOldSmartTime(3) = DateTime.UtcNow
                            ElseIf pOldSmartMessage(3) = vLogText Then
                                If DateDiff(DateInterval.Second, pOldSmartTime(3), DateTime.UtcNow) > My.Settings.sAlarmRepeat Then
                                    LogMessage = 1
                                    pOldSmartTime(3) = DateTime.UtcNow
                                End If
                            End If
                        Case "DEBUG"
                            If pOldSmartMessage(4) <> vLogText Then
                                LogMessage = 1
                                pOldSmartMessage(4) = vLogText
                                pOldSmartTime(4) = DateTime.UtcNow
                            ElseIf pOldSmartMessage(4) = vLogText Then
                                If DateDiff(DateInterval.Second, pOldSmartTime(4), DateTime.UtcNow) > My.Settings.sAlarmRepeat Then
                                    LogMessage = 1
                                    pOldSmartTime(4) = DateTime.UtcNow
                                End If
                            End If
                    End Select
                End If



                '--------------------------
                'only log unique messages
                '--------------------------
                If LogMessage = 1 Then
                    '--------------------------
                    'UPDATE SMART TIMESTAMP
                    '--------------------------
                    If vLogType <> "DEBUG" Then
                        pOldSequenceTimeSMART = Date.UtcNow
                    End If

                    '--------------------------
                    'TELEGRAM
                    '--------------------------
                    If vLogType = "ERROR" Then
                        'always Telegram error data as wakeup mechanism and keep repeating the error
                        TelegramMessage(Format(DateTime.UtcNow, "HH:mm:ss") + ": " + vLogText, My.Settings.sTelegramErrorID)
                    End If

                    '--------------------------
                    'TELEGRAM
                    '--------------------------
                    If vLogType = "BRIEF" And (My.Settings.sTelegramLoggingType = "BRIEF" Or My.Settings.sTelegramLoggingType = "FULL" Or vLogType = "ERROR" Or vLogType = "ESSENTIAL") Then
                        'if logtype is brief and full / brief logging enabled
                        TelegramMessage(Format(DateTime.UtcNow, "HH:mm:ss") + ": " + vLogText + vLogTextVar, My.Settings.sTelegramSessionID)
                    ElseIf (vLogType = "FULL" And My.Settings.sTelegramLoggingType = "FULL" Or vLogType = "ERROR" Or vLogType = "ESSENTIAL") Then
                        'logtype is full and full logging enabled
                        TelegramMessage(Format(DateTime.UtcNow, "HH:mm:ss") + ": " + vLogText + vLogTextVar, My.Settings.sTelegramSessionID)
                    End If

                    '--------------------------
                    ' TEXTBOXES ON FRMMAIN
                    '--------------------------
                    FrmMain.RTXLog.SelectionStart = FrmMain.RTXLog.TextLength
                    FrmMain.RTXLog.SelectionLength = 0

                    If vLogType = "ERROR" Then
                        FrmMain.RTXLog.SelectionColor = ColorTranslator.FromHtml("#d63031") 'red
                        FrmMain.RTXLog.AppendText(Format(DateTime.UtcNow(), "HH:mm:ss") + ": " + vLogText + vLogTextVar + vbCrLf)
                        FrmMain.RTXLog.SelectionColor = Color.Black
                        FrmMain.RTXLog.ScrollToCaret()
                        FrmMain.RTXLog.Refresh()
                    ElseIf vLogType = "ESSENTIAL" Then
                        FrmMain.RTXLog.SelectionColor = ColorTranslator.FromHtml("#4cd137") 'green
                        FrmMain.RTXLog.AppendText(Format(DateTime.UtcNow(), "HH:mm:ss") + ": " + vLogText + vLogTextVar + vbCrLf)
                        FrmMain.RTXLog.SelectionColor = Color.Black
                        FrmMain.RTXLog.ScrollToCaret()
                        FrmMain.RTXLog.Refresh()
                        pLastLogLineTimeStamp = DateTime.UtcNow()
                    ElseIf vLogType = "BRIEF" And (My.Settings.sLogType = "BRIEF" Or My.Settings.sLogType = "FULL" Or My.Settings.sLogType = "DEBUG") Then
                        FrmMain.RTXLog.SelectionColor = Color.Black
                        FrmMain.RTXLog.AppendText(Format(DateTime.UtcNow(), "HH:mm:ss") + ": " + vLogText + vLogTextVar + vbCrLf)
                        FrmMain.RTXLog.SelectionColor = Color.Black
                        FrmMain.StatusStrip.Items(0).Text = vLogText + vLogTextVar
                        FrmMain.RTXLog.ScrollToCaret()
                        FrmMain.RTXLog.Refresh()
                        pLastLogLineTimeStamp = DateTime.UtcNow()
                    ElseIf vLogType = "FULL" And (My.Settings.sLogType = "FULL" Or My.Settings.sLogType = "DEBUG") Then
                        FrmMain.RTXLog.SelectionColor = Color.DarkGray
                        FrmMain.RTXLog.AppendText(Format(DateTime.UtcNow(), "HH:mm:ss") + ": " + vLogText + vLogTextVar + vbCrLf)
                        FrmMain.StatusStrip.Items(0).Text = vLogText + vLogTextVar
                        FrmMain.RTXLog.ScrollToCaret()
                        FrmMain.RTXLog.Refresh()
                        pLastLogLineTimeStamp = DateTime.UtcNow()
                    ElseIf vLogType = "DEBUG" And My.Settings.sLogType = "DEBUG" Then
                        FrmMain.RTXLog.SelectionColor = Color.LightGray
                        FrmMain.RTXLog.AppendText(Format(DateTime.UtcNow(), "HH:mm:ss") + ": " + vLogText + vLogTextVar + vbCrLf)
                        FrmMain.RTXLog.SelectionColor = Color.Black
                        FrmMain.RTXLog.ScrollToCaret()
                        FrmMain.RTXLog.Refresh()
                    End If

                    'when ERROR, add a line to the textbox
                    If vLogType = "ERROR" Then
                        FrmMain.RTXErrors.AppendText(Format(DateTime.UtcNow(), "HH:mm:ss") + ": " + vLogText + vLogTextVar + vbCrLf)
                        FrmMain.RTXLog.ScrollToCaret()
                        FrmMain.RTXLog.Refresh()
                    End If

                    '--------------------------
                    'LOGFILES
                    '--------------------------

                    If (Not System.IO.Directory.Exists(My.Settings.sLogLocation)) Then
                        System.IO.Directory.CreateDirectory(My.Settings.sLogLocation)
                    End If

                    Dim WriteLine As Boolean
                    WriteLine = False

                    'vLogFile = My.Settings.sLogLocation + "\RoboticObsLog_" + DateTime.utcNow.ToString("yyyyMMdd") + ".txt"

                    If My.Settings.sLogType = "DEBUG" Then
                        WriteLine = True
                    ElseIf My.Settings.sLogType = "FULL" And (vLogType = "FULL" Or vLogType = "BRIEF" Or vLogType = "ESSENTIAL" Or vLogType = "ERROR") Then
                        WriteLine = True
                    ElseIf My.Settings.sLogType = "BRIEF" And (My.Settings.sLogType = "FULL" Or vLogType = "BRIEF" Or vLogType = "ESSENTIAL" Or vLogType = "ERROR") Then
                        WriteLine = True
                    End If

                    'write all lines in logfile                   
                    Dim Text As String
                    Text = aTime.ToString(aFormat) + ": " + vLogText + vLogTextVar + " (" + vLogType + ": " + vLogProcedure + ")"
                    pLogFileFull.WriteLine(aTime.ToString(aFormat) + ": " + vLogText + vLogTextVar + " (" + vLogType + ": " + vLogProcedure + ")")
                    pLogFileFull.Flush()

                    'write normal line in logfile                   
                    If WriteLine = True Then
                        Text = aTime.ToString(aFormat) + ": " + vLogText + vLogTextVar + " (" + vLogType + ": " + vLogProcedure + ")"
                        pLogFile.WriteLine(aTime.ToString(aFormat) + ": " + vLogText + vLogTextVar + " (" + vLogType + ": " + vLogProcedure + ")")
                        pLogFile.Flush()
                        FrmMain.RTXLog.SaveFile(pLogFileNameRTF)
                    End If
                End If
            End If

            '--------------------------
            'SOUND
            '--------------------------
            If My.Settings.sAlarmPlay = True And vLogType = "ERROR" And LogMessage = 1 Then
                PlaySound(My.Settings.sAlarmSound, True, "", "", False)
            End If

        Catch ex As Exception
            LogSessionEntry = "LogSessionEntry: " + ex.Message
        End Try
    End Function


    '--------------------------------------------------------------------------------------------------------------------------------------------
    ' ZIP FILES 
    '--------------------------------------------------------------------------------------------------------------------------------------------


    Public Function ZipAndCopyFolders() As String
        Dim i As Integer
        Dim ZipFileName As String

        ZipAndCopyFolders = "OK"
        Try
            'run over all folders in the image directory
            'if zip file does not yet exists, create zipfile and copy to the Synology folder

            FrmMain.Cursor = Cursors.WaitCursor

            If (Not System.IO.Directory.Exists(My.Settings.sSynologyPath)) Then
                System.IO.Directory.CreateDirectory(My.Settings.sSynologyPath)
            End If

            'check if the most recent folder is already zipped, if so delete
            Dim path As String = My.Settings.sCCDImagePath
            Dim lastdir As String

            lastdir = ""

            If Directory.Exists(path) Then
                Dim dir As New DirectoryInfo(path)
                Dim directories As DirectoryInfo() = dir.GetDirectories().OrderByDescending(Function(p) p.CreationTime).ToArray()
                For Each directory As DirectoryInfo In directories
                    lastdir = directory.Name
                    Exit For
                Next
            End If

            If My.Computer.FileSystem.FileExists(My.Settings.sSynologyPath + "\" + lastdir + ".zip") = True Then
                My.Computer.FileSystem.DeleteFile(My.Settings.sSynologyPath + "\" + lastdir + ".zip")
            End If

            'delete plate_solve files
            Dim dinfo As New DirectoryInfo(My.Settings.sCCDImagePath)
            For Each myFile In dinfo.GetFiles("PLATESOLVE_*", SearchOption.AllDirectories)
                My.Computer.FileSystem.DeleteFile(myFile.FullName)
            Next


            For Each Dir As String In Directory.GetDirectories(My.Settings.sCCDImagePath)
                ZipFileName = Dir.Substring(Dir.LastIndexOf("\") + 1, Dir.Length - Dir.LastIndexOf("\") - 1) + ".zip"

                If My.Computer.FileSystem.FileExists(My.Settings.sSynologyPath + "\" + ZipFileName) = False Then

                    'does not exist yet, make zipfile and copy
                    'CreateZipFile(Dir, Dir + ".zip", My.Settings.sSynologyPath + "\" + ZipFileName)

                    Bgw = New BackgroundWorker With {
                        .WorkerReportsProgress = True,
                        .WorkerSupportsCancellation = True
                    }

                    If Bgw.IsBusy = False Then
                        LogSessionEntry("BRIEF", "Zipping " + ZipFileName, "", "ZipAndCopyFolders", "PROGRAM")

                        PZipfolder = Dir
                        PZipFile = Dir + ".zip"
                        PZipSynologyLocation = My.Settings.sSynologyPath + "\" + ZipFileName

                        Bgw.RunWorkerAsync(1)

                    Else
                        LogSessionEntry("BRIEF", "Zipping cannot be done: background worker still busy!", "", "ZipAndCopyFolders", "PROGRAM")
                        ZipAndCopyFolders = "NOK"
                        Exit Function
                    End If

                    While Bgw.IsBusy = True
                        Application.DoEvents()
                    End While

                    i += 1
                    My.Application.DoEvents()
                End If
            Next

            If i > 0 Then
                LogSessionEntry("BRIEF", Format(i) + " ZIP-files created and copied !", "", "ZipAndCopyFolders", "PROGRAM")
            Else
                LogSessionEntry("BRIEF", Format(i) + " ZIP-files created and copied !", "", "ZipAndCopyFolders", "PROGRAM")
            End If
            FrmMain.Cursor = Cursors.Default

        Catch ex As Exception
            LogSessionEntry("ERROR", "ZipAndCopyFolders: " + ex.Message, "", "ZipAndCopyFolders", "PROGRAM")
            ZipAndCopyFolders = ex.Message
        End Try

    End Function



    Private Function Bgw_DoWork(ByVal sender As Object, ByVal e As DoWorkEventArgs) As String Handles Bgw.DoWork

        Bgw_DoWork = "OK"
        Try
            'if file exists delete
            If My.Computer.FileSystem.FileExists(PZipFile) Then
                My.Computer.FileSystem.DeleteFile(PZipFile)
            End If

            LogSessionEntry("FULL", "Creating ZIP-file: " + PZipFile, "", "CreateZipFile", "PROGRAM")
            'My.Application.DoEvents()
            'create zipfile
            ZipFile.CreateFromDirectory(PZipfolder, PZipFile, CompressionLevel.Optimal, True)
            My.Application.DoEvents()

            'copy to Synology folder
            My.Computer.FileSystem.CopyFile(PZipFile, PZipSynologyLocation)
            My.Application.DoEvents()

            'cleanup the file
            My.Computer.FileSystem.DeleteFile(PZipFile)

        Catch ex As Exception
            LogSessionEntry("ERROR", "CreateZipFile: " + ex.Message, "", "CreateZipFile", "PROGRAM")
            Bgw_DoWork = ex.Message
        End Try

    End Function


    '--------------------------------------------------------------------------------------------------------------------------------------------
    ' ZIP FILES 
    '--------------------------------------------------------------------------------------------------------------------------------------------


    Public Function Zip7AndCopyFolders(vRedoLast As Boolean) As String
        Dim i As Integer
        Dim ZipFileName As String

        Zip7AndCopyFolders = "OK"
        Try
            'run over all folders in the image directory
            'if zip file does not yet exists, create zipfile and copy to the Synology folder

            FrmMain.Cursor = Cursors.WaitCursor

            If (Not System.IO.Directory.Exists(My.Settings.sSynologyPath)) Then
                System.IO.Directory.CreateDirectory(My.Settings.sSynologyPath)
            End If

            'check if the most recent folder is already zipped, if so delete
            Dim path As String = My.Settings.sCCDImagePath
            Dim lastdir As String

            lastdir = ""

            If Directory.Exists(path) Then
                Dim dir As New DirectoryInfo(path)
                Dim directories As DirectoryInfo() = dir.GetDirectories().OrderByDescending(Function(p) p.CreationTime).ToArray()

                For Each directory As DirectoryInfo In directories
                    lastdir = directory.Name
                    Exit For
                Next
            End If


            If My.Computer.FileSystem.FileExists(My.Settings.sSynologyPath + "\" + lastdir + ".zip") = True And vRedoLast = True Then
                My.Computer.FileSystem.DeleteFile(My.Settings.sSynologyPath + "\" + lastdir + ".zip")
            End If

            For Each Dir As String In Directory.GetDirectories(My.Settings.sCCDImagePath)
                ZipFileName = Dir.Substring(Dir.LastIndexOf("\") + 1, Dir.Length - Dir.LastIndexOf("\") - 1) + ".zip"

                If My.Computer.FileSystem.FileExists(My.Settings.sSynologyPath + "\" + ZipFileName) = False Then
                    Bgw7 = New BackgroundWorker With {
                        .WorkerReportsProgress = True,
                        .WorkerSupportsCancellation = True
                    }

                    If Bgw7.IsBusy = False Then
                        LogSessionEntry("BRIEF", "Zipping " + ZipFileName, "", "Zip7AndCopyFolders", "PROGRAM")

                        PZipfolder = Dir
                        PZipFile = Dir + ".zip"
                        PZipSynologyLocation = My.Settings.sSynologyPath + "\" + ZipFileName

                        Bgw7.RunWorkerAsync(1)

                    Else
                        LogSessionEntry("BRIEF", "Zipping cannot be done: background worker still busy!", "", "Zip7AndCopyFolders", "PROGRAM")
                        Zip7AndCopyFolders = "NOK"
                        Exit Function
                    End If

                    While Bgw7.IsBusy = True
                        Application.DoEvents()
                    End While

                    i += 1
                    My.Application.DoEvents()
                End If
            Next

            If i > 0 Then
                LogSessionEntry("BRIEF", Format(i) + " ZIP-files created and copied !", "", "Zip7AndCopyFolders", "PROGRAM")
            Else
                LogSessionEntry("BRIEF", Format(i) + " ZIP-files created and copied !", "", "Zip7AndCopyFolders", "PROGRAM")
            End If

            FrmMain.Cursor = Cursors.Default


        Catch ex As Exception
            LogSessionEntry("ERROR", "ZipAndCopyFolders: " + ex.Message, "", "Zip7AndCopyFolders", "PROGRAM")
            Zip7AndCopyFolders = ex.Message
        End Try
    End Function



    Private Function Bgw7_DoWork(ByVal sender As Object, ByVal e As DoWorkEventArgs) As String Handles Bgw7.DoWork
        Bgw7_DoWork = "OK"
        Try
            'if file exists delete
            If My.Computer.FileSystem.FileExists(PZipFile) Then
                My.Computer.FileSystem.DeleteFile(PZipFile)
            End If

            LogSessionEntry("FULL", "Creating ZIP-file: " + PZipFile, "", "Create7ZipFile", "PROGRAM")

            '"C:\Program Files\7-Zip\7z.exe" a -tzip D:\RoboObsImages\20230706.zip D:\RoboObsImages\20230706

            Dim myProcess As Process = Process.Start(My.Settings.s7ZipLocation, " a -tzip " + PZipFile + " " + PZipfolder)
            myProcess.WaitForExit()
            myProcess.Close()

            My.Application.DoEvents()

            'copy to Synology folder
            My.Computer.FileSystem.CopyFile(PZipFile, PZipSynologyLocation)
            My.Application.DoEvents()

            'cleanup the file
            My.Computer.FileSystem.DeleteFile(PZipFile)

        Catch ex As Exception
            LogSessionEntry("ERROR", "CreateZipFile: " + ex.Message, "", "Create7ZipFile", "PROGRAM")
            Bgw7_DoWork = ex.Message
        End Try

    End Function

    Public Function RemoveIllegalFileNameChars(input As String, Optional replacement As String = "") As String
        Dim regexSearch = New String(Path.GetInvalidFileNameChars()) & New String(Path.GetInvalidPathChars()) + "()  "
        Dim r = New Regex(String.Format("[{0}]", Regex.Escape(regexSearch)))
        Return r.Replace(input, replacement)
    End Function

End Module
