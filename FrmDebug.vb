Imports System.Globalization

Public Class FrmDebug
    Private Sub ButtonKillInternet_Click(sender As Object, e As EventArgs) Handles ButtonKillInternet.Click
        My.Settings.sDebugKillInternet = True
        My.Settings.Save()
    End Sub

    Private Sub ButtonEnableInternet_Click(sender As Object, e As EventArgs) Handles ButtonEnableInternet.Click
        My.Settings.sDebugKillInternet = False
        My.Settings.Save()
    End Sub

    Private Sub FrmDebug_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        My.Settings.Save()
    End Sub

    Private Sub ButtonKillUPS_Click(sender As Object, e As EventArgs) Handles ButtonKillUPS.Click
        My.Settings.sDebugKillUPS = True
        My.Settings.Save()
    End Sub

    Private Sub ButtonEnableUPS_Click(sender As Object, e As EventArgs) Handles ButtonEnableUPS.Click
        My.Settings.sDebugKillUPS = False
        My.Settings.Save()
    End Sub

    Private Sub BtnSAFE_Click(sender As Object, e As EventArgs) Handles BtnSAFE.Click
        My.Settings.sDebugCloud = "SAFE"
        My.Settings.Save()
    End Sub

    Private Sub BtnUNSAFE_Click(sender As Object, e As EventArgs) Handles BtnUNSAFE.Click
        My.Settings.sDebugCloud = "UNSAFE"
        My.Settings.Save()
    End Sub

    Private Sub BtnDARK_Click(sender As Object, e As EventArgs) Handles BtnDARK.Click
        My.Settings.sDebugLight = "DARK"
        My.Settings.Save()
    End Sub

    Private Sub BtnLIGHT_Click(sender As Object, e As EventArgs) Handles BtnLIGHT.Click
        My.Settings.sDebugLight = "LIGHT"
        My.Settings.Save()
    End Sub

    Private Sub ChkDebugAAGData_CheckedChanged(sender As Object, e As EventArgs) Handles ChkDebugAAGData.CheckedChanged
        My.Settings.sDebugAAGData = ChkDebugAAGData.Checked
        My.Settings.Save()
    End Sub

    Private Sub ChkDebugCoordinates_CheckedChanged(sender As Object, e As EventArgs) Handles ChkDebugCoordinates.CheckedChanged
        My.Settings.sDebugSunMoonCoordinates = ChkDebugCoordinates.Checked
        My.Settings.Save()
    End Sub

    Private Sub CmbSunSettingRising_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CmbSunSettingRising.SelectedIndexChanged
        My.Settings.SDebugSunRisingSetting = CmbSunSettingRising.Text
        My.Settings.Save()
    End Sub

    Private Sub CmbMoonSettingRising_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CmbMoonSettingRising.SelectedIndexChanged
        My.Settings.SDebugMoonRisingSetting = CmbMoonSettingRising.Text
        My.Settings.Save()
    End Sub

    Private Sub FrmDebug_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim ciClone As CultureInfo = CType(CultureInfo.InvariantCulture.Clone(), CultureInfo)
        ciClone.NumberFormat.NumberDecimalSeparator = "."

        ChkDebugCoordinates.Checked = My.Settings.sDebugSunMoonCoordinates
        TxtSunAltitude.Text = My.Settings.SDebugSunAltitude.ToString(ciClone)
        TxtMoonAltitude.Text = My.Settings.SDebugMoonAltitude.ToString(ciClone)
        CmbSunSettingRising.Text = My.Settings.SDebugSunRisingSetting
        CmbMoonSettingRising.Text = My.Settings.SDebugMoonRisingSetting
        TxtMoonIllumination.Text = My.Settings.sDebugMoonIllumination.ToString(ciClone)
        ChkDebugAAGData.Checked = My.Settings.sDebugAAGData
    End Sub

    Private Sub TxtSunAltitude_Leave(sender As Object, e As EventArgs) Handles TxtSunAltitude.Leave
        Dim ciClone As CultureInfo = CType(CultureInfo.InvariantCulture.Clone(), CultureInfo)
        ciClone.NumberFormat.NumberDecimalSeparator = "."

        My.Settings.SDebugSunAltitude = Double.Parse(TxtSunAltitude.Text, ciClone)
        pStructEventTimes.SunAlt = Double.Parse(TxtSunAltitude.Text, ciClone)
        My.Settings.Save()
    End Sub

    Private Sub TxtMoonAltitude_Leave(sender As Object, e As EventArgs) Handles TxtMoonAltitude.Leave
        Dim ciClone As CultureInfo = CType(CultureInfo.InvariantCulture.Clone(), CultureInfo)
        ciClone.NumberFormat.NumberDecimalSeparator = "."

        My.Settings.SDebugMoonAltitude = Double.Parse(TxtMoonAltitude.Text, ciClone)
        pStructEventTimes.MoonAlt = Double.Parse(TxtMoonAltitude.Text, ciClone)
        My.Settings.Save()
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles ChkRunStatus.CheckedChanged
        If ChkRunStatus.Checked = True Then
            pRunStatus = "DONE"
            LogSessionEntry("DEBUG", "pRunStatus set to DONE", "", "CheckBox1_CheckedChanged", "PROGRAM")
        End If
        My.Settings.Save()
    End Sub

    Private Sub ButtonKillAAG_Click(sender As Object, e As EventArgs) Handles ButtonKillAAG.Click
        My.Settings.sDebugKillAAG = True
        My.Settings.Save()
    End Sub

    Private Sub ButtonEnableAAG_Click(sender As Object, e As EventArgs) Handles ButtonEnableAAG.Click
        My.Settings.sDebugKillAAG = False
        My.Settings.Save()
    End Sub

    Private Sub ChkISSequenceRunning_CheckedChanged(sender As Object, e As EventArgs) Handles ChkISSequenceRunning.CheckedChanged
        If ChkISSequenceRunning.Checked Then
            pIsSequenceRunning = True
        Else
            pIsSequenceRunning = False
        End If
        My.Settings.Save()
    End Sub

    Private Sub TxtMoonPhase_TextChanged(sender As Object, e As EventArgs) Handles TxtMoonIllumination.TextChanged
        Dim ciClone As CultureInfo = CType(CultureInfo.InvariantCulture.Clone(), CultureInfo)
        ciClone.NumberFormat.NumberDecimalSeparator = "."

        My.Settings.sDebugMoonIllumination = Double.Parse(TxtMoonIllumination.Text, ciClone)
        My.Settings.Save()
    End Sub

    Private Sub TimerDebug_Tick(sender As Object, e As EventArgs) Handles TimerDebug.Tick
        LblpAbort.Text = "pAbort: " + Format(pAbort)
        LblpIsSequenceRunning.Text = "pIsSequenceRunning: " + Format(pIsSequenceRunning)
        LblpIsActionRunning.Text = "pIsActionRunning: " + Format(pIsActionRunning)
        LblpRunStatus.Text = "pRunStatus: " + Format(pRunStatus)
        LblpManualAbort.Text = "pManualAbort: " + Format(pManualAbort)
        LblStartRun.Text = "pStartRun: " + Format(pStartRun)

        LblpIsEquipmentInitializing.Text = "pIsEquipmentInitializing: " + Format(pIsEquipmentInitializing)
        LblpEquipmentStatus.Text = "pEquipmentStatus: " + Format(pEquipmentStatus)

        LblpSmartError.Text = "pSmartError: " + Format(pSmartError)
        LblpOldProgramTimeSMART.Text = "pOldProgramTimeSMART: " + Format(pOldProgramTimeSMART)
        lblpOldSequenceTimeSMART.Text = "pOldSequenceTimeSMART: " + Format(pOldSequenceTimeSMART)

        LblMoonSafetyStatus.Text = "pMoonSafetyStatus: " + pStructEventTimes.MoonSafetyStatus
        LblpMoonCooldownStatus.Text = "pMoonCooldownStatus: " + pStructEventTimes.MoonCooldownStatus


    End Sub

    Private Sub BtnShowTargets_Click(sender As Object, e As EventArgs) Handles BtnShowTargets.Click
        If ShowMessage("Show unsafe targets ?", "YESNO", "What targets should be shown?") = vbYes Then
            DatabaseSelectDeepSky(False, True)
        Else
            DatabaseSelectDeepSky(False, False)
        End If

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim PanelCenterString, Panel1String, Panel2String, Panel3String, Panel4String As String
        Dim ciClone As CultureInfo = CType(CultureInfo.InvariantCulture.Clone(), CultureInfo)
        ciClone.NumberFormat.NumberDecimalSeparator = "."

        CalculateMosaic(Double.Parse(TxtMosaicCenterRA.Text, ciClone), Double.Parse(TxtMosaicCenterDEC.Text, ciClone), CmbMosaicType.Text, Convert.ToInt32(TxtMosaicOverlap.Text))

        PanelCenterString = "Center: RA " + pAUtil.HoursToHMS(Double.Parse(TxtMosaicCenterRA.Text, ciClone), "h ", "m ", "s ") + " - DEC " + pAUtil.DegreesToDMS(Double.Parse(TxtMosaicCenterDEC.Text, ciClone), "° ", "' ", """ ")
        Panel1String = "Panel 1: RA " + pAUtil.HoursToHMS(pStructMosaic.Panel1_RA2000, "h ", "m ", "s ") + " - DEC " + pAUtil.DegreesToDMS(pStructMosaic.Panel1_DEC2000, "° ", "' ", """ ")
        Panel2String = "Panel 2: RA " + pAUtil.HoursToHMS(pStructMosaic.Panel2_RA2000, "h ", "m ", "s ") + " - DEC " + pAUtil.DegreesToDMS(pStructMosaic.Panel2_DEC2000, "° ", "' ", """ ")
        Panel3String = "Panel 3: RA " + pAUtil.HoursToHMS(pStructMosaic.Panel3_RA2000, "h ", "m ", "s ") + " - DEC " + pAUtil.DegreesToDMS(pStructMosaic.Panel3_DEC2000, "° ", "' ", """ ")
        Panel4String = "Panel 4: RA " + pAUtil.HoursToHMS(pStructMosaic.Panel4_RA2000, "h ", "m ", "s ") + " - DEC " + pAUtil.DegreesToDMS(pStructMosaic.Panel4_DEC2000, "° ", "' ", """ ")


        TxtMosaic.Text = PanelCenterString + vbCrLf + Panel1String + vbCrLf + Panel2String + vbCrLf + Panel3String + vbCrLf + Panel4String

    End Sub

    Private Sub BtnError_Click(sender As Object, e As EventArgs) Handles BtnError.Click
        LogSessionEntry("ERROR", "SOME ERROR", "", "SOMETHING", "TSX")
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        DatabaseSelectHADS(True)
    End Sub
End Class