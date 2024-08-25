Module ModWeather

    Public pWeatherStatus As String 'SAFE / UNSAFE
    Public pLightStatus As String 'DARK / LIGHT / VERY LIGHT / UNKNOWN
    Public pCloudMonitorSafe As DateTime

    Public Structure StructWeather
        Public containsData As Integer 'when 1, filled, when 0 structure is empty
        Public dataGMTTime As DateTime '2020/03/13 20:39:41
        Public cwinfo As String 'Serial 1544, FW: 5.7
        Public clouds As Double '-14.16
        Public temp As Double '3.0
        Public wind As Double '-1
        Public gust As Double '-1
        Public rain As Double '5162
        Public light As Double '45774
        Public switch As Integer '1
        Public safe As Integer '1
        Public hum As Double '99
        Public dewp As Double '2.85
        Public AAGversion As String 'NEW/OLD
    End Structure

    Public pStrucWeather As StructWeather

    Public Function MonitorWeather() As String

        MonitorWeather = "OK"
        Try
            ProcessAAGData()
            ReportWeather()

        Catch ex As Exception
            MonitorWeather = "MonitorWeather: " + ex.Message
            LogSessionEntry("ERROR", "MonitorWeather: " + ex.Message, "", "MonitorWeather", "PROGRAM")
        End Try
    End Function


    Public Function ReportWeather() As String
        Dim startExecution As Date
        Dim executionTime As TimeSpan

        ReportWeather = "OK"
        Try
            startExecution = DateTime.UtcNow()

            If pStrucWeather.containsData = 1 Then

                FrmMain.lblLastRead.Text = "Last read: " + pLastKnownAAGConnected.ToString
                FrmMain.LblAmbientTemperature.Text = "Ambient temp: " + Format(pStrucWeather.temp) + "°C"

                'cloud status
                FrmMain.ToolTip.SetToolTip(FrmMain.lblCloud, Format(pStrucWeather.clouds))
                Select Case pStrucWeather.clouds
                    Case < My.Settings.sWeatherCloud_Clear
                        FrmMain.lblCloud.BackColor = ColorTranslator.FromHtml("#4cd137") 'green
                        FrmMain.lblCloud.Text = "Clear"
                        pStrucWeather.clouds = 1
                    'FrmMain.ToolTip
                    Case < My.Settings.sWeatherCloud_Cloudy
                        FrmMain.lblCloud.BackColor = Color.Orange
                        FrmMain.lblCloud.Text = "Cloudy"
                        If pStrucWeather.AAGversion = "OLD" Then
                            pStrucWeather.safe = 0
                        End If
                        pStrucWeather.clouds = 2
                    Case < My.Settings.sWeatherCloud_Overcast
                        FrmMain.lblCloud.BackColor = ColorTranslator.FromHtml("#d63031") 'red
                        FrmMain.lblCloud.Text = "Overcast"
                        If pStrucWeather.AAGversion = "OLD" Then
                            pStrucWeather.safe = 0
                        End If
                        pStrucWeather.clouds = 3
                    Case Else
                        FrmMain.lblCloud.BackColor = Color.White
                        FrmMain.lblCloud.Text = "Unknown"
                        If pStrucWeather.AAGversion = "OLD" Then
                            pStrucWeather.safe = 0
                        End If
                        pStrucWeather.clouds = 0
                End Select

                'light status
                FrmMain.ToolTip.SetToolTip(FrmMain.lblLight, Format(pStrucWeather.light))

                If My.Settings.sSimulatorMode = True And My.Settings.sDebugAAGData = True Then
                    Select Case My.Settings.sDebugLight
                        Case "DARK"
                            FrmMain.lblLight.Text = "Dark"
                            pLightStatus = "DARK"
                            FrmMain.lblLight.BackColor = Color.Gray
                            FrmMain.lblLight.Visible = True
                        Case "LIGHT"
                            FrmMain.lblLight.Text = "Light"
                            pLightStatus = "LIGHT"
                            FrmMain.lblLight.BackColor = Color.Yellow
                            FrmMain.lblLight.Visible = True
                    End Select
                Else

                    Select Case pStrucWeather.light
                        Case >= My.Settings.sWeatherLightness_Dark
                            FrmMain.lblLight.Text = "Dark"
                            pLightStatus = "DARK"
                            FrmMain.lblLight.BackColor = Color.Gray
                            FrmMain.lblLight.Visible = True
                        Case >= My.Settings.sWeatherLightness_Light
                            FrmMain.lblLight.Text = "Light"
                            pLightStatus = "LIGHT"
                            FrmMain.lblLight.BackColor = Color.Yellow
                            FrmMain.lblLight.Visible = True
                        Case >= My.Settings.sWeatherLightness_VeryLight
                            FrmMain.lblLight.Text = "Very Light"
                            pLightStatus = "VERY LIGHT"
                            FrmMain.lblLight.BackColor = Color.LightYellow
                            FrmMain.lblLight.Visible = True
                        Case -9
                            FrmMain.lblLight.Text = ""
                            pLightStatus = "N/A"
                            FrmMain.lblLight.Visible = False
                            If pStrucWeather.AAGversion = "OLD" Then
                                pStrucWeather.safe = 0
                            End If
                        Case Else
                            FrmMain.lblLight.BackColor = Color.White
                            pLightStatus = "UNKNOWN"
                            FrmMain.lblLight.Text = "Unknown"
                            FrmMain.lblLight.Visible = True
                            If pStrucWeather.AAGversion = "OLD" Then
                                pStrucWeather.safe = 0
                            End If
                    End Select
                End If
                'rain status
                FrmMain.ToolTip.SetToolTip(FrmMain.lblRain, Format(pStrucWeather.rain))
                Select Case pStrucWeather.rain
                    Case > My.Settings.sWeatherRain_Dry
                        FrmMain.lblRain.Text = "Dry"
                        FrmMain.lblRain.BackColor = Color.Transparent
                    Case > My.Settings.sWeatherRain_Wet
                        FrmMain.lblRain.Text = "Wet"
                        FrmMain.lblRain.BackColor = Color.LightBlue
                        If pStrucWeather.AAGversion = "OLD" Then
                            pStrucWeather.safe = 0
                        End If
                    Case > My.Settings.sWeatherRain_Rain
                        FrmMain.lblRain.Text = "Rain"
                        FrmMain.lblRain.BackColor = Color.Blue
                        If pStrucWeather.AAGversion = "OLD" Then
                            pStrucWeather.safe = 0
                        End If
                    Case Else
                        FrmMain.lblRain.Text = "Unknown"
                        FrmMain.lblRain.BackColor = Color.White
                        If pStrucWeather.AAGversion = "OLD" Then
                            pStrucWeather.safe = 0
                        End If
                End Select

                'humidity
                Select Case pStrucWeather.hum
                    Case > My.Settings.sWeatherHumidity_Humid
                        FrmMain.lblRelativeHumidity.Text = "Humidity: wet - " + Format(pStrucWeather.hum) + "%"
                        FrmMain.lblRelativeHumidity.BackColor = Color.LightBlue

                    Case > My.Settings.sWeatherHumidity_Normal
                        FrmMain.lblRelativeHumidity.Text = "Humidity: normal - " + Format(pStrucWeather.hum) + "%"
                        FrmMain.lblRelativeHumidity.BackColor = Color.LightCyan
                    Case > My.Settings.sWeatherHumidity_Dry
                        FrmMain.lblRelativeHumidity.Text = "Humidity: dry - " + Format(pStrucWeather.hum) + "%"
                        FrmMain.lblRelativeHumidity.BackColor = Color.Transparent
                    Case -9
                        FrmMain.lblRelativeHumidity.Text = "Humidity: N/A"
                        FrmMain.lblRelativeHumidity.BackColor = Color.Transparent
                    Case Else
                        FrmMain.lblRelativeHumidity.Text = "Humidity: N/A"
                        FrmMain.lblRelativeHumidity.BackColor = Color.White
                End Select

                Dim Switch As String
                'safe or unsafe / open or closed
                If My.Settings.sSimulatorMode = True And My.Settings.sDebugAAGData = True Then
                    Select Case My.Settings.sDebugCloud
                        Case "SAFE"
                            FrmMain.lblSafe.BackColor = Color.LightGreen
                            FrmMain.LblCloudSafe.BackColor = Color.LightGreen
                            pWeatherStatus = "SAFE"
                            FrmMain.LblCloudSafe.Text = "SAFE and Open"
                        Case "UNSAFE"
                            FrmMain.lblSafe.BackColor = Color.IndianRed
                            FrmMain.LblCloudSafe.BackColor = Color.IndianRed
                            pWeatherStatus = "UNSAFE"
                            FrmMain.LblCloudSafe.Text = "UNSAFE and Closed"
                    End Select
                Else
                    If pStrucWeather.safe = 1 And pStrucWeather.switch = 1 Then
                        FrmMain.lblSafe.BackColor = Color.LightGreen
                        FrmMain.LblCloudSafe.BackColor = Color.LightGreen
                        pWeatherStatus = "SAFE"
                        Switch = " and Open"
                    ElseIf pStrucWeather.safe = 1 And pStrucWeather.switch = 0 Then
                        FrmMain.lblSafe.BackColor = Color.IndianRed
                        FrmMain.LblCloudSafe.BackColor = Color.IndianRed
                        pWeatherStatus = "UNSAFE"
                        Switch = " and Closed"
                    ElseIf pStrucWeather.safe = 0 Then
                        FrmMain.lblSafe.BackColor = Color.IndianRed
                        FrmMain.LblCloudSafe.BackColor = Color.IndianRed
                        pWeatherStatus = "UNSAFE"
                        Switch = " and Closed"
                    Else
                        pWeatherStatus = "UNKNOWN"
                        FrmMain.lblSafe.BackColor = Color.Transparent
                        FrmMain.LblCloudSafe.BackColor = Color.Transparent
                        Switch = " and Unknown"
                    End If

                    FrmMain.LblCloudSafe.Text = If(pStrucWeather.safe = 0, "Unsafe", "Safe") + Switch

                End If

                LogSessionEntry("BRIEF", pWeatherStatus + " weather conditions.", "", "ReportWeather", "WEATHER_STATUS")

            Else
                'no cloud device defined
                FrmMain.lblLastRead.Text = "Last read: no device available!"
                FrmMain.LblAmbientTemperature.Text = "Ambient temp: N/A"

                'cloud status
                FrmMain.lblCloud.BackColor = Color.Transparent
                FrmMain.lblCloud.Text = ""
                'light status
                FrmMain.lblLight.BackColor = Color.Transparent
                FrmMain.lblLight.Text = ""
                'rain status
                FrmMain.lblRain.Text = ""
                FrmMain.lblRain.BackColor = Color.Transparent
                'humidity
                FrmMain.lblRelativeHumidity.Text = "Humidity:"

                'safe or unsafe / open or closed
                FrmMain.LblCloudSafe.Text = ""
                FrmMain.lblSafe.BackColor = Color.Transparent
                FrmMain.LblCloudSafe.BackColor = Color.Transparent
            End If


            '-------------------------------------------------------------------------------------------------------
            ' send alert when clear for a while
            '-------------------------------------------------------------------------------------------------------

            If pWeatherStatus = "SAFE" And pLightStatus = "DARK" Then
                If pCloudMonitorSafe = CDate("01-01-0001") Then
                    pCloudMonitorSafe = DateTime.UtcNow()
                End If

                If (pRunStatus <> "" And pRunStatus <> "ABORTED") Then
                    pCloudMonitorSafe = DateTime.UtcNow()
                Else
                    If DateDiff(DateInterval.Second, pCloudMonitorSafe, DateTime.UtcNow) > My.Settings.sWeatherClearDelay And (pRunStatus = "NOT RUNNING" Or pRunStatus = "ABORTED" Or pRunStatus = "ABORTING") And pContinueRunningCalibrationFrames = False Then
                        LogSessionEntry("ERROR", "Clear skies for: " + Format(My.Settings.sWeatherClearDelay) + " seconds detected! ", "", "ReportWeather", "WEATHER")
                        'rest the timer, so a message only pops up every timed minutes.
                        pCloudMonitorSafe = DateTime.UtcNow()
                    End If
                End If
            Else
                pCloudMonitorSafe = CDate("01-01-0001")
            End If

            executionTime = DateTime.UtcNow() - startExecution
            LogSessionEntry("DEBUG", "  ReportWeather: " + executionTime.ToString, "", "ReportWeather", "PROGRAM")

        Catch ex As Exception
            ReportWeather = "ReportWeather: " + ex.Message
            LogSessionEntry("ERROR", "ReportWeather: " + ex.Message, "", "ReportWeather", "PROGRAM")
        End Try
    End Function


    Public Sub ClearStrucWeather()
        With pStrucWeather
            .containsData = 0
            .dataGMTTime = Nothing
            .cwinfo = ""
            .clouds = 0
            .temp = 0
            .wind = 0
            .gust = 0
            .rain = 0
            .light = 0
            .switch = 0
            .safe = 0
            .hum = 0
            .dewp = 0
        End With
    End Sub

End Module