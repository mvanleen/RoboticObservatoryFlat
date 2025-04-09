Imports System.Threading

Module ModMount
    Public pMount As ASCOM.DriverAccess.Telescope
    Public pMountConnected As Boolean
    Public pMountStartupTime As Double
    Public Structure StrucMount
        Dim Altitude As Double
        Dim Azimuth As Double
        Dim AtPark As Boolean
        'Dim AtHome
        'Dim EquatorialSystem
        Dim Declination As Double
        Dim RightAscension As Double
        'Dim DriverInfo
        'Dim Name
        Dim SideOfPier As String
        'Dim SiderealTime
        'Dim DeclinationRate As Double
        'Dim RightAscensionRate As Double
        Dim Tracking As Boolean
        Dim TrackingRate As Double
        'Dim Slewing
        'Dim UTCDate
        'Dim CanMoveAxisRA
        'Dim CanMoveAxisDEC
    End Structure

    Public pStructMount As StrucMount
    Public pClosedLoopSlew As String
    Public pIsMountParking As Boolean

    'PierEast: Normal pointing state will be the state where a German Equatorial mount is on the East side of the pier, looking West, with the counterweights below the optical assembly and that pierEast will represent this pointing state.
    'PierWest: Beyond the pole (pierWest)	Where the mechanical Dec is in the range -180 deg to -90 deg or +90 deg to +180 deg.
    'pierEast	Normal pointing state
    'pierWest	Beyond the pole pointing state

    '-------------------------------------------------------------------------------------------------
    ' FOR 10MICRON MOUNT IN ASCOM DRIVER J2000 SHOULD BE DISABLED !
    '-------------------------------------------------------------------------------------------------

    Public Function MountConnect() As String
        Dim returnvalue As String
        Dim startExecution As Date
        Dim executionTime As TimeSpan

        'Dim webClient As New System.Net.WebClient
        'Dim result As String = webClient.DownloadString("http://user:secret@172.20.7.49/?cmd=5&p=1&a1=1&a2=0&s=2")

        MountConnect = "OK"
        Try
            startExecution = DateTime.UtcNow()
            LogSessionEntry("DEBUG", "  MountConnect...", "", "MountConnect", "PROGRAM")

            FrmMain.Cursor = Cursors.WaitCursor
            pIsActionRunning = True

            If My.Settings.sSimulatorMode = True Then
                LogSessionEntry("BRIEF", "Connecting to the mount...", "", "MountConnect", "MOUNT")
            Else
                If My.Settings.sMountDevice = "ASCOM.tenmicron_mount.Telescope" Then
                    LogSessionEntry("BRIEF", "Connecting to 10Micron mount...", "", "MountConnect", "MOUNT")
                ElseIf My.Settings.sMountDevice = "ASCOM.SoftwareBisque.Telescope" Then
                    LogSessionEntry("BRIEF", "Connecting to the Paramount...", "", "MountConnect", "MOUNT")
                Else
                    LogSessionEntry("BRIEF", "Connecting to the mount...", "", "MountConnect", "MOUNT")
                End If
            End If


            If My.Settings.sSimulatorMode = True Then
                pMount = New ASCOM.DriverAccess.Telescope("ASCOM.Simulator.Telescope") With {
                .Connected = True
                }
                LogSessionEntry("FULL", "Connected to mount.", "", "MountConnect", "MOUNT")
            Else
                If My.Settings.sMountDevice = "ASCOM.tenmicron_mount.Telescope" Then
                    Dim i As Long
                    Dim MountConnected As Boolean
                    Dim CurrentTime = Date.UtcNow

                    'first try to see if the mount is already powered
                    Try
                        pMount = New ASCOM.DriverAccess.Telescope("ASCOM.tenmicron_mount.Telescope") With {
                    .Connected = True
                     }

                        pMount.Connected = True
                        MountConnected = True
                    Catch ex2 As Exception
                        MountConnected = False
                    End Try

                    If MountConnected = False Then
                        'turn on the mount physically
                        Dim webClient As New System.Net.WebClient
                        Dim result As String = webClient.DownloadString(My.Settings.sMountStartupLink) '"http://user:secret@172.20.7.49/?cmd=5&p=1&a1=1&a2=0&s=2")

                        i = 0
                        LogSessionEntry("BRIEF", "Starting 10Micron mount.", "", "MountConnect", "MOUNT")
                        returnvalue = MountStartupTime()

                        Do While i <= pMountStartupTime
                            Application.DoEvents()
                            i = DateDiff(DateInterval.Second, CurrentTime, Date.UtcNow)
                            ' if run is to abort: do nothing and wait, we want to be sure the mount is on before aborting ! 
                            ' => no other events allowed !
                        Loop
                    End If
                    pMount.Connected = True
                    LogSessionEntry("BRIEF", "Connected to 10Micron mount.", "", "MountConnect", "MOUNT")
                ElseIf My.Settings.sMountDevice = "ASCOM.SoftwareBisque.Telescope" Then
                    Dim i As Long
                    Dim MountConnected As Boolean
                    Dim CurrentTime = Date.UtcNow

                    'first try to see if the mount is already powered
                    Try
                        pMount = New ASCOM.DriverAccess.Telescope("ASCOM.SoftwareBisque.Telescope") With {
                    .Connected = True
                     }

                        pMount.Connected = True
                        MountConnected = True
                    Catch ex2 As Exception
                        MountConnected = False
                    End Try

                    If MountConnected = False Then
                        i = 0
                        LogSessionEntry("BRIEF", "Starting Paramount MEII.", "", "MountConnect", "MOUNT")
                        returnvalue = MountStartupTime()

                        Do While i <= pMountStartupTime
                            Application.DoEvents()
                            i = DateDiff(DateInterval.Second, CurrentTime, Date.UtcNow)
                            ' if run is to abort: do nothing and wait, we want to be sure the mount is on before aborting !
                            ' => no other events allowed !
                        Loop
                    End If
                    pMount.Connected = True
                    LogSessionEntry("BRIEF", "Connected to Paramount MEII.", "", "MountConnect", "MOUNT")
                Else
                    pMount = New ASCOM.DriverAccess.Telescope(My.Settings.sMountDevice) With {
                    .Connected = True
                    }
                    LogSessionEntry("FULL", "Connected to mount.", "", "MountConnect", "MOUNT")
                End If
            End If

            pMountConnected = True
            pIsActionRunning = False

            'fill the fields on the main form
            returnvalue = CheckMountStatus()
            If returnvalue <> "OK" Then
                pIsActionRunning = False
                FrmMain.Cursor = Cursors.Default
                MountConnect = returnvalue
                Exit Function
            End If

            FrmMain.Cursor = Cursors.Default

            executionTime = DateTime.UtcNow() - startExecution
            LogSessionEntry("DEBUG", "  MountConnect: " + executionTime.ToString, "", "MountConnect", "MOUNT")

        Catch ex As Exception
            FrmMain.Cursor = Cursors.Default
            MountConnect = "MountConnect: " + ex.Message
            pIsActionRunning = False
            LogSessionEntry("ERROR", "MountConnect: " + ex.Message, "", "MountConnect", "MOUNT")
        End Try
    End Function

    Public Function MountStartupTime() As String
        Dim startExecution As Date
        Dim executionTime As TimeSpan

        MountStartupTime = "OK"
        Try
            startExecution = DateTime.UtcNow()
            LogSessionEntry("DEBUG", "  MountStartupTime...", "", "MountStartupTime", "MOUNT")

            If My.Settings.sSwitch1Mount = True Then
                pMountStartupTime = My.Settings.sSwitch1Startup
            ElseIf My.Settings.sSwitch2Mount = True Then
                pMountStartupTime = My.Settings.sSwitch2Startup
            ElseIf My.Settings.sSwitch3Mount = True Then
                pMountStartupTime = My.Settings.sSwitch3Startup
            ElseIf My.Settings.sSwitch4Mount = True Then
                pMountStartupTime = My.Settings.sSwitch4Startup
            ElseIf My.Settings.sSwitch5Mount = True Then
                pMountStartupTime = My.Settings.sSwitch5Startup
            ElseIf My.Settings.sSwitch6Mount = True Then
                pMountStartupTime = My.Settings.sSwitch6Startup
            ElseIf My.Settings.sSwitch7Mount = True Then
                pMountStartupTime = My.Settings.sSwitch7Startup
            ElseIf My.Settings.sSwitch8Mount = True Then
                pMountStartupTime = My.Settings.sSwitch8Startup
            Else
                pMountStartupTime = 0
                LogSessionEntry("ERROR", "Switch has no mount checked !", "", "MountStartupTime", "MOUNT")
            End If
            executionTime = DateTime.UtcNow() - startExecution
            LogSessionEntry("DEBUG", "  MountStartupTime: " + executionTime.ToString, "", "MountStartupTime", "MOUNT")

        Catch ex As Exception
            MountStartupTime = "MountStartupTime: " + ex.Message
            LogSessionEntry("ERROR", "MountStartupTime: " + ex.Message, "", "MountStartupTime", "MOUNT")
        End Try
    End Function

    Public Function MountDisconnect() As String
        Dim startExecution As Date
        Dim executionTime As TimeSpan

        MountDisconnect = "OK"
        Try
            startExecution = DateTime.UtcNow()
            LogSessionEntry("DEBUG", "  MountDisconnect...", "", "MountDisconnect", "MOUNT")

            FrmMain.Cursor = Cursors.WaitCursor
            If My.Settings.sSimulatorMode = True Then
                LogSessionEntry("BRIEF", "Disconnecting mount...", "", "MountDisonnect", "MOUNT")
            Else
                If My.Settings.sMountDevice = "ASCOM.tenmicron_mount.Telescope" Then
                    LogSessionEntry("BRIEF", "Disconnecting 10Micron...", "", "MountDisconnect", "MOUNT")
                ElseIf My.Settings.sMountDevice = "ASCOM.SoftwareBisque.Telescope" Then
                    LogSessionEntry("BRIEF", "Disconnecting Paramount MEII...", "", "MountDisconnect", "MOUNT")
                Else
                    LogSessionEntry("BRIEF", "Disconnecting mount...", "", "MountDisconnect", "MOUNT")
                End If
            End If

            'disconnect mount
            pMountConnected = False 'avoid a timer on mount status
            pMount.Connected = False

            If My.Settings.sSimulatorMode = True Then
                LogSessionEntry("FULL", "Mount disconnected.", "", "MountDisconnect", "MOUNT")
            Else
                If My.Settings.sMountDevice = "ASCOM.tenmicron_mount.Telescope" Then
                    LogSessionEntry("BRIEF", "10Micron mount disconnected...", "", "MountDisconnect", "MOUNT")
                ElseIf My.Settings.sMountDevice = "ASCOM.SoftwareBisque.Telescope" Then
                    LogSessionEntry("BRIEF", "Paramount MEII disconnected.", "", "MountDisconnect", "MOUNT")
                Else
                    LogSessionEntry("BRIEF", "Mount disconnected.", "", "MountDisconnect", "MOUNT")
                End If
            End If
            FrmMain.Cursor = Cursors.Default

            executionTime = DateTime.UtcNow() - startExecution
            LogSessionEntry("DEBUG", "  MountDisconnect: " + executionTime.ToString, "", "MountDisconnect", "MOUNT")

        Catch ex As Exception
            FrmMain.Cursor = Cursors.Default
            MountDisconnect = "MountDisconnect: " + ex.Message
            LogSessionEntry("ERROR", "MountDisconnect: " + ex.Message, "", "MountDisconnect", "MOUNT")
        End Try
    End Function

    Public Function CheckMountStatus() As String
        Dim startExecution As Date
        Dim executionTime As TimeSpan

        CheckMountStatus = "OK"
        Try
            startExecution = DateTime.UtcNow()
            LogSessionEntry("DEBUG", "  CheckMountStatus...", "", "CheckMountStatus", "MOUNT")

            'AUtil = New ASCOM.Utilities.Util

            pStructMount.Altitude = pMount.Altitude
            pStructMount.Azimuth = pMount.Azimuth
            pStructMount.AtPark = pMount.AtPark
            'pStructMount.AtHome = AMount.AtHome
            'pStructMount.EquatorialSystem = AMount.EquatorialSystem
            pStructMount.Declination = pMount.Declination
            pStructMount.RightAscension = pMount.RightAscension
            'pStructMount.DriverInfo = AMount.DriverInfo
            'pStructMount.Name = AMount.Name
            Try
                pStructMount.SideOfPier = Convert.ToString(pMount.SideOfPier)
            Catch ex As Exception
                pStructMount.SideOfPier = "-1"
            End Try

            'pStructMount.SiderealTime = AMount.SiderealTime
            'pStructMount.DeclinationRate = AMount.DeclinationRate
            'pStructMount.RightAscensionRate = AMount.RightAscensionRate
            pStructMount.Tracking = pMount.Tracking
                pStructMount.TrackingRate = pMount.TrackingRate
                'pStructMount.Slewing = AMount.Slewing
                'pStructMount.UTCDate = AMount.UTCDate
                'pStructMount.CanMoveAxisRA = AMount.CanMoveAxis(0)
                'pStructMount.CanMoveAxisDEC = AMount.CanMoveAxis(1)


                'handle visual aspect
                FrmMain.lblMountAlt.Text = "Alt " + pAUtil.DegreesToDMS(pStructMount.Altitude)
                FrmMain.lblMountAz.Text = "Az " + pAUtil.DegreesToDMS(pStructMount.Azimuth)

                'driveSidereal	0	Sidereal tracking rate (15.041 arcseconds per second).
                'driveLunar  1	Lunar tracking rate (14.685 arcseconds per second).
                'driveSolar  2	Solar tracking rate (15.0 arcseconds per second).
                'driveKing   3	King tracking rate (15.0369 arcseconds per second). 


                If pStructMount.AtPark = True Then
                    FrmMain.LblMountStatus.Text = "Parked"
                    FrmMain.LblMountStatus.BackColor = ColorTranslator.FromHtml("#d63031") 'red
                Else
                    If pStructMount.Tracking = True Then
                        If pStructMount.TrackingRate = 0 Then
                            FrmMain.LblMountStatus.Text = "Tracking: Sidereal"
                            FrmMain.LblMountStatus.BackColor = ColorTranslator.FromHtml("#4cd137") 'green
                        ElseIf pStructMount.TrackingRate = 1 Then
                            FrmMain.LblMountStatus.Text = "Tracking: Lunar"
                            FrmMain.LblMountStatus.BackColor = Color.Yellow
                        ElseIf pStructMount.TrackingRate = 2 Then
                            FrmMain.LblMountStatus.Text = "Tracking: Solar"
                            FrmMain.LblMountStatus.BackColor = Color.Yellow
                        ElseIf pStructMount.TrackingRate = 3 Then
                            FrmMain.LblMountStatus.Text = "Tracking: King"
                            FrmMain.LblMountStatus.BackColor = ColorTranslator.FromHtml("#4cd137") 'green
                        End If
                    Else
                        FrmMain.LblMountStatus.Text = "Not tracking"
                        FrmMain.LblMountStatus.BackColor = ColorTranslator.FromHtml("#d63031") 'red
                    End If
                End If

                FrmMain.LblMountRADEC.Text = "RA: " + pAUtil.HoursToHMS(pStructMount.RightAscension, "h ", "m ", "s ") + " DEC: " + pAUtil.DegreesToDMS(pStructMount.Declination, "° ", "' ", """ ")

                'pierEast    0	Normal pointing state - Mount on the East side of pier (looking West)
                'pierUnknown -1	Unknown Or indeterminate.
                'pierWest    1	Through the pole pointing state - Mount on the West side of pier (looking East) 

                If Convert.ToInt32(pStructMount.SideOfPier) = -1 Then
                    FrmMain.LblMountPierSide.Text = "Unkown pier side"
                ElseIf Convert.ToInt32(pStructMount.SideOfPier) = 0 Then
                    FrmMain.LblMountPierSide.Text = "Tube East of pier, looking West"
                ElseIf Convert.ToInt32(pStructMount.SideOfPier) = 1 Then
                    FrmMain.LblMountPierSide.Text = "Tube West of pier, looking East"
                End If

                'AUtil.Dispose()

                executionTime = DateTime.UtcNow() - startExecution
                LogSessionEntry("DEBUG", "  CheckMountStatus: " + executionTime.ToString, "", "CheckMountStatus", "MOUNT")

            Catch ex As Exception
                CheckMountStatus = "CheckMountStatus: " + ex.Message
            pStructMount.AtPark = False
            LogSessionEntry("ERROR", "CheckMountStatus: " + ex.Message, "", "CheckMountStatus", "MOUNT")
        End Try
    End Function

    'code om montering in batch aan en af te zetten
    ' http://user:secret@172.20.7.49/?cmd=5&p=1&a1=1&a2=0&s=2

    Public Function MountMoveAxis(vDirection As String, vSpeed As Double) As String
        Dim returnvalue As String
        Dim startExecution As Date
        Dim executionTime As TimeSpan

        MountMoveAxis = "OK"
        Try
            startExecution = DateTime.UtcNow()
            LogSessionEntry("DEBUG", "  MountMoveAxis...", "", "MountMoveAxis", "MOUNT")

            'will move indefinatly untill rate is reset to zero, when moving, is slewing is true
            If pMount.Slewing = False Then
                'axisPrimary	0	Primary axis (e.g., Right Ascension or Azimuth).
                'axisSecondary   1	Secondary axis (e.g., Declination Or Altitude).
                'axisTertiary    2	Tertiary axis (e.g. imager rotator/de-rotator). 
                If vDirection = "W" Then
                    pMount.MoveAxis(CType(0, ASCOM.DeviceInterface.TelescopeAxes), vSpeed)
                ElseIf vDirection = "E" Then
                    pMount.MoveAxis(CType(0, ASCOM.DeviceInterface.TelescopeAxes), vSpeed * -1)
                ElseIf vDirection = "N" Then
                    pMount.MoveAxis(CType(1, ASCOM.DeviceInterface.TelescopeAxes), vSpeed)
                ElseIf vDirection = "S" Then
                    pMount.MoveAxis(CType(1, ASCOM.DeviceInterface.TelescopeAxes), vSpeed * -1)
                End If
                LogSessionEntry("FULL", "Mount: moveAxis: " + vDirection + "...", "", "MountMoveAxis", "MOUNT")
            ElseIf pMount.Slewing = True Then
                If vDirection = "W" Then
                    pMount.MoveAxis(CType(0, ASCOM.DeviceInterface.TelescopeAxes), 0)
                ElseIf vDirection = "E" Then
                    pMount.MoveAxis(CType(0, ASCOM.DeviceInterface.TelescopeAxes), 0)
                ElseIf vDirection = "N" Then
                    pMount.MoveAxis(CType(1, ASCOM.DeviceInterface.TelescopeAxes), 0)
                ElseIf vDirection = "S" Then
                    pMount.MoveAxis(CType(1, ASCOM.DeviceInterface.TelescopeAxes), 0)
                End If
                LogSessionEntry("FULL", "Mount: moveAxis: stop...", "", "MountMoveAxis", "MOUNT")
            End If

            'fill the fields on the main form
            returnvalue = CheckMountStatus()
            If returnvalue <> "OK" Then
                MountMoveAxis = returnvalue
                Exit Function
            End If
            executionTime = DateTime.UtcNow() - startExecution
            LogSessionEntry("DEBUG", "  MountMoveAxis: " + executionTime.ToString, "", "MountMoveAxis", "MOUNT")

        Catch ex As Exception
            MountMoveAxis = "MountMoveAxis: " + ex.Message
            LogSessionEntry("ERROR", "Mount: moveAxis: " + ex.Message, "", "MountMoveAxis", "MOUNT")
        End Try
    End Function

    Public Function MountSlewToTarget(vTarget As String, vRATopocentric As Double, vDECTopocentric As Double, vRATopocentric_String As String, vDECTopocentric_String As String, vRA2000_string As String, vDEC2000_string As String, vShowMessages As Boolean, vManual As Boolean) As String
        'needs aborting capability
        Dim returnvalue As String
        Dim startExecution As Date
        Dim executionTime As TimeSpan

        MountSlewToTarget = "OK"
        Try
            startExecution = DateTime.UtcNow()
            LogSessionEntry("DEBUG", "  MountSlewToTarget...", "", "MountSlewToTarget", "MOUNT")

            FrmMain.Cursor = Cursors.WaitCursor
            pIsActionRunning = True
            startExecution = DateTime.UtcNow()

            ' if run needs to abort
            If (pAbort = True And vManual = False) Or pToolsAbort = True Then
                pIsActionRunning = False
                FrmMain.Cursor = Cursors.Default
                MountSlewToTarget = "SLEW_ABORTED"
                Exit Function
            End If

            If pRoofShutterStatus <> "OPEN" And My.Settings.sDisableSafetyCheck = False And My.Settings.sRoofDevice <> "NONE" Then
                'roof is not open, do not slew
                'not safe do nothing
                LogSessionEntry("ERROR", "Slewing to " + vTarget + " failed: the roof is not open!", "", "MountSlewToTarget", "MOUNT")
            Else
                'roof is open, slewing can be done

                ' if run needs to abort
                If (pAbort = True And vManual = False) Or pToolsAbort = True Then
                    pIsActionRunning = False
                    FrmMain.Cursor = Cursors.Default
                    MountSlewToTarget = "SLEW_ABORTED"
                    Exit Function
                End If

                pClosedLoopSlew = "SLEW"

                'Raises an error if AtPark is True, or if Tracking is False. 
                If pMount.AtPark = True Then
                    LogSessionEntry("BRIEF", "Unparking mount...", "", "MountSlewToTarget", "MOUNT")
                    pMount.Unpark()
                    LogSessionEntry("BRIEF", "Mount unparked...", "", "MountSlewToTarget", "MOUNT")
                End If

                If pMount.Tracking = False And pMount.CanSetTracking = True Then '1.98
                    LogSessionEntry("BRIEF", "Enabling tracking...", "", "MountSlewToTarget", "MOUNT")
                    pMount.Tracking = True
                    LogSessionEntry("BRIEF", "Tracking enabled.", "", "MountSlewToTarget", "MOUNT")
                End If

                If vShowMessages = True Then
                    'calculate alt az
                    returnvalue = CalculateObject(vRATopocentric, vDECTopocentric)
                    If returnvalue <> "OK" Then
                        MountSlewToTarget = returnvalue
                        Exit Function
                    End If

                    LogSessionEntry("BRIEF", "Slewing to " + vTarget + " target RA " + vRATopocentric_String + " - DEC " + vDECTopocentric_String + " / Alt " + Format(pStructObject.ObjectAlt, "#0.00") + "° - Az " + Format(pStructObject.ObjectAz, "#0.00") + "°", "", "MountSlewToTarget", "MOUNT")
                    'LogSessionEntry("BRIEF", "Slewing to " + vTarget + " RA " + vRA2000_string + " - DEC " + vDEC2000_string + " / Alt " + Format(pStructObject.ObjectAlt, "#0.00") + "° - Az " + Format(pStructObject.ObjectAz, "#0.00") + "°", "", "MountSlewToTarget", "MOUNT")
                End If

                'if target is below horizon: abort !
                returnvalue = CalculateObject(vRATopocentric, vDECTopocentric)
                If returnvalue <> "OK" Then
                    pIsActionRunning = False
                    pClosedLoopSlew = ""
                    FrmMain.Cursor = Cursors.Default
                    MountSlewToTarget = returnvalue
                    Exit Function
                End If

                If pStructObject.ObjectAlt > 10 Then
                    'move to RA / DEC in topocentric coordindates

                    pMount.TargetRightAscension = vRATopocentric
                    pMount.TargetDeclination = vDECTopocentric

                    ' if run needs to abort
                    If (pAbort = True And vManual = False) Or pToolsAbort = True Then
                        pIsActionRunning = False
                        FrmMain.Cursor = Cursors.Default
                        MountSlewToTarget = "SLEW_ABORTED"
                        Exit Function
                    End If

                    pMount.SlewToTargetAsync()
                    Do While pMount.Slewing = True
                        Thread.Sleep(250)
                        Application.DoEvents()

                        ' if run needs to abort
                        If (pAbort = True And vManual = False) Or pToolsAbort = True Then
                            returnvalue = MountAbortSlew()
                            pIsActionRunning = False
                            FrmMain.Cursor = Cursors.Default
                            MountSlewToTarget = "SLEW_ABORTED"
                            Exit Function
                        End If

                        If startExecution.AddSeconds(My.Settings.sMountTimeout) < DateTime.UtcNow() Then
                            LogSessionEntry("ERROR", "Mount slew timeout!", "MountSlewToTarget", "", "MOUNT")
                            returnvalue = PauseRun("ERROR: Mount slew timeout: PAUSING EQUIPMENT...", "",
                                                       "ERROR: Mount slew timeout: PauseEquipment: ", "",
                                                       "ERROR: equipment paused.", "", "PAUSING", "WAITING")
                            MountSlewToTarget = "TIMEOUT"
                            Exit Function
                        End If
                    Loop

                    If vShowMessages = True Then
                        Dim MountRA, MountDEC As String

                        'update mount coordinates
                        returnvalue = CheckMountStatus()
                        If returnvalue <> "OK" Then
                            FrmMain.Cursor = Cursors.Default
                            pIsActionRunning = False
                            MountSlewToTarget = returnvalue
                            Exit Function
                        End If
                        MountRA = pAUtil.HoursToHMS(pStructMount.RightAscension, "h ", "m ", "s ")
                        MountDEC = pAUtil.DegreesToDMS(pStructMount.Declination, "° ", "' ", """ ")

                        LogSessionEntry("BRIEF", "Slewed to " + vTarget + " actual RA " + MountRA + " - DEC " + MountDEC + " - Moon Alt " + Format(pStructEventTimes.MoonAlt, "#0.00") + "°", "", "MountSlewToTarget", "MOUNT")
                    End If
                Else
                    MountSlewToTarget = "BELOWHORIZON"
                    LogSessionEntry("BRIEF", "Slew could not be completed! " + vTarget + " is below the horizon at " + Format(pStructObject.ObjectAlt, "0.00") + "°", "", "MountSlewToTarget", "MOUNT")
                End If

                executionTime = DateTime.UtcNow() - startExecution
                LogSessionEntry("DEBUG", "  MountSlewToTarget: " + executionTime.ToString, "", "MountSlewToTarget", "MOUNT")

                'fill the fields on the main form
                returnvalue = CheckMountStatus()
                If returnvalue <> "OK" Then
                    FrmMain.Cursor = Cursors.Default
                    pIsActionRunning = False
                    MountSlewToTarget = returnvalue
                    Exit Function
                End If
                pClosedLoopSlew = ""
            End If
            pIsActionRunning = False
            FrmMain.Cursor = Cursors.Default

            executionTime = DateTime.UtcNow() - startExecution
            LogSessionEntry("DEBUG", "  MountSlewToTarget: " + executionTime.ToString, "", "MountSlewToTarget", "MOUNT")

        Catch ex As Exception
            FrmMain.Cursor = Cursors.Default
            MountSlewToTarget = "MountSlewToTarget: " + ex.Message
            LogSessionEntry("ERROR", "MountSlewToTarget: " + ex.Message, "", "MountSlewToTarget", "MOUNT")
            pIsActionRunning = False
            pClosedLoopSlew = ""
        End Try
    End Function

    Public Function MountAbortSlew() As String
        Dim returnvalue As String
        Dim startExecution As Date
        Dim executionTime As TimeSpan

        MountAbortSlew = "OK"
        Try
            startExecution = DateTime.UtcNow()
            LogSessionEntry("DEBUG", "  MountAbortSlew...", "", "MountAbortSlew", "MOUNT")

            FrmMain.Cursor = Cursors.WaitCursor

            If pMount.Slewing = True Then
                LogSessionEntry("FULL", "Aborting slew...", "", "MountAbortSlew", "MOUNT")
                pMount.AbortSlew()
                LogSessionEntry("BRIEF", "Slew aborted.", "", "MountAbortSlew", "MOUNT")
            End If
            pClosedLoopSlew = "ABORT"

            'fill the fields on the main form
            returnvalue = CheckMountStatus()
            If returnvalue <> "OK" Then
                FrmMain.Cursor = Cursors.Default
                MountAbortSlew = returnvalue
                Exit Function
            End If
            FrmMain.Cursor = Cursors.Default

            executionTime = DateTime.UtcNow() - startExecution
            LogSessionEntry("DEBUG", "  MountAbortSlew: " + executionTime.ToString, "", "MountAbortSlew", "MOUNT")

        Catch ex As Exception
            FrmMain.Cursor = Cursors.Default
            MountAbortSlew = "MountAbortSlew: " + ex.Message
            LogSessionEntry("ERROR", "MountAbortSlew: " + ex.Message, "", "MountAbortSlew", "MOUNT")
        End Try
    End Function

    Public Function MountPark() As String
        Dim returnvalue As String
        Dim startExecution As Date
        Dim executionTime As TimeSpan

        MountPark = "OK"
        Try
            startExecution = DateTime.UtcNow()
            LogSessionEntry("DEBUG", "  MountPark...", "", "ResetCalculateEventTimes", "MOUNT")

            FrmMain.Cursor = Cursors.WaitCursor
            pIsActionRunning = True

            If pMount.Slewing = True And pIsMountParking = False Then
                pMount.AbortSlew()
            End If

            If pIsMountParking = False Then
                pIsMountParking = True
                LogSessionEntry("BRIEF", "Parking mount...", "", "MountPark", "MOUNT")
                AsyncMountPark()

                While pMount.AtPark = False
                    Application.DoEvents()
                    If startExecution.AddSeconds(My.Settings.sMountTimeout) < DateTime.UtcNow() Then
                        LogSessionEntry("ERROR", "Mount parking timeout!", "", "MountPark", "MOUNT")
                        returnvalue = PauseRun("ERROR: Mount parking timeout: PAUSING EQUIPMENT...", "",
                                                       "ERROR: Mount parking timeout: PauseEquipment: ", "",
                                                       "ERROR: equipment paused.", "", "PAUSING", "WAITING")
                        MountPark = "TIMEOUT"
                        Exit Function
                    End If
                End While
                pIsMountParking = False
                LogSessionEntry("BRIEF", "Mount parked.", "", "MountPark", "MOUNT")
            Else
                LogSessionEntry("BRIEF", "Mount is already parking...", "", "MountPark", "MOUNT")
            End If

            'fill the fields on the main form
            returnvalue = CheckMountStatus()
            If returnvalue <> "OK" Then
                pIsActionRunning = False
                FrmMain.Cursor = Cursors.Default
                MountPark = returnvalue
                Exit Function
            End If

            pIsActionRunning = False

            FrmMain.Cursor = Cursors.Default

            executionTime = DateTime.UtcNow() - startExecution
            LogSessionEntry("DEBUG", "  MountPark: " + executionTime.ToString, "", "MountPark", "MOUNT")

        Catch ex As Exception
            FrmMain.Cursor = Cursors.Default
            pIsActionRunning = False
            MountPark = "MountPark: " + ex.Message
            LogSessionEntry("ERROR", "MountPark: " + ex.Message, "", "MountPark", "MOUNT")
        End Try
    End Function

    Async Sub AsyncMountPark()
        Try
            Dim result As Integer = Await Task.Run(Function()
                                                       pMount.Park()
                                                       Return 1
                                                   End Function).ConfigureAwait(True)
        Catch ex As Exception
            LogSessionEntry("ERROR", "AsyncMountPark: " + ex.Message, "", "AsyncMountPark", "MOUNT")
        End Try
    End Sub


    Public Function MountUnpark() As String
        Dim returnvalue As String
        Dim startExecution As Date
        Dim executionTime As TimeSpan

        MountUnpark = "OK"
        Try
            startExecution = DateTime.UtcNow()
            LogSessionEntry("DEBUG", "  MountUnpark...", "", "MountUnpark", "MOUNT")

            pIsActionRunning = True
            FrmMain.Cursor = Cursors.WaitCursor

            If pMount.Slewing = False Then
                LogSessionEntry("BRIEF", "Unparking mount...", "", "MountUnpark", "MOUNT")
                pMount.Unpark()
                LogSessionEntry("BRIEF", "Mount unparked.", "", "MountUnpark", "MOUNT")
            End If

            'fill the fields on the main form
            returnvalue = CheckMountStatus()
            If returnvalue <> "OK" Then
                pIsActionRunning = False
                FrmMain.Cursor = Cursors.Default
                MountUnpark = returnvalue
                Exit Function
            End If

            pIsActionRunning = False

            executionTime = DateTime.UtcNow() - startExecution
            LogSessionEntry("DEBUG", "  MountUnpark: " + executionTime.ToString, "", "MountUnpark", "MOUNT")

        Catch ex As Exception
            FrmMain.Cursor = Cursors.Default
            pIsActionRunning = False
            MountUnpark = "MountUnpark: " + ex.Message
            LogSessionEntry("ERROR", "MountUnpark: " + ex.Message, "", "MountUnpark", "MOUNT")
        End Try
    End Function

    Public Function MountEnableTracking() As String
        Dim returnvalue As String
        Dim startExecution As Date
        Dim executionTime As TimeSpan

        MountEnableTracking = "OK"
        Try
            startExecution = DateTime.UtcNow()
            LogSessionEntry("DEBUG", "  MountEnableTracking...", "", "MountEnableTracking", "MOUNT")

            If pMount.Tracking = False And pMount.CanSetTracking = True Then '1.98
                LogSessionEntry("BRIEF", "Enable tracking...", "", "MountEnableTracking", "MOUNT")
                pMount.Tracking = True
                LogSessionEntry("BRIEF", "Tracking enabled.", "", "MountEnableTracking", "MOUNT")
            End If


            'fill the fields on the main form
            returnvalue = CheckMountStatus()
            If returnvalue <> "OK" Then
                MountEnableTracking = returnvalue
                Exit Function
            End If

            executionTime = DateTime.UtcNow() - startExecution
            LogSessionEntry("DEBUG", "  MountEnableTracking: " + executionTime.ToString, "", "MountEnableTracking", "MOUNT")

        Catch ex As Exception
            MountEnableTracking = "MountEnableTracking: " + ex.Message
            LogSessionEntry("ERROR", "MountEnableTracking: " + ex.Message, "", "MountEnableTracking", "MOUNT")
        End Try
    End Function


    Public Function MountDisableTracking() As String
        Dim returnvalue As String
        Dim startExecution As Date
        Dim executionTime As TimeSpan

        MountDisableTracking = "OK"
        Try
            startExecution = DateTime.UtcNow()
            LogSessionEntry("DEBUG", "  MountDisableTracking...", "", "MountDisableTracking", "MOUNT")

            If pMount.Tracking = True And pMount.CanSetTracking = True Then '1.98
                LogSessionEntry("BRIEF", "Enable tracking...", "", "MountEnableTracking", "MOUNT")
                pMount.Tracking = False
                LogSessionEntry("BRIEF", "Tracking enabled.", "", "MountEnableTracking", "MOUNT")
            End If


            'fill the fields on the main form
            returnvalue = CheckMountStatus()
            If returnvalue <> "OK" Then
                MountDisableTracking = returnvalue
                Exit Function
            End If

            executionTime = DateTime.UtcNow() - startExecution
            LogSessionEntry("DEBUG", "  MountDisableTracking: " + executionTime.ToString, "", "MountDisableTracking", "MOUNT")

        Catch ex As Exception
            MountDisableTracking = "MountDisableTracking: " + ex.Message
            LogSessionEntry("ERROR", "MountDisableTracking: " + ex.Message, "", "MountDisableTracking", "MOUNT")
        End Try
    End Function

    Public Function MountSlewToAltAz(vAlt As Double, vAz As Double) As String
        'needs aborting capability
        Dim returnvalue As String
        Dim startExecution As Date
        Dim executionTime As TimeSpan

        MountSlewToAltAz = "OK"
        Try
            startExecution = DateTime.UtcNow()
            LogSessionEntry("DEBUG", "  MountSlewToAltAz...", "", "MountSlewToAltAz", "MOUNT")

            FrmMain.Cursor = Cursors.WaitCursor
            pIsActionRunning = True

            ' if run needs to abort
            If pAbort = True Then
                pIsActionRunning = False
                FrmMain.Cursor = Cursors.Default
                MountSlewToAltAz = "SLEW_ABORTED"
                Exit Function
            End If

            If pRoofShutterStatus <> "OPEN" And My.Settings.sDisableSafetyCheck = False And My.Settings.sRoofDevice <> "NONE" Then
                'roof is not open, do not slew
                'not safe do nothing
                LogSessionEntry("ERROR", "Slewing to Alt " + Format(vAlt, "#0.00") + "° Az " + Format(vAz, "#0.00") + "° failed: the roof is not open!", "", "MountSlewToAltAz", "MOUNT")
            Else
                'roof is open, slewing can be done
                ' if run needs to abort
                If pAbort = True Then
                    pIsActionRunning = False
                    FrmMain.Cursor = Cursors.Default
                    MountSlewToAltAz = "SLEW_ABORTED"
                    Exit Function
                End If

                'Raises an error if AtPark is True, or if Tracking is False. 
                If pMount.AtPark = True Then
                    LogSessionEntry("BRIEF", "Unparking mount...", "", "MountSlewToAltAz", "MOUNT")
                    pMount.Unpark()
                    LogSessionEntry("BRIEF", "Mount unparked...", "", "MountSlewToAltAz", "MOUNT")
                End If

                If pMount.Tracking = True And pMount.CanSetTracking = True Then '1.98
                    LogSessionEntry("BRIEF", "Disabling tracking...", "", "MountSlewToAltAz", "MOUNT")
                    pMount.Tracking = False
                    LogSessionEntry("BRIEF", "Tracking disabled.", "", "MountSlewToAltAz", "MOUNT")
                End If

                LogSessionEntry("BRIEF", "Slewing to Alt " + Format(vAlt, "#0.00") + "° Az " + Format(vAz, "#0.00") + "°", "", "MountSlewToAltAz", "MOUNT")
            End If

            pMount.SlewToAltAz(vAz, vAlt)

            Do While pMount.Slewing = True
                Thread.Sleep(250)
                Application.DoEvents()

                ' if run needs to abort
                If pAbort = True Then
                    returnvalue = MountAbortSlew()
                    pIsActionRunning = False
                    FrmMain.Cursor = Cursors.Default
                    MountSlewToAltAz = "SLEW_ABORTED"
                    Exit Function
                End If

                If startExecution.AddSeconds(My.Settings.sMountTimeout) < DateTime.UtcNow() Then
                    LogSessionEntry("ERROR", "Mount slew timeout!", "", "MountSlewToAltAz", "MOUNT")
                    returnvalue = PauseRun("ERROR: Mount slew timeout: PAUSING EQUIPMENT...", "",
                                                       "ERROR: Mount slew timeout: PauseEquipment: ", "",
                                                       "ERROR: equipment paused.", "", "PAUSING", "WAITING")
                    MountSlewToAltAz = "TIMEOUT"
                    Exit Function
                End If

            Loop

            LogSessionEntry("BRIEF", "Slewed to Alt " + Format(vAlt, "#0.00") + "° Az " + Format(vAz, "#0.00") + "° - Moon Alt " + Format(pStructEventTimes.MoonAlt, "#0.00") + "°", "", "MountSlewToAltAz", "MOUNT")

            'fill the fields on the main form
            returnvalue = CheckMountStatus()
            If returnvalue <> "OK" Then
                FrmMain.Cursor = Cursors.Default
                pIsActionRunning = False
                MountSlewToAltAz = returnvalue
                Exit Function
            End If

            pIsActionRunning = False
            FrmMain.Cursor = Cursors.Default

            executionTime = DateTime.UtcNow() - startExecution
            LogSessionEntry("DEBUG", "  MountSlewToAltAz: " + executionTime.ToString, "", "MountSlewToAltAz", "MOUNT")

        Catch ex As Exception
            FrmMain.Cursor = Cursors.Default
            MountSlewToAltAz = "MountSlewToAltAz: " + ex.Message
            LogSessionEntry("ERROR", "MountSlewToAltAz: " + ex.Message, "", "MountSlewToAltAz", "MOUNT")
            pIsActionRunning = False
        End Try
    End Function
End Module
