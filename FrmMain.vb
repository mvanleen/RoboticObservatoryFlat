﻿Imports System.ComponentModel
Imports System.Data.Entity
Imports Newtonsoft.Json.Linq

Public Class FrmMain

    Public pMonitorStatusColor As Boolean
    Private Sub FrmMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim returnvalue As String
        Try
            Dim time As DateTime = DateTime.UtcNow()
            Dim format As String = "yyyyMMdd_HHmm"

            'set the splitcontainer with
            SplitContainer.SplitterDistance = 40

            BtnStopRun.Enabled = False
            BtnStartRun.Enabled = True

            'will reset status fields        
            pRunStatus = "NOT RUNNING"
            pAbort = False
            pManualAbort = False
            pEquipmentSafetyStatusError = False

            pIsEquipmentInitializing = False
            pIsSafetyCheckRunning = False
            pIsSequenceRunning = False
            pIsActionRunning = False
            pIsMountParking = False
            pSmartError = False
            pSmartTimeStamp = CDate("01/01/0001")
            pWeatherSwitchDelay = DateTime.UtcNow

            pLastKnownUPSConnected = DateTime.UtcNow
            pLastKnownInternetConnected = DateTime.UtcNow
            pLastCalculateEventTimesLowIntensity = DateTime.UtcNow

            'reset logging timing fields
            LogInitializeArray()

            'initialize Cover - Roof - Mount: prevent collisions
            pRoofShutterStatus = "UNKNOWN"
            pCoverStatus = 4 'UNKNOWN
            pStructMount.AtPark = False 'at startup we assume mount is not parked

            'check if loglocation exists
            If (Not System.IO.Directory.Exists(My.Settings.sLogLocation)) Then
                System.IO.Directory.CreateDirectory(My.Settings.sLogLocation)
            End If

            'set the logfiles           
            pWeatherLogFileName = My.Settings.sLogLocation + time.ToString(format) + "_Weather_Data.log"
            pLogFileName = My.Settings.sLogLocation + "RoboticObsLog_" + DateTime.UtcNow.ToString("yyyyMMdd_HHmm") + ".txt"
            pLogFileNameRTF = My.Settings.sLogLocation + "RoboticObsLog_" + DateTime.UtcNow.ToString("yyyyMMdd_HHmm") + ".rtf"
            pLogFileNameFull = My.Settings.sLogLocation + "RoboticObsLog_" + DateTime.UtcNow.ToString("yyyyMMdd_HHmm") + "_Full.txt"

            pLogFile = My.Computer.FileSystem.OpenTextFileWriter(pLogFileName, True)
            pLogFileFull = My.Computer.FileSystem.OpenTextFileWriter(pLogFileNameFull, True)
            pWeatherFile = My.Computer.FileSystem.OpenTextFileWriter(pWeatherLogFileName, True)

            StatusStrip.Items(0).Text = ""

            'enable Telegram
            returnvalue = ConnectTelegram(My.Settings.sTelegramBOT)

            'starting software
            LogSessionEntry("ESSENTIAL", My.Settings.sObservatoryName + ": starting RoboticObs-software.", "", "FrmMain_Load", "PROGRAM")

            'start The Sky X
            returnvalue = ConnectTheSkyXApplication()
            pTheSkyXEquipmentConnected = False

            'set debug mode
            ChkSimulatorMode.Checked = My.Settings.sSimulatorMode

            'set safety check           
            ChkDisableSafetyCheck.Checked = My.Settings.sDisableSafetyCheck = True

            'set defaults for hardware
            If My.Settings.sRoofDevice = "" Then
                My.Settings.sRoofDevice = "NONE"
                My.Settings.Save()
            End If

            If My.Settings.sMountDevice = "" Then
                My.Settings.sMountDevice = "ScopeSim.Telescope"
                My.Settings.Save()
            End If

            If My.Settings.sCoverMethod = "" Then
                My.Settings.sCoverMethod = "NONE"
                My.Settings.Save()
            End If

            If My.Settings.sSwitchDevice = "" Then
                My.Settings.sSwitchDevice = "SwitchSim.Switch"
                My.Settings.Save()
            End If

            'enable roof 
            If My.Settings.sRoofDevice = "NONE" Then
                LblRoof.Text = "NO ROOF"
                LblRoof.BackColor = Color.Transparent
                ChkAutoStart.Enabled = False
            Else
                returnvalue = RoofConnect()
                If returnvalue = "OK" Then
                    returnvalue = CheckRoof()
                Else
                    LblRoof.Text = "ERROR ROOF"
                    LblRoof.BackColor = Color.Purple
                End If
                ChkAutoStart.Enabled = True
            End If

            'enable Switch
            returnvalue = SwitchConnect()
            If returnvalue = "OK" Then
                returnvalue = CheckSwitch()
            Else
                LblSwitch.Text = "LOST CON"
                LblSwitch.BackColor = Color.Purple
            End If

            returnvalue = CalculateEventTimesLowIntensity()

            TimerCheckCycle.Enabled = True
            TimerRoof.Enabled = True
            TimerCover.Enabled = True
            TimerSwitch.Enabled = True
            TimerWeather.Enabled = True
            TimerMount.Enabled = True
            TimerCCD.Enabled = True
            TimerDisaster.Enabled = True

            TimerCheckCycle_Tick(Nothing, Nothing)
            TimerRoof_Tick(Nothing, Nothing)
            TimerCover_Tick(Nothing, Nothing)
            TimerSwitch_Tick(Nothing, Nothing)
            TimerWeather_Tick(Nothing, Nothing)
            TimerMount_Tick(Nothing, Nothing)
            TimerCCD_Tick(Nothing, Nothing)

            LblLastFocusDateTime.Text = ""
            LblLastFocusTemperature.Text = ""

            'download the HADS-file
            Me.Cursor = Cursors.WaitCursor
            returnvalue = ReadHADSFile()
            If returnvalue <> "OK" Then
                LogSessionEntry("ERROR", "HADS-file is not read correctly.", "", "FrmMain_Load", "SEQUENCE")
            Else
                LogSessionEntry("BRIEF", "HADS-file is read correctly.", "", "FrmMain_Load", "SEQUENCE")
            End If
            Me.Cursor = Cursors.Default

        Catch ex As Exception
            ShowMessage("FrmMain_Load: " + ex.Message, "CRITICAL", "Error!")
        End Try

    End Sub

    Private Sub TimerCheckCycle_Tick(sender As Object, e As EventArgs) Handles TimerCheckCycle.Tick
        Try
            'check CheckCycle
            LogSessionEntry("DEBUG", "  TimerCheckCycle_Tick Start", "", "TimerCheckCycle_Tick", "SEQUENCE")
            CheckCycle()
            LogSessionEntry("DEBUG", "  TimerCheckCycle_Tick Stop", "", "TimerCheckCycle_Tick", "SEQUENCE")
        Catch ex As Exception
            ShowMessage("TimerCheckCycle_Tick: " + ex.Message, "CRITICAL", "Error!")
        End Try
    End Sub


    '------------------------------------------
    ' SPECIFIC CODE
    '------------------------------------------
    Private Sub CheckCycle()
        Dim returnvalue As String
        Dim i As Integer
        Dim diff As Long

        Try
            i = 0

            'check Internet
            returnvalue = CheckTimeoutInternet()
            If returnvalue <> "OK" And pRunStatus <> "ABORTED" Then
                'abort any sequence running
                pStartRun = False 'do not restart run
                pIsActionRunning = False
                pRunStatus = "ABORTING"
                returnvalue = PauseRun("Aborting run due to Internet error...", "",
                                        "Aborting run due to Internet error: CheckCycle: ", "",
                                        "Internet error: equipment paused, run aborted.", "", "ABORTING", "ABORTED")
                If returnvalue <> "OK" Then
                    Exit Sub
                End If
            End If

            returnvalue = CheckTimeoutUPS()
            If returnvalue <> "OK" And pRunStatus <> "ABORTED" Then
                'abort any sequence running
                pStartRun = False 'do not restart run
                pIsActionRunning = False
                pRunStatus = "ABORTING"
                returnvalue = PauseRun("Aborting run due to UPS error...", "",
                                        "Aborting run due to UPS error: CheckCycle: ", "",
                                        "UPS error: equipment paused, run aborted.", "", "ABORTING", "ABORTED")
                If returnvalue <> "OK" Then
                    Exit Sub
                End If
            End If

            diff = DateDiff(DateInterval.Second, pLastCalculateEventTimesLowIntensity, DateTime.UtcNow)

            If diff > 600 Then
                returnvalue = CalculateEventTimesLowIntensity()
                If returnvalue <> "OK" And pRunStatus <> "ABORTED" Then
                    'abort any sequence running
                    pStartRun = False 'do not restart run
                    pIsActionRunning = False
                    pRunStatus = "ABORTING"
                    returnvalue = PauseRun("Aborting run due to event times low error...", "",
                                        "Aborting run due to event times low error: CheckCycle: ", "",
                                        "Event times low error: equipment paused, run aborted.", "", "ABORTING", "ABORTED")
                    If returnvalue <> "OK" Then
                        Exit Sub
                    End If
                End If
                pLastCalculateEventTimesLowIntensity = DateTime.UtcNow()
            End If

            returnvalue = CalculateEventTimesHighIntensity()
            If returnvalue <> "OK" And pRunStatus <> "ABORTED" Then
                'abort any sequence running
                pStartRun = False 'do not restart run
                pIsActionRunning = False
                pRunStatus = "ABORTING"
                returnvalue = PauseRun("Aborting run due to event times high error...", "",
                                        "Aborting run due to event times high error: CheckCycle: ", "",
                                        "Event times high error: equipment paused, run aborted.", "", "ABORTING", "ABORTED")
                If returnvalue <> "OK" Then
                    Exit Sub
                End If
            End If

            'check if TSX is running
            returnvalue = CheckIsRunningTheSkyX()
            If returnvalue <> "OK" And pRunStatus <> "ABORTED" Then
                'abort any sequence running
                pStartRun = False 'do not restart run
                pIsActionRunning = False
                pRunStatus = "ABORTING"
                returnvalue = PauseRun("Aborting run due to TSX error...", "",
                                        "Aborting run due to TSX error: CheckCycle: ", "",
                                        "The Sky X error: equipment paused, run aborted.", "", "ABORTING", "ABORTED")
                If returnvalue <> "OK" Then
                    Exit Sub
                End If
            End If

        Catch ex As Exception
            ShowMessage("CheckCycle: " + ex.Message, "CRITICAL", "Error!")
        End Try
    End Sub

    Private Sub TimerRoof_Tick(sender As Object, e As EventArgs) Handles TimerRoof.Tick
        Dim returnvalue As String

        Try
            'check roof
            returnvalue = CheckRoof()
            If returnvalue <> "OK" And pRunStatus <> "ABORTED" Then
                'abort any sequence running
                pStartRun = False 'do not restart run
                pIsActionRunning = False
                pRunStatus = "ABORTING"
                returnvalue = PauseRun("Aborting run due to roof error...", "",
                                        "Aborting run due to roof error: TimerRoof_Tick: ", "",
                                        "Roof error: equipment paused.", "", "ABORTING", "ABORTED")
                If returnvalue <> "OK" Then
                    Exit Sub
                End If
                LblRoof.Text = "RF ERROR"
                LblRoof.BackColor = Color.IndianRed
            End If
        Catch ex As Exception
            ShowMessage("TimerRoof_Tick: " + ex.Message, "CRITICAL", "Error!")
        End Try
    End Sub

    Private Sub TimerCover_Tick(sender As Object, e As EventArgs) Handles TimerCover.Tick
        Dim returnvalue As String

        Try
            If My.Settings.sCoverMethod = "NONE" Then
                LblCover.Text = "NO COVER"
                LblCover.BackColor = Color.Silver
            Else
                If My.Settings.sCoverDevice = "ASCOM.SnapCap.CoverCalibrator" Then
                    LblCover.Text = "SNAPCAP"
                Else
                    LblCover.Text = "COVER"
                End If

                If pCoverConnected = True Then
                    'check cover
                    returnvalue = CheckCover()
                    If returnvalue <> "OK" And pRunStatus <> "ABORTED" Then
                        'abort any sequence running
                        pStartRun = False 'do not restart run
                        pIsActionRunning = False
                        pRunStatus = "ABORTING"
                        returnvalue = PauseRun("Aborting run due to cover error...", "",
                                                "Aborting run due to cover error: TimerCover_Tick: ", "",
                                                "Cover error: equipment paused.", "", "ABORTING", "ABORTED")
                        If returnvalue <> "OK" Then
                            Exit Sub
                        End If
                        LblCover.Text = "ERROR"
                        LblCover.BackColor = Color.Silver
                    End If
                End If
            End If

        Catch ex As Exception
            ShowMessage("TimerCover_Tick: " + ex.Message, "CRITICAL", "Error!")
        End Try
    End Sub

    Private Sub TimerSwitch_Tick(sender As Object, e As EventArgs) Handles TimerSwitch.Tick
        Dim returnvalue As String

        Try
            If My.Settings.sSwitchDevice = "Dragonfly.Switch" Then
                LblSwitch.Text = "DRAGONFLY"
            Else
                LblSwitch.Text = "SWITCH"
            End If

            'check Switch
            returnvalue = CheckSwitch()
            If returnvalue <> "OK" Then
                LblSwitch.Text = "LOST CON"
                LblSwitch.BackColor = Color.Purple
            End If

        Catch ex As Exception
            ShowMessage("TimerSwitch_Tick: " + ex.Message, "CRITICAL", "Error!")
        End Try
    End Sub

    Private Sub TimerWeather_Tick(sender As Object, e As EventArgs) Handles TimerWeather.Tick
        Dim returnvalue As String
        Dim i As Integer

        i = 0
        Try
            LogSessionEntry("DEBUG", "  TmrWeather_Tick", "", "TimerWeather_Tick", "PROGRAM")
            returnvalue = MonitorWeather()
            If returnvalue <> "OK" Then
                i += 1
                Exit Sub
            End If

            'check AAG timeout
            returnvalue = CheckTimeoutAAG()
            If returnvalue <> "OK" And pRunStatus <> "ABORTED" Then
                'abort any sequence running
                pStartRun = False 'do not restart run
                pIsActionRunning = False
                pRunStatus = "ABORTING"
                returnvalue = PauseRun("Aborting run due to AAG error...", "",
                                       "Aborting run due to AAG error: CheckCycle: ", "",
                                       "AAG error: equipment paused, run aborted.", "", "ABORTING", "ABORTED")
                If returnvalue <> "OK" Then
                    Exit Sub
                End If
            End If

            'check AAG timeout: see if new values are being delivered
            returnvalue = CheckTimeoutAAGTimestamp()
            If returnvalue <> "OK" And pRunStatus <> "ABORTED" Then
                'abort any sequence running
                pStartRun = False 'do not restart run
                pIsActionRunning = False
                pRunStatus = "ABORTING"
                returnvalue = PauseRun("Aborting run due to AAG error...", "",
                                       "Aborting run due to AAG error: CheckCycle: ", "",
                                       "AAG error: equipment paused, run aborted.", "", "ABORTING", "ABORTED")
                If returnvalue <> "OK" Then
                    Exit Sub
                End If
            End If

        Catch ex As Exception
            ShowMessage("TimerWeather_Tick: " + ex.Message, "CRITICAL", "Error!")
        End Try
    End Sub

    Private Sub TimerMount_Tick(sender As Object, e As EventArgs) Handles TimerMount.Tick
        Dim returnvalue As String

        Try
            LogSessionEntry("DEBUG", "  TimerMount_Tick Start", "", "TimerMount_Tick", "SEQUENCE")

            'check mount
            If pMountConnected = True Then
                returnvalue = CheckMountStatus()
                If returnvalue <> "OK" And pRunStatus <> "ABORTED" Then
                    'abort any sequence running
                    pStartRun = False 'do not restart run
                    pIsActionRunning = False
                    pRunStatus = "ABORTING"
                    returnvalue = PauseRun("Aborting run  due to mount error...", "",
                                           "Aborting run due to mount error: TimerMount_Tick: ", "",
                                           "Mount error: equipment paused.", "", "ABORTING", "ABORTED")
                    If returnvalue <> "OK" Then
                        Exit Sub
                    End If
                    lblMountAlt.Text = ""
                    lblMountAz.Text = ""
                    LblMountRADEC.Text = ""
                    LblMountStatus.Text = ""
                    LblMountPierSide.Text = "ERROR"
                    LblMountStatus.BackColor = Color.Transparent
                End If



            Else
                lblMountAlt.Text = ""
                lblMountAz.Text = ""
                LblMountRADEC.Text = ""
                LblMountStatus.Text = ""
                LblMountPierSide.Text = ""
                LblMountStatus.BackColor = Color.Transparent
            End If

            LogSessionEntry("DEBUG", "  TimerMount_Tick Stop", "", "TimerMount_Tick", "SEQUENCE")

        Catch ex As Exception
            ShowMessage("TimerMount_Tick: " + ex.Message, "CRITICAL", "Error!")
        End Try
    End Sub

    Private Sub TimerCCD_Tick(sender As Object, e As EventArgs) Handles TimerCCD.Tick
        Dim returnvalue As String

        Try
            LogSessionEntry("DEBUG", "  TimerCCD_Tick Start", "", "TimerCCD_Tick", "SEQUENCE")

            If pTheSkyXEquipmentConnected = True Then
                'check CCD
                returnvalue = CheckTheSkyXCCD()
                If returnvalue <> "OK" And pRunStatus <> "ABORTED" Then
                    'abort any sequence running
                    pStartRun = False 'do not restart run
                    pIsActionRunning = False
                    pRunStatus = "ABORTING"
                    returnvalue = PauseRun("Aborting run due to CCD error...", "",
                                           "Aborting run due to CCD error: TimerCCD_Tick: ", "",
                                           "CCD error: equipment paused.", "", "ABORTING", "ABORTED")
                    If returnvalue <> "OK" Then
                        Exit Sub
                    End If

                    LblFocusserPosition.Text = ""
                    LblFocusserTemperature.Text = ""
                    LblCCDExposureStatus.Text = ""
                    LblCCDTemp.Text = ""
                    LblCCDFilter.Text = ""
                End If

                Dim ExposureStatus As String
                ExposureStatus = ""

                If pTheSkyXExposureStatus <> Nothing Then
                    If pTheSkyXExposureStatus.Length > 14 Then
                        If pTheSkyXExposureStatus.Substring(0, 14) = "Exposing Light" Then
                            ExposureStatus = pTheSkyXExposureStatus.ToString.Substring(16)
                            ExposureStatus = ExposureStatus.ToString.Substring(0, ExposureStatus.ToString.IndexOf(" "))
                            ExposureStatus = "Exposing: " + ExposureStatus + " s"
                        End If
                    Else
                        ExposureStatus = pTheSkyXExposureStatus
                    End If
                End If


                LblCCDExposureStatus.Text = ExposureStatus  'pTheSkyXExposureStatus 'Exposing Light ( x left )

                LblCCDTemp.Text = "Temp: " + Format(pTheSkyXCCDTemp, "##0.00") + " °C"
                'retrieve filtername
                If pTheSkyXCamera.FilterIndexZeroBased = 0 Then
                    LblCCDFilter.Text = My.Settings.sCCDFilter1
                ElseIf pTheSkyXCamera.FilterIndexZeroBased = 1 Then
                    LblCCDFilter.Text = My.Settings.sCCDFilter2
                ElseIf pTheSkyXCamera.FilterIndexZeroBased = 2 Then
                    LblCCDFilter.Text = My.Settings.sCCDFilter3
                ElseIf pTheSkyXCamera.FilterIndexZeroBased = 3 Then
                    LblCCDFilter.Text = My.Settings.sCCDFilter4
                ElseIf pTheSkyXCamera.FilterIndexZeroBased = 4 Then
                    LblCCDFilter.Text = My.Settings.sCCDFilter5
                End If

            Else
                LblFocusserPosition.Text = ""
                LblFocusserTemperature.Text = ""
                LblCCDExposureStatus.Text = ""
                LblCCDTemp.Text = ""
                LblCCDFilter.Text = ""
            End If

            LogSessionEntry("DEBUG", "  TimerCCD_Tick Stop", "", "TimerCCD_Tick", "SEQUENCE")

        Catch ex As Exception
            ShowMessage("TimerCCD_Tick: " + ex.Message, "CRITICAL", "Error!")
        End Try
    End Sub

    Private Sub TimerStartRun_Tick(sender As Object, e As EventArgs) Handles TimerStartRun.Tick
        Dim returnvalue As String
        'check every minute if the run can start or not
        Try
            'check run
            LogSessionEntry("DEBUG", "  TimerStartRun Start", "", "TimerStartRun_Tick", "SEQUENCE")

            returnvalue = CheckRun()
            If returnvalue <> "OK" Then
                Exit Sub
            End If
            LogSessionEntry("DEBUG", "  TimerStartRun Stop", "", "TimerStartRun_Tick", "SEQUENCE")

        Catch ex As Exception
            ShowMessage("TimerStartRun_Tick: " + ex.Message, "CRITICAL", "Error!")
        End Try

    End Sub

    Private Sub FrmMain_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
        Try

            My.Application.DoEvents()

            FrmSplash.StartPosition = FormStartPosition.CenterScreen
            My.Application.DoEvents()
            FrmSplash.Show()
            My.Application.DoEvents()

            WaitSeconds(3, True, False)
            FrmSplash.Close()

            Dim returnvalue As String

            'check equipment status
            returnvalue = CheckEquipmentStatus()

            If My.Settings.sAutoStart = True Then
                ChkAutoStart.Checked = True
                TimerStartRun.Enabled = True
                TimerStartRun_Tick(Nothing, Nothing)
            Else
                ChkAutoStart.Checked = False
                TimerStartRun.Enabled = False
            End If


            If My.Settings.sAutoStart = True Then
                ChkAutoStart.Checked = True
                TimerStartRun.Enabled = True

            Else
                ChkAutoStart.Checked = False
                TimerStartRun.Enabled = False
            End If
        Catch ex As Exception
            ShowMessage("FrmMain_Shown: " + ex.Message, "CRITICAL", "Error!")
        End Try
    End Sub

    Private Sub TimerDisaster_Tick(sender As Object, e As EventArgs) Handles TimerDisaster.Tick
        Dim returnvalue As String

        'keeps running every 15 seconds: always
        'only runs when checkbox DisabledSafetyCheck is not enabled
        'only runs when no sequence is running: in that case SMART-error handling is dealing with errors
        'runs day and night:
        ' - day: shutdown equipment
        ' - night: pause equipment

        Try
            LogSessionEntry("DEBUG", "  TimerDisaster_Tick Start", "", "TimerDisaster_Tick", "SEQUENCE")

            If ChkDisableSafetyCheck.Checked = False Then
                If pStartRun = True Or pSmartError = True Or pEquipmentStatus = "PAUSING" Or pEquipmentStatus = "TURNOFF" Or pContinueRunningCalibrationFrames = True Then
                    'do nothing: error handling done by SMART-error handling / or SMART error occurring: avoid both running at the same time
                    'or equipment is pausing
                Else
                    'nothing is running: deal with errors
                    If pStructEventTimes.SunSafetyStatus = "UNSAFE" Then
                        'day
                        'run the safety check but only when it is not running yet
                        If pIsSafetyCheckRunning = False Then
                            returnvalue = RunSafetyCheck("DAY")
                            If returnvalue <> "OK" Then
                                Exit Sub
                            End If
                        End If

                        If pEquipmentSafetyStatusError = True Then
                            LogSessionEntry("ERROR", "SAFETY CHECK DAYLIGHT: EQUIPMENT WAS NOT SAFE !! ", "Sun Alt " + Format(pStructEventTimes.SunAlt, "#0.00") + "°", "TimerDisaster_Tick", "SEQUENCE")
                        End If

                    Else
                        'night
                        If pIsSafetyCheckRunning = False Then
                            returnvalue = RunSafetyCheck("NIGHT")
                            If returnvalue <> "OK" Then
                                Exit Sub
                            End If
                        End If

                        If pEquipmentSafetyStatusError = True Then
                            LogSessionEntry("ERROR", "SAFETY CHECK NIGHT: NOTHING RUNNING!", "", "TimerDisaster_Tick", "SEQUENCE")
                        End If
                    End If
                End If
            Else
                'do nothing, all safety checks can be ignored
            End If

            LogSessionEntry("DEBUG", "  TimerDisaster_Tick Stop", "", "TimerDisaster_Tick", "SEQUENCE")

        Catch ex As Exception
            ShowMessage("TimerDisaster_Tick: " + ex.Message, "CRITICAL", "Error!")
        End Try
    End Sub

    Private Sub ChkDisableSafetyCheck_CheckedChanged(sender As Object, e As EventArgs) Handles ChkDisableSafetyCheck.CheckedChanged
        If ChkDisableSafetyCheck.Checked = True Then
            'If ShowMessage("Are you sure to disable the safety check ?", vbYesNo, "Safety check") = vbYes Then
            TimerDisaster.Enabled = False
            ChkDisableSafetyCheck.BackColor = ColorTranslator.FromHtml("#d63031") 'red
            My.Settings.sDisableSafetyCheck = True
            'End If
        Else
            TimerDisaster.Enabled = True
            ChkDisableSafetyCheck.BackColor = Color.Transparent
            My.Settings.sDisableSafetyCheck = False
        End If
    End Sub

    Private Sub TimerColorStatus_Tick(sender As Object, e As EventArgs) Handles TimerColorStatus.Tick
        Try

            LblMonitorStatus.Text = pRunStatus

            'STARTUP,ABORTING,ABORTED,DAY,SHUTDOWN,WAITING,FLATS,DEEPSKY,VARIABLES,UNSAFE,DONE
            If pMonitorStatusColor = False Then
                Select Case pRunStatus
                    Case "NOT RUNNING"
                        LblMonitorStatus.BackColor = Color.Silver
                    Case "STARTUP"
                        LblMonitorStatus.BackColor = Color.LightBlue
                    Case "FLATS"
                        LblMonitorStatus.BackColor = Color.LightGreen
                    Case "DEEPSKY"
                        LblMonitorStatus.BackColor = Color.Green
                    Case "VARIABLES"
                        LblMonitorStatus.BackColor = Color.DarkGreen
                    Case "ABORTING"
                        LblMonitorStatus.BackColor = ColorTranslator.FromHtml("#d63031") 'red
                    Case "UNSAFE"
                        LblMonitorStatus.BackColor = ColorTranslator.FromHtml("#d63031") 'red
                        LblCCDExposureStatus.Text = ""
                    Case "SHUTDOWN"
                        LblMonitorStatus.BackColor = Color.Yellow
                    Case "DAY"
                        LblMonitorStatus.BackColor = Color.LightYellow
                        LblCCDExposureStatus.Text = ""
                    Case "WAITING"
                        LblMonitorStatus.BackColor = Color.LightSkyBlue
                    Case "ABORTED"
                        LblMonitorStatus.BackColor = Color.Orange
                        'LblCCDExposureStatus.Text = ""
                    Case "SMART"
                        LblMonitorStatus.BackColor = Color.DarkRed
                        LblCCDExposureStatus.Text = ""
                    Case "DONE"
                        LblMonitorStatus.BackColor = Color.LightCoral
                        LblCCDExposureStatus.Text = ""
                    Case "PAUSING"
                        LblMonitorStatus.BackColor = Color.LightCyan
                        LblCCDExposureStatus.Text = ""
                    Case Else
                        LblMonitorStatus.BackColor = Color.Purple
                End Select
                pMonitorStatusColor = True
            Else
                LblMonitorStatus.BackColor = Color.Silver
                pMonitorStatusColor = False
            End If

        Catch ex As Exception
            ShowMessage("TimerColorStatus_Tick: " + ex.Message, "CRITICAL", "Error!")
        End Try

    End Sub

    Private Sub TimerSmartError_Tick(sender As Object, e As EventArgs) Handles TimerSmartError.Tick
        Dim returnvalue As String

        Try
            If ChkDisableSafetyCheck.Checked = False And pManualAbort = False And pStartRun = True Then
                If pEquipmentStatus = "PAUSING" Or pEquipmentStatus = "TURNOFF" Then
                    'do nothing and wait
                Else
                    returnvalue = RunSmartErrorCheck()
                    If returnvalue <> "OK" Then
                        Exit Sub
                    End If
                End If
            End If

        Catch ex As Exception
            ShowMessage("TimerSmartError_Tick: " + ex.Message, "CRITICAL", "Error!")
        End Try
    End Sub

    Private Sub TimerHeartBeat_Tick_(sender As Object, e As EventArgs) Handles TimerHeartBeat.Tick
        Dim returnvalue As String

        Try
            returnvalue = MonitorHeartBeat()
            If returnvalue <> "OK" Then
                Exit Sub
            End If

        Catch ex As Exception
            ShowMessage("TimerSmartError_Tick: " + ex.Message, "CRITICAL", "Error!")
        End Try

    End Sub

    Private Sub TimerHang_Tick(sender As Object, e As EventArgs) Handles TimerHang.Tick
        If LblHang.BackColor = ColorTranslator.FromHtml("#4cd137") Then 'green Then
            LblHang.BackColor = Color.Transparent
        Else
            LblHang.BackColor = ColorTranslator.FromHtml("#4cd137") 'green
        End If
    End Sub

    Private Sub BtnExit_Click(sender As Object, e As EventArgs) Handles BtnExit.Click
        If pStartRun = True Then
            ShowMessage("First abort the run !", "CRITICAL", "Abort")
        Else
            Me.Close()
        End If
    End Sub

    Private Sub BtnProperties_Click(sender As Object, e As EventArgs) Handles BtnProperties.Click
        FrmProperties.Show()
    End Sub

    Private Sub BtnDebug_Click(sender As Object, e As EventArgs) Handles BtnDebug.Click
        FrmDebug.Show()
    End Sub

    Private Sub BtnTools_Click(sender As Object, e As EventArgs) Handles BtnTools.Click
        FrmTools.Show()
    End Sub

    Private Sub BtnStartSentinel_Click(sender As Object, e As EventArgs) Handles BtnStartSentinel.Click
        'try to start the sentinel
        StartProcess(My.Settings.sSentinelEXE)
    End Sub

    Private Sub BtnStopSound_Click(sender As Object, e As EventArgs) Handles BtnStopSound.Click
        StopSound()
    End Sub

    Private Sub BtnDeepsky_Click(sender As Object, e As EventArgs) Handles BtnDeepsky.Click
        FrmTarget.Show()
    End Sub

    Private Sub BtnHADS_Click(sender As Object, e As EventArgs) Handles BtnHADS.Click
        FrmHADS.Show()
    End Sub

    Private Sub BtnCalibration_Click(sender As Object, e As EventArgs) Handles BtnCalibration.Click

        FrmCalibration.Show()
    End Sub

    Private Sub BtnAbout_Click(sender As Object, e As EventArgs) Handles BtnAbout.Click

        FrmSplash.StartPosition = FormStartPosition.CenterScreen
        FrmSplash.Show()
    End Sub

    Private Sub FrmMain_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        Try
            LogInitializeArray()

            If pStartRun = True Then
                ShowMessage("First abort the run !", "CRITICAL", "Abort")
                e.Cancel = True
            End If
        Catch ex As Exception
            ShowMessage("FrmMain_Closing: " + ex.Message, "CRITICAL", "Error!")
        End Try
    End Sub

    Private Sub BtnMenu_Click(sender As Object, e As EventArgs) Handles BtnMenu.Click
        If SplitContainer.SplitterDistance > 40 Then
            SplitContainer.Width = 1190
            SplitContainer.SplitterDistance = 40
            Me.Width = 1195
        Else
            SplitContainer.Width = 1370
            SplitContainer.SplitterDistance = 220
            Me.Width = 1375
        End If
    End Sub

    Private Sub BtnStartRun_Click(sender As Object, e As EventArgs) Handles BtnStartRun.Click
        Try
            If ShowMessage("Do you want to manually start the run ?", "YESNO", "Start run...") = vbYes Then
                If My.Settings.sDisableSafetyCheck = True Then
                    ShowMessage("WARNING: ALL SAFETY CHECKS ARE DISABLED !", "CRITICAL", "Safety checks...")
                End If

                LogInitializeArray()

                If My.Settings.sSentinelAutostart = True Then
                    'try to start the sentinel
                    StartProcess(My.Settings.sSentinelEXE)
                End If


                If My.Settings.sRoofDevice <> "NONE" Then
                    'only when a sequence is not running, it can be started !
                    If pIsSequenceRunning = False And pStartRun = False Then

                        TimerStartRun.Enabled = True
                        BtnStartRun.Enabled = False
                        BtnStopRun.Enabled = True

                        'will reset status fields 
                        pStartRun = True
                        pAbort = False
                        pManualAbort = False
                        pIsSequenceRunning = False
                        pIsActionRunning = False
                        pSmartError = False
                        pSmartTimeStamp = CDate("01/01/0001")
                        pRunStatus = "STARTUP"
                        pEquipmentStatus = "PARTIAL" 'reset all equipment in case something was changed between runs

                        LogSessionEntry("ESSENTIAL", "STARTING RUN!", "", "BtnStartRun_Click", "PROGRAM")

                        TimerStartRun_Tick(Nothing, Nothing)
                    Else
                        LogSessionEntry("FULL", "Run already started!", "", "BtnStartRun_Click", "PROGRAM")
                    End If
                Else
                    If ShowMessage("Are you sure the roof is open ? ", "YESNO", "Roof warning...") = vbYes Then
                        'only when a sequence is not running, it can be started !
                        If pIsSequenceRunning = False And pStartRun = False Then

                            TimerStartRun.Enabled = True

                            BtnStartRun.Enabled = False
                            BtnStopRun.Enabled = True

                            'will reset status fields 
                            pStartRun = True
                            pAbort = False
                            pManualAbort = False
                            pIsSequenceRunning = False
                            pIsActionRunning = False
                            pSmartError = False
                            pSmartTimeStamp = CDate("01/01/0001")
                            pRunStatus = "STARTUP"
                            LogSessionEntry("ESSENTIAL", "STARTING RUN!", "", "BtnStartRun_Click", "PROGRAM")

                            TimerStartRun_Tick(Nothing, Nothing)
                        End If
                    End If
                End If
            End If

        Catch ex As Exception
            ShowMessage("BtnStartRun_Click: " + ex.Message, "CRITICAL", "Error!")
        End Try
    End Sub

    Private Sub BtnStopRun_Click(sender As Object, e As EventArgs) Handles BtnStopRun.Click
        Try
            If ShowMessage("Do you want to abort the run ?", "YESNO", "Abort run...") = vbYes Then
                'only when a sequence is running it can be aborted !
                If pStartRun = True And pRunStatus <> "ABORTED" Then
                    pRunStatus = "ABORTING"
                    LogSessionEntry("ESSENTIAL", "ABORTING RUN!", "", "BtnStopRun_Click", "PROGRAM")
                    pAbort = True
                    pManualAbort = True
                    pSmartError = False
                    pSmartTimeStamp = CDate("01/01/0001")
                    pStartRun = False 'do not restart run
                Else
                    TimerStartRun.Enabled = False
                    LblMonitorStatus.BackColor = Color.Silver
                    pRunStatus = "ABORTED"
                    LogSessionEntry("ESSENTIAL", "RUN ABORTED.", "", "BtnStopRun_Click", "SEQUENCE")
                    BtnStartRun.Enabled = True
                    BtnStopRun.Enabled = False
                    pStartRun = False 'do not restart run
                    pSmartError = False
                    pSmartTimeStamp = CDate("01/01/0001")
                    StopSound()
                End If
                LogInitializeArray()
                'reset timer disaster
                TimerDisaster.Stop()
                TimerDisaster.Start()
            End If

        Catch ex As Exception
            ShowMessage("BtnStopRun_Click: " + ex.Message, "CRITICAL", "Error!")
        End Try
    End Sub

    Private Sub ChkAutoStart_CheckedChanged_1(sender As Object, e As EventArgs) Handles ChkAutoStart.CheckedChanged
        Try
            If ChkAutoStart.Checked = True Then
                If pStartRun = True Then
                    'do nothing, already running
                Else
                    'start the run

                    If My.Settings.sSentinelAutostart = True Then
                        'try to start the sentinel
                        StartProcess(My.Settings.sSentinelEXE)
                    End If


                    pStartRun = True
                    pAbort = False
                    pManualAbort = False
                    pIsSequenceRunning = False
                    pSmartError = False
                    pSmartTimeStamp = CDate("01/01/0001")
                    pRunStatus = "STARTUP"
                    LogSessionEntry("FULL", "Enabling autostart!", "", "ChkAutoStart_CheckedChanged", "PROGRAM")

                    BtnStartRun.Enabled = False
                    BtnStopRun.Enabled = True

                    TimerStartRun.Enabled = True

                    If My.Settings.sAutoStart = False Then
                        My.Settings.sAutoStart = True
                        My.Settings.Save()
                    End If
                    LogSessionEntry("ESSENTIAL", "STARTING RUN!", "", "BtnStart_Click", "PROGRAM")
                End If

            Else
                'do nothing except disable the checkbox
                LogSessionEntry("FULL", "Disabling autostart!", "", "ChkAutoStart_CheckedChanged", "PROGRAM")
                If My.Settings.sAutoStart = True Then
                    My.Settings.sAutoStart = False
                    My.Settings.Save()
                End If
            End If

        Catch ex As Exception
            ShowMessage("ChkAutoStart_CheckedChanged: " + ex.Message, "CRITICAL", "Error!")
        End Try
    End Sub

    Private Sub ChkSimulatorMode_CheckedChanged(sender As Object, e As EventArgs) Handles ChkSimulatorMode.CheckedChanged
        Try
            If ChkSimulatorMode.Checked = True Then
                ChkSimulatorMode.BackColor = ColorTranslator.FromHtml("#d63031") 'red
                My.Settings.sSimulatorMode = True
            Else
                ChkSimulatorMode.BackColor = Color.Transparent
                My.Settings.sSimulatorMode = False
            End If
        Catch ex As Exception
            ShowMessage("ChkDebugMode_CheckedChanged: " + ex.Message, "CRITICAL", "Error!")
        End Try
    End Sub

    Private Sub BtnViewLog_Click(sender As Object, e As EventArgs) Handles BtnViewLog.Click
        System.Diagnostics.Process.Start("wordpad.exe", pLogFileNameRTF)
    End Sub

    Private Sub BtnClearLog_Click(sender As Object, e As EventArgs) Handles BtnClearLog.Click
        RTXLog.Clear()
    End Sub

    Private Sub BtnClearErrorLog_Click(sender As Object, e As EventArgs) Handles BtnClearErrorLog.Click
        RTXErrors.Clear()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs)
        ShowMessage("godamned error", "CRITICAL", "what the fuck")
    End Sub


End Class
