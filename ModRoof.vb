Imports System.Math
Imports System.Threading
Imports ASCOM
Imports Newtonsoft.Json.Serialization

Module ModRoof
    Private pRoof As ASCOM.DriverAccess.Dome

    Public pRoofShutterStatus As String
    Public pRoofConnected As Boolean

    Public Function RoofConnect() As String
        Dim returnvalue As String
        Dim startExecution As Date
        Dim executionTime As TimeSpan

        RoofConnect = "OK"
        Try
            FrmMain.Cursor = Cursors.WaitCursor
            startExecution = DateTime.UtcNow()

            LogSessionEntry("BRIEF", "Connecting to the roof...", "", "RoofConnect", "ROOF")
            If My.Settings.sSimulatorMode = True Then
                pRoof = New ASCOM.DriverAccess.Dome("DomeSim.Dome") With {
                 .Connected = True
                      }
            Else

                pRoof = New ASCOM.DriverAccess.Dome(My.Settings.sRoofDevice) With {
            .Connected = True
                 }
            End If
            LogSessionEntry("FULL", "Roof connected.", "", "RoofConnect", "ROOF")
            pRoofConnected = True

            'check roof, fill the fields on the main form
            returnvalue = CheckRoof()
            If returnvalue <> "OK" Then
                FrmMain.Cursor = Cursors.Default
                RoofConnect = returnvalue
                Exit Function
            End If

            FrmMain.Cursor = Cursors.Default
            executionTime = DateTime.UtcNow() - startExecution
            LogSessionEntry("DEBUG", "  RoofConnect: " + executionTime.ToString, "", "RoofConnect", "ROOF")

        Catch ex As Exception
            FrmMain.Cursor = Cursors.Default
            RoofConnect = "RoofConnect: " + ex.Message
            LogSessionEntry("ERROR", "RoofConnect: " + ex.Message, "", "RoofConnect", "ROOF")
        End Try
    End Function


    Public Function RoofDisconnect() As String
        Dim returnvalue As String
        Dim startExecution As Date
        Dim executionTime As TimeSpan

        RoofDisconnect = "OK"
        Try
            FrmMain.Cursor = Cursors.WaitCursor
            startExecution = DateTime.UtcNow()
            LogSessionEntry("BRIEF", "Disconnecting the roof...", "", "RoofDisconnect", "ROOF")

            If My.Settings.sRoofDevice <> "NONE" Then
                pRoof.Connected = False
                pRoof = Nothing
            End If
            pRoofConnected = False
            LogSessionEntry("BRIEF", "Roof disconnected.", "", "RoofDisconnect", "ROOF")

            'check roof, fill the fields on the main form
            returnvalue = CheckRoof()
            If returnvalue <> "OK" Then
                FrmMain.Cursor = Cursors.Default
                RoofDisconnect = returnvalue
                Exit Function
            End If

            FrmMain.Cursor = Cursors.Default
            executionTime = DateTime.UtcNow() - startExecution
            LogSessionEntry("DEBUG", "  RoofDisconnect: " + executionTime.ToString, "", "RoofDisconnect", "ROOF")

        Catch ex As Exception
            RoofDisconnect = "RoofDisconnect: " + ex.Message
            LogSessionEntry("ERROR", "RoofDisconnect: " + ex.Message, "", "RoofDisconnect", "ROOF")
        End Try
    End Function

    Public Function RoofOpenRoof() As String
        Dim returnvalue As String
        Dim startExecution As Date
        Dim executionTime As TimeSpan

        RoofOpenRoof = "OK"
        Try
            FrmMain.Cursor = Cursors.WaitCursor
            startExecution = DateTime.UtcNow()

            'delay for 1 sec to make sure AAG actually opened the switch
            Thread.Sleep(1000)

            If My.Settings.sRoofDevice <> "NONE" Then
                If pWeatherStatus <> "SAFE" And My.Settings.sDisableSafetyCheck = False Then
                    'not safe do nothing
                    LogSessionEntry("BRIEF", "Not safe, roof cannot open!", "", "RoofOpenRoof", "ROOF")
                Else
                    'first park the mount
                    If pMountConnected = True Then
                        returnvalue = MountPark()
                        If returnvalue <> "OK" Then
                            pEquipmentStatus = "ERROR"
                            RoofOpenRoof = "RoofOpenRoof: " + returnvalue '"ERROR PARKING MOUNT"
                        End If
                    Else
                        LogSessionEntry("BRIEF", "Mount not connected.", "", "PauseEquipment", "PROGRAM")
                    End If


                    LogSessionEntry("BRIEF", "Opening roof...", "", "RoofOpenRoof", "ROOF")
                    pRoof.OpenShutter()

                    Dim RoofStartOpening As Date
                    Dim RoofDiff As TimeSpan
                    RoofStartOpening = DateTime.UtcNow()

                    While pRoof.ShutterStatus <> 0 'OPEN
                        RoofDiff = DateTime.UtcNow.Subtract(RoofStartOpening)
                        If RoofDiff.TotalSeconds > My.Settings.sRoofOpenTimeout Then
                            'roof did not open in time: error !
                            LogSessionEntry("ERROR", "Roof does not open!", "", "RoofOpenRoof", "ROOF")
                            FrmMain.Cursor = Cursors.Default
                            RoofOpenRoof = "RoofOpenRoof: TIMEOUT"
                            Exit Function
                        Else
                            'wait some longer
                            'Thread.Sleep(500)
                            Application.DoEvents()
                            LogSessionEntry("BRIEF", "Opening roof...", "", "RoofOpenRoof", "ROOF")
                        End If
                    End While

                    'add extra time in debug mode
                    If My.Settings.sSimulatorMode = True Then
                        Thread.Sleep(5000)
                    End If

                    LogSessionEntry("BRIEF", "Roof opened.", "", "RoofOpenRoof", "ROOF")
                End If
            End If

            'check roof, fill the fields on the main form
            returnvalue = CheckRoof()
            If returnvalue <> "OK" Then
                FrmMain.Cursor = Cursors.Default
                RoofOpenRoof = returnvalue
                Exit Function
            End If

            FrmMain.Cursor = Cursors.Default
            executionTime = DateTime.UtcNow() - startExecution
            LogSessionEntry("DEBUG", "  RoofOpenRoof: " + executionTime.ToString, "", "RoofOpenRoof", "ROOF")

        Catch ex As Exception
            FrmMain.Cursor = Cursors.Default
            RoofOpenRoof = "RoofOpenRoof: " + ex.Message
            LogSessionEntry("ERROR", "RoofOpenRoof: " + ex.Message, "", "RoofOpenRoof", "ROOF")
        End Try
    End Function

    Public Function RoofCloseRoof() As String
        Dim returnvalue As String
        Dim RoofStartClosing As Date
        Dim RoofDiff As TimeSpan
        Dim startExecution As Date
        Dim executionTime As TimeSpan

        RoofCloseRoof = "OK"
        Try
            FrmMain.Cursor = Cursors.WaitCursor
            startExecution = DateTime.UtcNow()
            RoofStartClosing = DateTime.UtcNow()

            If My.Settings.sRoofDevice <> "NONE" Then
                LogSessionEntry("BRIEF", "Closing roof...", "", "RoofCloseRoof", "ROOF")
                pRoof.CloseShutter()
                While pRoof.ShutterStatus <> 1 'CLOSED
                    RoofDiff = DateTime.UtcNow.Subtract(RoofStartClosing)

                    If RoofDiff.TotalSeconds > My.Settings.sRoofOpenTimeout Then
                        'roof did not open in time: error !
                        LogSessionEntry("ERROR", "Roof does not close!", "", "RoofCloseRoof", "ROOF")
                        FrmMain.Cursor = Cursors.Default
                        RoofCloseRoof = "RoofCloseRoof: TIMEOUT"
                        Exit Function
                    Else
                        'wait some longer
                        'Thread.Sleep(500)
                        Application.DoEvents()
                        LogSessionEntry("FULL", "Closing roof...", "", "RoofCloseRoof", "ROOF")
                    End If
                End While

                LogSessionEntry("BRIEF", "Roof closed.", "", "RoofCloseRoof", "ROOF")
            End If


            'check roof, fill the fields on the main form
            returnvalue = CheckRoof()
            If returnvalue <> "OK" Then
                FrmMain.Cursor = Cursors.Default
                RoofCloseRoof = returnvalue
                Exit Function
            End If

            FrmMain.Cursor = Cursors.Default
            executionTime = DateTime.UtcNow() - startExecution
            LogSessionEntry("DEBUG", "  RoofCloseRoof: " + executionTime.ToString, "", "RoofCloseRoof", "ROOF")

        Catch ex As Exception
            FrmMain.Cursor = Cursors.Default
            RoofCloseRoof = "RoofCloseRoof: " + ex.Message
            LogSessionEntry("ERROR", "RoofCloseRoof: " + ex.Message, "", "RoofCloseRoof", "ROOF")
        End Try
    End Function

    Public Function RoofShutterStatus() As String
        Dim ShutterStatus As Integer
        Dim ShutterResult As String
        Dim startExecution As Date
        Dim executionTime As TimeSpan

        RoofShutterStatus = "OK"
        Try
            startExecution = DateTime.UtcNow()

            ShutterResult = ""
            ShutterStatus = pRoof.ShutterStatus
            LogSessionEntry("DEBUG", "  pRoof.ShutterStatus: " + Format(pRoof.ShutterStatus), "", "RoofDisconnect", "ROOF")

            If ShutterStatus = 0 Then
                ShutterResult = "OPEN"
            ElseIf ShutterStatus = 1 Then
                ShutterResult = "CLOSED"
            ElseIf ShutterStatus = 2 Then
                ShutterResult = "OPENING"
            ElseIf ShutterStatus = 3 Then
                ShutterResult = "CLOSING"
            ElseIf ShutterStatus = 4 Then
                ShutterResult = "ERROR"
            End If

            pRoofShutterStatus = ShutterResult

            executionTime = DateTime.UtcNow() - startExecution
            LogSessionEntry("DEBUG", "  RoofShutterStatus: " + executionTime.ToString, "", "RoofShutterStatus", "ROOF")

        Catch ex As Exception
            RoofShutterStatus = "RoofShutterStatus: " + ex.Message
            pRoofShutterStatus = ""
            LogSessionEntry("ERROR", "RoofShutterStatus: " + ex.Message, "", "RoofShutterStatus", "ROOF")
        End Try
    End Function


    Public Function RoofOpenShutterPct(vPct As Double) As String
        Dim returnvalue As String
        Dim startExecution As Date
        Dim executionTime As TimeSpan

        RoofOpenShutterPct = "OK"
        Try
            FrmMain.Cursor = Cursors.WaitCursor
            startExecution = DateTime.UtcNow()

            LogSessionEntry("BRIEF", "Opening roof " + Format(vPct) + "%...", "", "RoofOpenShutterPct", "ROOF")
            pRoof.SlewToAltitude(vPct)
            While Math.Round(pRoof.Altitude, 0) <> vPct
                ' Thread.Sleep(500)
                Application.DoEvents()
                LogSessionEntry("FULL", Format(vPct) + "% opening roof...", "", "RoofOpenShutterPct", "ROOF")
            End While
            LogSessionEntry("FULL", "Roof opened " + Format(vPct) + "%!", "", "RoofOpenShutterPct", "ROOF")


            'check roof, fill the fields on the main form
            returnvalue = CheckRoof()
            If returnvalue <> "OK" Then
                RoofOpenShutterPct = returnvalue
                FrmMain.Cursor = Cursors.Default
                Exit Function
            End If

            FrmMain.Cursor = Cursors.Default
            executionTime = DateTime.UtcNow() - startExecution
            LogSessionEntry("DEBUG", "  RoofOpenShutterPct: " + executionTime.ToString, "", "RoofOpenShutterPct", "ROOF")

        Catch ex As Exception
            FrmMain.Cursor = Cursors.Default
            RoofOpenShutterPct = "RoofOpenShutterPct: " + ex.Message
            LogSessionEntry("ERROR", "RoofOpenShutterPct: " + Format(vPct) + ex.Message, "", "RoofOpenShutterPct", "ROOF")
        End Try
    End Function

    Public Function CheckRoof() As String
        Dim returnvalue As String
        Dim startExecution As Date
        Dim executionTime As TimeSpan

        CheckRoof = "OK"
        Try
            startExecution = DateTime.UtcNow()
            If My.Settings.sRoofDevice = "NONE" Then
                FrmMain.LblRoof.BackColor = Color.Transparent
                FrmMain.LblRoof.Text = "NO ROOF"
            Else
                returnvalue = RoofShutterStatus()
                If returnvalue <> "OK" Then
                    CheckRoof = returnvalue
                    Exit Function
                End If

                If pRoofShutterStatus = "CLOSED" Then
                    FrmMain.LblRoof.BackColor = Color.Red
                    FrmMain.LblRoof.Text = "RF CLOSED"
                ElseIf pRoofShutterStatus = "OPENING" Then
                    FrmMain.LblRoof.BackColor = Color.Orange
                    FrmMain.LblRoof.Text = "RF OPENING"
                ElseIf pRoofShutterStatus = "CLOSING" Then
                    FrmMain.LblRoof.BackColor = Color.Orange
                    FrmMain.LblRoof.Text = "RF CLOSING"
                ElseIf pRoofShutterStatus = "OPEN" Then
                    FrmMain.LblRoof.BackColor = Color.Green
                    FrmMain.LblRoof.Text = "RF OPEN"
                ElseIf pRoofShutterStatus = "ERROR" Then
                    FrmMain.LblRoof.BackColor = Color.IndianRed
                    FrmMain.LblRoof.Text = "RF ERROR"
                    CheckRoof = "RF ERROR"
                    LogSessionEntry("ERROR", "CheckRoof: ROOF ERROR!", "", "CheckRoof", "ROOF")
                    Exit Function
                Else
                    FrmMain.LblRoof.BackColor = Color.Purple
                    FrmMain.LblRoof.Text = "UNKNOWN"
                    CheckRoof = "UNKNOWN"
                    LogSessionEntry("ERROR", "CheckRoof: ROOF ERROR!", "", "CheckRoof", "ROOF")
                    Exit Function
                End If
            End If

            executionTime = DateTime.UtcNow() - startExecution
            LogSessionEntry("DEBUG", "  CheckRoof: " + executionTime.ToString, "", "CheckRoof", "ROOF")
            LogSessionEntry("DEBUG", "  CheckRoof: " + pRoofShutterStatus, "", "CheckRoof", "ROOF")
        Catch ex As Exception
            CheckRoof = "CheckRoof: " + ex.Message
            LogSessionEntry("ERROR", "CheckRoof: " + ex.Message, "", "CheckRoof", "ROOF")
        End Try
    End Function
End Module
