Imports System.Globalization
Imports System.Linq.Expressions

Public Class FrmCalibration

    Private Sub BtnStart_Click(sender As Object, e As EventArgs) Handles BtnStart.Click
        Dim FilterNumber, Binning As Integer
        Dim returnvalue As String

        Try
            Dim ciClone As CultureInfo = CType(CultureInfo.InvariantCulture.Clone(), CultureInfo)
            ciClone.NumberFormat.NumberDecimalSeparator = "."

            If pStartRun = True Then
                'sequence already running
                MsgBox("Cannot start, there is already a run in progesss...", vbCritical, "Calibration frames...")
            Else
                'reset abort flag
                pAbort = False

                'check status Snapcap: should be closed
                If pTheSkyXEquipmentConnected = False Then
                    MsgBox("CCD is not connected... use tools to turn on.", vbCritical, "Calibration frames...")
                Else
                    If pCoverStatus <> 1 And My.Settings.sCoverMethod <> "NONE" Then 'closed
                        MsgBox("First close the cover.", vbCritical, "Calibration frames...")
                    Else
                        pContinueRunningCalibrationFrames = True
                        BtnStart.Enabled = False
                        BtnStop.Enabled = True

                        'check the camera is at the proper temperature
                        'Do While Math.Abs(pTheSkyXCCDTemp) + My.Settings.sCCDCoolingTemp > 1
                        Do While pTheSkyXCCDTemp >= My.Settings.sCCDCoolingTemp + 1
                            LogSessionEntry("BRIEF", "Waiting for camera to cool down...", "", "BtnStart_Click", "PROGRAM")
                            WaitSeconds(30, False, True)
                        Loop

                        LogSessionEntry("BRIEF", "Camera is cooled down...", "", "BtnStart_Click", "PROGRAM")

                        If ChkBias.Checked = True Then

                            'retrieve filternumber, the idea is of using the H filter to minimize light transfer
                            If CmbTargetFilter.Text = My.Settings.sCCDFilter1 Then
                                FilterNumber = 0
                            ElseIf CmbTargetFilter.Text = My.Settings.sCCDFilter2 Then
                                FilterNumber = 1
                            ElseIf CmbTargetFilter.Text = My.Settings.sCCDFilter3 Then
                                FilterNumber = 2
                            ElseIf CmbTargetFilter.Text = My.Settings.sCCDFilter4 Then
                                FilterNumber = 3
                            ElseIf CmbTargetFilter.Text = My.Settings.sCCDFilter5 Then
                                FilterNumber = 4
                            End If

                            If CmbTargetBinning.Text = "1x1" Then
                                Binning = 1
                            ElseIf CmbTargetBinning.Text = "2x2" Then
                                Binning = 2
                            ElseIf CmbTargetBinning.Text = "3x3" Then
                                Binning = 3
                            End If


                            returnvalue = RunCalibrationFrames("BIAS", Integer.Parse(TxtBiasNumberExposures.Text), 0.001, Binning, FilterNumber, CmbTargetFilter.Text, "bias calibration frame")
                            If returnvalue <> "OK" Then
                                pContinueRunningCalibrationFrames = False
                                Exit Sub
                            End If
                        End If

                        'retrieve filternumber, the idea is of using the H filter to minimize light transfer
                        If CmbTargetFilter.Text = My.Settings.sCCDFilter1 Then
                            FilterNumber = 0
                        ElseIf CmbTargetFilter.Text = My.Settings.sCCDFilter2 Then
                            FilterNumber = 1
                        ElseIf CmbTargetFilter.Text = My.Settings.sCCDFilter3 Then
                            FilterNumber = 2
                        ElseIf CmbTargetFilter.Text = My.Settings.sCCDFilter4 Then
                            FilterNumber = 3
                        ElseIf CmbTargetFilter.Text = My.Settings.sCCDFilter5 Then
                            FilterNumber = 4
                        End If

                        If CmbTargetBinning.Text = "1x1" Then
                            Binning = 1
                        ElseIf CmbTargetBinning.Text = "2x2" Then
                            Binning = 2
                        ElseIf CmbTargetBinning.Text = "3x3" Then
                            Binning = 3
                        End If

                        If ChkDarks1.Checked = True Then
                            returnvalue = RunCalibrationFrames("DARK", Integer.Parse(TxtDarkNumberExposures.Text), Double.Parse(TxtDarkExposureDuration1.Text, ciClone), Binning, FilterNumber, CmbTargetFilter.Text, "dark calibration frame")
                            If returnvalue <> "OK" Then
                                pContinueRunningCalibrationFrames = False
                                Exit Sub
                            End If
                        End If

                        If ChkDarks2.Checked = True Then
                            returnvalue = RunCalibrationFrames("DARK", Integer.Parse(TxtDarkNumberExposures.Text), Double.Parse(TxtDarkExposureDuration2.Text, ciClone), Binning, FilterNumber, CmbTargetFilter.Text, "dark calibration frame")
                            If returnvalue <> "OK" Then
                                pContinueRunningCalibrationFrames = False
                                Exit Sub
                            End If
                        End If

                        If ChkDarks3.Checked = True Then
                            returnvalue = RunCalibrationFrames("DARK", Integer.Parse(TxtDarkNumberExposures.Text), Double.Parse(TxtDarkExposureDuration3.Text, ciClone), Binning, FilterNumber, CmbTargetFilter.Text, "dark calibration frame")
                            If returnvalue <> "OK" Then
                                pContinueRunningCalibrationFrames = False
                                Exit Sub
                            End If
                        End If

                        If ChkDarks4.Checked = True Then
                            returnvalue = RunCalibrationFrames("DARK", Integer.Parse(TxtDarkNumberExposures.Text), Double.Parse(TxtDarkExposureDuration4.Text, ciClone), Binning, FilterNumber, CmbTargetFilter.Text, "dark calibration frame")
                            If returnvalue <> "OK" Then
                                pContinueRunningCalibrationFrames = False
                                Exit Sub
                            End If
                        End If


                        If ChkDarks5.Checked = True Then
                            returnvalue = RunCalibrationFrames("DARK", Integer.Parse(TxtDarkNumberExposures.Text), Double.Parse(TxtDarkExposureDuration5.Text, ciClone), Binning, FilterNumber, CmbTargetFilter.Text, "dark calibration frame")
                            If returnvalue <> "OK" Then
                                pContinueRunningCalibrationFrames = False
                                Exit Sub
                            End If
                        End If

                        BtnStart.Enabled = True
                        BtnStop.Enabled = False
                    End If
                End If

            End If

        Catch ex As Exception
            MsgBox("BtnStart_Click: " + ex.Message)
            pContinueRunningCalibrationFrames = False
            BtnStart.Enabled = True
            BtnStop.Enabled = False
        End Try

    End Sub

    Private Sub FrmCalibration_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            'add combobox options
            CmbTargetFilter.Items.Add(My.Settings.sCCDFilter1)
            CmbTargetFilter.Items.Add(My.Settings.sCCDFilter2)
            CmbTargetFilter.Items.Add(My.Settings.sCCDFilter3)
            CmbTargetFilter.Items.Add(My.Settings.sCCDFilter4)
            CmbTargetFilter.Items.Add(My.Settings.sCCDFilter5)
        Catch ex As Exception
            MsgBox("FrmCalibration_Load: " + ex.Message)
        End Try
    End Sub

    Private Sub BtnStop_Click(sender As Object, e As EventArgs) Handles BtnStop.Click
        Try
            pContinueRunningCalibrationFrames = False
            BtnStart.Enabled = True
            BtnStop.Enabled = False
        Catch ex As Exception
            MsgBox("BtnStop_Click: " + ex.Message)
        End Try

    End Sub
End Class