Imports System.Diagnostics.Eventing.Reader
Imports System.Net.Mail
Imports System.Reflection.Emit
Imports System.Security.Cryptography
Imports System.Security.Cryptography.X509Certificates

Module ModAstroUtils
    'all timings in UTC !
    Public pAUtils As ASCOM.Astrometry.AstroUtils.AstroUtils
    Public pANova As ASCOM.Astrometry.NOVAS.NOVAS31
    Public pAUtil As ASCOM.Utilities.Util
    Public pATransform As ASCOM.Astrometry.Transform.Transform

    'only used for conversion from 2000 to topocentric, do not use in slews ect ... create local variable
    Public pRATargetTopocentric As Double
    Public pDECTargetTopocentric As Double

    'target object, corrected with actual telescope position coming from the mount FOLLOWING A CLOSED LOOP SLEW
    Public pRAMountActualTopocentric As Double
    Public pDECMountActualTopocentric As Double

    'focus star
    Public pRAFocusTopocentric As Double
    Public pDECFocusTopocentric As Double


    Public pLastCalculateEventTimesLowIntensity As Date

    'https://ascom-standards.org/Help/Developer/html/T_ASCOM_Utilities_Util.htm

    Public Structure StrucEventTimes
        Dim LST As Double
        Dim GST As Double
        Dim HA As Double

        Dim MoonIllumination As Double
        Dim MoonPhase As String
        Dim MoonPhaseShort As String
        Dim MoonAboveEventLimitMidnight As Boolean
        Dim MoonNbrRiseEvents As Integer
        Dim MoonNbrSetEvents As Integer
        Dim MoonRise1 As Double
        Dim MoonRise2 As Double
        'Dim MoonTransit As Double
        Dim MoonSettingRising As String
        Dim MoonSet1 As Double
        Dim MoonSet2 As Double
        Dim MoonRA As Double
        Dim MoonDEC As Double
        Dim MoonAlt As Double
        Dim MoonAz As Double
        Dim MoonCompassDirection As String
        Dim MoonCompassIndex As Integer
        Dim MoonSafetyStatus As String 'SAFE/UNSAFE/COOLDOWN
        Dim MoonCooldownStatus As String 'COOL/NOTHING

        Dim SunRise As Double
        Dim SunSet As Double
        Dim SunRA As Double
        Dim SunDEC As Double
        Dim SunAlt As Double
        Dim SunAz As Double
        'Dim SunTransit As Double
        Dim SunSettingRising As String
        Dim SunCompasDirection As String
        Dim SunSafetyStatus As String 'SAFE/UNSAFE

        Dim CivilTwilightEvening As Double
        Dim CivilTwilightMorning As Double
        Dim AmateurTwilightEvening As Double
        Dim AmateurTwilightMorning As Double
        Dim AstronomicalTwilightEvening As Double
        Dim AstronomicalTwilightMorning As Double

        Dim SunOpenRoof As String 'when roof is to open
        Dim SunDuskFlats As String 'start dusk flats
        Dim SunStartRun As String 'start observations
        Dim SunStopRun As String 'stop observations
        Dim SunDawnFlats As String 'start dawn flats
    End Structure

    Public pStructEventTimes As StrucEventTimes

    Public Structure StrucObject
        Dim LST As Double
        Dim GST As Double
        Dim HA As Double
        Dim ObjectRA2000 As Double
        Dim ObjectDEC2000 As Double
        Dim ObjectAlt As Double
        Dim ObjectAz As Double
        Dim ObjectCompasDirection As String
        Dim ObjectCompasIndex As Integer
    End Structure

    Public pStructObject As StrucObject

    Public Structure StrucMosaic
        Dim Center_RA2000 As Double
        Dim Center_DEC2000 As Double
        Dim Panel1_RA2000 As Double
        Dim Panel1_DEC2000 As Double
        Dim Panel2_RA2000 As Double
        Dim Panel2_DEC2000 As Double
        Dim Panel3_RA2000 As Double
        Dim Panel3_DEC2000 As Double
        Dim Panel4_RA2000 As Double
        Dim Panel4_DEC2000 As Double
    End Structure

    Public pStructMosaic As StrucMosaic


    Public Function CalculateEventTimes() As String
        Dim returnvalue_double As Double
        Dim AObjectMoon As ASCOM.Astrometry.Object3
        Dim AObjectSun As ASCOM.Astrometry.Object3
        Dim AObserver As ASCOM.Astrometry.Observer
        Dim ASkyPos As ASCOM.Astrometry.SkyPos
        Dim ATransform As ASCOM.Astrometry.Transform.Transform

        Dim Day, Month, Year As Integer
        Dim DayPlusOne As DateTime
        Dim MoonPhase As Double
        Dim Phase As String
        Dim EventTimes As ArrayList

        Dim startExecution As Date
        Dim executionTime As TimeSpan

        CalculateEventTimes = "OK"
        Try
            startExecution = DateTime.UtcNow()
            LogSessionEntry("DEBUG", "  CalculateEventTimes...", "", "CalculateEventTimes", "PROGRAM")

            ResetCalculateEventTimes()

            pAUtils = New ASCOM.Astrometry.AstroUtils.AstroUtils
            pANova = New ASCOM.Astrometry.NOVAS.NOVAS31
            pAUtil = New ASCOM.Utilities.Util
            ATransform = New ASCOM.Astrometry.Transform.Transform

            '-----------------------------------------------------
            'MOON
            '-----------------------------------------------------

            'Illumination
            pStructEventTimes.MoonIllumination = pAUtils.MoonIllumination(pAUtils.JulianDateUtc)

            'Phase
            MoonPhase = pAUtils.MoonPhase(pAUtils.JulianDateUtc)
            Phase = ""

            Select Case MoonPhase
                Case -180.0 To -135.0
                    Phase = "Full Moon"
                Case -135.0 To -90.0
                    Phase = "Waning Gibbous"
                Case -90.0 To -45.0
                    Phase = "Last Quarter"
                Case -45.0 To 0.0
                    Phase = "Waning Crescent"
                Case 0.0 To 45.0
                    Phase = "New Moon"
                Case 45.0 To 90.0
                    Phase = "Waxing Crescent"
                Case 90.0 To 135.0
                    Phase = "First Quarter"
                Case 135.0 To 180.0
                    Phase = "Waxing Gibbous"
            End Select
            pStructEventTimes.MoonPhase = Phase

            If MoonPhase < 0 Then
                pStructEventTimes.MoonPhaseShort = "(waning)"
            Else
                pStructEventTimes.MoonPhaseShort = "(waxing)"
            End If

            'Moonrise / set
            Day = DateTime.UtcNow.Day
            Month = DateTime.UtcNow.Month
            Year = DateTime.UtcNow.Year

            EventTimes = pAUtils.EventTimes(ASCOM.Astrometry.EventType.MoonRiseMoonSet, Day, Month, Year, My.Settings.sObserverLatitude, My.Settings.sObserverLongitude, 0)
            pStructEventTimes.MoonAboveEventLimitMidnight = Convert.ToBoolean(EventTimes(0))
            pStructEventTimes.MoonNbrRiseEvents = Convert.ToInt16(EventTimes(1))
            pStructEventTimes.MoonNbrSetEvents = Convert.ToInt16(EventTimes(2))

            Select Case pStructEventTimes.MoonNbrRiseEvents
                Case 1
                    pStructEventTimes.MoonRise1 = Convert.ToDouble(EventTimes(3))
                Case 2
                    pStructEventTimes.MoonRise2 = Convert.ToDouble(EventTimes(4))
            End Select

            If pStructEventTimes.MoonNbrRiseEvents = 0 Then
                Select Case pStructEventTimes.MoonNbrSetEvents
                    Case 1
                        pStructEventTimes.MoonSet1 = Convert.ToDouble(EventTimes(3))
                    Case 2
                        pStructEventTimes.MoonSet2 = Convert.ToDouble(EventTimes(4))
                End Select
            ElseIf pStructEventTimes.MoonNbrRiseEvents = 1 Then
                Select Case pStructEventTimes.MoonNbrSetEvents
                    Case 1
                        pStructEventTimes.MoonSet1 = Convert.ToDouble(EventTimes(4))
                    Case 2
                        pStructEventTimes.MoonSet2 = Convert.ToDouble(EventTimes(5))
                End Select
            ElseIf pStructEventTimes.MoonNbrRiseEvents = 2 Then
                Select Case pStructEventTimes.MoonNbrSetEvents
                    Case 1
                        pStructEventTimes.MoonSet1 = Convert.ToDouble(EventTimes(5))
                    Case 2
                        pStructEventTimes.MoonSet2 = Convert.ToDouble(EventTimes(6))
                End Select
            End If

            With AObjectMoon
                .Name = "Moon"
                .Number = CType(11, ASCOM.Astrometry.Body)
            End With

            With AObjectSun
                .Name = "Sun"
                .Number = CType(10, ASCOM.Astrometry.Body)
            End With

            With AObserver
                .Where = CType(1, ASCOM.Astrometry.ObserverLocation)
                .OnSurf.Height = My.Settings.sObserverHeight
                .OnSurf.Latitude = My.Settings.sObserverLatitude
                .OnSurf.Longitude = My.Settings.sObserverLongitude
                .OnSurf.Pressure = My.Settings.sObserverPressure
                .OnSurf.Temperature = My.Settings.sObserverTemperature
            End With

#Disable Warning BC42109 ' Variable is used before it has been assigned a value
#Disable Warning BC42108 ' Variable is passed by reference before it has been assigned a value
            returnvalue_double = pANova.Place(pAUtils.JulianDateUtc, AObjectMoon, AObserver, 0, CType(3, ASCOM.Astrometry.CoordSys), 0, ASkyPos)
