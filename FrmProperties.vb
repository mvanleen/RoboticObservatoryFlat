Imports System.ComponentModel
Imports System.Globalization
Imports Telegram.Bot.Types.Enums

Public Class FrmProperties
    Private Sub FrmProperties_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            Dim ciClone As CultureInfo = CType(CultureInfo.InvariantCulture.Clone(), CultureInfo)
            ciClone.NumberFormat.NumberDecimalSeparator = "."

            ChkAlarmPlay.Checked = My.Settings.sAlarmPlay
            TxtAlarmSound.Text = My.Settings.sAlarmSound
            TxtAlarmRepeat.Text = My.Settings.sAlarmRepeat.ToString(ciClone)
            TxtCCDFilter1.Text = My.Settings.sCCDFilter1
            TxtCCDFilter2.Text = My.Settings.sCCDFilter2
            TxtCCDFilter3.Text = My.Settings.sCCDFilter3
            TxtCCDFilter4.Text = My.Settings.sCCDFilter4
            TxtCCDFilter5.Text = My.Settings.sCCDFilter5
            TxtCCDImageScale.Text = My.Settings.sCCDImageScale.ToString(ciClone)
            TxtCCDSensorsizeX.Text = My.Settings.sCCDSensorSizeX.ToString(ciClone)
            TxtCCDSensorSizeY.Text = My.Settings.sCCDSensorSizeY.ToString(ciClone)
            TxtCCDImagePath.Text = My.Settings.sCCDImagePath
            TxtCCDPixelSize.Text = My.Settings.sCCDPixelSize.ToString(ciClone)
            ChkCCDCoolingEnabled.Checked = My.Settings.sCCDCoolingEnabled
            TxtCCDCoolingTemperature.Text = My.Settings.sCCDCoolingTemp.ToString(ciClone)
            RdCCDDithering.Checked = My.Settings.sCCDDithering
            TxtCCDDitheringAmount.Text = My.Settings.sCCDDitheringAmount.ToString
            TxtCCDFocusSubFrame.Text = My.Settings.sCCDFocusSubFrame.ToString(ciClone)
            TxtCCDFocusEveryXDegrees.Text = My.Settings.sCCDFocusEveryXDegrees.ToString(ciClone)
            TxtCCDFocusEveryXExposures.Text = My.Settings.sCCDFocusEveryXImages.ToString(ciClone)
            ChkCCDFocusMeridianFlip.Checked = My.Settings.sCCDFocusMeridianFlip
            ChkCCDFocusClosedLoopSlew.Checked = My.Settings.sCCDFocusClosedLoopSlew
            TxtCCDFocusDefExp.Text = My.Settings.sCCDFocusDefExp.ToString(ciClone)
            TxtCCDFocusSamples.Text = My.Settings.sCCDFocusSamples.ToString(ciClone)
            txtCCDName.Text = My.Settings.sCCDName
            ChkCCDColorCamera.Checked = My.Settings.sCCDColorCamera
            TxtFocusDefaultPosition.Text = My.Settings.sCCDFocusDefaultPosition.ToString(ciClone)
            TxtFocusDefaultFork.Text = My.Settings.sCCDFocusDefaultFork.ToString(ciClone)
            ChkCCDBlindSolve.Checked = My.Settings.sCCDBlindSolve
            TxtInternetTimeout.Text = My.Settings.sInternetTimeout.ToString(ciClone)
            TxtLogLocation.Text = My.Settings.sLogLocation
            CmbLogType.Text = My.Settings.sLogType
            TxtMoonAltitudeAlwaysSafe.Text = My.Settings.sMoonAltitudeAlwaysSafe.ToString(ciClone)
            TxtMoonPhaseAlwaysSafe.Text = My.Settings.sMoonPhaseAlwaysSafe.ToString(ciClone)
            TxtMoonPhaseLimitLow.Text = My.Settings.sMoonPhaseLimitLow.ToString(ciClone)
            TxtMoonAltitudeLimitLow.Text = My.Settings.sMoonAltitudeLimitLow.ToString(ciClone)
            TxtMoonStartCooldownLow.Text = My.Settings.sMoonStartCooldownLow.ToString(ciClone)
            TxtMoonAltitudeLimitHigh.Text = My.Settings.sMoonAltitudeLimitHigh.ToString(ciClone)
            TxtMoonStartCooldownHigh.Text = My.Settings.sMoonStartCooldownHigh.ToString(ciClone)
            ChkMoonStartCooldown.Checked = My.Settings.sMoonStartCooldown
            TxtObservatoryName.Text = My.Settings.sObservatoryName
            TxtObservatoryHeight.Text = My.Settings.sObserverHeight.ToString(ciClone)
            TxtObservatoryLatitude.Text = My.Settings.sObserverLatitude.ToString(ciClone)
            TxtObservatoryLongitude.Text = My.Settings.sObserverLongitude.ToString(ciClone)
            TxtObservatoryPressure.Text = My.Settings.sObserverPressure.ToString(ciClone)
            TxtObservatoryTemperature.Text = My.Settings.sObserverTemperature.ToString(ciClone)
            TxtRoofDevice.Text = My.Settings.sRoofDevice
            If My.Settings.sRoofDevice = "NONE" Then
                ChkNoRoof.Checked = True
            End If
            TxtRoofOpenPartly.Text = My.Settings.sRoofOpenPartly.ToString(ciClone)
            TxtRoofOpenTimeout.Text = My.Settings.sRoofOpenTimeout.ToString(ciClone)
            TxtSwitchDevice.Text = My.Settings.sSwitchDevice
            TxtSwitch1Name.Text = My.Settings.sSwitch1Name
            TxtSwitch2Name.Text = My.Settings.sSwitch2Name
            TxtSwitch3Name.Text = My.Settings.sSwitch3Name
            TxtSwitch4Name.Text = My.Settings.sSwitch4Name
            TxtSwitch5Name.Text = My.Settings.sSwitch5Name
            TxtSwitch6Name.Text = My.Settings.sSwitch6Name
            TxtSwitch7Name.Text = My.Settings.sSwitch7Name
            TxtSwitch8Name.Text = My.Settings.sSwitch8Name
            ChkSwitch1Needed.Checked = My.Settings.sSwitch1Needed
            ChkSwitch2Needed.Checked = My.Settings.sSwitch2Needed
            ChkSwitch3Needed.Checked = My.Settings.sSwitch3Needed
            ChkSwitch4Needed.Checked = My.Settings.sSwitch4Needed
            ChkSwitch5Needed.Checked = My.Settings.sSwitch5Needed
            ChkSwitch6Needed.Checked = My.Settings.sSwitch6Needed
            ChkSwitch7Needed.Checked = My.Settings.sSwitch7Needed
            ChkSwitch8Needed.Checked = My.Settings.sSwitch8Needed
            TxtSwitch1Warning.Text = My.Settings.sSwitch1Warning
            TxtSwitch2Warning.Text = My.Settings.sSwitch2Warning
            TxtSwitch3Warning.Text = My.Settings.sSwitch3Warning
            TxtSwitch4Warning.Text = My.Settings.sSwitch4Warning
            TxtSwitch5Warning.Text = My.Settings.sSwitch5Warning
            TxtSwitch6Warning.Text = My.Settings.sSwitch6Warning
            TxtSwitch7Warning.Text = My.Settings.sSwitch7Warning
            TxtSwitch8Warning.Text = My.Settings.sSwitch8Warning
            TxtSwitch1Startup.Text = My.Settings.sSwitch1Startup.ToString(ciClone)
            TxtSwitch2Startup.Text = My.Settings.sSwitch2Startup.ToString(ciClone)
            TxtSwitch3Startup.Text = My.Settings.sSwitch3Startup.ToString(ciClone)
            TxtSwitch4Startup.Text = My.Settings.sSwitch4Startup.ToString(ciClone)
            TxtSwitch5Startup.Text = My.Settings.sSwitch5Startup.ToString(ciClone)
            TxtSwitch6Startup.Text = My.Settings.sSwitch6Startup.ToString(ciClone)
            TxtSwitch7Startup.Text = My.Settings.sSwitch7Startup.ToString(ciClone)
            TxtSwitch8Startup.Text = My.Settings.sSwitch8Startup.ToString(ciClone)
            RdSwitch1Mount.Checked = My.Settings.sSwitch1Mount
            RdSwitch2Mount.Checked = My.Settings.sSwitch2Mount
            RdSwitch3Mount.Checked = My.Settings.sSwitch3Mount
            RdSwitch4Mount.Checked = My.Settings.sSwitch4Mount
            RdSwitch5Mount.Checked = My.Settings.sSwitch5Mount
            RdSwitch6Mount.Checked = My.Settings.sSwitch6Mount
            RdSwitch7Mount.Checked = My.Settings.sSwitch7Mount
            RdSwitch8Mount.Checked = My.Settings.sSwitch8Mount
            TxtTelegramBOT.Text = My.Settings.sTelegramBOT
            TxtTelegramErrorID.Text = My.Settings.sTelegramErrorID
            CmbTelegramLoggingType.Text = My.Settings.sTelegramLoggingType
            TxtTelegramSessionID.Text = My.Settings.sTelegramSessionID
            TxtUPSCommunity.Text = My.Settings.sUPSCommunity
            TxtUPSHost.Text = My.Settings.sUPSHost
            TxtUPSrequestOid.Text = My.Settings.sUPSrequestOid
            TxtUPSTimeout.Text = My.Settings.sUPSTimeout.ToString(ciClone)
            ChkUPSNotPresent.Checked = My.Settings.sUPSNotPresent
            TxtWeatherClearDelay.Text = My.Settings.sWeatherClearDelay.ToString(ciClone)
            TxtWeatherCloud_Clear.Text = My.Settings.sWeatherCloud_Clear.ToString(ciClone)
            TxtWeatherCloud_Cloudy.Text = My.Settings.sWeatherCloud_Cloudy.ToString(ciClone)
            TxtWeatherCloud_Overcast.Text = My.Settings.sWeatherCloud_Overcast.ToString(ciClone)
            TxtWeatherHumidity_Dry.Text = My.Settings.sWeatherHumidity_Dry.ToString(ciClone)
            TxtWeatherHumidity_Humid.Text = My.Settings.sWeatherHumidity_Humid.ToString(ciClone)
            TxtWeatherHumidity_Normal.Text = My.Settings.sWeatherHumidity_Normal.ToString(ciClone)
            TxtWeatherLightness_Dark.Text = My.Settings.sWeatherLightness_Dark.ToString(ciClone)
            TxtWeatherLightness_Light.Text = My.Settings.sWeatherLightness_Light.ToString(ciClone)
            TxtWeatherLightness_VeryLight.Text = My.Settings.sWeatherLightness_VeryLight.ToString(ciClone)
            TxtWeatherRain_Dry.Text = My.Settings.sWeatherRain_Dry.ToString(ciClone)
            TxtWeatherRain_Rain.Text = My.Settings.sWeatherRain_Rain.ToString(ciClone)
            TxtWeatherRain_Wet.Text = My.Settings.sWeatherRain_Wet.ToString(ciClone)
            TxtWeatherTimeout.Text = My.Settings.sWeatherTimeout.ToString(ciClone)
            TxtWeatherURL.Text = My.Settings.sWeatherURL
            RdWeatherUseSwitch.Checked = My.Settings.sWeatherUseSwitch
            RdWeatherUseValues.Checked = My.Settings.sWeatherUseValues
            TxtWeatherSafeSwitchDelay.Text = My.Settings.sWeatherSafeSwitchDelay.ToString(ciClone)
            TxtMountDevice.Text = My.Settings.sMountDevice
            TxtMountStartupLink.Text = My.Settings.sMountStartupLink
            TxtCCDPlateSolveExposure.Text = My.Settings.sCCDPlateSolveExposure.ToString(ciClone)
            TxtCCDPlateSolveMaxError.Text = My.Settings.sCCDPlateSolveMaxError.ToString(ciClone)
            CmbTargetBinning.Text = My.Settings.sCCDPlateSolveBinning
            TxtCCDPlateSolveNbrTries.Text = My.Settings.sCCDPlateSolveNbrTries.ToString(ciClone)
            TxtSunOpenRoof.Text = My.Settings.sSunOpenRoof.ToString(ciClone)
            TxtSunDuskFlats.Text = My.Settings.sSunDuskFlats.ToString(ciClone)
            TxtSunStartRun.Text = My.Settings.sSunStartRun.ToString(ciClone)
            TxtSunStopRun.Text = My.Settings.sSunStopRun.ToString(ciClone)
            TxtSunDawnFlats.Text = My.Settings.sSunDawnFlats.ToString(ciClone)
            TxtObjectAltLimitEast.Text = My.Settings.sObjectAltLimitEast.ToString(ciClone)
            TxtObjectAltLimitWest.Text = My.Settings.sObjectAltLimitWest.ToString(ciClone)
            TxtSnapCapSerialPort.Text = My.Settings.sSnapCapSerialPort
            TxtCoverDevice.Text = My.Settings.sCoverDevice
            CmbCoverMethod.Text = My.Settings.sCoverMethod
            ChkSunDuskFlatsNeeded.Checked = My.Settings.sSunDuskFlatsNeeded
            ChkSunDawnFlatsNeeded.Checked = My.Settings.sSunDawnFlatsNeeded
            TxtSynologyPath.Text = My.Settings.sSynologyPath
            TxtSMARTTimeout.Text = My.Settings.sSMARTTimeout.ToString(ciClone)
            TxtSMARTCycle.Text = My.Settings.sSMARTCycle.ToString(ciClone)
            TxtSentinelEXE.Text = My.Settings.sSentinelEXE
            ChkSentinelAutostart.Checked = My.Settings.sSentinelAutostart
            ChkTurnOffCompletely.Checked = My.Settings.sTurnOffCompletely
            CmbObservationProgram.Text = My.Settings.sObservationProgram
            ChkMoonIgnore.Checked = My.Settings.sMoonIgnore
            RdMountMeridianFlip.Checked = My.Settings.sMountMeridianFlip
            TxtMountMeridianFlipMinutes.Text = My.Settings.sMountMeridianFlipMinutes.ToString(ciClone)
            Txt7ZipLocation.Text = My.Settings.s7ZipLocation
            TxtsAutoFlatMinExp.Text = My.Settings.sAutoFlatMinExp.ToString(ciClone)
            TxtsAutoFlatMaxExp.Text = My.Settings.sAutoFlatMaxExp.ToString(ciClone)
            TxtsAutoFlatMinADU.Text = My.Settings.sAutoFlatMinADU.ToString(ciClone)
            TxtsAutoFlatMaxADU.Text = My.Settings.sAutoFlatMaxADU.ToString(ciClone)
            TxtsAutoFlatAlt.Text = My.Settings.sAutoFlatAlt.ToString(ciClone)
            TxtsAutoFlatAz.Text = My.Settings.sAutoFlatAz.ToString(ciClone)
            CmbsAutoFlatBin.Text = My.Settings.sAutoFlatBin
            CmbsAutoFlatFilter.Text = My.Settings.sAutoFlatFilter
            TxtURLHads.Text = My.Settings.SHADSURL
            TxtHADSMag8.Text = My.Settings.sHADSMag8
            TxtHADSMag9.Text = My.Settings.sHADSMag9
            TxtHADSMag10.Text = My.Settings.sHADSMag10
            TxtHADSMag11.Text = My.Settings.sHADSMag11
            TxtHADSMag12.Text = My.Settings.sHADSMag12
            TxtHADSMag13.Text = My.Settings.sHADSMag13
            TxtHADSMag14.Text = My.Settings.sHADSMag14
            TxtHADSMag15.Text = My.Settings.sHADSMag15
            txtHADSDECCutOff.Text = My.Settings.sHADSDECCutOff.ToString(ciClone)
            TxtTelescopeName.Text = My.Settings.sTelescopeName
            TxtTelescopeAparture.Text = My.Settings.sTelescopeAparture.ToString(ciClone)
            TxtTelescopeFocalLength.Text = My.Settings.sTelescopeFocalLength.ToString(ciClone)
            TxtMountTimeout.Text = My.Settings.sMountTimeout.ToString(ciClone)
            TxtCoverTimeout.Text = My.Settings.sCoverTimeout.ToString(ciClone)
            TxtCCDTimeout.Text = My.Settings.sCCDTimeout.ToString(ciClone)


            'add combobox options
            CmbsAutoFlatFilter.Items.Add(My.Settings.sCCDFilter1)
            CmbsAutoFlatFilter.Items.Add(My.Settings.sCCDFilter2)
            CmbsAutoFlatFilter.Items.Add(My.Settings.sCCDFilter3)
            CmbsAutoFlatFilter.Items.Add(My.Settings.sCCDFilter4)
            CmbsAutoFlatFilter.Items.Add(My.Settings.sCCDFilter5)


            If ChkNoRoof.Checked = True Then
                TxtRoofDevice.Text = "NONE"
                TxtRoofDevice.Enabled = False
                BtnChooseRoof.Enabled = False
                TxtRoofOpenPartly.Enabled = False
                TxtRoofOpenTimeout.Enabled = False
            Else
                TxtRoofDevice.Enabled = False
                BtnChooseRoof.Enabled = True
                TxtRoofOpenPartly.Enabled = True
                TxtRoofOpenTimeout.Enabled = True
            End If

            TxtMountDevice.Enabled = False

        Catch ex As Exception
            ShowMessage("FrmProperties_Load: " + ex.Message, "CRITICAL", "Error!")
        End Try
    End Sub

    Private Sub BtnBrowseLog_Click(sender As Object, e As EventArgs) Handles BtnBrowseLog.Click
        Try
            If (FolderBrowserDialog.ShowDialog() = DialogResult.OK) Then
                If FolderBrowserDialog.SelectedPath.Substring(FolderBrowserDialog.SelectedPath.Length - 1, 1) = "\" Then
                    TxtLogLocation.Text = FolderBrowserDialog.SelectedPath
                Else
                    TxtLogLocation.Text = FolderBrowserDialog.SelectedPath + "\"
                End If
            End If
        Catch ex As Exception
            ShowMessage("BtnBrowseLog_Click: " + ex.Message, "CRITICAL", "Error!")
        End Try


    End Sub

    Private Sub BtnBrowseSound_Click(sender As Object, e As EventArgs) Handles BtnBrowseSound.Click
        Dim result As DialogResult = OpenFileDialog.ShowDialog()
        Try
            If result = Windows.Forms.DialogResult.OK Then
                TxtAlarmSound.Text = OpenFileDialog.FileName
            End If
        Catch ex As Exception
            ShowMessage("BtnBrowseSound_Click: " + ex.Message, "CRITICAL", "Error!")
        End Try
    End Sub

    Private Sub FrmProperties_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim returnvalue As String

        Try
            Dim ciClone As CultureInfo = CType(CultureInfo.InvariantCulture.Clone(), CultureInfo)
            ciClone.NumberFormat.NumberDecimalSeparator = "."

            'check if there are changes
            If ChkAlarmPlay.Checked = My.Settings.sAlarmPlay And
                TxtAlarmSound.Text = My.Settings.sAlarmSound And
                TxtAlarmRepeat.Text = CStr(My.Settings.sAlarmRepeat) And
                TxtCCDFilter1.Text = My.Settings.sCCDFilter1 And
                TxtCCDFilter2.Text = My.Settings.sCCDFilter2 And
                TxtCCDFilter3.Text = My.Settings.sCCDFilter3 And
                TxtCCDFilter4.Text = My.Settings.sCCDFilter4 And
                TxtCCDFilter5.Text = My.Settings.sCCDFilter5 And
                TxtCCDImageScale.Text = My.Settings.sCCDImageScale.ToString(ciClone) And
                TxtCCDSensorsizeX.Text = My.Settings.sCCDSensorSizeX.ToString(ciClone) And
                TxtCCDSensorSizeY.Text = My.Settings.sCCDSensorSizeY.ToString(ciClone) And
                TxtCCDFocusSubFrame.Text = My.Settings.sCCDFocusSubFrame.ToString(ciClone) And
                TxtCCDFocusEveryXDegrees.Text = My.Settings.sCCDFocusEveryXDegrees.ToString(ciClone) And
                TxtCCDFocusEveryXExposures.Text = My.Settings.sCCDFocusEveryXImages.ToString(ciClone) And
                ChkCCDFocusMeridianFlip.Checked = My.Settings.sCCDFocusMeridianFlip And
                ChkCCDFocusClosedLoopSlew.Checked = My.Settings.sCCDFocusClosedLoopSlew And
                TxtCCDPixelSize.Text = My.Settings.sCCDPixelSize.ToString(ciClone) And
                TxtFocusDefaultPosition.Text = My.Settings.sCCDFocusDefaultPosition.ToString(ciClone) And
                TxtFocusDefaultFork.Text = My.Settings.sCCDFocusDefaultFork.ToString(ciClone) And
                TxtCCDImagePath.Text = My.Settings.sCCDImagePath And
                RdCCDDithering.Checked = My.Settings.sCCDDithering And
                TxtCCDDitheringAmount.Text = My.Settings.sCCDDitheringAmount.ToString(ciClone) And
                ChkCCDBlindSolve.Checked = My.Settings.sCCDBlindSolve And
                ChkCCDCoolingEnabled.Checked = My.Settings.sCCDCoolingEnabled And
                TxtCCDCoolingTemperature.Text = My.Settings.sCCDCoolingTemp.ToString(ciClone) And
                txtCCDName.Text = My.Settings.sCCDName And
                ChkCCDColorCamera.Checked = My.Settings.sCCDColorCamera And
                TxtInternetTimeout.Text = My.Settings.sInternetTimeout.ToString(ciClone) And
                TxtLogLocation.Text = My.Settings.sLogLocation And
                CmbLogType.Text = My.Settings.sLogType And
                TxtMoonAltitudeAlwaysSafe.Text = My.Settings.sMoonAltitudeAlwaysSafe.ToString(ciClone) And
                TxtMoonPhaseAlwaysSafe.Text = My.Settings.sMoonPhaseAlwaysSafe.ToString(ciClone) And
                TxtMoonPhaseLimitLow.Text = My.Settings.sMoonPhaseLimitLow.ToString(ciClone) And
                TxtMoonAltitudeLimitLow.Text = My.Settings.sMoonAltitudeLimitLow.ToString(ciClone) And
                TxtMoonStartCooldownLow.Text = My.Settings.sMoonStartCooldownLow.ToString(ciClone) And
                TxtMoonAltitudeLimitHigh.Text = My.Settings.sMoonAltitudeLimitHigh.ToString(ciClone) And
                TxtMoonStartCooldownHigh.Text = My.Settings.sMoonStartCooldownHigh.ToString(ciClone) And
                ChkMoonStartCooldown.Checked = My.Settings.sMoonStartCooldown And
                ChkMoonIgnore.Checked = My.Settings.sMoonIgnore And
                TxtObservatoryName.Text = My.Settings.sObservatoryName And
                TxtObservatoryHeight.Text = My.Settings.sObserverHeight.ToString(ciClone) And
                TxtObservatoryLatitude.Text = My.Settings.sObserverLatitude.ToString(ciClone) And
                TxtObservatoryLongitude.Text = My.Settings.sObserverLongitude.ToString(ciClone) And
                TxtObservatoryPressure.Text = My.Settings.sObserverPressure.ToString(ciClone) And
                TxtObservatoryTemperature.Text = My.Settings.sObserverTemperature.ToString(ciClone) And
                TxtRoofDevice.Text = My.Settings.sRoofDevice And
                TxtRoofOpenPartly.Text = My.Settings.sRoofOpenPartly.ToString(ciClone) And
                TxtRoofOpenTimeout.Text = My.Settings.sRoofOpenTimeout.ToString(ciClone) And
                TxtSwitchDevice.Text = My.Settings.sSwitchDevice And
                TxtSwitch1Name.Text = My.Settings.sSwitch1Name And
                TxtSwitch2Name.Text = My.Settings.sSwitch2Name And
                TxtSwitch3Name.Text = My.Settings.sSwitch3Name And
                TxtSwitch4Name.Text = My.Settings.sSwitch4Name And
                TxtSwitch5Name.Text = My.Settings.sSwitch5Name And
                TxtSwitch6Name.Text = My.Settings.sSwitch6Name And
                TxtSwitch7Name.Text = My.Settings.sSwitch7Name And
                TxtSwitch8Name.Text = My.Settings.sSwitch8Name And
                ChkSwitch1Needed.Checked = My.Settings.sSwitch1Needed And
                ChkSwitch2Needed.Checked = My.Settings.sSwitch2Needed And
                ChkSwitch3Needed.Checked = My.Settings.sSwitch3Needed And
                ChkSwitch4Needed.Checked = My.Settings.sSwitch4Needed And
                ChkSwitch5Needed.Checked = My.Settings.sSwitch5Needed And
                ChkSwitch6Needed.Checked = My.Settings.sSwitch6Needed And
                ChkSwitch7Needed.Checked = My.Settings.sSwitch7Needed And
                ChkSwitch8Needed.Checked = My.Settings.sSwitch8Needed And
                TxtSwitch1Warning.Text = My.Settings.sSwitch1Warning And
                TxtSwitch2Warning.Text = My.Settings.sSwitch2Warning And
                TxtSwitch3Warning.Text = My.Settings.sSwitch3Warning And
                TxtSwitch4Warning.Text = My.Settings.sSwitch4Warning And
                TxtSwitch5Warning.Text = My.Settings.sSwitch5Warning And
                TxtSwitch6Warning.Text = My.Settings.sSwitch6Warning And
                TxtSwitch7Warning.Text = My.Settings.sSwitch7Warning And
                TxtSwitch8Warning.Text = My.Settings.sSwitch8Warning And
                TxtSwitch1Startup.Text = My.Settings.sSwitch1Startup.ToString(ciClone) And
                TxtSwitch2Startup.Text = My.Settings.sSwitch2Startup.ToString(ciClone) And
                TxtSwitch3Startup.Text = My.Settings.sSwitch3Startup.ToString(ciClone) And
                TxtSwitch4Startup.Text = My.Settings.sSwitch4Startup.ToString(ciClone) And
                TxtSwitch5Startup.Text = My.Settings.sSwitch5Startup.ToString(ciClone) And
                TxtSwitch6Startup.Text = My.Settings.sSwitch6Startup.ToString(ciClone) And
                TxtSwitch7Startup.Text = My.Settings.sSwitch7Startup.ToString(ciClone) And
                TxtSwitch8Startup.Text = My.Settings.sSwitch8Startup.ToString(ciClone) And
                RdSwitch1Mount.Checked = My.Settings.sSwitch1Mount And
                RdSwitch2Mount.Checked = My.Settings.sSwitch2Mount And
                RdSwitch3Mount.Checked = My.Settings.sSwitch3Mount And
                RdSwitch4Mount.Checked = My.Settings.sSwitch4Mount And
                RdSwitch5Mount.Checked = My.Settings.sSwitch5Mount And
                RdSwitch6Mount.Checked = My.Settings.sSwitch6Mount And
                RdSwitch7Mount.Checked = My.Settings.sSwitch7Mount And
                RdSwitch8Mount.Checked = My.Settings.sSwitch8Mount And
                TxtTelegramBOT.Text = My.Settings.sTelegramBOT And
                TxtTelegramErrorID.Text = My.Settings.sTelegramErrorID And
                CmbTelegramLoggingType.Text = My.Settings.sTelegramLoggingType And
                TxtTelegramSessionID.Text = My.Settings.sTelegramSessionID And
                TxtUPSCommunity.Text = My.Settings.sUPSCommunity And
                TxtUPSHost.Text = My.Settings.sUPSHost And
                TxtUPSrequestOid.Text = My.Settings.sUPSrequestOid And
                TxtUPSTimeout.Text = My.Settings.sUPSTimeout.ToString(ciClone) And
                ChkUPSNotPresent.Checked = My.Settings.sUPSNotPresent And
                TxtWeatherClearDelay.Text = My.Settings.sWeatherClearDelay.ToString(ciClone) And
                TxtWeatherCloud_Clear.Text = My.Settings.sWeatherCloud_Clear.ToString(ciClone) And
                TxtWeatherCloud_Cloudy.Text = My.Settings.sWeatherCloud_Cloudy.ToString(ciClone) And
                TxtWeatherCloud_Overcast.Text = My.Settings.sWeatherCloud_Overcast.ToString(ciClone) And
                TxtWeatherHumidity_Dry.Text = My.Settings.sWeatherHumidity_Dry.ToString(ciClone) And
                TxtWeatherHumidity_Humid.Text = My.Settings.sWeatherHumidity_Humid.ToString(ciClone) And
                TxtWeatherHumidity_Normal.Text = My.Settings.sWeatherHumidity_Normal.ToString(ciClone) And
                TxtWeatherLightness_Dark.Text = My.Settings.sWeatherLightness_Dark.ToString(ciClone) And
                TxtWeatherLightness_Light.Text = My.Settings.sWeatherLightness_Light.ToString(ciClone) And
                TxtWeatherLightness_VeryLight.Text = My.Settings.sWeatherLightness_VeryLight.ToString(ciClone) And
                TxtWeatherRain_Dry.Text = My.Settings.sWeatherRain_Dry.ToString(ciClone) And
                TxtWeatherRain_Rain.Text = My.Settings.sWeatherRain_Rain.ToString(ciClone) And
                TxtWeatherRain_Wet.Text = My.Settings.sWeatherRain_Wet.ToString(ciClone) And
                TxtWeatherTimeout.Text = My.Settings.sWeatherTimeout.ToString(ciClone) And
                TxtWeatherURL.Text = My.Settings.sWeatherURL And
                RdWeatherUseSwitch.Checked = My.Settings.sWeatherUseSwitch And
                RdWeatherUseValues.Checked = My.Settings.sWeatherUseValues And
                TxtWeatherSafeSwitchDelay.Text = My.Settings.sWeatherSafeSwitchDelay.ToString(ciClone) And
                TxtMountDevice.Text = My.Settings.sMountDevice And
                TxtMountStartupLink.Text = My.Settings.sMountStartupLink And
                TxtCCDPlateSolveExposure.Text = My.Settings.sCCDPlateSolveExposure.ToString(ciClone) And
                TxtCCDPlateSolveMaxError.Text = My.Settings.sCCDPlateSolveMaxError.ToString(ciClone) And
                CmbTargetBinning.Text = My.Settings.sCCDPlateSolveBinning And
                TxtCCDPlateSolveNbrTries.Text = My.Settings.sCCDPlateSolveNbrTries.ToString(ciClone) And
                TxtSunOpenRoof.Text = My.Settings.sSunOpenRoof.ToString(ciClone) And
                TxtSunDuskFlats.Text = My.Settings.sSunDuskFlats.ToString(ciClone) And
                TxtSunStartRun.Text = My.Settings.sSunStartRun.ToString(ciClone) And
                TxtSunStopRun.Text = My.Settings.sSunStopRun.ToString(ciClone) And
                TxtSunDawnFlats.Text = My.Settings.sSunDawnFlats.ToString(ciClone) And
                TxtObjectAltLimitEast.Text = My.Settings.sObjectAltLimitEast.ToString(ciClone) And
                TxtObjectAltLimitWest.Text = My.Settings.sObjectAltLimitWest.ToString(ciClone) And
                TxtSnapCapSerialPort.Text = My.Settings.sSnapCapSerialPort And
                CmbCoverMethod.Text = My.Settings.sCoverMethod And
                TxtCoverDevice.Text = My.Settings.sCoverDevice And
                TxtCCDFocusDefExp.Text = My.Settings.sCCDFocusDefExp.ToString(ciClone) And
                TxtCCDFocusSamples.Text = My.Settings.sCCDFocusSamples.ToString(ciClone) And
                ChkSunDuskFlatsNeeded.Checked = My.Settings.sSunDuskFlatsNeeded And
                ChkSunDawnFlatsNeeded.Checked = My.Settings.sSunDawnFlatsNeeded And
                TxtSynologyPath.Text = My.Settings.sSynologyPath And
                TxtSMARTTimeout.Text = My.Settings.sSMARTTimeout.ToString(ciClone) And
                TxtSMARTCycle.Text = My.Settings.sSMARTCycle.ToString(ciClone) And
                TxtSentinelEXE.Text = My.Settings.sSentinelEXE And
                ChkSentinelAutostart.Checked = My.Settings.sSentinelAutostart And
                ChkTurnOffCompletely.Checked = My.Settings.sTurnOffCompletely And
                CmbObservationProgram.Text = My.Settings.sObservationProgram And
                RdMountMeridianFlip.Checked = My.Settings.sMountMeridianFlip And
                TxtMountMeridianFlipMinutes.Text = My.Settings.sMountMeridianFlipMinutes.ToString(ciClone) And
                Txt7ZipLocation.Text = My.Settings.s7ZipLocation And
                TxtsAutoFlatMinExp.Text = My.Settings.sAutoFlatMinExp.ToString(ciClone) And
                TxtsAutoFlatMaxExp.Text = My.Settings.sAutoFlatMaxExp.ToString(ciClone) And
                TxtsAutoFlatMinADU.Text = My.Settings.sAutoFlatMinADU.ToString(ciClone) And
                TxtsAutoFlatMaxADU.Text = My.Settings.sAutoFlatMaxADU.ToString(ciClone) And
                TxtsAutoFlatAlt.Text = My.Settings.sAutoFlatAlt.ToString(ciClone) And
                TxtsAutoFlatAz.Text = My.Settings.sAutoFlatAz.ToString(ciClone) And
                CmbsAutoFlatBin.Text = My.Settings.sAutoFlatBin And
                CmbsAutoFlatFilter.Text = My.Settings.sAutoFlatFilter And
                TxtURLHads.Text = My.Settings.SHADSURL And
                TxtHADSMag8.Text = My.Settings.sHADSMag8 And
                TxtHADSMag9.Text = My.Settings.sHADSMag9 And
                TxtHADSMag10.Text = My.Settings.sHADSMag10 And
                TxtHADSMag11.Text = My.Settings.sHADSMag11 And
                TxtHADSMag12.Text = My.Settings.sHADSMag12 And
                TxtHADSMag13.Text = My.Settings.sHADSMag13 And
                TxtHADSMag14.Text = My.Settings.sHADSMag14 And
                TxtHADSMag15.Text = My.Settings.sHADSMag15 And
                txtHADSDECCutOff.Text = My.Settings.sHADSDECCutOff.ToString(ciClone) And
                TxtMountTimeout.Text = My.Settings.sMountTimeout.ToString(ciClone) And
                TxtCoverTimeout.Text = My.Settings.sCoverTimeout.ToString(ciClone) And
                TxtCCDTimeout.Text = My.Settings.sCCDTimeout.ToString(ciClone) And
                TxtTelescopeName.Text = My.Settings.sTelescopeName And
                TxtTelescopeAparture.Text = My.Settings.sTelescopeAparture.ToString(ciClone) And
                TxtTelescopeFocalLength.Text = My.Settings.sTelescopeFocalLength.ToString(ciClone) Then
                'no changes
            Else
                If ShowMessage("Do you want to save the changes ?", "OKCANCEL", "Save changes ?") = vbOK Then
                    My.Settings.sAlarmPlay = ChkAlarmPlay.Checked
                    My.Settings.sAlarmRepeat = Convert.ToInt32(TxtAlarmRepeat.Text)
                    My.Settings.sAlarmSound = TxtAlarmSound.Text
                    My.Settings.sCCDFilter1 = TxtCCDFilter1.Text
                    My.Settings.sCCDFilter2 = TxtCCDFilter2.Text
                    My.Settings.sCCDFilter3 = TxtCCDFilter3.Text
                    My.Settings.sCCDFilter4 = TxtCCDFilter4.Text
                    My.Settings.sCCDFilter5 = TxtCCDFilter5.Text
                    My.Settings.sCCDImageScale = Double.Parse(TxtCCDImageScale.Text, ciClone)
                    My.Settings.sCCDSensorSizeX = Convert.ToInt32(TxtCCDSensorsizeX.Text)
                    My.Settings.sCCDSensorSizeY = Convert.ToInt32(TxtCCDSensorSizeY.Text)
                    My.Settings.sCCDFocusSubFrame = Double.Parse(TxtCCDFocusSubFrame.Text, ciClone)
                    My.Settings.sCCDFocusEveryXDegrees = Double.Parse(TxtCCDFocusEveryXDegrees.Text, ciClone)
                    My.Settings.sCCDFocusEveryXImages = Convert.ToInt32(TxtCCDFocusEveryXExposures.Text)
                    My.Settings.sCCDFocusMeridianFlip = ChkCCDFocusMeridianFlip.Checked
                    My.Settings.sCCDFocusClosedLoopSlew = ChkCCDFocusClosedLoopSlew.Checked
                    My.Settings.sCCDFocusDefaultPosition = Double.Parse(TxtFocusDefaultPosition.Text, ciClone)
                    My.Settings.sCCDFocusDefaultFork = Double.Parse(TxtFocusDefaultFork.Text, ciClone)
                    My.Settings.sCCDImagePath = TxtCCDImagePath.Text
                    My.Settings.sCCDDithering = RdCCDDithering.Checked
                    My.Settings.sCCDDitheringAmount = Convert.ToInt32(TxtCCDDitheringAmount.Text)
                    My.Settings.sCCDBlindSolve = ChkCCDBlindSolve.Checked
                    My.Settings.sCCDCoolingEnabled = ChkCCDCoolingEnabled.Checked
                    My.Settings.sCCDCoolingTemp = Double.Parse(TxtCCDCoolingTemperature.Text, ciClone)
                    My.Settings.sCCDName = txtCCDName.Text
                    My.Settings.sCCDColorCamera = ChkCCDColorCamera.Checked
                    My.Settings.sCCDPixelSize = Double.Parse(TxtCCDPixelSize.Text, ciClone)
                    My.Settings.sInternetTimeout = Convert.ToInt32(TxtInternetTimeout.Text)
                    My.Settings.sLogLocation = TxtLogLocation.Text
                    My.Settings.sLogType = CmbLogType.Text
                    My.Settings.sMoonAltitudeAlwaysSafe = Double.Parse(TxtMoonAltitudeAlwaysSafe.Text, ciClone)
                    My.Settings.sMoonPhaseAlwaysSafe = Double.Parse(TxtMoonPhaseAlwaysSafe.Text, ciClone)
                    My.Settings.sMoonPhaseLimitLow = Double.Parse(TxtMoonPhaseLimitLow.Text, ciClone)
                    My.Settings.sMoonAltitudeLimitLow = Double.Parse(TxtMoonAltitudeLimitLow.Text, ciClone)
                    My.Settings.sMoonStartCooldownLow = Double.Parse(TxtMoonStartCooldownLow.Text, ciClone)
                    My.Settings.sMoonAltitudeLimitHigh = Double.Parse(TxtMoonAltitudeLimitHigh.Text, ciClone)
                    My.Settings.sMoonStartCooldownHigh = Double.Parse(TxtMoonStartCooldownHigh.Text, ciClone)
                    My.Settings.sMoonStartCooldown = ChkMoonStartCooldown.Checked
                    My.Settings.sMoonIgnore = ChkMoonIgnore.Checked
                    My.Settings.sMoonStartCooldown = ChkMoonStartCooldown.Checked
                    My.Settings.sObservatoryName = TxtObservatoryName.Text
                    My.Settings.sObserverHeight = Double.Parse(TxtObservatoryHeight.Text, ciClone)
                    My.Settings.sObserverLatitude = Double.Parse(TxtObservatoryLatitude.Text, ciClone)
                    My.Settings.sObserverLongitude = Double.Parse(TxtObservatoryLongitude.Text, ciClone)
                    My.Settings.sObserverPressure = Double.Parse(TxtObservatoryPressure.Text, ciClone)
                    My.Settings.sObserverTemperature = Double.Parse(TxtObservatoryTemperature.Text, ciClone)
                    My.Settings.sRoofDevice = TxtRoofDevice.Text
                    My.Settings.sRoofOpenPartly = Double.Parse(TxtRoofOpenPartly.Text, ciClone)
                    My.Settings.sRoofOpenTimeout = Double.Parse(TxtRoofOpenTimeout.Text, ciClone)
                    My.Settings.sSwitchDevice = TxtSwitchDevice.Text
                    My.Settings.sSwitch1Name = TxtSwitch1Name.Text
                    My.Settings.sSwitch2Name = TxtSwitch2Name.Text
                    My.Settings.sSwitch3Name = TxtSwitch3Name.Text
                    My.Settings.sSwitch4Name = TxtSwitch4Name.Text
                    My.Settings.sSwitch5Name = TxtSwitch5Name.Text
                    My.Settings.sSwitch6Name = TxtSwitch6Name.Text
                    My.Settings.sSwitch7Name = TxtSwitch7Name.Text
                    My.Settings.sSwitch8Name = TxtSwitch8Name.Text
                    My.Settings.sSwitch1Needed = ChkSwitch1Needed.Checked
                    My.Settings.sSwitch2Needed = ChkSwitch2Needed.Checked
                    My.Settings.sSwitch3Needed = ChkSwitch3Needed.Checked
                    My.Settings.sSwitch4Needed = ChkSwitch4Needed.Checked
                    My.Settings.sSwitch5Needed = ChkSwitch5Needed.Checked
                    My.Settings.sSwitch6Needed = ChkSwitch6Needed.Checked
                    My.Settings.sSwitch7Needed = ChkSwitch7Needed.Checked
                    My.Settings.sSwitch8Needed = ChkSwitch8Needed.Checked
                    My.Settings.sSwitch1Warning = TxtSwitch1Warning.Text
                    My.Settings.sSwitch2Warning = TxtSwitch2Warning.Text
                    My.Settings.sSwitch3Warning = TxtSwitch3Warning.Text
                    My.Settings.sSwitch4Warning = TxtSwitch4Warning.Text
                    My.Settings.sSwitch5Warning = TxtSwitch5Warning.Text
                    My.Settings.sSwitch6Warning = TxtSwitch6Warning.Text
                    My.Settings.sSwitch7Warning = TxtSwitch7Warning.Text
                    My.Settings.sSwitch8Warning = TxtSwitch8Warning.Text
                    My.Settings.sSwitch1Startup = Double.Parse(TxtSwitch1Startup.Text, ciClone)
                    My.Settings.sSwitch2Startup = Double.Parse(TxtSwitch2Startup.Text, ciClone)
                    My.Settings.sSwitch3Startup = Double.Parse(TxtSwitch3Startup.Text, ciClone)
                    My.Settings.sSwitch4Startup = Double.Parse(TxtSwitch4Startup.Text, ciClone)
                    My.Settings.sSwitch5Startup = Double.Parse(TxtSwitch5Startup.Text, ciClone)
                    My.Settings.sSwitch6Startup = Double.Parse(TxtSwitch6Startup.Text, ciClone)
                    My.Settings.sSwitch7Startup = Double.Parse(TxtSwitch7Startup.Text, ciClone)
                    My.Settings.sSwitch8Startup = Double.Parse(TxtSwitch8Startup.Text, ciClone)
                    My.Settings.sSwitch1Mount = RdSwitch1Mount.Checked
                    My.Settings.sSwitch2Mount = RdSwitch2Mount.Checked
                    My.Settings.sSwitch3Mount = RdSwitch3Mount.Checked
                    My.Settings.sSwitch4Mount = RdSwitch4Mount.Checked
                    My.Settings.sSwitch5Mount = RdSwitch5Mount.Checked
                    My.Settings.sSwitch6Mount = RdSwitch6Mount.Checked
                    My.Settings.sSwitch7Mount = RdSwitch7Mount.Checked
                    My.Settings.sSwitch8Mount = RdSwitch8Mount.Checked
                    My.Settings.sTelegramBOT = TxtTelegramBOT.Text
                    My.Settings.sTelegramErrorID = TxtTelegramErrorID.Text
                    My.Settings.sTelegramLoggingType = CmbTelegramLoggingType.Text
                    My.Settings.sTelegramSessionID = TxtTelegramSessionID.Text
                    My.Settings.sUPSCommunity = TxtUPSCommunity.Text
                    My.Settings.sUPSHost = TxtUPSHost.Text
                    My.Settings.sUPSrequestOid = TxtUPSrequestOid.Text
                    My.Settings.sUPSTimeout = Double.Parse(TxtUPSTimeout.Text, ciClone)
                    My.Settings.sUPSNotPresent = ChkUPSNotPresent.Checked
                    My.Settings.sWeatherClearDelay = Double.Parse(TxtWeatherClearDelay.Text, ciClone)
                    My.Settings.sWeatherCloud_Clear = Double.Parse(TxtWeatherCloud_Clear.Text, ciClone)
                    My.Settings.sWeatherCloud_Cloudy = Double.Parse(TxtWeatherCloud_Cloudy.Text, ciClone)
                    My.Settings.sWeatherCloud_Overcast = Double.Parse(TxtWeatherCloud_Overcast.Text, ciClone)
                    My.Settings.sWeatherHumidity_Dry = Double.Parse(TxtWeatherHumidity_Dry.Text, ciClone)
                    My.Settings.sWeatherHumidity_Humid = Double.Parse(TxtWeatherHumidity_Humid.Text, ciClone)
                    My.Settings.sWeatherHumidity_Normal = Double.Parse(TxtWeatherHumidity_Normal.Text, ciClone)
                    My.Settings.sWeatherLightness_Dark = Double.Parse(TxtWeatherLightness_Dark.Text, ciClone)
                    My.Settings.sWeatherLightness_Light = Double.Parse(TxtWeatherLightness_Light.Text, ciClone)
                    My.Settings.sWeatherLightness_VeryLight = Double.Parse(TxtWeatherLightness_VeryLight.Text, ciClone)
                    My.Settings.sWeatherRain_Dry = Double.Parse(TxtWeatherRain_Dry.Text, ciClone)
                    My.Settings.sWeatherRain_Rain = Double.Parse(TxtWeatherRain_Rain.Text, ciClone)
                    My.Settings.sWeatherRain_Wet = Double.Parse(TxtWeatherRain_Wet.Text, ciClone)
                    My.Settings.sWeatherTimeout = Double.Parse(TxtWeatherTimeout.Text, ciClone)
                    My.Settings.sWeatherURL = TxtWeatherURL.Text
                    My.Settings.sWeatherUseSwitch = RdWeatherUseSwitch.Checked
                    My.Settings.sWeatherUseValues = RdWeatherUseValues.Checked
                    My.Settings.sWeatherSafeSwitchDelay = Convert.ToInt32(TxtWeatherSafeSwitchDelay.Text)
                    My.Settings.sMountDevice = TxtMountDevice.Text
                    My.Settings.sMountStartupLink = TxtMountStartupLink.Text
                    My.Settings.sCCDPlateSolveExposure = Double.Parse(TxtCCDPlateSolveExposure.Text, ciClone)
                    My.Settings.sCCDPlateSolveMaxError = Convert.ToInt32(TxtCCDPlateSolveMaxError.Text)
                    My.Settings.sCCDPlateSolveBinning = CmbTargetBinning.Text
                    My.Settings.sCCDPlateSolveNbrTries = Convert.ToInt32(TxtCCDPlateSolveNbrTries.Text)
                    My.Settings.sSunOpenRoof = Double.Parse(TxtSunOpenRoof.Text, ciClone)
                    My.Settings.sSunDuskFlats = Double.Parse(TxtSunDuskFlats.Text, ciClone)
                    My.Settings.sSunStartRun = Double.Parse(TxtSunStartRun.Text, ciClone)
                    My.Settings.sSunStopRun = Double.Parse(TxtSunStopRun.Text, ciClone)
                    My.Settings.sSunDawnFlats = Double.Parse(TxtSunDawnFlats.Text, ciClone)
                    My.Settings.sObjectAltLimitEast = Double.Parse(TxtObjectAltLimitEast.Text, ciClone)
                    My.Settings.sObjectAltLimitWest = Double.Parse(TxtObjectAltLimitWest.Text, ciClone)
                    My.Settings.sSnapCapSerialPort = TxtSnapCapSerialPort.Text
                    My.Settings.sCoverMethod = CmbCoverMethod.Text
                    My.Settings.sCoverDevice = TxtCoverDevice.Text
                    My.Settings.sCCDFocusDefExp = Double.Parse(TxtCCDFocusDefExp.Text, ciClone)
                    My.Settings.sCCDFocusSamples = Convert.ToInt32(TxtCCDFocusSamples.Text)
                    My.Settings.sSunDuskFlatsNeeded = ChkSunDuskFlatsNeeded.Checked
                    My.Settings.sSunDawnFlatsNeeded = ChkSunDawnFlatsNeeded.Checked
                    My.Settings.sSynologyPath = TxtSynologyPath.Text
                    My.Settings.sSMARTTimeout = Double.Parse(TxtSMARTTimeout.Text, ciClone)
                    My.Settings.sSMARTCycle = Double.Parse(TxtSMARTCycle.Text, ciClone)
                    My.Settings.sSentinelEXE = TxtSentinelEXE.Text
                    My.Settings.sSentinelAutostart = ChkSentinelAutostart.Checked
                    My.Settings.sTurnOffCompletely = ChkTurnOffCompletely.Checked
                    My.Settings.sObservationProgram = CmbObservationProgram.Text
                    My.Settings.sMountMeridianFlip = RdMountMeridianFlip.Checked
                    My.Settings.sMountMeridianFlipMinutes = Double.Parse(TxtMountMeridianFlipMinutes.Text, ciClone)
                    My.Settings.s7ZipLocation = Txt7ZipLocation.Text
                    My.Settings.sAutoFlatMinExp = Double.Parse(TxtsAutoFlatMinExp.Text, ciClone)
                    My.Settings.sAutoFlatMaxExp = Double.Parse(TxtsAutoFlatMaxExp.Text, ciClone)
                    My.Settings.sAutoFlatMinADU = Double.Parse(TxtsAutoFlatMinADU.Text, ciClone)
                    My.Settings.sAutoFlatMaxADU = Double.Parse(TxtsAutoFlatMaxADU.Text, ciClone)
                    My.Settings.sAutoFlatAlt = Double.Parse(TxtsAutoFlatAlt.Text, ciClone)
                    My.Settings.sAutoFlatAz = Double.Parse(TxtsAutoFlatAz.Text, ciClone)
                    My.Settings.sAutoFlatBin = CmbsAutoFlatBin.Text
                    My.Settings.sAutoFlatFilter = CmbsAutoFlatFilter.Text
                    My.Settings.SHADSURL = TxtURLHads.Text
                    My.Settings.sHADSMag8 = TxtHADSMag8.Text
                    My.Settings.sHADSMag9 = TxtHADSMag9.Text
                    My.Settings.sHADSMag10 = TxtHADSMag10.Text
                    My.Settings.sHADSMag11 = TxtHADSMag11.Text
                    My.Settings.sHADSMag12 = TxtHADSMag12.Text
                    My.Settings.sHADSMag13 = TxtHADSMag13.Text
                    My.Settings.sHADSMag14 = TxtHADSMag14.Text
                    My.Settings.sHADSMag15 = TxtHADSMag15.Text
                    My.Settings.sHADSDECCutOff = Double.Parse(txtHADSDECCutOff.Text, ciClone)
                    My.Settings.sTelescopeName = TxtTelescopeName.Text
                    My.Settings.sTelescopeAparture = Convert.ToInt32(TxtTelescopeAparture.Text)
                    My.Settings.sTelescopeFocalLength = Convert.ToInt32(TxtTelescopeFocalLength.Text)
                    My.Settings.sMountTimeout = Double.Parse(TxtMountTimeout.Text, ciClone)
                    My.Settings.sCoverTimeout = Double.Parse(TxtCoverTimeout.Text, ciClone)
                    My.Settings.sCCDTimeout = Double.Parse(TxtCCDTimeout.Text, ciClone)
                    My.Settings.Save()

                    If pTheSkyXEquipmentConnected = True Then
                        pTheSkyXCamera.TemperatureSetPoint = My.Settings.sCCDCoolingTemp
                        pTheSkyXCamera.RegulateTemperature = Convert.ToInt32(My.Settings.sCCDCoolingEnabled)
                    End If

                    returnvalue = CalculateEventTimesLowIntensity()
                    If returnvalue <> "OK" Then
                        Exit Sub
                    End If

                    If TxtRoofDevice.Text = "NONE" Then
                        TxtRoofOpenPartly.Enabled = False
                        TxtRoofOpenTimeout.Enabled = False
                    Else
                        TxtRoofOpenPartly.Enabled = True
                        TxtRoofOpenTimeout.Enabled = True
                    End If
                End If
            End If
        Catch ex As Exception
            ShowMessage("FrmProperties_FormClosing: " + ex.Message, "CRITICAL", "Error!")
        End Try
    End Sub

    Private Sub TxtCCDImagePath_Validating(sender As Object, e As EventArgs) Handles TxtCCDImagePath.Validating
        If TxtCCDImagePath.Text.Last <> "\" Then
            ShowMessage("Last character should be \ !", "CRITICAL", "Invalid input.")
        End If
    End Sub

    Private Sub BTnBrowseImageLocation_Click(sender As Object, e As EventArgs)
        Try
            If (FolderBrowserDialog.ShowDialog() = DialogResult.OK) Then
                TxtCCDImagePath.Text = FolderBrowserDialog.SelectedPath
            End If
        Catch ex As Exception
            ShowMessage("BtnBrowseLog_Click: " + ex.Message, "CRITICAL", "Error!")
        End Try
    End Sub

    Private Sub BTnBrowseImageLocation_Click_1(sender As Object, e As EventArgs) Handles BtnBrowseImageLocation.Click
        Try
            If (FolderBrowserDialog.ShowDialog() = DialogResult.OK) Then
                If FolderBrowserDialog.SelectedPath.Substring(FolderBrowserDialog.SelectedPath.Length - 1, 1) = "\" Then
                    TxtCCDImagePath.Text = FolderBrowserDialog.SelectedPath
                Else
                    TxtCCDImagePath.Text = FolderBrowserDialog.SelectedPath + "\"
                End If
            End If
        Catch ex As Exception
            ShowMessage("BtnBrowseLog_Click: " + ex.Message, "CRITICAL", "Error!")
        End Try
    End Sub

    Private Sub BtnBrowseSynologyLocation_Click(sender As Object, e As EventArgs) Handles BtnBrowseSynologyLocation.Click
        Try
            If (FolderBrowserDialog.ShowDialog() = DialogResult.OK) Then
                If FolderBrowserDialog.SelectedPath.Substring(FolderBrowserDialog.SelectedPath.Length - 1, 1) = "\" Then
                    TxtSynologyPath.Text = FolderBrowserDialog.SelectedPath
                Else
                    TxtSynologyPath.Text = FolderBrowserDialog.SelectedPath + "\"
                End If
            End If
        Catch ex As Exception
            ShowMessage("BtnBrowseLog_Click: " + ex.Message, "CRITICAL", "Error!")
        End Try
    End Sub

    Private Sub BtnSentinel_Click(sender As Object, e As EventArgs) Handles BtnSentinel.Click
        Dim result As DialogResult = OpenFileDialog.ShowDialog()
        Try
            If result = Windows.Forms.DialogResult.OK Then
                TxtSentinelEXE.Text = OpenFileDialog.FileName
            End If
        Catch ex As Exception
            ShowMessage("BtnBrowseSound_Click: " + ex.Message, "CRITICAL", "Error!")
        End Try
    End Sub


    Private Sub TxtCCDImagePath_Validating(sender As Object, e As CancelEventArgs) Handles TxtCCDImagePath.Validating
        If TxtCCDImagePath.Text.Last <> "\" Then
            ShowMessage("Last character must be \ !", "CRITICAL", "Correct input!")
            TxtCCDImagePath.Focus()
        End If
    End Sub

    Private Sub TxtSynologyPath_Validating(sender As Object, e As CancelEventArgs) Handles TxtSynologyPath.Validating
        If TxtSynologyPath.Text.Last <> "\" Then
            ShowMessage("Last character must be \ !", "CRITICAL", "Correct input!")
            TxtSynologyPath.Focus()
        End If
    End Sub


    Private Sub TxtLogLocation_Validating(sender As Object, e As CancelEventArgs) Handles TxtLogLocation.Validating
        If TxtLogLocation.Text.Last <> "\" Then
            ShowMessage("Last character must be \ !", "CRITICAL", "Incorrect input...")
            TxtLogLocation.Focus()
        End If
    End Sub

    Private Sub TxtSunOpenRoof_Validating(sender As Object, e As EventArgs) Handles TxtSunOpenRoof.Validating
        Dim ciClone As CultureInfo = CType(CultureInfo.InvariantCulture.Clone(), CultureInfo)
        ciClone.NumberFormat.NumberDecimalSeparator = "."

        If Double.Parse(TxtSunOpenRoof.Text, ciClone) < 0 Then
            ShowMessage("Must be larger or equal to zero!", "CRITICAL", "Incorrect input...")
            TxtLogLocation.Focus()
        End If
    End Sub

    Private Sub TxtSunDuskFlats_Validating(sender As Object, e As EventArgs) Handles TxtSunDuskFlats.Validating
        Dim ciClone As CultureInfo = CType(CultureInfo.InvariantCulture.Clone(), CultureInfo)
        ciClone.NumberFormat.NumberDecimalSeparator = "."

        If Double.Parse(TxtSunDuskFlats.Text, ciClone) < -10 Then
            ShowMessage("Must be larger or equal than minus ten!", "CRITICAL", "Incorrect input...")
            TxtLogLocation.Focus()
        End If
    End Sub

    Private Sub TxtSunStartRun_Validating(sender As Object, e As EventArgs) Handles TxtSunStartRun.Validating
        Dim ciClone As CultureInfo = CType(CultureInfo.InvariantCulture.Clone(), CultureInfo)
        ciClone.NumberFormat.NumberDecimalSeparator = "."

        If Double.Parse(TxtSunStartRun.Text, ciClone) > 0 Then
            ShowMessage("Must be smaller than zero!", "CRITICAL", "Inorrect input...")
            TxtLogLocation.Focus()
        End If
    End Sub

    Private Sub CmbSnapCapMethod_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CmbCoverMethod.SelectedIndexChanged
        If CmbCoverMethod.Text <> "SERIAL" Then
            TxtSnapCapSerialPort.Enabled = False
            TxtCoverDevice.Enabled = False
        Else
            TxtSnapCapSerialPort.Enabled = True
            TxtCoverDevice.Enabled = False
        End If
    End Sub

    Private Sub ChkUPSNotPresent_CheckedChanged(sender As Object, e As EventArgs) Handles ChkUPSNotPresent.CheckedChanged
        If ChkUPSNotPresent.Checked = True Then
            TxtUPSCommunity.Enabled = False
            TxtUPSHost.Enabled = False
            TxtUPSrequestOid.Enabled = False
        Else
            TxtUPSCommunity.Enabled = True
            TxtUPSHost.Enabled = True
            TxtUPSrequestOid.Enabled = True
        End If
    End Sub


    Private Sub Txt7ZipLocation_Validating(sender As Object, e As EventArgs) Handles Txt7ZipLocation.Validating
        If Txt7ZipLocation.Text.Contains("7z.exe") = False Then
            ShowMessage("Must contain the path and filename of 7z.exe!", "CRITICAL", "Incorrect input...")
            Txt7ZipLocation.Focus()
        End If
    End Sub

    Private Sub BtnBrowse7Zip_Click(sender As Object, e As EventArgs) Handles BtnBrowse7Zip.Click
        Dim result As DialogResult = OpenFileDialog.ShowDialog()
        Try
            If result = Windows.Forms.DialogResult.OK Then
                Txt7ZipLocation.Text = OpenFileDialog.FileName
            End If
        Catch ex As Exception
            ShowMessage("BtnBrowseSound_Click: " + ex.Message, "CRITICAL", "Error!")
        End Try
    End Sub

    Private Sub BtnChooseMount_Click(sender As Object, e As EventArgs) Handles BtnChooseMount.Click
        Using obj As New ASCOM.Utilities.Chooser
            obj.DeviceType = "Telescope"
            TxtMountDevice.Text = obj.Choose(TxtMountDevice.Text)
        End Using
    End Sub

    Private Sub BtnChooseRoof_Click(sender As Object, e As EventArgs) Handles BtnChooseRoof.Click
        Using obj As New ASCOM.Utilities.Chooser
            obj.DeviceType = "Dome"
            TxtRoofDevice.Text = obj.Choose(TxtRoofDevice.Text)
        End Using
    End Sub

    Private Sub ChkNoRoof_CheckedChanged(sender As Object, e As EventArgs) Handles ChkNoRoof.CheckedChanged
        If ChkNoRoof.Checked = True Then
            TxtRoofDevice.Text = "NONE"
            TxtRoofDevice.Enabled = False
            BtnChooseRoof.Enabled = False
            TxtRoofOpenPartly.Enabled = False
            TxtRoofOpenTimeout.Enabled = False
        Else
            TxtRoofDevice.Text = ""
            TxtRoofDevice.Enabled = False
            BtnChooseRoof.Enabled = True
            TxtRoofOpenPartly.Enabled = True
            TxtRoofOpenTimeout.Enabled = True
        End If
    End Sub

    Private Sub TxtRoofDevice_TextChanged(sender As Object, e As EventArgs) Handles TxtRoofDevice.TextChanged
        If TxtRoofDevice.Text = "NONE" Then
            TxtRoofOpenPartly.Enabled = False
            TxtRoofOpenTimeout.Enabled = False
        Else
            TxtRoofOpenPartly.Enabled = True
            TxtRoofOpenTimeout.Enabled = True
        End If
    End Sub

    Private Sub BtnCover_Click(sender As Object, e As EventArgs) Handles BtnCover.Click
        Using obj As New ASCOM.Utilities.Chooser
            obj.DeviceType = "CoverCalibrator"
            TxtCoverDevice.Text = obj.Choose(TxtCoverDevice.Text)
        End Using
    End Sub

    Private Sub BtnChooseSwitch_Click(sender As Object, e As EventArgs) Handles BtnChooseSwitch.Click
        Using obj As New ASCOM.Utilities.Chooser
            obj.DeviceType = "Switch"
            TxtSwitchDevice.Text = obj.Choose(TxtSwitchDevice.Text)
        End Using
    End Sub

    Private Sub TxtMountDevice_TextChanged(sender As Object, e As EventArgs) Handles TxtMountDevice.TextChanged
        If TxtMountDevice.Text = "ASCOM.tenmicron_mount.Telescope" Then
            TxtMountStartupLink.Enabled = True
        Else
            TxtMountStartupLink.Enabled = False
        End If
    End Sub

    Private Sub RdWeatherUseSwitch_CheckedChanged(sender As Object, e As EventArgs) Handles RdWeatherUseSwitch.CheckedChanged
        TxtWeatherSafeSwitchDelay.Enabled = False
    End Sub

    Private Sub RdWeatherUseValues_CheckedChanged(sender As Object, e As EventArgs) Handles RdWeatherUseValues.CheckedChanged
        TxtWeatherSafeSwitchDelay.Enabled = True
    End Sub

End Class