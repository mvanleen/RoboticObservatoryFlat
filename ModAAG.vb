Imports System.IO
Imports System.Net

Module ModAAG

    Public pAAGFile As String
    Public pLastKnownAAGConnected As DateTime

    'http://185.228.120.224:17045/cgi-bin/cgiLastData
    'http://customer.astrohostinge-eye.es:17045/cgi-bin/cgiLastData
    'dataGMTTime=2020/03/13 20:39:41
    'cwinfo=Serial 1544, FW: 5.7
    'clouds=-14.160000
    'temp=3.000000
    'wind=-1
    'gust=-1
    'rain=5162
    'light=45774
    'switch=1
    'safe=1
    'hum=99
    'dewp=2.850000

    'clouded at night
    'dataGMTTime=2020/03/14 19:43:22
    'cwinfo=Serial 1544, FW: 5.7
    'clouds=0.430000
    'temp=13.230000
    'wind=-1
    'gust=-1
    'rain=5120
    'light=34316
    'switch=0
    'safe=0
    'hum=88
    'dewp=11.280000

    'clear: aag_sld.dat
    'dataGMTTime=2020/12/18 1905:20
    'cwinfo=Serial: 1814, FW: 5.73
    'clouds=-9.490000
    'temp=7.830000
    'wind=-1
    'gust=-1
    'rain=4480
    'light=57232
    'switch=1
    'safe=1
    'hum=90
    'dewp=6.290000

    'aag_sld.dat
    '2020-12-13 21:07:04,00 C K    8,2    7,0    7,0    0,0  88    5,2  12 0 0 00000 044178,87991 3 1 1 1 1 1
    '2020-12-18 20:09:36,00 C K   -9,6    7,8    7,8    0,0  90    6,3  12 0 0 00000 044183,84000 1 1 1 1 0 0
    '2020-12-18 20:18:08.00 C K   -9.5    7.7    7.7    0.0  90    6.2  12 0 0 00000 044183.84593 1 1 1 1 0 0

    '2020-12-13 20:54:43,00
    'C
    'K
    '12,2 clouds
    '7,1 
    '7,1 temp
    '0,0
    '87 hum
    '5,1 dewp
    '10
    '0
    '0
    '00000
    '044178,87133 light
    '3 1 clear 2 cloudy 3 very cloudy
    '1 1 calm 2 windy
    '1 1 no rain 2 rain
    '1 1 dark 2 light
    '1 switch ?
    '1 safe ?

    Public Function GetAAGData() As String
        Dim startExecution As Date
        Dim executionTime As TimeSpan

        GetAAGData = "OK"
        Try

            startExecution = DateTime.UtcNow()

            If My.Settings.sDebugAAGData = False Then
                Dim address As String = My.Settings.sWeatherURL
                Dim client As New WebClient()
                Dim reader As New StreamReader(client.OpenRead(address))
                pAAGFile = reader.ReadToEnd
                pLastKnownAAGConnected = DateTime.UtcNow
                reader.Close()
            ElseIf My.Settings.sDebugKillAAG = True Then
                'do nothing and do not update the time
                LogSessionEntry("FULL", "GetAAGData: no AAG data received !", "", "GetAAGData", "WEATHER")
            Else
                ' werkt niet string nodig
                pAAGFile = "dataGMTTime=" + DateTime.UtcNow().ToString + vbCrLf +
                            "cwinfo=Serial: 1396, FW: 5.71
                            clouds=-0.130000
                            temp=45.650000
                            wind=-1
                            gust=-1
                            rain=3968
                            light=1
                            switch=1
                            safe=1
                            hum=14
                            dewp=11.900000"
                pLastKnownAAGConnected = DateTime.UtcNow
            End If
            'log all the weather data in a file, simulator or not
            LogSessionEntry("DEBUG", "  GetAAGData" + pAAGFile, "", "GetAAGData", "WEATHER_DATA")
            LogSessionEntry("DEBUG", "  GetAAGData: " + executionTime.ToString, "", "GetAAGData", "WEATHER")

        Catch ex As Exception
            GetAAGData = "GetAAGData: " + ex.Message
            LogSessionEntry("FULL", "GetAAGData: " + ex.Message, "", "GetAAGData", "WEATHER")
        End Try
    End Function



    Public Function CheckTimeoutAAG() As String
        'wrapper that takes into account a delay before the alarm is raised
        Dim i As Long
        Dim startExecution As Date
        Dim executionTime As TimeSpan

        CheckTimeoutAAG = "OK"
        Try
            startExecution = DateTime.UtcNow()

            'If My.Settings.sSimulatorMode = vbFalse Then
            i = DateDiff(DateInterval.Second, pLastKnownAAGConnected, Date.UtcNow)

            'log file is not changing
            If i >= My.Settings.sWeatherTimeout Then
                FrmMain.lblSafe.BackColor = Color.Red
                FrmMain.LblCloudSafe.Text = "AAG offline"
                CheckTimeoutAAG = "NOK"
                LogSessionEntry("ERROR", "AAG offline !", "", "CheckTimeoutAAG", "WEATHER")
            End If

            executionTime = DateTime.UtcNow() - startExecution
            LogSessionEntry("DEBUG", "  CheckTimeoutAAG: " + executionTime.ToString, "", "CheckTimeoutAAG", "WEATHER")

        Catch ex As Exception
            CheckTimeoutAAG = "CheckTimeoutAAG: " + ex.Message
            LogSessionEntry("ERROR", "CheckTimeoutAAG: " + ex.Message, "", "CheckTimeoutAAG", "WEATHER")
        End Try
    End Function


    Public Function ProcessAAGData() As String
        Dim returnvalue As String
        Dim i As Integer
        Dim startExecution As Date
        Dim executionTime As TimeSpan

        ProcessAAGData = "OK"
        Try
            startExecution = DateTime.UtcNow()

            'get the file
            returnvalue = GetAAGData()
            If returnvalue <> "OK" Then
                '  FrmMain.TimerWeather.Interval = 60000
                ProcessAAGData = returnvalue
                Exit Function
                'Else
                '    'reset the interval
                '   FrmMain.TimerWeather.Interval = 5000
            End If

            ClearStrucWeather()

            ' Split string based on comma
            'Dim words As String() = pAAGFile.Split(New Char() {vbLf})
            Dim words As String() = pAAGFile.Split(CChar(vbLf))

            i = words.Count

            If i = 9 Then
                'old model cloudwatcher
                ' Use For Each loop over words and display them
                pStrucWeather.dataGMTTime = CDate(words(0).Replace("dataGMTTime=", ""))
                Double.TryParse(words(1).Replace("clouds=", ""), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, pStrucWeather.clouds)
                Double.TryParse(words(2).Replace("temp=", ""), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, pStrucWeather.temp)
                Double.TryParse(words(3).Replace("wind=", ""), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, pStrucWeather.wind)
                Double.TryParse(words(4).Replace("rain=", ""), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, pStrucWeather.rain)
                Double.TryParse(words(5).Replace("light=", ""), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, pStrucWeather.light)
                Double.TryParse(words(6).Replace("switch=", ""), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, Convert.ToInt16(pStrucWeather.switch))
                pStrucWeather.safe = 1
                pStrucWeather.AAGversion = "OLD"
                pStrucWeather.containsData = 1
            Else
                'new model cloudwatcher
                ' Use For Each loop over words and display them
                pStrucWeather.dataGMTTime = CDate(words(0).Replace("dataGMTTime=", ""))
                pStrucWeather.cwinfo = words(1)

                Double.TryParse(words(2).Replace("clouds=", ""), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, pStrucWeather.clouds)
                Double.TryParse(words(3).Replace("temp=", ""), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, pStrucWeather.temp)
                Double.TryParse(words(4).Replace("wind=", ""), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, pStrucWeather.wind)
                Double.TryParse(words(5).Replace("gust=", ""), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, pStrucWeather.gust)
                Double.TryParse(words(6).Replace("rain=", ""), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, pStrucWeather.rain)
                Double.TryParse(words(7).Replace("light=", ""), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, pStrucWeather.light)
                Integer.TryParse(words(8).Replace("switch=", ""), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, pStrucWeather.switch)
                Integer.TryParse(words(9).Replace("safe=", ""), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, pStrucWeather.safe)
                Double.TryParse(words(10).Replace("hum=", ""), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, pStrucWeather.hum)
                Double.TryParse(words(11).Replace("dewp=", ""), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, pStrucWeather.dewp)
                pStrucWeather.AAGversion = "NEW"
                pStrucWeather.containsData = 1

                '1 means 100, bug AAG
                If pStrucWeather.hum = 1 Then
                    pStrucWeather.hum = 100
                End If
            End If

            executionTime = DateTime.UtcNow() - startExecution
            LogSessionEntry("DEBUG", "  ProcessAAGData" + executionTime.ToString, "", "ProcessAAGData", "WEATHER")

        Catch ex As Exception
            ProcessAAGData = "ProcessAAGData: " + ex.Message
            LogSessionEntry("ERROR", "ProcessAAGData: " + ex.Message, "", "ProcessAAGData", "WEATHER")
        End Try
    End Function
End Module
