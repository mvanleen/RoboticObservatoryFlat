Imports System.ComponentModel
Imports System.Data.Entity
Imports System.IO
Imports System.Runtime.InteropServices.WindowsRuntime
Imports System.Runtime.Serialization.Formatters
Imports System.Text.RegularExpressions
Imports System.Threading
Imports ASCOM.Astrometry
Imports Microsoft.VisualBasic.ApplicationServices
'Imports TheSky6Library
'Imports TheSkyXLib

'WHEN IMAGE IS CROPPED DURING IMAGE LINK: GOTO EDIT PREFERENCE, ALL THE WAY DOWN AND SET TO 100

Module ModTheSkyX
    Public pTheSkyXApp As TheSky64Lib.Application
    Public pTheSkyXCamera As TheSky64Lib.ccdsoftCamera
    Public pTheSkyXImage As New TheSky64Lib.ccdsoftImage
    Public pTheSkyXImageLink As TheSky64Lib.ImageLink
    Public pTheSkyXImageLinkResults As TheSky64Lib.ImageLinkResults
    Public pTheSkyXConnected As Boolean
    Public pTheSkyXEquipmentConnected As Boolean
    Public pTheSkyXTakingImage As Boolean
    Public pTheSkyXExposureStatus As String
    Public pTheSkyXCCDTemp As Double
    Public pTheSkyXChart As TheSky64Lib.sky6StarChart
    Public pTheSkyXObjInfo As TheSky64Lib.sky6ObjectInformation
    'Public pThesky6RASCOMTele As TheSkyXLib.sky6RASCOMTele
    Public pTheskXRASCOMTele6 As TheSky64Lib.sky6RASCOMTele

    Public pFWHMFullFrameMedian As Double
    Public pFWHMFullFrameAverage As Double
    Public pFWHMFullFrameNbrStars As Integer
    Public pImageDatedPath As String 'sImagePath from my settings added with a date portion
    Public pImageSavePath As String ' pImageSavePath + filename: used for solving

    Public pCurrentFocusserPosition As Integer
    Public pCurrentFocusserTemperature As Double
    Public pInitialFocusTemperature As Double 'original focus temperature

    Public pCurrentFWHM As Double
    Public pCurrentFilterNumber As Integer
    Public pCurrentFilterName As String

    Public pSolveRA2000 As Double 'RA2000 after solve in Double, used to show message of solved coordinates
    Public pSolveDEC2000 As Double

    Public pSolveRATopocentric As Double 'RA topocentric after solve in Double
    Public pSolveDECTopocentric As Double

    Public pTotalSolveErrorRA As Double 'total error in RA after solve in Double
    Public pTotalSolveErrorDEC As Double

    Public pContinueRunningCalibrationFrames As Boolean
    Public pTheSkyXImageAverageValue As Double

    Public Structure StructTargetObject
        Public Altitude As Double
        Public Azimuth As Double
        Public RightAscension As Double
        Public Declination As Double
        Public TransitTime As Double
        Public RiseTime As Double
        Public SetTime As Double
        Public HA As Double
    End Structure

    Public pStrucTargetObject As StructTargetObject

    Public pTheSkyXErrCode As Integer
    Public pTheSkyXErrMessage As String

    Public pNbrFocusSamples As Integer

    Private WithEvents BgwImageLink As BackgroundWorker
    Private WithEvents BgwAtFocus3 As BackgroundWorker
    Private bgwErrorImageLink As String
    Private bgwErrorAtFocus3 As String


    '--------------------------------------------------------------------------------
    ' HELPER
    '--------------------------------------------------------------------------------

    Public Function DefineImageDatedPath(vType As String) As String
        'NORMAL / PLATESOLVE / FOCUS
        Dim SubFolder As String
        Dim startExecution As Date
        Dim executionTime As TimeSpan

        DefineImageDatedPath = "OK"
        Try
            startExecution = DateTime.UtcNow()
            LogSessionEntry("DEBUG", "  DefineImageDatedPath...", "", "DefineImageDatedPath", "TSX")

            If Convert.ToInt32(DateTime.UtcNow.ToString("HH")) < 12 Then
                'images still belong to previous day
                SubFolder = DateTime.UtcNow.AddDays(-1).ToString("yyyyMMdd")
            Else
                'images belong to today
                SubFolder = DateTime.UtcNow.ToString("yyyyMMdd")
            End If


            If My.Settings.sCCDImagePath.Last = "\" Then
                pImageDatedPath = My.Settings.sCCDImagePath + SubFolder + "\"
            Else
                pImageDatedPath = My.Settings.sCCDImagePath + "\" + SubFolder + "\"
            End If

            If vType = "PLATESOLVE" Then
                pImageDatedPath += "Platesolve\"
            ElseIf vType = "FOCUS" Then
                pImageDatedPath += "Focus\"
            End If


            executionTime = DateTime.UtcNow() - startExecution
            LogSessionEntry("DEBUG", "  DefineImageDatedPath: " + executionTime.ToString, "", "DefineImageDatedPath", "TSX")

        Catch ex As Exception
            DefineImageDatedPath = "DefineImageDatedPath: " + ex.Message
            LogSessionEntry("ERROR", "DefineImageDatedPath: " + ex.Message, "", "DefineImageDatedPath", "TSX")
        End Try
    End Function


    '--------------------------------------------------------------------------------
    ' CONNECT THE SKY X
    '--------------------------------------------------------------------------------
    Public Function ConnectTheSkyXApplication() As String
        Dim startExecution As Date
        Dim executionTime As TimeSpan

        ConnectTheSkyXApplication = "OK"
        Try
            startExecution = DateTime.UtcNow()
            LogSessionEntry("DEBUG", "  ConnectTheSkyXApplication...", "", "ConnectTheSkyXApplication", "TSX")

            FrmMain.Cursor = Cursors.WaitCursor
            pTheSkyXApp = New TheSky64Lib.Application
            pTheSkyXConnected = True

            'pThesky6RASCOMTele = New TheSkyXLib.sky6RASCOMTele
            pTheskXRASCOMTele6 = New TheSky64Lib.sky6RASCOMTele

            FrmMain.Cursor = Cursors.Default
            executionTime = DateTime.UtcNow() - startExecution
            LogSessionEntry("DEBUG", "  ConnectTheSkyXApplication: " + executionTime.ToString, "", "ConnectTheSkyXApplication", "TSX")

        Catch ex As Exception
            FrmMain.Cursor = Cursors.Default
            ConnectTheSkyXApplication = "ConnectTheSkyXApplication: " + ex.Message
            LogSessionEntry("ERROR", "ConnectTheSkyXApplication: " + ex.Message, "", "ConnectTheSkyXApplication", "TSX")
        End Try
    End Function

    Public Function DisconnectTheSkyXApplication() As String
        Dim startExecution As Date
        Dim executionTime As TimeSpan

        DisconnectTheSkyXApplication = "OK"
        Try
            startExecution = DateTime.UtcNow()
            LogSessionEntry("DEBUG", "  DisconnectTheSkyXApplication...", "", "DisconnectTheSkyXApplication", "TSX")

            FrmMain.Cursor = Cursors.WaitCursor
            pTheSkyXApp = Nothing
            pTheSkyXConnected = False
            FrmMain.Cursor = Cursors.Default

            executionTime = DateTime.UtcNow() - startExecution
            LogSessionEntry("DEBUG", "  DisconnectTheSkyXApplication: " + executionTime.ToString, "", "DisconnectTheSkyXApplication", "TSX")

        Catch ex As Exception
            FrmMain.Cursor = Cursors.Default
            DisconnectTheSkyXApplication = "DisconnectTheSkyXApplication: " + ex.Message
            LogSessionEntry("ERROR", "DisconnectTheSkyXApplication: " + ex.Message, "", "DisconnectTheSkyXApplication", "TSX")
        End Try
    End Function

    Public Function CheckIsRunningTheSkyX() As String
        Dim TSXRunning As String
        Dim startExecution As Date
        Dim executionTime As TimeSpan

        CheckIsRunningTheSkyX = "OK"
        Try
            startExecution = DateTime.UtcNow()
            LogSessionEntry("DEBUG", "  ResetCalculateEventTimes...", "", "ResetCalculateEventTimes", "TSX")

            TSXRunning = CheckIfRunning("TheSky64")
            If TSXRunning = "OK" Then
                FrmMain.LblTSX.BackColor = ColorTranslator.FromHtml("#4cd137") 'green
            Else
                FrmMain.LblTSX.BackColor = ColorTranslator.FromHtml("#d63031") 'red
                CheckIsRunningTheSkyX = "NOK"
                LogSessionEntry("ERROR", "TheSkyX is not running!", "", "ConnectTheSkyXFocusser", "TSX")
            End If
            executionTime = DateTime.UtcNow() - startExecution
            LogSessionEntry("DEBUG", "  CheckIsRunningTheSkyX: " + executionTime.ToString, "", "CheckIsRunningTheSkyX", "TSX")

        Catch ex As Exception
            CheckIsRunningTheSkyX = "CheckIsRunningTheSkyX: " + ex.Message
            LogSessionEntry("ERROR", "CheckIsRunningTheSkyX: " + ex.Message, "", "CheckIsRunningTheSkyX", "TSX")
        End Try
    End Function


    '--------------------------------------------------------------------------------
    ' CONNECT DISCONNECT ALL CCD RELATED EQUIPMENT
    '--------------------------------------------------------------------------------
    Public Function ConnectTheSkyXEquipment(vShowMessages As Boolean) As String
        Dim returnvalue As String
        Dim startExecution As Date
        Dim executionTime As TimeSpan

        ConnectTheSkyXEquipment = "OK"
        Try
            startExecution = DateTime.UtcNow()
            LogSessionEntry("DEBUG", "  ConnectTheSkyXEquipment...", "", "ConnectTheSkyXEquipment", "TSX")

            returnvalue = ConnectTheSkyXCamera(vShowMessages)
            If returnvalue <> "OK" Then
                ConnectTheSkyXEquipment = returnvalue
                Exit Function
            End If
            'WaitSeconds(2)

            returnvalue = ConnectTheSkyXFocusser(vShowMessages)
            If returnvalue <> "OK" Then
                ConnectTheSkyXEquipment = returnvalue
                Exit Function
            End If
            'WaitSeconds(2)

            returnvalue = ConnectTheSkyXFilterWheel(vShowMessages)
            If returnvalue <> "OK" Then
                ConnectTheSkyXEquipment = returnvalue
                Exit Function
            End If
            'WaitSeconds(2)

            'check TheSkyXCCD: fill fields on main form
            returnvalue = CheckTheSkyXCCD()
            If returnvalue <> "OK" Then
                ConnectTheSkyXEquipment = "CheckTheSkyXCCD: " + returnvalue
            End If
            pTheSkyXEquipmentConnected = True

            executionTime = DateTime.UtcNow() - startExecution
            LogSessionEntry("DEBUG", "  ConnectTheSkyXEquipment: " + executionTime.ToString, "", "ConnectTheSkyXEquipment", "TSX")

        Catch ex As Exception
            ConnectTheSkyXEquipment = "ConnectTheSkyXEquipment: " + ex.Message
            If vShowMessages = True Then
                LogSessionEntry("ERROR", "ConnectTheSkyXEquipment: " + ex.Message, "", "ConnectTheSkyXEquipment", "TSX")
            End If

        End Try

    End Function

    Public Function DisconnectTheSkyXEquipment() As String
        Dim returnvalue As String
        Dim startExecution As Date
        Dim executionTime As TimeSpan

        DisconnectTheSkyXEquipment = "OK"
        Try
            startExecution = DateTime.UtcNow()
            LogSessionEntry("DEBUG", "  DisconnectTheSkyXEquipment...", "", "DisconnectTheSkyXEquipment", "TSX")

            returnvalue = DisconnectTheSkyXCamera()
            If returnvalue <> "OK" Then
                DisconnectTheSkyXEquipment = returnvalue
                Exit Function
            End If

            returnvalue = DisconnectTheSkyXFocusser()
            If returnvalue <> "OK" Then
                DisconnectTheSkyXEquipment = returnvalue
                Exit Function
            End If

            returnvalue = DisconnectTheSkyXFilterWheel()
            If returnvalue <> "OK" Then
                DisconnectTheSkyXEquipment = returnvalue
                Exit Function
            End If

            pTheSkyXEquipmentConnected = False

            'check TheSkyXCCD: fill fields on main form
            'returnvalue = CheckTheSkyXCCD()
            'If returnvalue <> "OK" Then
            'DisconnectTheSkyXEquipment = "CheckTheSkyXCCD: " + returnvalue
            'End If

            executionTime = DateTime.UtcNow() - startExecution
            LogSessionEntry("DEBUG", "  DisconnectTheSkyXEquipment: " + executionTime.ToString, "", "DisconnectTheSkyXEquipment", "TSX")

        Catch ex As Exception
            DisconnectTheSkyXEquipment = "DisconnectTheSkyXEquipment: " + ex.Message
            LogSessionEntry("ERROR", "DisconnectTheSkyXEquipment: " + ex.Message, "", "DisconnectTheSkyXEquipment", "TSX")
        End Try

    End Function

    '--------------------------------------------------------------------------------
    ' FOCUSSER
    '--------------------------------------------------------------------------------
    Public Function ConnectTheSkyXFocusser(vShowMessages As Boolean) As String
        Dim startExecution As Date
        Dim executionTime As TimeSpan

        ConnectTheSkyXFocusser = "OK"
        Try
            startExecution = DateTime.UtcNow()
            LogSessionEntry("DEBUG", "  ConnectTheSkyXFocusser...", "", "ConnectTheSkyXFocusser", "TSX")

            FrmMain.Cursor = Cursors.WaitCursor
            If vShowMessages = True Then
                LogSessionEntry("BRIEF", "Connecting to the focusser...", "", "ConnectTheSkyXFocusser", "TSX")
            End If

            pTheSkyXCamera.focConnect()
            LogSessionEntry("FULL", "Focusser connected.", "", "ConnectTheSkyXFocusser", "TSX")

            FrmMain.Cursor = Cursors.Default
            executionTime = DateTime.UtcNow() - startExecution
            LogSessionEntry("DEBUG", "  ConnectTheSkyXFocusser: " + executionTime.ToString, "", "ConnectTheSkyXFocusser", "TSX")

        Catch ex As Exception
            FrmMain.Cursor = Cursors.Default
            ConnectTheSkyXFocusser = "Connect TheSkyX focusser: " + ex.Message
            If vShowMessages = True Then
                LogSessionEntry("ERROR", "ConnectTheSkyXFocusser: " + ex.Message, "", "ConnectTheSkyXFocusser", "TSX")
            Else
                LogSessionEntry("FULL", "Focusser not connected.", "", "ConnectTheSkyXFocusser", "TSX")
            End If
        End Try
    End Function


    Public Function DisconnectTheSkyXFocusser() As String
        Dim startExecution As Date
        Dim executionTime As TimeSpan

        DisconnectTheSkyXFocusser = "OK"
        Try
            FrmMain.Cursor = Cursors.WaitCursor
            startExecution = DateTime.UtcNow()
            LogSessionEntry("DEBUG", "  DisconnectTheSkyXFocusser...", "", "DisconnectTheSkyXFocusser", "TSX")

            LogSessionEntry("BRIEF", "Disconnecting the focusser...", "", "DisconnectTheSkyXFocusser", "TSX")
            pTheSkyXCamera.focDisconnect()
            LogSessionEntry("FULL", "Focusser disconnected.", "", "DisconnectTheSkyXFocusser", "TSX")

            FrmMain.Cursor = Cursors.Default
            executionTime = DateTime.UtcNow() - startExecution
            LogSessionEntry("DEBUG", "  DisconnectTheSkyXFocusser: " + executionTime.ToString, "", "DisconnectTheSkyXFocusser", "TSX")

        Catch ex As Exception
            FrmMain.Cursor = Cursors.Default
            DisconnectTheSkyXFocusser = "Disconnect TheSkyX focusser: " + ex.Message
            LogSessionEntry("ERROR", "DisconnectTheSkyXFocusser: " + ex.Message, "", "DisconnectTheSkyXFocusser", "TSX")
        End Try
    End Function

    Public Function MoveTheSkyXFocusser(vTXTDirection As String, vINTSteps As Integer) As String
        'Number of steps, IN/OUT
        Dim returnvalue As String
        Dim startExecution As Date
        Dim executionTime As TimeSpan

        MoveTheSkyXFocusser = "OK"
        Try
            startExecution = DateTime.UtcNow()
            LogSessionEntry("DEBUG", "  MoveTheSkyXFocusser...", "", "MoveTheSkyXFocusser", "TSX")

            FrmMain.Cursor = Cursors.WaitCursor
            LogSessionEntry("FULL", "Moving the focusser " + vTXTDirection + ":" + Format(vINTSteps) + " steps.", "", "MoveTheSkyXFocusser", "TSX")
            If vTXTDirection = "IN" Then
                pTheSkyXCamera.focMoveIn(vINTSteps)
            Else
                pTheSkyXCamera.focMoveOut(vINTSteps)
            End If

            'check TheSkyXCCD: fill fields on main form
            returnvalue = CheckTheSkyXCCD()
            If returnvalue <> "OK" Then
                MoveTheSkyXFocusser = "CheckTheSkyXCCD: " + returnvalue
            End If
            FrmMain.Cursor = Cursors.Default

            executionTime = DateTime.UtcNow() - startExecution
            LogSessionEntry("DEBUG", "  MoveTheSkyXFocusser: " + executionTime.ToString, "", "MoveTheSkyXFocusser", "TSX")

        Catch ex As Exception
            FrmMain.Cursor = Cursors.Default
            MoveTheSkyXFocusser = "Moving TheSkyX focusser: " + vTXTDirection + ":" + Format(vINTSteps) + " steps: " + ex.Message
            LogSessionEntry("ERROR", "MoveTheSkyXFocusser: " + ex.Message, "", "MoveTheSkyXFocusser", "TSX")
        End Try
    End Function

    Public Function MoveAbsoluteTheSkyXFocusser(vINTSteps As Integer) As String
        'move to absolute position
        Dim returnvalue As String
        Dim steps As Integer
        Dim startExecution As Date
        Dim executionTime As TimeSpan

        MoveAbsoluteTheSkyXFocusser = "OK"
        Try
            startExecution = DateTime.UtcNow()
            LogSessionEntry("DEBUG", "  MoveAbsoluteTheSkyXFocusser...", "", "MoveAbsoluteTheSkyXFocusser", "TSX")

            FrmMain.Cursor = Cursors.WaitCursor
            'get the actual focus position
            returnvalue = GetTheSkyXFocusserPosition()
            If returnvalue <> "OK" Then
                MoveAbsoluteTheSkyXFocusser = returnvalue
                FrmMain.Cursor = Cursors.Default
                MoveAbsoluteTheSkyXFocusser = "NOK"
                Exit Function
            End If

            steps = pCurrentFocusserPosition - vINTSteps

            If steps > 0 Then
                pTheSkyXCamera.focMoveIn(steps)
            Else
                pTheSkyXCamera.focMoveOut(steps)
            End If

            'check TheSkyXCCD: fill fields on main form
            returnvalue = CheckTheSkyXCCD()
            If returnvalue <> "OK" Then
                MoveAbsoluteTheSkyXFocusser = "CheckTheSkyXCCD: " + returnvalue
            End If

            FrmMain.Cursor = Cursors.Default
            LogSessionEntry("FULL", "Moving the focusser to position " + Format(vINTSteps) + ".", "", "MoveAbsoluteTheSkyXFocusser", "TSX")
            executionTime = DateTime.UtcNow() - startExecution
            LogSessionEntry("DEBUG", "  MoveAbsoluteTheSkyXFocusser: " + executionTime.ToString, "", "MoveAbsoluteTheSkyXFocusser", "TSX")

        Catch ex As Exception
            FrmMain.Cursor = Cursors.Default
            MoveAbsoluteTheSkyXFocusser = "Moving TheSkyX focusser to: " + Format(vINTSteps) + " steps: " + ex.Message
            LogSessionEntry("ERROR", "MoveAbsoluteTheSkyXFocusser: " + ex.Message, "", "MoveAbsoluteTheSkyXFocusser", "TSX")
        End Try
    End Function

    Public Function GetTheSkyXFocusserPosition() As String
        'number of steps, IN/OUT
        Dim startExecution As Date
        Dim executionTime As TimeSpan

        GetTheSkyXFocusserPosition = "OK"
        Try
            startExecution = DateTime.UtcNow()
            LogSessionEntry("DEBUG", "  GetTheSkyXFocusserPosition...", "", "GetTheSkyXFocusserPosition", "TSX")

            pCurrentFocusserPosition = pTheSkyXCamera.focPosition
            pCurrentFocusserTemperature = pTheSkyXCamera.focTemperature

            executionTime = DateTime.UtcNow() - startExecution
            LogSessionEntry("DEBUG", "  GetTheSkyXFocusserPosition: " + executionTime.ToString, "", "GetTheSkyXFocusserPosition", "TSX")

        Catch ex As Exception
            GetTheSkyXFocusserPosition = "GetTheSkyXFocusserPosition: " + ex.Message
            LogSessionEntry("ERROR", "GetTheSkyXFocusserPosition: " + ex.Message, "", "GetTheSkyXFocusserPosition", "TSX")
        End Try
    End Function


    '--------------------------------------------------------------------------------
    ' CCD RELATED STUFF
    '--------------------------------------------------------------------------------
    '----------------------------------------
    ' CONNECTING
    '----------------------------------------
    Public Function ConnectTheSkyXCamera(vShowMessages As Boolean) As String
        Dim startExecution As Date
        Dim executionTime As TimeSpan

        ConnectTheSkyXCamera = "OK"
        Try
            startExecution = DateTime.UtcNow()
            LogSessionEntry("DEBUG", "  ConnectTheSkyXCamera...", "", "ConnectTheSkyXCamera", "TSX")

            FrmMain.Cursor = Cursors.WaitCursor
            If vShowMessages = True Then
                LogSessionEntry("BRIEF", "Connecting to the camera...", "", "ConnectTheSkyXCamera", "TSX")
            End If

            pTheSkyXCamera = New TheSky64Lib.ccdsoftCamera
            pTheSkyXCamera.Connect()
            pTheSkyXCamera.Asynchronous = vbTrue
            pTheSkyXCamera.AutoSaveOn = vbFalse

            pTheSkyXCamera.TemperatureSetPoint = My.Settings.sCCDCoolingTemp
            pTheSkyXCamera.RegulateTemperature = Convert.ToInt32(My.Settings.sCCDCoolingEnabled)
            LogSessionEntry("FULL", "Camera connected.", "", "ConnectTheSkyXCamera", "TSX")

            FrmMain.Cursor = Cursors.Default

            executionTime = DateTime.UtcNow() - startExecution
            LogSessionEntry("DEBUG", "  ConnectTheSkyXCamera: " + executionTime.ToString, "", "ConnectTheSkyXCamera", "TSX")

        Catch ex As Exception
            FrmMain.Cursor = Cursors.Default
            ConnectTheSkyXCamera = "Connect TheSkyX camera: " + ex.Message
            If vShowMessages = True Then
                LogSessionEntry("ERROR", "ConnectTheSkyXCamera: " + ex.Message, "", "ConnectTheSkyXCamera", "TSX")
            Else
                LogSessionEntry("FULL", "Camera not connected.", "", "ConnectTheSkyXCamera", "TSX")
            End If

        End Try
    End Function

    Public Function DisconnectTheSkyXCamera() As String
        Dim startExecution As Date
        Dim executionTime As TimeSpan

        DisconnectTheSkyXCamera = "OK"
        Try
            startExecution = DateTime.UtcNow()
            LogSessionEntry("DEBUG", "  DisconnectTheSkyXCamera...", "", "DisconnectTheSkyXCamera", "TSX")

            FrmMain.Cursor = Cursors.WaitCursor
            LogSessionEntry("BRIEF", "Disconnecting the camera...", "", "DisconnectTheSkyXCamera", "TSX")
            pTheSkyXCamera.Disconnect()
            pTheSkyXCamera.ShutDownTemperatureRegulationOnDisconnect = vbTrue
            LogSessionEntry("FULL", "Camera disconnected.", "", "DisconnectTheSkyXCamera", "TSX")
            FrmMain.Cursor = Cursors.Default

            executionTime = DateTime.UtcNow() - startExecution
            LogSessionEntry("DEBUG", "  DisconnectTheSkyXCamera: " + executionTime.ToString, "", "DisconnectTheSkyXCamera", "TSX")
        Catch ex As Exception
            FrmMain.Cursor = Cursors.Default
            DisconnectTheSkyXCamera = "Disconnect TheSkyX camera: " + ex.Message
            LogSessionEntry("ERROR", "DisconnectTheSkyXCamera: " + ex.Message, "", "DisconnectTheSkyXCamera", "TSX")
        End Try
    End Function

    '----------------------------------------
    ' FILTER WHEEL
    '----------------------------------------
    Public Function ConnectTheSkyXFilterWheel(vShowMessages As Boolean) As String
        Dim startExecution As Date
        Dim executionTime As TimeSpan

        ConnectTheSkyXFilterWheel = "OK"
        Try
            startExecution = DateTime.UtcNow()
            LogSessionEntry("DEBUG", "  ConnectTheSkyXFilterWheel...", "", "ConnectTheSkyXFilterWheel", "TSX")

            FrmMain.Cursor = Cursors.WaitCursor
            If vShowMessages = True Then
                LogSessionEntry("BRIEF", "Connecting to the filterwheel...", "", "ConnectTheSkyXFilterWheel", "TSX")
            End If

            pTheSkyXCamera.filterWheelConnect()
            LogSessionEntry("FULL", "Filterwheel connected.", "", "ConnectTheSkyXFilterWheel", "TSX")

            FrmMain.Cursor = Cursors.Default
            executionTime = DateTime.UtcNow() - startExecution
            LogSessionEntry("DEBUG", "  ConnectTheSkyXFilterWheel: " + executionTime.ToString, "", "ConnectTheSkyXFilterWheel", "TSX")

        Catch ex As Exception
            FrmMain.Cursor = Cursors.Default
            ConnectTheSkyXFilterWheel = "Connect TheSkyX filterwheel: " + ex.Message
            If vShowMessages = True Then
                LogSessionEntry("ERROR", "ConnectTheSkyXFilterWheel: " + ex.Message, "", "ConnectTheSkyXFilterWheel", "TSX")
            Else
                LogSessionEntry("FULL", "Filterwheel not connected.", "", "ConnectTheSkyXFilterWheel", "TSX")
            End If
        End Try
    End Function

    Public Function DisconnectTheSkyXFilterWheel() As String
        Dim startExecution As Date
        Dim executionTime As TimeSpan

        DisconnectTheSkyXFilterWheel = "OK"
        Try
            startExecution = DateTime.UtcNow()
            LogSessionEntry("DEBUG", "  DisconnectTheSkyXFilterWheel...", "", "DisconnectTheSkyXFilterWheel", "TSX")

            FrmMain.Cursor = Cursors.WaitCursor
            LogSessionEntry("BRIEF", "Disconnecting the filterwheel...", "", "DisconnectTheSkyXFilterWheel", "TSX")
            pTheSkyXCamera.filterWheelDisconnect()
            LogSessionEntry("FULL", "Filterwheel disconnected.", "", "DisconnectTheSkyXFilterWheel", "TSX")

            FrmMain.Cursor = Cursors.Default
            executionTime = DateTime.UtcNow() - startExecution
            LogSessionEntry("DEBUG", "  DisconnectTheSkyXFilterWheel" + executionTime.ToString, "", "Filterwheel disconnected", "TSX")

        Catch ex As Exception
            FrmMain.Cursor = Cursors.Default
            DisconnectTheSkyXFilterWheel = "Disconnect TheSkyX filterwheel: " + ex.Message
            LogSessionEntry("ERROR", "DisconnectTheSkyXFilterWheel: " + ex.Message, "", "DisconnectTheSkyXFilterWheel", "TSX")
        End Try
    End Function
    Public Function GetTheSkyXCurrrentFilter() As String
        Dim startExecution As Date
        Dim executionTime As TimeSpan

        GetTheSkyXCurrrentFilter = "OK"
        Try
            startExecution = DateTime.UtcNow()
            LogSessionEntry("DEBUG", "  GetTheSkyXCurrrentFilter...", "", "GetTheSkyXCurrrentFilter", "TSX")

            pCurrentFilterNumber = pTheSkyXCamera.PropLng("m_nFilterIndex Real-Time")
            'get the filtername
            Select Case pCurrentFilterNumber
                Case 0
                    pCurrentFilterName = My.Settings.sCCDFilter1
                Case 1
                    pCurrentFilterName = My.Settings.sCCDFilter2
                Case 2
                    pCurrentFilterName = My.Settings.sCCDFilter3
                Case 3
                    pCurrentFilterName = My.Settings.sCCDFilter4
                Case 4
                    pCurrentFilterName = My.Settings.sCCDFilter5
            End Select
            LogSessionEntry("DEBUG", "  Current filter:" + pCurrentFilterName, "", "GetTheSkyXCurrrentFilter", "TSX")

            executionTime = DateTime.UtcNow() - startExecution
            LogSessionEntry("DEBUG", "  GetTheSkyXCurrrentFilter: " + executionTime.ToString, "", "GetTheSkyXCurrrentFilter", "TSX")

        Catch ex As Exception
            GetTheSkyXCurrrentFilter = "GetTheSkyXCurrrentFilter: " + ex.Message
            LogSessionEntry("ERROR", "GetTheSkyXCurrrentFilter: " + ex.Message, "", "GetTheSkyXCurrrentFilter", "TSX")
        End Try
    End Function


    '----------------------------------------
    ' TAKE IMAGE
    '----------------------------------------
    Public Function TakeImageTheSkyX(vExposureTime As Double, Binning As Integer, vImageName As String, vSubFrame As Boolean, vLinkImage As Boolean, vFilterNumber As Integer, vFilterName As String, vMessageText As String, vManual As Boolean, vFrameType As String, vRA As String, vDEC As String) As String
        Dim startExecution As Date
        Dim executionTime As TimeSpan
        Dim returnvalue As String

        TakeImageTheSkyX = "OK"
        Try
            startExecution = DateTime.UtcNow()
            LogSessionEntry("DEBUG", "  TakeImageTheSkyX...", "", "TakeImageTheSkyX", "TSX")

            pIsActionRunning = True
            ' if run is to abort: exit
            If (pAbort = True And vManual = False) Or pToolsAbort = True Then
                pIsActionRunning = False
                pTheSkyXExposureStatus = ""
                TakeImageTheSkyX = "IMAGE_ABORTED"
                Exit Function
            End If

            'check if there is already an image exposing
            Try
                If pTheSkyXCamera.IsExposureComplete = 0 Then
                    'abort the current image
                    pTheSkyXCamera.Abort()
                End If
            Catch
            End Try

            If vImageName = "PLATESOLVE" Then
                DefineImageDatedPath("PLATESOLVE")
            Else
                DefineImageDatedPath("NORMAL")
            End If

            'check if camera is cooled if not wait
            If vImageName <> "PLATESOLVE" Then
                'check the camera is at the proper temperature
                Do While Math.Abs(pTheSkyXCCDTemp) + My.Settings.sCCDCoolingTemp > 2
                    LogSessionEntry("BRIEF", "Waiting for camera to cool down...", "", "TakeImageTheSkyX", "TSX")
                    WaitSeconds(30, False, True)
                Loop
            End If

            LogSessionEntry("BRIEF", "Starting " + Format(vExposureTime) + "s " + vMessageText + "...", "", "TakeImageTheSkyX", "TSX")
            pTheSkyXCamera.ExposureTime = vExposureTime
            pTheSkyXCamera.BinX = Binning
            pTheSkyXCamera.BinY = Binning

            'set a subframe if applicable
            If vSubFrame = True And Convert.ToDouble(My.Settings.sCCDFocusSubFrame) <> 100 Then
                pTheSkyXCamera.Subframe = 1
                Dim SubframeLeft = Math.Round((My.Settings.sCCDSensorSizeX / 2) - ((My.Settings.sCCDSensorSizeX * (Convert.ToDouble(My.Settings.sCCDFocusSubFrame) / 100)) / 2), 0)
                Dim SubframeRight = Math.Round((My.Settings.sCCDSensorSizeX / 2) + ((My.Settings.sCCDSensorSizeX * (Convert.ToDouble(My.Settings.sCCDFocusSubFrame) / 100)) / 2), 0)
                Dim SubframeTop = Math.Round((My.Settings.sCCDSensorSizeY / 2) - ((My.Settings.sCCDSensorSizeY * (Convert.ToDouble(My.Settings.sCCDFocusSubFrame) / 100)) / 2), 0)
                Dim SubframeBottom = Math.Round((My.Settings.sCCDSensorSizeY / 2) + ((My.Settings.sCCDSensorSizeY * (Convert.ToDouble(My.Settings.sCCDFocusSubFrame) / 100)) / 2), 0)

                pTheSkyXCamera.SubframeTop = Convert.ToInt32(SubframeTop)
                pTheSkyXCamera.SubframeLeft = Convert.ToInt32(SubframeLeft)
                pTheSkyXCamera.SubframeRight = Convert.ToInt32(SubframeRight)
                pTheSkyXCamera.SubframeBottom = Convert.ToInt32(SubframeBottom)
            Else pTheSkyXCamera.Subframe = 0
            End If

            'set the filter used for the image
            pTheSkyXCamera.FilterIndexZeroBased = vFilterNumber
            pTheSkyXCamera.Asynchronous = 1

            'first do events so no double commands are given to TSX
            My.Application.DoEvents()

            ' if run is to abort: exit
            If (pAbort = True And vManual = False) Or pToolsAbort = True Then
                pIsActionRunning = False
                pTheSkyXExposureStatus = ""
                TakeImageTheSkyX = "IMAGE_ABORTED"
                Exit Function
            End If

            pTheSkyXCamera.TakeImage()
            pTheSkyXTakingImage = True

            Do While pTheSkyXTakingImage = True
                Thread.Sleep(250)
                My.Application.DoEvents()

                pTheSkyXExposureStatus = pTheSkyXCamera.ExposureStatus

                If pTheSkyXCamera.IsExposureComplete = 1 Then
                    pTheSkyXTakingImage = False
                End If

                ' if run is to abort: exit
                If (pAbort = True And vManual = False) Or pToolsAbort = True Then
                    pTheSkyXCamera.Abort()
                    pTheSkyXExposureStatus = ""
                    pIsActionRunning = False
                    TakeImageTheSkyX = "IMAGE_ABORTED"
                    Exit Function
                End If

                If startExecution.AddSeconds(vExposureTime + My.Settings.sCCDTimeout) < DateTime.UtcNow() Then
                    LogSessionEntry("ERROR", "Take image The Sky X timeout!", "", "TakeImageTheSkyX", "TSX")
                    returnvalue = PauseRun("ERROR: Take image The Sky X timeout: PAUSING EQUIPMENT...", "",
                                                       "ERROR: Take image The Sky X timeout: PauseEquipment", "",
                                                       "ERROR: equipment paused.", "", "PAUSING", "WAITING")
                    TakeImageTheSkyX = "IMAGE_ABORTED"
                    Exit Function
                End If
            Loop

            ' if run is to abort: exit
            If (pAbort = True And vManual = False) Or pToolsAbort = True Then
                pIsActionRunning = False
                pTheSkyXExposureStatus = ""
                TakeImageTheSkyX = "IMAGE_ABORTED"
                Exit Function
            End If

            pTheSkyXImage = New TheSky64Lib.ccdsoftImage
            pTheSkyXImage.AttachToActiveImager()

            'check and create folder
            If Directory.Exists(pImageDatedPath) = False Then
                Directory.CreateDirectory(pImageDatedPath)
            End If

            'get rid of special characters in filename
            Dim cleanImageName As String
            cleanImageName = RemoveIllegalFileNameChars(vImageName)

            pImageSavePath = pImageDatedPath + cleanImageName + "_" + vFilterName + "_Exp" + Format(vExposureTime) + "s_Bin" + Format(Binning) + "_" + DateTime.UtcNow.ToString("yyyyMMdd_HHmmmss") + ".fit"
            pTheSkyXImage.Path = pImageSavePath

            If vFrameType = "LIGHT" Then
                pTheSkyXImage.setFITSKeyword("IMAGETYP", "Light Frame")
            ElseIf vFrameType = "BIAS" Then
                pTheSkyXImage.setFITSKeyword("IMAGETYP", "Bias Frame")
            ElseIf vFrameType = "DARK" Then
                pTheSkyXImage.setFITSKeyword("IMAGETYP", "Dark Frame")
            ElseIf vFrameType = "FLAT" Then
                pTheSkyXImage.setFITSKeyword("IMAGETYP", "Flat Frame")
            End If

            'set some extra keywords
            pTheSkyXImage.setFITSKeyword("OBJCTRA", vRA)
            pTheSkyXImage.setFITSKeyword("OBJCTDEC", vDEC)
            pTheSkyXImage.setFITSKeyword("OBJECT", cleanImageName)
            pTheSkyXImage.setFITSKeyword("INSTRUME", My.Settings.sCCDName)          '/ SBIGFITSEXT The model camera used.       
            pTheSkyXImage.setFITSKeyword("COLORCCD", My.Settings.sCCDColorCamera)   '/ Non zero If image Is from a Bayer color ccd    
            pTheSkyXImage.setFITSKeyword("FOCALLEN", My.Settings.sTelescopeFocalLength)
            pTheSkyXImage.setFITSKeyword("TELESCOP", My.Settings.sTelescopeName)
            pTheSkyXImage.setFITSKeyword("APTDIA", My.Settings.sTelescopeAparture)
            pTheSkyXImage.setFITSKeyword("OBSERVER", My.Settings.sObservatoryName)
            pTheSkyXImage.Save()

            ' if run is to abort: exit
            If (pAbort = True And vManual = False) Or pToolsAbort = True Then
                pIsActionRunning = False
                pTheSkyXExposureStatus = ""
                TakeImageTheSkyX = "IMAGE_ABORTED"
                Exit Function
            End If

            If vMessageText <> "plate solve image" Then
                LogSessionEntry("BRIEF", "Exposure completed: " + vImageName + "_" + vFilterName + "_Exp" + Format(vExposureTime) + "s_Bin" + Format(Binning) + "_" + DateTime.UtcNow.ToString("yyyyMMdd_HHmmmss") + ".fit", "", "TakeImageTheSkyX", "TSX")
            Else
                LogSessionEntry("FULL", "Exposure completed: " + vImageName + "_" + vFilterName + "_Exp" + Format(vExposureTime) + "s_Bin" + Format(Binning) + "_" + DateTime.UtcNow.ToString("yyyyMMdd_HHmmmss") + ".fit", "", "TakeImageTheSkyX", "TSX")
            End If
            pTheSkyXExposureStatus = ""

            ' if run is to abort: exit
            If (pAbort = True And vManual = False) Or pToolsAbort = True Then
                pIsActionRunning = False
                pTheSkyXExposureStatus = ""
                TakeImageTheSkyX = "IMAGE_ABORTED"
                Exit Function
            End If


            If vLinkImage = True Then
                pTheSkyXImageLink = New TheSky64Lib.ImageLink
                pTheSkyXImageLinkResults = New TheSky64Lib.ImageLinkResults

                pCurrentFWHM = 0
                pTheSkyXImageLink.pathToFITS = pImageSavePath
                pTheSkyXImageLink.scale = My.Settings.sCCDImageScale * Binning

                Dim TUC_SETSCRPTIMAGELINKUSEALLSKY = 12
                Dim TUC_SETAPRUSEALLSKY = 13

                'pThesky6RASCOMTele.DoCommand(TUC_SETSCRPTIMAGELINKUSEALLSKY, "1")
                'pThesky6RASCOMTele.DoCommand(TUC_SETAPRUSEALLSKY, "1")

                pTheskXRASCOMTele6.DoCommand(TUC_SETSCRPTIMAGELINKUSEALLSKY, "1")
                pTheskXRASCOMTele6.DoCommand(TUC_SETAPRUSEALLSKY, "1")

                If My.Settings.sCCDBlindSolve = True Then
                    pTheSkyXImageLink.unknownScale = True
                Else
                    pTheSkyXImageLink.unknownScale = False
                End If

                ' if run is to abort: exit
                If (pAbort = True And vManual = False) Or pToolsAbort = True Then
                    pIsActionRunning = False
                    pTheSkyXExposureStatus = ""
                    TakeImageTheSkyX = "IMAGE_ABORTED"
                    Exit Function
                End If

                FrmMain.Cursor = Cursors.WaitCursor

                BgwImageLink = New BackgroundWorker With {
                    .WorkerReportsProgress = True,
                    .WorkerSupportsCancellation = True
                }

                If BgwImageLink.IsBusy = False Then
                    LogSessionEntry("BRIEF", "Solving image...", "", "TakeImageTheSkyX", "TSX")
                    BgwImageLink.RunWorkerAsync(1)
                Else
                    LogSessionEntry("BRIEF", "Image Not solved: background worker busy!", "", "TakeImageTheSkyX", "TSX")
                    pTheSkyXExposureStatus = ""
                    TakeImageTheSkyX = "NOK"
                    Exit Function
                End If

                While BgwImageLink.IsBusy = True
                    My.Application.DoEvents()
                End While

                FrmMain.Cursor = Cursors.Default

                ' if run is to abort: exit
                If (pAbort = True And vManual = False) Or pToolsAbort = True Then
                    pIsActionRunning = False
                    pTheSkyXExposureStatus = ""
                    TakeImageTheSkyX = "IMAGE_ABORTED"
                    Exit Function
                End If


                If bgwErrorImageLink <> "OK" Then
                    'error
                    LogSessionEntry("BRIEF", bgwErrorImageLink, "", "TakeImageTheSkyX", "TSX")
                    pTheSkyXExposureStatus = ""
                    TakeImageTheSkyX = "PLATE_SOLVE_ERROR"
                    Exit Function
                Else
                    pCurrentFWHM = pTheSkyXImageLinkResults.imageFWHMInArcSeconds
                    pSolveRA2000 = pTheSkyXImageLinkResults.imageCenterRAJ2000
                    pSolveDEC2000 = pTheSkyXImageLinkResults.imageCenterDecJ2000

                    'set the exact position for use in run deepsky procedure
                    ConvertTargetJ2000ToTopocentric(pSolveRA2000, pSolveDEC2000)
                    pSolveRATopocentric = pRATargetTopocentric
                    pSolveDECTopocentric = pDECTargetTopocentric

                    LogSessionEntry("BRIEF", "Image solved. FWHM: " + Format(pCurrentFWHM, "0.00") + " arcsec. Solved position RA " + pAUtil.HoursToHMS(pSolveRATopocentric, "h ", "m ", "s ") + " - DEC " + pAUtil.DegreesToDMS(pSolveDECTopocentric, "° ", "' ", """ "), "", "TakeImageTheSkyX", "TSX")
                    TakeImageTheSkyX = "OK"
                End If

                pTheSkyXImageLink = Nothing
                pTheSkyXImageLinkResults = Nothing
            End If

            pTheSkyXImage.Close()
            pTheSkyXTakingImage = False
            pIsActionRunning = False

            executionTime = DateTime.UtcNow() - startExecution
            LogSessionEntry("DEBUG", "  TakeImageTheSkyX: " + executionTime.ToString, "", "TakeImageTheSkyX", "TSX")

        Catch ex As Exception
            Select Case ex.HResult
                Case 1655
                    TakeImageTheSkyX = "PLATE_SOLVE_ERROR"
                    pTheSkyXTakingImage = False
                    pTheSkyXExposureStatus = ""
                    pSolveDEC2000 = 0
                    pSolveRA2000 = 0
                    pCurrentFWHM = 0
                    pIsActionRunning = False
                    LogSessionEntry("BRIEF", "Plate solve error: 1655!", "", "TakeImageTheSkyX", "TSX")
                Case 655
                    TakeImageTheSkyX = "PLATE_SOLVE_ERROR"
                    pTheSkyXTakingImage = False
                    pTheSkyXExposureStatus = ""
                    pSolveDEC2000 = 0
                    pSolveRA2000 = 0
                    pCurrentFWHM = 0
                    pIsActionRunning = False
                    LogSessionEntry("BRIEF", "TPlate solve error: 655!", "", "TakeImageTheSkyX", "TSX")
                Case 1212 'image aborted
                    TakeImageTheSkyX = "IMAGE_ABORTED"
                    pTheSkyXTakingImage = False
                    pTheSkyXExposureStatus = ""
                    pSolveDEC2000 = 0
                    pSolveRA2000 = 0
                    pCurrentFWHM = 0
                    pIsActionRunning = False
                    LogSessionEntry("BRIEF", "Image aborted!", "", "TakeImageTheSkyX", "TSX")
                Case Else
                    TakeImageTheSkyX = "Taking image TheSkyX: " + ex.Message
                    pTheSkyXTakingImage = False
                    pTheSkyXExposureStatus = ""
                    pSolveDEC2000 = 0
                    pSolveRA2000 = 0
                    pCurrentFWHM = 0
                    pIsActionRunning = False
                    LogSessionEntry("ERROR", "TakeImageTheSkyX: " + ex.Message, "", "TakeImageTheSkyX", "TSX")
            End Select
        End Try
    End Function

    Private Sub BgwImageLink_DoWork(ByVal sender As Object, ByVal e As DoWorkEventArgs) Handles BgwImageLink.DoWork
        Try
            bgwErrorImageLink = "OK"
            pTheSkyXImageLink.execute()
        Catch ex As Exception
            bgwErrorImageLink = ex.Message
            Exit Sub
        End Try
    End Sub


    Private Sub BgwImageLink_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs) Handles BgwImageLink.RunWorkerCompleted
        ' do nothing
    End Sub

    Public Function AbortImageTheSkyX() As String
        Dim startExecution As Date
        Dim executionTime As TimeSpan

        AbortImageTheSkyX = "OK"
        Try
            startExecution = DateTime.UtcNow()
            LogSessionEntry("DEBUG", "  AbortImageTheSkyX...", "", "AbortImageTheSkyX", "TSX")

            pTheSkyXCamera.Abort()
            pTheSkyXTakingImage = False
            LogSessionEntry("BRIEF", "Image aborted.", "", "AbortImageTheSkyX", "TSX")

            executionTime = DateTime.UtcNow() - startExecution
            LogSessionEntry("DEBUG", "  AbortImageTheSkyX: " + executionTime.ToString, "", "AbortImageTheSkyX", "TSX")

        Catch ex As Exception
            AbortImageTheSkyX = "Aborting image: " + ex.Message
            LogSessionEntry("ERROR", "AbortImageTheSkyX: " + ex.Message, "", "AbortImageTheSkyX", "TSX")
        End Try
    End Function


    '--------------------------------------------------------------------------------
    ' @FOCUS3
    '--------------------------------------------------------------------------------
    Public Function TheSkyXAtFocus3(vFocusMode As String, vNbrFocusSamples As Integer, vExposureTime As Double, vBinning As Integer, vSubFrame As Boolean, vFilterNumber As Integer) As String
        'Dim AtFocusReturnValue As Integer
        Dim returnvalue As String
        Dim startExecution As Date
        Dim executionTime As TimeSpan

        '#define SB_OK                                             0                   //No Error.
        '#define ERR_NOERROR                                       0                   //|No Error.|
        '#define ERR_AF_ERRORFIRST                                 7000                //|@Focus Error.|
        '#define ERR_AF_DIVERGED                                   7001                //|@Focus diverged. |
        '#define ERR_AF_UNDERSAMPLED                               7003                //|Insufficient data To measure focus, increase exposure time. |
        'Error code = 5 (5). No additional information is available. 

        TheSkyXAtFocus3 = "OK"
        Try
            FrmMain.Cursor = Cursors.WaitCursor
            LogSessionEntry("DEBUG", "  TheSkyXAtFocus3...", "", "TheSkyXAtFocus3", "TSX")
            LogSessionEntry("BRIEF", "Running @Focus3... (Position = " + Format(pTheSkyXCamera.focPosition) + ")", "", "TheSkyX@Focus3", "TSX")

            startExecution = DateTime.UtcNow()
            pIsActionRunning = True

            'first check the focusser is at plausible position, if not move to default position
            If My.Settings.sSimulatorMode = False Then
                returnvalue = GetTheSkyXFocusserPosition()
                If returnvalue <> "OK" Then
                    'error
                    LogSessionEntry("ESSENTIAL", "@Focus3: " + returnvalue, "", "TheSkyX@Focus3", "TSX")
                    TheSkyXAtFocus3 = "@Focus3 failed: " + returnvalue
                    Exit Function
                End If

                If pCurrentFocusserPosition >= My.Settings.sCCDFocusDefaultPosition - My.Settings.sCCDFocusDefaultFork And
                        pCurrentFocusserPosition <= My.Settings.sCCDFocusDefaultPosition + My.Settings.sCCDFocusDefaultFork Then
                    'do nothing focus is where to be expected
                Else
                    'move focusser to default position
                    returnvalue = MoveAbsoluteTheSkyXFocusser(Convert.ToInt32(My.Settings.sCCDFocusDefaultPosition))
                    If returnvalue <> "OK" Then
                        'error
                        LogSessionEntry("ESSENTIAL", "@Focus3: " + returnvalue, "", "TheSkyX@Focus3", "TSX")
                        TheSkyXAtFocus3 = "@Focus3 failed: " + returnvalue
                        Exit Function
                    End If
                End If
            End If

            DefineImageDatedPath("FOCUS")
            pTheSkyXCamera.AutoSavePath = pImageDatedPath

            pTheSkyXCamera.ExposureTime = vExposureTime
            pTheSkyXCamera.BinX = vBinning
            pTheSkyXCamera.BinY = vBinning
            'set a subframe if applicable
            If vSubFrame = True And My.Settings.sCCDFocusSubFrame <> 100 Then

                Dim SubframeLeft = Math.Round(((My.Settings.sCCDSensorSizeX / 2) - (((My.Settings.sCCDFocusSubFrame / 100) / 2) * My.Settings.sCCDSensorSizeX)), 0)
                Dim SubframeRight = Math.Round(((My.Settings.sCCDSensorSizeX / 2) + (((My.Settings.sCCDFocusSubFrame / 100) / 2) * My.Settings.sCCDSensorSizeX)), 0)
                Dim SubframeTop = Math.Round(((My.Settings.sCCDSensorSizeY / 2) - (((My.Settings.sCCDFocusSubFrame / 100) / 2) * My.Settings.sCCDSensorSizeY)), 0)
                Dim SubframeBottom = Math.Round(((My.Settings.sCCDSensorSizeY / 2) + (((My.Settings.sCCDFocusSubFrame / 100) / 2) * My.Settings.sCCDSensorSizeY)), 0)

                pTheSkyXCamera.SubframeTop = Convert.ToInt32(SubframeTop)
                pTheSkyXCamera.SubframeLeft = Convert.ToInt32(SubframeLeft)
                pTheSkyXCamera.SubframeRight = Convert.ToInt32(SubframeRight)
                pTheSkyXCamera.SubframeBottom = Convert.ToInt32(SubframeBottom)
                pTheSkyXCamera.Subframe = 1
            Else
                pTheSkyXCamera.Subframe = 0
            End If

            'set the filter used for the image
            pTheSkyXCamera.FilterIndexZeroBased = vFilterNumber
            'set exposure time
            pTheSkyXCamera.FocusExposureTime = vExposureTime

            ' if run is to abort: exit
            If pAbort = True Then
                pIsActionRunning = False
                FrmMain.Cursor = Cursors.Default
                TheSkyXAtFocus3 = "NOK"
                Exit Function
            End If

            'If FocusMode = "Automatic" Then
            ''automatic mode
            'AtFocusReturnValue = pTheSkyXCamera.AtFocus3(NbrFocusSamples, vbTrue)
            'Else
            ''manual mode
            'AtFocusReturnValue = pTheSkyXCamera.AtFocus3(NbrFocusSamples, vbFalse)
            'End If

            'If AtFocusReturnValue = 0 Then
            'LogSessionEntry("BRIEF", "@Focus3 succeeded, position = " + Format(pTheSkyXCamera.focPosition), "TheSkyX@Focus3", "TSX")
            'End If
            If My.Settings.sSimulatorMode = False Then
                BgwAtFocus3 = New BackgroundWorker With {
                    .WorkerReportsProgress = True,
                    .WorkerSupportsCancellation = True
                }

                pNbrFocusSamples = vNbrFocusSamples

                If BgwAtFocus3.IsBusy = False Then
                    LogSessionEntry("BRIEF", "Running @Focus3... (Position = " + Format(pTheSkyXCamera.focPosition) + ")", "", "TakeImageTheSkyX", "TSX")
                    BgwAtFocus3.RunWorkerAsync(1)
                Else
                    LogSessionEntry("BRIEF", "Not focusing: background worker busy!", "TakeImageTheSkyX", "", "TSX")
                    TheSkyXAtFocus3 = "@Focus3 failed: background worker busy!"
                    Exit Function
                End If

                While BgwAtFocus3.IsBusy = True
                    My.Application.DoEvents()
                End While

                FrmMain.Cursor = Cursors.Default

                If bgwErrorAtFocus3 <> "OK" Then
                    'error
                    LogSessionEntry("ESSENTIAL", "@Focus3: " + bgwErrorAtFocus3, "", "TheSkyX@Focus3", "TSX")
                    TheSkyXAtFocus3 = "@Focus3 failed: " + bgwErrorAtFocus3
                    Exit Function
                Else
                    LogSessionEntry("BRIEF", "@Focus3 succeeded. (Position = " + Format(pTheSkyXCamera.focPosition) + ", Temperature: " + Format(pInitialFocusTemperature, "##0.00 °C)"), "", "TheSkyX@Focus3", "TSX")
                End If
            Else
                LogSessionEntry("BRIEF", "@Focus3 succeeded. (Debug position = " + Format(pTheSkyXCamera.focPosition) + ", Temperature: " + Format(pInitialFocusTemperature, "##0.00 °C)"), "", "TheSkyX@Focus3", "TSX")
            End If
            FrmMain.Cursor = Cursors.Default
            pIsActionRunning = False

            executionTime = DateTime.UtcNow() - startExecution
            LogSessionEntry("DEBUG", "TheSkyXAtFocus3" + executionTime.ToString, "", "TheSkyX@Focus3", "TSX")

        Catch ex As Exception
            FrmMain.Cursor = Cursors.Default
            TheSkyXAtFocus3 = "@Focus3 failed: " + ex.Message
            pIsActionRunning = False
            LogSessionEntry("ESSENTIAL", "@Focus3: " + ex.Message, "", "TheSkyX@Focus3", "TSX")
        End Try
    End Function


    Private Sub BgwAtFocus3_DoWork(ByVal sender As Object, ByVal e As DoWorkEventArgs) Handles BgwAtFocus3.DoWork
        Try
            bgwErrorAtFocus3 = "OK"
            pTheSkyXCamera.AtFocus3(pNbrFocusSamples, True)
        Catch ex As Exception
            bgwErrorAtFocus3 = ex.Message
            Exit Sub
        End Try
    End Sub


    Public Function GetFWHMTheSkyX(vImageSavePath As String, vMode As String) As String
        'calculate the FWHM avg or median for all defined subframes
        Dim j As Integer
        Dim totalFWHM As Double

        Dim arrayX() As Integer
        Dim arrayY() As Integer
        Dim arrayFWHM() As Double
        Dim arrayFullSortedFWHM() As Double

        Dim startExecution As Date
        Dim executionTime As TimeSpan

        GetFWHMTheSkyX = "OK"
        Try
            startExecution = DateTime.UtcNow()
            LogSessionEntry("DEBUG", "  GetFWHMTheSkyX...", "", "GetFWHMTheSkyX", "TSX")

            pFWHMFullFrameMedian = 0
            pFWHMFullFrameAverage = 0

            'pTheSkyXImage = New TheSkyXLib.ccdsoftImage
            pTheSkyXImage.Path = vImageSavePath

            pTheSkyXImage.Open()
            pTheSkyXImage.InsertWCS(vbTrue)

            arrayX = CType(pTheSkyXImage.InventoryArray(0), Integer())
            arrayY = CType(pTheSkyXImage.InventoryArray(1), Integer())
            'arrayMagnitude = TheSkyXImage.InventoryArray(2)
            'arrayClass = TheSkyXImage.InventoryArray(3)
            arrayFWHM = CType(pTheSkyXImage.InventoryArray(4), Double())
            'arrayMajorAxis = TheSkyXImage.InventoryArray(5)
            'arrayMinorAxis = TheSkyXImage.InventoryArray(6)
            'arrayTheta = TheSkyXImage.InventoryArray(7)
            'arrayEllipticity = TheSkyXImage.InventoryArray(8)

            Dim arrayFWHMTop(arrayX.Length) As Double
            Dim arrayFWHMMiddle(arrayX.Length) As Double
            Dim arrayFWHMBottom(arrayX.Length) As Double

            Dim i As Integer

            '-----------------------------------
            'FULL FRAME
            '-----------------------------------
            'get fullframe average

            arrayFullSortedFWHM = arrayFWHM
            Array.Sort(arrayFullSortedFWHM)
            pFWHMFullFrameNbrStars = arrayX.Length

            j = 0
            If vMode = "Average" Then
                i = 0
                totalFWHM = 0
                Do While i < arrayX.Length
                    totalFWHM += arrayFWHM(i)
                    i += 1
                Loop
                pFWHMFullFrameAverage = totalFWHM / arrayX.Length
                LogSessionEntry("FULL", "Full Frame: nbr of stars: " + Format(pFWHMFullFrameNbrStars) + " avg: " + Format(pFWHMFullFrameAverage, "#0.00") + " px", "", "GetFWHMTheSkyX", "TSX")
            Else
                'get fullframe median
                j = arrayX.Length Mod 2

                If j = 0 Then
                    pFWHMFullFrameMedian = arrayFullSortedFWHM(Convert.ToInt32((arrayX.Length / 2)))
                ElseIf j = 1 Then
                    pFWHMFullFrameMedian = arrayFullSortedFWHM(Convert.ToInt32((arrayX.Length + 1) / 2))
                End If
                LogSessionEntry("DEBUG", "  Full Frame: nbr of stars: " + Format(pFWHMFullFrameNbrStars) + " median: " + Format(pFWHMFullFrameMedian, "#0.00") + " px", "", "GetFWHMTheSkyX", "TSX")
            End If

            executionTime = DateTime.UtcNow() - startExecution
            LogSessionEntry("DEBUG", "  GetFWHMTheSkyX: " + executionTime.ToString, "", "GetFWHMTheSkyX", "TSX")

        Catch ex As Exception
            GetFWHMTheSkyX = "GetFWHMTheSkyX: " + ex.Message
            LogSessionEntry("ERROR", "GetFWHMTheSkyX: " + ex.Message, "", "GetFWHMTheSkyX", "TSX")
        End Try
    End Function

    '--------------------------------------------------------------------------------
    ' Find object and pass coordinates
    '--------------------------------------------------------------------------------
    Public Function FindTheSkyXTarget(vTarget As String) As String
        FindTheSkyXTarget = "OK"
        Dim startExecution As Date
        Dim executionTime As TimeSpan

        FindTheSkyXTarget = "OK"
        Try
            startExecution = DateTime.UtcNow()
            LogSessionEntry("DEBUG", "  FindTheSkyXTarget...", "", "FindTheSkyXTarget", "TSX")

            pTheSkyXChart = New TheSky64Lib.sky6StarChart
            pTheSkyXObjInfo = New TheSky64Lib.sky6ObjectInformation

            pTheSkyXChart.Find(vTarget)
            pTheSkyXObjInfo.Property(TheSky64Lib.Sk6ObjectInformationProperty.sk6ObjInfoProp_ALT) ' Set object info for “altitude”
            pStrucTargetObject.Altitude = Convert.ToDouble(pTheSkyXObjInfo.ObjInfoPropOut)
            pTheSkyXObjInfo.Property(TheSky64Lib.Sk6ObjectInformationProperty.sk6ObjInfoProp_AZM) ' Set object info for “azimuth”
            pStrucTargetObject.Azimuth = Convert.ToDouble(pTheSkyXObjInfo.ObjInfoPropOut)

            pTheSkyXObjInfo.Property(TheSky64Lib.Sk6ObjectInformationProperty.sk6ObjInfoProp_RA_2000) ' Set object info for “RA”
            pStrucTargetObject.RightAscension = Convert.ToDouble(pTheSkyXObjInfo.ObjInfoPropOut)

            pTheSkyXObjInfo.Property(TheSky64Lib.Sk6ObjectInformationProperty.sk6ObjInfoProp_DEC_2000) ' Set object info for “DEC”
            pStrucTargetObject.Declination = Convert.ToDouble(pTheSkyXObjInfo.ObjInfoPropOut)

            pTheSkyXObjInfo.Property(TheSky64Lib.Sk6ObjectInformationProperty.sk6ObjInfoProp_TRANSIT_TIME) ' Set object transit time
            pStrucTargetObject.TransitTime = Convert.ToDouble(pTheSkyXObjInfo.ObjInfoPropOut)

            pTheSkyXObjInfo.Property(TheSky64Lib.Sk6ObjectInformationProperty.sk6ObjInfoProp_RISE_TIME) ' Set object rise time
            pStrucTargetObject.RiseTime = Convert.ToDouble(pTheSkyXObjInfo.ObjInfoPropOut)

            pTheSkyXObjInfo.Property(TheSky64Lib.Sk6ObjectInformationProperty.sk6ObjInfoProp_SET_TIME) ' Set object set time
            pStrucTargetObject.SetTime = Convert.ToDouble(pTheSkyXObjInfo.ObjInfoPropOut)

            pTheSkyXObjInfo.Property(TheSky64Lib.Sk6ObjectInformationProperty.sk6ObjInfoProp_HA_HOURS) ' Set object hour angle
            pStrucTargetObject.HA = Convert.ToDouble(pTheSkyXObjInfo.ObjInfoPropOut)

            pTheSkyXChart = Nothing
            pTheSkyXObjInfo = Nothing

            executionTime = DateTime.UtcNow() - startExecution
            LogSessionEntry("DEBUG", "  FindTheSkyXTarget: " + executionTime.ToString, "", "FindTheSkyXTarget", "TSX")

        Catch ex As Exception
            If Err.Number = 1250 Then
                FindTheSkyXTarget = "NOTFOUND"
            Else
                FindTheSkyXTarget = "FindTheSkyXTarget: " + ex.Message
                LogSessionEntry("ERROR", "FindTheSkyXTarget: " + ex.Message, "", "FindTheSkyXTarget", "TSX")
            End If

            pTheSkyXChart = Nothing
            pTheSkyXObjInfo = Nothing
        End Try
    End Function

    '--------------------------------------------------------------------------------
    ' Center on coordinates
    '--------------------------------------------------------------------------------
    Public Function FindTheSkyXRADEC(vRA As Double, vDEC As Double) As String
        FindTheSkyXRADEC = "OK"
        Dim startExecution As Date
        Dim executionTime As TimeSpan

        Try
            startExecution = DateTime.UtcNow()
            LogSessionEntry("DEBUG", "  FindTheSkyXRADEC...", "", "FindTheSkyXRADEC", "TSX")

            pTheSkyXChart = New TheSky64Lib.sky6StarChart
            pTheSkyXChart.RightAscension = vRA
            pTheSkyXChart.Declination = vDEC
            pTheSkyXChart = Nothing

            executionTime = DateTime.UtcNow() - startExecution
            LogSessionEntry("DEBUG", "  FindTheSkyXRADEC: " + executionTime.ToString, "", "FindTheSkyXRADEC", "TSX")

        Catch ex As Exception
            FindTheSkyXRADEC = "FindTheSkyXRADEC: " + ex.Message
            LogSessionEntry("ERROR", "FindTheSkyXRADEC: " + ex.Message, "", "FindTheSkyXRADEC", "TSX")
            pTheSkyXChart = Nothing
        End Try
    End Function

    Public Function ClosedLoopSlew(vRATopocentric As Double, vDECTopocentric As Double, vRATopocentric_String As String, vDECTopocentric_String As String, vRA2000_String As String, vDEC2000_String As String, vTargetName As String, vTargetFilter As String, vManual As Boolean, vRATopocentric_StringTSX As String, vDECTopocentric_StringTSX As String) As String
        'slew to target, take image, plate solve, reslew, take image and verify if within tolerance

        Dim returnvalue As String
        Dim i = 0
        Dim ErrorRAsec As Double
        Dim ErrorDECsec As Double
        Dim ErrorRABelowTreshold As Boolean
        Dim ErrorDECBelowTreshold As Boolean
        Dim CorrectionRAString, CorrectionDECString As String
        Dim CorrectionRA, CorrectionDEC As Double
        Dim PlateSolveError As Boolean
        Dim startExecution As Date
        Dim executionTime As TimeSpan
        Dim SolveErrorRA, SolveErrorDEC As Double

        ClosedLoopSlew = "OK"
        Try
            startExecution = DateTime.UtcNow()
            LogSessionEntry("DEBUG", "  ClosedLoopSlew...", "", "ClosedLoopSlew", "TSX")

            pIsActionRunning = True
            LogSessionEntry("ESSENTIAL", "Closed loop slew to " + vTargetName + " - Moon Alt " + Format(pStructEventTimes.MoonAlt, "#0.00") + "°", "", "ClosedLoopSlew", "TSX")
            ' if run is to abort: exit
            If (pAbort = True And vManual = False) Or pToolsAbort = True Then
                LogSessionEntry("BRIEF", "Closed loop slew to " + vTargetName + " aborted!", "", "ClosedLoopSlew", "TSX")
                pIsActionRunning = False
                ClosedLoopSlew = "SLEW_ABORTED"
                Exit Function
            End If


            pClosedLoopSlew = "SLEW"

            '-------------------------------------------------------------------
            'connect CCD equipment if not connected
            '-------------------------------------------------------------------
            If pTheSkyXEquipmentConnected = False Then
                returnvalue = ConnectTheSkyXEquipment(True)
                If returnvalue <> "OK" Then
                    LogSessionEntry("BRIEF", "Closed loop slew to " + vTargetName + " aborted!", "", "ClosedLoopSlew", "TSX")
                    pIsActionRunning = False
                    ClosedLoopSlew = "NOK"
                    Exit Function
                End If
            End If

            ErrorRABelowTreshold = False
            ErrorDECBelowTreshold = False
            PlateSolveError = False
            SolveErrorRA = 0
            SolveErrorDEC = 0
            pSolveRATopocentric = 0
            pSolveDECTopocentric = 0

            If (pAbort = True And vManual = False) Or pToolsAbort = True Then
                LogSessionEntry("BRIEF", "Closed loop slew to " + vTargetName + " aborted!", "", "ClosedLoopSlew", "TSX")
                pIsActionRunning = False
                ClosedLoopSlew = "SLEW_ABORTED"
                Exit Function
            End If

            While i < My.Settings.sCCDPlateSolveNbrTries And Not (ErrorRABelowTreshold = True And ErrorDECBelowTreshold = True) And pClosedLoopSlew <> "ABORT"
                'do events, get last mount coordinates
                My.Application.DoEvents()
                ' if run is to abort: exit
                If (pAbort = True And vManual = False) Or pToolsAbort = True Then
                    LogSessionEntry("BRIEF", "Closed loop slew to " + vTargetName + " aborted!", "", "ClosedLoopSlew", "TSX")
                    pIsActionRunning = False
                    ClosedLoopSlew = "SLEW_ABORTED"
                    Exit Function
                End If

                '-------------------------------------------------------------------
                'DO A SLEW AFTER MODIFYING COORDINATES FOR ERROR
                '-------------------------------------------------------------------
                'calculate pointing error in arcseconds, first time no error
                If i > 0 Then
                    SolveErrorRA = vRATopocentric - pSolveRATopocentric
                    SolveErrorDEC = vDECTopocentric - pSolveDECTopocentric
                    'calculate offset coordinates
                    CorrectionRA = pStructMount.RightAscension + SolveErrorRA 'corrected RA
                    CorrectionDEC = pStructMount.Declination + SolveErrorDEC 'corrected DEC
                Else
                    SolveErrorRA = 0
                    SolveErrorDEC = 0
                    CorrectionRA = vRATopocentric 'corrected RA
                    CorrectionDEC = vDECTopocentric ''corrected DEC
                End If

                CorrectionRAString = pAUtil.HoursToHMS(CorrectionRA, "h ", "m ", "s ")
                CorrectionDECString = pAUtil.DegreesToDMS(CorrectionDEC, "° ", "' ", """ ")

                'convert error in arcsec
                ErrorRAsec = Math.Abs(SolveErrorRA * 54000)
                ErrorDECsec = Math.Abs(SolveErrorDEC * 3600)

                'check of pointing error is below treshold, except first slew
                'RA error treshold
                If ErrorRAsec < My.Settings.sCCDPlateSolveMaxError And i > 0 Then
                    ErrorRABelowTreshold = True
                Else
                    ErrorRABelowTreshold = False
                End If
                'DEC error treshold
                If ErrorDECsec < My.Settings.sCCDPlateSolveMaxError And i > 0 Then
                    ErrorDECBelowTreshold = True
                Else
                    ErrorDECBelowTreshold = False
                End If

                ' if run is to abort: exit
                If (pAbort = True And vManual = False) Or pToolsAbort = True Then
                    LogSessionEntry("BRIEF", "Closed loop slew to " + vTargetName + " aborted!", "", "ClosedLoopSlew", "TSX")
                    pIsActionRunning = False
                    ClosedLoopSlew = "SLEW_ABORTED"
                    Exit Function
                End If

                'DO SLEW IF ERROR IS STILL TOO LARGE
                If pMountConnected = True And (ErrorRABelowTreshold = False Or ErrorDECBelowTreshold = False) And pClosedLoopSlew <> "ABORT" Then
                    returnvalue = MountSlewToTarget(vTargetName, CorrectionRA, CorrectionDEC, CorrectionRAString, CorrectionDECString, CorrectionRAString, CorrectionDECString, True, vManual)
                    If returnvalue <> "OK" Then
                        pIsActionRunning = False
                        ClosedLoopSlew = returnvalue
                        Exit Function
                    End If
                End If

                ' if run is to abort: exit
                If (pAbort = True And vManual = False) Or pToolsAbort = True Then
                    LogSessionEntry("BRIEF", "Closed loop slew to " + vTargetName + " aborted!", "", "ClosedLoopSlew", "TSX")
                    pIsActionRunning = False
                    ClosedLoopSlew = "SLEW_ABORTED"
                    Exit Function
                End If

                If pClosedLoopSlew <> "ABORT" Or pAbort = False Then
                    'ONLY IN SIMULATOR MODE: wait for 5 seconds so the software can keep up
                    If My.Settings.sSimulatorMode = True Then
                        LogSessionEntry("DEBUG", "  Debug: sleeping 5 sec...", "", "ClosedLoopSlew", "TSX")
                        Dim x = 0

                        Do While x < 50
                            Thread.Sleep(100)
                            My.Application.DoEvents()
                            x += 1
                        Loop

                        ' if run is to abort: exit
                        If (pAbort = True And vManual = False) Or pToolsAbort = True Then
                            LogSessionEntry("BRIEF", "Closed loop slew to " + vTargetName + " aborted!", "", "ClosedLoopSlew", "TSX")
                            pIsActionRunning = False
                            ClosedLoopSlew = "SLEW_ABORTED"
                            Exit Function
                        End If

                    End If

                    '-------------------------------------------------------------------
                    'TAKE IMAGE TO CALCULATED NEW ERROR
                    '-------------------------------------------------------------------
                    Dim FilterNumber As Integer
                    Dim binning As Integer

                    'retrieve filternumber
                    If vTargetFilter = My.Settings.sCCDFilter1 Then
                        FilterNumber = 0
                    ElseIf vTargetFilter = My.Settings.sCCDFilter2 Then
                        FilterNumber = 1
                    ElseIf vTargetFilter = My.Settings.sCCDFilter3 Then
                        FilterNumber = 2
                    ElseIf vTargetFilter = My.Settings.sCCDFilter4 Then
                        FilterNumber = 3
                    ElseIf vTargetFilter = My.Settings.sCCDFilter5 Then
                        FilterNumber = 4
                    End If

                    If My.Settings.sCCDPlateSolveBinning = "1x1" Then
                        binning = 1
                    ElseIf My.Settings.sCCDPlateSolveBinning = "2x2" Then
                        binning = 2
                    ElseIf My.Settings.sCCDPlateSolveBinning = "3x3" Then
                        binning = 3
                    End If

                    My.Application.DoEvents()

                    ' if run is to abort: exit
                    If (pAbort = True And vManual = False) Or pToolsAbort = True Then
                        LogSessionEntry("BRIEF", "Closed loop slew to " + vTargetName + " aborted!", "", "ClosedLoopSlew", "TSX")
                        pIsActionRunning = False
                        ClosedLoopSlew = "SLEW_ABORTED"
                        Exit Function
                    End If

                    'TAKE IMAGE AND SOLVE THE IMAGE
                    returnvalue = TakeImageTheSkyX(My.Settings.sCCDPlateSolveExposure, binning, "PLATESOLVE", False, True, FilterNumber, vTargetFilter, "plate solve image", vManual, "LIGHT", vRATopocentric_StringTSX, vDECTopocentric_StringTSX)

                    My.Application.DoEvents()
                    ' if run is to abort: exit
                    If (pAbort = True And vManual = False) Or pToolsAbort = True Then
                        LogSessionEntry("BRIEF", "Closed loop slew to " + vTargetName + " aborted!", "", "ClosedLoopSlew", "TSX")
                        pIsActionRunning = False
                        ClosedLoopSlew = "SLEW_ABORTED"
                        Exit Function
                    End If

                    If returnvalue = "PLATE_SOLVE_ERROR" Or returnvalue = "IMAGE_ABORTED" Then
                        pSolveRATopocentric = vRATopocentric
                        pSolveDECTopocentric = vDECTopocentric
                        PlateSolveError = True
                        ErrorRABelowTreshold = False
                        ErrorDECBelowTreshold = False
                    Else
                        PlateSolveError = False
                        SolveErrorRA = vRATopocentric - pSolveRATopocentric
                        SolveErrorDEC = vDECTopocentric - pSolveDECTopocentric
                        CorrectionRAString = pAUtil.HoursToHMS(CorrectionRA, "h ", "m ", "s ")
                        CorrectionDECString = pAUtil.DegreesToDMS(CorrectionDEC, "° ", "' ", """ ")

                        'convert error in arcsec
                        ErrorRAsec = Math.Abs(SolveErrorRA * 54000)
                        ErrorDECsec = Math.Abs(SolveErrorDEC * 3600)

                        'check of pointing error is below treshold, except first slew
                        If ErrorRAsec < My.Settings.sCCDPlateSolveMaxError Then
                            ErrorRABelowTreshold = True
                        Else
                            ErrorRABelowTreshold = False
                        End If

                        If ErrorDECsec < My.Settings.sCCDPlateSolveMaxError Then
                            ErrorDECBelowTreshold = True
                        Else
                            ErrorDECBelowTreshold = False
                        End If

                        LogSessionEntry("BRIEF", "Correcting error: RA " + Format(ErrorRAsec, "0") + " arcsec - DEC " + Format(ErrorDECsec, "0") + " arcsec.", "", "ClosedLoopSlew", "TSX")
                    End If

                    i += 1
                End If

                ' if run is to abort: exit
                If (pAbort = True And vManual = False) Or pToolsAbort = True Then
                    pIsActionRunning = False
                    LogSessionEntry("BRIEF", "Closed loop slew to " + vTargetName + " aborted!", "", "ClosedLoopSlew", "TSX")
                    ClosedLoopSlew = "SLEW_ABORTED"
                    Exit Function
                End If
            End While

            'only for manual slews
            If pClosedLoopSlew = "ABORT" Then
                LogSessionEntry("BRIEF", "Closed loop slew to " + vTargetName + " aborted!", "", "ClosedLoopSlew", "TSX")
                ClosedLoopSlew = "NOK"
            Else
                If (ErrorRABelowTreshold = False Or ErrorDECBelowTreshold = False) Or PlateSolveError = True Then
                    LogSessionEntry("ESSENTIAL", "Closed loop slew to " + vTargetName + " failed!", "", "ClosedLoopSlew", "TSX")
                    ClosedLoopSlew = "NOK"
                Else
                    'if slew succeeded, calculate the total offset to the original position for use in mosaic
                    pTotalSolveErrorRA = vRATopocentric - pStructMount.RightAscension
                    pTotalSolveErrorDEC = vDECTopocentric - pStructMount.Declination
                    'set the actual mount position needed to calculate any offset
                    pRAMountActualTopocentric = pStructMount.RightAscension
                    pDECMountActualTopocentric = pStructMount.Declination

                    LogSessionEntry("ESSENTIAL", "Closed loop slew to " + vTargetName + " completed! ° - Moon Alt " + Format(pStructEventTimes.MoonAlt, "#0.00") + "°", "", "ClosedLoopSlew", "TSX")
                    ClosedLoopSlew = "OK"
                End If
            End If
            pClosedLoopSlew = ""

            executionTime = DateTime.UtcNow() - startExecution
            LogSessionEntry("DEBUG", "  ClosedLoopSlew: " + executionTime.ToString, "", "ClosedLoopSlew", "TSX")

        Catch ex As Exception
            LogSessionEntry("ERROR", "ClosedLoopSlew: " + Err.ToString(), "", "ClosedLoopSlew", "TSX")
            ClosedLoopSlew = "NOK"
            pIsActionRunning = False
            pClosedLoopSlew = ""
        End Try
    End Function

    Public Function CheckTheSkyXCCD() As String
        Dim returnvalue As String
        Dim startExecution As Date
        Dim executionTime As TimeSpan

        CheckTheSkyXCCD = "OK"
        Try
            startExecution = DateTime.UtcNow()
            LogSessionEntry("DEBUG", "  CheckTheSkyXCCD...", "", "CheckTheSkyXCCD", "TSX")

            If pTheSkyXTakingImage = True Then
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

                FrmMain.StatusStrip.Items(0).Text = ExposureStatus  'pTheSkyXExposureStatus 'Exposing Light ( x left )
                FrmMain.LblCCDExposureStatus.Text = ExposureStatus
            End If

            pTheSkyXCCDTemp = pTheSkyXCamera.Temperature

            'check focusser / CCCD
            If pTheSkyXEquipmentConnected = True Then
                returnvalue = GetTheSkyXFocusserPosition()
                If returnvalue <> "OK" Then
                    CheckTheSkyXCCD = returnvalue
                    Exit Function
                End If

                FrmMain.LblFocusserPosition.Text = "Position: " + Format(pCurrentFocusserPosition)
                FrmMain.LblFocusserTemperature.Text = "Temp: " + Format(pCurrentFocusserTemperature, "##0.00") + " °C"
            Else
                FrmMain.LblFocusserPosition.Text = ""
                FrmMain.LblFocusserTemperature.Text = ""
            End If

            executionTime = DateTime.UtcNow() - startExecution
            LogSessionEntry("DEBUG", "  CheckTheSkyXCCD: " + executionTime.ToString, "", "CheckTheSkyXCCD", "TSX")

        Catch ex As Exception
            CheckTheSkyXCCD = "CheckTheSkyXCCD: " + ex.Message
            LogSessionEntry("ERROR", "CheckTheSkyXCCD: " + ex.Message, "", "CheckTheSkyXCCD", "TSX")
        End Try
    End Function


    Public Function RunCalibrationFrames(vCalibrationType As String, vNumberofExposures As Integer, vExposureTime As Double, vBinning As Integer, vFilterNumber As Integer, vFilterName As String, vMessageText As String) As String
        Dim startExecution As Date
        Dim executionTime As TimeSpan
        Dim i As Integer
        Dim returnvalue As String

        RunCalibrationFrames = "OK"
        Try
            startExecution = DateTime.UtcNow()
            LogSessionEntry("DEBUG", "  RunCalibrationFrames...", "", "RunCalibrationFrames", "TSX")

            i = 0

            Do While i < vNumberofExposures And pContinueRunningCalibrationFrames = True
                returnvalue = TakeImageTheSkyX(vExposureTime, vBinning, vCalibrationType, False, False, vFilterNumber, vFilterName, vMessageText + " (" + Format(i + 1) + "/" + Format(vNumberofExposures) + ")", False, vCalibrationType, "", "")
                If returnvalue <> "OK" And returnvalue <> "IMAGE_ABORTED" Then
                    pIsSequenceRunning = False
                    RunCalibrationFrames = returnvalue
                    LogSessionEntry("ERROR", "Image failed!", "", "RunDeepsky", "TSX")
                End If

                i += 1
            Loop

            executionTime = DateTime.UtcNow() - startExecution
            LogSessionEntry("DEBUG", "  RunCalibrationFrames: " + executionTime.ToString, "", "RunCalibrationFrames", "TSX")

        Catch ex As Exception
            RunCalibrationFrames = "RunCalibrationFrames: " + ex.Message
            LogSessionEntry("ERROR", "RunCalibrationFrames: " + ex.Message, "", "RunCalibrationFrames", "TSX")
        End Try
    End Function


    Public Function CalculateAverageImage(vImagePath As String) As String
        Dim startExecution As Date
        Dim executionTime As TimeSpan
        Dim i As Integer
        Dim TheSkyXImage As New TheSky64Lib.ccdsoftImage

        CalculateAverageImage = "OK"
        Try
            startExecution = DateTime.UtcNow()
            LogSessionEntry("DEBUG", "  CalculateAverageImage...", "", "CalculateAverageImage", "TSX")

            i = 0
            TheSkyXImage.Path = vImagePath
            TheSkyXImage.Open()
            pTheSkyXImageAverageValue = TheSkyXImage.averagePixelValue()
            TheSkyXImage.Close()

            executionTime = DateTime.UtcNow() - startExecution
            LogSessionEntry("DEBUG", "  RunCalibrationFrames: " + executionTime.ToString, "", "CalculateAverageImage", "TSX")

        Catch ex As Exception
            CalculateAverageImage = "CalculateAverageImage: " + ex.Message
            LogSessionEntry("ERROR", "CalculateAverageImage: " + ex.Message, "", "CalculateAverageImage", "TSX")
        End Try
    End Function


    '----------------------------------------
    ' TAKE AND SAVE FLAT IMAGE
    '----------------------------------------
    Public Function TakeFlatTheSkyX(vExposureTime As Double, vBinning As Integer, vImageName As String, vFilterNumber As Integer, vFilterName As String) As String
        Dim startExecution As Date
        Dim executionTime As TimeSpan
        Dim returnvalue As String

        TakeFlatTheSkyX = "OK"
        Try
            startExecution = DateTime.UtcNow()
            LogSessionEntry("DEBUG", "  TakeFlatTheSkyX...", "", "TakeFlatTheSkyX", "TSX")

            pIsActionRunning = True

            ' if run is to abort: exit
            If pAbort = True Then
                pIsActionRunning = False
                pTheSkyXExposureStatus = ""
                TakeFlatTheSkyX = "IMAGE_ABORTED"
                Exit Function
            End If

            'check if there is already an image exposing
            Try
                If pTheSkyXCamera.IsExposureComplete = 0 Then
                    'abort the current image
                    pTheSkyXCamera.Abort()
                End If
            Catch
            End Try

            DefineImageDatedPath("NORMAL")

            LogSessionEntry("BRIEF", "Starting " + Format(vExposureTime, "#0.00") + "s " + " flat exposure...", "", "TakeFlatTheSkyX", "TSX")
            pTheSkyXCamera.ExposureTime = vExposureTime
            pTheSkyXCamera.BinX = vBinning
            pTheSkyXCamera.BinY = vBinning

            'no subframe if applicable
            pTheSkyXCamera.Subframe = 0

            'set the filter used for the image
            pTheSkyXCamera.FilterIndexZeroBased = vFilterNumber
            pTheSkyXCamera.Asynchronous = 1

            'first do events so no double commands are given to TSX
            My.Application.DoEvents()

            ' if run is to abort: exit
            If pAbort = True Then
                pIsActionRunning = False
                pTheSkyXExposureStatus = ""
                TakeFlatTheSkyX = "IMAGE_ABORTED"
                Exit Function
            End If

            pTheSkyXCamera.TakeImage()
            pTheSkyXTakingImage = True

            Do While pTheSkyXTakingImage = True
                Thread.Sleep(250)
                My.Application.DoEvents()

                pTheSkyXExposureStatus = pTheSkyXCamera.ExposureStatus

                If pTheSkyXCamera.IsExposureComplete = 1 Then
                    pTheSkyXTakingImage = False
                End If

                ' if run is to abort: exit
                If pAbort = True Then
                    pTheSkyXCamera.Abort()
                    pTheSkyXExposureStatus = ""
                    pIsActionRunning = False
                    TakeFlatTheSkyX = "IMAGE_ABORTED"
                    Exit Function
                End If

                If startExecution.AddSeconds(vExposureTime + My.Settings.sCCDTimeout) < DateTime.UtcNow() Then
                    LogSessionEntry("ERROR", "Take image The Sky X timeout!", "", "TakeFlatTheSkyX", "TSX")
                    returnvalue = PauseRun("ERROR: Take image The Sky X timeout: PAUSING EQUIPMENT...", "",
                                           "ERROR: Take image The Sky X timeout: PauseEquipment", "",
                                           "ERROR: equipment paused.", "", "PAUSING", "WAITING")
                    TakeFlatTheSkyX = "IMAGE_ABORTED"
                    Exit Function
                End If
            Loop

            ' if run is to abort: exit
            If pAbort = True Then
                pIsActionRunning = False
                pTheSkyXExposureStatus = ""
                TakeFlatTheSkyX = "IMAGE_ABORTED"
                Exit Function
            End If

            Dim ImageSavePath As String
            Dim TheSkyXImage = New TheSky64Lib.ccdsoftImage
            TheSkyXImage.AttachToActiveImager()


            'calculate average
            pTheSkyXImageAverageValue = TheSkyXImage.averagePixelValue()

            If pTheSkyXImageAverageValue >= My.Settings.sAutoFlatMinADU And pTheSkyXImageAverageValue <= My.Settings.sAutoFlatMaxADU And
                    vExposureTime >= My.Settings.sAutoFlatMinExp And vExposureTime <= My.Settings.sAutoFlatMaxExp Then
                'image is between the ADU ane exposure treshold and should be saved
                'check and create folder
                If Directory.Exists(pImageDatedPath) = False Then
                    Directory.CreateDirectory(pImageDatedPath)
                End If

                ImageSavePath = pImageDatedPath + vImageName + "_" + vFilterName + "_Bin" + Format(vBinning) + "_" + DateTime.UtcNow.ToString("yyyyMMdd_HHmmmss") + ".fit"
                TheSkyXImage.Path = ImageSavePath
                TheSkyXImage.setFITSKeyword("IMAGETYP", "Flat Frame")
                TheSkyXImage.Save()
                LogSessionEntry("BRIEF", "Flat saved. ADU= " + Format(pTheSkyXImageAverageValue, "0#"), " Sun Alt " + Format(pStructEventTimes.SunAlt, "#0.00"), "TakeFlatTheSkyX", "TSX")
            Else
                'do not save image
                LogSessionEntry("BRIEF", "Flat not saved. ADU= " + Format(pTheSkyXImageAverageValue, "0#"), " Sun Alt " + Format(pStructEventTimes.SunAlt, "#0.00"), "TakeFlatTheSkyX", "TSX")
            End If


            pTheSkyXExposureStatus = ""

            TheSkyXImage.Close()
            pTheSkyXTakingImage = False
            pIsActionRunning = False

            executionTime = DateTime.UtcNow() - startExecution
            LogSessionEntry("DEBUG", "  TakeFlatTheSkyX: " + executionTime.ToString, "", "TakeFlatTheSkyX", "TSX")

        Catch ex As Exception
            Select Case ex.HResult
                Case 1655
                    TakeFlatTheSkyX = "PLATE_SOLVE_ERROR"
                    pTheSkyXTakingImage = False
                    pTheSkyXExposureStatus = ""
                    pIsActionRunning = False
                    LogSessionEntry("BRIEF", "Plate solve error: 1655!", "", "TakeFlatTheSkyX", "TSX")
                Case 655
                    TakeFlatTheSkyX = "PLATE_SOLVE_ERROR"
                    pTheSkyXTakingImage = False
                    pTheSkyXExposureStatus = ""
                    pIsActionRunning = False
                    LogSessionEntry("BRIEF", "TPlate solve error: 655!", "", "TakeFlatTheSkyX", "TSX")
                Case 1212 'image aborted
                    TakeFlatTheSkyX = "IMAGE_ABORTED"
                    pTheSkyXTakingImage = False
                    pTheSkyXExposureStatus = ""
                    pIsActionRunning = False
                    LogSessionEntry("BRIEF", "Image aborted!", "", "TakeFlatTheSkyX", "TSX")
                Case Else
                    TakeFlatTheSkyX = "Taking image TheSkyX: " + ex.Message
                    pTheSkyXTakingImage = False
                    pTheSkyXExposureStatus = ""
                    pIsActionRunning = False
                    LogSessionEntry("ERROR", "TakeFlatTheSkyX: " + ex.Message, "", "TakeFlatTheSkyX", "TSX")
            End Select
        End Try
    End Function



    '    Public Sub IdentifyErrorTheSkyX(ErrorCode As Integer, ErrorMessage As String)
    '    'example: ex.Message = "The astrometric solution failed. Error = 655."
    '
    '    Dim temp As String
    '    Dim Errcode As Integer
    '    Dim ErrMessage As String
    '
    '
    '    Try
    '            'get the errorcode
    '            temp = ErrorMessage.Substring(ErrorMessage.IndexOf("Error = ") + 7)
    '            Errcode = temp.Substring(0, temp.Length - 1)
    '            ErrMessage = ErrorMessage.Substring(0, ErrorMessage.IndexOf("Error = "))
    '
    '            pTheSkyXErrCode = Errcode
    '            pTheSkyXErrMessage = ErrMessage
    '
    '    Catch ex
    '            LogSessionEntry("ERROR", "IdentifyErrorTheSkyX: " + ex.Message, "IdentifyErrorTheSkyX", "TSX")
    '    End Try
    '    End Sub




    'X2 Standard  Version 1.23
    '
    '    Main Page
    '    Related Pages
    '    Modules
    '    Classes
    '    Files
    '
    '    licensedinterfaces
    '
    'sberrorx.h
    ' //Copyright Software Bisque 2017
    '
    '#ifndef SBERRORX_H
    '#define SBERRORX_H
    '#define SB_OK                                             0                   //No Error.
    '#define ERR_NOERROR                                       0                   //|No Error.|
    '#define ERR_COMMNOLINK                                    200                 //|The operation failed because there Is no connection To the device.|
    '#define ERR_COMMOPENING                                   201                 //|Could Not open communications port.  The port Is either In use by another application Or Not recognized by the system.|
    '#define ERR_COMMSETTINGS                                  202                 //|The communications port could Not support the specified settings.|
    '#define ERR_NORESPONSE                                    203                 //|No response from the device.|
    '#define ERR_MEMORY                                        205                 //|Error: memory Error.|
    '#define ERR_CMDFAILED                                     206                 //|Error: command failed.|
    '#define ERR_DATAOUT                                       207                 //|Transmit time-out.|
    '#define ERR_TXTIMEOUT                                     208                 //|Transmission time-out.|
    '#define ERR_RXTIMEOUT                                     209                 //|Receive time-out.|
    '#define ERR_POSTMESSAGE                                   210                 //|Post message failed.|
    '#define ERR_POINTER                                       211                 //|Pointer Error.|
    '#define ERR_ABORTEDPROCESS                                212                 //|Process aborted.|
    '#define ERR_AUTOTERMINATE                                 213                 //|Error, poor communication, connection automatically terminated.|
    '#define ERR_INTERNETSETTINGS                              214                 //|Error, cannot connect To host.|
    '#define ERR_NOLINK                                        215                 //|No connection To the device.|
    '#define ERR_DEVICEPARKED                                  216                 //|Error, the device Is parked And must be unparked Using the Unpark command before proceeding.|
    '#define ERR_DRIVERNOTFOUND                                217                 //|A necessary driver was Not found.|
    '#define ERR_LIMITSEXCEEDED                                218                 //|Limits exceeded.|
    '#define ERR_COMMANDINPROGRESS                             219                 //|Command In progress.|
    '#define ERR_CMD_IN_PROGRESS_MODELESSDLG                   110                 //|A Window command Is already In progress.|
    '#define ERR_CMD_IN_PROGRESS_DBQRY                         111                 //|A Database Query command Is already In progress.|
    '#define ERR_CMD_IN_PROGRESS_IMGSYS                        112                 //|An Imaging System command Is already In progress.|
    '#define ERR_CMD_IN_PROGRESS_FW                            113                 //|A Filter Wheel command Is already Is In progress.|
    '#define ERR_CMD_IN_PROGRESS_SATTRACK                      114                 //|A Satellite Tracking command Is already Is In progress.|
    '#define ERR_CMD_IN_PROGRESS_CAL_RUN                       115                 //|A TPoint calibration run Is already In progress.|
    '#define ERR_CMD_IN_PROGRESS_THEATER                       116                 //|A Theater Suite Command In already In progress.|
    '#define ERR_CMD_IN_PROGRESS_FOC                           117                 //|A Focuser command Is already Is In progress.|
    '#define ERR_CMD_IN_PROGRESS_OTA                           118                 //|An OTA command Is already In progress.|
    '#define ERR_CMD_IN_PROGRESS_APR                           119                 //|An Automated Pointing Calibration run Is already In progress.|
    '#define ERR_CMD_IN_PROGRESS_CAM                           120                 //|A Camera command Is already In progress.|
    '#define ERR_CMD_IN_PROGRESS_MNT                           121                 //|A Mount command Is already In progress.|
    '#define ERR_CMD_IN_PROGRESS_CLS                           122                 //|A Closed Loop Slew command already In progress.|
    '#define ERR_CMD_IN_PROGRESS_DOME                          123                 //|A Dome command Is already In progress.|
    '#define ERR_CMD_IN_PROGRESS_ROT                           124                 //|A Rotator command Is already In progress.|
    '#define ERR_WAITING_CAM_CMD                               130                 //|Error waiting On camera command To complete.|
    '#define ERR_UNEXPECTED_CALLING_THREAD                     131                 //|Unexpected Error.|
    '#define ERR_WAITING_FOR_CAM_TEMP                          132                 //|Error waiting On camera temperature To complete.|
    '#define ERR_EXITING_CAM                                   133                 //|Error deleting camera.|
    '#define ERR_MISSED_CLOCK_SYNC_TIME                        134                 //|Error missed syncing exposure start On unix time stamp.|
    '#define ERR_FOCUSERNOTHOMED                               140                 //|The operation failed because the focuser Is Not yet homed.|
    '#define ERR_ROTATORNOTHOMED                               141                 //|The operation failed because the rotator Is Not yet homed.|
    '#define ERR_DEVICENOTSUPPORTED                            220                 //|Device Not supported.|
    '#define ERR_NOTPOINT                                      221                 //|TPoint model Not available.|
    '#define ERR_MOUNTNOTSYNCED                                222                 //|The operation failed because the mount Not yet been synchronized To a known star.|
    '#define ERR_USERASCLIENT                                  223                 //|You must use the RASClient application To connect To a remote observatory.|
    '#define ERR_THESKYNOTRUNNING                              224                 //|The attempted operation requires the TheSky5 Level IV Or TheSky6/TheSkyX Professional Edition And it must be running.|
    '#define ERR_NODEVICESELECTED                              225                 //|No device has been selected.|
    '#define ERR_CANTLAUNCHTHESKY                              226                 //|Unable To launch TheSky.|
    '#define ERR_NOTINITIALIZED                                227                 //|Telescope Not initialized. The telescope must be initialized In order To perform this operation.|
    '#define ERR_COMMANDNOTSUPPORTED                           228                 //|This command Is Not supported by the selected device.|
    '#define ERR_LX200DESTBELOWHORIZ                           229                 //|The Slew command failed because the LX200/Autostar reports that the destination coordinates are below the horizon.|
    '#define ERR_LX200OUTSIDELIMIT                             230                 //|The Slew command failed because the LX200/Autostar reports that slewing To the destination coordinates Is Not possible. Was the telescope synchronized?|
    '#define ERR_MOUNTNOTHOMED                                 231                 //|The operation failed because the mount Is Not yet homed.|
    '#define ERR_TPOINT_NO_MORE_SAMPLES                        232                 //|TPoint Not accepting additional pointing samples.|
    '#define ERR_JOYSTICKING                                   233                 //|The operation failed because the joystick Is being activated Or the device Is under manual control.|
    '#define ERR_NOPARKPOSITION                                234                 //|Error, no park position has been Set.|
    '#define ERR_BADPOINTINGSAMPLE                             235                 //|The pointing sample was rejected because it Is too far out To be valid.This Error Is typically the result Of an exceedingly poor polar alignment Or an improperly initialized mount, For example an incorrect star synchronization.To avoid this Error, Double check your polar alignment With the 'Rough Polar Alignment' (Paramount's only) and or telescope initialization.|
    '#define ERR_DSSRXTIMEOUT                                  236                 //|Time-out downloading DSS photo.|
    '#define ERR_BADSYNCINTOMODEL                              237                 //|The 'Sync mount into the existing model' is rejected because it is too far out.Double check polar alignment, date and time and location.|
    '#define ERR_MOUNT1NOTPARKED                               238                 //|The mount Is Not parked.|
    '#define ERR_MOUNT2NOTPARKED                               239                 //|The mount number 2 Is Not parked.|
    '#define ERR_MOUNT3NOTPARKED                               240                 //|The mount number 3 Is Not parked.|
    '#define FLASH_REPROGRAMMED                                3015                //|Turn off power, move dip switches To off position, Then turn power On And reconnect.|
    '#define FLASH_NEEDSREPROGRAMMED                           3016                //|Firmware needs re-programmed.  This will reset all limit minimum And maximum values.|
    '#define FIRMWARE_NOT_SUPPORTED                            3017                //|Firmware version Is Not supported.|
    '#define FLASH_IN_PROGRAM_MODE                             3018                //|The mount firmware In Is program mode And cannot be communicated With.Please turn the mount off.Wait at least 5 seconds.Then turn it back On To proceed.|
    '#define FLASH_NOT_IN_PROGRAM_MODE                         3019                //|The mount firmware Is Not In the correct state To be re-programmed.|
    '#define ERR_OBJECTNOTFOUND                                250                 //|Object Not found.|
    '#define ERR_OBJECTTOOLOW                                  251                 //|Object too low.|
    '#define ERR_MISSING_NIGHTVISIONMODE_THEME                 252                 //|Setting Night Vision Mode failed.On Windows, make sure the required file 'TheSkyX Night Vision Mode.Theme' is available to the Windows Display Properties dialog.|
    '#define ERR_DISPLAY_PROPS_ALREADY_RUNNING                 253                 //|The Windows Display Properties dialog Is open.  Please close it And Try again.|
    '#define ERR_THEME_NOT_SAVED                               254                 //|Night Vision cannot be invoked because the current theme has been modified without being saved. Please save the current theme by clicking Start, Control Panel, Display, And from the Themes tab, click Save As.|
    '#define ERR_NOOBJECTSELECTED                              255                 //|The command failed because there Is no target.  Find Or click On a target.|
    '#define ERR_BADDOMEGEOMETRY                               256                 //|Invalid dome geometry.|
    '#define ERR_BADPACKET                                     300                 //|Bad packet.|
    '#define ERR_BADCHECKSUM                                   301                 //|Bad checksum.|
    '#define ERR_UNKNOWNRESPONSE                               302                 //|Unknown response.|
    '#define ERR_UNKNOWNCMD                                    303                 //|Unknown command.|
    '#define ERR_BADSEQUENCENUM                                304                 //|Bad sequence number.|
    '#define ERR_ENCRYPTION                                    305                 //|Packet encryption failed.|
    '#define ERR_TASHIFT                                       400                 //|Track And Accumulate Shift Error.|
    '#define ERR_TAACCUM                                       401                 //|Track And Accumulate Accumulation Error.|
    '#define ERR_TACENTROID                                    402                 //|Track And Accumulate Centroid Error.|
    '#define ERR_TAREMOVEPEDESTAL                              403                 //|Track And Accumulate Pedestal Error.|
    '#define ERR_TASUBOFFSET                                   404                 //|Track And Accumulate Subtract Offset.|
    '#define ERR_TARESIZEIMAGE                                 405                 //|Track And Accumulate Resize Error.|
    '#define ERR_TACLEARBUF                                    406                 //|Track And Accumulate Clear Buffer.|
    '#define ERR_TAFINDMINMAX                                  407                 //|Track And Accumulate find min/max Error.|
    '#define ERR_TASTARBRTDOWN50                               408                 //|Track And Accumulate star brightness down 50%.|
    '#define ERR_TAUSERRECTNOTFOUND                            409                 //|Track And Accumulate rectangle Not found.|
    '#define ERR_COMBINE_BPP                                   500                 //|Combine Not available For the image bits-per-pixel.|
    '#define ERR_COMBINE_FILETYPE                              501                 //|Incorrect file type For this combine Function.|
    '#define ERR_COMBINE_READTRKLST                            502                 //|Error reading track list.|
    '#define ERR_OUTOFDISKSPACE                                503                 //|Out Of disk space.|
    '#define ERR_SATURATEDPIXELS                               504                 //|Cannot proceed, saturated pixels found. If possible lower your exposure time.|
    '#define ERR_FILEAREREADONLY                               505                 //|Unable To complete the operation because one Or more files are read only (Windows) Or locked (Mac).|
    '#define ERR_PATHNOTFOUND                                  506                 //|Unable To create Or access the folder.|
    '#define ERR_FILEMUSTBESAVED                               507                 //|Please save the photo before Using this command.|
    '#define ERR_FILEISSTALE                                   508                 //|The data file Is stale.|
    '#define ERR_STARTOODIM1                                   550                 //|Star too Dim.  Lost during +X move.|
    '#define ERR_STARTOODIM2                                   551                 //|Star too Dim.  Lost during -X move.|
    '#define ERR_STARTOODIM3                                   552                 //|Star too Dim.  Lost during +Y move.|
    '#define ERR_STARTOODIM4                                   553                 //|Star too Dim.  Lost during -Y move.|
    '#define ERR_MOVEMENTTOOSMALL1                             554                 //|Motion too small during +X move.  Increase calibration time.|
    '#define ERR_MOVEMENTTOOSMALL2                             555                 //|Motion too small during -X move.  Increase calibration time.|
    '#define ERR_MOVEMENTTOOSMALL3                             556                 //|Motion too small during +Y move.  Increase calibration time.|
    '#define ERR_MOVEMENTTOOSMALL4                             557                 //|Motion too small during -Y move.  Increase calibration time.|
    '#define ERR_STARTOOCLOSETOEDGE1                           558                 //|Star too close To edge after +X move.|
    '#define ERR_STARTOOCLOSETOEDGE2                           559                 //|Star too close To edge after -X move.|
    '#define ERR_STARTOOCLOSETOEDGE3                           560                 //|Star too close To edge after +Y move.|
    '#define ERR_STARTOOCLOSETOEDGE4                           561                 //|Star too close To edge after -Y move.|
    '#define ERR_AXISNOTPERPENDICULAR1                         562                 //|Invalid motion In X axis.|
    '#define ERR_AXISNOTPERPENDICULAR2                         563                 //|Invalid motion In Y axis.|
    '#define ERR_BOTHAXISDISABLED                              564                 //|Unable To calibrate, both axis are disabled.  At least one axis must be enabled To calibrate.|
    '#define ERR_RECALIBRATE                                   565                 //|Autoguider calibration Is required.  The Declination at calibration Is unknown, but declination Is now known.|
    '#define ERR_NOBRIGHTOBJECTFOUND                           566                 //|No bright Object found On image.|
    '#define ERR_INSUFFICIENTCORRELATION                       567                 //|Insufficient correlation between target image And image under analysis.|
    '#define ERR_ROTATORCONNECTED                              568                 //|Autoguider calibration Is required.  A camera rotator was connected after calibration was performed.|
    '#define ENUM_ERR_ROTATORDISCONNECTED                      569                 //|Autoguider calibration Is required.  A camera rotator was disconnected after calibration was performed.|
    '#define ERR_IMAGESIZECHANGED                              570                 //|Autoguider calibration Is required.  Image size changed since most recent calibration.|
    '#define ENUM_ERR_PARAMOUNT_SYNC_NOT_REQ                   572                 //|The Paramount does Not require star synchronization.|
    '#define ERR_DSSNAMETOLONG                                 600                 //|The file name And/Or path Is too Long.|
    '#define ERR_DSSNOTINITED                                  601                 //|The Digitized Sky Survey Setup Is Not properly initialized, please check Digitized Sky Survey Setup parameters.|
    '#define ERR_DSSSYSERROR                                   602                 //|System Error.  Please verify Digitized Sky Survey Setup parameters are correct And make sure the data Is present.|
    '#define ERR_DSSWRONGDISK                                  603                 //|Wrong Disk.|
    '#define ERR_DSSNOIMAGE                                    604                 //|No image found To extract.|
    '#define ERR_DSSINVALIDCOORDINATE                          605                 //|Invalid coordinate(s).|
    '#define ERR_DSSINVALIDSIZE                                606                 //|Invalid size.|
    '#define ERR_DSSDLLOLD                                     607                 //|The file DSS_DLL.DLL Is old And Not compatible With this program. Please obtain the latest DSS_DLL.DLL.|
    '#define ERR_DSSCDROM                                      608                 //|Unable To access the Digitized Sky Survey data.  Make sure the volume Or drive Is valid.|
    '#define ERR_DSSHEADERSPATH                                609                 //|Unable To access the headers path specified In Digitized Sky Survey Setup.  Please correct the path.|
    '#define ERR_DSSNODSSDISK                                  610                 //|The Digitized Sky Survey data Is Not present In the specified location.|
    '#define ERR_DSSNOTINSURVEY                                611                 //|Not In survey.|
    '#define ERR_SE_INTERNAL_ERROR                             612                 //|An Error occurred within Source Extraction.|
    '#define ERR_ILINK_NOSCALE                                 650                 //|Image Link has no image scale.|
    '#define ERR_ILINK_TOOFEWBMP                               651                 //|Image Link failed because there are Not enough stars In the photo.  Possible solutions include:<ol><li>Try a longer exposure.</li> <li>Lower the <b><i>Detection Threshold</i></b> In the <b><i>Source Extraction Setup</i></b> window To detect fainter stars In the photo.</li><li>Lower the <b><i>Minimum Number Of Pixels Above Threshold</i></b> In the <b><i>Source Extraction Setup</i></b> window To extract stars near the background.</li></ol>|
    '#define ERR_ILINK_TOOFEWSKY                               652                 //|Image Link failed because there are an insufficient number Of matching cataloged stars.  There must be at least eight cataloged stars In Each image To perform an Image Link. Verify which star databases are active.|
    '#define ERR_ILINK_NOMATCHFOUND                            653                 //|Image Link failed, no pattern matching found.  Make sure the RA/Dec coordinates In the FITS header are correct, And Double-check the image scale.|
    '#define ERR_ILINK_NOIMAGE                                 654                 //|Image Link failed because there Is no FITS photo To compare.  Click the <b>Open Fits</b> button On the <b>Search</b> tab To proceed.|
    '#define ERR_ILINK_ERR_ASTROM_SOLN_FAILED                  655                 //|The astrometric solution failed.|
    '#define ERR_ILINK_TOO_FEW_PAIRS                           656                 //|Not enough photo-catalog pairs For an astrometric solution.|
    '#define ERR_ILINK_INVALID_SCALE                           657                 //|The astrometric solution returned an invalid image scale.|
    '#define ERR_ILINK_SOLN_QUESTIONABLE                       658                 //|The astrometric solution appears invalid.|
    '#define ERR_ILINK_RMS_POOR                                659                 //|The astrometric solution RMS appears invalid.|
    '#define ERR_ILINK_WRITING_INTERMEDIATE_FILE               660                 //|Error writing intermediate astrometry file.|
    '#define ERR_ILINK_TOO_MANY_OBJECTS                        661                 //|Too many light sources were found In the photo, increase the Source Extraction Setup's <b>Detection threshold</b> setting (Setup tab).|
    '#define ERR_ILINK_REQUIRED                                662                 //|This operation requires a successful Image Link And one has Not been performed.|
    '#define ERR_SKIPIMAGE                                     700                 //|Skip image Error.|
    '#define ERR_BADFORMAT                                     701                 //|Unrecognized Or bad file format.|
    '#define ERR_OPENINGFILE                                   702                 //|Unable To open file.|
    '#define ERR_FEATURENAINLEVEL                              703                 //|This edition does Not support the requested feature.|
    '#define ERR_SOCKETEXCEPTION                               704                 //|An Error occurred during a network Call.|
    '#define ERR_CANTCREATETHREAD                              705                 //|Unable To create a New thread.|
    '#define ERR_F_DOESNOTEXIST                                709                 //|The file Or folder does Not exist.|
    '#define ERR_F_ACCESS_WRITE                                707                 //|Access denied. You Do Not have write access To the file Or folder Or item.|
    '#define ERR_F_ACCESS_READ                                 706                 //|Access denied. You Do Not have read access To the file Or folder Or item.|
    '#define ERR_F_ACCESS_RW                                   708                 //|Access denied. You Do Not have read/write access To the file Or folder Or item.|
    '#define ERR_OPENGL_NOT_COMPAT                             711                 //|A newer version Of OpenGL Is required To run this application.|
    '#define ERR_CHANGE_PASSWORD                               730                 //|You are required To change your password before you can access this site.|
    '#define ERR_OP_REQUIRES_OPENGL                            732                 //|This feature requires hardware 3D acceleration.<br><br>Click <a href='http://www.bisque.com/videocards'>here</a> for a list of recommended video cards.<br><br>|
    '#define ERR_INDEX_OUT_OF_RANGE                            733                 //|The index Is out Of range.|
    '#define ERR_TRIAL_EXPIRED                                 734                 //|The trial period has expired.|
    '#define ERR_INVALID_SNUM                                  735                 //|Invalid serial number.|
    '#define ERR_OP_REQUIRES_OPENGL2PLUS                       736                 //|This feature requires <b>advanced capabilities</b> Of OpenGL 2.0 Or later.<br><br>Go To Preferences, Advanced tab (On Mac, TheSkyX Menu, On Windows Tools Menu) And enable 'OpenGL 2 Plus Features' to see if it works with your video card hardware.<br><br><div style='color:OliveDrab'>Warning, your video card might not be capable of this feature.</div>  <br><br>Click <a href='http://www.bisque.com/videocards'>here</a> for a list of recommended video cards. |
    '#define ERR_BADWEATHER                                    737                 //|Bad weather prohibits this operation.|
    '#define ERR_WEATHERSTATION_NOT_READY1                     738                 //|The weather station Is Not connected.|
    '#define ERR_WEATHERSTATION_NOT_READY2                     739                 //|The weather station Is still initializing.|
    '#define ERR_WEATHERSTATION_NOT_READY3                     740                 //|Communication With the weather station Is poor Or lost.|
    '#define ERR_WEATHERSTATION_NOT_READY4                     741                 //|The weather station Is In an unknown state.|
    '#define ERR_SGSTARBRTDOWN50                               800                 //|Self-guide star brightness down 50%.|
    '#define ERR_SGNEXT                                        801                 //|Self-guide Next Error.|
    '#define ERR_SGNEXT2                                       802                 //|Self-guide Next two Error.|
    '#define ERR_MM_DOME_NOT_CONNECTED                         805                 //|The mount motion has been prohibitied because the dome Is Not connected.  The software Is configured To prohibit mount motion unless the dome Is open.  Connect the dome so that the software can determine If the dome Is open.|
    '#define ERR_MM_DOME_NOT_OPEN                              806                 //|The mount motion has been prohibitied because the dome Is Not open.  The software Is configured To prohibit mount motion unless the dome Is open.  Open the dome To allow mount motion.|
    '#define ERR_MNCPFIRSTERROR                                900                 //|MNCP first Error.|
    '#define ERR_MNCPLASTERROR                                 999                 //|MNCP last Error.|
    '#define ERR_AUTOSAVE                                      1130                //|Auto-save Error.|
    '#define ERR_UPLOADNOTST6FILE                              1150                //|Unable To load ST-6 file.|
    '#define ERR_NOHEADADJNEEDED                               1151                //|No head adjustment needed.|
    '#define ERR_NOTCFW6A                                      1152                //|Not a CFW 6A.|
    '#define ERR_NOINTERFACE                                   1153                //|No Interface has been selected.|
    '#define ERR_CAMERANOTFOUND                                1154                //|Camera Not found.|
    '#define ERR_BAUDSWITCHFAILED                              1155                //|Baud switch failed.|
    '#define ERR_CANNOTUPLOADDARK                              1156                //|Unable To upload dark frame.|
    '#define ERR_SKIPPINGDARK                                  1157                //|Skipping dark.|
    '#define ERR_SKIPPINGLIGHT                                 1158                //|Skipping light.|
    '#define ERR_SELFGUIDENA                                   1159                //|Self guide Not available.|
    '#define ERR_TRACKLOGNA                                    1160                //|Tracking log Not available.|
    '#define ERR_AOREQUIREST78                                 1161                //|AO Not available For this camera.|
    '#define ERR_CALIBRATEAONOTON                              1162                //|AO Not calibrated.|
    '#define ERR_WRONGCAMERAFOUND                              1163                //|A camera was detected, but it does Not match the one selected.|
    '#define ERR_PIXEL_MATH_OPERAND                            1164                //|Cannot multiply Or divide the image pixels by an operand less than 0.001.|
    '#define ERR_IMAGE_SIZE                                    1165                //|Enlarged image would exceed maximum image size. Try cropping it first.|
    '#define ERR_CANNOT_COLORGRAB                              1166                //|There Is Not a color filter wheel attached.|
    '#define ERR_WRONGCFWFOUND                                 1167                //|A filter wheel was detected, but it does Not match the one selected.|
    '#define FILTERNOTFOUND                                    1168                //|The filter name Is Not valid, please correct it.|
    '#define ERR_APOGEECFGNAME                                 1200                //|A required initialization file was Not found.  Go To Camera, Setup, And press the Settings button To choose the correct file.|
    '#define ERR_APOGEECFGDATA                                 1201                //|Error In Apogee INI file.|
    '#define ERR_APOGEELOAD                                    1202                //|Error transferring APCCD.INI data To camera.|
    '#define ERR_APOGEEOPENOFFSET                              1220                //|Invalid base I/O address passed To Function.|
    '#define ERR_APOGEEOPENOFFSET1                             1221                //|Register access operation Error.|
    '#define ERR_APOGEEOPENOFFSET2                             1222                //|Invalid CCD geometry.|
    '#define ERR_APOGEEOPENOFFSET3                             1223                //|Invalid horizontal binning factor.|
    '#define ERR_APOGEEOPENOFFSET4                             1224                //|Invalid vertical binning factor.|
    '#define ERR_APOGEEOPENOFFSET5                             1225                //|Invalid AIC value.|
    '#define ERR_APOGEEOPENOFFSET6                             1226                //|Invalid BIC value.|
    '#define ERR_APOGEEOPENOFFSET7                             1227                //|Invalid line offset value.|
    '#define ERR_APOGEEOPENOFFSET8                             1228                //|CCD controller Sub-system Not initialized.|
    '#define ERR_APOGEEOPENOFFSET9                             1229                //|CCD cooler failure.|
    '#define ERR_APOGEEOPENOFFSET10                            1230                //|Failure reading image data.|
    '#define ERR_APOGEEOPENOFFSET11                            1231                //|Invalid buffer pointer specified.|
    '#define ERR_APOGEEOPENOFFSET12                            1232                //|File Not found Or Not valid.|
    '#define ERR_APOGEEOPENOFFSET13                            1233                //|Camera configuration data Is invalid.|
    '#define ERR_APOGEEOPENOFFSET14                            1234                //|Invalid CCD handle passed To Function.|
    '#define ERR_APOGEEOPENOFFSET15                            1235                //|Invalid parameter passed To Function.|
    '#define ERR_GPSTFPNOTRUNNING                              1300                //|Shutter timing Is enabled, but the GPSTFP application Is Not running.|
    '#define ERR_IMAGECALWRONGBPP                              5000                //|Unable To reduce. The image being reduced doesn't have the same bits per pixel as the reduction frames.|
    '#define ERR_IMAGECALWRONGSIZE                             5001                //|Unable To reduce. The image being reduced Is larger than the reduction frames.|
    '#define ERR_IMAGECALWRONGBIN                              5002                //|Unable To reduce. The image being reduced doesn't have the same bin mode as the reduction frames.|
    '#define ERR_IMAGECALWRONGSUBFRAME                         5003                //|Unable To reduce. The image being reduced doesn't entirely overlap the reduction frames. Make sure the subframes overlap.|
    '#define ERR_IMAGECALGROUPINUSE                            5004                //|Unable To proceed. The image reduction group Is currently In use.|
    '#define ERR_IMAGECALNOSUCHGROUP                           5005                //|Unable To proceed. The selected image reduction group no longer exists.|
    '#define ERR_IMAGECALNOFRAMES                              5006                //|Unable To proceed. The selected image reduction group does Not contain any reduction frames.|
    '#define ERR_WRONGBPP                                      5020                //|Unable To proceed. The images don't have the same bits per pixel.|
    '#define ERR_WRONGSIZE                                     5021                //|Unable To proceed. The images don't have the same dimensions.|
    '#define ERR_WRONGTYPE                                     5022                //|Unable To proceed. The images don't have the same format.|
    '#define ERR_NOIMAGESINFOLDER                              5050                //|Unable To proceed. The folder doesn't contain any readable images.|
    '#define ERR_NOPATTERNMATCH                                5051                //|The files could Not be aligned. No pattern match was found.|
    '#define ERR_NOTFITS                                       5070                //|This operation requires a FITS file.|
    '#define ERR_KVW_NOMINIMA                                  6000                //|KVW_NOMINIMA.|
    '#define ERR_KVW_DETERMINANTZERO                           6001                //|KVW_DETERMINANTZERO.|
    '#define ERR_KVW_DIVISIONBYZERO                            6002                //|KVW_DIVISIONBYZERO.|
    '#define ERR_KVW_NOTENOUGHPOINTS                           6003                //|KVW_NOTENOUGHPOINTS.|
    '#define ERR_AF_ERRORFIRST                                 7000                //|@Focus Error.|
    '#define ERR_AF_DIVERGED                                   7001                //|@Focus diverged. |
    '#define ERR_AF_UNDERSAMPLED                               7003                //|Insufficient data To measure focus, increase exposure time. |
    '#define ERR_LT_TARGET_LOST_DEC_TOO_HIGH                   7500                //|Target lost, declination too high To maintain tracking.|
    '#define ERR_LT_TARGET_LOST_CANNOT_TRACK                   7501                //|Target lost, unable To maintain tracking.|
    '#define ERR_LT_TARGET_LOST_BELOW_HORIZON                  7502                //|Target lost, below horizon.|
    '#define ERR_LT_TARGET_NOT_A_SATELLITE                     7503                //|Target Not a satellite.|
    '#define ERR_FLICCD_E_FIRST                                8000                //|ERR_FLICCD_E_FIRST|
    '#define ERR_FLICCD_E_NOTSUPP                              8001                //|ERR_FLICCD_E_NOTSUPP|
    '#define ERR_FLICCD_E_INVALID_PARAMETER                    8002                //|ERR_FLICCD_E_INVALID_PARAMETER|
    '#define ERR_FLICCD_E_INVALID_COMPORT                      8003                //|ERR_FLICCD_E_INVALID_COMPORT|
    '#define ERR_FLICCD_E_COMPORT_ERROR                        8004                //|ERR_FLICCD_E_COMPORT_ERROR|
    '#define ERR_FLICCD_E_FAILED_RESET                         8005                //|ERR_FLICCD_E_FAILED_RESET|
    '#define ERR_FLICCD_E_COMMTIMEOUT                          8006                //|ERR_FLICCD_E_COMMTIMEOUT|
    '#define ERR_FLICCD_E_BADDATA                              8007                //|ERR_FLICCD_E_BADDATA|
    '#define ERR_FLICCD_E_NOCALIBRATE                          8008                //|ERR_FLICCD_E_NOCALIBRATE|
    '#define ERR_FLICCD_E_DEVICE_NOT_CONFIGURED                8009                //|ERR_FLICCD_E_DEVICE_NOT_CONFIGUR|
    '#define ERR_FLICCD_E_COMMWRITE                            8010                //|ERR_FLICCD_E_COMMWRITE|
    '#define ERR_FLICCD_E_INVALID_DEVICE                       8011                //|ERR_FLICCD_E_INVALID_DEVICE|
    '#define ERR_FLICCD_E_FUNCTION_NOT_SUPPORTED               8012                //|ERR_FLICCD_E_FUNCTION_NOT_SUPPORTED|
    '#define ERR_FLICCD_E_BAD_BOUNDS                           8013                //|ERR_FLICCD_E_BAD_BOUNDS|
    '#define ERR_FLICCD_E_GRABTIMEOUT                          8014                //|ERR_FLICCD_E_GRABTIMEOUT|
    '#define ERR_FLICCD_E_TODATAHB                             8015                //|ERR_FLICCD_E_TODATAHB|
    '#define ERR_FLICCD_E_TODATALB                             8016                //|ERR_FLICCD_E_TODATALB|
    '#define ERR_FLICCD_E_ECPNOTREADY                          8017                //|ERR_FLICCD_E_ECPNOTREADY|
    '#define ERR_FLICCD_E_ECPREADTIMEOUTHB                     8018                //|ERR_FLICCD_E_ECPREADTIMEOUTHB|
    '#define ERR_FLICCD_E_ECPREADTIMEOUTLB                     8019                //|ERR_FLICCD_E_ECPREADTIMEOUTLB|
    '#define ERR_FLICCD_E_ECPREADTIMEOUT                       8020                //|ERR_FLICCD_E_ECPREADTIMEOUT|
    '#define ERR_FLICCD_E_ECPREVERSETIMEOUT                    8021                //|ERR_FLICCD_E_ECPREVERSETIMEOUT|
    '#define ERR_FLICCD_E_ECPWRITETIMEOUTHB                    8022                //|ERR_FLICCD_E_ECPWRITETIMEOUTHB|
    '#define ERR_FLICCD_E_ECPWRITETIMEOUTLB                    8023                //|ERR_FLICCD_E_ECPWRITETIMEOUTLB|
    '#define ERR_FLICCD_E_ECPWRITETIMEOUT                      8024                //|ERR_FLICCD_E_ECPWRITETIMEOUT|
    '#define ERR_FLICCD_E_FORWARDTIMEOUT                       8025                //|ERR_FLICCD_E_FORWARDTIMEOUT|
    '#define ERR_FLICCD_E_NOTECP                               8026                //|ERR_FLICCD_E_NOTECP|
    '#define ERR_FLICCD_E_FUNCTIONNOTSUPP                      8027                //|ERR_FLICCD_E_FUNCTIONNOTSUPP|
    '#define ERR_FLICCD_E_NODEVICES                            8028                //|ERR_FLICCD_E_NODEVICES|
    '#define ERR_FLICCD_E_WRONGOS                              8029                //|ERR_FLICCD_E_WRONGOS|
    '#define ERR_TEMMA_RAERROR                                 8030                //|Slew/sync Error: Temma reports the right ascension Is incorrect For go To Or synchronization.|
    '#define ERR_TEMMA_DECERROR                                8031                //|Slew/sync Error: Temma reports the declination Is incorrect For go To Or synchronization.|
    '#define ERR_TEMMA_TOOMANYDIGITS                           8032                //|Slew/sync Error: Temma reports the format Error For go To Or synchronization.|
    '#define ERR_TEMMA_BELOWHORIZON                            8033                //|Slew/sync Error: Temma reports the Object Is below the horizon.|
    '#define ERR_TEMMA_STANDBYMODE                             8034                //|Slew Error: Temma reports the mount Is In standby mode.|
    '#define ERR_ACLUNDEFINEDERR                               1                   //|ACL undefined Error.|
    '#define ERR_ACLSYNTAX                                     2                   //|ACL syntax Error.|
    '#define ERR_ACLTYPEMISMATCH                               10                  //|ACL type mismatch Error.|
    '#define ERR_ACLRANGE                                      11                  //|ACL range Error.|
    '#define ERR_ACLVALREADONLY                                12                  //|ACL value Is read only.|
    '#define ERR_ACLCMDUNSUPPORTED                             13                  //|ACL command Is unsupported.|
    '#define ERR_ACLUNSUPPORTID                                14                  //|ACL unsupported id.|
    '#define ERR_ACLCMDINACTIVE                                15                  //|ACL command inactive.|
    '#define ERR_ACLGOTOILLEGAL                                100                 //|ACL illegal go To command.|
    '#define ERR_ACLGOTOBELOWHRZ                               101                 //|ACL Error: destination Is below the horizon.|
    '#define ERR_ACLGOTOLIMITS                                 102                 //|ACL go To limit.|
    '#define ERR_NOT_IMPL                                      11000               //|This command Is Not supported.|
    '#define ERR_NOT_IMPL_IN_MODEL                             11001               //|This command Is Not implemented In the model.|
    '#define ERR_OPENING_FOVI_FILES                            11002               //|One Of the Field Of View Indicator database files cannot be found. (Abnormal installation.)|
    '#define ERR_NO_IRIDIUM_SATELLITES                         11003               //|No Iridium satellite two-line elements are currently loaded.|
    '#define ERR_ACCESS_DENIED                                 11004               //|Access Is denied.  Check your username And Or password.|
    '#define ERR_ALL_TLES_DATE_REJECTED                        11005               //|All TLEs were Date rejected, so no satellites will be loaded. Check the Date Of the TLEs And make sure TheSkyX's date is within 45 days of this date.|
    '#define ERR_SBSCODEBASE                                   1000                //|Base offset For creating wire safe scodes|
    '#define ERR_SBIGST7FIRST                                  30000               //|SBIG ST7 first Error.|
    '#define ERR_SBIGCCCFWFIRST                                31000               //|SBIG first cfw Error.|
    '#define ENUM_ERR_CFISIOFIRST                              33000               //|CFITSIO first Error.|
    '#define ERR_CUSTOMAPIFIRST                                1400                //|Custom api Error code first.|
    '#define ERR_CUSTOMAPILAST                                 1499                //|Custom api error code last.|
    '#define ERR_IPLSUITEERR                                   1500                //|IPL suite error first|
    '#define ERR_GDIERR_BASE                                   1600                //|GDI error base|
    '#define ERR_SBIGTCEEXTFIRST                               1050                //|SBIG TCE error first.|
    '#define ERR_SBIGTCEEXTLAST                                1099                //|SBIG TCE error last.|
    '#define ERR_SBIGSERIALFIRST                               1100                //|SBIG serial error first.|
    '#define ERR_SBIGSERIALLAST                                1125                //|SBIG serial error last.|
    '#define ERR_MKS_ERROR_FIRST                               20000               //|MKS first error.|
    '#define ERR_MKS_ERROR_LAST                                25000               //|MKS last error.|
    '#define ERR_SOCKET_ERROR_FIRST                            27000               //|Socket first error.|    
    '#define ERR_SOCKET_ERROR_LAST                             27100               //|Socket last error.|
    '#define ERR_MKS_COMM_BASE                                 21000               //|COMM_BASE.|
    '#define ERR_MKS_COMM_OKPACKET                             21000               //|COMM_OKPACKET.|
    '#define ERR_MKS_COMM_NOPACKET                             21001               //|Serial command packet not included with command. COMM_NOPACKET.|
    '#define ERR_MKS_COMM_TIMEOUT                              21002               //|Receive time-out.COMM_TIMEOUT.|
    '#define ERR_MKS_COMM_COMMERROR                            21003               //|Serial communication error. COMM_COMMERROR.|
    '#define ERR_MKS_COMM_BADCHAR                              21004               //|Invalid serial command error. COMM_BADCHAR.|
    '#define ERR_MKS_COMM_OVERRUN                              21005               //|Packet overrun error. COMM_OVERRUN.|
    '#define ERR_MKS_COMM_BADCHECKSUM                          21006               //|Bad checksum error. COMM_BADCHECKSU.|
    '#define ERR_MKS_COMM_BADLEN                               21007               //|Invalid length of serial command error. COMM_BADLEN.|
    '#define ERR_MKS_COMM_BADCOMMAND                           21008               //|Invalid serial command error. COMM_BADCOMMAND.|
    '#define ERR_MKS_COMM_INITFAIL                             21009               //|Could not open communications port.  The port is either in use by another application or not recognized by the system. COMM_INITFAIL|
    '#define ERR_MKS_COMM_NACK                                 21010               //|No acknowledgement of command from device. COMM_NACK.|
    '#define ERR_MKS_COMM_BADID                                21011               //|Invalid identifier. COMM_BADID.|
    '#define ERR_MKS_COMM_BADSEQ                               21012               //|Invalid command sequence. COMM_BADSEQ.|
    '#define ERR_MKS_COMM_BADVALCODE                           21013               //|Invalid command code. COMM_BADVALCODE.|
    '#define ERR_MKS_MAIN_BASE                                 22000               //|MAIN_BASE.|
    '#define ERR_MKS_MAIN_WRONG_UNIT                           22001               //|MAIN_WRONG_UNIT.|
    '#define ERR_MKS_MAIN_BADMOTORINIT                         22002               //|MAIN_BADMOTORINIT.|
    '#define ERR_MKS_MAIN_BADMOTORSTATE                        22003               //|Unable to slew because the mount has not been homed. Click Telescope, Options, Find Home to home the mount.|
    '#define ERR_MKS_MAIN_BADSERVOSTATE                        22004               //|Indexing before finding switch 1.|
    '#define ERR_MKS_MAIN_SERVOBUSY                            22005               //|Indexing before finding switch 2.|
    '#define ERR_MKS_MAIN_BAD_PEC_LENGTH                       22006               //|Invalid length of PEC table. MAIN_BAD_PEC_LENGTH.|
    '#define ERR_MKS_MAIN_AT_LIMIT                             22007               //|The mount is at a minimum or maximum position limit and cannot be slewed.  This error may be the result of improper synchronization near the meridian. When syncing near the meridian, be sure the optical tube assembly and the synchronization star are on opposite sides of the meridian.|
    '#define ERR_MKS_MAIN_NOT_HOMED                            22008               //|Mount has not been homed. Click Telescope, Options, Find Home to home the mount.|
    '#define ERR_MKS_MAIN_BAD_POINT_ADD                        22009               //|Object-Tracking point error.|
    '#define ERR_MKS_MAIN_INVALID_PEC                          22010               //|The PEC table is invalid.|
    '#define ERR_MKS_SLEW_PAST_LIMIT                           22011               //|The slew is not possible because the target is beyond a slew limit.Slew limits prevent the mount from colliding with the pier and or encountering a physical hard stop.  In other words, a target beyond a slew limit is mechanically unreachable.|
    '#define ERR_MKS_MAIN_BAD_CONTROL_CODE                     22020               //|MKS4000: Command-code is invalid.|
    '#define ERR_MKS_MAIN_BAD_SYSTEM_ID                        22021               //|Unknown system type (not an MKS 3000 or MKS 4000)|
    '#define ERR_MKS_FLASH_BASE                                23000               //|FLASH_BASE.|
    '#define ERR_MKS_FLASH_PROGERR                             23001               //|FLASH_PROGERR.|
    '#define ERR_MKS_FLASH_ERASEERR                            23002               //|FLASH_ERASEERR.|
    '#define ERR_MKS_FLASH_TIMEOUT                             23003               //|FLASH_TIMEOUT.|
    '#define ERR_MKS_FLASH_CANT_OPEN_FILE                      23004               //|FLASH_CANT_OPEN_FILE.|
    '#define ERR_MKS_FLASH_BAD_FILE                            23005               //|FLASH_BAD_FILE.|
    '#define ERR_MKS_FLASH_FILE_READ_ERR                       23006               //|FLASH_FILE_READ_ERR.|
    '#define ERR_MKS_FLASH_BADVALID                            23007               //|FLASH_BADVALID.|
    '#define ERR_MKS_FLASH_INVALID_SECTION                     23008               //|MKS4000: Invalid FLASH section.|
    '#define ERR_MKS_FLASH_INVALID_ADDRESS                     23009               //|MKS4000: Invalid FLASH address.|
    '#define ERR_MKS_MOTOR_BASE                                24000               //|MOTOR_BASE.|
    '#define ERR_MKS_MOTOR_OK                                  24000               //|MOTOR_OK.|
    '#define ERR_MKS_MOTOR_OVERCURRENT                         24001               //|MOTOR_OVERCURRENT.|
    '#define ERR_MKS_MOTOR_POSERRORLIM                         24002               //|<b>The mount cannot slew. See the list of likely reasons below.<br><br>To recover, turn the mount off, wait a few moments and then turn the mount back on.<br><br></b><table border=1><tr><th>Possible Reasons In Order of Likelihood</th><th>Solution</th></tr><tr><td>1. The mount payload is too far out of balance.</td><td>Carefully balance the payload.</td></tr><tr><td>2. A transport lock knob is in the lock position.</td><td>Unlock the transport lock knob(s).</td></tr><tr><td>3. The mount has encountered a physical obstacle.</td><td>Move the obstacle.</td></tr><tr><td>4. You've recently added through the mount cabling.</td><td>Make sure you did not accidentally unplug an internal mount cable.  Also make sure the added cabling is not binding a mount axis from rotating.</td></tr><tr><td>5. The worm block cam adjustment has been adjusted recently and it is too tight.</td><td>See the technical article on adjusting the worm block.</td></tr><tr><td>6. The ambient temperature is near or below freezing.</td><td>Lower mount speed/acceleration.</td></tr></table><br><br>|
    '#define ERR_MKS_MOTOR_STILL_ON                            24003               //|Motor still on but command needs it stopped.|
    '#define ERR_MKS_MOTOR_NOT_ON                              24004               //|Motor off.|
    '#define ERR_MKS_MOTOR_STILL_MOVING                        24005               //|Motor still slewing but command needs it stopped.|
    '#define ERR_MKS_MOTOR_FIELD_TIMEOUT                       24006               //|Timed out while fielding.|
    '#define ERR_MKS_MOTOR_BAD_CONTROL_STATE                   24007               //|MOTOR_BAD_CONTROL_STATE.|
    '#define ERR_MKS_MOTOR_BAD_SERVO_STATE                     24005               //|MOTOR_BAD_SERVO_STATE.|
    '#define ERR_GEMINI_OBJECT_BELOW_HORIZON                   275                 //|Gemini - Object below the horizon.|
    '#define ERR_GEMINI_NO_OBJECT_SELECTED                     276                 //|Gemini - No object selected.|
    '#define ERR_GEMINI_MANUAL_CONTROL                         277                 //|Gemini - Hand paddle is in manual control mode or the Prevent Slews option is turned on.|
    '#define ERR_GEMINI_POSITION_UNREACHABLE                   278                 //|Gemini - Position is unreachable.|
    '#define ERR_GEMINI_NOT_ALIGNED                            279                 //|Gemini - Gemini not aligned.|
    '#define ERR_GEMINI_OUTSIDE_LIMITS                         280                 //|Gemini - Outside slew limits.|
    '#define ERR_GEMINI_VERSION_NOT_SUPPORTED                  281                 //|Gemini - Version 4 or later is required. Please update your Gemini firmware.|
    '#define ERR_VIXEN_UNKNOWN                                 290                 //|Star Book - Unknown error accessing mount.|
    '#define ERR_VIXEN_URLNOTSET                               291                 //|Star Book - The specified URL appears to be invalid.|
    '#define ERR_VIXEN_STATUSINVALID                           292                 //|Star Book - No or invalid data received.|
    '#define ERR_VIXEN_STATUSNOTAVAILABLE                      293                 //|Star Book - Error reading mount status.|
    '#define ERR_VIXEN_ILLEGALSTATE                            294                 //|Star Book - Mount in wrong state to accept this command.|
    '#define ERR_VIXEN_SETRADECERROR                           295                 //|Star Book - Error when trying to set RA/Dec.  Make sure the new alignment object is more than 10 degrees from the previous alignment object.|
    '#define ERR_VIXEN_INVALIDFORMAT                           296                 //|Star Book - Command incorrectly formatted.|
    '#define ERR_VIXEN_BELOWHORIZON                            297                 //|Star Book - Target below the horizon.|
    '#define ERR_VIXEN_HOMEERROR                               298                 //|Star Book - Error with HOME command.|
    '#define ERR_OPEN_NV_THEME                                 11101               //|Error opening TheSkyX Night Vision Mode Theme.  Click the Night Vision Mode Setup command on the Display menu and verify that the Night Vision Mode them file name is correct and the theme exists.|
    '#define ERR_OPEN_STANDARD_THEME                           11102               //|Error opening the Standard Theme.  Click the Night Vision Mode Setup command on the Display menu and verify that the Standard Theme file name is correct and the theme exists.|
    '#define ERR_INVALID_DATA                                  11103               //|The comet or minor planet orbital element data, or data file, contains invalid data and cannot be used to display this object or these objects.|
    '#endif // SBERRORX_H
    'X2 Examples
    '(C) Software Bisque, Inc. All rights reserved. 

End Module