#Enable Warning BC42108 ' Variable is passed by reference before it has been assigned a value
#Enable Warning BC42109 ' Variable is used before it has been assigned a value
            If returnvalue_double <> 0 Then
                LogSessionEntry("ERROR", "CalculateEventTimesMoon: " + Format(returnvalue_double), "", "CalculateEventTimesMoon", "PROGRAM")
                CalculateEventTimes = Format(returnvalue_double)
                Exit Function
            End If

            pStructEventTimes.MoonRA = ASkyPos.RA
            pStructEventTimes.MoonDEC = ASkyPos.Dec

            ATransform.SiteElevation = My.Settings.sObserverHeight
            ATransform.SiteLatitude = My.Settings.sObserverLatitude
            ATransform.SiteLongitude = My.Settings.sObserverLongitude
            ATransform.SiteTemperature = My.Settings.sObserverTemperature
            ATransform.SetTopocentric(ASkyPos.RA, ASkyPos.Dec)


            If My.Settings.sSimulatorMode = True And My.Settings.sDebugSunMoonCoordinates = True Then
                pStructEventTimes.MoonAlt = My.Settings.SDebugMoonAltitude
                pStructEventTimes.MoonSettingRising = My.Settings.SDebugMoonRisingSetting
                pStructEventTimes.MoonIllumination = My.Settings.sDebugMoonIllumination
            Else
                pStructEventTimes.MoonAlt = ATransform.ElevationTopocentric
                If pStructEventTimes.MoonAlt < ATransform.ElevationTopocentric Then
                    pStructEventTimes.MoonSettingRising = "RISING"
                Else
                    pStructEventTimes.MoonSettingRising = "SETTING"
                End If
            End If
            pStructEventTimes.MoonAz = ATransform.AzimuthTopocentric

            'Cardinal Direction 	Degree Direction
            'N 348.75 - 11.25
            'NNE 11.25 - 33.75
            'NE 33.75 - 56.25
            'ENE 56.25 - 78.75
            'E 78.75 - 101.25
            'ESE 101.25 - 123.75
            'SE 123.75 - 146.25
            'SSE 146.25 - 168.75
            'S 168.75 - 191.25
            'SSW 191.25 - 213.75
            'SW 213.75 - 236.25
            'WSW 236.25 - 258.75
            'W 258.75 - 281.25
            'WNW 281.25 - 303.75
            'NW 303.75 - 326.25
            'NNW 326.25 - 348.75

            Select Case ATransform.AzimuthTopocentric
                Case 348.75# To 11.25#
                    pStructEventTimes.MoonCompassDirection = "N"
                    pStructEventTimes.MoonCompassIndex = 16
                Case 11.25# To 33.75#
                    pStructEventTimes.MoonCompassDirection = "NNE"
                    pStructEventTimes.MoonCompassIndex = 15
                Case 33.75# To 56.25#
                    pStructEventTimes.MoonCompassDirection = "NE"
                    pStructEventTimes.MoonCompassIndex = 14
                Case 56.25# To 78.75#
                    pStructEventTimes.MoonCompassDirection = "ENE"
                    pStructEventTimes.MoonCompassIndex = 13
                Case 78.75# To 101.25#
                    pStructEventTimes.MoonCompassDirection = "E"
                    pStructEventTimes.MoonCompassIndex = 12
                Case 101.25# To 123.75#
                    pStructEventTimes.MoonCompassDirection = "ESE"
                    pStructEventTimes.MoonCompassIndex = 11
                Case 123.75# To 146.25#
                    pStructEventTimes.MoonCompassDirection = "SE"
                    pStructEventTimes.MoonCompassIndex = 10
                Case 146.25# To 168.75#
                    pStructEventTimes.MoonCompassDirection = "SSE"
                    pStructEventTimes.MoonCompassIndex = 9
                Case 168.75# To 191.25#
                    pStructEventTimes.MoonCompassDirection = "S"
                    pStructEventTimes.MoonCompassIndex = 8
                Case 191.25# To 213.75#
                    pStructEventTimes.MoonCompassDirection = "SSW"
                    pStructEventTimes.MoonCompassIndex = 7
                Case 213.75# To 236.25#
                    pStructEventTimes.MoonCompassDirection = "SW"
                    pStructEventTimes.MoonCompassIndex = 6
                Case 236.25# To 258.75#
                    pStructEventTimes.MoonCompassDirection = "WSW"
                    pStructEventTimes.MoonCompassIndex = 5
                Case 258.75# To 281.25#
                    pStructEventTimes.MoonCompassDirection = "W"
                    pStructEventTimes.MoonCompassIndex = 4
                Case 281.25# To 303.75#
                    pStructEventTimes.MoonCompassDirection = "WNW"
                    pStructEventTimes.MoonCompassIndex = 3
                Case 303.75# To 326.25#
                    pStructEventTimes.MoonCompassDirection = "NW"
                    pStructEventTimes.MoonCompassIndex = 2
                Case 326.25# To 348.75#
                    pStructEventTimes.MoonCompassDirection = "NNW"
                    pStructEventTimes.MoonCompassIndex = 1
            End Select

            'calculate moon safety status
            'If pStructEventTimes.MoonAlt <= My.Settings.sMoonAltitudeLimit Then
            'pStructEventTimes.MoonSafetyStatus = "SAFE"
            'Else
            'If pStructEventTimes.MoonIllumination * 100 <= My.Settings.sMoonPhaseLimit Then
            'pStructEventTimes.MoonSafetyStatus = "SAFE"
            'Else
            'pStructEventTimes.MoonSafetyStatus = "UNSAFE"
            'End If
            'End If

            'If pStructEventTimes.MoonIllumination * 100 <= My.Settings.sMoonPhaseLimit Then
            ' pStructEventTimes.MoonSafetyStatus = "SAFE"
            'Else
            '  If pStructEventTimes.MoonAlt <= My.Settings.sMoonAltitudeLimit Then 'And pStructEventTimes.MoonSettingRising = "SETTING"
            '   pStructEventTimes.MoonSafetyStatus = "SAFE"
            '  ElseIf My.Settings.sMoonStartCooldown = True And pStructEventTimes.MoonAlt <= My.Settings.sMoonCoolDownLimit And pStructEventTimes.MoonSettingRising = "SETTING" Then
            '   pStructEventTimes.MoonSafetyStatus = "COOLDOWN"
            '  Else
            '   pStructEventTimes.MoonSafetyStatus = "UNSAFE"
            '  End If
            'End If

            '&&
            If pStructEventTimes.MoonAlt <= My.Settings.sMoonAltitudeAlwaysSafe Then
                pStructEventTimes.MoonSafetyStatus = "SAFE"
            Else
                If pStructEventTimes.MoonIllumination * 100 <= My.Settings.sMoonPhaseAlwaysSafe Then
                    pStructEventTimes.MoonSafetyStatus = "SAFE"
                ElseIf pStructEventTimes.MoonIllumination * 100 <= My.Settings.sMoonPhaseLimitLow And pStructEventTimes.MoonAlt <= My.Settings.sMoonAltitudeLimitLow Then
                    pStructEventTimes.MoonSafetyStatus = "SAFE"
                ElseIf pStructEventTimes.MoonIllumination * 100 > My.Settings.sMoonPhaseLimitLow And pStructEventTimes.MoonAlt <= My.Settings.sMoonAltitudeLimitHigh Then
                    pStructEventTimes.MoonSafetyStatus = "SAFE"
                Else
                    pStructEventTimes.MoonSafetyStatus = "UNSAFE"
                End If
            End If

            If pStructEventTimes.MoonSafetyStatus = "SAFE" Then
                pStructEventTimes.MoonCooldownStatus = "COOL"
            Else
                If pStructEventTimes.MoonIllumination * 100 <= My.Settings.sMoonPhaseLimitLow And pStructEventTimes.MoonAlt <= My.Settings.sMoonStartCooldownLow Then
                    pStructEventTimes.MoonCooldownStatus = "COOL"
                ElseIf pStructEventTimes.MoonIllumination * 100 > My.Settings.sMoonPhaseLimitLow And pStructEventTimes.MoonAlt <= My.Settings.sMoonStartCooldownHigh Then
                    pStructEventTimes.MoonCooldownStatus = "COOL"
                Else
                    pStructEventTimes.MoonCooldownStatus = "NOTHING"
                End If
            End If


            '-----------------------------------------------------
            'GST / LST
            '-----------------------------------------------------

            'caculate LST / hour angle
            'LMST = GMST + (observer's east longitude)
            Dim GST, LST, DeltaT, JulianDateUTC, JulianInt, JulianFrac As Double
            JulianDateUTC = pAUtils.JulianDateUtc()
            DeltaT = pAUtils.DeltaUT(pAUtils.JulianDateUtc())
            JulianInt = Math.Truncate(pAUtils.JulianDateUtc())
            JulianFrac = pAUtils.JulianDateUtc() - JulianInt

            pANova.SiderealTime(JulianInt, JulianFrac, DeltaT, 0, 0, 0, GST)
            pStructEventTimes.GST = GST

            'calculate LST
            LST = GST + (My.Settings.sObserverLongitude * 0.06666666666666668)
            pStructEventTimes.LST = LST

            '-----------------------------------------------------
            'SUN
            '-----------------------------------------------------

            'Sunrise / set
            Day = DateTime.UtcNow.Day
            Month = DateTime.UtcNow.Month
            Year = DateTime.UtcNow.Year

            EventTimes = pAUtils.EventTimes(ASCOM.Astrometry.EventType.SunRiseSunset, Day, Month, Year, My.Settings.sObserverLatitude, My.Settings.sObserverLongitude, 0)

            pStructEventTimes.SunRise = Convert.ToDouble(EventTimes(3))
            pStructEventTimes.SunSet = Convert.ToDouble(EventTimes(4))

#Disable Warning BC42109 ' Variable is used before it has been assigned a value
            returnvalue_double = pANova.Place(pAUtils.JulianDateUtc, AObjectSun, AObserver, 0, CType(3, ASCOM.Astrometry.CoordSys), 0, ASkyPos)
