Imports System.ComponentModel
Imports System.Threading

Module ModSwitch

    Public pSwitchingEquipmentOn, pSwitchingEquipmentOff As Boolean 'while switching on equipment, don't do anything else
    Public pSwitchEquipmentOnOff As String 'is the necessary gear turned on / off
    Public pSwitchAnyOn As Boolean 'is any of the switches turned on
    Public pSwitchConnected As Boolean

    Private WithEvents BgwSwitchStatus As BackgroundWorker
    Private BgwErrorSwitchStatus As String
    Private ResetErrorSwitchStatus As String
    Private OldResetErrorSwitchStatus As String

    Private ASwitch As ASCOM.DriverAccess.Switch
    Public Structure StrucSwitch
        Dim Switch1Status As Boolean
        Dim Switch2Status As Boolean
        Dim Switch3Status As Boolean
        Dim Switch4Status As Boolean
        Dim Switch5Status As Boolean
        Dim Switch6Status As Boolean
        Dim Switch7Status As Boolean
        Dim Switch8Status As Boolean
    End Structure

    Public pStructSwitch As StrucSwitch


    Public Function SwitchConnect() As String
        Dim returnvalue As String
        Dim startExecution As Date
        Dim executionTime As TimeSpan

        SwitchConnect = "OK"
        Try
            FrmMain.Cursor = Cursors.WaitCursor
            startExecution = DateTime.UtcNow()
            If My.Settings.sSimulatorMode = True Then
                LogSessionEntry("BRIEF", "Connecting to the switch...", "", "SwitchConnect", "SWITCH")
            ElseIf My.Settings.sSwitchDevice = "Dragonfly.Switch" Then
                LogSessionEntry("BRIEF", "Connecting to the Dragonfly...", "", "SwitchConnect", "SWITCH")
            Else
                LogSessionEntry("BRIEF", "Connecting to the switch...", "", "SwitchConnect", "SWITCH")
            End If


            If My.Settings.sSimulatorMode = True Then 'ASCOM.Simulator.Switch
                ASwitch = New ASCOM.DriverAccess.Switch("SwitchSim.Switch") With {
                .Connected = True
                }
                LogSessionEntry("FULL", "Switch connected.", "", "SwitchConnect", "SWITCH")
            ElseIf My.Settings.sSwitchDevice = "Dragonfly.Switch" Then
                ASwitch = New ASCOM.DriverAccess.Switch("Dragonfly.Switch") With {
                    .Connected = True
                  }
                LogSessionEntry("FULL", "Dragonfly connected.", "", "SwitchConnect", "SWITCH")
            Else
                ASwitch = New ASCOM.DriverAccess.Switch(My.Settings.sSwitchDevice) With {
                       .Connected = True
                    }
                LogSessionEntry("FULL", "Switch connected.", "", "SwitchConnect", "SWITCH")
            End If
            pSwitchConnected = True

            BgwSwitchStatus = New BackgroundWorker With {
            .WorkerReportsProgress = True,
             .WorkerSupportsCancellation = True
                 }

            'check switch: fill fields on main form
            returnvalue = CheckSwitch()
            If returnvalue <> "OK" Then
                SwitchConnect = "SwitchConnect: " + returnvalue
            End If

            FrmMain.Cursor = Cursors.Default
            executionTime = DateTime.UtcNow() - startExecution
            LogSessionEntry("DEBUG", "  SwitchConnect: " + executionTime.ToString, "", "SwitchConnect", "SWITCH")

        Catch ex As Exception
            FrmMain.Cursor = Cursors.Default
            SwitchConnect = "SwitchConnect: " + ex.Message
            LogSessionEntry("ERROR", "SwitchConnect: " + ex.Message, "", "SwitchConnect", "SWITCH")
        End Try
    End Function

    Public Function SwitchDisconnect() As String
        Dim returnvalue As String
        Dim startExecution As Date
        Dim executionTime As TimeSpan

        SwitchDisconnect = "OK"
        Try
            FrmMain.Cursor = Cursors.WaitCursor
            startExecution = DateTime.UtcNow()

            If My.Settings.sSimulatorMode = True Then
                LogSessionEntry("BRIEF", "Disconnecting switch...", "", "SwitchDisconnect", "SWITCH")
            Else
                LogSessionEntry("BRIEF", "Disconnecting Dragonfly...", "", "SwitchDisconnect", "SWITCH")
            End If

            ASwitch.Connected = False
            If My.Settings.sSimulatorMode = True Then
                LogSessionEntry("FULL", "Switch disconnected.", "", "SwitchDisconnect", "SWITCH")
            Else
                LogSessionEntry("FULL", "Dragonfly disconnected.", "", "SwitchDisconnect", "SWITCH")
            End If
            pSwitchConnected = False

            'check switch: fill fields on main form
            returnvalue = CheckSwitch()
            If returnvalue <> "OK" Then
                SwitchDisconnect = "SwitchDisconnect: " + returnvalue
            End If

            FrmMain.Cursor = Cursors.Default
            executionTime = DateTime.UtcNow() - startExecution
            LogSessionEntry("DEBUG", "  SwitchDisconnect: " + executionTime.ToString, "", "SwitchDisconnect", "SWITCH")

        Catch ex As Exception
            FrmMain.Cursor = Cursors.Default
            SwitchDisconnect = "SwitchDisconnect: " + ex.Message
            LogSessionEntry("ERROR", "SwitchDisconnect: " + ex.Message, "", "SwitchDisconnect", "SWITCH")
        End Try
    End Function

    Public Function SwitchStatus() As String
        Dim startExecution As Date
        Dim executionTime As TimeSpan

        SwitchStatus = "OK"
        Try
            startExecution = DateTime.UtcNow()
            LogSessionEntry("DEBUG", "  2SwitchStatus: " + startExecution.ToString, "", "SwitchStatus", "SWITCH")

            'pStructSwitch.Switch1Status = ASwitch.GetSwitch(0)
            'pStructSwitch.Switch2Status = ASwitch.GetSwitch(1)
            'pStructSwitch.Switch3Status = ASwitch.GetSwitch(2)
            'pStructSwitch.Switch4Status = ASwitch.GetSwitch(3)
            'pStructSwitch.Switch5Status = ASwitch.GetSwitch(4)
            'pStructSwitch.Switch6Status = ASwitch.GetSwitch(5)
            'pStructSwitch.Switch7Status = ASwitch.GetSwitch(6)
            'pStructSwitch.Switch8Status = ASwitch.GetSwitch(7)

            '-----------------------------



            If BgwSwitchStatus.IsBusy = False Then
                LogSessionEntry("DEBUG", "  SwitchStatus - RunWorkerAsync: " + startExecution.ToString, "", "SwitchStatus", "SWITCH")
                BgwSwitchStatus.RunWorkerAsync(1)
                'Else
                'LogSessionEntry("BRIEF", "Switch Status: background already occupied!", "SwitchStatus", "SWITCH")
                'SwitchStatus = "NOK"
                'Exit Function
            End If

            While BgwSwitchStatus.IsBusy = True
                My.Application.DoEvents()
            End While

            If BgwErrorSwitchStatus <> "OK" Then
                SwitchStatus = BgwErrorSwitchStatus
            End If

            LogSessionEntry("DEBUG", "  3SwitchStatus: " + Format(pStructSwitch.Switch1Status) + "-" + Format(pStructSwitch.Switch2Status) + "-" + Format(pStructSwitch.Switch3Status) + "-" + Format(pStructSwitch.Switch4Status) _
                + "-" + Format(pStructSwitch.Switch5Status) + "-" + Format(pStructSwitch.Switch6Status) + "-" + Format(pStructSwitch.Switch7Status) + "-" + Format(pStructSwitch.Switch8Status), "", "SwitchStatus", "SWITCH")
            executionTime = DateTime.UtcNow() - startExecution
            LogSessionEntry("DEBUG", "  4SwitchStatus: " + executionTime.ToString, "", "SwitchStatus", "SWITCH")

        Catch ex As Exception
            SwitchStatus = "5SwitchStatus: " + ex.Message
            LogSessionEntry("DEBUG", "1SwitchStatus: " + ex.Message, "SwitchStatus", "", "SWITCH") 'changed to full as switch sometimes disconnects
        End Try

    End Function


    Private Sub BgwSwitchStatus_DoWork(ByVal sender As Object, ByVal e As DoWorkEventArgs) Handles BgwSwitchStatus.DoWork
        Try
            BgwErrorSwitchStatus = "OK"

            pStructSwitch.Switch1Status = ASwitch.GetSwitch(0)
            pStructSwitch.Switch2Status = ASwitch.GetSwitch(1)
            pStructSwitch.Switch3Status = ASwitch.GetSwitch(2)
            pStructSwitch.Switch4Status = ASwitch.GetSwitch(3)
            pStructSwitch.Switch5Status = ASwitch.GetSwitch(4)
            pStructSwitch.Switch6Status = ASwitch.GetSwitch(5)
            pStructSwitch.Switch7Status = ASwitch.GetSwitch(6)
            pStructSwitch.Switch8Status = ASwitch.GetSwitch(7)
            ResetErrorSwitchStatus = "OK"
        Catch ex As Exception
            ResetErrorSwitchStatus = "NOK"
            BgwErrorSwitchStatus = ex.Message
        End Try
    End Sub


    Private Sub BgwSwitchStatus_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs) Handles BgwSwitchStatus.RunWorkerCompleted
        ' do nothing
        LogSessionEntry("DEBUG", "BgwSwitchStatus_RunWorkerCompleted: " + BgwErrorSwitchStatus, "", "BgwSwitchStatus_RunWorkerCompleted", "SWITCH")

        If BgwErrorSwitchStatus <> "OK" Then
            LogSessionEntry("BRIEF", "Switch status: " + BgwErrorSwitchStatus, "", "BgwSwitchStatus_RunWorkerCompleted", "SWITCH")
            FrmMain.LblSwitch.Text = "LOST CON"
            FrmMain.LblSwitch.BackColor = Color.Purple
            OldResetErrorSwitchStatus = "NOK"
        End If

        'show a message when communication is restored
        If ResetErrorSwitchStatus = "OK" Then
            If OldResetErrorSwitchStatus = "NOK" Then
                'switch is again up and running
                LogSessionEntry("BRIEF", "Switch was reset!", "", "BgwSwitchStatus_RunWorkerCompleted", "SWITCH")
                OldResetErrorSwitchStatus = "OK"
            End If
        End If

    End Sub

    Public Function CheckSwitch() As String
        Dim returnvalue As String
        Dim startExecution As Date
        Dim executionTime As TimeSpan
        Dim SwitchNeeded, SwitchAvailable As Integer

        CheckSwitch = "OK"
        Try
            startExecution = DateTime.UtcNow()

            returnvalue = SwitchStatus()
            If returnvalue <> "OK" Then
                CheckSwitch = returnvalue
                Exit Function
            End If

            SwitchNeeded = 0
            SwitchAvailable = 0

            If My.Settings.sSwitch1Needed = True Then
                SwitchNeeded += 1
                If pStructSwitch.Switch1Status = True Then SwitchAvailable += 1
            End If
            If My.Settings.sSwitch2Needed = True Then
                SwitchNeeded += 1
                If pStructSwitch.Switch2Status = True Then SwitchAvailable += 1
            End If
            If My.Settings.sSwitch3Needed = True Then
                SwitchNeeded += 1
                If pStructSwitch.Switch3Status = True Then SwitchAvailable += 1
            End If
            If My.Settings.sSwitch4Needed = True Then
                SwitchNeeded += 1
                If pStructSwitch.Switch4Status = True Then SwitchAvailable += 1
            End If
            If My.Settings.sSwitch5Needed = True Then
                SwitchNeeded += 1
                If pStructSwitch.Switch5Status = True Then SwitchAvailable += 1
            End If
            If My.Settings.sSwitch6Needed = True Then
                SwitchNeeded += 1
                If pStructSwitch.Switch6Status = True Then SwitchAvailable += 1
            End If
            If My.Settings.sSwitch7Needed = True Then
                SwitchNeeded += 1
                If pStructSwitch.Switch7Status = True Then SwitchAvailable += 1
            End If

            If (pStructSwitch.Switch1Status = True And My.Settings.sSwitch1Needed = True) Or
                    (pStructSwitch.Switch2Status = True And My.Settings.sSwitch2Needed = True) Or
                    (pStructSwitch.Switch3Status = True And My.Settings.sSwitch3Needed = True) Or
                    (pStructSwitch.Switch4Status = True And My.Settings.sSwitch4Needed = True) Or
                    (pStructSwitch.Switch5Status = True And My.Settings.sSwitch5Needed = True) Or
                    (pStructSwitch.Switch6Status = True And My.Settings.sSwitch6Needed = True) Or
                    (pStructSwitch.Switch7Status = True And My.Settings.sSwitch7Needed = True) Then
                pSwitchAnyOn = True
            Else
                pSwitchAnyOn = False

            End If

            If SwitchAvailable = SwitchNeeded Then
                FrmMain.LblSwitch.BackColor = Color.Green
                pSwitchEquipmentOnOff = "ON"
            Else
                FrmMain.LblSwitch.BackColor = Color.Red
                pSwitchEquipmentOnOff = "OFF"
            End If

            executionTime = DateTime.UtcNow() - startExecution
            LogSessionEntry("DEBUG", "  CheckSwitch: " + executionTime.ToString, "", "CheckSwitch", "SWITCH")

        Catch ex As Exception
            CheckSwitch = "CheckSwitch: " + ex.Message
            LogSessionEntry("ERROR", "CheckSwitch: " + ex.Message, "", "CheckSwitch", "SWITCH")
        End Try
    End Function

    Public Function SwitchEnable(vID As Integer, vState As Boolean) As String
        Dim returnvalue As String
        Dim OnOff As String
        Dim startExecution As Date
        Dim executionTime As TimeSpan

        SwitchEnable = "OK"
        Try
            FrmMain.Cursor = Cursors.WaitCursor
            startExecution = DateTime.UtcNow()

            ASwitch.SetSwitch(CShort(vID), vState)
            If vState = True Then
                OnOff = "on"
            Else
                OnOff = "off"
            End If

            Select Case vID
                Case 0
                    LogSessionEntry("BRIEF", "Switching " + OnOff + " " + My.Settings.sSwitch1Name + "...", "", "SwitchEnable", "SWITCH")
                Case 1
                    LogSessionEntry("BRIEF", "Switching " + OnOff + " " + My.Settings.sSwitch2Name + "...", "", "SwitchEnable", "SWITCH")
                Case 2
                    LogSessionEntry("BRIEF", "Switching " + OnOff + " " + My.Settings.sSwitch3Name + "...", "", "SwitchEnable", "SWITCH")
                Case 3
                    LogSessionEntry("BRIEF", "Switching " + OnOff + " " + My.Settings.sSwitch4Name + "...", "", "SwitchEnable", "SWITCH")
                Case 4
                    LogSessionEntry("BRIEF", "Switching " + OnOff + " " + My.Settings.sSwitch5Name + "...", "", "SwitchEnable", "SWITCH")
                Case 5
                    LogSessionEntry("BRIEF", "Switching " + OnOff + " " + My.Settings.sSwitch6Name + "...", "", "SwitchEnable", "SWITCH")
                Case 6
                    LogSessionEntry("BRIEF", "Switching " + OnOff + " " + My.Settings.sSwitch7Name + "...", "", "SwitchEnable", "SWITCH")
                Case 7
                    LogSessionEntry("BRIEF", "Switching " + OnOff + " " + My.Settings.sSwitch8Name + "...", "", "SwitchEnable", "SWITCH")
            End Select

            'check switch: fill fields on main form
            returnvalue = CheckSwitch()
            If returnvalue <> "OK" Then
                SwitchEnable = "SwitchEnable: " + returnvalue
            End If

            FrmMain.Cursor = Cursors.Default
            executionTime = DateTime.UtcNow() - startExecution
            LogSessionEntry("DEBUG", "  SwitchEnable: " + executionTime.ToString, "", "SwitchEnable", "SWITCH")

        Catch ex As Exception
            FrmMain.Cursor = Cursors.Default
            SwitchEnable = "SwitchEnable: " + ex.Message
            LogSessionEntry("ERROR", "SwitchEnable: " + ex.Message, "", "SwitchEnable", "SWITCH")
        End Try

    End Function



    Public Function SwitchEquipmentOn() As String
        Dim returnvalue As String
        Dim startExecution As Date
        Dim executionTime As TimeSpan

        SwitchEquipmentOn = "OK"
        Try
            startExecution = DateTime.UtcNow()

            If pSwitchingEquipmentOff = True Then
                LogSessionEntry("BRIEF", "Cannot switch equipment on: switching off not completed!", "", "SwitchEquipmentOn", "SWITCH")
            Else
                LogSessionEntry("BRIEF", "Switching on equipment...", "", "SwitchEquipmentOn", "SWITCH")
                pSwitchingEquipmentOn = True

                'wait 5 seconds between turning equipment on/off
                If My.Settings.sSwitch1Needed = True And pStructSwitch.Switch1Status = False Then
                    returnvalue = SwitchEnable(0, True)
                    If returnvalue <> "OK" Then
                        pSwitchingEquipmentOn = False
                        SwitchEquipmentOn = returnvalue
                        Exit Function
                    End If
                    WaitSeconds(5, False, True)

                    If My.Settings.sSwitch1Mount = True Then
                        returnvalue = StartSwitchMount()
                        If returnvalue <> "OK" Then
                            pSwitchingEquipmentOn = False
                            SwitchEquipmentOn = returnvalue
                            Exit Function
                        End If
                    Else
                        If My.Settings.sSwitch1Startup > 0 Then
                            LogSessionEntry("BRIEF", "Waiting for equipment to start: " + Format(My.Settings.sSwitch1Startup) + " seconds...", "", "SwitchEquipmentOn", "SWITCH")
                            WaitSeconds(My.Settings.sSwitch1Startup, False, True)
                        End If
                    End If
                End If

                If My.Settings.sSwitch2Needed = True And pStructSwitch.Switch2Status = False Then
                    returnvalue = SwitchEnable(1, True)
                    If returnvalue <> "OK" Then
                        pSwitchingEquipmentOn = False
                        SwitchEquipmentOn = returnvalue
                        Exit Function
                    End If
                    WaitSeconds(5, False, True)

                    If My.Settings.sSwitch2Mount = True Then
                        returnvalue = StartSwitchMount()
                        If returnvalue <> "OK" Then
                            pSwitchingEquipmentOn = False
                            SwitchEquipmentOn = returnvalue
                            Exit Function
                        End If
                    Else
                        If My.Settings.sSwitch2Startup > 0 Then
                            LogSessionEntry("BRIEF", "Waiting for equipment to start: " + Format(My.Settings.sSwitch2Startup) + " seconds...", "", "SwitchEquipmentOn", "SWITCH")
                            WaitSeconds(My.Settings.sSwitch2Startup, False, True)
                        End If
                    End If
                End If

                If My.Settings.sSwitch3Needed = True And pStructSwitch.Switch3Status = False Then
                    returnvalue = SwitchEnable(2, True)
                    If returnvalue <> "OK" Then
                        pSwitchingEquipmentOn = False
                        SwitchEquipmentOn = returnvalue
                        Exit Function
                    End If
                    WaitSeconds(5, False, True)

                    If My.Settings.sSwitch3Mount = True Then
                        returnvalue = StartSwitchMount()
                        If returnvalue <> "OK" Then
                            pSwitchingEquipmentOn = False
                            SwitchEquipmentOn = returnvalue
                            Exit Function
                        End If
                    Else
                        If My.Settings.sSwitch3Startup > 0 Then
                            LogSessionEntry("BRIEF", "Waiting for equipment to start: " + Format(My.Settings.sSwitch3Startup) + " seconds...", "", "SwitchEquipmentOn", "SWITCH")
                            WaitSeconds(My.Settings.sSwitch3Startup, False, True)
                        End If
                    End If
                End If

                If My.Settings.sSwitch4Needed = True And pStructSwitch.Switch4Status = False Then
                    returnvalue = SwitchEnable(3, True)
                    If returnvalue <> "OK" Then
                        pSwitchingEquipmentOn = False
                        SwitchEquipmentOn = returnvalue
                        Exit Function
                    End If
                    WaitSeconds(5, False, True)

                    If My.Settings.sSwitch4Mount = True Then
                        returnvalue = StartSwitchMount()
                        If returnvalue <> "OK" Then
                            pSwitchingEquipmentOn = False
                            SwitchEquipmentOn = returnvalue
                            Exit Function
                        End If
                    Else
                        If My.Settings.sSwitch4Startup > 0 Then
                            LogSessionEntry("BRIEF", "Waiting for equipment to start: " + Format(My.Settings.sSwitch4Startup) + " seconds...", "", "SwitchEquipmentOn", "SWITCH")
                            WaitSeconds(My.Settings.sSwitch4Startup, False, True)
                        End If
                    End If
                End If

                If My.Settings.sSwitch5Needed = True And pStructSwitch.Switch5Status = False Then
                    returnvalue = SwitchEnable(4, True)
                    If returnvalue <> "OK" Then
                        pSwitchingEquipmentOn = False
                        SwitchEquipmentOn = returnvalue
                        Exit Function
                    End If
                    WaitSeconds(5, False, True)

                    If My.Settings.sSwitch5Mount = True Then
                        returnvalue = StartSwitchMount()
                        If returnvalue <> "OK" Then
                            pSwitchingEquipmentOn = False
                            SwitchEquipmentOn = returnvalue
                            Exit Function
                        End If
                    Else
                        If My.Settings.sSwitch5Startup > 0 Then
                            LogSessionEntry("BRIEF", "Waiting for equipment to start: " + Format(My.Settings.sSwitch5Startup) + " seconds...", "", "SwitchEquipmentOn", "SWITCH")
                            WaitSeconds(My.Settings.sSwitch5Startup, False, True)
                        End If
                    End If
                End If

                If My.Settings.sSwitch6Needed = True And pStructSwitch.Switch6Status = False Then
                    returnvalue = SwitchEnable(5, True)
                    If returnvalue <> "OK" Then
                        pSwitchingEquipmentOn = False
                        SwitchEquipmentOn = returnvalue
                        Exit Function
                    End If
                    WaitSeconds(5, False, True)

                    If My.Settings.sSwitch6Mount = True Then
                        returnvalue = StartSwitchMount()
                        If returnvalue <> "OK" Then
                            pSwitchingEquipmentOn = False
                            SwitchEquipmentOn = returnvalue
                            Exit Function
                        End If
                    Else
                        If My.Settings.sSwitch6Startup > 0 Then
                            LogSessionEntry("BRIEF", "Waiting for equipment to start: " + Format(My.Settings.sSwitch6Startup) + " seconds...", "", "SwitchEquipmentOn", "SWITCH")
                            WaitSeconds(My.Settings.sSwitch6Startup, False, True)
                        End If
                    End If
                End If

                If My.Settings.sSwitch7Needed = True And pStructSwitch.Switch7Status = False Then
                    returnvalue = SwitchEnable(6, True)
                    If returnvalue <> "OK" Then
                        pSwitchingEquipmentOn = False
                        SwitchEquipmentOn = returnvalue
                        Exit Function
                    End If
                    WaitSeconds(5, False, True)

                    If My.Settings.sSwitch7Mount = True Then
                        returnvalue = StartSwitchMount()
                        If returnvalue <> "OK" Then
                            pSwitchingEquipmentOn = False
                            SwitchEquipmentOn = returnvalue
                            Exit Function
                        End If
                    Else
                        If My.Settings.sSwitch7Startup > 0 Then
                            LogSessionEntry("BRIEF", "Waiting for equipment to start: " + Format(My.Settings.sSwitch7Startup) + " seconds...", "", "SwitchEquipmentOn", "SWITCH")
                            WaitSeconds(My.Settings.sSwitch7Startup, False, True)
                        End If
                    End If
                End If

                If My.Settings.sSwitch8Needed = True And pStructSwitch.Switch8Status = False Then
                    returnvalue = SwitchEnable(7, True)
                    If returnvalue <> "OK" Then
                        pSwitchingEquipmentOn = False
                        SwitchEquipmentOn = returnvalue
                        Exit Function
                    End If
                    WaitSeconds(5, False, True)

                    If My.Settings.sSwitch8Mount = True Then
                        returnvalue = StartSwitchMount()
                        If returnvalue <> "OK" Then
                            pSwitchingEquipmentOn = False
                            SwitchEquipmentOn = returnvalue
                            Exit Function
                        End If
                    Else
                        If My.Settings.sSwitch8Startup > 0 Then
                            LogSessionEntry("BRIEF", "Waiting for equipment to start: " + Format(My.Settings.sSwitch8Startup) + " seconds...", "", "SwitchEquipmentOn", "SWITCH")
                            WaitSeconds(My.Settings.sSwitch8Startup, False, True)
                        End If
                    End If
                End If

                LogSessionEntry("BRIEF", "Equipment switched on.", "", "SwitchEquipmentOn", "SWITCH")
                pSwitchingEquipmentOn = False
            End If

            executionTime = DateTime.UtcNow() - startExecution
            LogSessionEntry("DEBUG", "  SwitchEquipmentOn:  " + executionTime.ToString, "", "SwitchEquipmentOn", "SWITCH")

        Catch ex As Exception
            SwitchEquipmentOn = "SwitchEquipmentOn: " + ex.Message
            pSwitchingEquipmentOn = False
            LogSessionEntry("ERROR", "SwitchEquipmentOn: " + ex.Message, "", "SwitchEquipmentOn", "SWITCH")
        End Try

    End Function

    Private Function StartSwitchMount() As String
        Dim returnvalue As String

        StartSwitchMount = "OK"
        Try
            returnvalue = MountStartupTime()
            If My.Settings.sMountDevice = "ASCOM.tenmicron_mount.Telescope" Then
                LogSessionEntry("BRIEF", "Waiting for Baader remote switch to power up.", "", "MountConnect", "MOUNT")
                WaitSeconds(15, False, True)

                If My.Settings.sSimulatorMode = False Then
                    'turn on the mount physically
                    Dim webClient As New System.Net.WebClient
                    Dim result As String = webClient.DownloadString(My.Settings.sMountStartupLink)
                    Dim CurrentTime = Date.UtcNow
                    Dim i As Long

                    i = 0
                    LogSessionEntry("BRIEF", "Starting 10Micron mount...", "", "MountConnect", "MOUNT")
                    FrmMain.Cursor = Cursors.WaitCursor


                    Do While i <= pMountStartupTime
                        LogSessionEntry("BRIEF", "Starting 10Micron mount...", "", "MountConnect", "MOUNT")
                        Application.DoEvents()
                        i = DateDiff(DateInterval.Second, CurrentTime, Date.UtcNow)
                        ' if run is to abort: do nothing and wait, we want to be sure the mount is on before aborting !                            
                    Loop
                Else
                    LogSessionEntry("BRIEF", "Starting 10Micron mount...", "", "MountConnect", "MOUNT")
                End If
            Else
                'other mounts
                LogSessionEntry("BRIEF", "Starting mount...", "", "MountConnect", "MOUNT")
                WaitSeconds(pMountStartupTime, False, True)
            End If
            FrmMain.Cursor = Cursors.Default
        Catch ex As Exception
            StartSwitchMount = "StartSwitchMount: " + ex.Message
            LogSessionEntry("ERROR", "StartSwitchMount: " + ex.Message, "", "StartSwitchMount", "SWITCH")
        End Try
    End Function

    Public Function SwitchEquipmentOff() As String
        Dim returnvalue As String
        Dim startExecution As Date
        Dim executionTime As TimeSpan

        SwitchEquipmentOff = "OK"
        Try
            startExecution = DateTime.UtcNow()

            If pSwitchingEquipmentOn = True Then
                LogSessionEntry("BRIEF", "Cannot switch off equipment: switching on not completed!", "", "SwitchEquipmentOff", "SWITCH")
            Else
                LogSessionEntry("BRIEF", "Switching off equipment...", "", "SwitchEquipmentOff", "SWITCH")
                pSwitchingEquipmentOff = True

                'wait 5 seconds between turning equipment on/off
                If My.Settings.sSwitch1Needed = True And pStructSwitch.Switch1Status = True Then
                    If My.Settings.sSwitch1Mount = True Then
                        returnvalue = ShutdownSwitchMount()
                        If returnvalue <> "OK" Then
                            pSwitchingEquipmentOn = False
                            SwitchEquipmentOff = returnvalue
                            Exit Function
                        End If
                    End If

                    returnvalue = SwitchEnable(0, False)
                    If returnvalue <> "OK" Then
                        pSwitchingEquipmentOn = False
                        SwitchEquipmentOff = returnvalue
                        Exit Function
                    End If
                    WaitSeconds(5, False, True)
                End If

                If My.Settings.sSwitch2Needed = True And pStructSwitch.Switch2Status = True Then
                    If My.Settings.sSwitch2Mount = True Then
                        returnvalue = ShutdownSwitchMount()
                        If returnvalue <> "OK" Then
                            pSwitchingEquipmentOn = False
                            SwitchEquipmentOff = returnvalue
                            Exit Function
                        End If
                    End If

                    returnvalue = SwitchEnable(1, False)
                    If returnvalue <> "OK" Then
                        pSwitchingEquipmentOn = False
                        SwitchEquipmentOff = returnvalue
                        Exit Function
                    End If
                    WaitSeconds(5, False, True)
                End If

                If My.Settings.sSwitch3Needed = True And pStructSwitch.Switch3Status = True Then
                    If My.Settings.sSwitch3Mount = True Then
                        returnvalue = ShutdownSwitchMount()
                        If returnvalue <> "OK" Then
                            pSwitchingEquipmentOn = False
                            SwitchEquipmentOff = returnvalue
                            Exit Function
                        End If
                    End If

                    returnvalue = SwitchEnable(2, False)
                    If returnvalue <> "OK" Then
                        pSwitchingEquipmentOn = False
                        SwitchEquipmentOff = returnvalue
                        Exit Function
                    End If
                    WaitSeconds(5, False, True)
                End If

                If My.Settings.sSwitch4Needed = True And pStructSwitch.Switch4Status = True Then
                    If My.Settings.sSwitch4Mount = True Then
                        returnvalue = ShutdownSwitchMount()
                        If returnvalue <> "OK" Then
                            pSwitchingEquipmentOn = False
                            SwitchEquipmentOff = returnvalue
                            Exit Function
                        End If
                    End If

                    returnvalue = SwitchEnable(3, False)
                    If returnvalue <> "OK" Then
                        pSwitchingEquipmentOn = False
                        SwitchEquipmentOff = returnvalue
                        Exit Function
                    End If
                    WaitSeconds(5, False, True)
                End If

                If My.Settings.sSwitch5Needed = True And pStructSwitch.Switch5Status = True Then
                    If My.Settings.sSwitch5Mount = True Then
                        returnvalue = ShutdownSwitchMount()
                        If returnvalue <> "OK" Then
                            pSwitchingEquipmentOn = False
                            SwitchEquipmentOff = returnvalue
                            Exit Function
                        End If
                    End If

                    returnvalue = SwitchEnable(4, False)
                    If returnvalue <> "OK" Then
                        pSwitchingEquipmentOn = False
                        SwitchEquipmentOff = returnvalue
                        Exit Function
                    End If
                    WaitSeconds(5, False, True)
                End If

                If My.Settings.sSwitch6Needed = True And pStructSwitch.Switch6Status = True Then
                    If My.Settings.sSwitch6Mount = True Then
                        returnvalue = ShutdownSwitchMount()
                        If returnvalue <> "OK" Then
                            pSwitchingEquipmentOn = False
                            SwitchEquipmentOff = returnvalue
                            Exit Function
                        End If
                    End If

                    returnvalue = SwitchEnable(5, False)
                    If returnvalue <> "OK" Then
                        pSwitchingEquipmentOn = False
                        SwitchEquipmentOff = returnvalue
                        Exit Function
                    End If
                    WaitSeconds(5, False, True)
                End If

                If My.Settings.sSwitch7Needed = True And pStructSwitch.Switch7Status = True Then
                    If My.Settings.sSwitch7Mount = True Then
                        returnvalue = ShutdownSwitchMount()
                        If returnvalue <> "OK" Then
                            pSwitchingEquipmentOn = False
                            SwitchEquipmentOff = returnvalue
                            Exit Function
                        End If
                    End If

                    returnvalue = SwitchEnable(6, False)
                    If returnvalue <> "OK" Then
                        pSwitchingEquipmentOn = False
                        SwitchEquipmentOff = returnvalue
                        Exit Function
                    End If
                    WaitSeconds(5, False, True)
                End If

                If My.Settings.sSwitch8Needed = True And pStructSwitch.Switch8Status = True Then
                    If My.Settings.sSwitch8Mount = True Then
                        returnvalue = ShutdownSwitchMount()
                        If returnvalue <> "OK" Then
                            pSwitchingEquipmentOn = False
                            SwitchEquipmentOff = returnvalue
                            Exit Function
                        End If
                    End If

                    returnvalue = SwitchEnable(7, False)
                    If returnvalue <> "OK" Then
                        pSwitchingEquipmentOn = False
                        SwitchEquipmentOff = returnvalue
                        Exit Function
                    End If
                    WaitSeconds(5, False, True)
                End If

                LogSessionEntry("BRIEF", "Equipment switched off.", "", "SwitchEquipmentOff", "SWITCH")
                pSwitchingEquipmentOff = False
            End If

            executionTime = DateTime.UtcNow() - startExecution
            LogSessionEntry("DEBUG", "  SwitchEquipmentOff: " + executionTime.ToString, "", "SwitchEquipmentOff", "SWITCH")
        Catch ex As Exception
            SwitchEquipmentOff = "SwitchEquipmentOff: " + ex.Message
            pSwitchingEquipmentOff = False
            LogSessionEntry("ERROR", "SwitchEquipmentOff: " + ex.Message, "", "SwitchEquipmentOff", "SWITCH")
        End Try
    End Function

    Private Function ShutdownSwitchMount() As String
        Dim returnvalue As String
        Dim MountConnected As Boolean

        ShutdownSwitchMount = "OK"
        Try
            'first try to see if the mount is already powered down
            Try
                pMount.Connected = True
                MountConnected = True
            Catch ex2 As Exception
                MountConnected = False
            End Try

            'set to false to avoid timer issues
            pMountConnected = False
            If MountConnected = True And My.Settings.sSimulatorMode = False And My.Settings.sMountDevice = "ASCOM.tenmicron_mount.Telescope" Then
                'turn off the mount physically
                Dim webClient As New System.Net.WebClient
                Dim result As String = webClient.DownloadString(My.Settings.sMountStartupLink)
                Dim CurrentTime = Date.UtcNow
                Dim i As Long

                i = 0
                LogSessionEntry("BRIEF", "Shutting down 10Micron mount...", "", "ShutdownSwitchMount", "MOUNT")
                returnvalue = MountStartupTime()

                Do While i <= pMountStartupTime
                    LogSessionEntry("BRIEF", "Shutting down 10Micron mount...", "", "ShutdownSwitchMount", "MOUNT")
                    Application.DoEvents()
                    i = DateDiff(DateInterval.Second, CurrentTime, Date.UtcNow)
                Loop
                LogSessionEntry("BRIEF", "10Micron mount shut down...", "", "ShutdownSwitchMount", "MOUNT")
            End If

        Catch ex As Exception
            ShutdownSwitchMount = "ShutdownSwitchMount: " + ex.Message
            LogSessionEntry("ERROR", "ShutdownSwitchMount: " + ex.Message, "", "ShutdownSwitchMount", "SWITCH")
        End Try
    End Function

    Public Function SwitchAllEquipmentOff() As String
        'switches off anything needed or not
        Dim returnvalue As String
        Dim startExecution As Date
        Dim executionTime As TimeSpan

        SwitchAllEquipmentOff = "OK"
        Try
            startExecution = DateTime.UtcNow()

            If pSwitchingEquipmentOn = True Then
                LogSessionEntry("BRIEF", "Cannot switch off equipment: switching on not completed!", "", "SwitchAllEquipmentOff", "SWITCH")
            Else
                LogSessionEntry("BRIEF", "Switching off equipment...", "", "SwitchAllEquipmentOfff", "SWITCH")
                pSwitchingEquipmentOff = True

                'wait 5 seconds between turning equipment on/off
                If pStructSwitch.Switch1Status = True Then
                    If My.Settings.sSwitch1Mount = True Then
                        returnvalue = ShutdownSwitchMount()
                        If returnvalue <> "OK" Then
                            pSwitchingEquipmentOn = False
                            SwitchAllEquipmentOff = returnvalue
                            Exit Function
                        End If
                    End If

                    returnvalue = SwitchEnable(0, False)
                    If returnvalue <> "OK" Then
                        pSwitchingEquipmentOn = False
                        SwitchAllEquipmentOff = returnvalue
                        Exit Function
                    End If
                    WaitSeconds(5, False, True)
                End If

                If pStructSwitch.Switch2Status = True Then
                    If My.Settings.sSwitch2Mount = True Then
                        returnvalue = ShutdownSwitchMount()
                        If returnvalue <> "OK" Then
                            pSwitchingEquipmentOn = False
                            SwitchAllEquipmentOff = returnvalue
                            Exit Function
                        End If
                    End If

                    returnvalue = SwitchEnable(1, False)
                    If returnvalue <> "OK" Then
                        pSwitchingEquipmentOn = False
                        SwitchAllEquipmentOff = returnvalue
                        Exit Function
                    End If
                    WaitSeconds(5, False, True)
                End If

                If pStructSwitch.Switch3Status = True Then
                    If My.Settings.sSwitch3Mount = True Then
                        returnvalue = ShutdownSwitchMount()
                        If returnvalue <> "OK" Then
                            pSwitchingEquipmentOn = False
                            SwitchAllEquipmentOff = returnvalue
                            Exit Function
                        End If
                    End If

                    returnvalue = SwitchEnable(2, False)
                    If returnvalue <> "OK" Then
                        pSwitchingEquipmentOn = False
                        SwitchAllEquipmentOff = returnvalue
                        Exit Function
                    End If
                    WaitSeconds(5, False, True)
                End If

                If pStructSwitch.Switch4Status = True Then
                    If My.Settings.sSwitch4Mount = True Then
                        returnvalue = ShutdownSwitchMount()
                        If returnvalue <> "OK" Then
                            pSwitchingEquipmentOn = False
                            SwitchAllEquipmentOff = returnvalue
                            Exit Function
                        End If
                    End If

                    returnvalue = SwitchEnable(3, False)
                    If returnvalue <> "OK" Then
                        pSwitchingEquipmentOn = False
                        SwitchAllEquipmentOff = returnvalue
                        Exit Function
                    End If
                End If

                If pStructSwitch.Switch5Status = True Then
                    If My.Settings.sSwitch5Mount = True Then
                        returnvalue = ShutdownSwitchMount()
                        If returnvalue <> "OK" Then
                            pSwitchingEquipmentOn = False
                            SwitchAllEquipmentOff = returnvalue
                            Exit Function
                        End If
                    End If

                    returnvalue = SwitchEnable(4, False)
                    If returnvalue <> "OK" Then
                        pSwitchingEquipmentOn = False
                        SwitchAllEquipmentOff = returnvalue
                        Exit Function
                    End If
                    WaitSeconds(5, False, True)
                End If

                If pStructSwitch.Switch6Status = True Then
                    If My.Settings.sSwitch6Mount = True Then
                        returnvalue = ShutdownSwitchMount()
                        If returnvalue <> "OK" Then
                            pSwitchingEquipmentOn = False
                            SwitchAllEquipmentOff = returnvalue
                            Exit Function
                        End If
                    End If

                    returnvalue = SwitchEnable(5, False)
                    If returnvalue <> "OK" Then
                        pSwitchingEquipmentOn = False
                        SwitchAllEquipmentOff = returnvalue
                        Exit Function
                    End If
                    WaitSeconds(5, False, True)
                End If

                If pStructSwitch.Switch7Status = True Then
                    If My.Settings.sSwitch7Mount = True Then
                        returnvalue = ShutdownSwitchMount()
                        If returnvalue <> "OK" Then
                            pSwitchingEquipmentOn = False
                            SwitchAllEquipmentOff = returnvalue
                            Exit Function
                        End If
                    End If

                    returnvalue = SwitchEnable(6, False)
                    If returnvalue <> "OK" Then
                        pSwitchingEquipmentOn = False
                        SwitchAllEquipmentOff = returnvalue
                        Exit Function
                    End If
                    WaitSeconds(5, False, True)
                End If

                If pStructSwitch.Switch8Status = True Then
                    If My.Settings.sSwitch8Mount = True Then
                        returnvalue = ShutdownSwitchMount()
                        If returnvalue <> "OK" Then
                            pSwitchingEquipmentOn = False
                            SwitchAllEquipmentOff = returnvalue
                            Exit Function
                        End If
                    End If

                    returnvalue = SwitchEnable(7, False)
                    If returnvalue <> "OK" Then
                        pSwitchingEquipmentOn = False
                        SwitchAllEquipmentOff = returnvalue
                        Exit Function
                    End If
                    WaitSeconds(5, False, True)
                End If
                LogSessionEntry("BRIEF", "Equipment switched off.", "", "SwitchAllEquipmentOff", "SWITCH")
                pSwitchingEquipmentOff = False
            End If

            executionTime = DateTime.UtcNow() - startExecution
            LogSessionEntry("DEBUG", "  SwitchAllEquipmentOff: " + executionTime.ToString, "", "SwitchAllEquipmentOff", "SWITCH")

        Catch ex As Exception
            SwitchAllEquipmentOff = "SwitchEquipmentOff: " + ex.Message
            pSwitchingEquipmentOff = False
            LogSessionEntry("ERROR", "SwitchAllEquipmentOff: " + ex.Message, "", "SwitchAllEquipmentOff", "SWITCH")
        End Try
    End Function

End Module
