Imports System.IO.Ports 'To Access the SerialPort Object
Imports System.Threading

Module modGeminiSnapCapSerial
    'https://www.xanthium.in/serial-port-programming-visual-basic-dotnet-for-embedded-developers

    Public pSerialPort As SerialPort
    Public pSerialPortStatus As String 'NOTHING / OPEN / CLOSED

    Public pSnapStatus As String
    Public pSnapMotor As String
    Public pAllSerialPorts(10) As String 'contains all the serial ports found on the system

    Function GetSerialPorts() As String
        Dim Port As String
        Dim i As Integer
        Dim AvailablePorts() As String = SerialPort.GetPortNames()
        Dim startExecution As Date
        Dim executionTime As TimeSpan

        GetSerialPorts = "OK"
        Try
            startExecution = DateTime.UtcNow()
            LogSessionEntry("DEBUG", "  GetSerialPorts...", "", "GetSerialPorts", "SNAPCAP")

            i = 0

            For Each Port In AvailablePorts
                pAllSerialPorts(i) = Port
                i += 1
            Next Port

            executionTime = DateTime.UtcNow() - startExecution
            LogSessionEntry("DEBUG", "  GetSerialPorts: " + executionTime.ToString, "", "GetSerialPorts", "SNAPCAP")

        Catch ex As Exception
            GetSerialPorts = "GetSerialPorts: " + ex.ToString
            LogSessionEntry("ERROR", "GetSerialPorts: " + ex.ToString, "", "GetSerialPorts", "SNAPCAP")
        End Try
    End Function



    Function OpenSerialPort(vPort As String, vShowMessages As Boolean) As String
        Dim startExecution As Date
        Dim executionTime As TimeSpan

        OpenSerialPort = "OK"
        Try
            startExecution = DateTime.UtcNow()
            LogSessionEntry("DEBUG", "  OpenSerialPort...", "", "OpenSerialPort", "SNAPCAP")

            pSerialPort = New SerialPort With {
                .PortName = vPort,   'Assign the port name to the MyCOMPort object
                .BaudRate = 38400,     'Assign the Baudrate to the MyCOMPort object
                .Parity = Parity.None,   'Parity bits = none  
                .DataBits = 8,             'No of Data bits = 8
                .StopBits = StopBits.One  'No of Stop bits = 1
                }

            'open port
            pSerialPort.Open()
            pSerialPortStatus = "OPEN"

            executionTime = DateTime.UtcNow() - startExecution
            LogSessionEntry("DEBUG", "  OpenSerialPort: " + executionTime.ToString, "", "OpenSerialPort", "SNAPCAP")

        Catch ex As Exception
            OpenSerialPort = "OpenSerialPort: " + ex.ToString
            If vShowMessages = True Then
                LogSessionEntry("ERROR", "OpenSerialPort trying again: " + ex.ToString, "", "OpenSerialPort", "SNAPCAP")
            End If
        End Try
    End Function


    Function CloseSerialPort() As String
        Dim startExecution As Date
        Dim executionTime As TimeSpan

        CloseSerialPort = "OK"
        Try
            startExecution = DateTime.UtcNow()
            LogSessionEntry("DEBUG", "  CloseSerialPort...", "", "CloseSerialPort", "SNAPCAP")

            pSerialPort.Close()                      ' Close port
            pSerialPortStatus = "CLOSED"

            executionTime = DateTime.UtcNow() - startExecution
            LogSessionEntry("DEBUG", "  CloseSerialPort: " + executionTime.ToString, "", "CloseSerialPort", "SNAPCAP")

        Catch ex As Exception
            CloseSerialPort = "CloseSerialPort: " + ex.ToString
            LogSessionEntry("ERROR", "CloseSerialPort: " + ex.ToString, "", "CloseSerialPort", "SNAPCAP")
        End Try
    End Function

    Function WriteSerialPort(vPort As String, vCommand As String, vShowMessages As Boolean) As String
        Dim returnvalue As String
        Dim startExecution As Date
        Dim executionTime As TimeSpan

        'returnvalue of function contains actual data !
        'WriteSerialPort = ""
        Try
            startExecution = DateTime.UtcNow()
            LogSessionEntry("DEBUG", "  WriteSerialPort...", "", "WriteSerialPort", "SNAPCAP")

            ' Open the port
            If pSerialPortStatus <> "OPEN" Then
                LogSessionEntry("FULL", "  Serial port not open: opening port..." + startExecution.ToString, "", "WriteSerialPort", "SNAPCAP")
                returnvalue = OpenSerialPort(vPort, vShowMessages)
                'If returnvalue <> "OK" Then
                ' WriteSerialPort = returnvalue
                ' Exit Function
                'End If

                If returnvalue <> "OK" Then
                    LogSessionEntry("FULL", "  Serial port did not open: retrying..." + startExecution.ToString, "", "WriteSerialPort", "SNAPCAP")
                    'retry
                    Thread.Sleep(50)
                    returnvalue = OpenSerialPort(vPort, vShowMessages)
                    If returnvalue <> "OK" Then
                        WriteSerialPort = returnvalue
                        Exit Function
                    End If
                End If

            End If

            pSerialPort.Write(vbCrLf + vCommand + vbCrLf)
            LogSessionEntry("DEBUG", "  writing..." + startExecution.ToString, "", "WriteSerialPort", "SNAPCAP")
            Thread.Sleep(100)
            Dim n As Integer = pSerialPort.BytesToRead 'find number of bytes in buf
            WriteSerialPort = pSerialPort.ReadExisting()

            '&&
            'returnvalue = CloseSerialPort()
            'If returnvalue <> "OK" Then
            'WriteSerialPort = returnvalue
            'Exit Function
            'End If

            executionTime = DateTime.UtcNow() - startExecution
            LogSessionEntry("DEBUG", "  WriteSerialPort: " + executionTime.ToString, "", "WriteSerialPort", "SNAPCAP")

        Catch ex As Exception
            WriteSerialPort = "WriteSerialPort: " + ex.ToString
            LogSessionEntry("ERROR", "WriteSerialPort: " + ex.ToString, "", "WriteSerialPort", "SNAPCAP")
        End Try

    End Function

    Function CloseSnapCap(vCOMPort As String) As String
        Dim returnvalue As String
        Dim startExecution As Date
        Dim executionTime As TimeSpan

        CloseSnapCap = "OK"
        Try
            startExecution = DateTime.UtcNow()
            LogSessionEntry("DEBUG", "  CloseSnapCap...", "", "CloseSnapCap", "SNAPCAP")

            FrmMain.Cursor = Cursors.WaitCursor
            startExecution = DateTime.UtcNow()

            'not debugging
            returnvalue = GetStatusSnapCap(vCOMPort, True)
            If returnvalue <> "OK" Then
                FrmMain.Cursor = Cursors.Default
                CloseSnapCap = returnvalue
                Exit Function
            End If

            If pSnapStatus = "OPEN" Or pSnapStatus = "USERABORT" Then
                'close
                returnvalue = WriteSerialPort(vCOMPort, ">C000", True) 'returnvalue contains actual data !

            ElseIf pSnapStatus = "OPENCIRCUIT" Then
                'first open then close
                returnvalue = WriteSerialPort(vCOMPort, ">O000", True) 'returnvalue contains actual data !

                'wait for it to open
                Do While pSnapStatus <> "OPEN"
                    'Thread.Sleep(500)
                    My.Application.DoEvents()
                    'check status again and wait for it to close
                    returnvalue = GetStatusSnapCap(vCOMPort, True)
                    If returnvalue <> "OK" Then
                        FrmMain.Cursor = Cursors.Default
                        CloseSnapCap = returnvalue
                        Exit Function
                    End If

                    If startExecution.AddSeconds(My.Settings.sCoverTimeout) < DateTime.UtcNow() Then
                        LogSessionEntry("ERROR", "", "CloseSnapCap", "", "SNAPCAP")
                        returnvalue = PauseRun("ERROR: Snapcap close timeout: PAUSING EQUIPMENT...", "",
                                               "ERROR: Snapcap close timeout: PauseEquipment: ", "",
                                               "ERROR: equipment paused.", "", "PAUSING", "WAITING")
                        CloseSnapCap = "TIMEOUT"
                        Exit Function
                    End If

                Loop
                returnvalue = WriteSerialPort(vCOMPort, ">C000", True) 'returnvalue contains actual data !
            End If

            'wait for it to close
            Do While pSnapStatus <> "CLOSED"
                'Thread.Sleep(500)
                My.Application.DoEvents()
                'check status again and wait for it to close
                returnvalue = GetStatusSnapCap(vCOMPort, True)
                If returnvalue <> "OK" Then
                    FrmMain.Cursor = Cursors.Default
                    CloseSnapCap = returnvalue
                    Exit Function
                End If

                If startExecution.AddSeconds(My.Settings.sCoverTimeout) < DateTime.UtcNow() Then
                    LogSessionEntry("ERROR", "Snapcap close timeout!", "", "CloseSnapCap", "SNAPCAP")
                    returnvalue = PauseRun("ERROR: Snapcap close timeout: PAUSING EQUIPMENT...", "",
                                           "ERROR: Snapcap close timeout: PauseEquipment: ", "",
                                           "ERROR: equipment paused.", "", "PAUSING", "WAITING")
                    CloseSnapCap = "TIMEOUT"
                    Exit Function
                End If
            Loop
            FrmMain.Cursor = Cursors.Default

            executionTime = DateTime.UtcNow() - startExecution
            LogSessionEntry("DEBUG", "  CloseSnapCap: " + executionTime.ToString, "", "CloseSnapCap", "SNAPCAP")
            LogSessionEntry("BRIEF", "CloseSnapCap succeeded! ", "", "CloseSnapCap", "SNAPCAP")

        Catch ex As Exception
            FrmMain.Cursor = Cursors.Default
            CloseSnapCap = "CloseSnapCap: " + ex.ToString
            LogSessionEntry("ERROR", "CloseSnapCap: " + ex.ToString, "", "CloseSnapCap", "SNAPCAP")
        End Try
    End Function

    Function OpenSnapCap(vCOMPort As String) As String
        Dim returnvalue As String
        Dim startExecution As Date
        Dim executionTime As TimeSpan

        OpenSnapCap = "OK"
        Try
            startExecution = DateTime.UtcNow()
            LogSessionEntry("DEBUG", "  OpenSnapCap...", "", "OpenSnapCap", "SNAPCAP")

            FrmMain.Cursor = Cursors.WaitCursor

            'not debugging
            returnvalue = GetStatusSnapCap(vCOMPort, True)
            If returnvalue <> "OK" Then
                FrmMain.Cursor = Cursors.Default
                OpenSnapCap = returnvalue
                Exit Function
            End If

            If pSnapStatus = "OPENCIRCUIT" Or pSnapStatus = "CLOSED" Then
                'open
                returnvalue = WriteSerialPort(vCOMPort, ">O000", True) 'returnvalue contains actual data !

                'wait for it to open
                Do While pSnapStatus <> "OPEN"
                    'Thread.Sleep(500)
                    My.Application.DoEvents()
                    'check status again and wait for it to close
                    returnvalue = GetStatusSnapCap(vCOMPort, True)
                    If returnvalue <> "OK" Then
                        FrmMain.Cursor = Cursors.Default
                        OpenSnapCap = returnvalue
                        Exit Function
                    End If

                    If startExecution.AddSeconds(My.Settings.sCoverTimeout) < DateTime.UtcNow() Then
                        LogSessionEntry("ERROR", "Snapcap open timeout!", "", "OpenSnapCap", "SNAPCAP")
                        returnvalue = PauseRun("ERROR: Snapcap open timeout: PAUSING EQUIPMENT...", "",
                                               "ERROR: Snapcap open timeout: PauseEquipment: ", "",
                                               "ERROR: equipment paused.", "", "PAUSING", "WAITING")
                        OpenSnapCap = "TIMEOUT"
                        Exit Function
                    End If
                Loop
            End If
            FrmMain.Cursor = Cursors.Default

            executionTime = DateTime.UtcNow() - startExecution
            LogSessionEntry("DEBUG", "  OpenSnapCap : " + executionTime.ToString, "", "OpenSnapCap", "SNAPCAP")
            'LogSessionEntry("BRIEF", "OpenSnapCap succeeded!", "OpenSnapCap", "SNAPCAP")

        Catch ex As Exception
            OpenSnapCap = "OpenSnapCap: " + ex.ToString
            LogSessionEntry("ERROR", "OpenSnapCap : " + ex.ToString, "", "OpenSnapCap", "SNAPCAP")
        End Try
    End Function

    Function GetStatusSnapCap(vCOMPort As String, vShowMessages As Boolean) As String
        Dim returnvalue, tmpSnapMotor, tmpSnapStatus As String
        Dim startExecution As Date
        Dim executionTime As TimeSpan

        GetStatusSnapCap = "OK"
        Try
            startExecution = DateTime.UtcNow()
            LogSessionEntry("DEBUG", "  GetStatusSnapCap...", "", "GetStatusSnapCap", "SNAPCAP")

            returnvalue = WriteSerialPort(vCOMPort, ">S000", vShowMessages) 'returnvalue contains actual data !
            tmpSnapMotor = returnvalue.Substring(2, 1)

            If tmpSnapMotor = "1" Then
                pSnapMotor = "RUNNING"
                pCoverStatus = 2
            ElseIf tmpSnapMotor = "0" Then
                pSnapMotor = "STOPPED"
            Else
                pSnapMotor = "ERROR Reading SnapCap"
                'pCoverConnected = False
                '&& 
                LogSessionEntry("BRIEF", "  ERROR Reading SnapCap: " + executionTime.ToString, "", "GetStatusSnapCap", "SNAPCAP")
            End If

            tmpSnapStatus = returnvalue.Substring(4, 1)

            If tmpSnapStatus = "1" Then
                pSnapStatus = "OPEN"
                pCoverStatus = 3
            ElseIf tmpSnapStatus = "2" Then
                pSnapStatus = "CLOSED"
                pCoverStatus = 1
            ElseIf tmpSnapStatus = "3" Then
                pSnapStatus = "TIMEOUT"
                pCoverStatus = 4
            ElseIf tmpSnapStatus = "4" Then
                pSnapStatus = "OPENCIRCUIT"
                pCoverStatus = 5
            ElseIf tmpSnapStatus = "5" Then
                pSnapStatus = "OVERCURRENT"
                pCoverStatus = 5
            ElseIf tmpSnapStatus = "6" Then
                pSnapStatus = "USERABORT"
                pCoverStatus = 4
            Else
                pSnapStatus = "ERROR Reading SnapCap"
                pCoverStatus = 5
                'pCoverConnected = False
                LogSessionEntry("FULL", "  ERROR Reading SnapCap: " + executionTime.ToString, "", "GetStatusSnapCap", "SNAPCAP")
            End If

            'convert to ASCOM-values
            'NotPresent	0	This device does not have a cover that can be closed independently
            'Closed  1	The cover Is closed
            'Moving  2	The cover Is moving to a New position
            'Open    3	The cover Is open
            'Unknown 4	The state of the cover Is unknown
            'Error 5	The device encountered an Error When changing state 

            executionTime = DateTime.UtcNow() - startExecution
            LogSessionEntry("DEBUG", "  GetStatusSnapCap: " + executionTime.ToString, "", "GetStatusSnapCap", "SNAPCAP")

        Catch ex As Exception
            GetStatusSnapCap = "GetStatusSnapCap: " + ex.ToString
            LogSessionEntry("ERROR", "GetStatusSnapCap: " + ex.ToString, "", "GetStatusSnapCap", "SNAPCAP")
        End Try
    End Function


    Function PartlyOpenSnapCap(vCOMPort As String, vMilliSeconds As Integer) As String
        Dim returnvalue As String
        Dim startExecution As Date
        Dim executionTime As TimeSpan

        PartlyOpenSnapCap = "OK"
        Try
            startExecution = DateTime.UtcNow()
            LogSessionEntry("DEBUG", "  PartlyOpenSnapCap...", "", "PartlyOpenSnapCap", "PROGRAM")

            FrmMain.Cursor = Cursors.WaitCursor
            returnvalue = GetStatusSnapCap(vCOMPort, True)
            If returnvalue <> "OK" Then
                FrmMain.Cursor = Cursors.Default
                PartlyOpenSnapCap = returnvalue
                Exit Function
            End If


            If pSnapStatus = "CLOSED" Then
                returnvalue = WriteSerialPort(vCOMPort, ">O000", True) 'returnvalue contains actual data !
                Thread.Sleep(vMilliSeconds)
                returnvalue = WriteSerialPort(vCOMPort, ">A000", True) 'returnvalue contains actual data !
            Else
                PartlyOpenSnapCap = "Already open !"
            End If
            FrmMain.Cursor = Cursors.Default

            executionTime = DateTime.UtcNow() - startExecution
            LogSessionEntry("DEBUG", "  PartlyOpenSnapCap: " + executionTime.ToString, "", "PartlyOpenSnapCap", "SNAPCAP")

        Catch ex As Exception
            FrmMain.Cursor = Cursors.Default
            PartlyOpenSnapCap = "PartlyOpenSnapCap: " + ex.ToString
            LogSessionEntry("ERROR", "PartlyOpenSnapCap: " + ex.ToString, "", "PartlyOpenSnapCap", "SNAPCAP")
        End Try
    End Function

    Function LightSnapCap(vCOMPort As String, vLevel As Integer) As String
        Dim returnvalue As String
        Dim lightlevel As Double
        Dim startExecution As Date
        Dim executionTime As TimeSpan

        LightSnapCap = "OK"
        Try
            startExecution = DateTime.UtcNow()
            LogSessionEntry("DEBUG", "  LightSnapCap...", "", "LightSnapCap", "PROGRAM")

            lightlevel = (vLevel / 100) * 255

            If vLevel > 0 Then
                'turn the light on and set level
                returnvalue = WriteSerialPort(vCOMPort, ">L000", True) 'returnvalue contains actual data !
                Thread.Sleep(100)
                returnvalue = WriteSerialPort(vCOMPort, ">B" + Format(lightlevel, "000"), True) 'returnvalue contains actual data !
            Else
                'turn the light off
                returnvalue = WriteSerialPort(vCOMPort, ">D000", True) 'returnvalue contains actual data !
            End If

            executionTime = DateTime.UtcNow() - startExecution
            LogSessionEntry("DEBUG", "  LightSnapCap: " + executionTime.ToString, "", "LightSnapCap", "SNAPCAP")

        Catch ex As Exception
            LightSnapCap = "LightSnapCap: " + ex.ToString
            LogSessionEntry("ERROR", "LightSnapCap : " + ex.ToString, "", "LightSnapCap", "SNAPCAP")
        End Try
    End Function
End Module