#Enable Warning BC42109 ' Variable is used before it has been assigned a value
            If returnvalue_double <> 0 Then
                LogSessionEntry("ERROR", "CalculateEventTimesSun: " + Format(returnvalue_double), "", "CalculateEventTimesSun", "PROGRAM")
                CalculateEventTimes = Format(returnvalue_double)
                Exit Function
            End If

            pStructEventTimes.SunRA = ASkyPos.RA
            pStructEventTimes.SunDEC = ASkyPos.Dec

            ATransform.SiteElevation = My.Settings.sObserverHeight
            ATransform.SiteLatitude = My.Settings.sObserverLatitude
            ATransform.SiteLongitude = My.Settings.sObserverLongitude
            ATransform.SiteTemperature = My.Settings.sObserverTemperature
            ATransform.SetTopocentric(ASkyPos.RA, ASkyPos.Dec)

            If My.Settings.sSimulatorMode = True And My.Settings.sDebugSunMoonCoordinates = True Then
                pStructEventTimes.SunAlt = My.Settings.SDebugSunAltitude
                pStructEventTimes.SunSettingRising = My.Settings.SDebugSunRisingSetting
            Else
                pStructEventTimes.SunAlt = ATransform.ElevationTopocentric
                If pStructEventTimes.SunAlt < ATransform.ElevationTopocentric Then
                    pStructEventTimes.SunSettingRising = "RISING"
                Else
                    pStructEventTimes.SunSettingRising = "SETTING"
                End If
            End If
            pStructEventTimes.SunAz = ATransform.AzimuthTopocentric



            'Cardinal Direction 	Degree Direction
            'N 348.75 - 11.25
            'NNE 11.25 - 33.75
            'NE 33.75 - 56.25
            'ENE 56.25 - 78.75
            'E 78.75 - 101.25
            'ESE 101.25 - 123.75
            'SE 123.75 - 146.25
            'SSE 146.25 - 168.75
            'S 168.75 - 191.25
            'SSW 191.25 - 213.75
            'SW 213.75 - 236.25
            'WSW 236.25 - 258.75
            'W 258.75 - 281.25
            'WNW 281.25 - 303.75
            'NW 303.75 - 326.25
            'NNW 326.25 - 348.75

            Select Case ATransform.AzimuthTopocentric
                Case 348.75# To 11.25#
                    pStructEventTimes.SunCompasDirection = "N"
                Case 11.25# To 33.75#
                    pStructEventTimes.SunCompasDirection = "NNE"
                Case 33.75# To 56.25#
                    pStructEventTimes.SunCompasDirection = "NE"
                Case 56.25# To 78.75#
                    pStructEventTimes.SunCompasDirection = "ENE"
                Case 78.75# To 101.25#
                    pStructEventTimes.SunCompasDirection = "E"
                Case 101.25# To 123.75#
                    pStructEventTimes.SunCompasDirection = "ESE"
                Case 123.75# To 146.25#
                    pStructEventTimes.SunCompasDirection = "SE"
                Case 146.25# To 168.75#
                    pStructEventTimes.SunCompasDirection = "SSE"
                Case 168.75# To 191.25#
                    pStructEventTimes.SunCompasDirection = "S"
                Case 191.25# To 213.75#
                    pStructEventTimes.SunCompasDirection = "SSW"
                Case 213.75# To 236.25#
                    pStructEventTimes.SunCompasDirection = "SW"
                Case 236.25# To 258.75#
                    pStructEventTimes.SunCompasDirection = "WSW"
                Case 258.75# To 281.25#
                    pStructEventTimes.SunCompasDirection = "W"
                Case 281.25# To 303.75#
                    pStructEventTimes.SunCompasDirection = "WNW"
                Case 303.75# To 326.25#
                    pStructEventTimes.SunCompasDirection = "NW"
                Case 326.25# To 348.75#
                    pStructEventTimes.SunCompasDirection = "NNW"
            End Select

            'calculate Sun safety status
            If (pStructEventTimes.SunAlt > 0 And pStructEventTimes.SunSettingRising = "RISING") Or (pStructEventTimes.SunAlt >= My.Settings.sSunOpenRoof And pStructEventTimes.SunSettingRising = "SETTING") Then
                pStructEventTimes.SunSafetyStatus = "UNSAFE"
            Else
                pStructEventTimes.SunSafetyStatus = "SAFE"
            End If


            '-----------------------------------------------------
            ' TWILIGHT
            '-----------------------------------------------------
            ' civil twilight = 6°
            ' nautical twilight = 12°
            ' amateurAstronomicalTwilight = 15°
            ' astronomical twilight = 18°
            'Moonrise / set

            ' civil twilight = 6°
            Month = DateTime.UtcNow.Month
            Year = DateTime.UtcNow.Year
            EventTimes = pAUtils.EventTimes(ASCOM.Astrometry.EventType.CivilTwilight, Day, Month, Year, My.Settings.sObserverLatitude, My.Settings.sObserverLongitude, 0)
            pStructEventTimes.CivilTwilightEvening = Convert.ToDouble(EventTimes(4))

            DayPlusOne = DateTime.UtcNow.AddDays(1)
            Day = DayPlusOne.Day
            Month = DayPlusOne.Month
            Year = DayPlusOne.Year
            EventTimes = pAUtils.EventTimes(ASCOM.Astrometry.EventType.CivilTwilight, Day, Month, Year, My.Settings.sObserverLatitude, My.Settings.sObserverLongitude, 0)
            pStructEventTimes.CivilTwilightMorning = Convert.ToDouble(EventTimes(3))

            ' amateurAstronomicalTwilight = 15°
            Month = DateTime.UtcNow.Month
            Year = DateTime.UtcNow.Year
            EventTimes = pAUtils.EventTimes(ASCOM.Astrometry.EventType.AmateurAstronomicalTwilight, Day, Month, Year, My.Settings.sObserverLatitude, My.Settings.sObserverLongitude, 0)
            pStructEventTimes.AmateurTwilightEvening = Convert.ToDouble(EventTimes(4))

            DayPlusOne = DateTime.UtcNow.AddDays(1)
            Day = DayPlusOne.Day
            Month = DayPlusOne.Month
            Year = DayPlusOne.Year
            EventTimes = pAUtils.EventTimes(ASCOM.Astrometry.EventType.AmateurAstronomicalTwilight, Day, Month, Year, My.Settings.sObserverLatitude, My.Settings.sObserverLongitude, 0)
            pStructEventTimes.AmateurTwilightMorning = Convert.ToDouble(EventTimes(3))

            ' AstronomicalTwilight = 15°
            Month = DateTime.UtcNow.Month
            Year = DateTime.UtcNow.Year
            EventTimes = pAUtils.EventTimes(ASCOM.Astrometry.EventType.AstronomicalTwilight, Day, Month, Year, My.Settings.sObserverLatitude, My.Settings.sObserverLongitude, 0)
            pStructEventTimes.AstronomicalTwilightEvening = Convert.ToDouble(EventTimes(4))

            DayPlusOne = DateTime.UtcNow.AddDays(1)
            Day = DayPlusOne.Day
            Month = DayPlusOne.Month
            Year = DayPlusOne.Year
            EventTimes = pAUtils.EventTimes(ASCOM.Astrometry.EventType.AstronomicalTwilight, Day, Month, Year, My.Settings.sObserverLatitude, My.Settings.sObserverLongitude, 0)
            pStructEventTimes.AstronomicalTwilightMorning = Convert.ToDouble(EventTimes(3))


            'dispose of all
            'AUtils.Dispose()
            'ANova.Dispose()
            'AUtil.Dispose()
            'ATransform.Dispose()

            '--------------------------------------------------
            ' handle the visual aspect
            '--------------------------------------------------

            'LST / GST
            FrmMain.LblLST.Text = "LST: " + pAUtil.HoursToHMS(pStructEventTimes.LST)
            'FrmMain.LblGST.Text = "GST: " + pAUtil.HoursToHMS(pStructEventTimes.GST)

            'Moon
            FrmMain.LblPhaseMoon.Text = Format(pStructEventTimes.MoonIllumination * 100, "0") + "% " + pStructEventTimes.MoonPhaseShort
            If pStructEventTimes.MoonAboveEventLimitMidnight = True Then
                'moon is setting before it is rising
                FrmMain.LblRisingSettingMoon.Text = "Set:" + pAUtil.HoursToHMS(pStructEventTimes.MoonSet1) + " - Rise: " + pAUtil.HoursToHMS(pStructEventTimes.MoonRise1)
            Else
                'moon is rising before it is setting
                FrmMain.LblRisingSettingMoon.Text = "Rise: " + pAUtil.HoursToHM(pStructEventTimes.MoonRise1) + " - Set: " + pAUtil.HoursToHM(pStructEventTimes.MoonSet1)
            End If

            FrmMain.LblMoonSettingRising.Text = pStructEventTimes.MoonSettingRising

            FrmMain.LblRAMoon.Text = "RA: " + pAUtil.HoursToHMS(pStructEventTimes.MoonRA, "h ", "m ", "s ")
            FrmMain.LblDECMoon.Text = "DEC: " + pAUtil.DegreesToDMS(pStructEventTimes.MoonDEC, "° ", "' ", """ ")
            FrmMain.LblAltMoon.Text = pAUtil.DegreesToDM(pStructEventTimes.MoonAlt) + " " + pStructEventTimes.MoonCompassDirection

            If pStructEventTimes.MoonSafetyStatus = "SAFE" Then
                FrmMain.LblAltMoon.BackColor = ColorTranslator.FromHtml("#4cd137") 'green
                'ElseIf pStructEventTimes.MoonSafetyStatus = "UNSAFE" Or pStructEventTimes.MoonSafetyStatus = "COOLDOWN" Then
            ElseIf pStructEventTimes.MoonSafetyStatus = "UNSAFE" Then
                FrmMain.LblAltMoon.BackColor = ColorTranslator.FromHtml("#d63031") 'red
            Else
                FrmMain.LblAltMoon.BackColor = Color.Transparent
            End If

            'Sun
            FrmMain.LblRASun.Text = "RA: " + pAUtil.HoursToHMS(pStructEventTimes.SunRA, "h ", "m ", "s ")
            FrmMain.LblDECSun.Text = "DEC: " + pAUtil.DegreesToDMS(pStructEventTimes.SunDEC, "° ", "' ", """ ")
            FrmMain.LblAltSun.Text = pAUtil.DegreesToDM(pStructEventTimes.SunAlt) + " " + pStructEventTimes.SunCompasDirection

            FrmMain.LblRisingSettingSun.Text = "Rise: " + pAUtil.HoursToHM(pStructEventTimes.SunRise) + " - Set: " + pAUtil.HoursToHM(pStructEventTimes.SunSet)

            If pStructEventTimes.SunSafetyStatus = "SAFE" Then
                FrmMain.LblAltSun.BackColor = ColorTranslator.FromHtml("#4cd137") 'green
            ElseIf pStructEventTimes.SunSafetyStatus = "UNSAFE" Then
                FrmMain.LblAltSun.BackColor = ColorTranslator.FromHtml("#d63031") 'red
            Else
                FrmMain.LblAltSun.BackColor = Color.Transparent
            End If

            FrmMain.LblSunSettingRising.Text = pStructEventTimes.SunSettingRising

            FrmMain.LblCivilTwilight.Text = "Civil twilight:" + pAUtil.HoursToHM(pStructEventTimes.CivilTwilightEvening) + "-" + pAUtil.HoursToHM(pStructEventTimes.CivilTwilightMorning)
            'FrmMain.LblAmateurTwilight.Text = "Amateur:" + pAUtil.HoursToHM(pStructEventTimes.AmateurTwilightEvening) + "-" + pAUtil.HoursToHM(pStructEventTimes.AmateurTwilightMorning)
            FrmMain.LblAstronomicalTwilight.Text = "Astronomical twilight:" + pAUtil.HoursToHM(pStructEventTimes.AstronomicalTwilightEvening) + "-" + pAUtil.HoursToHM(pStructEventTimes.AstronomicalTwilightMorning)

            executionTime = DateTime.UtcNow() - startExecution
            LogSessionEntry("DEBUG", "  CalculateEventTimes: " + executionTime.ToString, "", "CalculateEventTimes", "PROGRAM")

        Catch ex As Exception
            CalculateEventTimes = "CalculateEventTimes: " + ex.Message
            LogSessionEntry("ERROR", "CalculateEventTimes: " + ex.Message, "", "CalculateEventTimes", "PROGRAM")
        End Try
    End Function


    Public Function CalculateEventTimesHighIntensity() As String
        'only calculate stuff that frequently changes and refresh every 10 seconds
        Dim returnvalue_short As Short
        Dim AObjectMoon As ASCOM.Astrometry.Object3
        Dim AObjectSun As ASCOM.Astrometry.Object3
        Dim AObserver As ASCOM.Astrometry.Observer
        Dim ASkyPos As ASCOM.Astrometry.SkyPos
        'Dim ATransform As ASCOM.Astrometry.Transform.Transform

        'Dim Day, Month, Year
        'Dim DayPlusOne
        Dim MoonPhase As Double
        Dim Phase As String
        'Dim EventTimes

        Dim startExecution As Date
        Dim executionTime As TimeSpan

        CalculateEventTimesHighIntensity = "OK"
        Try

            startExecution = DateTime.UtcNow()
            LogSessionEntry("DEBUG", "  CalculateEventTimesHighIntensity..." + executionTime.ToString, "", "CalculateEventTimesHighIntensity", "PROGRAM")

            pAUtils = New ASCOM.Astrometry.AstroUtils.AstroUtils
            pANova = New ASCOM.Astrometry.NOVAS.NOVAS31
            '    AUtil = New ASCOM.Utilities.Util
            pATransform = New ASCOM.Astrometry.Transform.Transform

            '-----------------------------------------------------
            'MOON
            '-----------------------------------------------------

            'Illumination
            pStructEventTimes.MoonIllumination = pAUtils.MoonIllumination(pAUtils.JulianDateUtc)

            'Phase
            MoonPhase = pAUtils.MoonPhase(pAUtils.JulianDateUtc)
            Phase = ""

            Select Case MoonPhase
                Case -180.0 To -135.0
                    Phase = "Full Moon"
                Case -135.0 To -90.0
                    Phase = "Waning Gibbous"
                Case -90.0 To -45.0
                    Phase = "Last Quarter"
                Case -45.0 To 0.0
                    Phase = "Waning Crescent"
                Case 0.0 To 45.0
                    Phase = "New Moon"
                Case 45.0 To 90.0
                    Phase = "Waxing Crescent"
                Case 90.0 To 135.0
                    Phase = "First Quarter"
                Case 135.0 To 180.0
                    Phase = "Waxing Gibbous"
            End Select
            pStructEventTimes.MoonPhase = Phase

            If MoonPhase < 0 Then
                pStructEventTimes.MoonPhaseShort = "(waning)"
            Else
                pStructEventTimes.MoonPhaseShort = "(waxing)"
            End If


            With AObjectMoon
                .Name = "Moon"
                .Number = CType(11, ASCOM.Astrometry.Body)
            End With

            With AObjectSun
                .Name = "Sun"
                .Number = CType(10, ASCOM.Astrometry.Body)
            End With

            With AObserver
                .Where = CType(1, ASCOM.Astrometry.ObserverLocation)
                .OnSurf.Height = My.Settings.sObserverHeight
                .OnSurf.Latitude = My.Settings.sObserverLatitude
                .OnSurf.Longitude = My.Settings.sObserverLongitude
                .OnSurf.Pressure = My.Settings.sObserverPressure
                .OnSurf.Temperature = My.Settings.sObserverTemperature
            End With

#Disable Warning BC42109 ' Variable is used before it has been assigned a value
#Disable Warning BC42108 ' Variable is passed by reference before it has been assigned a value
            returnvalue_short = pANova.Place(pAUtils.JulianDateUtc, AObjectMoon, AObserver, 0, CType(3, ASCOM.Astrometry.CoordSys), 0, ASkyPos)
#Enable Warning BC42108 ' Variable is passed by reference before it has been assigned a value
#Enable Warning BC42109 ' Variable is used before it has been assigned a value
            If returnvalue_short <> 0 Then
                LogSessionEntry("ERROR", "CalculateEventTimesMoon: " + returnvalue_short.ToString, "", "CalculateEventTimesMoon", "PROGRAM")
                CalculateEventTimesHighIntensity = returnvalue_short.ToString
                Exit Function
            End If

            pStructEventTimes.MoonRA = ASkyPos.RA
            pStructEventTimes.MoonDEC = ASkyPos.Dec

            pATransform.SiteElevation = My.Settings.sObserverHeight
            pATransform.SiteLatitude = My.Settings.sObserverLatitude
            pATransform.SiteLongitude = My.Settings.sObserverLongitude
            pATransform.SiteTemperature = My.Settings.sObserverTemperature
            pATransform.SetTopocentric(ASkyPos.RA, ASkyPos.Dec)


            If My.Settings.sSimulatorMode = True And My.Settings.sDebugSunMoonCoordinates = True Then
                pStructEventTimes.MoonAlt = My.Settings.SDebugMoonAltitude
                pStructEventTimes.MoonSettingRising = My.Settings.SDebugMoonRisingSetting
                pStructEventTimes.MoonIllumination = My.Settings.sDebugMoonIllumination
            Else
                If pATransform.AzimuthTopocentric > 180 Then
                    pStructEventTimes.MoonSettingRising = "SETTING"
                Else
                    pStructEventTimes.MoonSettingRising = "RISING"
                End If
                pStructEventTimes.MoonAlt = pATransform.ElevationTopocentric
                pStructEventTimes.MoonAz = pATransform.AzimuthTopocentric
            End If

            'Cardinal Direction 	Degree Direction
            'N 348.75 - 11.25
            'NNE 11.25 - 33.75
            'NE 33.75 - 56.25
            'ENE 56.25 - 78.75
            'E 78.75 - 101.25
            'ESE 101.25 - 123.75
            'SE 123.75 - 146.25
            'SSE 146.25 - 168.75
            'S 168.75 - 191.25
            'SSW 191.25 - 213.75
            'SW 213.75 - 236.25
            'WSW 236.25 - 258.75
            'W 258.75 - 281.25
            'WNW 281.25 - 303.75
            'NW 303.75 - 326.25
            'NNW 326.25 - 348.75

            Select Case pATransform.AzimuthTopocentric
                Case 348.75# To 11.25#
                    pStructEventTimes.MoonCompassDirection = "N"
                    pStructEventTimes.MoonCompassIndex = 16
                Case 11.25# To 33.75#
                    pStructEventTimes.MoonCompassDirection = "NNE"
                    pStructEventTimes.MoonCompassIndex = 15
                Case 33.75# To 56.25#
                    pStructEventTimes.MoonCompassDirection = "NE"
                    pStructEventTimes.MoonCompassIndex = 14
                Case 56.25# To 78.75#
                    pStructEventTimes.MoonCompassDirection = "ENE"
                    pStructEventTimes.MoonCompassIndex = 13
                Case 78.75# To 101.25#
                    pStructEventTimes.MoonCompassDirection = "E"
                    pStructEventTimes.MoonCompassIndex = 12
                Case 101.25# To 123.75#
                    pStructEventTimes.MoonCompassDirection = "ESE"
                    pStructEventTimes.MoonCompassIndex = 11
                Case 123.75# To 146.25#
                    pStructEventTimes.MoonCompassDirection = "SE"
                    pStructEventTimes.MoonCompassIndex = 10
                Case 146.25# To 168.75#
                    pStructEventTimes.MoonCompassDirection = "SSE"
                    pStructEventTimes.MoonCompassIndex = 9
                Case 168.75# To 191.25#
                    pStructEventTimes.MoonCompassDirection = "S"
                    pStructEventTimes.MoonCompassIndex = 8
                Case 191.25# To 213.75#
                    pStructEventTimes.MoonCompassDirection = "SSW"
                    pStructEventTimes.MoonCompassIndex = 7
                Case 213.75# To 236.25#
                    pStructEventTimes.MoonCompassDirection = "SW"
                    pStructEventTimes.MoonCompassIndex = 6
                Case 236.25# To 258.75#
                    pStructEventTimes.MoonCompassDirection = "WSW"
                    pStructEventTimes.MoonCompassIndex = 5
                Case 258.75# To 281.25#
                    pStructEventTimes.MoonCompassDirection = "W"
                    pStructEventTimes.MoonCompassIndex = 4
                Case 281.25# To 303.75#
                    pStructEventTimes.MoonCompassDirection = "WNW"
                    pStructEventTimes.MoonCompassIndex = 3
                Case 303.75# To 326.25#
                    pStructEventTimes.MoonCompassDirection = "NW"
                    pStructEventTimes.MoonCompassIndex = 2
                Case 326.25# To 348.75#
                    pStructEventTimes.MoonCompassDirection = "NNW"
                    pStructEventTimes.MoonCompassIndex = 1
            End Select

            'calculate moon safety status
            '&&
            If pStructEventTimes.MoonAlt <= My.Settings.sMoonAltitudeAlwaysSafe Then
                pStructEventTimes.MoonSafetyStatus = "SAFE"
            Else
                If pStructEventTimes.MoonIllumination * 100 <= My.Settings.sMoonPhaseAlwaysSafe Then
                    pStructEventTimes.MoonSafetyStatus = "SAFE"
                ElseIf pStructEventTimes.MoonIllumination * 100 <= My.Settings.sMoonPhaseLimitLow And pStructEventTimes.MoonAlt <= My.Settings.sMoonAltitudeLimitLow Then
                    pStructEventTimes.MoonSafetyStatus = "SAFE"
                ElseIf pStructEventTimes.MoonIllumination * 100 > My.Settings.sMoonPhaseLimitLow And pStructEventTimes.MoonAlt <= My.Settings.sMoonAltitudeLimitHigh Then
                    pStructEventTimes.MoonSafetyStatus = "SAFE"
                Else
                    pStructEventTimes.MoonSafetyStatus = "UNSAFE"
                End If
            End If

            If pStructEventTimes.MoonSafetyStatus = "SAFE" Then
                pStructEventTimes.MoonCooldownStatus = "COOL"
            Else
                If pStructEventTimes.MoonIllumination * 100 <= My.Settings.sMoonPhaseLimitLow And pStructEventTimes.MoonAlt <= My.Settings.sMoonStartCooldownLow Then
                    pStructEventTimes.MoonCooldownStatus = "COOL"
                ElseIf pStructEventTimes.MoonIllumination * 100 > My.Settings.sMoonPhaseLimitLow And pStructEventTimes.MoonAlt <= My.Settings.sMoonStartCooldownHigh Then
                    pStructEventTimes.MoonCooldownStatus = "COOL"
                Else
                    pStructEventTimes.MoonCooldownStatus = "NOTHING"
                End If
            End If

            '-----------------------------------------------------
            'GST / LST
            '-----------------------------------------------------

            'caculate LST / hour angle
            'LMST = GMST + (observer's east longitude)
            Dim GST, LST, DeltaT, JulianDateUTC, JulianInt, JulianFrac As Double
            JulianDateUTC = pAUtils.JulianDateUtc()
            DeltaT = pAUtils.DeltaUT(pAUtils.JulianDateUtc())
            JulianInt = Math.Truncate(pAUtils.JulianDateUtc())
            JulianFrac = pAUtils.JulianDateUtc() - JulianInt

            pANova.SiderealTime(JulianInt, JulianFrac, DeltaT, 0, 0, 0, GST)
            pStructEventTimes.GST = GST

            'calculate LST
            LST = GST + (My.Settings.sObserverLongitude * 0.06666666666666668)
            pStructEventTimes.LST = LST

            '-----------------------------------------------------
            'SUN
            '-----------------------------------------------------
