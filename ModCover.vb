Imports System.Threading
Imports System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip
Module ModCover

    Private pCover As ASCOM.DriverAccess.CoverCalibrator
    Public pCoverStatus As Integer

    'NotPresent	0	This device does not have a cover that can be closed independently
    'Closed  1	The cover Is closed
    'Moving  2	The cover Is moving to a New position
    'Open    3	The cover Is open
    'Unknown 4	The state of the cover Is unknown
    'Error 5	The device encountered an Error When changing state 

    Public pCoverConnected As Boolean

    Public Function CoverConnect() As String
        Dim returnvalue As String
        Dim startExecution As Date
        Dim executionTime As TimeSpan

        CoverConnect = "OK"
        Try
            FrmMain.Cursor = Cursors.WaitCursor
            startExecution = DateTime.UtcNow()

            LogSessionEntry("BRIEF", "Connecting to the cover...", "", "CoverConnect", "COVER")


            If My.Settings.sSimulatorMode = True Then
                If My.Settings.sCoverMethod = "ASCOM" Then
                    pCover = New ASCOM.DriverAccess.CoverCalibrator("ASCOM.Simulator.CoverCalibrator") With {
                .Connected = True
            }
                End If

            Else
                If My.Settings.sCoverMethod = "ASCOM" Then
                    pCover = New ASCOM.DriverAccess.CoverCalibrator(My.Settings.sCoverDevice) With {
                .Connected = True
            }
                End If

            End If
            pCoverConnected = True

            'check SnapCap: fill fields on main form
            returnvalue = CheckCover()
            If returnvalue <> "OK" Then
                CoverConnect = "CheckCover: " + returnvalue
            End If

            FrmMain.Cursor = Cursors.Default
            executionTime = DateTime.UtcNow() - startExecution
            LogSessionEntry("DEBUG", "  CoverConnect: " + executionTime.ToString, "", "CoverConnect", "COVER")
            LogSessionEntry("FULL", "Cover connected.", "", "CoverConnect", "COVER")

        Catch ex As Exception
            pCoverConnected = False
            FrmMain.Cursor = Cursors.Default
            CoverConnect = "CoverConnect: " + ex.Message
            LogSessionEntry("ERROR", "CoverConnect: " + ex.Message, "", "CoverConnect", "COVER")
        End Try
    End Function

    Public Function CoverDisconnect() As String
        Dim returnvalue As String
        Dim startExecution As Date
        Dim executionTime As TimeSpan

        CoverDisconnect = "OK"
        Try
            FrmMain.Cursor = Cursors.WaitCursor
            startExecution = DateTime.UtcNow()

            LogSessionEntry("BRIEF", "Disconnecting Cover...", "", "CoverDisconnect", "COVER")

            If My.Settings.sCoverMethod = "ASCOM" Then
                pCover.Connected = False
            End If

            pCoverConnected = False

            'check Cover: fill fields on main form
            returnvalue = CheckCover()
            If returnvalue <> "OK" Then
                CoverDisconnect = "CheckCover: " + returnvalue
            End If

            FrmMain.Cursor = Cursors.Default
            LogSessionEntry("FULL", "Cover disconnected.", "", "CoverDisconnect", "COVER")
            executionTime = DateTime.UtcNow() - startExecution
            LogSessionEntry("DEBUG", "  CoverDisconnect: " + executionTime.ToString, "", "CoverDisconnect", "COVER")

        Catch ex As Exception
            pCoverConnected = False
            FrmMain.Cursor = Cursors.Default
            CoverDisconnect = "CoverDisconnect: " + ex.Message
            LogSessionEntry("ERROR", "CoverDisconnect: " + ex.Message, "", "CoverDisconnect", "COVER")
        End Try
    End Function

    Function CoverOpen() As String
        Dim returnvalue As String
        Dim startExecution As Date
        Dim executionTime As TimeSpan

        CoverOpen = "OK"
        Try
            FrmMain.Cursor = Cursors.WaitCursor
            startExecution = DateTime.UtcNow()

            LogSessionEntry("BRIEF", "Cover opening...", "", "CoverOpen", "COVER")
            'do not open cover is scope is not parked
            If pStructMount.AtPark = True Then
                If My.Settings.sCoverMethod = "ASCOM" Then
                    pCover.OpenCover()
                    While pCover.CoverState <> 3
                        'Thread.Sleep(500)
                        My.Application.DoEvents()

                        If startExecution.AddSeconds(My.Settings.sCoverTimeout) < DateTime.UtcNow() Then
                            LogSessionEntry("ERROR", "Cover open timeout!", "", "CoverOpen", "COVER")
                            returnvalue = PauseRun("ERROR: Cover open timeout: PAUSING EQUIPMENT...", "",
                                                   "ERROR: Cover open timeout: PauseEquipment: ", "",
                                                   "ERROR: equipment paused.", "", "PAUSING", "WAITING")
                            CoverOpen = "TIMEOUT"
                            Exit Function
                        End If
                    End While
                Else
                    'serial
                    returnvalue = OpenSnapCap(My.Settings.sSnapCapSerialPort)
                    If returnvalue <> "OK" Then
                        FrmMain.Cursor = Cursors.Default
                        CoverOpen = returnvalue
                        Exit Function
                    End If
                End If
                LogSessionEntry("BRIEF", "Cover open.", "", "CoverOpen", "COVER")
            Else
                LogSessionEntry("ERROR", "Cover cannot open when the mount is not parked!", "", "CoverOpen", "COVER")
                CoverOpen = "NOK"
            End If

            'check Cover: fill fields on main form
            returnvalue = CheckCover()
            If returnvalue <> "OK" Then
                CoverOpen = "CheckCover: " + returnvalue
            End If

            FrmMain.Cursor = Cursors.Default
            executionTime = DateTime.UtcNow() - startExecution
            LogSessionEntry("DEBUG", "  CoverOpen: " + executionTime.ToString, "", "CoverOpen", "COVER")
        Catch ex As Exception
            FrmMain.Cursor = Cursors.Default
            CoverOpen = "CoverOpen: " + ex.Message
            LogSessionEntry("ERROR", "CoverOpen: " + ex.Message, "", "CoverOpen", "COVER")
        End Try
    End Function

    Function CoverClose() As String
        Dim returnvalue As String
        Dim startExecution As Date
        Dim executionTime As TimeSpan

        CoverClose = "OK"
        Try
            FrmMain.Cursor = Cursors.WaitCursor
            startExecution = DateTime.UtcNow()

            LogSessionEntry("BRIEF", "Cover closing...", "", "CoverClose", "COVER")
            'do not close cover is scope is not parked
            If pCoverConnected = True Then
                If pStructMount.AtPark = True Then

                    If My.Settings.sCoverMethod = "ASCOM" Then
                        pCover.CloseCover()
                        While pCover.CoverState <> 1 And pCover.CoverState <> 4
                            'Thread.Sleep(500)
                            My.Application.DoEvents()
                            LogSessionEntry("FULL", "Cover closing...", "", "CoverClose", "COVER")

                            If startExecution.AddSeconds(My.Settings.sCoverTimeout) < DateTime.UtcNow() Then
                                LogSessionEntry("ERROR", "Cover close timeout!", "", "CoverClose", "COVER")
                                returnvalue = PauseRun("ERROR: Cover close timeout: PAUSING EQUIPMENT...", "",
                                                       "ERROR: Cover close timeout: PauseEquipment: ", "",
                                                       "ERROR: equipment paused.", "", "PAUSING", "WAITING")
                                CoverClose = "TIMEOUT"
                                Exit Function
                            End If

                        End While
                    Else
                        returnvalue = CloseSnapCap(My.Settings.sSnapCapSerialPort)
                        If returnvalue <> "OK" Then
                            FrmMain.Cursor = Cursors.Default
                            CoverClose = returnvalue
                            Exit Function
                        End If
                    End If
                    LogSessionEntry("BRIEF", "Cover closed.", "", "CoverClose", "COVER")
                Else
                    LogSessionEntry("ERROR", "Cover cannot close when mount is not parked!", "", "CoverClose", "COVER")
                    CoverClose = "NOK"
                End If
            End If


            'check Cover: fill fields on main form
            returnvalue = CheckCover()
            If returnvalue <> "OK" Then
                CoverClose = "CheckCover: " + returnvalue
            End If

            FrmMain.Cursor = Cursors.Default
            executionTime = DateTime.UtcNow() - startExecution
            LogSessionEntry("DEBUG", "  CoverClose: " + executionTime.ToString, "", "CoverClose", "COVER")

        Catch ex As Exception
            FrmMain.Cursor = Cursors.Default
            CoverClose = "CoverClose: " + ex.Message
            LogSessionEntry("ERROR", "CoverClose: " + ex.Message, "", "CoverClose", "COVER")
        End Try
    End Function


    Function CoverGetStatus(vShowMessages As Boolean) As String
        Dim returnvalue As String
        Dim startExecution As Date
        Dim executionTime As TimeSpan

        CoverGetStatus = "OK"
        Try
            startExecution = DateTime.UtcNow()

            If My.Settings.sCoverMethod = "ASCOM" Then
                pCoverStatus = pCover.CoverState
            Else
                returnvalue = GetStatusSnapCap(My.Settings.sSnapCapSerialPort, vShowMessages)
                If returnvalue <> "OK" Then
                    CoverGetStatus = returnvalue
                    Exit Function
                End If
            End If

            executionTime = DateTime.UtcNow() - startExecution
            LogSessionEntry("DEBUG", "  CoverGetStatus: " + executionTime.ToString, "", "CoverGetStatus", "COVER")
        Catch ex As Exception
            CoverGetStatus = "CoverGetStatus: " + ex.Message
            If vShowMessages = True Then
                LogSessionEntry("ERROR", "CoverGetStatus: " + ex.Message, "", "CoverGetStatus", "COVER")
            End If
        End Try
    End Function


    Public Function CheckCover() As String
        Dim returnvalue As String
        Dim startExecution As Date
        Dim executionTime As TimeSpan

        CheckCover = "OK"
        Try
            startExecution = DateTime.UtcNow()
            returnvalue = CoverGetStatus(True)
            If returnvalue <> "OK" Then
                CheckCover = returnvalue
                Exit Function
            End If

            'NotPresent	0	This device does not have a cover that can be closed independently
            'Closed  1	The cover Is closed
            'Moving  2	The cover Is moving to a New position
            'Open    3	The cover Is open
            'Unknown 4	The state of the cover Is unknown
            'Error 5	The device encountered an Error When changing state 

            If pCoverStatus = 1 Then
                FrmMain.LblCover.BackColor = ColorTranslator.FromHtml("#d63031") 'red
                FrmMain.LblCover.Text = "CV CLOSED"
            ElseIf pCoverStatus = 2 Then
                FrmMain.LblCover.BackColor = Color.Orange
                FrmMain.LblCover.Text = "CV MOVING"
            ElseIf pCoverStatus = 3 Then
                FrmMain.LblCover.BackColor = ColorTranslator.FromHtml("#4cd137") 'green
                FrmMain.LblCover.Text = "CV OPEN"
            ElseIf pCoverStatus = 4 Then
                FrmMain.LblCover.BackColor = Color.Transparent
                FrmMain.LblCover.Text = "COVER"
            Else
                FrmMain.LblCover.BackColor = Color.Transparent
                FrmMain.LblCover.Text = "COVER"
            End If
            executionTime = DateTime.UtcNow() - startExecution
            LogSessionEntry("DEBUG", "  CheckCover: " + executionTime.ToString, "", "CheckCover", "COVER")
        Catch ex As Exception
            CheckCover = "CheckCover: " + ex.Message
            LogSessionEntry("ERROR", "CheckCover: " + ex.Message, "", "CheckCover", "COVER")
        End Try
    End Function
End Module
