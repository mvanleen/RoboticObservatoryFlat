Imports System.IO
Imports System.Net.Security
Imports System.Security.Policy
Imports ASCOM.Astrometry
Imports ASCOM.DeviceInterface
Imports Google.Apis.Auth.OAuth2
Imports RoboticObs.ModAstroUtils
'Imports TheSky6Library

Module ModRun
    'will handle everything of the run

    Public pRunStatus As String 'STARTUP,ABORTING,ABORTED,DAY,SHUTDOWN,WAITING,FLATS,DEEPSKY,VARIABLES,UNSAFE,DONE,PAUSING,SMART
    Public pIsSequenceRunning As Boolean 'True when deepsky/variable/flats sequence is running, false when not running. Prevents starting multiple instances.
    Public pIsActionRunning As Boolean  'Set to true when action starts / ends. Example take image or park. In combination with abort.
    Public pAbort As Boolean 'Abort a sequence for whatever reason: true or false
    Public pManualAbort As Boolean 'If the STOP button is pushed, sequence should not recommence unless START button is pushed or checkbox is checked.
    Public pStartRun As Boolean 'Run is started or not.
    Public pEquipmentStatus As String 'Equipmentstatus ON / OFF / PAUSED / ERROR / TURNON / TURNOFF / PAUSING / PARTIAL (all on except roof)
    Public pIsEquipmentInitializing As Boolean   'True when a procedure for switching on/off equipment is running; must wait untill ready to do something else! Prevents starting multiple instances of TurnOn/TurnOff/PauseEquipment

    'safety check: constantly running
    Public pEquipmentSafetyStatusError As Boolean 'when running safetycheck: if anything was still connected TRUE = ERROR FALSE = everything was safe => only used for displaying messages
    Public pIsSafetyCheckRunning As Boolean 'True when safety check is running, false when not running, prevents starting multiple instances.
    Public pSmartTimeStamp As Date 'timestamp switch between safe / unsafe
    Public pSmartError As Boolean   'smart Error is active
    Public pLastLogLineTimeStamp As Date 'timestamp of last logline FULL / BRIEF / ERROR
    Public pToolsAbort As Boolean 'when abort is pushed on the Tools form


    'basic run scenario's
    ' moon = safe :
    '        all night long
    '        setting => after some time
    '        rising => at first okay but later not
    ' clear / clouded
    ' program: DEEPSKY / VARIABLES / DEEPSKY ELSE VARIABLES
    '   - only deepsky
    '   - only variables (do not take moon into account)
    '   - deepsky + switch to variables when moon is unsafe
    '
    '
    'abort procedure
    '----------------
    ' every procedure that needs to be aborted runs sequentially
    ' when abort is given pAbort = TRUE
    ' possible reasons: 
    '       - manually aborted
    '       - unsafe
    '       - switch to variables / deepsky
    '       - daybreak
    '       - disaster

    'other reasons:
    '       - no more targets
    'what needs to be done
    '       - manual abort: nothing
    '       - unsafe / disaster : pause
    '       - daybreak : shutdown
    '       - switch : nothing

    ' disaster error handling
    '--------------------------
    ' - runs constantly day and night when NO SEQUENCE IS RUNNING
    '      - day: roof cannot be open / mount cannot track

    ' smart error handling
    '----------------------
    '   -ALARM: Weather is safe, sequence running but the roof is not open
    '   -ALARM: Weather is safe, sequence running, mount unparked but the roof is not open
    '   -ALARM: Weather is safe, sequence running but the mount is not tracking nor at park position
    '   -ALARM: Weather is safe, sequence hangs and is not waiting
    '   -ALARM: Weather is UNSAFE, delay is passed; mount is not yet parked!"
    '   -ALARM: Weather is UNSAFE, delay is passed; roof is not yet closed!"


    Public Function TurnOnEquipment(vOpenRoof As Boolean) As String
        'turn on equipment
        'if an error would occur, abort the run as soon as possible
        Dim returnvalue As String

        TurnOnEquipment = "OK"
        Try
            pRunStatus = "STARTUP"
            LogSessionEntry("DEBUG", "pRunStatus set to STARTUP.", "", "CheckRun", "SEQUENCE")
            pEquipmentStatus = "TURNON"
            'LogSessionEntry("FULL", "Startup: turning on power.", "TurnOnEquipment", "PROGRAM") double message
            pIsEquipmentInitializing = True

            '---------------------------------------------
            ' Switching on equipment
            '---------------------------------------------
            'when equipment is not yet turned on
            If pSwitchEquipmentOnOff = "OFF" Then
                'turn on equipment
                returnvalue = SwitchEquipmentOn()
                If returnvalue <> "OK" Then
                    pEquipmentStatus = "ERROR"
                    pIsEquipmentInitializing = False
                    AbortRun()
                    TurnOnEquipment = returnvalue
                    Exit Function
                End If
                'Else
                '   LogSessionEntry("FULL", "Equipment already switched on...", "TurnOnEquipment", "PROGRAM")
            End If

            '---------------------------------------------
            ' Switching on mount
            '---------------------------------------------
            If pMountConnected = False Then
                returnvalue = MountConnect()
                If returnvalue <> "OK" Then
                    pEquipmentStatus = "ERROR"
                    pIsEquipmentInitializing = False
                    TurnOnEquipment = returnvalue
                End If
            Else
                LogSessionEntry("FULL", "Mount already connected.", "", "TurnOnEquipment", "PROGRAM")
            End If
            WaitSeconds(2, False, True)

            If pMountConnected = True Then
                returnvalue = CheckMountStatus()
                If returnvalue <> "OK" Then
                    pEquipmentStatus = "ERROR"
                    pIsEquipmentInitializing = False
                    AbortRun()
                    TurnOnEquipment = returnvalue
                    Exit Function
                End If
            Else
                LogSessionEntry("ERROR", "Mount is not connected!", "", "TurnOnEquipment", "PROGRAM")
                AbortRun()
            End If

            '---------------------------------------------
            ' Switching on the Cover
            '---------------------------------------------
            If My.Settings.sCoverMethod = "NONE" Then
                'do nothing
            Else
                If pCoverConnected = False Then
                    returnvalue = CoverConnect()
                    If returnvalue <> "OK" Then
                        pEquipmentStatus = "ERROR"
                        pIsEquipmentInitializing = False
                        TurnOnEquipment = returnvalue
                        Exit Function
                    End If
                    WaitSeconds(2, False, True)

                    returnvalue = CoverGetStatus(True)
                    If returnvalue <> "OK" Then
                        pEquipmentStatus = "ERROR"
                        pIsEquipmentInitializing = False
                        AbortRun()
                        TurnOnEquipment = returnvalue
                        Exit Function
                    End If
                End If

                If (pCoverStatus = 1 Or pCoverStatus = 2) Then
                    'park the mount if not parked
                    If pStructMount.AtPark <> True Then
                        returnvalue = MountPark()
                        If returnvalue <> "OK" Then
                            pEquipmentStatus = "ERROR"
                            pIsEquipmentInitializing = False
                            AbortRun()
                            TurnOnEquipment = returnvalue
                            Exit Function
                        End If
                    End If

                    returnvalue = CoverOpen()
                    If returnvalue <> "OK" Then
                        pEquipmentStatus = "ERROR"
                        pIsEquipmentInitializing = False
                        AbortRun()
                        TurnOnEquipment = returnvalue
                        Exit Function
                    End If
                Else
                    LogSessionEntry("BRIEF", "Cover already open.", "", "TurnOnEquipment", "PROGRAM")
                End If

                'check cover status
                returnvalue = CoverGetStatus(True)
                If returnvalue <> "OK" Then
                    pEquipmentStatus = "ERROR"
                    pIsEquipmentInitializing = False
                    AbortRun()
                    TurnOnEquipment = returnvalue
                    Exit Function
                End If

                If (pCoverStatus <> 3) Then
                    LogSessionEntry("ERROR", "Cover did not open!", "", "TurnOnEquipment", "PROGRAM")
                    AbortRun()
                End If
            End If


            '---------------------------------------------
            'Turn on CCD
            '---------------------------------------------
            If pTheSkyXEquipmentConnected = False Then
                returnvalue = ConnectTheSkyXEquipment(True)
                If returnvalue <> "OK" Then
                    pEquipmentStatus = "ERROR"
                    pIsEquipmentInitializing = False
                    AbortRun()
                    TurnOnEquipment = returnvalue
                    Exit Function
                End If
            End If

            '---------------------------------------------
            ' Open roof
            '---------------------------------------------
            If vOpenRoof = True And My.Settings.sRoofDevice <> "NONE" Then

                If pRoofShutterStatus = "" Then
                    returnvalue = RoofShutterStatus()
                    If returnvalue <> "OK" Then
                        pEquipmentStatus = "ERROR"
                        pIsEquipmentInitializing = False
                        AbortRun()
                        TurnOnEquipment = returnvalue
                        Exit Function
                    End If
                End If

                If (pRoofShutterStatus <> "OPEN") Then
                    returnvalue = RoofOpenRoof()
                    If returnvalue <> "OK" Then
                        pEquipmentStatus = "ERROR"
                        pIsEquipmentInitializing = False
                        AbortRun()
                        TurnOnEquipment = returnvalue
                        Exit Function
                    End If
                Else
                    LogSessionEntry("BRIEF", "Roof already open.", "", "TurnOnEquipment", "PROGRAM")
                End If

                ' check if the roof is really open
                returnvalue = RoofShutterStatus()
                If returnvalue <> "OK" Then
                    pEquipmentStatus = "ERROR"
                    pIsEquipmentInitializing = False
                    AbortRun()
                    TurnOnEquipment = returnvalue
                    Exit Function
                End If

                If pRoofShutterStatus <> "OPEN" Then
                    LogSessionEntry("ERROR", "Roof did not open!", "", "TurnOnEquipment", "PROGRAM")
                    AbortRun()
                End If
            End If

            'done switching everything on
            pIsEquipmentInitializing = False

            If vOpenRoof = True Then
                pEquipmentStatus = "ON"
            Else
                pEquipmentStatus = "PARTIAL"
            End If

            pRunStatus = "NOT RUNNING"
            LogSessionEntry("BRIEF", "STARTUP: power turned on.", "", "TurnOnEquipment", "PROGRAM")

        Catch ex As Exception
            TurnOnEquipment = "TurnOnEquipment: " + ex.Message
            pEquipmentStatus = "ERROR"
            pIsEquipmentInitializing = False
            AbortRun()
            LogSessionEntry("ERROR", "TurnOnEquipment: " + ex.Message, "", "TurnOnEquipment", "PROGRAM")
        End Try
    End Function

    Public Function TurnOffEquipment() As String
        'turnoff equipment, on error continue with shutdown
        Dim returnvalue As String

        TurnOffEquipment = "OK"
        Try
            pRunStatus = "SHUTDOWN"
            LogSessionEntry("BRIEF", "SHUTDOWN: turning off equipment...", "", "TurnOffEquipment", "PROGRAM")
            pIsEquipmentInitializing = True
            pEquipmentStatus = "TURNOFF"

            'park the mount
            If pMountConnected = True Then
                returnvalue = MountPark()
                If returnvalue <> "OK" Then
                    pEquipmentStatus = "ERROR"
                    TurnOffEquipment = returnvalue '"ERROR PARKING MOUNT"
                End If
            Else
                LogSessionEntry("BRIEF", "Mount not connected.", "", "TurnOffEquipment", "PROGRAM")
            End If

            'close the roof
            If My.Settings.sRoofDevice = "NONE" Then
                'do nothing, no roof defined
            Else
                If pRoofShutterStatus = "" Then
                    returnvalue = RoofShutterStatus()
                    If returnvalue <> "OK" Then
                        pEquipmentStatus = "ERROR"
                        TurnOffEquipment = returnvalue '"ERROR QUERY SHUTTER STATUS"
                    End If
                End If
                If pRoofShutterStatus = "OPEN" Or pRoofShutterStatus = "OPENING" Then
                    returnvalue = RoofCloseRoof()
                    If returnvalue <> "OK" Then
                        pEquipmentStatus = "ERROR"
                        TurnOffEquipment = returnvalue '"ERROR CLOSING ROOF"
                    End If
                Else
                    LogSessionEntry("BRIEF", "Roof already closed.", "", "TurnOffEquipment", "PROGRAM")
                End If
            End If

            'close Cover                
            If pCoverConnected = True Then
                'close Cover, but only when not yet closed
                If pCoverStatus <> 1 Then
                    returnvalue = CoverClose()
                    If returnvalue <> "OK" Then
                        pEquipmentStatus = "ERROR"
                        TurnOffEquipment = returnvalue '"ERROR CLOSING COVER"
                    End If
                End If

                'disconnect Cover
                returnvalue = CoverDisconnect()
                pCoverConnected = False
                If returnvalue <> "OK" Then
                    pEquipmentStatus = "ERROR"
                    TurnOffEquipment = returnvalue '"ERROR DISCONNECT COVER"
                End If
            Else
                LogSessionEntry("BRIEF", "Cover not connected.", "", "TurnOffEquipment", "PROGRAM")
            End If

            'turn off the mount
            If pMountConnected = True Then
                returnvalue = MountDisconnect()
                If returnvalue <> "OK" Then
                    pEquipmentStatus = "ERROR"
                    TurnOffEquipment = returnvalue '"ERROR TURNING OFF MOUNT"
                End If
            Else
                LogSessionEntry("BRIEF", "Mount already turned off.", "", "TurnOffEquipment", "PROGRAM")
            End If

            'disconnect the CCD
            If pTheSkyXEquipmentConnected = True Then
                returnvalue = DisconnectTheSkyXEquipment()
                If returnvalue <> "OK" Then
                    pEquipmentStatus = "ERROR"
                    TurnOffEquipment = returnvalue '"NOK"
                End If
            Else
                LogSessionEntry("BRIEF", "CCD already disconnected.", "", "PauseEquipment", "PROGRAM")
            End If

            'turn off switches equipment
            If pSwitchAnyOn = True Then
                returnvalue = SwitchEquipmentOff()
                If returnvalue <> "OK" Then
                    pEquipmentStatus = "ERROR"
                    TurnOffEquipment = returnvalue '"ERROR SWITCHING OFF EQUIPMENT"
                End If
            Else
                LogSessionEntry("BRIEF", "Equipment already switched off.", "", "TurnOffEquipment", "PROGRAM")
            End If

            pIsEquipmentInitializing = False

            pRunStatus = "NOT RUNNING"
            If pEquipmentStatus <> "ERROR" Then
                pEquipmentStatus = "OFF"
                LogSessionEntry("BRIEF", "Shutdown: equipment turned off.", "", "TurnOffEquipment", "PROGRAM")
                TurnOffEquipment = "OK"
            Else
                LogSessionEntry("ERROR", "Shutdown: equipment turned off with ERRORS!", "", "TurnOffEquipment", "PROGRAM")
            End If

        Catch ex As Exception
            TurnOffEquipment = "TurnOffEquipment: " + ex.Message
            pEquipmentStatus = "ERROR"
            pIsEquipmentInitializing = False
            LogSessionEntry("ERROR", "TurnOffEquipment: " + ex.Message, "", "TurnOffEquipment", "PROGRAM")
        End Try
    End Function

    Public Function PauseEquipment() As String
        'pause equipment, continue pausing on error 
        Dim returnvalue As String

        PauseEquipment = "OK"
        Try
            LogSessionEntry("ESSENTIAL", "Pausing: parking mount and closing roof.", "", "PauseEquipment", "PROGRAM")

            pIsEquipmentInitializing = True
            pEquipmentStatus = "PAUSING"

            'park the mount
            If pMountConnected = True Then
                returnvalue = MountPark()
                If returnvalue <> "OK" Then
                    pEquipmentStatus = "ERROR"
                    PauseEquipment = returnvalue '"ERROR PARKING MOUNT"
                End If
            Else
                LogSessionEntry("BRIEF", "Mount not connected.", "", "PauseEquipment", "PROGRAM")
            End If

            'close the roof
            If My.Settings.sRoofDevice = "NONE" Then
                'do nothing, no roof defined
            Else
                If pRoofShutterStatus = "" Then
                    returnvalue = RoofShutterStatus()
                    If returnvalue <> "OK" Then
                        pEquipmentStatus = "ERROR"
                        PauseEquipment = returnvalue
                    End If
                End If

                If pRoofShutterStatus = "OPEN" Or pRoofShutterStatus = "OPENING" Then
                    returnvalue = RoofCloseRoof()
                    If returnvalue <> "OK" Then
                        pEquipmentStatus = "ERROR"
                        PauseEquipment = returnvalue
                    End If
                Else
                    LogSessionEntry("BRIEF", "Roof already closed.", "", "PauseEquipment", "PROGRAM")
                End If
            End If

            'disconnect the CCD
            If pTheSkyXEquipmentConnected = True Then
                returnvalue = DisconnectTheSkyXEquipment()
                If returnvalue <> "OK" Then
                    pEquipmentStatus = "ERROR"
                    PauseEquipment = returnvalue
                    Exit Function
                End If
            Else
                LogSessionEntry("BRIEF", "CCD already disconnected.", "", "PauseEquipment", "PROGRAM")
            End If

            pIsEquipmentInitializing = False


            If pEquipmentStatus <> "ERROR" Then
                pEquipmentStatus = "PAUSED"
                LogSessionEntry("BRIEF", "Paused: mount parked, roof closed.", "", "PauseEquipment", "PROGRAM")
                PauseEquipment = "OK"
            Else
                LogSessionEntry("ERROR", "Pause: equipment paused with ERRORS!", "", "PauseEquipment", "PROGRAM")
            End If

        Catch ex As Exception
            PauseEquipment = "PauseEquipment: " + ex.Message
            pIsEquipmentInitializing = False
            LogSessionEntry("ERROR", "PauseEquipment: " + ex.Message, "", "PauseEquipment", "PROGRAM")
        End Try
    End Function


    Public Function CheckEquipmentStatus() As String
        'only used when starting the program to determine the actual state of the equipment
        Dim returnvalue As String

        CheckEquipmentStatus = "OK"
        Try
            'switch
            returnvalue = CheckSwitch()
            If pSwitchAnyOn = False Then
                LogSessionEntry("FULL", "All equipment is turned off.", "", "CheckEquipmentStatus", "PROGRAM")
            Else
                LogSessionEntry("FULL", "Some switches are on...", "", "CheckEquipmentStatus", "PROGRAM")
                pEquipmentStatus = "PARTIAL"

                'mount
                LogSessionEntry("FULL", "Checking if mount is on...", "", "CheckEquipmentStatus", "PROGRAM")
                If My.Settings.sSimulatorMode = False Then
                    If My.Settings.sMountDevice = "ASCOM.tenmicron_mount.Telescope" Then
                        pMount = New ASCOM.DriverAccess.Telescope("ASCOM.tenmicron_mount.Telescope")
                    ElseIf My.Settings.sMountDevice = "ASCOM.SoftwareBisque.Telescope" Then
                        pMount = New ASCOM.DriverAccess.Telescope("ASCOM.SoftwareBisque.Telescope")
                    Else
                        pMount = New ASCOM.DriverAccess.Telescope(My.Settings.sMountDevice)
                    End If
                    'see if we can connect
                    Try
                        pMount.Connected = True
                        pMountConnected = True
                    Catch ex2 As Exception
                        pMountConnected = False
                    End Try
                    If pMountConnected = True Then
                        returnvalue = CheckMountStatus()
                        LogSessionEntry("FULL", "Mount connected.", "", "CheckEquipmentStatus", "PROGRAM")
                    End If
                End If

                'Cover
                LogSessionEntry("FULL", "Checking if Cover is on...", "", "CheckEquipmentStatus", "PROGRAM")
                returnvalue = CoverGetStatus(False)
                If pCoverStatus = 1 Or pCoverStatus = 2 Or pCoverStatus = 3 Then
                    pCoverConnected = True
                    LogSessionEntry("FULL", "Cover connected.", "", "CheckEquipmentStatus", "PROGRAM")
                Else
                    pCoverConnected = False
                End If


                'TSX: try to connect the camera's
                LogSessionEntry("FULL", "Checking if camera and focusser are on...", "", "CheckEquipmentStatus", "PROGRAM")
                returnvalue = ConnectTheSkyXEquipment(False)

            End If
            LogSessionEntry("FULL", "Equipment check completed.", "", "CheckEquipmentStatus", "PROGRAM")
        Catch ex As Exception
            CheckEquipmentStatus = "CheckEquipmentStatus: " + ex.Message
            pEquipmentStatus = "ERROR"
            LogSessionEntry("ERROR", "CheckEquipmentStatus: " + ex.Message, "", "CheckEquipmentStatus", "PROGRAM")
        End Try
    End Function

    Public Function RunSafetyCheck(vSafetyTime As String) As String
        'will terminate all activity in the observatory, used for day conditions
        'absolute priority to close the roof, even mount park fails
        Dim returnvalue As String

        RunSafetyCheck = "OK"
        Try
            pEquipmentSafetyStatusError = False
            LogSessionEntry("FULL", "RUNNING SAFETY CHECK " + vSafetyTime, "", "RunSafetyCheck", "PROGRAM")

            If vSafetyTime = "DAY" And pIsSequenceRunning = False Then
                If pEquipmentStatus <> "OFF" And pEquipmentStatus <> "PAUSED" Then
                    LogSessionEntry("ESSENTIAL", "EXECUTING SAFETY MEASURES " + vSafetyTime, "", "RunSafetyCheck", "PROGRAM")
                    pIsSafetyCheckRunning = True
                    pIsEquipmentInitializing = True

                    'kill everything during the daytime

                    'park the mount
                    If pMountConnected = True Then
                        LogSessionEntry("ESSENTIAL", "SAFETY CHECK: parking mount...", "", "RunSafetyCheck", "PROGRAM")
                        returnvalue = MountPark()
                        pEquipmentSafetyStatusError = True

                        If returnvalue <> "OK" Then
                            RunSafetyCheck = "NOK"
                            pIsSafetyCheckRunning = False
                            pEquipmentSafetyStatusError = True
                            LogSessionEntry("BRIEF", "Mount not parking: continuing to close roof.", "", "RunSafetyCheck", "PROGRAM")
                            'if mount does not park: DO NOT EXIT THE PROCEDURE BUT CONTINUE WITH THE ROOF
                            'Exit Function
                        End If
                    End If

                    'close the roof
                    If My.Settings.sRoofDevice = "NONE" Then
                        'do nothing, no roof defined
                    Else
                        If pRoofShutterStatus = "" Then
                            returnvalue = RoofShutterStatus()
                            If returnvalue <> "OK" Then
                                RunSafetyCheck = "NOK"
                                pIsSafetyCheckRunning = False
                                pEquipmentSafetyStatusError = True
                                'Exit Function 'do not exit, attempt to close roof
                            End If
                        End If

                        If pRoofShutterStatus = "OPEN" Or pRoofShutterStatus = "OPENING" Then
                            LogSessionEntry("ESSENTIAL", "SAFETY CHECK: closing roof...", "", "RunSafetyCheck", "PROGRAM")
                            returnvalue = RoofCloseRoof()
                            pEquipmentSafetyStatusError = True

                            If returnvalue <> "OK" Then
                                pIsSafetyCheckRunning = False
                                pEquipmentSafetyStatusError = True
                                pIsEquipmentInitializing = False
                                RunSafetyCheck = returnvalue
                                Exit Function
                            End If
                        End If
                    End If

                    'close cover, CoverClose will check if mount is parked or not.                
                    If pCoverConnected = True Then
                        LogSessionEntry("ESSENTIAL", "SAFETY CHECK: closing Cover...", "", "RunSafetyCheck", "PROGRAM")
                        If pCoverStatus <> 1 Then
                            'close Cover
                            returnvalue = CoverClose()
                            pEquipmentSafetyStatusError = True

                            If returnvalue <> "OK" Then
                                pIsSafetyCheckRunning = False
                                pEquipmentSafetyStatusError = True
                                pIsEquipmentInitializing = False
                                RunSafetyCheck = returnvalue
                                Exit Function
                            End If
                        End If

                        'disconnect Cover
                        returnvalue = CoverDisconnect()
                        If returnvalue <> "OK" Then
                            pIsSafetyCheckRunning = False
                            pEquipmentSafetyStatusError = True
                            pIsEquipmentInitializing = False
                            RunSafetyCheck = returnvalue
                            Exit Function
                        End If
                    End If

                    'disconnect CCD
                    If pTheSkyXEquipmentConnected = True Then
                        returnvalue = DisconnectTheSkyXEquipment()
                        If returnvalue <> "OK" Then
                            pIsSafetyCheckRunning = False
                            pEquipmentSafetyStatusError = True
                            pIsEquipmentInitializing = False
                            RunSafetyCheck = returnvalue
                            Exit Function
                        End If
                    End If

                    'turn off switches equipment
                    If (pSwitchEquipmentOnOff <> "OFF" Or pSwitchAnyOn = True) And pSwitchingEquipmentOff = False Then
                        LogSessionEntry("ESSENTIAL", "SAFETY CHECK: turning off all switches...", "", "RunSafetyCheck", "PROGRAM")
                        returnvalue = SwitchEquipmentOff()
                        pEquipmentSafetyStatusError = True

                        If returnvalue <> "OK" Then
                            pIsSafetyCheckRunning = False
                            pEquipmentSafetyStatusError = True
                            pIsEquipmentInitializing = False
                            RunSafetyCheck = returnvalue
                            Exit Function
                        End If
                    End If
                    pEquipmentStatus = "OFF"
                End If

            Else
                If pEquipmentStatus <> "OFF" And pEquipmentStatus <> "PAUSED" Then
                    'only pause equiment when at night and no sequence is running
                    LogSessionEntry("ESSENTIAL", "RUNNING SAFETY CHECK " + vSafetyTime, "", "RunSafetyCheck", "PROGRAM")

                    'park the mount
                    If pMountConnected = True Then
                        returnvalue = MountPark()
                        pEquipmentSafetyStatusError = True

                        If returnvalue <> "OK" Then
                            pIsSafetyCheckRunning = False
                            pEquipmentSafetyStatusError = True
                            pIsEquipmentInitializing = False
                            'if mount does not park: DO NOT EXIT THE PROCEDURE BUT CONTINUE WITH THE ROOF
                            'RunSafetyCheck = returnvalue
                            'Exit Function
                            LogSessionEntry("BRIEF", "Mount not parking: continuing to close roof.", "", "RunSafetyCheck", "PROGRAM")
                        End If
                    Else
                        LogSessionEntry("BRIEF", "Mount not connected.", "", "RunSafetyCheck", "PROGRAM")
                    End If

                    'close the roof
                    If My.Settings.sRoofDevice = "NONE" Then
                        'do nothing, no roof defined
                    Else
                        If pRoofShutterStatus = "" Then
                            returnvalue = RoofShutterStatus()
                            If returnvalue <> "OK" Then
                                pIsSafetyCheckRunning = False
                                pEquipmentSafetyStatusError = True
                                pIsEquipmentInitializing = False
                                RunSafetyCheck = returnvalue
                                Exit Function
                            End If
                        End If

                        If pRoofShutterStatus = "OPEN" Or pRoofShutterStatus = "OPENING" Then
                            returnvalue = RoofCloseRoof()
                            pEquipmentSafetyStatusError = True

                            If returnvalue <> "OK" Then
                                pIsSafetyCheckRunning = False
                                pEquipmentSafetyStatusError = True
                                pIsEquipmentInitializing = False
                                RunSafetyCheck = returnvalue
                                Exit Function
                            End If
                        Else
                            LogSessionEntry("BRIEF", "Roof already closed.", "", "RunSafetyCheck", "PROGRAM")
                        End If
                    End If

                    'disconnect the CCD
                    If pTheSkyXEquipmentConnected = True Then
                        returnvalue = DisconnectTheSkyXEquipment()
                        If returnvalue <> "OK" Then
                            pIsSafetyCheckRunning = False
                            pEquipmentSafetyStatusError = True
                            pIsEquipmentInitializing = False
                            RunSafetyCheck = returnvalue
                            Exit Function
                        End If
                    Else
                        LogSessionEntry("BRIEF", "CCD already disconnected.", "", "RunSafetyCheck", "PROGRAM")
                    End If

                    pIsEquipmentInitializing = False
                    pEquipmentStatus = "PAUSED"

                    LogSessionEntry("BRIEF", "Paused: mount parked, roof closed.", "", "RunSafetyCheck", "PROGRAM")
                End If
            End If

            pIsSafetyCheckRunning = False
            pIsEquipmentInitializing = False

        Catch ex As Exception
            RunSafetyCheck = "RunSafetyCheck: " + ex.Message
            pIsSafetyCheckRunning = False
            pEquipmentSafetyStatusError = True
            pIsEquipmentInitializing = False
            LogSessionEntry("ERROR", "RunSafetyCheck: " + ex.Message, "", "RunSafetyCheck", "PROGRAM")
        End Try
    End Function



    Public Function CheckRun() As String
        'starts run       
        'HANDLE ABORT AND MANUAL ABORT
        'PHASE 1: DAY OR NIGHT: CHECK
        '   DAY: turn off and close roof
        '   NIGHT: SAFE OR UNSAFE
        '	NIGHT SAFE: DO RUN
        '		PHASE 2: COOLDOWN TELESCOPE
        '			MOON IS SAFE OR CAN BE IGNORED OR VARIABLES: COOLDOWN TELESCOPE
        '			MOON IS UNSAFE: PAUSE
        '		PHASE 3: DUSK FLATS
        '			MOON IS SAFE OR CAN BE IGNORED OR VARIABLES: DUSK FLATS NEEDED ?
        '				DUSK FLATS ARE NEEDED ELSE COOLDOWN
        '				DUSK FLATS NOT NEEDED: START COOLING
        '			MOON IS UNSAFE: PAUSE TELESCOPE
        '		PHASE 4: RUN SEQUENCE
        '			PART 1: FIRST TIME ONLY: CHECK DATABASE IF THERE ARE STILL TARGETS
        '			PART 2: DECIDE WHAT TO DO	
        '				OBSERVATION PROGRAM = VARIABLES
        '				OBSERVATION PROGRAM = DEEPSKY / DEEPSKY ELSE VARIABLES
        '				    MOON IS SAFE OR CAN BE IGNORED GENERAL / TARGET SPECIFIC: RUN DEEPSKY
        '					DEEPSKY TARGETS AVAILABLE: RUN DEEPSKY
        '					NO DEEPSKY TARGETS AVAILABLE: SWITCH TO VARIABLES OR PAUSE EQUIPMENT
        '						PROGRAM = DEEPSKY: DO NOTHING AND PAUSE EQUIPMENT
        '						PROGRAM = DEEPSKY ELSE VARIABLES: SWITCH TO VARIABLES
        '				    MOON IS UNSAFE AND CANNOT BE IGNORED: PAUSE OF RUN VARIABLES
        '						PROGRAM = DEEPSKY: PAUSE
        '						PROGRAM = DEEPSKY ELSE VARIABLES: RUN VARIABLES
        '		WAIT FOR FLATS
        '			FLATS NEEDED: ABORT AND WAIT
        '			NO FLATS NEEDED: STOP RUN COMPLETELY
        '		DAWN FLATS: ONLY WHEN EQUIPMENT IS ON
        '			FLATS NEEDED: RUN DAWN FLATS
        '			NO FLATS NEEDED: STOP RUN COMPLETELY
        '	NIGHT UNSAFE: PAUSE RUN 

        Dim returnvalue As String

        CheckRun = "OK"
        Try
            '--------------------------------------------------------------------------------------------------------
            'HANDLE ABORT AND MANUAL ABORT
            '--------------------------------------------------------------------------------------------------------
            If pAbort = True Then
                If pIsSequenceRunning = True Then
                    'abort the sequence
                    LogSessionEntry("ESSENTIAL", "ABORTING RUN", "", "CheckRun", "SEQUENCE")
                    pRunStatus = "ABORTING"
                    pIsSequenceRunning = False
                Else
                    If pManualAbort = True Then
                        'manual abort, do not pause equipment, wait for the start button to be pushed or the checkbox is changed                 
                        pStartRun = False 'do not restart run unless manual action of user was taken
                        FrmMain.BtnStartRun.Enabled = True
                        FrmMain.BtnStopRun.Enabled = False
                        pRunStatus = "ABORTED"
                        LogSessionEntry("ESSENTIAL", "RUN ABORTED MANUEL.", "", "CheckRun", "SEQUENCE")
                    ElseIf pSmartError = True Then
                        pStartRun = False 'do not restart run unless manual action of user was taken
                        pRunStatus = "SMART"
                        LogSessionEntry("DEBUG", "pRunStatus set to SMART.", "", "CheckRun", "SEQUENCE")
                        pSmartTimeStamp = CDate("01/01/0001")
                        If pIsEquipmentInitializing = True Then
                            'do nothing and wait until paused
                        Else
                            'manual abort, do not pause equipment, wait for the start button to be pushed or the checkbox is changed                 
                            LogSessionEntry("ERROR", "RUN STOPPED DUE TO SMART ERROR.", "", "CheckRun", "SEQUENCE")
                        End If
                    End If
                End If
            End If

            'start the run when needed
            If pStartRun = True Then
                '--------------------------------------------------------------------------------------------------------
                'PHASE 1: DAY OR NIGHT: CHECK
                '--------------------------------------------------------------------------------------------------------
                If (pStructEventTimes.SunAlt >= 0 And pStructEventTimes.SunSettingRising = "RISING") Or
                (pStructEventTimes.SunAlt > My.Settings.sSunOpenRoof And pStructEventTimes.SunSettingRising = "SETTING") Then
                    '--------------------------------------------------------------------------------------------------------
                    'DAY: turn off and close roof
                    '--------------------------------------------------------------------------------------------------------
                    'abort any ongoing sequence
                    If pIsSequenceRunning = True And pRunStatus <> "DAY" And pAbort = False Then
                        pAbort = True
                        pRunStatus = "ABORTING"
                        LogSessionEntry("ESSENTIAL", "DAY: aborting run... ", "Sun Alt " + Format(pStructEventTimes.SunAlt, "#0.00") + "°", "CheckRun", "SEQUENCE")
                    End If

                    'turn off or pause equipment
                    If My.Settings.sTurnOffCompletely = True And pEquipmentStatus <> "OFF" And pIsEquipmentInitializing = False And pIsActionRunning = False Then
                        LogSessionEntry("ESSENTIAL", "DAY: SHUTDOWN EQUIPMENT...", "", "CheckRun", "SEQUENCE")
                        'shutdown equipment
                        If pIsEquipmentInitializing = False Then
                            returnvalue = TurnOffEquipment()
                            If returnvalue <> "OK" Then
                                LogSessionEntry("ERROR", "DAY: TurnOffEquipment: " + returnvalue, "", "TurnOffEquipment", "SEQUENCE")
                                CheckRun = returnvalue
                                Exit Function
                            End If
                        End If
                    ElseIf My.Settings.sTurnOffCompletely = False And pEquipmentStatus <> "PAUSED" And pIsEquipmentInitializing = False And pIsActionRunning = False Then
                        'only pause equipment
                        LogSessionEntry("ESSENTIAL", "DAY: EQUIPMENT REMAINS SWITCHED ON FOR VENTILATION PURPOSES...", "", "CheckRun", "SEQUENCE")
                        If pIsEquipmentInitializing = False Then
                            returnvalue = PauseEquipment()
                            If returnvalue <> "OK" Then
                                LogSessionEntry("ERROR", "DAY: PauseEquipment: " + returnvalue, "", "PauseEquipment", "SEQUENCE")
                                CheckRun = returnvalue
                                Exit Function
                            End If
                        End If
                    End If

                    If pEquipmentStatus = "OFF" And pIsEquipmentInitializing = True Then
                        'do nothing and wait for equipment to pause
                    ElseIf (pEquipmentStatus = "OFF" Or pEquipmentStatus = "PAUSED") And pIsEquipmentInitializing = False And pRunStatus <> "DAY" Then
                        'now fully aborted
                        pIsSequenceRunning = False
                        LogInitializeArray()
                        pRunStatus = "DAY"
                        LogSessionEntry("ESSENTIAL", "DAY: zipping folders.", "", "CheckRun", "SEQUENCE")
                        'zip and copy any image files that are not done yet
                        returnvalue = Zip7AndCopyFolders(False)
                        If returnvalue <> "OK" Then
                            LogSessionEntry("ERROR", "DAY: Zip7AndCopyFolders: " + returnvalue, "", "Zip7AndCopyFolders", "SEQUENCE")
                            CheckRun = returnvalue
                            Exit Function
                        End If
                    End If

                    'when there is no roof, the run should be stopped completely
                    If My.Settings.sRoofDevice = "NONE" And pStructEventTimes.SunSettingRising = "RISING" Then
                        LogSessionEntry("ESSENTIAL", "No roof defined, sun rising: aborting run.", "", "CheckRun", "SEQUENCE")
                        pStartRun = False 'do not restart run.
                        pRunStatus = "ABORTED"
                        pAbort = True
                    End If

                Else
                    '----------------------------------------------------
                    'NIGHT: SAFE OR UNSAFE
                    '----------------------------------------------------
                    If pWeatherStatus = "SAFE" Then
                        '----------------------------------------------------
                        'NIGHT SAFE: DO RUN
                        '----------------------------------------------------                       
                        If pStructEventTimes.SunAlt <= My.Settings.sSunOpenRoof And pStructEventTimes.SunSettingRising = "SETTING" And
                            pStructEventTimes.SunAlt > My.Settings.sSunDuskFlats Then
                            '--------------------------------------------------------------------------------------------------------
                            ' PHASE 2: COOLDOWN TELESCOPE
                            '--------------------------------------------------------------------------------------------------------
                            'pStructEventTimes.MoonSafetyStatus = "COOLDOWN" Or
                            If pStructEventTimes.MoonSafetyStatus = "SAFE" Or
                                My.Settings.sMoonIgnore = True Or
                                pStructEventTimes.MoonCooldownStatus = "COOLDOWN" Or
                                My.Settings.sObservationProgram = "VARIABLES" Or
                                My.Settings.sObservationProgram = "DEEPSKY ELSE VARIABLES" Then
                                '--------------------------------------------------------------------------------------------------------
                                ' MOON IS SAFE OR CAN BE IGNORED OR VARIABLES: COOLDOWN TELESCOPE
                                '--------------------------------------------------------------------------------------------------------
                                'turn on equipment and open roof if needed
                                If pEquipmentStatus <> "ON" And pIsEquipmentInitializing = False Then
                                    pRunStatus = "STARTUP"
                                    LogSessionEntry("ESSENTIAL", "STARTUP: turning on power... ", "Sun Alt " + Format(pStructEventTimes.SunAlt, "#0.00") + "° - Moon Alt " + Format(pStructEventTimes.MoonAlt, "#0.00") + "°", "CheckRun", "SEQUENCE")
                                    returnvalue = TurnOnEquipment(True)
                                    If returnvalue <> "OK" Then
                                        LogSessionEntry("ERROR", "CheckRun: " + returnvalue, "", "CheckRun", "SEQUENCE")
                                        CheckRun = returnvalue
                                        Exit Function
                                    End If
                                ElseIf pEquipmentStatus <> "ON" And pIsEquipmentInitializing = True Then
                                    'do nothing and wait for equipment to switch on
                                ElseIf pEquipmentStatus = "ON" And pIsEquipmentInitializing = False Then
                                    pRunStatus = "WAITING"
                                    LogSessionEntry("ESSENTIAL", "DUSK FLATS COOLDOWN: waiting for dusk...", "", "CheckRun", "SEQUENCE")
                                End If
                            Else
                                '--------------------------------------------------------------------------------------------------------
                                'MOON IS UNSAFE: PAUSE
                                '--------------------------------------------------------------------------------------------------------
                                returnvalue = PauseRun("DUSK FLATS COOLDOWN: MOON UNSAFE: PAUSING EQUIPMENT...", "",
                                                           "DUSK FLATS COOLDOWN: MOON UNSAFE: PauseEquipment: ", "",
                                                           "DUSK FLATS COOLDOWN: MOON UNSAFE: equipment paused.", "", "PAUSING", "WAITING")
                                If returnvalue <> "OK" Then
                                    CheckRun = returnvalue
                                    Exit Function
                                End If
                            End If
                        ElseIf pStructEventTimes.SunAlt <= My.Settings.sSunDuskFlats And pStructEventTimes.SunAlt > My.Settings.sSunStartRun And
                                   pStructEventTimes.SunSettingRising = "SETTING" Then 'And pIsSequenceRunning <> True 
                            '--------------------------------------------------------------------------------------------------------
                            ' PHASE 3: DUSK FLATS
                            '--------------------------------------------------------------------------------------------------------
                            'pStructEventTimes.MoonSafetyStatus = "COOLDOWN" Or
                            If pStructEventTimes.MoonSafetyStatus = "SAFE" Or
                                My.Settings.sMoonIgnore = True Or
                                pStructEventTimes.MoonCooldownStatus = "COOLDOWN" Or
                                My.Settings.sObservationProgram = "VARIABLES" Or
                                My.Settings.sObservationProgram = "DEEPSKY ELSE VARIABLES" Then

                                '--------------------------------------------------------------------------------------------------------
                                'MOON IS SAFE OR CAN BE IGNORED OR VARIABLES: DUSK FLATS NEEDED ?
                                '--------------------------------------------------------------------------------------------------------
                                If My.Settings.sSunDuskFlatsNeeded = True Then
                                    '--------------------------------------------------------------------------------------------------------
                                    'DUSK FLATS ARE NEEDED ELSE COOLDOWN
                                    '--------------------------------------------------------------------------------------------------------
                                    'turn on equipment and open roof if needed
                                    If pEquipmentStatus <> "ON" And pIsEquipmentInitializing = False Then
                                        pRunStatus = "STARTUP"
                                        LogSessionEntry("ESSENTIAL", "STARTUP: turning on power... ", "Sun Alt " + Format(pStructEventTimes.SunAlt, "#0.00") + "° - Moon Alt " + Format(pStructEventTimes.MoonAlt, "#0.00") + "°", "CheckRun", "SEQUENCE")
                                        returnvalue = TurnOnEquipment(True)
                                        If returnvalue <> "OK" Then
                                            LogSessionEntry("ERROR", "TurnOnEquipment: " + returnvalue, "", "TurnOnEquipment", "SEQUENCE")
                                            CheckRun = returnvalue
                                            Exit Function
                                        End If
                                    ElseIf pEquipmentStatus <> "ON" And pIsEquipmentInitializing = True Then
                                        'do nothing and wait for equipment to switch on
                                    ElseIf pEquipmentStatus = "ON" And pIsEquipmentInitializing = False Then
                                        '---------------------------------------
                                        'EQUIPMENT IS ON: RUN FLATS
                                        '---------------------------------------
                                        'start flats run
                                        If (pIsSequenceRunning = False And pRunStatus <> "FLATS") Then
                                            pAbort = False
                                            If pStructEventTimes.SunAlt > My.Settings.sSunDuskFlats - 4 Then
                                                LogSessionEntry("ESSENTIAL", "DUSK FLATS running...", "", "CheckRun", "SEQUENCE")
                                                pIsSequenceRunning = True
                                                pRunStatus = "FLATS"
                                                returnvalue = RunFlats()
                                                If returnvalue <> "OK" Then
                                                    CheckRun = returnvalue
                                                    Exit Function
                                                End If
                                            Else
                                                LogSessionEntry("ESSENTIAL", "DUSK FLATS: not enough light. Waiting for sun to set.", "", "CheckRun", "SEQUENCE")
                                                pIsSequenceRunning = False

                                                If pStructMount.AtPark = False Then
                                                    'park the mount and continue waiting
                                                    returnvalue = MountPark()
                                                    If returnvalue <> "OK" Then
                                                        CheckRun = returnvalue
                                                        LogSessionEntry("ERROR", "Mount park: " + returnvalue, "", "MountPark", "SEQUENCE")
                                                        Exit Function
                                                    End If
                                                End If

                                                pRunStatus = "WAITING"
                                                LogSessionEntry("DEBUG", "Set pRunStatus to WAITING", "", "RunFlats", "SEQUENCE")
                                            End If

                                        End If
                                    End If
                                Else
                                    '---------------------------------------
                                    'DUSK FLATS NOT NEEDED: START COOLING
                                    '---------------------------------------
                                    'turn on equipment and open roof if needed
                                    If pEquipmentStatus <> "ON" And pIsEquipmentInitializing = False Then
                                        pRunStatus = "STARTUP"
                                        LogSessionEntry("ESSENTIAL", "STARTUP: turning on power... ", "Sun Alt " + Format(pStructEventTimes.SunAlt, "#0.00") + "° - Moon Alt " + Format(pStructEventTimes.MoonAlt, "#0.00") + "°", "CheckRun", "SEQUENCE")
                                        returnvalue = TurnOnEquipment(True)
                                        If returnvalue <> "OK" Then
                                            LogSessionEntry("ERROR", "TurnOnEquipment: " + returnvalue, "", "TurnOnEquipment", "SEQUENCE")
                                            CheckRun = returnvalue
                                            Exit Function
                                        End If
                                    ElseIf pEquipmentStatus <> "ON" And pIsEquipmentInitializing = True Then
                                        'do nothing and wait for equipment to switch on
                                    ElseIf pEquipmentStatus = "ON" And pIsEquipmentInitializing = False Then
                                        pRunStatus = "WAITING"
                                        LogSessionEntry("ESSENTIAL", "DUSK FLATS: no flats required, cooling equipment...", "", "CheckRun", "SEQUENCE")
                                    End If
                                End If
                            Else
                                '--------------------------------------------------------------------------------------------------------
                                ' MOON IS UNSAFE: PAUSE TELESCOPE
                                '--------------------------------------------------------------------------------------------------------
                                returnvalue = PauseRun("DUSK FLATS: MOON UNSAFE: PAUSING EQUIPMENT... ", "Moon Alt: " + Format(pStructEventTimes.MoonAlt, "#0.00") + "°",
                                               "DUSK FLATS: MOON UNSAFE: PauseEquipment: ", "",
                                               "DUSK FLATS: MOON UNSAFE: equipment paused.", "", "PAUSING", "WAITING")
                                If returnvalue <> "OK" Then
                                    CheckRun = returnvalue
                                    Exit Function
                                End If
                            End If
                        ElseIf ((pStructEventTimes.SunAlt <= My.Settings.sSunStartRun And pStructEventTimes.SunSettingRising = "SETTING") Or
                                (pStructEventTimes.SunAlt < My.Settings.sSunStopRun And pStructEventTimes.SunSettingRising = "RISING")) Then
                            '---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
                            'PHASE 4: RUN SEQUENCE
                            '---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
                            '-------------------------------------------------------------------
                            'PART 1: FIRST TIME ONLY: CHECK DATABASE IF THERE ARE STILL TARGETS
                            '-------------------------------------------------------------------
                            'calculate number of deepsky targets, but only the first time

                            If My.Settings.sObservationProgram <> "VARIABLES" Then
                                If pStructEventTimes.MoonSafetyStatus = "SAFE" Or My.Settings.sMoonIgnore = True Then
                                    'normal flow
                                    If pDSLTarget.ID = 0 Then
                                        If pRunStatus <> "VARIABLES" And pRunStatus <> "STARTUP" Then
                                            LogSessionEntry("BRIEF", "Checking deepsky database...", " Moon Alt: " + Format(pStructEventTimes.MoonAlt, "#0.00") + "°", "DatabaseSelectDeepSky", "PROGRAM")
                                        End If

                                        returnvalue = DatabaseSelectDeepSky(True, False)
                                        If returnvalue <> "OK" Then
                                            LogSessionEntry("ERROR", "DatabaseSelectDeepSkyTarget: " + returnvalue, "", "DatabaseSelectDeepSkyTarget", "SEQUENCE")
                                            CheckRun = returnvalue
                                            Exit Function
                                        End If
                                    End If

                                Else
                                    'check for new targets with ignore moon option on
                                    returnvalue = DatabaseSelectDeepSky(True, True)
                                    If returnvalue <> "OK" Then
                                        LogSessionEntry("ERROR", "DatabaseSelectDeepSkyTarget: " + returnvalue, "", "DatabaseSelectDeepSkyTarget", "SEQUENCE")
                                        CheckRun = returnvalue
                                        Exit Function
                                    End If
                                End If
                            End If

                            If My.Settings.sObservationProgram <> "DEEPSKY" Then
                                'calculate number of variable targets
                                If pVSLTarget.ID = 0 Then
                                    LogSessionEntry("BRIEF", "Checking HADS-database...", " Moon Alt: " + Format(pStructEventTimes.MoonAlt, "#0.00") + "°", "DatabaseSelectHADS", "PROGRAM")
                                    returnvalue = DatabaseSelectHADS(True)
                                    If returnvalue <> "OK" Then
                                        LogSessionEntry("ERROR", "DatabaseSelectHADS: " + returnvalue, "", "DatabaseSelectHADS", "SEQUENCE")
                                        CheckRun = returnvalue
                                        Exit Function
                                    End If
                                End If
                            End If

                            '-------------------------------------------------------------------------------
                            'PART 2: DECIDE WHAT TO DO
                            '-------------------------------------------------------------------------------
                            If My.Settings.sObservationProgram = "VARIABLES" Then
                                '-------------------------------------------------------------------------------
                                'OBSERVATION PROGRAM = VARIABLES
                                '-------------------------------------------------------------------------------
                                If pVSLTarget.ID > 0 Then
                                    'still targets to look at
                                    If pEquipmentStatus <> "ON" And pIsEquipmentInitializing = False Then
                                        '--------------------------------------------------------------------------
                                        'STARTUP
                                        '--------------------------------------------------------------------------
                                        pAbort = False
                                        pRunStatus = "STARTUP"
                                        LogSessionEntry("ESSENTIAL", "STARTUP VARIABLES: turning on power... ", "Sun Alt " + Format(pStructEventTimes.SunAlt, "#0.00") + "°", "CheckRun", "SEQUENCE")
                                        returnvalue = TurnOnEquipment(True)
                                        If returnvalue <> "OK" Then
                                            LogSessionEntry("ERROR", "TurnOnEquipment: " + returnvalue, "", "TurnOnEquipment", "SEQUENCE")
                                            CheckRun = returnvalue
                                            Exit Function
                                        End If
                                    ElseIf pEquipmentStatus <> "ON" And pIsEquipmentInitializing = True Then
                                        '--------------------------------------------------------------------------
                                        'STARTUP WAIT: do nothing and wait for equipment to switch on
                                        '--------------------------------------------------------------------------
                                    ElseIf pEquipmentStatus = "ON" And pIsEquipmentInitializing = False Then
                                        '----------------------------------------------
                                        'RUN VARIABLES PROGRAM
                                        '----------------------------------------------
                                        'check if variables or flats are running, first abort other sequence if needed
                                        If pRunStatus <> "VARIABLES" And pIsActionRunning = True Then
                                            pAbort = True
                                            pIsSequenceRunning = False
                                            pRunStatus = "ABORTING"
                                            LogSessionEntry("ESSENTIAL", "VARIABLES: aborting and switching to variables...", " Moon Alt: " + Format(pStructEventTimes.MoonAlt, "#0.00") + "°", "CheckRun", "SEQUENCE")
                                        ElseIf pRunStatus <> "VARIABLES" And pIsActionRunning = False Then
                                            pIsSequenceRunning = False
                                            pRunStatus = "VARIABLES"
                                            LogSessionEntry("DEBUG", "Set pRunStatus to VARIABLES", "", "CheckRun", "SEQUENCE")
                                        Else
                                            'start variable run
                                            If pIsSequenceRunning = False Then
                                                pAbort = False
                                                pRunStatus = "VARIABLES"
                                                LogSessionEntry("ESSENTIAL", "VARIABLES running...", "", "CheckRun", "SEQUENCE")

                                                returnvalue = RunHADS()
                                                If returnvalue <> "OK" Then
                                                    CheckRun = returnvalue
                                                    Exit Function
                                                End If

                                                'calculate number of variable targets
                                                returnvalue = DatabaseSelectHADS(True)
                                                If returnvalue <> "OK" Then
                                                    LogSessionEntry("ERROR", "DatabaseSelectHADS: " + returnvalue, "", "DatabaseSelectHADS", "SEQUENCE")
                                                    CheckRun = returnvalue
                                                    Exit Function
                                                End If
                                            End If
                                        End If
                                    End If
                                End If
                            Else
                                '-------------------------------------------------------------------------------
                                'OBSERVATION PROGRAM = DEEPSKY / DEEPSKY ELSE VARIABLES
                                '-------------------------------------------------------------------------------
                                If pStructEventTimes.MoonSafetyStatus = "SAFE" Or My.Settings.sMoonIgnore = True Or pDSLTarget.TargetIgnoreMoon = True Then
                                    '-------------------------------------------------------------------------------
                                    'MOON IS SAFE OR CAN BE IGNORED GENERAL / TARGET SPECIFIC: RUN DEEPSKY
                                    '-------------------------------------------------------------------------------
                                    If pDSLTarget.ID > 0 Then
                                        '-------------------------------------------------------------------------------
                                        'DEEPSKY TARGETS AVAILABLE: RUN DEEPSKY
                                        '-------------------------------------------------------------------------------
                                        If pEquipmentStatus <> "ON" And pIsEquipmentInitializing = False Then
                                            '--------------------------------------------------------------------------
                                            'STARTUP
                                            '--------------------------------------------------------------------------
                                            pAbort = False
                                            pRunStatus = "STARTUP"
                                            LogSessionEntry("ESSENTIAL", "STARTUP DEEPSKY: turning on power...", " Sun Alt " + Format(pStructEventTimes.SunAlt, "#0.00") + "°", "CheckRun", "SEQUENCE")
                                            returnvalue = TurnOnEquipment(True)
                                            If returnvalue <> "OK" Then
                                                LogSessionEntry("ERROR", "TurnOnEquipment: " + returnvalue, "", "TurnOnEquipment", "SEQUENCE")
                                                CheckRun = returnvalue
                                                Exit Function
                                            End If
                                        ElseIf pEquipmentStatus <> "ON" And pIsEquipmentInitializing = True Then
                                            '--------------------------------------------------------------------------
                                            'STARTUP WAIT: do nothing and wait for equipment to switch on
                                            '--------------------------------------------------------------------------
                                        ElseIf pEquipmentStatus = "ON" And pIsEquipmentInitializing = False Then
                                            '----------------------------------------------
                                            'RUN DEEPSKY PROGRAM
                                            '----------------------------------------------
                                            'check if variables or flats are running, first abort other sequence if needed
                                            If pRunStatus <> "DEEPSKY" And pIsActionRunning = True Then
                                                pAbort = True
                                                pIsSequenceRunning = False
                                                pRunStatus = "ABORTING"
                                                LogSessionEntry("ESSENTIAL", "DEEPSKY: aborting and switching to deepsky...", " Moon Alt: " + Format(pStructEventTimes.MoonAlt, "#0.00") + "°", "CheckRun", "SEQUENCE")
                                            ElseIf pRunStatus <> "DEEPSKY" And pIsActionRunning = False Then
                                                pIsSequenceRunning = False
                                                pRunStatus = "DEEPSKY"
                                                LogSessionEntry("DEBUG", "Set pRunStatus to DEEPSKY", "", "CheckRun", "SEQUENCE")
                                            Else
                                                'start deepsky run
                                                If pIsSequenceRunning = False Then
                                                    pAbort = False
                                                    pRunStatus = "DEEPSKY"
                                                    LogSessionEntry("ESSENTIAL", "DEEPSKY running...", "", "CheckRun", "SEQUENCE")

                                                    If pDSLTarget.TargetMosaic = False Then
                                                        If pStructEventTimes.MoonSafetyStatus = "SAFE" Or My.Settings.sMoonIgnore = True Then
                                                            'normal target
                                                            returnvalue = RunDeepsky(False)
                                                            If returnvalue <> "OK" Then
                                                                CheckRun = returnvalue
                                                                Exit Function
                                                            End If
                                                        Else
                                                            'only targets with ignore moon on
                                                            returnvalue = RunDeepsky(True)
                                                            If returnvalue <> "OK" Then
                                                                CheckRun = returnvalue
                                                                Exit Function
                                                            End If
                                                        End If
                                                    Else
                                                        If pStructEventTimes.MoonSafetyStatus = "SAFE" Or My.Settings.sMoonIgnore = True Then
                                                            'run mosaic
                                                            returnvalue = RunDeepskyMosaic(False)
                                                            If returnvalue <> "OK" Then
                                                                CheckRun = returnvalue
                                                                Exit Function
                                                            End If
                                                        Else
                                                            'only mosaic with ignore moon on
                                                            returnvalue = RunDeepskyMosaic(True)
                                                            If returnvalue <> "OK" Then
                                                                CheckRun = returnvalue
                                                                Exit Function
                                                            End If
                                                        End If

                                                    End If

                                                    'calculate number of deepsky targets
                                                    If pStructEventTimes.MoonSafetyStatus = "SAFE" Or My.Settings.sMoonIgnore = True Then
                                                        'normal flow
                                                        returnvalue = DatabaseSelectDeepSky(True, False)
                                                        If returnvalue <> "OK" Then
                                                            LogSessionEntry("ERROR", "DatabaseSelectDeepSkyTarget: " + returnvalue, "", "DatabaseSelectDeepSkyTarget", "SEQUENCE")
                                                            CheckRun = returnvalue
                                                            Exit Function
                                                        End If
                                                    Else
                                                        'check for new targets with ignore moon option on
                                                        returnvalue = DatabaseSelectDeepSky(True, True)
                                                        If returnvalue <> "OK" Then
                                                            LogSessionEntry("ERROR", "DatabaseSelectDeepSkyTarget: " + returnvalue, "", "DatabaseSelectDeepSkyTarget", "SEQUENCE")
                                                            CheckRun = returnvalue
                                                            Exit Function
                                                        End If
                                                    End If
                                                End If
                                            End If
                                        End If
                                    Else
                                        '-------------------------------------------------------------------------------
                                        'NO DEEPSKY TARGETS AVAILABLE: SWITCH TO VARIABLES OR PAUSE EQUIPMENT
                                        '-------------------------------------------------------------------------------
                                        If My.Settings.sObservationProgram = "DEEPSKY" Then
                                            '-------------------------------------------------------------------------------
                                            'PROGRAM = DEEPSKY: DO NOTHING AND PAUSE EQUIPMENT
                                            '-------------------------------------------------------------------------------
                                            LogSessionEntry("ERROR", "NO DEEPSKY LEFT...", "", "CheckRun", "SEQUENCE")
                                            returnvalue = PauseRun("NO DEEPSKY LEFT: PAUSING EQUIPMENT...", "",
                                           "NO DEEPSKY LEFT: PauseEquipment: ", "",
                                           "NO DEEPSKY LEFT: equipment paused.", "", "PAUSING", "DONE")
                                            If returnvalue <> "OK" Then
                                                CheckRun = returnvalue
                                                Exit Function
                                            End If
                                        Else
                                            '---------------------------------------------------------
                                            'PROGRAM = DEEPSKY ELSE VARIABLES: SWITCH TO VARIABLES
                                            '---------------------------------------------------------
                                            'LogSessionEntry("ESSENTIAL", "Switching to variables...", " Moon Alt: " + Format(pStructEventTimes.MoonAlt, "#0.00") + "°", "DatabaseSelectDeepSkyTarget", "SEQUENCE") has no use and is repeated in inapproriate places
                                            If pVSLTarget.ID > 0 Then
                                                '-------------------------------------------------------------------------------
                                                'SAFE: VARIABLES: targets and safe so run variables
                                                '-------------------------------------------------------------------------------
                                                If pEquipmentStatus <> "ON" And pIsEquipmentInitializing = False Then
                                                    '--------------------------------------------------------------------------
                                                    'STARTUP
                                                    '--------------------------------------------------------------------------
                                                    pAbort = False
                                                    pRunStatus = "STARTUP"
                                                    LogSessionEntry("ESSENTIAL", "STARTUP VARIABLES: turning on power...", " Sun Alt " + Format(pStructEventTimes.SunAlt, "#0.00") + "°" + " Moon Alt: " + Format(pStructEventTimes.MoonAlt, "#0.00") + "°", "CheckRun", "SEQUENCE")
                                                    returnvalue = TurnOnEquipment(True)
                                                    If returnvalue <> "OK" Then
                                                        LogSessionEntry("ERROR", "TurnOnEquipment: " + returnvalue, "", "TurnOnEquipment", "SEQUENCE")
                                                        CheckRun = returnvalue
                                                        Exit Function
                                                    End If
                                                ElseIf pEquipmentStatus <> "ON" And pIsEquipmentInitializing = True Then
                                                    '--------------------------------------------------------------------------
                                                    'STARTUP WAIT: do nothing and wait for equipment to switch on
                                                    '--------------------------------------------------------------------------
                                                ElseIf pEquipmentStatus = "ON" And pIsEquipmentInitializing = False Then
                                                    '----------------------------------------------
                                                    'RUN VARIABLE PROGRAM
                                                    '----------------------------------------------
                                                    'check if variables or flats are running, first abort other sequence if needed
                                                    If pRunStatus <> "VARIABLES" And pIsActionRunning = True Then
                                                        pAbort = True
                                                        pIsSequenceRunning = False
                                                        pRunStatus = "ABORTING"
                                                        LogSessionEntry("ESSENTIAL", "VARIABLES: aborting and switching to variables...", " Moon Alt: " + Format(pStructEventTimes.MoonAlt, "#0.00") + "°", "CheckRun", "SEQUENCE")
                                                    ElseIf pRunStatus <> "VARIABLES" And pIsActionRunning = False Then
                                                        pIsSequenceRunning = False
                                                        pRunStatus = "VARIABLES"
                                                        LogSessionEntry("DEBUG", "Set pRunStatus to VARIABLES", "", "CheckRun", "SEQUENCE")
                                                    Else
                                                        'start variable run
                                                        If pIsSequenceRunning = False Then
                                                            pAbort = False
                                                            pRunStatus = "VARIABLES"
                                                            LogSessionEntry("ESSENTIAL", "VARIABLES running...", "", "CheckRun", "SEQUENCE")

                                                            returnvalue = RunHADS()
                                                            If returnvalue <> "OK" Then
                                                                CheckRun = returnvalue
                                                                Exit Function
                                                            End If

                                                            'calculate number of variable targets
                                                            returnvalue = DatabaseSelectHADS(True)
                                                            If returnvalue <> "OK" Then
                                                                LogSessionEntry("ERROR", "DatabaseSelectHADS: " + returnvalue, "", "DatabaseSelectHADS", "SEQUENCE")
                                                                CheckRun = returnvalue
                                                                Exit Function
                                                            End If
                                                        End If
                                                    End If
                                                End If
                                            End If
                                        End If
                                    End If
                                Else
                                    '--------------------------------------------------------------------------------------------------------
                                    'MOON IS UNSAFE AND CANNOT BE IGNORED: PAUSE OF RUN VARIABLES
                                    '--------------------------------------------------------------------------------------------------------
                                    If My.Settings.sObservationProgram = "DEEPSKY" Then
                                        '--------------------------------------------------------------------------------------------------------
                                        'PROGRAM = DEEPSKY: PAUSE
                                        '--------------------------------------------------------------------------------------------------------

                                        'If pStructEventTimes.MoonSafetyStatus = "COOLDOWN" And pDUSLTarget.ID = 0 And pVSLTarget.ID = 0 Then
                                        If pStructEventTimes.MoonCooldownStatus = "COOLDOWN" And pDUSLTarget.ID = 0 And pVSLTarget.ID = 0 Then
                                            'turn on equipment and open roof if needed
                                            If pEquipmentStatus <> "ON" And pIsEquipmentInitializing = False Then
                                                pRunStatus = "STARTUP"
                                                LogSessionEntry("ESSENTIAL", "STARTUP: turning on power...", " Sun Alt " + Format(pStructEventTimes.SunAlt, "#0.00") + "° - Moon Alt " + Format(pStructEventTimes.MoonAlt, "#0.00") + "°", "CheckRun", "SEQUENCE")
                                                returnvalue = TurnOnEquipment(True)
                                                If returnvalue <> "OK" Then
                                                    LogSessionEntry("ERROR", "CheckRun: " + returnvalue, "", "CheckRun", "SEQUENCE")
                                                    CheckRun = returnvalue
                                                    Exit Function
                                                End If
                                            ElseIf pEquipmentStatus <> "ON" And pIsEquipmentInitializing = True Then
                                                'do nothing and wait for equipment to switch on
                                            ElseIf pEquipmentStatus = "ON" And pIsEquipmentInitializing = False Then
                                                pRunStatus = "WAITING"
                                                LogSessionEntry("ESSENTIAL", "MOON COOLDOWN PERIOD: waiting for Moon to set...", "", "CheckRun", "SEQUENCE")
                                            End If
                                        ElseIf pRunStatus = "WAITING" Then
                                            LogSessionEntry("ESSENTIAL", "MOON UNSAFE: waiting for Moon to set...", "  Moon Alt: " + Format(pStructEventTimes.MoonAlt, "#0.00") + "°", "CheckRun", "SEQUENCE")
                                        Else
                                            returnvalue = PauseRun("DONE: PAUSING EQUIPMENT...", "",
                                                       "DONE: PauseEquipment: ", "",
                                                       "DEEPSKY: MOON UNSAFE...", "", "PAUSING", "WAITING")
                                            If returnvalue <> "OK" Then
                                                CheckRun = returnvalue
                                                Exit Function
                                            End If
                                        End If
                                    Else
                                        '--------------------------------------------------------------------------------------------------------
                                        'PROGRAM = DEEPSKY ELSE VARIABLES: RUN VARIABLES
                                        '--------------------------------------------------------------------------------------------------------
                                        If pVSLTarget.ID > 0 Then
                                            If pEquipmentStatus <> "ON" And pIsEquipmentInitializing = False Then
                                                '--------------------------------------------------------------------------
                                                'STARTUP
                                                '--------------------------------------------------------------------------
                                                pAbort = False
                                                pRunStatus = "STARTUP"
                                                LogSessionEntry("ESSENTIAL", "STARTUP VARIABLES: turning on power...", " Sun Alt " + Format(pStructEventTimes.SunAlt, "#0.00") + "°" + " Moon Alt: " + Format(pStructEventTimes.MoonAlt, "#0.00") + "°", "CheckRun", "SEQUENCE")
                                                returnvalue = TurnOnEquipment(True)
                                                If returnvalue <> "OK" Then
                                                    LogSessionEntry("ERROR", "TurnOnEquipment: " + returnvalue, "", "TurnOnEquipment", "SEQUENCE")
                                                    CheckRun = returnvalue
                                                    Exit Function
                                                End If
                                            ElseIf pEquipmentStatus <> "ON" And pIsEquipmentInitializing = True Then
                                                '--------------------------------------------------------------------------
                                                'STARTUP WAIT: do nothing and wait for equipment to switch on
                                                '--------------------------------------------------------------------------
                                            ElseIf pEquipmentStatus = "ON" And pIsEquipmentInitializing = False Then
                                                '----------------------------------------------
                                                'RUN VARIABLES PROGRAM
                                                '----------------------------------------------
                                                'check if variables or flats are running, first abort other sequence if needed
                                                If pRunStatus <> "VARIABLES" And pIsActionRunning = True Then
                                                    pAbort = True
                                                    pIsSequenceRunning = False
                                                    pRunStatus = "ABORTING"
                                                    LogSessionEntry("ESSENTIAL", "VARIABLES: aborting and switching to variables...", "  Moon Alt: " + Format(pStructEventTimes.MoonAlt, "#0.00") + "°", "CheckRun", "SEQUENCE")
                                                ElseIf pRunStatus <> "VARIABLES" And pIsActionRunning = False Then
                                                    pIsSequenceRunning = False
                                                    pRunStatus = "VARIABLES"
                                                    LogSessionEntry("DEBUG", "Set pRunStatus to VARIABLES", "", "CheckRun", "SEQUENCE")
                                                Else
                                                    'start variable run
                                                    If pIsSequenceRunning = False Then
                                                        pAbort = False
                                                        pRunStatus = "VARIABLES"
                                                        LogSessionEntry("ESSENTIAL", "VARIABLES running...", "", "CheckRun", "SEQUENCE")

                                                        returnvalue = RunHADS()
                                                        If returnvalue <> "OK" Then
                                                            CheckRun = returnvalue
                                                            Exit Function
                                                        End If

                                                        'calculate number of variable targets
                                                        returnvalue = DatabaseSelectHADS(True)
                                                        If returnvalue <> "OK" Then
                                                            LogSessionEntry("ERROR", "DatabaseSelectHADS: " + returnvalue, "", "DatabaseSelectHADS", "SEQUENCE")
                                                            CheckRun = returnvalue
                                                            Exit Function
                                                        End If
                                                    End If
                                                End If
                                            End If
                                        End If
                                    End If
                                End If
                            End If
                        ElseIf pStructEventTimes.SunAlt >= My.Settings.sSunStopRun And pStructEventTimes.SunAlt < My.Settings.sSunDawnFlats And
                               pStructEventTimes.SunSettingRising = "RISING" And pIsEquipmentInitializing = False Then
                            '--------------------------------------------------------------------------------------------------------
                            'WAIT FOR FLATS
                            '--------------------------------------------------------------------------------------------------------
                            If My.Settings.sSunDawnFlatsNeeded = True Then
                                '--------------------------------------------------------------------------------------------------------
                                'FLATS NEEDED: ABORT AND WAIT
                                '--------------------------------------------------------------------------------------------------------
                                'abort ongoing sequence
                                If pIsSequenceRunning = True Then
                                    pAbort = True
                                    pRunStatus = "ABORTING"
                                    LogSessionEntry("ESSENTIAL", "DAWN FLATS: aborting run...", " Sun Alt " + Format(pStructEventTimes.SunAlt, "#0.00") + "°", "CheckRun", "SEQUENCE")
                                End If

                                If pEquipmentStatus <> "ON" And pIsEquipmentInitializing = False Then
                                    'Do nothing, will not turn on equipment just to run flats
                                    pRunStatus = "WAITING"
                                    LogSessionEntry("DEBUG", "Set pRunStatus to WAITING", "", "CheckRun", "SEQUENCE")
                                Else
                                    pRunStatus = "WAITING"
                                    LogSessionEntry("ESSENTIAL", "DAWN FLATS: waiting for dawn flats...", " Sun Alt " + Format(pStructEventTimes.SunAlt, "#0.00") + "°", "CheckRun", "SEQUENCE")
                                End If
                            Else
                                '--------------------------------------------------------------------------------------------------------
                                'NO FLATS NEEDED: STOP RUN COMPLETELY
                                '--------------------------------------------------------------------------------------------------------
                                returnvalue = PauseRun("DAWN: PAUSING EQUIPMENT...", "",
                                                           "DAWN: PauseEquipment: ", "",
                                                           "DAWN: equipment paused.", "", "PAUSING", "WAITING")
                                If returnvalue <> "OK" Then
                                    CheckRun = returnvalue
                                    Exit Function
                                End If
                            End If
                        ElseIf pStructEventTimes.SunAlt >= My.Settings.sSunDawnFlats And pStructEventTimes.SunAlt < 0 And
                             pStructEventTimes.SunSettingRising = "RISING" And pIsSequenceRunning <> True And pIsEquipmentInitializing = False Then
                            '--------------------------------------------------------------------------------------------------------
                            'DAWN FLATS: ONLY WHEN EQUIPMENT IS ON
                            '--------------------------------------------------------------------------------------------------------
                            If My.Settings.sSunDawnFlatsNeeded = True Then
                                '--------------------------------------------------------------------------------------------------------
                                'FLATS NEEDED: RUN DAWN FLATS
                                '--------------------------------------------------------------------------------------------------------
                                'check if variables or flats are running, first abort other sequence if needed
                                If pIsSequenceRunning = True And pRunStatus <> "FLATS" Then
                                    pAbort = True
                                    pRunStatus = "ABORTING"
                                    LogSessionEntry("ESSENTIAL", "DAWN FLATS: aborting, switching to flats...", " Sun Alt " + Format(pStructEventTimes.SunAlt, "#0.00") + "°", "CheckRun", "SEQUENCE")
                                End If

                                'start flats run
                                If pEquipmentStatus = "ON" And pIsSequenceRunning = False And pRunStatus <> "FLATS" Then
                                    pAbort = False
                                    pIsSequenceRunning = True
                                    pRunStatus = "FLATS"
                                    LogSessionEntry("ESSENTIAL", "DAWN FLATS running...", " Sun Alt " + Format(pStructEventTimes.SunAlt, "#0.00") + "°", "CheckRun", "SEQUENCE")

                                    returnvalue = RunFlats()
                                    If returnvalue <> "OK" Then
                                        CheckRun = returnvalue
                                        Exit Function
                                    End If
                                Else
                                    pRunStatus = "WAITING"
                                End If
                            Else
                                '--------------------------------------------------------------------------------------------------------
                                'NO FLATS NEEDED: STOP RUN COMPLETELY
                                '--------------------------------------------------------------------------------------------------------
                                returnvalue = PauseRun("DAWN: PAUSING EQUIPMENT...", "",
                                                       "DAWN: PauseEquipment: ", "",
                                                       "DAWN: equipment paused.", "", "PAUSING", "WAITING")
                                If returnvalue <> "OK" Then
                                    CheckRun = returnvalue
                                    Exit Function
                                End If
                            End If
                        End If

                    Else
                        '----------------------------------------------------
                        'NIGHT UNSAFE: PAUSE RUN 
                        '----------------------------------------------------
                        returnvalue = PauseRun("CLOUDS UNSAFE: PAUSING EQUIPMENT...", "",
                                               "CLOUDS UNSAFE: PauseEquipment: ", "",
                                               "CLOUDS UNSAFE: equipment paused.", "", "PAUSING", "UNSAFE")
                        If returnvalue <> "OK" Then
                            CheckRun = returnvalue
                            Exit Function
                        End If

                    End If
                End If
            End If

        Catch ex As Exception
            CheckRun = "CheckRun: " + ex.Message
            LogSessionEntry("ERROR", "CheckRun: " + ex.Message, "", "CheckRun", "PROGRAM")
        End Try
    End Function

    Public Function PauseRun(vMessage2 As String, vMessage2Var As String, vMessage3 As String, vMessage3Var As String, vMessage4 As String, vMessage4Var As String, vRunStatus1 As String, vRunStatus2 As String) As String
        Dim returnvalue As String

        PauseRun = "OK"
        Try
            'abort any ongoing sequence
            pAbort = True

            If (pEquipmentStatus <> "PAUSED" And pEquipmentStatus <> "ERROR") And pIsEquipmentInitializing = False And pIsActionRunning = False Then
                'pause equipment, but only when actions are finished and equipment is not already pausing
                LogSessionEntry("ESSENTIAL", vMessage2, vMessage2Var, "PauseRun", "SEQUENCE")
                pRunStatus = vRunStatus1

                returnvalue = PauseEquipment()
                If returnvalue <> "OK" Then
                    LogSessionEntry("ERROR", vMessage3 + returnvalue, vMessage3Var, "PauseRun", "PROGRAM")
                    PauseRun = returnvalue
                    Exit Function
                End If
            ElseIf pEquipmentStatus = "PAUSED" And pIsEquipmentInitializing = True Then
                'do nothing and wait for equipment to pause
            ElseIf pEquipmentStatus = "PAUSED" And pIsEquipmentInitializing = False And pRunStatus <> vRunStatus2 Then
                'now fully aborted
                pRunStatus = vRunStatus2
                'pAbort = False => lijkt ervoor te zorgen dat deepsky gewoon verder loopt met plate solve en toestanden
                If (vRunStatus2 = "ABORTED") Then
                    LogSessionEntry("ESSENTIAL", "RUN ABORTED.", "", "PauseRun", "SEQUENCE")
                    FrmMain.BtnStartRun.Enabled = True
                    FrmMain.BtnStopRun.Enabled = False
                End If
                pIsSequenceRunning = False
                LogSessionEntry("ESSENTIAL", vMessage4, vMessage4Var, "PauseRun", "SEQUENCE")
            End If
        Catch ex As Exception
            pIsSequenceRunning = False
            PauseRun = "NOK"
            LogSessionEntry("ERROR", "PauseRun: " + ex.Message, "", "PauseRun", "PROGRAM")
        End Try

    End Function



    Public Function RunDeepskyMosaic(vUnsafe As Boolean) As String
        Dim i, iPanel1, iPanel2, iPanel3, iPanel4, CurrentPanel As Integer
        Dim txtImage, txtPanel As String
        Dim returnvalue As String
        Dim NbrFramesToExposePanel1, NbrFramesToExposePanel2, NbrFramesToExposePanel3, NbrFramesToExposePanel4 As Integer
        Dim NumberOfExposures, TotalNumberOfExposures As Integer
        Dim RA2000, DEC2000 As Double
        Dim RA2000HH, RA2000MM, RA2000SS, DEC2000DG, DEC2000MM, DEC2000SS As String
        Dim RATopocentric, DECTopocentric As Double
        Dim RATopocentricCorrected, DECTopocentricCorrected As Double
        Dim TopocentricRAString, TopocentricDECString As String
        Dim TopocentricRAStringTSX, TopocentricDECStringTSX As String
        Dim FilterNumber, Binning As Integer

        Dim DitherPixelsX, DitherPixelsY As Integer
        Dim DitherOffsetX, DitherOffsetY As Double
        Dim DitherRATopocentric, DitherDECTopocentric As Double
        Dim DitherRATopocentricString, DitherDECTopocentricString As String

        Dim random As New Random()
        Dim InitialSideOfMeridian As Integer

        RunDeepskyMosaic = "OK"
        Try
            iPanel1 = 0
            iPanel2 = 0
            iPanel3 = 0
            iPanel4 = 0
            i = 0

            RA2000HH = ""
            RA2000MM = ""
            RA2000SS = ""
            DEC2000DG = ""
            DEC2000MM = ""
            DEC2000SS = ""

            ' if run is to abort: exit
            If pAbort = True Then
                RunDeepskyMosaic = "NOK"
                Exit Function
            End If

            pIsSequenceRunning = True
            pRunStatus = "DEEPSKY"
            LogSessionEntry("ESSENTIAL", "Running deepsky mosaic flow...", "", "RunDeepskyMosaic", "SEQUENCE")

            'select a target
            returnvalue = DatabaseSelectDeepSky(False, vUnsafe)
            If returnvalue <> "OK" Then
                pIsSequenceRunning = False
                RunDeepskyMosaic = "NOK"
                Exit Function
            End If

            '----------------------------------------------------------------------------------------------------
            'if a target was selected start the run
            '----------------------------------------------------------------------------------------------------
            If pDSLTarget.ID <> 0 And pDSLTarget.TargetMosaic = True Then
                '----------------------------------------------------------------------------------------------------
                'FOCUS: goto focusstar or object and focus
                '----------------------------------------------------------------------------------------------------
                If pAbort = True Then
                    pIsSequenceRunning = False
                    RunDeepskyMosaic = "NOK"
                    Exit Function
                End If

                LogSessionEntry("ESSENTIAL", "Focussing first time in run...", "", "RunDeepskyMosaic", "SEQUENCE")
                returnvalue = FocusDeepsky(My.Settings.sCCDFocusClosedLoopSlew)

                If returnvalue <> "OK" And returnvalue <> "FOCUS_ABORTED" Then
                    pIsSequenceRunning = False
                    RunDeepskyMosaic = returnvalue

                    'mark target as unusable
                    returnvalue = DatabaseMarkErrorDeepSky(returnvalue)
                    If returnvalue <> "OK" Then
                        pIsSequenceRunning = False
                        RunDeepskyMosaic = returnvalue
                        Exit Function
                    Else
                        'target is marked unusable, exiting
                        pIsSequenceRunning = False
                        RunDeepskyMosaic = "NOK"
                        Exit Function
                    End If
                End If

                FrmMain.LblLastFocusDateTime.Text = "Last focus: " + DateTime.UtcNow.ToString("dd/MM/yyyy HH:mm")
                FrmMain.LblLastFocusTemperature.Text = "Last temperature:" + Format(pCurrentFocusserTemperature, "0.00") + "°"

                'retrieve filternumber
                If pDSLTarget.TargetFilter = My.Settings.sCCDFilter1 Then
                    FilterNumber = 0
                ElseIf pDSLTarget.TargetFilter = My.Settings.sCCDFilter2 Then
                    FilterNumber = 1
                ElseIf pDSLTarget.TargetFilter = My.Settings.sCCDFilter3 Then
                    FilterNumber = 2
                ElseIf pDSLTarget.TargetFilter = My.Settings.sCCDFilter4 Then
                    FilterNumber = 3
                ElseIf pDSLTarget.TargetFilter = My.Settings.sCCDFilter5 Then
                    FilterNumber = 4
                End If

                'convert binning
                If pDSLTarget.TargetBin = "1x1" Then
                    Binning = 1
                ElseIf pDSLTarget.TargetBin = "2x2" Then
                    Binning = 2
                ElseIf pDSLTarget.TargetBin = "3x3" Then
                    Binning = 3
                End If

                'take into account the exposures already done
                'only panels that are part of the mosaic, put the rest to zero
                If pDSLTarget.TargetMosaicType = "2x2" Then
                    'all panels
                    NbrFramesToExposePanel1 = pDSLTarget.TargetMosaicFramesPerPanel - pDSLTarget.TargetPanel1NbrExposedFrames - iPanel1
                    NbrFramesToExposePanel2 = pDSLTarget.TargetMosaicFramesPerPanel - pDSLTarget.TargetPanel2NbrExposedFrames - iPanel2
                    NbrFramesToExposePanel3 = pDSLTarget.TargetMosaicFramesPerPanel - pDSLTarget.TargetPanel3NbrExposedFrames - iPanel3
                    NbrFramesToExposePanel4 = pDSLTarget.TargetMosaicFramesPerPanel - pDSLTarget.TargetPanel4NbrExposedFrames - iPanel4
                Else
                    'all other cases there are but 2 panels
                    NbrFramesToExposePanel1 = pDSLTarget.TargetMosaicFramesPerPanel - pDSLTarget.TargetPanel1NbrExposedFrames - iPanel1
                    NbrFramesToExposePanel2 = pDSLTarget.TargetMosaicFramesPerPanel - pDSLTarget.TargetPanel2NbrExposedFrames - iPanel2
                    NbrFramesToExposePanel3 = 0
                    NbrFramesToExposePanel4 = 0
                End If

                'set panel to start with
                If NbrFramesToExposePanel1 > 0 Then
                    CurrentPanel = 1
                ElseIf NbrFramesToExposePanel2 > 0 Then
                    CurrentPanel = 2
                ElseIf NbrFramesToExposePanel2 > 0 Then
                    CurrentPanel = 3
                ElseIf NbrFramesToExposePanel2 > 0 Then
                    CurrentPanel = 4
                Else
                    CurrentPanel = 1
                End If

                'set initial value
                InitialSideOfMeridian = Convert.ToInt32(pStructMount.SideOfPier)

                '-----------------------------------------------------------------------
                'CLOSED LOOP SLEW TO CENTER OF TARGET TO CALCULATE EXACT POINTING ERROR
                '-----------------------------------------------------------------------
                RA2000 = pAUtil.HMSToHours(Format(pDSLTarget.TargetRA2000HH) + " " + Format(pDSLTarget.TargetRA2000MM) + " " + Format(pDSLTarget.TargetRA2000SS))
                DEC2000 = pAUtil.DMSToDegrees(Format(pDSLTarget.TargetDEC2000DG) + " " + Format(pDSLTarget.TargetDEC2000MM) + " " + Format(pDSLTarget.TargetDEC2000SS))
                'convert to topographic coordinates
                ConvertTargetJ2000ToTopocentric(RA2000, DEC2000)
                RATopocentric = pRATargetTopocentric
                DECTopocentric = pDECTargetTopocentric


                TopocentricRAString = pAUtil.HoursToHMS(RATopocentric, "h ", "m ", "s ")
                TopocentricDECString = pAUtil.DegreesToDMS(DECTopocentric, "° ", "' ", """ ")
                TopocentricRAStringTSX = pAUtil.HoursToHMS(RATopocentric, " ", " ", " ")
                TopocentricDECStringTSX = pAUtil.DegreesToDMS(DECTopocentric, " ", " ", " ")

                'closed loop slew to target
                If pMountConnected = True Then
                    returnvalue = ClosedLoopSlew(RATopocentric, DECTopocentric, TopocentricRAString, TopocentricDECString, Format(pDSLTarget.TargetRA2000HH) + "h " + Format(pDSLTarget.TargetRA2000MM) + "m " + Format(pDSLTarget.TargetRA2000SS) + "s ", Format(pDSLTarget.TargetDEC2000DG) + "° " + Format(pDSLTarget.TargetDEC2000MM) + "' " + Format(pDSLTarget.TargetDEC2000SS) + """ ", pDSLTarget.TargetName, pDSLTarget.TargetFilter, False, TopocentricRAStringTSX, TopocentricDECStringTSX)

                    If returnvalue <> "OK" And returnvalue <> "SLEW_ABORTED" Then
                        'mark target as unusable
                        returnvalue = DatabaseMarkErrorDeepSky("Closed loop slew failed.")
                        If returnvalue <> "OK" Then
                            pIsSequenceRunning = False
                            RunDeepskyMosaic = returnvalue
                            Exit Function
                        Else
                            'target is marked unusable, exiting
                            pIsSequenceRunning = False
                            RunDeepskyMosaic = "NOK"
                            Exit Function
                        End If
                    End If
                End If

                '-----------------------------------------------------------------------
                'LOOP WHILE PANELS ARE NOT ALL COMPLETED
                '-----------------------------------------------------------------------
                While (iPanel1 < NbrFramesToExposePanel1 Or
                    iPanel2 < NbrFramesToExposePanel2 Or
                    iPanel3 < NbrFramesToExposePanel3 Or
                    iPanel4 < NbrFramesToExposePanel4)

                    '----------------------------------------------------------------------------------------------------
                    'CHECK IF THIS IS STILL THE TARGET WE WANT TO LOOK AT
                    '----------------------------------------------------------------------------------------------------
                    Dim CurrentTarget As String
                    CurrentTarget = pDSLTarget.TargetName
                    returnvalue = DatabaseSelectDeepSky(True, vUnsafe)
                    If returnvalue <> "OK" Then
                        LogSessionEntry("ERROR", "DatabaseSelectDeepSkyTarget: " + returnvalue, "", "RunDeepskyMosaic", "SEQUENCE")
                        RunDeepskyMosaic = returnvalue
                        Exit Function
                    End If

                    If CurrentTarget <> pDSLTarget.TargetName And pDSLTarget.TargetName <> "" Then
                        LogSessionEntry("ESSENTIAL", "Switching target to: " + pDSLTarget.TargetName, "", "RunDeepskyMosaic", "SEQUENCE")
                        pIsSequenceRunning = False
                        RunDeepskyMosaic = "NOK"
                        Exit Function
                    End If

                    '----------------------------------------------------------------------------------------------------
                    'CLOSED LOOP SLEW to target with dithering
                    '----------------------------------------------------------------------------------------------------
                    'get the RA/DEC for the current panel
                    If CurrentPanel = 1 Then
                        RA2000 = pAUtil.HMSToHours(Format(pDSLTarget.TargetPanel1RA2000HH) + " " + Format(pDSLTarget.TargetPanel1RA2000MM) + " " + Format(pDSLTarget.TargetPanel1RA2000SS))
                        DEC2000 = pAUtil.DMSToDegrees(Format(pDSLTarget.TargetPanel1DEC2000DG) + " " + Format(pDSLTarget.TargetPanel1DEC2000MM) + " " + Format(pDSLTarget.TargetPanel1DEC2000SS))
                        RA2000HH = pDSLTarget.TargetPanel1RA2000HH
                        RA2000MM = pDSLTarget.TargetPanel1RA2000MM
                        RA2000SS = pDSLTarget.TargetPanel1RA2000SS
                        DEC2000DG = pDSLTarget.TargetPanel1DEC2000DG
                        DEC2000MM = pDSLTarget.TargetPanel1DEC2000MM
                        DEC2000SS = pDSLTarget.TargetPanel1DEC2000SS
                    ElseIf CurrentPanel = 2 Then
                        RA2000 = pAUtil.HMSToHours(Format(pDSLTarget.TargetPanel2RA2000HH) + " " + Format(pDSLTarget.TargetPanel2RA2000MM) + " " + Format(pDSLTarget.TargetPanel2RA2000SS))
                        DEC2000 = pAUtil.DMSToDegrees(Format(pDSLTarget.TargetPanel2DEC2000DG) + " " + Format(pDSLTarget.TargetPanel2DEC2000MM) + " " + Format(pDSLTarget.TargetPanel2DEC2000SS))
                        RA2000HH = pDSLTarget.TargetPanel2RA2000HH
                        RA2000MM = pDSLTarget.TargetPanel2RA2000MM
                        RA2000SS = pDSLTarget.TargetPanel2RA2000SS
                        DEC2000DG = pDSLTarget.TargetPanel2DEC2000DG
                        DEC2000MM = pDSLTarget.TargetPanel2DEC2000MM
                        DEC2000SS = pDSLTarget.TargetPanel2DEC2000SS
                    ElseIf CurrentPanel = 3 Then
                        RA2000 = pAUtil.HMSToHours(Format(pDSLTarget.TargetPanel3RA2000HH) + " " + Format(pDSLTarget.TargetPanel3RA2000MM) + " " + Format(pDSLTarget.TargetPanel3RA2000SS))
                        DEC2000 = pAUtil.DMSToDegrees(Format(pDSLTarget.TargetPanel3DEC2000DG) + " " + Format(pDSLTarget.TargetPanel3DEC2000MM) + " " + Format(pDSLTarget.TargetPanel3DEC2000SS))
                        RA2000HH = pDSLTarget.TargetPanel3RA2000HH
                        RA2000MM = pDSLTarget.TargetPanel3RA2000MM
                        RA2000SS = pDSLTarget.TargetPanel3RA2000SS
                        DEC2000DG = pDSLTarget.TargetPanel3DEC2000DG
                        DEC2000MM = pDSLTarget.TargetPanel3DEC2000MM
                        DEC2000SS = pDSLTarget.TargetPanel3DEC2000SS
                    ElseIf CurrentPanel = 4 Then
                        RA2000 = pAUtil.HMSToHours(Format(pDSLTarget.TargetPanel4RA2000HH) + " " + Format(pDSLTarget.TargetPanel4RA2000MM) + " " + Format(pDSLTarget.TargetPanel4RA2000SS))
                        DEC2000 = pAUtil.DMSToDegrees(Format(pDSLTarget.TargetPanel4DEC2000DG) + " " + Format(pDSLTarget.TargetPanel4DEC2000MM) + " " + Format(pDSLTarget.TargetPanel4DEC2000SS))
                        RA2000HH = pDSLTarget.TargetPanel4RA2000HH
                        RA2000MM = pDSLTarget.TargetPanel4RA2000MM
                        RA2000SS = pDSLTarget.TargetPanel4RA2000SS
                        DEC2000DG = pDSLTarget.TargetPanel4DEC2000DG
                        DEC2000MM = pDSLTarget.TargetPanel4DEC2000MM
                        DEC2000SS = pDSLTarget.TargetPanel4DEC2000SS
                    End If

                    'convert to topographic coordinates
                    ConvertTargetJ2000ToTopocentric(RA2000, DEC2000)
                    RATopocentric = pRATargetTopocentric
                    DECTopocentric = pDECTargetTopocentric

                    '---------------------------------------------------------
                    ' TAKE INTO ACCOUNT POINTING ERROR
                    '---------------------------------------------------------
                    'detemine the real slew needed using the offset following a closed loop slew
                    LogSessionEntry("ESSENTIAL", "Total pointing error RA: " + Format(Math.Abs(pTotalSolveErrorRA * 54000), "#0.0") + " arcsec - DEC: " + Format(Math.Abs(pTotalSolveErrorDEC * 3600), "#0.0"), " arcsec.", "RunDeepsky", "SEQUENCE")
                    RATopocentricCorrected = RATopocentric - pTotalSolveErrorRA
                    DECTopocentricCorrected = DECTopocentric - pTotalSolveErrorDEC

                    If My.Settings.sCCDDithering = True Then
                        '--------------------------------------------------------
                        'DITHER
                        '--------------------------------------------------------
                        DitherPixelsX = random.Next(0, My.Settings.sCCDDitheringAmount)
                        DitherPixelsY = random.Next(0, My.Settings.sCCDDitheringAmount)
                        DitherOffsetX = DitherPixelsX * My.Settings.sCCDImageScale * (1 / Math.Cos(Math.PI * DECTopocentricCorrected / 180.0))
                        DitherOffsetY = DitherPixelsY * My.Settings.sCCDImageScale

                        'pRATargetActualTopocentric and pDECTargetActualTopocentric have been updated to the ACTUAL position in the closed loop slew function
                        DitherRATopocentric = RATopocentricCorrected + (DitherOffsetX * 0.000277778) 'convert to hours
                        DitherDECTopocentric = DECTopocentricCorrected + (DitherOffsetY * 0.000277778) 'convert to degrees
                    Else
                        DitherRATopocentric = RATopocentricCorrected
                        DitherDECTopocentric = DECTopocentricCorrected
                    End If
                    DitherRATopocentricString = pAUtil.HoursToHMS(DitherRATopocentric, "h ", "m ", "s ")
                    DitherDECTopocentricString = pAUtil.DegreesToDMS(DitherDECTopocentric, "° ", "' ", """ ")

                    'calculate alt az
                    returnvalue = CalculateObject(DitherRATopocentric, DitherDECTopocentric)
                    If returnvalue <> "OK" Then
                        RunDeepskyMosaic = returnvalue
                        Exit Function
                    End If

                    'slew to offset target
                    LogSessionEntry("BRIEF", "Moving mount to: RA " + DitherRATopocentricString + " - DEC " + DitherDECTopocentricString + " / Alt " + Format(pStructObject.ObjectAlt, "#0.00") + "° - Az " + Format(pStructObject.ObjectAz, "#0.00") + "° / Moon Alt " + Format(pStructEventTimes.MoonAlt, "#0.00") + "°", "", "RunDeepsky", "SEQUENCE")
                    returnvalue = MountSlewToTarget(pDSLTarget.TargetName, DitherRATopocentric, DitherDECTopocentric, DitherRATopocentricString, DitherDECTopocentricString, RA2000HH + ":" + RA2000MM + ":" + RA2000SS, DEC2000DG + ":" + DEC2000MM + ":" + DEC2000SS, False, False)

                    If returnvalue <> "OK" And returnvalue <> "SLEW_ABORTED" Then
                        pIsSequenceRunning = False
                        RunDeepskyMosaic = returnvalue
                        LogSessionEntry("ESSENTIAL", "Slew failed!", "", "RunDeepsky", "SEQUENCE")

                        'mark target as unusable
                        returnvalue = DatabaseMarkErrorDeepSky("Slew failed.")
                        If returnvalue <> "OK" Then
                            pIsSequenceRunning = False
                            RunDeepskyMosaic = returnvalue
                            Exit Function
                        Else
                            'target is marked unusable, exiting
                            pIsSequenceRunning = False
                            RunDeepskyMosaic = "NOK"
                            Exit Function
                        End If
                    End If

                    ' if run is to abort: exit
                    If pAbort = True Then
                        pIsSequenceRunning = False
                        RunDeepskyMosaic = "NOK"
                        Exit Function
                    End If

                    '----------------------------------------------------------------------------------------------------
                    'TAKE AN IMAGE OF THE PANEL
                    '----------------------------------------------------------------------------------------------------
                    txtImage = ""
                    txtPanel = ""
                    If CurrentPanel = 1 Then
                        iPanel1 += 1
                        txtImage = "Panel 1: exposure " + Format(iPanel1) + "/" + Format(NbrFramesToExposePanel1) + " "
                        txtPanel = "panel1"
                    ElseIf CurrentPanel = 2 Then
                        iPanel2 += 1
                        txtImage = "Panel 2: exposure " + Format(iPanel2) + "/" + Format(NbrFramesToExposePanel2) + " "
                        txtPanel = "panel2"
                    ElseIf CurrentPanel = 3 Then
                        iPanel3 += 1
                        txtImage = "Panel 3: exposure " + Format(iPanel3) + "/" + Format(NbrFramesToExposePanel3) + " "
                        txtPanel = "panel3"
                    ElseIf CurrentPanel = 4 Then
                        iPanel4 += 1
                        txtImage = "Panel 4: exposure " + Format(iPanel4) + "/" + Format(NbrFramesToExposePanel4) + " "
                        txtPanel = "panel4"
                    End If
                    i += 1 'general counter used for focussing

                    returnvalue = TakeImageTheSkyX(pDSLTarget.TargetExposure, Binning, pDSLTarget.TargetName + "_" + txtPanel, False, False, FilterNumber, pDSLTarget.TargetFilter, txtImage, False, "LIGHT", TopocentricRAStringTSX, TopocentricDECStringTSX)
                    If returnvalue <> "OK" And returnvalue <> "IMAGE_ABORTED" Then
                        pIsSequenceRunning = False
                        RunDeepskyMosaic = returnvalue
                        LogSessionEntry("ESSENTIAL", "Image failed!", "", "RunDeepskyMosaic", "SEQUENCE")

                        'mark target as unusable
                        returnvalue = DatabaseMarkErrorDeepSky("Image failed.")
                        If returnvalue <> "OK" Then
                            pIsSequenceRunning = False
                            RunDeepskyMosaic = returnvalue
                            Exit Function
                        Else
                            'target is marked unusable, exiting
                            pIsSequenceRunning = False
                            RunDeepskyMosaic = "NOK"
                            Exit Function
                        End If
                    End If

                    'update the database
                    If returnvalue = "OK" Then
                        If CurrentPanel = 1 Then
                            NumberOfExposures = pDSLTarget.TargetPanel1NbrExposedFrames + 1
                        ElseIf CurrentPanel = 2 Then
                            NumberOfExposures = pDSLTarget.TargetPanel2NbrExposedFrames + 1
                        ElseIf CurrentPanel = 3 Then
                            NumberOfExposures = pDSLTarget.TargetPanel3NbrExposedFrames + 1
                        ElseIf CurrentPanel = 4 Then
                            NumberOfExposures = pDSLTarget.TargetPanel4NbrExposedFrames + 1
                        End If

                        TotalNumberOfExposures = pDSLTarget.TargetPanel1NbrExposedFrames + pDSLTarget.TargetPanel2NbrExposedFrames + pDSLTarget.TargetPanel3NbrExposedFrames + pDSLTarget.TargetPanel4NbrExposedFrames + 1

                        returnvalue = DatabaseUpdateNbrExpDeepSky(NumberOfExposures, CurrentPanel, TotalNumberOfExposures, pDSLTarget.TargetMosaicType)
                        If returnvalue <> "OK" Then
                            pIsSequenceRunning = False
                            LogSessionEntry("ESSENTIAL", "Target update failed!", "", "RunDeepskyMosaic", "SEQUENCE")
                            RunDeepskyMosaic = returnvalue
                            Exit Function
                        End If
                    End If

                    ' if run is to abort: exit
                    If pAbort = True Then
                        pIsSequenceRunning = False
                        RunDeepskyMosaic = "NOK"
                        Exit Function
                    End If

                    '--------------------------------------------------------
                    'meridian flip if needed
                    '--------------------------------------------------------
                    If pStructMount.Azimuth > 180 + (My.Settings.sMountMeridianFlipMinutes * 0.25) And Convert.ToInt32(pStructMount.SideOfPier) = 1 Then
                        'do pier flip by doing a slew
                        LogSessionEntry("ESSENTIAL", "Doing meridian flip...", "", "RunDeepsky", "SEQUENCE")
                        returnvalue = ClosedLoopSlew(RATopocentric, DECTopocentric, TopocentricRAString, TopocentricDECString, Format(pDSLTarget.TargetRA2000HH) + "h " + Format(pDSLTarget.TargetRA2000MM) + "m " + Format(pDSLTarget.TargetRA2000SS) + "s ", Format(pDSLTarget.TargetDEC2000DG) + "° " + Format(pDSLTarget.TargetDEC2000MM) + "' " + Format(pDSLTarget.TargetDEC2000SS) + """ ", pDSLTarget.TargetName, pDSLTarget.TargetFilter, False, TopocentricRAStringTSX, TopocentricDECStringTSX)

                        ' if run is to abort: exit
                        If returnvalue <> "OK" And returnvalue <> "SLEW_ABORTED" Then
                            pIsSequenceRunning = False
                            RunDeepskyMosaic = returnvalue
                            LogSessionEntry("ESSENTIAL", "Meridian flip failed!", "", "RunDeepsky", "SEQUENCE")

                            'mark target as unusable
                            returnvalue = DatabaseMarkErrorDeepSky("Meridian flip failed.")
                            If returnvalue <> "OK" Then
                                pIsSequenceRunning = False
                                RunDeepskyMosaic = returnvalue
                                Exit Function
                            Else
                                'target is marked unusable, exiting
                                pIsSequenceRunning = False
                                RunDeepskyMosaic = "NOK"
                                Exit Function
                            End If
                        End If

                        '---------------------------------------------------------------------------------------
                        'following meridian flip: do a closed loop slew to center target for exact coordinates
                        '---------------------------------------------------------------------------------------
                        RA2000 = pAUtil.HMSToHours(Format(pDSLTarget.TargetRA2000HH) + " " + Format(pDSLTarget.TargetRA2000MM) + " " + Format(pDSLTarget.TargetRA2000SS))
                        DEC2000 = pAUtil.DMSToDegrees(Format(pDSLTarget.TargetDEC2000DG) + " " + Format(pDSLTarget.TargetDEC2000MM) + " " + Format(pDSLTarget.TargetDEC2000SS))

                        'convert to topographic coordinates
                        ConvertTargetJ2000ToTopocentric(RA2000, DEC2000)
                        RATopocentric = pRATargetTopocentric
                        DECTopocentric = pDECTargetTopocentric

                        TopocentricRAString = pAUtil.HoursToHMS(RATopocentric, "h ", "m ", "s ")
                        TopocentricDECString = pAUtil.DegreesToDMS(DECTopocentric, "° ", "' ", """ ")
                        TopocentricRAStringTSX = pAUtil.HoursToHMS(RATopocentric, " ", " ", " ")
                        TopocentricDECStringTSX = pAUtil.DegreesToDMS(DECTopocentric, " ", " ", " ")

                        'closed loop slew to target
                        If pMountConnected = True Then
                            returnvalue = ClosedLoopSlew(RATopocentric, DECTopocentric, TopocentricRAString, TopocentricDECString, Format(pDSLTarget.TargetRA2000HH) + "h " + Format(pDSLTarget.TargetRA2000MM) + "m " + Format(pDSLTarget.TargetRA2000SS) + "s ", Format(pDSLTarget.TargetDEC2000DG) + "° " + Format(pDSLTarget.TargetDEC2000MM) + "' " + Format(pDSLTarget.TargetDEC2000SS) + """ ", pDSLTarget.TargetName, pDSLTarget.TargetFilter, False, TopocentricRAStringTSX, TopocentricDECStringTSX)

                            If returnvalue <> "OK" And returnvalue <> "SLEW_ABORTED" Then
                                'mark target as unusable
                                returnvalue = DatabaseMarkErrorDeepSky("Closed loop slew failed.")
                                If returnvalue <> "OK" Then
                                    pIsSequenceRunning = False
                                    RunDeepskyMosaic = returnvalue
                                    Exit Function
                                Else
                                    'target is marked unusable, exiting
                                    pIsSequenceRunning = False
                                    RunDeepskyMosaic = "NOK"
                                    Exit Function
                                End If
                            End If
                        End If
                    End If

                    '-----------------------------------------------------------------------
                    'refocus following meridian flip
                    'refocus if temperature changed too much
                    'refocus if number of exposure reached
                    '-----------------------------------------------------------------------
                    If (InitialSideOfMeridian <> Convert.ToInt32(pStructMount.SideOfPier) And My.Settings.sCCDFocusMeridianFlip = True) Or
                       Math.Abs(pCurrentFocusserTemperature - pInitialFocusTemperature) >= My.Settings.sCCDFocusEveryXDegrees Or
                       i >= My.Settings.sCCDFocusEveryXImages Then

                        If InitialSideOfMeridian <> Convert.ToInt32(pStructMount.SideOfPier) And My.Settings.sCCDFocusMeridianFlip = True Then
                            LogSessionEntry("ESSENTIAL", "Focussing following pier flip...", "", "RunDeepsky", "SEQUENCE")
                        ElseIf Math.Abs(pCurrentFocusserTemperature - pInitialFocusTemperature) >= My.Settings.sCCDFocusEveryXDegrees Then
                            LogSessionEntry("ESSENTIAL", "Focussing due to temperature change: " + Format(pInitialFocusTemperature, "##0.00") + "° -> " + Format(pCurrentFocusserTemperature, "##0.00") + "°C", "", "RunDeepsky", "SEQUENCE")
                        ElseIf i >= My.Settings.sCCDFocusEveryXImages Then
                            LogSessionEntry("ESSENTIAL", "Focussing following " + Format(My.Settings.sCCDFocusEveryXImages) + " exposures...", "", "RunDeepsky", "SEQUENCE")
                        Else
                            LogSessionEntry("ESSENTIAL", "Focussing for unknown reason...", "", "RunDeepsky", "SEQUENCE")
                        End If

                        InitialSideOfMeridian = Convert.ToInt32(pStructMount.SideOfPier)
                        i = 0

                        FrmMain.LblLastFocusDateTime.Text = "Last focus: " + Format(DateTime.UtcNow)
                        FrmMain.LblLastFocusTemperature.Text = "Last temperature:" + Format(pCurrentFocusserTemperature, "0.00") + "°"


                        'focus goto focusstar or object and focus
                        If My.Settings.sSimulatorMode = True Then
                            LogSessionEntry("ESSENTIAL", "Now focussing in debug mode... focussed.", "", "RunDeepsky", "SEQUENCE")
                            pInitialFocusTemperature = pCurrentFocusserTemperature
                        Else
                            ' if run is to abort: exit
                            If pAbort = True Then
                                pIsSequenceRunning = False
                                RunDeepskyMosaic = "NOK"
                                Exit Function
                            End If

                            returnvalue = FocusDeepsky(My.Settings.sCCDFocusClosedLoopSlew)
                            If returnvalue <> "OK" And returnvalue <> "FOCUS_ABORTED" Then
                                pIsSequenceRunning = False
                                RunDeepskyMosaic = returnvalue

                                'mark target as unusable
                                returnvalue = DatabaseMarkErrorDeepSky("Refocus failed.")
                                If returnvalue <> "OK" Then
                                    pIsSequenceRunning = False
                                    RunDeepskyMosaic = returnvalue
                                    Exit Function
                                Else
                                    'target is marked unusable, exiting
                                    pIsSequenceRunning = False
                                    RunDeepskyMosaic = "NOK"
                                    Exit Function
                                End If
                            End If
                        End If

                        '-----------------------------------------------------------------------------------
                        'following a refocus: do a closed loop slew to center target for exact coordinates
                        '-----------------------------------------------------------------------------------
                        RA2000 = pAUtil.HMSToHours(Format(pDSLTarget.TargetRA2000HH) + " " + Format(pDSLTarget.TargetRA2000MM) + " " + Format(pDSLTarget.TargetRA2000SS))
                        DEC2000 = pAUtil.DMSToDegrees(Format(pDSLTarget.TargetDEC2000DG) + " " + Format(pDSLTarget.TargetDEC2000MM) + " " + Format(pDSLTarget.TargetDEC2000SS))
                        'convert to topographic coordinates
                        ConvertTargetJ2000ToTopocentric(RA2000, DEC2000)
                        RATopocentric = pRATargetTopocentric
                        DECTopocentric = pDECTargetTopocentric

                        TopocentricRAString = pAUtil.HoursToHMS(RATopocentric, "h ", "m ", "s ")
                        TopocentricDECString = pAUtil.DegreesToDMS(DECTopocentric, "° ", "' ", """ ")
                        TopocentricRAStringTSX = pAUtil.HoursToHMS(RATopocentric, " ", " ", " ")
                        TopocentricDECStringTSX = pAUtil.DegreesToDMS(DECTopocentric, " ", " ", " ")

                        'closed loop slew to target
                        If pMountConnected = True Then
                            returnvalue = ClosedLoopSlew(RATopocentric, DECTopocentric, TopocentricRAString, TopocentricDECString, Format(pDSLTarget.TargetRA2000HH) + "h " + Format(pDSLTarget.TargetRA2000MM) + "m " + Format(pDSLTarget.TargetRA2000SS) + "s ", Format(pDSLTarget.TargetDEC2000DG) + "° " + Format(pDSLTarget.TargetDEC2000MM) + "' " + Format(pDSLTarget.TargetDEC2000SS) + """ ", pDSLTarget.TargetName, pDSLTarget.TargetFilter, False, TopocentricRAStringTSX, TopocentricDECStringTSX)

                            If returnvalue <> "OK" And returnvalue <> "SLEW_ABORTED" Then
                                'mark target as unusable
                                returnvalue = DatabaseMarkErrorDeepSky("Closed loop slew failed.")
                                If returnvalue <> "OK" Then
                                    pIsSequenceRunning = False
                                    RunDeepskyMosaic = returnvalue
                                    Exit Function
                                Else
                                    'target is marked unusable, exiting
                                    pIsSequenceRunning = False
                                    RunDeepskyMosaic = "NOK"
                                    Exit Function
                                End If
                            End If
                        End If
                    End If

                    '----------------------------------------------------------------------------------------------------
                    ' CALCULATE NEXT PANEL
                    '----------------------------------------------------------------------------------------------------

                    If pDSLTarget.TargetMosaicType = "2x2" Then
                        If CurrentPanel = 1 Then
                            If iPanel2 < NbrFramesToExposePanel2 Then
                                CurrentPanel = 2
                            ElseIf iPanel3 < NbrFramesToExposePanel3 Then
                                CurrentPanel = 3
                            ElseIf iPanel4 < NbrFramesToExposePanel4 Then
                                CurrentPanel = 4
                            ElseIf iPanel1 < NbrFramesToExposePanel1 Then
                                CurrentPanel = 1
                            End If
                        ElseIf CurrentPanel = 2 Then
                            If iPanel3 < NbrFramesToExposePanel3 Then
                                CurrentPanel = 3
                            ElseIf iPanel4 < NbrFramesToExposePanel4 Then
                                CurrentPanel = 4
                            ElseIf iPanel1 < NbrFramesToExposePanel1 Then
                                CurrentPanel = 1
                            ElseIf iPanel2 < NbrFramesToExposePanel2 Then
                                CurrentPanel = 2
                            End If
                        ElseIf CurrentPanel = 3 Then
                            If iPanel4 < NbrFramesToExposePanel4 Then
                                CurrentPanel = 4
                            ElseIf iPanel1 < NbrFramesToExposePanel1 Then
                                CurrentPanel = 1
                            ElseIf iPanel2 < NbrFramesToExposePanel2 Then
                                CurrentPanel = 2
                            ElseIf iPanel3 < NbrFramesToExposePanel3 Then
                                CurrentPanel = 3
                            End If
                        ElseIf CurrentPanel = 4 Then
                            If iPanel1 < NbrFramesToExposePanel1 Then
                                CurrentPanel = 1
                            ElseIf iPanel2 < NbrFramesToExposePanel2 Then
                                CurrentPanel = 2
                            ElseIf iPanel3 < NbrFramesToExposePanel3 Then
                                CurrentPanel = 3
                            ElseIf iPanel4 < NbrFramesToExposePanel4 Then
                                CurrentPanel = 4
                            End If
                        End If
                    Else 'all other cases only 2 panels
                        If CurrentPanel = 1 Then
                            If iPanel2 < NbrFramesToExposePanel2 Then
                                CurrentPanel = 2
                            ElseIf iPanel1 < NbrFramesToExposePanel1 Then
                                CurrentPanel = 1
                            End If
                        ElseIf CurrentPanel = 2 Then
                            If iPanel1 < NbrFramesToExposePanel1 Then
                                CurrentPanel = 1
                            ElseIf iPanel2 < NbrFramesToExposePanel2 Then
                                CurrentPanel = 2
                            End If
                        End If
                    End If

                End While
            End If

            pIsSequenceRunning = False
            'reset the counter, so a check is performed if a new target is available
            pDSLTarget.ID = 0

        Catch ex As Exception
            RunDeepskyMosaic = "RunDeepskyMosaic: " + ex.Message
            pIsSequenceRunning = False
            LogSessionEntry("ERROR", "RunDeepskyMosaic: " + ex.Message, "", "RunDeepskyMosaic", "PROGRAM")
        End Try
    End Function

    Public Function RunDeepsky(vUnsafe As Boolean) As String
        Dim i As Integer
        Dim returnvalue As String
        Dim NbrFramesToExpose As Integer
        Dim NumberOfExposures As Integer
        Dim RA2000, DEC2000 As Double
        Dim RATopocentric, DECTopocentric As Double
        Dim TopocentricRAString, TopocentricDECString As String
        Dim TopocentricRAStringTSX, TopocentricDECStringTSX As String

        Dim random As New Random()

        Dim InitialSideOfMeridian, InitialTargetNumber As Integer

        RunDeepsky = "OK"
        Try
            i = 1
            InitialTargetNumber = 0

            ' if run is to abort: exit
            If pAbort = True Then
                RunDeepsky = "NOK"
                Exit Function
            End If

            pIsSequenceRunning = True
            pRunStatus = "DEEPSKY"
            LogSessionEntry("FULL", "Running deepsky flow...", "", "CheckRun", "SEQUENCE")

            'select a target
            returnvalue = DatabaseSelectDeepSky(False, vUnsafe)
            If returnvalue <> "OK" Then
                pIsSequenceRunning = False
                RunDeepsky = "NOK"
                Exit Function
            End If

            '----------------------------------------------------------------------------------------------------
            'if a target was selected start the run
            '----------------------------------------------------------------------------------------------------
            If pDSLTarget.ID <> 0 And pDSLTarget.TargetMosaic = False Then
                '----------------------------------------------------------------------------------------------------
                'FOCUS: goto focusstar or object and focus
                '----------------------------------------------------------------------------------------------------
                If pAbort = True Then
                    pIsSequenceRunning = False
                    RunDeepsky = "NOK"
                    Exit Function
                End If
                LogSessionEntry("ESSENTIAL", "Focussing first time in run...", "", "RunDeepsky", "SEQUENCE")
                returnvalue = FocusDeepsky(My.Settings.sCCDFocusClosedLoopSlew)

                If returnvalue <> "OK" And returnvalue <> "FOCUS_ABORTED" Then
                    pIsSequenceRunning = False
                    RunDeepsky = returnvalue

                    'mark target as unusable
                    returnvalue = DatabaseMarkErrorDeepSky(returnvalue)
                    If returnvalue <> "OK" Then
                        pIsSequenceRunning = False
                        RunDeepsky = returnvalue
                        Exit Function
                    Else
                        'target is marked unusable, exiting
                        pIsSequenceRunning = False
                        RunDeepsky = "NOK"
                        Exit Function
                    End If
                End If

                FrmMain.LblLastFocusDateTime.Text = "Last focus: " + DateTime.UtcNow.ToString("dd/MM/yyyy HH:mm")
                FrmMain.LblLastFocusTemperature.Text = "Last temperature:" + Format(pCurrentFocusserTemperature, "0.00") + "°"

                '----------------------------------------------------------------------------------------------------
                'CLOSED LOOP SLEW to target
                '----------------------------------------------------------------------------------------------------
                If pDSLTarget.TargetIsComet = True Then
                    'target is a comet: check TSX to get the last set of coordinates
                    LogSessionEntry("FULL", "Retrieving current coordinates for " + pDSLTarget.TargetName, "", "RunDeepsky", "SEQUENCE")
                    returnvalue = FindTheSkyXTarget(pDSLTarget.TargetName)

                    Select Case returnvalue
                        Case "NOTFOUND"
                            LogSessionEntry("ESSENTIAL", "Comet position lookup failed ! Using predefined coordinates...", "", "RunDeepsky", "SEQUENCE")
                            'use the coordinates stored in db
                            RA2000 = pAUtil.HMSToHours(Format(pDSLTarget.TargetRA2000HH) + "h " + Format(pDSLTarget.TargetRA2000MM) + "m " + Format(pDSLTarget.TargetRA2000SS) + "s ")
                            DEC2000 = pAUtil.DMSToDegrees(Format(pDSLTarget.TargetDEC2000DG) + "° " + Format(pDSLTarget.TargetDEC2000MM) + "' " + Format(pDSLTarget.TargetDEC2000SS) + """ ")
                        Case "OK"
                            'use newly found coordinates
                            RA2000 = pStrucTargetObject.RightAscension
                            DEC2000 = pStrucTargetObject.Declination
                        Case Else
                            LogSessionEntry("ESSENTIAL", "Comet position lookup failed ! Using predefined coordinates...", "", "RunDeepsky", "SEQUENCE")
                            'use the coordinates stored in db
                            RA2000 = pAUtil.HMSToHours(Format(pDSLTarget.TargetRA2000HH) + " " + Format(pDSLTarget.TargetRA2000MM) + " " + Format(pDSLTarget.TargetRA2000SS))
                            DEC2000 = pAUtil.DMSToDegrees(Format(pDSLTarget.TargetDEC2000DG) + " " + Format(pDSLTarget.TargetDEC2000MM) + " " + Format(pDSLTarget.TargetDEC2000SS))
                    End Select
                Else
                    'use the coordinates stored in db
                    RA2000 = pAUtil.HMSToHours(Format(pDSLTarget.TargetRA2000HH) + " " + Format(pDSLTarget.TargetRA2000MM) + " " + Format(pDSLTarget.TargetRA2000SS))
                    DEC2000 = pAUtil.DMSToDegrees(Format(pDSLTarget.TargetDEC2000DG) + " " + Format(pDSLTarget.TargetDEC2000MM) + " " + Format(pDSLTarget.TargetDEC2000SS))
                End If

                'convert to topographic coordinates
                ConvertTargetJ2000ToTopocentric(RA2000, DEC2000)
                RATopocentric = pRATargetTopocentric
                DECTopocentric = pDECTargetTopocentric

                TopocentricRAString = pAUtil.HoursToHMS(RATopocentric, "h ", "m ", "s ")
                TopocentricDECString = pAUtil.DegreesToDMS(DECTopocentric, "° ", "' ", """ ")
                TopocentricRAStringTSX = pAUtil.HoursToHMS(RATopocentric, " ", " ", " ")
                TopocentricDECStringTSX = pAUtil.DegreesToDMS(DECTopocentric, " ", " ", " ")

                'retrieve filternumber
                Dim FilterNumber, Binning As Integer
                If pDSLTarget.TargetFilter = My.Settings.sCCDFilter1 Then
                    FilterNumber = 0
                ElseIf pDSLTarget.TargetFilter = My.Settings.sCCDFilter2 Then
                    FilterNumber = 1
                ElseIf pDSLTarget.TargetFilter = My.Settings.sCCDFilter3 Then
                    FilterNumber = 2
                ElseIf pDSLTarget.TargetFilter = My.Settings.sCCDFilter4 Then
                    FilterNumber = 3
                ElseIf pDSLTarget.TargetFilter = My.Settings.sCCDFilter5 Then
                    FilterNumber = 4
                End If

                ' if run is to abort: exit
                If pAbort = True Then
                    pIsSequenceRunning = False
                    RunDeepsky = "NOK"
                    Exit Function
                End If

                'closed loop slew to target
                If pMountConnected = True Then
                    returnvalue = ClosedLoopSlew(RATopocentric, DECTopocentric, TopocentricRAString, TopocentricDECString, Format(pDSLTarget.TargetRA2000HH) + "h " + Format(pDSLTarget.TargetRA2000MM) + "m " + Format(pDSLTarget.TargetRA2000SS) + "s ", Format(pDSLTarget.TargetDEC2000DG) + "° " + Format(pDSLTarget.TargetDEC2000MM) + "' " + Format(pDSLTarget.TargetDEC2000SS) + """ ", pDSLTarget.TargetName, pDSLTarget.TargetFilter, False, TopocentricRAStringTSX, TopocentricDECStringTSX)

                    If returnvalue <> "OK" And returnvalue <> "SLEW_ABORTED" Then
                        'mark target as unusable
                        returnvalue = DatabaseMarkErrorDeepSky("Closed loop slew failed.")
                        If returnvalue <> "OK" Then
                            pIsSequenceRunning = False
                            RunDeepsky = returnvalue
                            Exit Function
                        Else
                            'target is marked unusable, exiting
                            pIsSequenceRunning = False
                            RunDeepsky = "NOK"
                            Exit Function
                        End If
                    End If
                End If

                'convert binning
                If pDSLTarget.TargetBin = "1x1" Then
                    Binning = 1
                ElseIf pDSLTarget.TargetBin = "2x2" Then
                    Binning = 2
                ElseIf pDSLTarget.TargetBin = "3x3" Then
                    Binning = 3
                End If

                'how many frames are needed
                Dim oldTargetNbrFrames As Integer
                oldTargetNbrFrames = pDSLTarget.TargetNbrFrames
                NbrFramesToExpose = pDSLTarget.TargetNbrFrames - pDSLTarget.TargetNbrExposedFrames

                'set initial value
                InitialSideOfMeridian = Convert.ToInt32(pStructMount.SideOfPier)

                '-----------------------------------------------------------------------
                'loop
                '-----------------------------------------------------------------------
                While i <= NbrFramesToExpose
                    ' if run is to abort: exit
                    If pAbort = True Then
                        pIsSequenceRunning = False
                        RunDeepsky = "NOK"
                        Exit Function
                    End If

                    'check if this is still the target we want to look at
                    Dim CurrentTarget As String
                    CurrentTarget = pDSLTarget.TargetName
                    returnvalue = DatabaseSelectDeepSky(True, vUnsafe)
                    If returnvalue <> "OK" Then
                        LogSessionEntry("ERROR", "DatabaseSelectDeepSkyTarget: " + returnvalue, "", "RunDeepsky", "SEQUENCE")
                        RunDeepsky = returnvalue
                        Exit Function
                    End If

                    'how many frames are needed: if change occured during run
                    If pDSLTarget.TargetNbrFrames <> oldTargetNbrFrames Then
                        oldTargetNbrFrames = pDSLTarget.TargetNbrFrames
                        NbrFramesToExpose = pDSLTarget.TargetNbrFrames - pDSLTarget.TargetNbrExposedFrames
                        i = 1
                    End If


                    If CurrentTarget <> pDSLTarget.TargetName And pDSLTarget.TargetName <> "" Then
                        LogSessionEntry("ESSENTIAL", "Switching target to: " + pDSLTarget.TargetName, "", "RunDeepsky", "SEQUENCE")
                        pIsSequenceRunning = False
                        RunDeepsky = "NOK"
                        Exit Function
                    End If

                    ' if run is to abort: exit
                    If pAbort = True Then
                        pIsSequenceRunning = False
                        RunDeepsky = "NOK"
                        Exit Function
                    End If

                    'take an image
                    returnvalue = TakeImageTheSkyX(pDSLTarget.TargetExposure, Binning, pDSLTarget.TargetName, False, False, FilterNumber, pDSLTarget.TargetFilter, "exposure " + Format(i) + "/" + Format(NbrFramesToExpose) + " ", False, "LIGHT", TopocentricRAStringTSX, TopocentricDECStringTSX)
                    If returnvalue <> "OK" And returnvalue <> "IMAGE_ABORTED" Then
                        pIsSequenceRunning = False
                        RunDeepsky = returnvalue
                        LogSessionEntry("ESSENTIAL", "Image failed!", "", "RunDeepsky", "SEQUENCE")

                        'mark target as unusable
                        returnvalue = DatabaseMarkErrorDeepSky("Image failed.")
                        If returnvalue <> "OK" Then
                            pIsSequenceRunning = False
                            RunDeepsky = returnvalue
                            Exit Function
                        Else
                            'target is marked unusable, exiting
                            pIsSequenceRunning = False
                            RunDeepsky = "NOK"
                            Exit Function
                        End If
                    End If

                    'update the database
                    If returnvalue = "OK" Then
                        NumberOfExposures = pDSLTarget.TargetNbrExposedFrames + 1

                        returnvalue = DatabaseUpdateNbrExpDeepSky(NumberOfExposures, 0, 0, "NONE")
                        If returnvalue <> "OK" Then
                            pIsSequenceRunning = False
                            LogSessionEntry("ESSENTIAL", "Target update failed!", "", "RunDeepsky", "SEQUENCE")
                            RunDeepsky = returnvalue
                            Exit Function
                        End If
                    End If
                    i += 1

                    ' if run is to abort: exit
                    If pAbort = True Then
                        pIsSequenceRunning = False
                        RunDeepsky = "NOK"
                        Exit Function
                    End If

                    '--------------------------------------------------------
                    'dither / meridian flip: if another exposure is needed
                    '--------------------------------------------------------
                    If i <= NbrFramesToExpose Then
                        If My.Settings.sCCDDithering = True Then
                            '--------------------------------------------------------
                            'dither 
                            '--------------------------------------------------------
                            Dim DitherPixelsX, DitherPixelsY As Integer
                            Dim DitherOffsetX, DitherOffsetY As Double
                            Dim DitherRATopocentric, DitherDECTopocentric As Double
                            Dim DitherRATopocentricString, DitherDECTopocentricString As String

                            DitherPixelsX = random.Next(0, My.Settings.sCCDDitheringAmount)
                            DitherPixelsY = random.Next(0, My.Settings.sCCDDitheringAmount)
                            DitherOffsetX = DitherPixelsX * My.Settings.sCCDImageScale * (1 / Math.Cos(Math.PI * pDECMountActualTopocentric / 180.0))
                            DitherOffsetY = DitherPixelsY * My.Settings.sCCDImageScale

                            'pRAMountActualTopocentric and pDECMountActualTopocentric have been updated to the ACTUAL mount position in the closed loop slew function
                            'DitherRATopocentric = pSolveRATopocentric + (DitherOffsetX * 0.000277778) 'pRATargetActualTopocentric + (DitherOffsetX * 0.000277778) 'convert to hours
                            'DitherDECTopocentric = pSolveDECTopocentric + (DitherOffsetY * 0.000277778) 'pDECTargetActualTopocentric + (DitherOffsetY * 0.000277778) 'convert to degrees
                            DitherRATopocentric = pRAMountActualTopocentric + (DitherOffsetX * 0.000277778) 'convert to hours
                            DitherDECTopocentric = pDECMountActualTopocentric + (DitherOffsetY * 0.000277778) 'convert to degrees

                            DitherRATopocentricString = pAUtil.HoursToHMS(DitherRATopocentric, "h ", "m ", "s ")
                            DitherDECTopocentricString = pAUtil.DegreesToDMS(DitherDECTopocentric, "° ", "' ", """ ")

                            'calculate alt az
                            returnvalue = CalculateObject(DitherRATopocentric, DitherDECTopocentric)
                            If returnvalue <> "OK" Then
                                RunDeepsky = returnvalue
                                Exit Function
                            End If

                            'slew to offset target
                            LogSessionEntry("BRIEF", "Moving mount to: RA " + DitherRATopocentricString + " - DEC " + DitherDECTopocentricString + " / Alt " + Format(pStructObject.ObjectAlt, "#0.00") + "° - Az " + Format(pStructObject.ObjectAz, "#0.00") + "° - Moon Alt " + Format(pStructEventTimes.MoonAlt, "#0.00") + "°", "", "RunDeepsky", "SEQUENCE")
                            returnvalue = MountSlewToTarget(pDSLTarget.TargetName, DitherRATopocentric, DitherDECTopocentric, DitherRATopocentricString, DitherDECTopocentricString, "", "", False, False)
                            If returnvalue <> "OK" And returnvalue <> "SLEW_ABORTED" Then
                                pIsSequenceRunning = False
                                RunDeepsky = returnvalue
                                LogSessionEntry("ESSENTIAL", "Dither slew failed!", "", "RunDeepsky", "SEQUENCE")

                                'mark target as unusable
                                returnvalue = DatabaseMarkErrorDeepSky("Dither slew failed.")
                                If returnvalue <> "OK" Then
                                    pIsSequenceRunning = False
                                    RunDeepsky = returnvalue
                                    Exit Function
                                Else
                                    'target is marked unusable, exiting
                                    pIsSequenceRunning = False
                                    RunDeepsky = "NOK"
                                    Exit Function
                                End If
                            End If
                        Else
                            '--------------------------------------------------------
                            'meridian flip if needed
                            '--------------------------------------------------------
                            If pStructMount.Azimuth > 180 + (My.Settings.sMountMeridianFlipMinutes * 0.25) And Convert.ToInt32(pStructMount.SideOfPier) = 1 Then
                                'do pier flip by doing a slew
                                LogSessionEntry("ESSENTIAL", "Doing meridian flip...", "", "RunDeepsky", "SEQUENCE")
                                returnvalue = ClosedLoopSlew(RATopocentric, DECTopocentric, TopocentricRAString, TopocentricDECString, Format(pDSLTarget.TargetRA2000HH) + "h " + Format(pDSLTarget.TargetRA2000MM) + "m " + Format(pDSLTarget.TargetRA2000SS) + "s ", Format(pDSLTarget.TargetDEC2000DG) + "° " + Format(pDSLTarget.TargetDEC2000MM) + "' " + Format(pDSLTarget.TargetDEC2000SS) + """ ", pDSLTarget.TargetName, pDSLTarget.TargetFilter, False, TopocentricRAStringTSX, TopocentricDECStringTSX)

                                ' if run is to abort: exit
                                If returnvalue <> "OK" And returnvalue <> "SLEW_ABORTED" Then
                                    pIsSequenceRunning = False
                                    RunDeepsky = returnvalue
                                    LogSessionEntry("ESSENTIAL", "Meridian flip failed!", "", "RunDeepsky", "SEQUENCE")

                                    'mark target as unusable
                                    returnvalue = DatabaseMarkErrorDeepSky("Meridian flip failed.")
                                    If returnvalue <> "OK" Then
                                        pIsSequenceRunning = False
                                        RunDeepsky = returnvalue
                                        Exit Function
                                    Else
                                        'target is marked unusable, exiting
                                        pIsSequenceRunning = False
                                        RunDeepsky = "NOK"
                                        Exit Function
                                    End If
                                End If
                            End If
                        End If
                    End If


                    '-----------------------------------------------------------------------
                    'refocus following meridian flip
                    'refocus if temperature changed too much
                    'refocus if number of exposure reached
                    '-----------------------------------------------------------------------
                    If (InitialSideOfMeridian <> Convert.ToInt32(pStructMount.SideOfPier) And My.Settings.sCCDFocusMeridianFlip = True) Or
                        Math.Abs(pCurrentFocusserTemperature - pInitialFocusTemperature) >= My.Settings.sCCDFocusEveryXDegrees Or
                       i > My.Settings.sCCDFocusEveryXImages + InitialTargetNumber Then

                        If InitialSideOfMeridian <> Convert.ToInt32(pStructMount.SideOfPier) And My.Settings.sCCDFocusMeridianFlip = True Then
                            LogSessionEntry("ESSENTIAL", "Focussing following pier flip...", "", "RunDeepsky", "SEQUENCE")
                        ElseIf Math.Abs(pCurrentFocusserTemperature - pInitialFocusTemperature) >= My.Settings.sCCDFocusEveryXDegrees Then
                            LogSessionEntry("ESSENTIAL", "Focussing due to temperature change: " + Format(pInitialFocusTemperature, "##0.00") + "° -> " + Format(pCurrentFocusserTemperature, "##0.00") + "°C", "", "RunDeepsky", "SEQUENCE")
                        ElseIf i > My.Settings.sCCDFocusEveryXImages + InitialTargetNumber Then
                            LogSessionEntry("ESSENTIAL", "Focussing following " + Format(My.Settings.sCCDFocusEveryXImages) + " exposures...", "", "RunDeepsky", "SEQUENCE")
                        Else
                            LogSessionEntry("ESSENTIAL", "Focussing for unknown reason...", "", "RunDeepsky", "SEQUENCE")
                        End If

                        InitialSideOfMeridian = Convert.ToInt32(pStructMount.SideOfPier)
                        InitialTargetNumber = i

                        FrmMain.LblLastFocusDateTime.Text = "Last focus: " + Format(DateTime.UtcNow)
                        FrmMain.LblLastFocusTemperature.Text = "Last temperature:" + Format(pCurrentFocusserTemperature, "0.00") + "°"

                        'focus goto focusstar or object and focus
                        If My.Settings.sSimulatorMode = True Then
                            LogSessionEntry("ESSENTIAL", "Now focussing in debug mode... focussed.", "", "RunDeepsky", "SEQUENCE")
                            pInitialFocusTemperature = pCurrentFocusserTemperature
                        Else
                            ' if run is to abort: exit
                            If pAbort = True Then
                                pIsSequenceRunning = False
                                RunDeepsky = "NOK"
                                Exit Function
                            End If

                            returnvalue = FocusDeepsky(My.Settings.sCCDFocusClosedLoopSlew)
                            If returnvalue <> "OK" And returnvalue <> "FOCUS_ABORTED" Then
                                pIsSequenceRunning = False
                                RunDeepsky = returnvalue

                                'mark target as unusable
                                returnvalue = DatabaseMarkErrorDeepSky("Refocus failed.")
                                If returnvalue <> "OK" Then
                                    pIsSequenceRunning = False
                                    RunDeepsky = returnvalue
                                    Exit Function
                                Else
                                    'target is marked unusable, exiting
                                    pIsSequenceRunning = False
                                    RunDeepsky = "NOK"
                                    Exit Function
                                End If
                            End If
                        End If

                        'when refocussed do a closed loop slew to target
                        If pMountConnected = True Then
                            ' if run is to abort: exit
                            If pAbort = True Then
                                pIsSequenceRunning = False
                                RunDeepsky = "NOK"
                                Exit Function
                            End If

                            returnvalue = ClosedLoopSlew(RATopocentric, DECTopocentric, TopocentricRAString, TopocentricDECString, Format(pDSLTarget.TargetRA2000HH) + "h " + Format(pDSLTarget.TargetRA2000MM) + "m " + Format(pDSLTarget.TargetRA2000SS) + "s ", Format(pDSLTarget.TargetDEC2000DG) + "° " + Format(pDSLTarget.TargetDEC2000MM) + "' " + Format(pDSLTarget.TargetDEC2000SS) + """ ", pDSLTarget.TargetName, pDSLTarget.TargetFilter, False, TopocentricRAStringTSX, TopocentricDECStringTSX)

                            If returnvalue <> "OK" And returnvalue <> "SLEW_ABORTED" Then
                                pIsSequenceRunning = False
                                RunDeepsky = returnvalue
                                LogSessionEntry("ESSENTIAL", "Closed loop slew following refocus failed!", "", "RunDeepsky", "SEQUENCE")

                                'mark target as unusable
                                returnvalue = DatabaseMarkErrorDeepSky("Closed loop slew following refocus failed.")
                                If returnvalue <> "OK" Then
                                    pIsSequenceRunning = False
                                    RunDeepsky = returnvalue
                                    Exit Function
                                Else
                                    'target is marked unusable, exiting
                                    pIsSequenceRunning = False
                                    RunDeepsky = "NOK"
                                    Exit Function
                                End If
                            End If
                        End If
                    End If
                End While
                LogSessionEntry("ESSENTIAL", "Done: " + pDSLTarget.TargetName + " exposed " + Format(i - 1) + " out of " + Format(NbrFramesToExpose) + " frames: selecting new target.", "", "RunDeepsky", "SEQUENCE")
            End If

            pIsSequenceRunning = False
            'reset the counter, so a check is performed if a new target is available
            pDSLTarget.ID = 0

        Catch ex As Exception
            RunDeepsky = "RunDeepsky: " + ex.Message
            pIsSequenceRunning = False
            LogSessionEntry("ERROR", "RunDeepsky: " + ex.Message, "", "RunDeepsky", "PROGRAM")
        End Try
    End Function


    Public Function FocusDeepsky(vClosedLoopSlew As Boolean) As String
        'focus deepsky target
        Dim returnvalue As String
        Dim RA2000, DEC2000 As Double
        Dim TopocentricRAString, TopocentricDECString, TargetName As String
        Dim TargetRA2000HH, TargetRA2000MM, TargetRA2000SS As Double
        Dim TargetDEC2000DG, TargetDEC2000MM, TargetDEC2000SS As Double
        Dim TopocentricRAStringTSX, TopocentricDECStringTSX As String
        Dim Focusstar As Boolean

        FocusDeepsky = "OK"
        Try
            '----------------------------------------------------------------------------------------------------
            'if a focusstar is defined, slew to focusstar
            '----------------------------------------------------------------------------------------------------
            ' if run is to abort: exit
            If pAbort = True Then
                FocusDeepsky = "FOCUS_ABORTED"
                Exit Function
            End If

            If pDSLTarget.FocusStarName <> "" Then
                'use focusstar
                RA2000 = pAUtil.HMSToHours(Format(pDSLTarget.FocusStarRA2000HH) + " " + Format(pDSLTarget.FocusStarRA2000MM) + " " + Format(pDSLTarget.FocusStarRA2000SS))
                DEC2000 = pAUtil.DMSToDegrees(Format(pDSLTarget.FocusStarDEC2000DG) + " " + Format(pDSLTarget.FocusStarDEC2000MM) + " " + Format(pDSLTarget.FocusStarDEC2000SS))

                TargetRA2000HH = Double.Parse(pDSLTarget.FocusStarRA2000HH)
                TargetRA2000MM = Double.Parse(pDSLTarget.FocusStarRA2000MM)
                TargetRA2000SS = Double.Parse(pDSLTarget.FocusStarRA2000SS)
                TargetDEC2000DG = Double.Parse(pDSLTarget.FocusStarDEC2000DG)
                TargetDEC2000MM = Double.Parse(pDSLTarget.FocusStarDEC2000MM)
                TargetDEC2000SS = Double.Parse(pDSLTarget.FocusStarDEC2000SS)
                TargetName = pDSLTarget.FocusStarName
                Focusstar = True
            Else
                'use object
                RA2000 = pAUtil.HMSToHours(Format(pDSLTarget.TargetRA2000HH) + " " + Format(pDSLTarget.TargetRA2000MM) + " " + Format(pDSLTarget.TargetRA2000SS))
                DEC2000 = pAUtil.DMSToDegrees(Format(pDSLTarget.TargetDEC2000DG) + " " + Format(pDSLTarget.TargetDEC2000MM) + " " + Format(pDSLTarget.TargetDEC2000SS))

                TargetRA2000HH = Double.Parse(pDSLTarget.TargetRA2000HH)
                TargetRA2000MM = Double.Parse(pDSLTarget.TargetRA2000MM)
                TargetRA2000SS = Double.Parse(pDSLTarget.TargetRA2000SS)
                TargetDEC2000DG = Double.Parse(pDSLTarget.TargetDEC2000DG)
                TargetDEC2000MM = Double.Parse(pDSLTarget.TargetDEC2000MM)
                TargetDEC2000SS = Double.Parse(pDSLTarget.TargetDEC2000SS)
                TargetName = pDSLTarget.TargetName
                Focusstar = False
            End If

            'convert to topographic coordinates
            returnvalue = ConvertFocusJ2000ToTopocentric(RA2000, DEC2000)
            If returnvalue <> "OK" Then
                LogSessionEntry("ERROR", "Focus target coordinate conversion failed!", "", "FocusDeepsky", "SEQUENCE")
                FocusDeepsky = "Focus target coordinate conversion failed!"
                Exit Function
            End If

            TopocentricRAString = pAUtil.HoursToHMS(pRAFocusTopocentric, "h ", "m ", "s ")
            TopocentricDECString = pAUtil.DegreesToDMS(pDECFocusTopocentric, "° ", "' ", """ ")
            TopocentricRAStringTSX = pAUtil.HoursToHMS(pRAFocusTopocentric, " ", " ", " ")
            TopocentricDECStringTSX = pAUtil.DegreesToDMS(pDECFocusTopocentric, " ", " ", " ")

            'if target is below horizon: abort !
            returnvalue = CalculateObject(RA2000, DEC2000)
            If returnvalue <> "OK" Then
                LogSessionEntry("ERROR", "Focus target altitude / azimuth calculation failed!", "", "FocusDeepsky", "SEQUENCE")
                FocusDeepsky = "Focus target altitude / azimuth calculation failed!"
                Exit Function
            End If

            ' if run is to abort: exit
            If pAbort = True Then
                FocusDeepsky = "FOCUS_ABORTED"
                Exit Function
            End If

            'retrieve filternumber
            Dim FilterNumber, Binning As Integer
            If pDSLTarget.TargetFilter = My.Settings.sCCDFilter1 Then
                FilterNumber = 0
            ElseIf pDSLTarget.TargetFilter = My.Settings.sCCDFilter2 Then
                FilterNumber = 1
            ElseIf pDSLTarget.TargetFilter = My.Settings.sCCDFilter3 Then
                FilterNumber = 2
            ElseIf pDSLTarget.TargetFilter = My.Settings.sCCDFilter4 Then
                FilterNumber = 3
            ElseIf pDSLTarget.TargetFilter = My.Settings.sCCDFilter5 Then
                FilterNumber = 4
            End If


            'slew to target
            If vClosedLoopSlew = True Then
                returnvalue = ClosedLoopSlew(pRAFocusTopocentric, pDECFocusTopocentric, TopocentricRAString, TopocentricDECString, Format(pDSLTarget.TargetRA2000HH) + " " + Format(TargetRA2000MM) + " " + Format(TargetRA2000SS), Format(TargetDEC2000DG) + " " + Format(TargetDEC2000MM) + " " + Format(TargetDEC2000SS), TargetName, pDSLTarget.TargetFilter, False, TopocentricRAStringTSX, TopocentricDECStringTSX)
            Else
                returnvalue = MountSlewToTarget(TargetName, pRAFocusTopocentric, pDECFocusTopocentric, TopocentricRAString, TopocentricDECString, Format(pDSLTarget.TargetRA2000HH) + " " + Format(TargetRA2000MM) + " " + Format(TargetRA2000SS), Format(TargetDEC2000DG) + " " + Format(TargetDEC2000MM) + " " + Format(TargetDEC2000SS), False, False)
            End If

            If returnvalue <> "OK" And returnvalue <> "SLEW_ABORTED" Then
                LogSessionEntry("ERROR", "Focus slew failed!", "", "RunDeepsky", "SEQUENCE")
                FocusDeepsky = "Focus slew failed."
                Exit Function
            End If

            'convert binning
            If pDSLTarget.TargetBin = "1x1" Then
                Binning = 1
            ElseIf pDSLTarget.TargetBin = "2x2" Then
                Binning = 2
            ElseIf pDSLTarget.TargetBin = "3x3" Then
                Binning = 3
            End If

            ' if run is to abort: exit
            If pAbort = True Then
                FocusDeepsky = "FOCUS_ABORTED"
                Exit Function
            End If

            'set the new temperature focussed on
            pInitialFocusTemperature = pCurrentFocusserTemperature

            'focus
            returnvalue = TheSkyXAtFocus3("Manual", My.Settings.sCCDFocusSamples, pDSLTarget.FocusStarExposure, Binning, True, FilterNumber)
            If returnvalue <> "OK" And returnvalue <> "Error code = 5 (5). No additional information is available." Then
                LogSessionEntry("ESSENTIAL", "Retrying focus...", "", "RunDeepsky", "SEQUENCE")
                returnvalue = TheSkyXAtFocus3("Manual", 1, pDSLTarget.FocusStarExposure, Binning, True, FilterNumber)
                If returnvalue <> "OK" And returnvalue <> "Error code = 5 (5). No additional information is available." Then
                    LogSessionEntry("ESSENTIAL", "Focus failed a second time, continuing with other target...", "", "RunDeepsky", "SEQUENCE")
                    FocusDeepsky = "Focus failed a second time, continuing with other target..."
                    Exit Function
                ElseIf returnvalue = "Error code = 5 (5). No additional information is available." Then
                    FocusDeepsky = "FOCUS_ABORTED"
                    Exit Function
                End If
            ElseIf returnvalue = "Error code = 5 (5). No additional information is available." Then
                FocusDeepsky = "FOCUS_ABORTED"
                Exit Function
            End If
        Catch ex As Exception
            FocusDeepsky = "FocusDeepsky: " + ex.Message
            LogSessionEntry("ERROR", "FocusDeepsky: " + ex.Message, "", "FocusDeepsky", "PROGRAM")
        End Try
    End Function


    Public Function FocusHADS(vClosedLoopSlew As Boolean) As String
        'focus deepsky target
        Dim returnvalue As String
        Dim RA2000, DEC2000 As Double
        Dim TopocentricRAString, TopocentricDECString, TargetName As String
        Dim TargetRA2000HH, TargetRA2000MM, TargetRA2000SS As Double
        Dim TargetDEC2000DG, TargetDEC2000MM, TargetDEC2000SS As Double
        Dim TopocentricRAStringTSX, TopocentricDECStringTSX As String
        Dim Focusstar As Boolean

        FocusHADS = "OK"
        Try
            '----------------------------------------------------------------------------------------------------
            'if a focusstar is defined, slew to focusstar
            '----------------------------------------------------------------------------------------------------
            ' if run is to abort: exit
            If pAbort = True Then
                FocusHADS = "FOCUS_ABORTED"
                Exit Function
            End If

            If pVSLTarget.FocusStarName <> "" Then
                'use focusstar
                RA2000 = pAUtil.HMSToHours(Format(pVSLTarget.FocusStarRA2000HH) + " " + Format(pVSLTarget.FocusStarRA2000MM) + " " + Format(pVSLTarget.FocusStarRA2000SS))
                DEC2000 = pAUtil.DMSToDegrees(Format(pVSLTarget.FocusStarDEC2000DG) + " " + Format(pVSLTarget.FocusStarDEC2000MM) + " " + Format(pVSLTarget.FocusStarDEC2000SS))

                TargetRA2000HH = Double.Parse(pVSLTarget.FocusStarRA2000HH)
                TargetRA2000MM = Double.Parse(pVSLTarget.FocusStarRA2000MM)
                TargetRA2000SS = Double.Parse(pVSLTarget.FocusStarRA2000SS)
                TargetDEC2000DG = Double.Parse(pVSLTarget.FocusStarDEC2000DG)
                TargetDEC2000MM = Double.Parse(pVSLTarget.FocusStarDEC2000MM)
                TargetDEC2000SS = Double.Parse(pVSLTarget.FocusStarDEC2000SS)
                TargetName = pVSLTarget.FocusStarName
                Focusstar = True
            Else
                'use object
                RA2000 = pAUtil.HMSToHours(Format(pVSLTarget.HADSRA2000HH) + " " + Format(pVSLTarget.HADSRA2000MM) + " " + Format(pVSLTarget.HADSRA2000SS))
                DEC2000 = pAUtil.DMSToDegrees(Format(pVSLTarget.HADSDEC2000DG) + " " + Format(pVSLTarget.HADSDEC2000MM) + " " + Format(pVSLTarget.HADSDEC2000SS))

                TargetRA2000HH = Double.Parse(pVSLTarget.HADSRA2000HH)
                TargetRA2000MM = Double.Parse(pVSLTarget.HADSRA2000MM)
                TargetRA2000SS = Double.Parse(pVSLTarget.HADSRA2000SS)
                TargetDEC2000DG = Double.Parse(pVSLTarget.HADSDEC2000DG)
                TargetDEC2000MM = Double.Parse(pVSLTarget.HADSDEC2000MM)
                TargetDEC2000SS = Double.Parse(pVSLTarget.HADSDEC2000SS)
                TargetName = pVSLTarget.HADSName
                Focusstar = False
            End If

            'convert to topographic coordinates
            returnvalue = ConvertFocusJ2000ToTopocentric(RA2000, DEC2000)
            If returnvalue <> "OK" Then
                LogSessionEntry("ERROR", "Focus target coordinate conversion failed!", "", "FocusHADS", "SEQUENCE")
                FocusHADS = "Focus target coordinate conversion failed!"
                Exit Function
            End If

            TopocentricRAString = pAUtil.HoursToHMS(pRAFocusTopocentric, "h ", "m ", "s ")
            TopocentricDECString = pAUtil.DegreesToDMS(pDECFocusTopocentric, "° ", "' ", """ ")
            TopocentricRAStringTSX = pAUtil.HoursToHMS(pRAFocusTopocentric, " ", " ", " ")
            TopocentricDECStringTSX = pAUtil.DegreesToDMS(pDECFocusTopocentric, " ", " ", " ")

            'if target is below horizon: abort !
            returnvalue = CalculateObject(RA2000, DEC2000)
            If returnvalue <> "OK" Then
                LogSessionEntry("ERROR", "Focus target altitude / azimuth calculation failed!", "", "FocusHADS", "SEQUENCE")
                FocusHADS = "Focus target altitude / azimuth calculation failed!"
                Exit Function
            End If

            ' if run is to abort: exit
            If pAbort = True Then
                FocusHADS = "FOCUS_ABORTED"
                Exit Function
            End If

            'slew to target
            'returnvalue = MountSlewToTarget(TargetName, pRAFocusTopocentric, pDECFocusTopocentric, TopocentricRAString, TopocentricDECString, Format(pVSLTarget.HADSRA2000HH) + " " + Format(TargetRA2000MM) + " " + Format(TargetRA2000SS), Format(TargetDEC2000DG) + " " + Format(TargetDEC2000MM) + " " + Format(TargetDEC2000SS), True, False)

            'slew to target
            If vClosedLoopSlew = True Then
                returnvalue = ClosedLoopSlew(pRAFocusTopocentric, pDECFocusTopocentric, TopocentricRAString, TopocentricDECString, Format(pDSLTarget.TargetRA2000HH) + " " + Format(TargetRA2000MM) + " " + Format(TargetRA2000SS), Format(TargetDEC2000DG) + " " + Format(TargetDEC2000MM) + " " + Format(TargetDEC2000SS), TargetName, pDSLTarget.TargetFilter, False, TopocentricRAStringTSX, TopocentricDECStringTSX)
            Else
                returnvalue = MountSlewToTarget(TargetName, pRAFocusTopocentric, pDECFocusTopocentric, TopocentricRAString, TopocentricDECString, Format(pDSLTarget.TargetRA2000HH) + " " + Format(TargetRA2000MM) + " " + Format(TargetRA2000SS), Format(TargetDEC2000DG) + " " + Format(TargetDEC2000MM) + " " + Format(TargetDEC2000SS), False, False)
            End If
            If returnvalue <> "OK" And returnvalue <> "SLEW_ABORTED" Then
                LogSessionEntry("ERROR", "Focus slew failed!", "", "FocusHADS", "SEQUENCE")
                FocusHADS = "Focus slew failed."
                Exit Function
            End If

            'retrieve filternumber
            Dim FilterNumber, Binning As Integer
            If pVSLTarget.HADSFilter = My.Settings.sCCDFilter1 Then
                FilterNumber = 0
            ElseIf pVSLTarget.HADSFilter = My.Settings.sCCDFilter2 Then
                FilterNumber = 1
            ElseIf pVSLTarget.HADSFilter = My.Settings.sCCDFilter3 Then
                FilterNumber = 2
            ElseIf pVSLTarget.HADSFilter = My.Settings.sCCDFilter4 Then
                FilterNumber = 3
            ElseIf pVSLTarget.HADSFilter = My.Settings.sCCDFilter5 Then
                FilterNumber = 4
            End If

            'convert binning
            If pVSLTarget.HADSBin = "1x1" Then
                Binning = 1
            ElseIf pVSLTarget.HADSBin = "2x2" Then
                Binning = 2
            ElseIf pVSLTarget.HADSBin = "3x3" Then
                Binning = 3
            End If

            ' if run is to abort: exit
            If pAbort = True Then
                FocusHADS = "FOCUS_ABORTED"
                Exit Function
            End If

            'set the new temperature focussed on
            pInitialFocusTemperature = pCurrentFocusserTemperature

            'focus
            returnvalue = TheSkyXAtFocus3("Manual", My.Settings.sCCDFocusSamples, pVSLTarget.FocusStarExposure, Binning, True, FilterNumber)
            If returnvalue <> "OK" And returnvalue <> "Error code = 5 (5). No additional information is available." Then
                LogSessionEntry("ESSENTIAL", "Retrying focus...", "", "FocusHADS", "SEQUENCE")
                returnvalue = TheSkyXAtFocus3("Manual", 1, pVSLTarget.FocusStarExposure, Binning, True, FilterNumber)
                If returnvalue <> "OK" And returnvalue <> "Error code = 5 (5). No additional information is available." Then
                    LogSessionEntry("ESSENTIAL", "Focus failed a second time, continuing with other target...", "", "FocusHADS", "SEQUENCE")
                    FocusHADS = "Focus failed a second time, continuing with other target..."
                    Exit Function
                ElseIf returnvalue = "Error code = 5 (5). No additional information is available." Then
                    FocusHADS = "FOCUS_ABORTED"
                    Exit Function
                End If
            ElseIf returnvalue = "Error code = 5 (5). No additional information is available." Then
                FocusHADS = "FOCUS_ABORTED"
                Exit Function
            End If
        Catch ex As Exception
            FocusHADS = "FocusHADS: " + ex.Message
            LogSessionEntry("ERROR", "FocusHADS: " + ex.Message, "", "FocusHADS", "PROGRAM")
        End Try
    End Function


    Public Function RunHADS() As String
        Dim i As Integer
        Dim returnvalue As String
        Dim RA2000, DEC2000 As Double
        Dim RATopocentric, DECTopocentric As Double
        Dim TopocentricRAString, TopocentricDECString As String
        Dim TopocentricRAStringTSX, TopocentricDECStringTSX As String
        Dim random As New Random()
        Dim InitialSideOfMeridian, InitialTargetNumber As Integer

        RunHADS = "OK"
        Try
            i = 1
            InitialTargetNumber = 0

            ' if run is to abort: exit
            If pAbort = True Then
                RunHADS = "NOK"
                Exit Function
            End If

            pIsSequenceRunning = True
            pRunStatus = "VARIABLES"
            LogSessionEntry("DEBUG", "Set pRunStatus to VARIABLES", "", "RunHADS", "SEQUENCE")

            'select a target
            LogSessionEntry("BRIEF", "Selecting HADS-target", "", "RunHADS", "SEQUENCE")
            returnvalue = DatabaseSelectHADS(False)
            If returnvalue <> "OK" Then
                pIsSequenceRunning = False
                RunHADS = "NOK"
                Exit Function
            End If

            '----------------------------------------------------------------------------------------------------
            'if a HADS was selected start the run
            '----------------------------------------------------------------------------------------------------
            If pVSLTarget.ID <> 0 Then
                '----------------------------------------------------------------------------------------------------
                'FOCUS: goto focusstar or object and focus
                '----------------------------------------------------------------------------------------------------
                If pAbort = True Then
                    pIsSequenceRunning = False
                    RunHADS = "NOK"
                    Exit Function
                End If
                LogSessionEntry("ESSENTIAL", "Focussing first time in run...", "", "RunHADS", "SEQUENCE")
                returnvalue = FocusHADS(My.Settings.sCCDFocusClosedLoopSlew)

                If returnvalue <> "OK" And returnvalue <> "FOCUS_ABORTED" Then
                    pIsSequenceRunning = False
                    RunHADS = returnvalue

                    'mark HADS as unusable
                    returnvalue = DatabaseMarkErrorHADS(returnvalue)
                    If returnvalue <> "OK" Then
                        pIsSequenceRunning = False
                        RunHADS = returnvalue
                        Exit Function
                    Else
                        'HADS is marked unusable, exiting
                        pIsSequenceRunning = False
                        RunHADS = "NOK"
                        Exit Function
                    End If
                End If

                FrmMain.LblLastFocusDateTime.Text = "Last focus: " + DateTime.UtcNow.ToString("dd/MM/yyyy HH:mm")
                FrmMain.LblLastFocusTemperature.Text = "Last temperature:" + Format(pCurrentFocusserTemperature, "0.00") + "°"

                '----------------------------------------------------------------------------------------------------
                'CLOSED LOOP SLEW to HADS
                '----------------------------------------------------------------------------------------------------
                RA2000 = pAUtil.HMSToHours(Format(pVSLTarget.HADSRA2000HH) + " " + Format(pVSLTarget.HADSRA2000MM) + " " + Format(pVSLTarget.HADSRA2000SS))
                DEC2000 = pAUtil.DMSToDegrees(Format(pVSLTarget.HADSDEC2000DG) + " " + Format(pVSLTarget.HADSDEC2000MM) + " " + Format(pVSLTarget.HADSDEC2000SS))

                'convert to topographic coordinates
                ConvertTargetJ2000ToTopocentric(RA2000, DEC2000)
                RATopocentric = pRATargetTopocentric
                DECTopocentric = pDECTargetTopocentric

                TopocentricRAString = pAUtil.HoursToHMS(RATopocentric, "h ", "m ", "s ")
                TopocentricDECString = pAUtil.DegreesToDMS(DECTopocentric, "° ", "' ", """ ")
                TopocentricRAStringTSX = pAUtil.HoursToHMS(RATopocentric, " ", " ", " ")
                TopocentricDECStringTSX = pAUtil.DegreesToDMS(DECTopocentric, " ", " ", " ")

                'if target is below horizon: abort !
                returnvalue = CalculateObject(RA2000, DEC2000)
                If returnvalue <> "OK" Then
                    pIsSequenceRunning = False
                    RunHADS = returnvalue
                    LogSessionEntry("ESSENTIAL", "HADS altitude/azimuth calculation failed!", "", "RunHADS", "SEQUENCE")
                End If

                'retrieve filternumber
                Dim FilterNumber, Binning As Integer
                If pVSLTarget.HADSFilter = My.Settings.sCCDFilter1 Then
                    FilterNumber = 0
                ElseIf pVSLTarget.HADSFilter = My.Settings.sCCDFilter2 Then
                    FilterNumber = 1
                ElseIf pVSLTarget.HADSFilter = My.Settings.sCCDFilter3 Then
                    FilterNumber = 2
                ElseIf pVSLTarget.HADSFilter = My.Settings.sCCDFilter4 Then
                    FilterNumber = 3
                ElseIf pVSLTarget.HADSFilter = My.Settings.sCCDFilter5 Then
                    FilterNumber = 4
                End If

                ' if run is to abort: exit
                If pAbort = True Then
                    pIsSequenceRunning = False
                    RunHADS = "NOK"
                    Exit Function
                End If

                'closed loop slew to target
                If pMountConnected = True Then
                    returnvalue = ClosedLoopSlew(RATopocentric, DECTopocentric, TopocentricRAString, TopocentricDECString, Format(pVSLTarget.HADSRA2000HH) + "h " + Format(pVSLTarget.HADSRA2000MM) + "m " + Format(pVSLTarget.HADSRA2000SS) + "s ", Format(pVSLTarget.HADSDEC2000DG) + "° " + Format(pVSLTarget.HADSDEC2000MM) + "' " + Format(pVSLTarget.HADSDEC2000SS) + """ ", pVSLTarget.HADSName, pVSLTarget.HADSFilter, False, TopocentricRAStringTSX, TopocentricDECStringTSX)

                    If returnvalue <> "OK" And returnvalue <> "SLEW_ABORTED" Then
                        'mark HADS as unusable
                        returnvalue = DatabaseMarkErrorHADS("Closed loop slew failed.")
                        If returnvalue <> "OK" Then
                            pIsSequenceRunning = False
                            RunHADS = returnvalue
                            Exit Function
                        Else
                            'HADS is marked unusable, exiting
                            pIsSequenceRunning = False
                            RunHADS = "NOK"
                            Exit Function
                        End If
                    End If
                End If

                'convert binning
                If pVSLTarget.HADSBin = "1x1" Then
                    Binning = 1
                ElseIf pVSLTarget.HADSBin = "2x2" Then
                    Binning = 2
                ElseIf pVSLTarget.HADSBin = "3x3" Then
                    Binning = 3
                End If

                'set initial value
                InitialSideOfMeridian = Convert.ToInt32(pStructMount.SideOfPier)

                '-----------------------------------------------------------------------
                'loop
                '-----------------------------------------------------------------------
                While pStructMount.Altitude >= My.Settings.sObjectAltLimitEast
                    ' if run is to abort: exit
                    If pAbort = True Then
                        pIsSequenceRunning = False
                        RunHADS = "NOK"
                        Exit Function
                    End If

                    'take an image
                    returnvalue = TakeImageTheSkyX(pVSLTarget.HADSExposure, Binning, pVSLTarget.HADSName, False, False, FilterNumber, pVSLTarget.HADSFilter, "exposure #" + Format(i), False, "LIGHT", TopocentricRAStringTSX, TopocentricDECStringTSX)
                    If returnvalue <> "OK" And returnvalue <> "IMAGE_ABORTED" Then
                        pIsSequenceRunning = False
                        RunHADS = returnvalue
                        LogSessionEntry("ESSENTIAL", "Image failed!", "", "RunHADS", "SEQUENCE")

                        'mark HADS as unusable
                        returnvalue = DatabaseMarkErrorHADS("Image failed.")
                        If returnvalue <> "OK" Then
                            pIsSequenceRunning = False
                            RunHADS = returnvalue
                            Exit Function
                        Else
                            'HADS is marked unusable, exiting
                            pIsSequenceRunning = False
                            RunHADS = "NOK"
                            Exit Function
                        End If
                    Else
                        'set the last observed date
                        returnvalue = DatabaseSetLastObservedHADS(pVSLTarget.ID)
                        If returnvalue <> "OK" Then
                            pIsSequenceRunning = False
                            RunHADS = returnvalue
                            Exit Function
                        End If
                    End If
                    i += 1

                    ' if run is to abort: exit
                    If pAbort = True Then
                        pIsSequenceRunning = False
                        RunHADS = "NOK"
                        Exit Function
                    End If

                    '--------------------------------------------------------
                    'dither / meridian flip: if another exposure is needed
                    '--------------------------------------------------------
                    If My.Settings.sCCDDithering = True Then
                        '--------------------------------------------------------
                        'dither 
                        '--------------------------------------------------------
                        Dim DitherPixelsX, DitherPixelsY As Integer
                        Dim DitherOffsetX, DitherOffsetY As Double
                        Dim DitherRATopocentric, DitherDECTopocentric As Double
                        Dim DitherRATopocentricString, DitherDECTopocentricString As String

                        DitherPixelsX = random.Next(0, My.Settings.sCCDDitheringAmount)
                        DitherPixelsY = random.Next(0, My.Settings.sCCDDitheringAmount)
                        DitherOffsetX = DitherPixelsX * My.Settings.sCCDImageScale * (1 / Math.Cos(Math.PI * pDECMountActualTopocentric / 180.0))
                        DitherOffsetY = DitherPixelsY * My.Settings.sCCDImageScale

                        'pRAMountActualTopocentric and pDECMountActualTopocentric have been updated to the ACTUAL position in the closed loop slew function
                        'DitherRATopocentric = pSolveRATopocentric + (DitherOffsetX * 0.000277778) 'pRATargetActualTopocentric + (DitherOffsetX * 0.000277778) 'convert to hours
                        'DitherDECTopocentric = pSolveDECTopocentric + (DitherOffsetY * 0.000277778) 'pDECTargetActualTopocentric + (DitherOffsetY * 0.000277778) 'convert to degrees
                        DitherRATopocentric = pRAMountActualTopocentric + (DitherOffsetX * 0.000277778) 'convert to hours
                        DitherDECTopocentric = pDECMountActualTopocentric + (DitherOffsetY * 0.000277778) 'convert to degrees

                        DitherRATopocentricString = pAUtil.HoursToHMS(DitherRATopocentric, "h ", "m ", "s ")
                        DitherDECTopocentricString = pAUtil.DegreesToDMS(DitherDECTopocentric, "° ", "' ", """ ")

                        'calculate alt az
                        returnvalue = CalculateObject(DitherRATopocentric, DitherDECTopocentric)
                        If returnvalue <> "OK" Then
                            RunHADS = returnvalue
                            Exit Function
                        End If

                        'slew to offset target
                        LogSessionEntry("BRIEF", "Moving mount to: RA " + DitherRATopocentricString + " - DEC " + DitherDECTopocentricString + " / Alt " + Format(pStructObject.ObjectAlt, "#0.00") + "° - Az " + Format(pStructObject.ObjectAz, "#0.00") + "° - Moon Alt " + Format(pStructEventTimes.MoonAlt, "#0.00") + "°", "", "RunHADS", "SEQUENCE")
                        returnvalue = MountSlewToTarget(pVSLTarget.HADSName, DitherRATopocentric, DitherDECTopocentric, DitherRATopocentricString, DitherDECTopocentricString, "", "", False, False)
                        If returnvalue <> "OK" And returnvalue <> "SLEW_ABORTED" Then
                            pIsSequenceRunning = False
                            RunHADS = returnvalue
                            LogSessionEntry("ESSENTIAL", "Dither slew failed!", "", "RunHADS", "SEQUENCE")

                            'mark HADS as unusable
                            returnvalue = DatabaseMarkErrorHADS("Dither slew failed.")
                            If returnvalue <> "OK" Then
                                pIsSequenceRunning = False
                                RunHADS = returnvalue
                                Exit Function
                            Else
                                'HADS is marked unusable, exiting
                                pIsSequenceRunning = False
                                RunHADS = "NOK"
                                Exit Function
                            End If
                        End If
                    Else
                        '--------------------------------------------------------
                        'meridian flip if needed
                        '--------------------------------------------------------
                        If pStructMount.Azimuth > 180 + (My.Settings.sMountMeridianFlipMinutes * 0.25) And Convert.ToInt32(pStructMount.SideOfPier) = 1 Then
                            'do pier flip by doing a slew
                            LogSessionEntry("ESSENTIAL", "Doing meridian flip...", "", "RunHADS", "SEQUENCE")
                            returnvalue = ClosedLoopSlew(RATopocentric, DECTopocentric, TopocentricRAString, TopocentricDECString, Format(pVSLTarget.HADSRA2000HH) + "h " + Format(pVSLTarget.HADSRA2000MM) + "m " + Format(pVSLTarget.HADSRA2000SS) + "s ", Format(pVSLTarget.HADSDEC2000DG) + "° " + Format(pVSLTarget.HADSDEC2000MM) + "' " + Format(pVSLTarget.HADSDEC2000SS) + """ ", pVSLTarget.HADSName, pVSLTarget.HADSFilter, False, TopocentricRAStringTSX, TopocentricDECStringTSX)

                            ' if run is to abort: exit
                            If returnvalue <> "OK" And returnvalue <> "SLEW_ABORTED" Then
                                pIsSequenceRunning = False
                                RunHADS = returnvalue
                                LogSessionEntry("ESSENTIAL", "Meridian flip failed!", "", "RunHADS", "SEQUENCE")

                                'mark target as unusable
                                returnvalue = DatabaseMarkErrorHADS("Meridian flip failed.")
                                If returnvalue <> "OK" Then
                                    pIsSequenceRunning = False
                                    RunHADS = returnvalue
                                    Exit Function
                                Else
                                    'target is marked unusable, exiting
                                    pIsSequenceRunning = False
                                    RunHADS = "NOK"
                                    Exit Function
                                End If
                            End If
                        End If
                    End If

                    '-----------------------------------------------------------------------
                    'refocus following meridian flip
                    'refocus if temperature changed too much
                    'refocus if number of exposure reached
                    '-----------------------------------------------------------------------
                    If (InitialSideOfMeridian <> Convert.ToInt32(pStructMount.SideOfPier) And My.Settings.sCCDFocusMeridianFlip = True) Or
                        Math.Abs(pCurrentFocusserTemperature - pInitialFocusTemperature) >= My.Settings.sCCDFocusEveryXDegrees Or
                          i > My.Settings.sCCDFocusEveryXImages + InitialTargetNumber Then
                        If InitialSideOfMeridian <> Convert.ToInt32(pStructMount.SideOfPier) And My.Settings.sCCDFocusMeridianFlip = True Then
                            LogSessionEntry("ESSENTIAL", "Focussing following pier flip...", "", "RunHADS", "SEQUENCE")
                        ElseIf Math.Abs(pCurrentFocusserTemperature - pInitialFocusTemperature) >= My.Settings.sCCDFocusEveryXDegrees Then
                            LogSessionEntry("ESSENTIAL", "Focussing due to temperature change: " + Format(pInitialFocusTemperature, "##0.00") + "° -> " + Format(pCurrentFocusserTemperature, "##0.00") + "°C", "", "RunHADS", "SEQUENCE")
                        ElseIf i > My.Settings.sCCDFocusEveryXImages + InitialTargetNumber Then
                            LogSessionEntry("ESSENTIAL", "Focussing following " + Format(My.Settings.sCCDFocusEveryXImages) + " exposures...", "", "RunDeepsky", "SEQUENCE")
                        Else
                            LogSessionEntry("ESSENTIAL", "Focussing for unknown reason...", "", "RunHADS", "SEQUENCE")
                        End If

                        InitialSideOfMeridian = Convert.ToInt32(pStructMount.SideOfPier)
                        InitialTargetNumber = i

                        FrmMain.LblLastFocusDateTime.Text = "Last focus: " + Format(DateTime.UtcNow)
                        FrmMain.LblLastFocusTemperature.Text = "Last temperature:" + Format(pCurrentFocusserTemperature, "0.00") + "°"

                        'focus goto focusstar or object and focus
                        If My.Settings.sSimulatorMode = True Then
                            LogSessionEntry("ESSENTIAL", "Now focussing in debug mode... focussed.", "", "RunHADS", "SEQUENCE")
                            pInitialFocusTemperature = pCurrentFocusserTemperature
                        Else
                            ' if run is to abort: exit
                            If pAbort = True Then
                                pIsSequenceRunning = False
                                RunHADS = "NOK"
                                Exit Function
                            End If

                            returnvalue = FocusHADS(My.Settings.sCCDFocusClosedLoopSlew)
                            If returnvalue <> "OK" And returnvalue <> "FOCUS_ABORTED" Then
                                pIsSequenceRunning = False
                                RunHADS = returnvalue

                                'mark target as unusable
                                returnvalue = DatabaseMarkErrorHADS("Refocus failed.")
                                If returnvalue <> "OK" Then
                                    pIsSequenceRunning = False
                                    RunHADS = returnvalue
                                    Exit Function
                                Else
                                    'target is marked unusable, exiting
                                    pIsSequenceRunning = False
                                    RunHADS = "NOK"
                                    Exit Function
                                End If
                            End If
                        End If

                        'when refocussed do a closed loop slew to target
                        If pMountConnected = True Then
                            ' if run is to abort: exit
                            If pAbort = True Then
                                pIsSequenceRunning = False
                                RunHADS = "NOK"
                                Exit Function
                            End If

                            returnvalue = ClosedLoopSlew(RATopocentric, DECTopocentric, TopocentricRAString, TopocentricDECString, Format(pVSLTarget.HADSRA2000HH) + "h " + Format(pVSLTarget.HADSRA2000MM) + "m " + Format(pVSLTarget.HADSRA2000SS) + "s ", Format(pVSLTarget.HADSDEC2000DG) + "° " + Format(pVSLTarget.HADSDEC2000MM) + "' " + Format(pVSLTarget.HADSDEC2000SS) + """ ", pVSLTarget.HADSName, pVSLTarget.HADSFilter, False, TopocentricRAStringTSX, TopocentricDECStringTSX)

                            If returnvalue <> "OK" And returnvalue <> "SLEW_ABORTED" Then
                                pIsSequenceRunning = False
                                RunHADS = returnvalue
                                LogSessionEntry("ESSENTIAL", "Closed loop slew following refocus failed!", "", "RunHADS", "SEQUENCE")

                                'mark target as unusable
                                returnvalue = DatabaseMarkErrorHADS("Closed loop slew following refocus failed.")
                                If returnvalue <> "OK" Then
                                    pIsSequenceRunning = False
                                    RunHADS = returnvalue
                                    Exit Function
                                Else
                                    'target is marked unusable, exiting
                                    pIsSequenceRunning = False
                                    RunHADS = "NOK"
                                    Exit Function
                                End If
                            End If
                        End If
                    End If
                End While

                LogSessionEntry("ESSENTIAL", "TESTING JUST BEFORE PROBABLE ERROR", "", "RunHADS", "SEQUENCE")
                'set the last observed date
                returnvalue = DatabaseSetLastObservedHADS(pVSLTarget.ID)
                If returnvalue <> "OK" Then
                    pIsSequenceRunning = False
                    RunHADS = returnvalue
                    Exit Function
                End If

                LogSessionEntry("ESSENTIAL", pVSLTarget.HADSName + "too low... Selecting new target.", "", "RunHADS", "SEQUENCE")
            End If

            pIsSequenceRunning = False
            'reset the counter, so a check is performed if a new target is available
            pVSLTarget.ID = 0

        Catch ex As Exception
            RunHADS = "RunHADS: " + ex.Message
            pIsSequenceRunning = False
            LogSessionEntry("ERROR", "RunHADS: " + ex.Message, "", "RunHADS", "PROGRAM")
        End Try
    End Function

    Public Function RunFlats() As String
        '-------------------------------------------------------------------------------------------
        ' AUTOFLATS
        '-------------------------------------------------------------------------------------------
        ' Start equipment, open roof
        ' Slew to flat position: 
        ' Disable tracking: smart error handling should take this into account
        ' Start with exposures when Sun <x degrees
        ' Exposure length should be between min - max treshold: favor longer exposures as the spectrum will be more in line with night exposures
        ' Calculate average of every frame, when frame between min - max treshold: save and store
        ' Keep exposing until max exposure reached and average is too dark
        Dim returnvalue As String

        RunFlats = "OK"
        Try
            'run automated flats
            'slew to flat position
            returnvalue = MountSlewToAltAz(My.Settings.sAutoFlatAlt, My.Settings.sAutoFlatAz)
            If returnvalue <> "OK" Then
                LogSessionEntry("ERROR", "Autoflat slew failed!", "", "RunFlats(", "SEQUENCE")
                RunFlats = "Autoflat slew failed."
                Exit Function
            End If
            'make sure tracking is off

            returnvalue = MountDisableTracking()
            If returnvalue <> "OK" Then
                LogSessionEntry("ERROR", "Disable tracking failed!", "", "RunFlats(", "SEQUENCE")
                RunFlats = "Autoflat slew failed."
                Exit Function
            End If

            pTheSkyXImageAverageValue = 0
            Dim CurrentExposure As Double
            CurrentExposure = My.Settings.sAutoFlatMinExp
            'start with making exposures until max exposure reached + min ADU reached

            Do While CurrentExposure <= My.Settings.sAutoFlatMaxExp
                'retrieve filternumber
                Dim FilterNumber, Binning As Integer
                If My.Settings.sAutoFlatFilter = My.Settings.sCCDFilter1 Then
                    FilterNumber = 0
                ElseIf My.Settings.sAutoFlatFilter = My.Settings.sCCDFilter2 Then
                    FilterNumber = 1
                ElseIf My.Settings.sAutoFlatFilter = My.Settings.sCCDFilter3 Then
                    FilterNumber = 2
                ElseIf My.Settings.sAutoFlatFilter = My.Settings.sCCDFilter4 Then
                    FilterNumber = 3
                ElseIf My.Settings.sAutoFlatFilter = My.Settings.sCCDFilter5 Then
                    FilterNumber = 4
                End If

                'convert binning
                If My.Settings.sAutoFlatBin = "1x1" Then
                    Binning = 1
                ElseIf My.Settings.sAutoFlatBin = "2x2" Then
                    Binning = 2
                ElseIf My.Settings.sAutoFlatBin = "3x3" Then
                    Binning = 3
                End If

                returnvalue = TakeFlatTheSkyX(CurrentExposure, Binning, "Flat", FilterNumber, My.Settings.sAutoFlatFilter)
                If returnvalue <> "OK" And returnvalue <> "IMAGE_ABORTED" Then
                    pIsSequenceRunning = False
                    RunFlats = returnvalue
                    LogSessionEntry("ESSENTIAL", "Image failed!", "", "RunFlats", "SEQUENCE")
                End If

                'calculate the next exposure
                Dim DesiredADU As Double
                Dim NewExposure As Double

                DesiredADU = (My.Settings.sAutoFlatMinADU + My.Settings.sAutoFlatMaxADU) / 2
                NewExposure = DesiredADU / (pTheSkyXImageAverageValue / CurrentExposure)
                CurrentExposure = NewExposure
                LogSessionEntry("BRIEF", "New exposure calculated at: " + Format(CurrentExposure, "#0.00") + " s...", "", "RunFlats", "SEQUENCE")

                ' if run is to abort: exit
                If pAbort = True Then
                    RunFlats = "FLATS_ABORTED"
                    Exit Function
                End If

                If CurrentExposure > My.Settings.sAutoFlatMaxExp Then
                    LogSessionEntry("BRIEF", "Not enough light to continue flats...", "", "RunFlats", "SEQUENCE")
                End If
            Loop

            'first set status to waiting, will initiate multiple parks otherwise
            pRunStatus = "WAITING"
            LogSessionEntry("DEBUG", "Set pRunStatus to WAITING", "", "RunFlats", "SEQUENCE")

            'park the mount and continue waiting
            returnvalue = MountPark()
            If returnvalue <> "OK" Then
                pIsSequenceRunning = False
                RunFlats = returnvalue
                LogSessionEntry("ESSENTIAL", "Park mount failed!", "", "RunFlats", "SEQUENCE")
            End If

        Catch ex As Exception
            RunFlats = "RunFlats: " + ex.Message
            LogSessionEntry("ERROR", "RunFlats: " + ex.Message, "", "RunFlats", "PROGRAM")
        End Try


    End Function


    Public Function RunSmartErrorCheck() As String
        ' smart error handling
        '----------------------
        ' only runs when a run is running
        '   -ALARM: Weather is safe, sequence running but the roof is not open!
        '   -ALARM: Weather is safe, sequence running but the cover is not open!
        '   -ALARM: Weather is safe, sequence running but the mount is not tracking nor at park position!
        '   -ALARM: Weather is safe, sequence running, mount unparked but the roof is not open!
        '   -ALARM: Weather is safe, sequence hangs and is not waiting!
        '   -ALARM: Weather is UNSAFE, delay is passed; mount is not yet parked!
        '   -ALARM: Weather is UNSAFE, delay is passed; roof is not yet closed!
        Dim returnvalue As String

        RunSmartErrorCheck = "OK"
        Try
            '--------------------------------------------------------
            ' SAFE SITUATIONS : no further action required
            '--------------------------------------------------------

            LogSessionEntry("DEBUG", "  Run Smart Error Check...", "", "RunSmartErrorCheck", "PROGRAM")

            'only when the run is running smart checks are needed
            'when a SmartError is active, it should repaet
            'IF weather = safe
            'And (moon = safe And program = deepsky 
            '   Or program <> deepsky)
            If (pStartRun = True Or pSmartError = True) Then
                If pWeatherStatus = "SAFE" And
                    (My.Settings.sObservationProgram = "DEEPSKY" And (pStructEventTimes.MoonSafetyStatus = "SAFE" Or My.Settings.sMoonIgnore = True) Or
                    My.Settings.sObservationProgram <> "DEEPSKY") Then
                    '---------------------------------------------------------
                    'SAFE
                    '---------------------------------------------------------
                    'ALARM: Weather is safe, sequence running but the cover is not open!
                    'ALARM: Weather is safe, sequence running but the roof is not open!
                    'ALARM: Weather is safe, sequence running but the mount is not tracking nor at park position!
                    'ALARM: Weather is safe, sequence is hanging, no new log entries!


                    If pStructEventTimes.SunAlt <= My.Settings.sSunStartRun And pCoverStatus <> 3 And My.Settings.sCoverMethod <> "NONE" And pEquipmentStatus = "ON" Then
                        'ALARM: Weather is safe, sequence running but the cover is not open!
                        'set the timestamp

                        If pSmartTimeStamp = CDate("01/01/0001") Then
                            pSmartTimeStamp = DateTime.UtcNow()
                        End If

                        If DateTime.UtcNow() - pSmartTimeStamp > TimeSpan.FromSeconds(My.Settings.sSMARTCycle) Then
                            LogSessionEntry("ERROR", "SMART ERROR: Weather is safe, run active but the cover is not open!", "", "RunSmartErrorCheck", "SMART")
                            pSmartTimeStamp = DateTime.UtcNow()
                            pStartRun = False 'do not restart run
                            pSmartError = True
                            pIsActionRunning = False
                            pRunStatus = "ABORTING"
                            returnvalue = PauseRun("SMART ERROR: Weather is safe, run active but the cover is not open: PAUSING EQUIPMENT...", "",
                                                    "SMART ERROR: Weather is safe, run active but the cover is not open: PauseEquipment: ", "",
                                                    "SMART ERROR: equipment paused.", "", "PAUSING", "WAITING")
                            If returnvalue <> "OK" Then
                                RunSmartErrorCheck = returnvalue
                                Exit Function
                            End If
                        End If

                    ElseIf pStructEventTimes.SunAlt <= My.Settings.sSunStartRun And pRoofShutterStatus <> "OPEN" And My.Settings.sRoofDevice <> "NONE" And pEquipmentStatus = "ON" Then
                        'ALARM: Weather is safe, sequence running but the roof is not open!
                        'set the timestamp

                        If pSmartTimeStamp = CDate("01/01/0001") Then
                            pSmartTimeStamp = DateTime.UtcNow()
                        End If

                        If DateTime.UtcNow() - pSmartTimeStamp > TimeSpan.FromSeconds(My.Settings.sSMARTCycle) Then
                            LogSessionEntry("ERROR", "SMART ERROR: Weather is safe, run active but the roof is not open!", "", "RunSmartErrorCheck", "SMART")
                            pSmartTimeStamp = DateTime.UtcNow()
                            pStartRun = False 'do not restart run
                            pSmartError = True
                            pIsActionRunning = False
                            pRunStatus = "ABORTING"
                            returnvalue = PauseRun("SMART ERROR: Weather is safe, run active but the roof is not open: PAUSING EQUIPMENT...", "",
                                                    "SMART ERROR: Weather is safe, run active but the roof is not open: PauseEquipment: ", "",
                                                    "SMART ERROR: equipment paused.", "", "PAUSING", "WAITING")
                            If returnvalue <> "OK" Then
                                RunSmartErrorCheck = returnvalue
                                Exit Function
                            End If
                        End If

                    ElseIf pStructEventTimes.SunAlt <= My.Settings.sSunStartRun And Not (pStructMount.Tracking = True Or pStructMount.AtPark = True) And pEquipmentStatus = "ON" Then
                        'ALARM: Weather is safe, sequence running but the mount is not tracking nor at park position!
                        'set the timestamp

                        If pSmartTimeStamp = CDate("01/01/0001") Then
                            pSmartTimeStamp = DateTime.UtcNow()
                        End If

                        If DateTime.UtcNow() - pSmartTimeStamp > TimeSpan.FromSeconds(My.Settings.sSMARTCycle) Then
                            LogSessionEntry("ERROR", "SMART ERROR: Weather is safe, run active but the mount is not tracking nor at park position!", "", "RunSmartErrorCheck", "SMART")
                            pSmartTimeStamp = DateTime.UtcNow()
                            pStartRun = False 'do not restart run
                            pSmartError = True
                            pIsActionRunning = False
                            pRunStatus = "ABORTING"
                            returnvalue = PauseRun("SMART ERROR: Weather is safe, run active but the mount is not tracking nor at park position: PAUSING EQUIPMENT...", "",
                                                   "SMART ERROR: Weather is safe, run active but the mount is not tracking nor at park position: PauseEquipment: ", "",
                                                   "SMART ERROR: equipment paused.", "", "PAUSING", "WAITING")
                            If returnvalue <> "OK" Then
                                RunSmartErrorCheck = returnvalue
                                Exit Function
                            End If
                        End If
                    ElseIf DateTime.UtcNow() - pOldProgramTimeSMART > TimeSpan.FromSeconds(My.Settings.sSMARTTimeout) And
                            ((pStructEventTimes.SunAlt <= My.Settings.sSunStartRun - 1 And pStructEventTimes.SunSettingRising = "SETTING") Or 'wait 1 degree so new messages can be written to log
                            (pStructEventTimes.SunAlt < My.Settings.sSunStopRun + 1 And pStructEventTimes.SunSettingRising = "RISING")) Then
                        'ALARM: Weather is safe, sequence is hanging, program is no longer responding!
                        LogSessionEntry("ERROR", "SMART ERROR: Weather is safe, program is hanging!", "", "RunSmartErrorCheck", "SMART")
                        pSmartTimeStamp = DateTime.UtcNow()
                        pStartRun = False 'do not restart run
                        pSmartError = True
                        pIsActionRunning = False
                        pRunStatus = "ABORTING"
                        returnvalue = PauseRun("SMART ERROR: Weather is safe, program is hanging: PAUSING EQUIPMENT...", "",
                                               "SMART ERROR: Weather is safe, program is hanging: PauseEquipment: ", "",
                                               "SMART ERROR: equipment paused.", "", "PAUSING", "WAITING")
                        If returnvalue <> "OK" Then
                            RunSmartErrorCheck = returnvalue
                            Exit Function
                        End If
                    ElseIf DateTime.UtcNow() - pOldSequenceTimeSMART > TimeSpan.FromSeconds(My.Settings.sSMARTTimeout) And (pRunStatus <> "DAY" And pRunStatus <> "WAITING" And pRunStatus <> "UNSAFE") Then
                        'ALARM: Weather is safe, sequence is hanging, no new log entries!
                        LogSessionEntry("ERROR", "SMART ERROR: Weather is safe, run is hanging!", "", "RunSmartErrorCheck", "SMART")
                        pSmartTimeStamp = DateTime.UtcNow()
                        pStartRun = False 'do not restart run
                        pSmartError = True
                        pIsActionRunning = False
                        pRunStatus = "ABORTING"
                        returnvalue = PauseRun("SMART ERROR: Weather is safe, run is hanging: PAUSING EQUIPMENT...", "",
                                               "SMART ERROR: Weather is safe, run is hanging: PauseEquipment: ", "",
                                               "SMART ERROR: equipment paused.", "", "PAUSING", "WAITING")
                        If returnvalue <> "OK" Then
                            RunSmartErrorCheck = returnvalue
                            Exit Function
                        End If
                    End If
                Else
                    'ALARM: Weather is safe, sequence is hanging: no new messages are logged!
                    pSmartTimeStamp = CDate("01/01/0001")
                End If


            ElseIf pWeatherStatus = "UNSAFE" Then
                '---------------------------------------------------------
                'UNSAFE
                '---------------------------------------------------------
                '   -ALARM: Weather is UNSAFE, delay is passed; mount is not yet parked!
                '   -ALARM: Weather is UNSAFE, delay is passed; roof is not yet closed!

                If pRoofShutterStatus <> "CLOSED" And My.Settings.sRoofDevice <> "NONE" Then
                    'ALARM: Weather is UNSAFE, delay is passed; roof is not yet closed!

                    'set the timestamp
                    If pSmartTimeStamp = CDate("01/01/0001") Then
                        pSmartTimeStamp = DateTime.UtcNow()
                    End If

                    If DateTime.UtcNow() - pSmartTimeStamp > TimeSpan.FromSeconds(My.Settings.sSMARTCycle) Then
                        LogSessionEntry("ERROR", "SMART ERROR: Weather is UNSAFE, delay is passed; roof is not yet closed!", "", "RunSmartErrorCheck", "SMART")
                        pSmartTimeStamp = DateTime.UtcNow()
                        pStartRun = False 'do not restart run
                        pSmartError = True
                        pIsActionRunning = False
                        pRunStatus = "ABORTING"
                        returnvalue = PauseRun("SMART ERROR: Weather is UNSAFE, delay is passed; roof is not yet closed: PAUSING EQUIPMENT...", "",
                                                       "SMART ERROR: Weather is UNSAFE, delay is passed; roof is not yet closed: PauseEquipment: ", "",
                                                       "SMART ERROR: equipment paused.", "", "PAUSING", "WAITING")
                        If returnvalue <> "OK" Then
                            RunSmartErrorCheck = returnvalue
                            Exit Function
                        End If
                    End If
                ElseIf pMountConnected = True And pStructMount.AtPark <> True Then
                    'ALARM: Weather is UNSAFE, delay is passed; mount is not yet parked, but only during night time.

                    'set the timestamp
                    If pSmartTimeStamp = CDate("01/01/0001") Then
                        pSmartTimeStamp = DateTime.UtcNow()
                    End If

                    If DateTime.UtcNow() - pSmartTimeStamp > TimeSpan.FromSeconds(My.Settings.sSMARTCycle) Then
                        LogSessionEntry("ERROR", "SMART ERROR: Weather is UNSAFE, delay is passed; mount is not yet parked!", "", "RunSmartErrorCheck", "SMART")
                        pSmartTimeStamp = DateTime.UtcNow()
                        pStartRun = False 'do not restart run
                        pSmartError = True
                        pIsActionRunning = False
                        pRunStatus = "ABORTING"
                        returnvalue = PauseRun("SMART ERROR: Weather is UNSAFE, delay is passed; mount is not yet parked: PAUSING EQUIPMENT...", "",
                                               "SMART ERROR: Weather is UNSAFE, delay is passed; mount is not yet parked: PauseEquipment: ", "",
                                               "SMART ERROR: equipment paused.", "", "PAUSING", "WAITING")
                        If returnvalue <> "OK" Then
                            RunSmartErrorCheck = returnvalue
                            Exit Function
                        End If
                    End If
                Else
                    pSmartTimeStamp = CDate("01/01/0001")
                End If
            End If

        Catch ex As Exception
            RunSmartErrorCheck = "RunSmartErrorCheck: " + ex.Message
            AbortRun()
            LogSessionEntry("ERROR", "RunSmartErrorCheck: " + ex.Message, "", "RunSmartErrorCheck", "PROGRAM")
        End Try
    End Function

    Private Sub AbortRun()
        pAbort = True
        pRunStatus = "ABORTING"
        pStartRun = False 'do not restart run       
        LogSessionEntry("BRIEF", "Aborting run!", "", "AbortRun", "PROGRAM")
    End Sub

End Module