#Disable Warning BC42109 ' Variable is used before it has been assigned a value
            returnvalue_short = pANova.Place(pAUtils.JulianDateUtc, AObjectSun, AObserver, 0, CType(3, ASCOM.Astrometry.CoordSys), 0, ASkyPos)
#Enable Warning BC42109 ' Variable is used before it has been assigned a value
            If returnvalue_short <> 0 Then
                LogSessionEntry("ERROR", "CalculateEventTimesSun: " + returnvalue_short.ToString, "", "CalculateEventTimesSun", "PROGRAM")
                CalculateEventTimesHighIntensity = returnvalue_short.ToString
                Exit Function
            End If

            pStructEventTimes.SunRA = ASkyPos.RA
            pStructEventTimes.SunDEC = ASkyPos.Dec

            pATransform.SiteElevation = My.Settings.sObserverHeight
            pATransform.SiteLatitude = My.Settings.sObserverLatitude
            pATransform.SiteLongitude = My.Settings.sObserverLongitude
            pATransform.SiteTemperature = My.Settings.sObserverTemperature
            pATransform.SetTopocentric(ASkyPos.RA, ASkyPos.Dec)

            If My.Settings.sSimulatorMode = True And My.Settings.sDebugSunMoonCoordinates = True Then
                pStructEventTimes.SunAlt = My.Settings.SDebugSunAltitude
                pStructEventTimes.SunSettingRising = My.Settings.SDebugSunRisingSetting
            Else
                If pATransform.AzimuthTopocentric > 180 Then
                    pStructEventTimes.SunSettingRising = "SETTING"
                Else
                    pStructEventTimes.SunSettingRising = "RISING"
                End If
                pStructEventTimes.SunAlt = pATransform.ElevationTopocentric
                pStructEventTimes.SunAz = pATransform.AzimuthTopocentric
            End If



            'Cardinal Direction 	Degree Direction
            'N 348.75 - 11.25
            'NNE 11.25 - 33.75
            'NE 33.75 - 56.25
            'ENE 56.25 - 78.75
            'E 78.75 - 101.25
            'ESE 101.25 - 123.75
            'SE 123.75 - 146.25
            'SSE 146.25 - 168.75
            'S 168.75 - 191.25
            'SSW 191.25 - 213.75
            'SW 213.75 - 236.25
            'WSW 236.25 - 258.75
            'W 258.75 - 281.25
            'WNW 281.25 - 303.75
            'NW 303.75 - 326.25
            'NNW 326.25 - 348.75

            Select Case pATransform.AzimuthTopocentric
                Case 348.75# To 11.25#
                    pStructEventTimes.SunCompasDirection = "N"
                Case 11.25# To 33.75#
                    pStructEventTimes.SunCompasDirection = "NNE"
                Case 33.75# To 56.25#
                    pStructEventTimes.SunCompasDirection = "NE"
                Case 56.25# To 78.75#
                    pStructEventTimes.SunCompasDirection = "ENE"
                Case 78.75# To 101.25#
                    pStructEventTimes.SunCompasDirection = "E"
                Case 101.25# To 123.75#
                    pStructEventTimes.SunCompasDirection = "ESE"
                Case 123.75# To 146.25#
                    pStructEventTimes.SunCompasDirection = "SE"
                Case 146.25# To 168.75#
                    pStructEventTimes.SunCompasDirection = "SSE"
                Case 168.75# To 191.25#
                    pStructEventTimes.SunCompasDirection = "S"
                Case 191.25# To 213.75#
                    pStructEventTimes.SunCompasDirection = "SSW"
                Case 213.75# To 236.25#
                    pStructEventTimes.SunCompasDirection = "SW"
                Case 236.25# To 258.75#
                    pStructEventTimes.SunCompasDirection = "WSW"
                Case 258.75# To 281.25#
                    pStructEventTimes.SunCompasDirection = "W"
                Case 281.25# To 303.75#
                    pStructEventTimes.SunCompasDirection = "WNW"
                Case 303.75# To 326.25#
                    pStructEventTimes.SunCompasDirection = "NW"
                Case 326.25# To 348.75#
                    pStructEventTimes.SunCompasDirection = "NNW"
            End Select

            'calculate Sun safety status
            If (pStructEventTimes.SunAlt > 0 And pStructEventTimes.SunSettingRising = "RISING") Or (pStructEventTimes.SunAlt >= My.Settings.sSunOpenRoof And pStructEventTimes.SunSettingRising = "SETTING") Then
                pStructEventTimes.SunSafetyStatus = "UNSAFE"
            Else
                pStructEventTimes.SunSafetyStatus = "SAFE"
            End If


            '--------------------------------------------------
            ' handle the visual aspect
            '--------------------------------------------------

            'LST / GST
            FrmMain.LblLST.Text = "LST: " + pAUtil.HoursToHMS(pStructEventTimes.LST)
            'FrmMain.LblGST.Text = "GST: " + pAUtil.HoursToHMS(pStructEventTimes.GST)

            'Moon
            FrmMain.LblPhaseMoon.Text = Format(pStructEventTimes.MoonIllumination * 100, "0") + "% " + pStructEventTimes.MoonPhaseShort
            FrmMain.LblMoonSettingRising.Text = pStructEventTimes.MoonSettingRising

            FrmMain.LblRAMoon.Text = "RA: " + pAUtil.HoursToHMS(pStructEventTimes.MoonRA, "h ", "m ", "s ")
            FrmMain.LblDECMoon.Text = "DEC: " + pAUtil.DegreesToDMS(pStructEventTimes.MoonDEC, "° ", "' ", """ ")
            FrmMain.LblAltMoon.Text = pAUtil.DegreesToDM(pStructEventTimes.MoonAlt) + " " + pStructEventTimes.MoonCompassDirection

            If pStructEventTimes.MoonSafetyStatus = "SAFE" Then
                FrmMain.LblAltMoon.BackColor = ColorTranslator.FromHtml("#4cd137") 'green
                'ElseIf pStructEventTimes.MoonSafetyStatus = "UNSAFE" Or pStructEventTimes.MoonSafetyStatus = "COOLDOWN" Then
            ElseIf pStructEventTimes.MoonSafetyStatus = "UNSAFE" Then
                FrmMain.LblAltMoon.BackColor = ColorTranslator.FromHtml("#d63031") 'red
            Else
                FrmMain.LblAltMoon.BackColor = Color.Transparent
            End If

            'Sun
            FrmMain.LblRASun.Text = "RA: " + pAUtil.HoursToHMS(pStructEventTimes.SunRA, "h ", "m ", "s ")
            FrmMain.LblDECSun.Text = "DEC: " + pAUtil.DegreesToDMS(pStructEventTimes.SunDEC, "° ", "' ", """ ")
            FrmMain.LblAltSun.Text = pAUtil.DegreesToDM(pStructEventTimes.SunAlt) + " " + pStructEventTimes.SunCompasDirection

            If pStructEventTimes.SunSafetyStatus = "SAFE" Then
                FrmMain.LblAltSun.BackColor = ColorTranslator.FromHtml("#4cd137") 'green
            ElseIf pStructEventTimes.SunSafetyStatus = "UNSAFE" Then
                FrmMain.LblAltSun.BackColor = ColorTranslator.FromHtml("#d63031") 'red
            Else
                FrmMain.LblAltSun.BackColor = Color.Transparent
            End If

            FrmMain.LblSunSettingRising.Text = pStructEventTimes.SunSettingRising

            executionTime = DateTime.UtcNow() - startExecution
            LogSessionEntry("DEBUG", "  CalculateEventTimesHighIntensity: " + executionTime.ToString, "", "CalculateEventTimesHighIntensity", "PROGRAM")

        Catch ex As Exception
            CalculateEventTimesHighIntensity = "CalculateEventTimesHighIntensity: " + ex.Message
            LogSessionEntry("ERROR", "CalculateEventTimesHighIntensity: " + ex.Message, "", "CalculateEventTimesHighIntensity", "PROGRAM")
        End Try
    End Function



    Public Function CalculateEventTimesLowIntensity() As String
        Dim returnvalue As String
        'Dim AObjectMoon As ASCOM.Astrometry.Object3
        'Dim AObjectSun As ASCOM.Astrometry.Object3
        'Dim AObserver As ASCOM.Astrometry.Observer
        'Dim ASkyPos As ASCOM.Astrometry.SkyPos
        'Dim ATransform As ASCOM.Astrometry.Transform.Transform

        Dim Day, Month, Year As Integer
        Dim DayPlusOne As DateTime
        'Dim MoonPhase, Phase
        Dim EventTimes As ArrayList

        Dim startExecution As Date
        Dim executionTime As TimeSpan

        CalculateEventTimesLowIntensity = "OK"
        Try

            startExecution = DateTime.UtcNow()
            LogSessionEntry("DEBUG", "  CalculateEventTimesLowIntensity...", "", "CalculateEventTimesLowIntensity", "PROGRAM")

            'ResetCalculateEventTimes() 'only in low intensity

            pAUtils = New ASCOM.Astrometry.AstroUtils.AstroUtils
            'ANova = New ASCOM.Astrometry.NOVAS.NOVAS31
            pAUtil = New ASCOM.Utilities.Util
            'ATransform = New ASCOM.Astrometry.Transform.Transform

            '-----------------------------------------------------
            'MOON
            '-----------------------------------------------------
            'Moonrise / set
            Day = DateTime.UtcNow.Day
            Month = DateTime.UtcNow.Month
            Year = DateTime.UtcNow.Year

            EventTimes = pAUtils.EventTimes(ASCOM.Astrometry.EventType.MoonRiseMoonSet, Day, Month, Year, My.Settings.sObserverLatitude, My.Settings.sObserverLongitude, 0)
            pStructEventTimes.MoonAboveEventLimitMidnight = Convert.ToBoolean(EventTimes(0))
            pStructEventTimes.MoonNbrRiseEvents = Convert.ToInt16(EventTimes(1))
            pStructEventTimes.MoonNbrSetEvents = Convert.ToInt16(EventTimes(2))

            Select Case pStructEventTimes.MoonNbrRiseEvents
                Case 1
                    pStructEventTimes.MoonRise1 = Convert.ToDouble(EventTimes(3))
                Case 2
                    pStructEventTimes.MoonRise2 = Convert.ToDouble(EventTimes(4))
            End Select

            If pStructEventTimes.MoonNbrRiseEvents = 0 Then
                Select Case pStructEventTimes.MoonNbrSetEvents
                    Case 1
                        pStructEventTimes.MoonSet1 = Convert.ToDouble(EventTimes(3))
                    Case 2
                        pStructEventTimes.MoonSet2 = Convert.ToDouble(EventTimes(4))
                End Select
            ElseIf pStructEventTimes.MoonNbrRiseEvents = 1 Then
                Select Case pStructEventTimes.MoonNbrSetEvents
                    Case 1
                        pStructEventTimes.MoonSet1 = Convert.ToDouble(EventTimes(4))
                    Case 2
                        pStructEventTimes.MoonSet2 = Convert.ToDouble(EventTimes(5))
                End Select
            ElseIf pStructEventTimes.MoonNbrRiseEvents = 2 Then
                Select Case pStructEventTimes.MoonNbrSetEvents
                    Case 1
                        pStructEventTimes.MoonSet1 = Convert.ToDouble(EventTimes(5))
                    Case 2
                        pStructEventTimes.MoonSet2 = Convert.ToDouble(EventTimes(6))
                End Select
            End If


            '-----------------------------------------------------
            'SUN
            '-----------------------------------------------------

            'Sunrise / set
            Day = DateTime.UtcNow.Day
            Month = DateTime.UtcNow.Month
            Year = DateTime.UtcNow.Year

            EventTimes = pAUtils.EventTimes(ASCOM.Astrometry.EventType.SunRiseSunset, Day, Month, Year, My.Settings.sObserverLatitude, My.Settings.sObserverLongitude, 0)

            pStructEventTimes.SunRise = Convert.ToDouble(EventTimes(3))
            pStructEventTimes.SunSet = Convert.ToDouble(EventTimes(4))

            '-----------------------------------------------------
            ' TWILIGHT
            '-----------------------------------------------------
            ' civil twilight = 6°
            ' nautical twilight = 12°
            ' amateurAstronomicalTwilight = 15°
            ' astronomical twilight = 18°
            'Moonrise / set

            ' civil twilight = 6°
            Month = DateTime.UtcNow.Month
            Year = DateTime.UtcNow.Year
            EventTimes = pAUtils.EventTimes(ASCOM.Astrometry.EventType.CivilTwilight, Day, Month, Year, My.Settings.sObserverLatitude, My.Settings.sObserverLongitude, 0)
            pStructEventTimes.CivilTwilightEvening = Convert.ToDouble(EventTimes(4))

            DayPlusOne = DateTime.UtcNow.AddDays(1)
            Day = DayPlusOne.Day
            Month = DayPlusOne.Month
            Year = DayPlusOne.Year
            EventTimes = pAUtils.EventTimes(ASCOM.Astrometry.EventType.CivilTwilight, Day, Month, Year, My.Settings.sObserverLatitude, My.Settings.sObserverLongitude, 0)
            pStructEventTimes.CivilTwilightMorning = Convert.ToDouble(EventTimes(3))

            ' amateurAstronomicalTwilight = 15°
            Month = DateTime.UtcNow.Month
            Year = DateTime.UtcNow.Year
            EventTimes = pAUtils.EventTimes(ASCOM.Astrometry.EventType.AmateurAstronomicalTwilight, Day, Month, Year, My.Settings.sObserverLatitude, My.Settings.sObserverLongitude, 0)
            pStructEventTimes.AmateurTwilightEvening = Convert.ToDouble(EventTimes(4))

            DayPlusOne = DateTime.UtcNow.AddDays(1)
            Day = DayPlusOne.Day
            Month = DayPlusOne.Month
            Year = DayPlusOne.Year
            EventTimes = pAUtils.EventTimes(ASCOM.Astrometry.EventType.AmateurAstronomicalTwilight, Day, Month, Year, My.Settings.sObserverLatitude, My.Settings.sObserverLongitude, 0)
            pStructEventTimes.AmateurTwilightMorning = Convert.ToDouble(EventTimes(3))

            ' AstronomicalTwilight = 15°
            Month = DateTime.UtcNow.Month
            Year = DateTime.UtcNow.Year
            EventTimes = pAUtils.EventTimes(ASCOM.Astrometry.EventType.AstronomicalTwilight, Day, Month, Year, My.Settings.sObserverLatitude, My.Settings.sObserverLongitude, 0)
            pStructEventTimes.AstronomicalTwilightEvening = Convert.ToDouble(EventTimes(4))

            DayPlusOne = DateTime.UtcNow.AddDays(1)
            Day = DayPlusOne.Day
            Month = DayPlusOne.Month
            Year = DayPlusOne.Year
            EventTimes = pAUtils.EventTimes(ASCOM.Astrometry.EventType.AstronomicalTwilight, Day, Month, Year, My.Settings.sObserverLatitude, My.Settings.sObserverLongitude, 0)
            pStructEventTimes.AstronomicalTwilightMorning = Convert.ToDouble(EventTimes(3))

            '--------------------------------------------------
            ' handle the visual aspect
            '--------------------------------------------------
            'Moon
            If pStructEventTimes.MoonAboveEventLimitMidnight = True Then
                'moon is setting before it is rising
                FrmMain.LblRisingSettingMoon.Text = "Set: " + pAUtil.HoursToHM(pStructEventTimes.MoonSet1) + " - Rise: " + pAUtil.HoursToHM(pStructEventTimes.MoonRise1)
            Else
                'moon is rising before it is setting
                FrmMain.LblRisingSettingMoon.Text = "Rise: " + pAUtil.HoursToHM(pStructEventTimes.MoonRise1) + " - Set: " + pAUtil.HoursToHM(pStructEventTimes.MoonSet1)
            End If

            'Sun
            FrmMain.LblRisingSettingSun.Text = "Rise: " + pAUtil.HoursToHM(pStructEventTimes.SunRise) + " - Set: " + pAUtil.HoursToHM(pStructEventTimes.SunSet)

            FrmMain.LblCivilTwilight.Text = "Civil twilight: " + pAUtil.HoursToHM(pStructEventTimes.CivilTwilightEvening) + "-" + pAUtil.HoursToHM(pStructEventTimes.CivilTwilightMorning)
            'FrmMain.LblAmateurTwilight.Text = "Amateur: " + pAUtil.HoursToHM(pStructEventTimes.AmateurTwilightEvening) + "-" + pAUtil.HoursToHM(pStructEventTimes.AmateurTwilightMorning)
            FrmMain.LblAstronomicalTwilight.Text = "Astronomical twilight: " + pAUtil.HoursToHM(pStructEventTimes.AstronomicalTwilightEvening) + "-" + pAUtil.HoursToHM(pStructEventTimes.AstronomicalTwilightMorning)

            returnvalue = CalculateSunMoonTimes("SUN")
            If returnvalue <> "OK" Then
                CalculateEventTimesLowIntensity = returnvalue
                Exit Function
            End If

            executionTime = DateTime.UtcNow() - startExecution
            LogSessionEntry("DEBUG", "  CalculateEventTimesLowIntensity: " + executionTime.ToString, "", "CalculateEventTimesLowIntensity", "PROGRAM")

        Catch ex As Exception
            CalculateEventTimesLowIntensity = "CalculateEventTimesLowIntensity: " + ex.Message
            LogSessionEntry("ERROR", "CalculateEventTimesLowIntensity: " + ex.Message, "", "CalculateEventTimesLowIntensity", "PROGRAM")
        End Try
    End Function



    Private Sub ResetCalculateEventTimes()
        Dim startExecution As Date
        Dim executionTime As TimeSpan

        startExecution = DateTime.UtcNow()
        LogSessionEntry("DEBUG", "  ResetCalculateEventTimes...", "", "ResetCalculateEventTimes", "PROGRAM")

        With pStructEventTimes
            .LST = Nothing
            .GST = Nothing
            .HA = Nothing

            .MoonIllumination = Nothing
            .MoonPhase = Nothing
            .MoonPhaseShort = Nothing
            .MoonAboveEventLimitMidnight = Nothing
            .MoonNbrRiseEvents = Nothing
            .MoonNbrSetEvents = Nothing
            .MoonRise1 = Nothing
            .MoonRise2 = Nothing
            .MoonSet1 = Nothing
            .MoonSet2 = Nothing
            .MoonRA = Nothing
            .MoonDEC = Nothing
            .MoonAlt = Nothing
            .MoonAz = Nothing
            .MoonSafetyStatus = Nothing

            .SunRise = Nothing
            .SunSet = Nothing
            .SunRA = Nothing
            .SunDEC = Nothing
            .SunAlt = Nothing
            .SunAz = Nothing
            .SunCompasDirection = Nothing
            .SunSafetyStatus = Nothing

            .CivilTwilightEvening = Nothing
            .CivilTwilightMorning = Nothing
            .AmateurTwilightEvening = Nothing
            .AmateurTwilightMorning = Nothing
            .AstronomicalTwilightEvening = Nothing
            .AstronomicalTwilightMorning = Nothing
        End With

        executionTime = DateTime.UtcNow() - startExecution
        LogSessionEntry("DEBUG", "  ResetCalculateEventTimes: " + executionTime.ToString, "", "ResetCalculateEventTimes", "PROGRAM")
    End Sub

    '------------------------------------------------------------------------------------------------------------------------------------------
    ' OBJECT
    '------------------------------------------------------------------------------------------------------------------------------------------

    Public Function CalculateObject(vRA2000 As Double, vDEC2000 As Double) As String
        Dim AObserver As ASCOM.Astrometry.Observer
        'Dim ATransform As ASCOM.Astrometry.Transform.Transform
        Dim ATransform = New ASCOM.Astrometry.Transform.Transform
        Dim startExecution As Date
        Dim executionTime As TimeSpan

        CalculateObject = "OK"
        Try
            startExecution = DateTime.UtcNow()
            LogSessionEntry("DEBUG", "  CalculateObject...", "", "CalculateObject", "PROGRAM")

            With AObserver
                .Where = CType(1, ASCOM.Astrometry.ObserverLocation)
                .OnSurf.Height = My.Settings.sObserverHeight
                .OnSurf.Latitude = My.Settings.sObserverLatitude
                .OnSurf.Longitude = My.Settings.sObserverLongitude
                .OnSurf.Pressure = My.Settings.sObserverPressure
                .OnSurf.Temperature = My.Settings.sObserverTemperature
            End With

            ResetCalculateObject()

            pAUtils = New ASCOM.Astrometry.AstroUtils.AstroUtils
            pANova = New ASCOM.Astrometry.NOVAS.NOVAS31
            pAUtil = New ASCOM.Utilities.Util
            'ATransform = New ASCOM.Astrometry.Transform.Transform

            ATransform.SiteElevation = My.Settings.sObserverHeight
            ATransform.SiteLatitude = My.Settings.sObserverLatitude
            ATransform.SiteLongitude = My.Settings.sObserverLongitude
            ATransform.SiteTemperature = My.Settings.sObserverTemperature
            ATransform.SetJ2000(vRA2000, vDEC2000)

            pStructObject.ObjectAlt = ATransform.ElevationTopocentric
            pStructObject.ObjectAz = ATransform.AzimuthTopocentric

            'Cardinal Direction 	Degree Direction
            '16 N 348.75 - 11.25
            '15 NNE 11.25 - 33.75
            '14 NE 33.75 - 56.25
            '13 ENE 56.25 - 78.75
            '12 E 78.75 - 101.25
            '11 ESE 101.25 - 123.75
            '10 SE 123.75 - 146.25
            '9 SSE 146.25 - 168.75
            '8 S 168.75 - 191.25
            '7 SSW 191.25 - 213.75
            '6 SW 213.75 - 236.25
            '5 WSW 236.25 - 258.75
            '4 W 258.75 - 281.25
            '3 WNW 281.25 - 303.75
            '2 NW 303.75 - 326.25
            '1 NNW 326.25 - 348.75

            Select Case ATransform.AzimuthTopocentric
                Case 348.75# To 360.0#
                    pStructObject.ObjectCompasDirection = "N"
                    pStructObject.ObjectCompasIndex = 16
                Case 0.00# To 11.25#
                    pStructObject.ObjectCompasDirection = "N"
                    pStructObject.ObjectCompasIndex = 16
                Case 11.25# To 33.75#
                    pStructObject.ObjectCompasDirection = "NNE"
                    pStructObject.ObjectCompasIndex = 15
                Case 33.75# To 56.25#
                    pStructObject.ObjectCompasDirection = "NE"
                    pStructObject.ObjectCompasIndex = 14
                Case 56.25# To 78.75#
                    pStructObject.ObjectCompasDirection = "ENE"
                    pStructObject.ObjectCompasIndex = 13
                Case 78.75# To 101.25#
                    pStructObject.ObjectCompasDirection = "E"
                    pStructObject.ObjectCompasIndex = 12
                Case 101.25# To 123.75#
                    pStructObject.ObjectCompasDirection = "ESE"
                    pStructObject.ObjectCompasIndex = 11
                Case 123.75# To 146.25#
                    pStructObject.ObjectCompasDirection = "SE"
                    pStructObject.ObjectCompasIndex = 10
                Case 146.25# To 168.75#
                    pStructObject.ObjectCompasDirection = "SSE"
                    pStructObject.ObjectCompasIndex = 9
                Case 168.75# To 191.25#
                    pStructObject.ObjectCompasDirection = "S"
                    pStructObject.ObjectCompasIndex = 8
                Case 191.25# To 213.75#
                    pStructObject.ObjectCompasDirection = "SSW"
                    pStructObject.ObjectCompasIndex = 7
                Case 213.75# To 236.25#
                    pStructObject.ObjectCompasDirection = "SW"
                    pStructObject.ObjectCompasIndex = 6
                Case 236.25# To 258.75#
                    pStructObject.ObjectCompasDirection = "WSW"
                    pStructObject.ObjectCompasIndex = 5
                Case 258.75# To 281.25#
                    pStructObject.ObjectCompasDirection = "W"
                    pStructObject.ObjectCompasIndex = 4
                Case 281.25# To 303.75#
                    pStructObject.ObjectCompasDirection = "WNW"
                    pStructObject.ObjectCompasIndex = 3
                Case 303.75# To 326.25#
                    pStructObject.ObjectCompasDirection = "NW"
                    pStructObject.ObjectCompasIndex = 2
                Case 326.25# To 348.75#
                    pStructObject.ObjectCompasDirection = "NNW"
                    pStructObject.ObjectCompasIndex = 1
            End Select

            executionTime = DateTime.UtcNow() - startExecution
            LogSessionEntry("DEBUG", "  CalculateObject: " + executionTime.ToString, "", "CalculateObject", "PROGRAM")

        Catch ex As Exception
            CalculateObject = "CalculateObject: " + ex.Message
            LogSessionEntry("ERROR", "CalculateObject: " + ex.Message, "", "CalculateObject", "PROGRAM")
        End Try
    End Function

    Private Sub ResetCalculateObject()
        Dim startExecution As Date
        Dim executionTime As TimeSpan

        startExecution = DateTime.UtcNow()
        LogSessionEntry("DEBUG", "  ResetCalculateObject...", "", "ResetCalculateObject", "PROGRAM")

        With pStructObject
            .LST = Nothing
            .GST = Nothing
            .HA = Nothing
            .ObjectRA2000 = Nothing
            .ObjectDEC2000 = Nothing
            .ObjectAlt = Nothing
            .ObjectAz = Nothing
            .ObjectCompasDirection = Nothing
        End With

        executionTime = DateTime.UtcNow() - startExecution
        LogSessionEntry("DEBUG", "  ResetCalculateObject: " + executionTime.ToString, "", "ResetCalculateObject", "PROGRAM")

    End Sub

    Public Function ConvertTargetJ2000ToTopocentric(vRA2000 As Double, vDEC2000 As Double) As String
        Dim AObserver As ASCOM.Astrometry.Observer
        'Dim ATransform As ASCOM.Astrometry.Transform.Transform
        Dim ATransform = New ASCOM.Astrometry.Transform.Transform

        Dim startExecution As Date
        Dim executionTime As TimeSpan

        startExecution = DateTime.UtcNow()
        LogSessionEntry("DEBUG", "  ConvertTargetJ2000ToTopocentric...", "", "ConvertTargetJ2000ToTopocentric", "PROGRAM")

        ConvertTargetJ2000ToTopocentric = "OK"
        Try

            With AObserver
                .Where = CType(1, ASCOM.Astrometry.ObserverLocation)
                .OnSurf.Height = My.Settings.sObserverHeight
                .OnSurf.Latitude = My.Settings.sObserverLatitude
                .OnSurf.Longitude = My.Settings.sObserverLongitude
                .OnSurf.Pressure = My.Settings.sObserverPressure
                .OnSurf.Temperature = My.Settings.sObserverTemperature
            End With

            ResetCalculateObject()

            pAUtils = New ASCOM.Astrometry.AstroUtils.AstroUtils
            pANova = New ASCOM.Astrometry.NOVAS.NOVAS31
            pAUtil = New ASCOM.Utilities.Util

            ATransform.SiteElevation = My.Settings.sObserverHeight
            ATransform.SiteLatitude = My.Settings.sObserverLatitude
            ATransform.SiteLongitude = My.Settings.sObserverLongitude
            ATransform.SiteTemperature = My.Settings.sObserverTemperature
            ATransform.SetJ2000(vRA2000, vDEC2000)

            pRATargetTopocentric = ATransform.RATopocentric
            pDECTargetTopocentric = ATransform.DECTopocentric
            LogSessionEntry("DEBUG", "  ConvertTargetJ2000ToTopocentric: RA " + Format(pRATargetTopocentric) + " DEC " + Format(pDECTargetTopocentric), "", "ConvertTargetJ2000ToTopocentric", "PROGRAM")

            executionTime = DateTime.UtcNow() - startExecution
            LogSessionEntry("DEBUG", "  ConvertTargetJ2000ToTopocentric: " + executionTime.ToString, "", "ConvertTargetJ2000ToTopocentric", "PROGRAM")


        Catch ex As Exception
            ConvertTargetJ2000ToTopocentric = "ConvertTargetJ2000ToTopocentric: " + ex.Message
            LogSessionEntry("ERROR", "ConvertTargetJ2000ToTopocentric: " + ex.Message, "", "ConvertTargetJ2000ToTopocentric", "PROGRAM")
        End Try

    End Function

    Public Function ConvertFocusJ2000ToTopocentric(vRA2000 As Double, vDEC2000 As Double) As String
        Dim AObserver As ASCOM.Astrometry.Observer
        'Dim ATransform As ASCOM.Astrometry.Transform.Transform
        Dim ATransform = New ASCOM.Astrometry.Transform.Transform
        Dim startExecution As Date
        Dim executionTime As TimeSpan

        startExecution = DateTime.UtcNow()
        LogSessionEntry("DEBUG", "  ConvertFocusJ2000ToTopocentric...", "", "ConvertFocusJ2000ToTopocentric", "PROGRAM")

        ConvertFocusJ2000ToTopocentric = "OK"
        Try

            With AObserver
                .Where = CType(1, ASCOM.Astrometry.ObserverLocation)
                .OnSurf.Height = My.Settings.sObserverHeight
                .OnSurf.Latitude = My.Settings.sObserverLatitude
                .OnSurf.Longitude = My.Settings.sObserverLongitude
                .OnSurf.Pressure = My.Settings.sObserverPressure
                .OnSurf.Temperature = My.Settings.sObserverTemperature
            End With

            ResetCalculateObject()

            pAUtils = New ASCOM.Astrometry.AstroUtils.AstroUtils
            pANova = New ASCOM.Astrometry.NOVAS.NOVAS31
            pAUtil = New ASCOM.Utilities.Util
            'ATransform = New ASCOM.Astrometry.Transform.Transform

            ATransform.SiteElevation = My.Settings.sObserverHeight
            ATransform.SiteLatitude = My.Settings.sObserverLatitude
            ATransform.SiteLongitude = My.Settings.sObserverLongitude
            ATransform.SiteTemperature = My.Settings.sObserverTemperature
            ATransform.SetJ2000(vRA2000, vDEC2000)

            pRAFocusTopocentric = ATransform.RATopocentric
            pDECFocusTopocentric = ATransform.DECTopocentric
            LogSessionEntry("DEBUG", "  ConvertFocusJ2000ToTopocentric: RA " + Format(pRAFocusTopocentric) + " DEC " + Format(pDECFocusTopocentric), "", "ConvertFocusJ2000ToTopocentric", "PROGRAM")

            executionTime = DateTime.UtcNow() - startExecution
            LogSessionEntry("DEBUG", "  ConvertFocusJ2000ToTopocentrics: " + executionTime.ToString, "", "ConvertFocusJ2000ToTopocentric", "PROGRAM")


        Catch ex As Exception
            ConvertFocusJ2000ToTopocentric = "ConvertFocusJ2000ToTopocentric: " + ex.Message
            LogSessionEntry("ERROR", "ConvertFocusJ2000ToTopocentric: " + ex.Message, "", "ConvertFocusJ2000ToTopocentric", "PROGRAM")
        End Try

    End Function
    Public Function CalculateSunMoonTimesOrig(vPlanet As String) As String
        ' calculates when object reaches certain attitude
        '1 hour julian date = 0,4166700002
        '1 minute julian date = 0,0006945

        Dim AObjectMoon As ASCOM.Astrometry.Object3
        Dim AObjectSun As ASCOM.Astrometry.Object3
        Dim AObserver As ASCOM.Astrometry.Observer
        Dim ASkyPos As ASCOM.Astrometry.SkyPos

        Dim ATransform = New ASCOM.Astrometry.Transform.Transform
        Dim offset_min As Double
        Dim offset_julian As Double
        Dim notfound As Boolean

        Dim startExecution As Date
        Dim executionTime As TimeSpan

        CalculateSunMoonTimesOrig = "OK"
        Try
            startExecution = DateTime.UtcNow()
            LogSessionEntry("DEBUG", "CalculateObject...", "", "CalculateObject", "PROGRAM")

            pANova = New ASCOM.Astrometry.NOVAS.NOVAS31

            offset_min = 0
            notfound = True

            pStructEventTimes.SunOpenRoof = ""
            pStructEventTimes.SunDuskFlats = ""
            pStructEventTimes.SunStartRun = ""
            pStructEventTimes.SunStopRun = ""
            pStructEventTimes.SunDawnFlats = ""

            AObserver.OnSurf.Height = My.Settings.sObserverHeight
            AObserver.OnSurf.Latitude = My.Settings.sObserverLatitude
            AObserver.OnSurf.Longitude = My.Settings.sObserverLongitude
            AObserver.OnSurf.Pressure = My.Settings.sObserverPressure
            AObserver.OnSurf.Temperature = My.Settings.sObserverTemperature

            With AObjectMoon
                .Name = "Moon"
                .Number = CType(11, ASCOM.Astrometry.Body)
            End With

            With AObjectSun
                .Name = "Sun"
                .Number = CType(10, ASCOM.Astrometry.Body)
            End With

            ATransform.SiteElevation = My.Settings.sObserverHeight
            ATransform.SiteLatitude = My.Settings.sObserverLatitude
            ATransform.SiteLongitude = My.Settings.sObserverLongitude
            ATransform.SiteTemperature = My.Settings.sObserverTemperature

            Do While notfound = True And offset_min < 1440
                offset_julian = offset_min * 0.0006945 + pAUtils.JulianDateUtc

#Disable Warning BC42109 ' Variable is used before it has been assigned a value
#Disable Warning BC42108 ' Variable is passed by reference before it has been assigned a value
                If vPlanet = "SUN" Then
                    pANova.Place(offset_julian, AObjectSun, AObserver, pAUtils.DeltaT, 0, 0, ASkyPos)
                Else
                    pANova.Place(offset_julian, AObjectMoon, AObserver, pAUtils.DeltaT, 0, 0, ASkyPos)
                End If
#Enable Warning BC42108 ' Variable is passed by reference before it has been assigned a value
#Enable Warning BC42109 ' Variable is used before it has been assigned a value

                ATransform.JulianDateUTC = offset_julian
                ATransform.SetJ2000(ASkyPos.RA, ASkyPos.Dec)

                'only valid for sunset
                'loops runs down => for negative numbers inversed; exit when value is found
                If vPlanet = "SUN" Then
                    If ATransform.ElevationTopocentric > My.Settings.sSunOpenRoof And ATransform.ElevationTopocentric <= My.Settings.sSunOpenRoof + 0.2 And pStructEventTimes.SunOpenRoof = "" And ATransform.AzimuthTopocentric >= 191.25 Then
                        pStructEventTimes.SunOpenRoof = pAUtils.FormatJD(offset_julian, "t")
                        LogSessionEntry("BRIEF", "Roof open at " + pStructEventTimes.SunOpenRoof, "", "CalculateSunMoonTimes", "PROGRAM")
                    End If

                    If ATransform.ElevationTopocentric > My.Settings.sSunDuskFlats And ATransform.ElevationTopocentric <= My.Settings.sSunDuskFlats + 0.2 And pStructEventTimes.SunDuskFlats = "" And ATransform.AzimuthTopocentric >= 191.25 Then
                        pStructEventTimes.SunDuskFlats = pAUtils.FormatJD(offset_julian, "t")
                        LogSessionEntry("BRIEF", "Dusk flats at " + pStructEventTimes.SunDuskFlats, "", "CalculateSunMoonTimes", "PROGRAM")
                    End If

                    If ATransform.ElevationTopocentric < My.Settings.sSunStartRun And ATransform.ElevationTopocentric >= My.Settings.sSunStartRun - 0.2 And pStructEventTimes.SunStartRun = "" And ATransform.AzimuthTopocentric >= 191.25 Then
                        pStructEventTimes.SunStartRun = pAUtils.FormatJD(offset_julian, "t")
                        LogSessionEntry("BRIEF", "Starting run at " + pStructEventTimes.SunStartRun, "", "CalculateSunMoonTimes", "PROGRAM")
                    End If

                    If ATransform.ElevationTopocentric < My.Settings.sSunStopRun And ATransform.ElevationTopocentric >= My.Settings.sSunStopRun - 0.2 And pStructEventTimes.SunStopRun = "" And ATransform.AzimuthTopocentric < 191.25 Then
                        pStructEventTimes.SunStopRun = pAUtils.FormatJD(offset_julian, "t")
                        LogSessionEntry("BRIEF", "Stopping run at " + pStructEventTimes.SunStopRun, "", "CalculateSunMoonTimes", "PROGRAM")
                    End If


                    If ATransform.ElevationTopocentric < My.Settings.sSunDawnFlats And ATransform.ElevationTopocentric >= My.Settings.sSunDawnFlats - 0.2 And pStructEventTimes.SunDawnFlats = "" And ATransform.AzimuthTopocentric < 191.25 Then
                        pStructEventTimes.SunDawnFlats = pAUtils.FormatJD(offset_julian, "t")

                        LogSessionEntry("BRIEF", "Dawn flats at " + pStructEventTimes.SunDawnFlats, "", "CalculateSunMoonTimes", "PROGRAM")
                        executionTime = DateTime.UtcNow() - startExecution
                        notfound = False
                    End If
                End If

                offset_min += 1

            Loop

            executionTime = DateTime.UtcNow() - startExecution
            LogSessionEntry("DEBUG", "CalculateObject: " + executionTime.ToString, "", "CalculateObject", "PROGRAM")


        Catch ex As Exception
            CalculateSunMoonTimesOrig = "CalculateSunMoonTimes: " + ex.Message
            LogSessionEntry("ERROR", "CalculateSunMoonTimes: " + ex.Message, "", "CalculateSunMoonTimes", "PROGRAM")
        End Try
    End Function
    Public Function CalculateSunMoonTimes(vPlanet As String) As String
        ' calculates when object reaches certain attitude
        '1 hour julian date = 0,4166700002
        '1 minute julian date = 0,0006945

        Dim AObjectMoon As ASCOM.Astrometry.Object3
        Dim AObjectSun As ASCOM.Astrometry.Object3
        Dim AObserver As ASCOM.Astrometry.Observer
        Dim ASkyPos As ASCOM.Astrometry.SkyPos

        Dim ATransform = New ASCOM.Astrometry.Transform.Transform
        Dim offset_min As Double
        Dim julian As Double
        Dim offset_julian As Double
        Dim notfound As Boolean

        Dim startExecution As Date
        Dim executionTime As TimeSpan

        CalculateSunMoonTimes = "OK"
        Try
            startExecution = DateTime.UtcNow()
            LogSessionEntry("DEBUG", "CalculateSunMoonTimes: " + executionTime.ToString, "", "CalculateSunMoonTimes", "PROGRAM")

            pANova = New ASCOM.Astrometry.NOVAS.NOVAS31

            offset_min = 0
            notfound = True

            pStructEventTimes.SunOpenRoof = ""
            pStructEventTimes.SunDuskFlats = ""
            pStructEventTimes.SunStartRun = ""
            pStructEventTimes.SunStopRun = ""
            pStructEventTimes.SunDawnFlats = ""

            AObserver.OnSurf.Height = My.Settings.sObserverHeight
            AObserver.OnSurf.Latitude = My.Settings.sObserverLatitude
            AObserver.OnSurf.Longitude = My.Settings.sObserverLongitude
            AObserver.OnSurf.Pressure = My.Settings.sObserverPressure
            AObserver.OnSurf.Temperature = My.Settings.sObserverTemperature

            With AObjectMoon
                .Name = "Moon"
                .Number = CType(11, ASCOM.Astrometry.Body)
            End With

            With AObjectSun
                .Name = "Sun"
                .Number = CType(10, ASCOM.Astrometry.Body)
            End With

            ATransform.SiteElevation = My.Settings.sObserverHeight
            ATransform.SiteLatitude = My.Settings.sObserverLatitude
            ATransform.SiteLongitude = My.Settings.sObserverLongitude
            ATransform.SiteTemperature = My.Settings.sObserverTemperature

            If DateTime.UtcNow().Hour < 12 Then
                'run of previous date
                julian = Math.Truncate(pAUtils.JulianDateUtc) - 1
            Else
                'run of today
                julian = Math.Truncate(pAUtils.JulianDateUtc)
            End If


            Do While notfound = True And offset_min < 1440
                offset_julian = offset_min * 0.0006945 + julian

#Disable Warning BC42109 ' Variable is used before it has been assigned a value
#Disable Warning BC42108 ' Variable is passed by reference before it has been assigned a value
                If vPlanet = "SUN" Then
                    pANova.Place(offset_julian, AObjectSun, AObserver, pAUtils.DeltaT, 0, 0, ASkyPos)
                Else
                    pANova.Place(offset_julian, AObjectMoon, AObserver, pAUtils.DeltaT, 0, 0, ASkyPos)
                End If
#Enable Warning BC42108 ' Variable is passed by reference before it has been assigned a value
#Enable Warning BC42109 ' Variable is used before it has been assigned a value

                ATransform.JulianDateUTC = offset_julian
                ATransform.SetJ2000(ASkyPos.RA, ASkyPos.Dec)

                'only valid for sunset
                'loops runs down => for negative numbers inversed; exit when value is found
                If vPlanet = "SUN" Then
                    If ATransform.ElevationTopocentric > My.Settings.sSunOpenRoof And ATransform.ElevationTopocentric <= My.Settings.sSunOpenRoof + 0.2 And pStructEventTimes.SunOpenRoof = "" And ATransform.AzimuthTopocentric >= 191.25 Then
                        pStructEventTimes.SunOpenRoof = pAUtils.FormatJD(offset_julian, "t")
                        LogSessionEntry("DEBUG", "  Roof open at " + pStructEventTimes.SunOpenRoof, "", "CalculateSunMoonTimes", "PROGRAM")
                    ElseIf pStructEventTimes.SunOpenRoof = "" Then
                        If ATransform.ElevationTopocentric <= My.Settings.sSunOpenRoof + 5 Then
                            offset_min += 1
                        Else
                            offset_min += 10
                        End If
                    Else
                        If ATransform.ElevationTopocentric > My.Settings.sSunDuskFlats And ATransform.ElevationTopocentric <= My.Settings.sSunDuskFlats + 0.2 And pStructEventTimes.SunDuskFlats = "" And ATransform.AzimuthTopocentric >= 191.25 Then
                            pStructEventTimes.SunDuskFlats = pAUtils.FormatJD(offset_julian, "t")
                            LogSessionEntry("DEBUG", "  Dusk flats at " + pStructEventTimes.SunDuskFlats, "", "CalculateSunMoonTimes", "PROGRAM")
                        ElseIf pStructEventTimes.SunDuskFlats = "" Then
                            If ATransform.ElevationTopocentric <= My.Settings.sSunDuskFlats + 5 Then
                                offset_min += 1
                            Else
                                offset_min += 10
                            End If
                        Else
                            If ATransform.ElevationTopocentric < My.Settings.sSunStartRun And ATransform.ElevationTopocentric >= My.Settings.sSunStartRun - 0.2 And pStructEventTimes.SunStartRun = "" And ATransform.AzimuthTopocentric >= 191.25 Then
                                pStructEventTimes.SunStartRun = pAUtils.FormatJD(offset_julian, "t")
                                LogSessionEntry("DEBUG", "  Starting run at " + pStructEventTimes.SunStartRun, "", "CalculateSunMoonTimes", "PROGRAM")
                            ElseIf pStructEventTimes.SunStartRun = "" Then
                                If ATransform.ElevationTopocentric >= My.Settings.sSunStartRun - 5 Then
                                    offset_min += 1
                                Else
                                    offset_min += 10
                                End If
                            Else
                                If ATransform.ElevationTopocentric < My.Settings.sSunStopRun And ATransform.ElevationTopocentric >= My.Settings.sSunStopRun - 0.2 And pStructEventTimes.SunStopRun = "" And ATransform.AzimuthTopocentric < 191.25 Then
                                    pStructEventTimes.SunStopRun = pAUtils.FormatJD(offset_julian, "t")
                                    LogSessionEntry("DEBUG", "  Stopping run at " + pStructEventTimes.SunStopRun, "", "CalculateSunMoonTimes", "PROGRAM")
                                ElseIf pStructEventTimes.SunStopRun = "" Then
                                    If ATransform.ElevationTopocentric >= My.Settings.sSunStopRun - 5 Then
                                        offset_min += 1
                                    Else
                                        offset_min += 10
                                    End If
                                Else
                                    If ATransform.ElevationTopocentric < My.Settings.sSunDawnFlats And ATransform.ElevationTopocentric >= My.Settings.sSunDawnFlats - 0.2 And pStructEventTimes.SunDawnFlats = "" And ATransform.AzimuthTopocentric < 191.25 Then
                                        pStructEventTimes.SunDawnFlats = pAUtils.FormatJD(offset_julian, "t")
                                        LogSessionEntry("DEBUG", "  Dawn flats at " + pStructEventTimes.SunDawnFlats, "", "CalculateSunMoonTimes", "PROGRAM")
                                        executionTime = DateTime.UtcNow() - startExecution
                                        notfound = False
                                    ElseIf pStructEventTimes.SunDawnFlats = "" Then
                                        If ATransform.ElevationTopocentric >= My.Settings.sSunDawnFlats - 5 Then
                                            offset_min += 1
                                        Else
                                            offset_min += 10
                                        End If
                                    End If
                                End If
                            End If
                        End If
                    End If
                End If

            Loop

            FrmMain.LblSunOpenRoof.Text = "Open roof: " + pStructEventTimes.SunOpenRoof
            FrmMain.LblSunDuskFlats.Text = "Dusk flats: " + pStructEventTimes.SunDuskFlats
            FrmMain.LblSunStartRun.Text = "Start run: " + pStructEventTimes.SunStartRun
            FrmMain.LblSunStopRun.Text = "Stop run: " + pStructEventTimes.SunStopRun
            FrmMain.LblSunDawnFlats.Text = "Dawn flats: " + pStructEventTimes.SunDawnFlats

            executionTime = DateTime.UtcNow() - startExecution
            LogSessionEntry("DEBUG", "  CalculateSunMoonTimes: " + executionTime.ToString, "", "CalculateSunMoonTimes", "PROGRAM")


        Catch ex As Exception
            CalculateSunMoonTimes = "CalculateSunMoonTimes: " + ex.Message
            LogSessionEntry("ERROR", "CalculateSunMoonTimes: " + ex.Message, "", "CalculateSunMoonTimes", "PROGRAM")
        End Try
    End Function


    Public Function CalculateMosaic(vRA_Center As Double, vDEC_Center As Double, vMosaicType As String, vOverlap As Integer) As String
        ' calculates panels in mosaic
        Dim startExecution As Date
        Dim executionTime As TimeSpan
        Dim XSizeHours, YSizeDegrees As Double
        Dim XSizeHoursCorrected, YSizeDegreesCorrected As Double
        CalculateMosaic = "OK"
        Try
            startExecution = DateTime.UtcNow()
            LogSessionEntry("DEBUG", "  CalculateMosaic...", "", "CalculateMosaic", "PROGRAM")


            ClearStructMosaic()

            'convert arcsec to hours/degrees
            ' 1 hour RA = 15 degrees = 15*3600= 54000 arcseconds
            ' 1 degree DEC = 3600 arcseconds

            'Center, 13hr 15' 49", 42° 01' 46" 
            'Pane 1, 13hr 19' 30", 42º 28' 33", 0.00, 90.60, 60.00, 10%, 1, 1
            'Pane 2, 13hr 12' 08", 42º 28' 33", 0.00, 90.60, 60.00, 10%, 1, 2
            'Pane 3, 13hr 19' 27", 41º 34' 33", 0.00, 90.60, 60.00, 10%, 2, 1
            'Pane 4, 13hr 12' 11", 41º 34' 33", 0.00, 90.60, 60.00, 10%, 2, 2
            'Pane, RA, DEC, Position Angle (East), Pane width (arcmins), Pane height (arcmins), Overlap, Row, Column

            'Image Scale * Sensorsize - Overlap
            'XSizeHours = ((My.Settings.sCCDImageScale * My.Settings.sCCDSensorSizeX) - ((My.Settings.sCCDImageScale * My.Settings.sCCDSensorSizeX) * (vOverlap / 100))) / 54000
            'YSizeDegrees = ((My.Settings.sCCDImageScale * My.Settings.sCCDSensorSizeY) - ((My.Settings.sCCDImageScale * My.Settings.sCCDSensorSizeY) * (vOverlap / 100))) / 3600

            XSizeHours = ((((206 * My.Settings.sCCDPixelSize) / My.Settings.sTelescopeFocalLength) * My.Settings.sCCDSensorSizeX) / 54000) * (1 / Math.Cos(Math.PI * vDEC_Center / 180.0))
            YSizeDegrees = ((((206 * My.Settings.sCCDPixelSize) / My.Settings.sTelescopeFocalLength) * My.Settings.sCCDSensorSizeY) / 3600)

            'old does not take sphere into account
            'XSizeHours = ((((206 * My.Settings.sCCDPixelSize) / My.Settings.sTelescopeFocalLength) * My.Settings.sCCDSensorSizeX) / 54000)
            'YSizeDegrees = ((((206 * My.Settings.sCCDPixelSize) / My.Settings.sTelescopeFocalLength) * My.Settings.sCCDSensorSizeY) / 3600)

            XSizeHoursCorrected = XSizeHours - (XSizeHours * (vOverlap / 100))
            YSizeDegreesCorrected = YSizeDegrees - (YSizeDegrees * (vOverlap / 100))

            '0.86*6248=5373,28=89,55'
            '0.86*4176=3591,36=59.85'

            '5377.167/54000=0.099577
            '3593.95/3600=0.99832

            '13.26369+(0.099577/2)
            '42.02937+(0.99832/2)

            ' 206 * Pixel Size (microns) / Focal Length (mm)=0.860622
            '206*3.76/900*6248=5377.167=89.62'
            '206*3.76/900*4176=3593.95=59.89'

            If vMosaicType = "1x2" Then
                '|1|
                '|2|

                pStructMosaic.Panel1_RA2000 = vRA_Center
                pStructMosaic.Panel1_DEC2000 = vDEC_Center + (YSizeDegreesCorrected / 2)

                pStructMosaic.Panel2_RA2000 = vRA_Center
                pStructMosaic.Panel2_DEC2000 = vDEC_Center - (YSizeDegreesCorrected / 2)

            ElseIf vMosaicType = "2x1" Then
                '|12|

                pStructMosaic.Panel1_RA2000 = vRA_Center + (XSizeHoursCorrected / 2)
                pStructMosaic.Panel1_DEC2000 = vDEC_Center

                pStructMosaic.Panel2_RA2000 = vRA_Center - (XSizeHoursCorrected / 2)
                pStructMosaic.Panel2_DEC2000 = vDEC_Center

            ElseIf vMosaicType = "2x2" Then
                '|12|
                '|34|

                pStructMosaic.Panel1_RA2000 = vRA_Center + (XSizeHoursCorrected / 2)
                pStructMosaic.Panel1_DEC2000 = vDEC_Center + (YSizeDegreesCorrected / 2)

                pStructMosaic.Panel2_RA2000 = vRA_Center - (XSizeHoursCorrected / 2)
                pStructMosaic.Panel2_DEC2000 = vDEC_Center + (YSizeDegreesCorrected / 2)

                pStructMosaic.Panel3_RA2000 = vRA_Center + (XSizeHoursCorrected / 2)
                pStructMosaic.Panel3_DEC2000 = vDEC_Center - (YSizeDegreesCorrected / 2)

                pStructMosaic.Panel4_RA2000 = vRA_Center - (XSizeHoursCorrected / 2)
                pStructMosaic.Panel4_DEC2000 = vDEC_Center - (YSizeDegreesCorrected / 2)

            End If

            executionTime = DateTime.UtcNow() - startExecution
            LogSessionEntry("DEBUG", "  CalculateMosaic: " + executionTime.ToString, "", "CalculateMosaic", "PROGRAM")

        Catch ex As Exception
            CalculateMosaic = "CalculateMosaic: " + ex.Message
            LogSessionEntry("ERROR", "CalculateMosaic: " + ex.Message, "", "CalculateMosaic", "PROGRAM")
        End Try
    End Function

    Public Sub ClearStructMosaic()
        ' calculates panels in mosaic
        Dim startExecution As Date
        Dim executionTime As TimeSpan

        Try
            startExecution = DateTime.UtcNow()
            LogSessionEntry("DEBUG", "  ClearStructMosaic: " + executionTime.ToString, "", "ClearStructMosaic", "PROGRAM")

            pStructMosaic.Panel1_RA2000 = vbEmpty
            pStructMosaic.Panel1_DEC2000 = vbEmpty
            pStructMosaic.Panel2_RA2000 = vbEmpty
            pStructMosaic.Panel2_DEC2000 = vbEmpty
            pStructMosaic.Panel3_RA2000 = vbEmpty
            pStructMosaic.Panel3_DEC2000 = vbEmpty
            pStructMosaic.Panel4_RA2000 = vbEmpty
            pStructMosaic.Panel4_DEC2000 = vbEmpty

            executionTime = DateTime.UtcNow() - startExecution
            LogSessionEntry("DEBUG", "  ClearStructMosaic: " + executionTime.ToString, "", "ClearStructMosaic", "PROGRAM")

        Catch ex As Exception
            LogSessionEntry("ERROR", "ClearStructMosaic: " + ex.Message, "", "ClearStructMosaic", "PROGRAM")
        End Try
    End Sub
End Module
