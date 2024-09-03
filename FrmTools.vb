Imports System.ComponentModel
Imports System.Globalization
Imports System.Net.NetworkInformation
Imports ASCOM.Astrometry
Imports ASCOM.DeviceInterface

Public Class FrmTools


    Private Sub LblRoof_Click(sender As Object, e As EventArgs) Handles LblRoof.Click
        Dim ReturnValue As String
        Try
            If My.Settings.sRoofDevice = "NONE" Then
                'do nothing, no roof present
            Else
                ReturnValue = RoofShutterStatus()
                If ReturnValue <> "OK" Then
                    ShowDefaultCursor()
                    Exit Sub
                End If

                If pRoofShutterStatus = "CLOSED" Or pRoofShutterStatus = "CLOSING" Then
                    ShowWaitCursor()
                    ReturnValue = RoofOpenRoof()
                    If ReturnValue <> "OK" Then
                        ShowDefaultCursor()
                        Exit Sub
                    End If
                ElseIf pRoofShutterStatus = "OPEN" Or pRoofShutterStatus = "OPENING" Then
                    ShowWaitCursor()
                    ReturnValue = RoofCloseRoof()
                    If ReturnValue <> "OK" Then
                        ShowDefaultCursor()
                        Exit Sub
                    End If
                End If

                'set equipent status on PARTIAL when manual change was made => disaster check will close roof
                pEquipmentStatus = "PARTIAL"

                TimerRoof_Tick(Nothing, Nothing)
                ShowDefaultCursor()
            End If
        Catch ex As Exception
            LogSessionEntry("ERROR", "TxtRoofShutterStatus_Click: " + Err.ToString(), "", "TxtRoofShutterStatus_Click", "PROGRAM")
            ShowDefaultCursor()
        End Try
    End Sub

    Private Sub TimerRoof_Tick(sender As Object, e As EventArgs) Handles TimerRoof.Tick
        Try
            If My.Settings.sRoofDevice = "NONE" Then
                LblRoof.BackColor = Color.Transparent
                LblRoof.Text = "No roof"
            Else
                If pRoofConnected = True Then
                    LblRoof.Text = "Roof " + pRoofShutterStatus

                    If pRoofShutterStatus = "CLOSED" Then
                        LblRoof.BackColor = ColorTranslator.FromHtml("#d63031") 'red
                        LblRoof.Text = "Roof CLOSED"
                    ElseIf pRoofShutterStatus = "OPENING" Then
                        LblRoof.BackColor = Color.Orange
                        LblRoof.Text = "Roof OPENING..."
                    ElseIf pRoofShutterStatus = "CLOSING" Then
                        LblRoof.BackColor = Color.Orange
                        LblRoof.Text = "Roof CLOSING..."
                    ElseIf pRoofShutterStatus = "OPEN" Then
                        LblRoof.BackColor = ColorTranslator.FromHtml("#4cd137") 'green
                        LblRoof.Text = "Roof OPEN"
                    Else
                        LblRoof.BackColor = Color.Purple
                        LblRoof.Text = "Roof " + pRoofShutterStatus
                    End If
                End If
            End If
        Catch ex As Exception
            LogSessionEntry("ERROR", "TimerRoof_Tick: " + ex.Message, "", "TimerRoof_Tick", "PROGRAM")
        End Try
    End Sub

    Private Sub FrmTools_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        pToolsAbort = False

        Try
            If pTheSkyXEquipmentConnected = True Then
                BtnTakeImage.Enabled = True
            Else
                BtnTakeImage.Enabled = False
            End If

            BtnAbortImage.Enabled = False
            ChkOpenRoof.Checked = My.Settings.sToolsOpenRoof

            'add combobox options
            CmbTargetFilter.Items.Add(My.Settings.sCCDFilter1)
            CmbTargetFilter.Items.Add(My.Settings.sCCDFilter2)
            CmbTargetFilter.Items.Add(My.Settings.sCCDFilter3)
            CmbTargetFilter.Items.Add(My.Settings.sCCDFilter4)
            CmbTargetFilter.Items.Add(My.Settings.sCCDFilter5)

            TimerCover.Enabled = True
            TimerCCDFocusser.Enabled = True
            TimerSwitch.Enabled = True
            TimerRoof.Enabled = True
            TimerMount.Enabled = True

            If My.Settings.sRoofDevice = "NONE" Then
                BtnRoofOpenPCT.Enabled = False
            Else
                BtnRoofOpenPCT.Enabled = True
            End If

            If My.Settings.sCoverMethod = "NONE" Then
                BtnConnectCover.Enabled = False
                BtnDisconnectCover.Enabled = False
            Else
                BtnConnectCover.Enabled = True
                BtnDisconnectCover.Enabled = True
            End If

            TimerCover_Tick(Nothing, Nothing)
            TimerFocusser_Tick(Nothing, Nothing)
            TimerSwitch_Tick(Nothing, Nothing)
            TimerRoof_Tick(Nothing, Nothing)
            TimerMount_Tick(Nothing, Nothing)
        Catch ex As Exception
            LogSessionEntry("ERROR", "FrmTools_Load: " + ex.Message, "", "FrmTools_Load", "PROGRAM")
        End Try

    End Sub

    Private Sub BtnRoofOpenPCT_Click(sender As Object, e As EventArgs) Handles BtnRoofOpenPCT.Click

        Dim returnValue As String

        Try
            ShowWaitCursor()
            returnValue = RoofOpenShutterPct(My.Settings.sRoofOpenPartly)
            If returnValue <> "OK" Then
                ShowDefaultCursor()
                Exit Sub
            End If

            'set equipent status on PARTIAL when manual change was made => disaster check will close roof
            pEquipmentStatus = "PARTIAL"

            ShowDefaultCursor()
        Catch ex As Exception
            LogSessionEntry("ERROR", "BtnRoofOpenPCT_Click: " + ex.Message, "", "BtnRoofOpenPCT_Click", "PROGRAM")
            ShowDefaultCursor()
        End Try


    End Sub

    Private Sub TimerCover_Tick(sender As Object, e As EventArgs) Handles TimerCover.Tick
        Try
            If pCoverConnected = True Then
                'NotPresent	0	This device does not have a cover that can be closed independently
                'Closed  1	The cover Is closed
                'Moving  2	The cover Is moving to a New position
                'Open    3	The cover Is open
                'Unknown 4	The state of the cover Is unknown
                'Error 5	The device encountered an Error When changing state 
                LblCover.Text = "Cover"

                If pCoverStatus = 1 Then
                    LblCover.BackColor = ColorTranslator.FromHtml("#d63031") 'red
                    LblCover.Text = "Cover CLOSED"
                ElseIf pCoverStatus = 2 Then
                    LblCover.BackColor = Color.Orange
                    LblCover.Text = "Cover MOVING"
                ElseIf pCoverStatus = 3 Then
                    LblCover.BackColor = ColorTranslator.FromHtml("#4cd137") 'green
                    LblCover.Text = "Cover OPEN"
                ElseIf pCoverStatus = 4 Then
                    LblCover.BackColor = Color.Purple
                    LblCover.Text = "Cover UNKNOWN"
                Else
                    LblCover.BackColor = Color.Purple
                    LblCover.Text = "Cover UNKNOWN"
                End If
                BtnConnectCover.BackColor = ColorTranslator.FromHtml("#4cd137") 'green
                BtnDisconnectCover.BackColor = ColorTranslator.FromHtml("#0066CC") 'blue
                BtnConnectCover.Enabled = False
                BtnDisconnectCover.Enabled = True
            Else
                BtnDisconnectCover.BackColor = ColorTranslator.FromHtml("#d63031") 'red
                BtnConnectCover.BackColor = ColorTranslator.FromHtml("#0066CC") 'blue
                LblCover.Text = ""
                LblCover.BackColor = Color.Silver
                BtnConnectCover.Enabled = True
                BtnDisconnectCover.Enabled = False
            End If

            If My.Settings.sCoverMethod = "NONE" Then
                BtnConnectCover.Enabled = False
                BtnDisconnectCover.Enabled = False
            Else
                BtnConnectCover.Enabled = True
                BtnDisconnectCover.Enabled = True
            End If

        Catch ex As Exception
            LogSessionEntry("ERROR", "TimerCover_Tick:  " + ex.Message, "", "TimerCover_Tick", "PROGRAM")
        End Try

    End Sub

    Private Sub LblCover_Click(sender As Object, e As EventArgs) Handles LblCover.Click
        Dim returnvalue As String
        Try

            If pCoverStatus = 1 Or pCoverStatus = 2 Then
                ShowWaitCursor()
                returnvalue = CoverOpen()
                If returnvalue <> "OK" Then
                    ShowDefaultCursor()
                    Exit Sub
                End If

                'set equipent status on on when manual change was made => disaster check will close roof
                pEquipmentStatus = "ON"

            ElseIf pCoverStatus = 3 Or pCoverStatus = 2 Then
                ShowWaitCursor()
                returnvalue = CoverClose()
                If returnvalue <> "OK" Then
                    ShowDefaultCursor()
                    Exit Sub
                End If
            End If

            'set equipent status on PARTIAL when manual change was made => disaster check will close roof
            pEquipmentStatus = "PARTIAL"

            TimerCover_Tick(Nothing, Nothing)
            ShowDefaultCursor()
        Catch ex As Exception
            LogSessionEntry("ERROR", "lblCoverStatus_Click: " + Err.ToString(), "", "lblCoverStatus_Click", "PROGRAM")
            ShowDefaultCursor()
        End Try
    End Sub

    Private Sub TimerSwitch_Tick(sender As Object, e As EventArgs) Handles TimerSwitch.Tick
        'get status and fill the fields

        Try
            'check Switch
            If pSwitchConnected = True Then
                'fill switch names
                lblSwitch1Status.Text = My.Settings.sSwitch1Name
                lblSwitch2Status.Text = My.Settings.sSwitch2Name
                lblSwitch3Status.Text = My.Settings.sSwitch3Name
                lblSwitch4Status.Text = My.Settings.sSwitch4Name
                lblSwitch5Status.Text = My.Settings.sSwitch5Name
                lblSwitch6Status.Text = My.Settings.sSwitch6Name
                lblSwitch7Status.Text = My.Settings.sSwitch7Name
                lblSwitch8Status.Text = My.Settings.sSwitch8Name

                'fill the fields
                If pStructSwitch.Switch1Status = True Then
                    lblSwitch1Status.BackColor = ColorTranslator.FromHtml("#4cd137") 'green
                Else
                    lblSwitch1Status.BackColor = ColorTranslator.FromHtml("#d63031") 'red
                End If

                If pStructSwitch.Switch2Status = True Then
                    lblSwitch2Status.BackColor = ColorTranslator.FromHtml("#4cd137") 'green
                Else
                    lblSwitch2Status.BackColor = ColorTranslator.FromHtml("#d63031") 'red
                End If

                If pStructSwitch.Switch3Status = True Then
                    lblSwitch3Status.BackColor = ColorTranslator.FromHtml("#4cd137") 'green
                Else
                    lblSwitch3Status.BackColor = ColorTranslator.FromHtml("#d63031") 'red
                End If

                If pStructSwitch.Switch4Status = True Then
                    lblSwitch4Status.BackColor = ColorTranslator.FromHtml("#4cd137") 'green
                Else
                    lblSwitch4Status.BackColor = ColorTranslator.FromHtml("#d63031") 'red
                End If

                If pStructSwitch.Switch5Status = True Then
                    lblSwitch5Status.BackColor = ColorTranslator.FromHtml("#4cd137") 'green
                Else
                    lblSwitch5Status.BackColor = ColorTranslator.FromHtml("#d63031") 'red
                End If

                If pStructSwitch.Switch6Status = True Then
                    lblSwitch6Status.BackColor = ColorTranslator.FromHtml("#4cd137") 'green
                Else
                    lblSwitch6Status.BackColor = ColorTranslator.FromHtml("#d63031") 'red
                End If

                If pStructSwitch.Switch7Status = True Then
                    lblSwitch7Status.BackColor = ColorTranslator.FromHtml("#4cd137") 'green
                Else
                    lblSwitch7Status.BackColor = ColorTranslator.FromHtml("#d63031") 'red
                End If

                If pStructSwitch.Switch8Status = True Then
                    lblSwitch8Status.BackColor = ColorTranslator.FromHtml("#4cd137") 'green
                Else
                    lblSwitch8Status.BackColor = ColorTranslator.FromHtml("#d63031") 'red
                End If
            End If
        Catch ex As Exception
            LogSessionEntry("ERROR", "TimerSwitch_Tick: " + Err.ToString(), "", "TimerSwitch_Tick", "PROGRAM")
        End Try

    End Sub

    Private Sub BtnStartAllEquipment_Click(sender As Object, e As EventArgs) Handles btnStartAllEquipment.Click
        'wait 3 seconds between turning equipment on/off
        Dim returnvalue As String
        Try

            ShowWaitCursor()
            returnvalue = SwitchEquipmentOn()
            If returnvalue <> "OK" Then
                ShowDefaultCursor()
                Exit Sub
            End If
            ShowDefaultCursor()

        Catch ex As Exception
            LogSessionEntry("ERROR", "BtnStartAllEquipment_Click: " + Err.ToString(), "", "TimerSwitch_Tick", "PROGRAM")
        End Try
    End Sub

    Private Sub LblSwitch1Status_Click(sender As Object, e As EventArgs) Handles lblSwitch1Status.Click
        Dim returnvalue As String
        Try
            If My.Settings.sSwitch1Warning <> "" Then
                If ShowMessage(My.Settings.sSwitch1Warning, "OKONLY", "Warning") = vbOK Then
                    If lblSwitch1Status.BackColor = ColorTranslator.FromHtml("#4cd137") Then 'green
                        ShowWaitCursor()
                        returnvalue = SwitchEnable(0, False)
                        If returnvalue <> "OK" Then
                            ShowDefaultCursor()
                            Exit Sub
                        End If
                        ShowDefaultCursor()
                    Else
                        ShowWaitCursor()
                        returnvalue = SwitchEnable(0, True)
                        If returnvalue <> "OK" Then
                            ShowDefaultCursor()
                            Exit Sub
                        End If
                        ShowDefaultCursor()
                    End If
                End If
            Else
                If lblSwitch1Status.BackColor = ColorTranslator.FromHtml("#4cd137") Then 'green
                    ShowWaitCursor()
                    returnvalue = SwitchEnable(0, False)
                    If returnvalue <> "OK" Then
                        ShowDefaultCursor()
                        Exit Sub
                    End If
                    ShowDefaultCursor()
                Else
                    ShowWaitCursor()
                    returnvalue = SwitchEnable(0, True)
                    If returnvalue <> "OK" Then
                        ShowDefaultCursor()
                        Exit Sub
                    End If
                    ShowDefaultCursor()
                End If
            End If

            TimerSwitch_Tick(Nothing, Nothing)
            ShowDefaultCursor()
            'set equipent status on on when manual change was made => disaster check will close roof
            pEquipmentStatus = "ON"

        Catch ex As Exception
            ShowDefaultCursor()
            LogSessionEntry("ERROR", "lblSwitch1Status_Click: " + Err.ToString(), "", "lblSwitch1Status_Click", "PROGRAM")
        End Try

    End Sub

    Private Sub LblSwitch2Status_Click(sender As Object, e As EventArgs) Handles lblSwitch2Status.Click
        Dim returnvalue As String

        Try
            If My.Settings.sSwitch2Warning <> "" Then
                If ShowMessage(My.Settings.sSwitch2Warning, "OKONLY", "Warning") = vbOK Then
                    If lblSwitch2Status.BackColor = ColorTranslator.FromHtml("#4cd137") Then 'green
                        ShowWaitCursor()
                        returnvalue = SwitchEnable(1, False)
                        If returnvalue <> "OK" Then
                            ShowDefaultCursor()
                            Exit Sub
                        End If
                        ShowDefaultCursor()
                    Else
                        ShowWaitCursor()
                        returnvalue = SwitchEnable(1, True)
                        If returnvalue <> "OK" Then
                            ShowDefaultCursor()
                            Exit Sub
                        End If
                        ShowDefaultCursor()
                    End If
                End If
            Else
                If lblSwitch2Status.BackColor = ColorTranslator.FromHtml("#4cd137") Then 'green
                    ShowWaitCursor()
                    returnvalue = SwitchEnable(1, False)
                    If returnvalue <> "OK" Then
                        ShowDefaultCursor()
                        Exit Sub
                    End If
                    ShowDefaultCursor()
                Else
                    ShowWaitCursor()
                    returnvalue = SwitchEnable(1, True)
                    If returnvalue <> "OK" Then
                        ShowDefaultCursor()
                        Exit Sub
                    End If
                    ShowDefaultCursor()
                End If
            End If
            TimerSwitch_Tick(Nothing, Nothing)
            ShowDefaultCursor()

            'set equipent status on on when manual change was made => disaster check will close roof
            pEquipmentStatus = "ON"

        Catch ex As Exception
            ShowDefaultCursor()
            LogSessionEntry("ERROR", "lblSwitch2Status_Click: " + Err.ToString(), "", "lblSwitch2Status_Click", "PROGRAM")
        End Try
    End Sub

    Private Sub LblSwitch3Status_Click(sender As Object, e As EventArgs) Handles lblSwitch3Status.Click
        Dim returnvalue As String

        Try
            If My.Settings.sSwitch3Warning <> "" Then
                If ShowMessage(My.Settings.sSwitch3Warning, "OKONLY", "Warning") = vbOK Then
                    If lblSwitch3Status.BackColor = ColorTranslator.FromHtml("#4cd137") Then 'green
                        ShowWaitCursor()
                        returnvalue = SwitchEnable(2, False)
                        If returnvalue <> "OK" Then
                            ShowDefaultCursor()
                            Exit Sub
                        End If
                        ShowDefaultCursor()
                    Else
                        ShowWaitCursor()
                        returnvalue = SwitchEnable(2, True)
                        If returnvalue <> "OK" Then
                            ShowDefaultCursor()
                            Exit Sub
                        End If
                        ShowDefaultCursor()
                    End If
                End If
            Else
                If lblSwitch3Status.BackColor = ColorTranslator.FromHtml("#4cd137") Then 'green
                    ShowWaitCursor()
                    returnvalue = SwitchEnable(2, False)
                    If returnvalue <> "OK" Then
                        ShowDefaultCursor()
                        Exit Sub
                    End If
                Else
                    ShowWaitCursor()
                    returnvalue = SwitchEnable(2, True)
                    If returnvalue <> "OK" Then
                        ShowDefaultCursor()
                        Exit Sub
                    End If
                End If
            End If
            TimerSwitch_Tick(Nothing, Nothing)
            ShowDefaultCursor()

            'set equipent status on on when manual change was made => disaster check will close roof
            pEquipmentStatus = "ON"

        Catch ex As Exception
            ShowDefaultCursor()
            LogSessionEntry("ERROR", "lblSwitch3Status_Click: " + Err.ToString(), "", "lblSwitch3Status_Click", "PROGRAM")
        End Try
    End Sub

    Private Sub LblSwitch4Status_Click(sender As Object, e As EventArgs) Handles lblSwitch4Status.Click
        Dim returnvalue As String

        Try
            If My.Settings.sSwitch4Warning <> "" Then
                If ShowMessage(My.Settings.sSwitch4Warning, "OKONLY", "Warning") = vbOK Then
                    If lblSwitch4Status.BackColor = ColorTranslator.FromHtml("#4cd137") Then 'green
                        ShowWaitCursor()
                        returnvalue = SwitchEnable(3, False)
                        If returnvalue <> "OK" Then
                            ShowDefaultCursor()
                            Exit Sub
                        End If
                        ShowDefaultCursor()
                    Else
                        ShowWaitCursor()
                        returnvalue = SwitchEnable(3, True)
                        If returnvalue <> "OK" Then
                            ShowDefaultCursor()
                            Exit Sub
                        End If
                        ShowDefaultCursor()
                    End If
                End If
            Else
                If lblSwitch4Status.BackColor = ColorTranslator.FromHtml("#4cd137") Then 'green
                    ShowWaitCursor()
                    returnvalue = SwitchEnable(3, False)
                    If returnvalue <> "OK" Then
                        ShowDefaultCursor()
                        Exit Sub
                    End If
                    ShowDefaultCursor()
                Else
                    ShowWaitCursor()
                    returnvalue = SwitchEnable(3, True)
                    If returnvalue <> "OK" Then
                        ShowDefaultCursor()
                        Exit Sub
                    End If
                    ShowDefaultCursor()
                End If
            End If
            TimerSwitch_Tick(Nothing, Nothing)
            ShowDefaultCursor()

            'set equipent status on on when manual change was made => disaster check will close roof
            pEquipmentStatus = "ON"

        Catch ex As Exception
            ShowDefaultCursor()
            LogSessionEntry("ERROR", "lblSwitch4Status_Click: " + Err.ToString(), "", "lblSwitch4Status_Click", "PROGRAM")
        End Try
    End Sub

    Private Sub LblSwitch5Status_Click(sender As Object, e As EventArgs) Handles lblSwitch5Status.Click
        Dim returnvalue As String

        Try
            If My.Settings.sSwitch5Warning <> "" Then
                If ShowMessage(My.Settings.sSwitch5Warning, "OKONLY", "Warning") = vbOK Then
                    If lblSwitch5Status.BackColor = ColorTranslator.FromHtml("#4cd137") Then 'green
                        ShowWaitCursor()
                        returnvalue = SwitchEnable(4, False)
                        If returnvalue <> "OK" Then
                            ShowDefaultCursor()
                            Exit Sub
                        End If
                        ShowDefaultCursor()
                    Else
                        ShowWaitCursor()
                        returnvalue = SwitchEnable(4, True)
                        If returnvalue <> "OK" Then
                            ShowDefaultCursor()
                            Exit Sub
                        End If
                        ShowDefaultCursor()
                    End If
                End If
            Else
                If lblSwitch5Status.BackColor = ColorTranslator.FromHtml("#4cd137") Then 'green
                    ShowWaitCursor()
                    returnvalue = SwitchEnable(4, False)
                    If returnvalue <> "OK" Then
                        ShowDefaultCursor()
                        Exit Sub
                    End If
                    ShowDefaultCursor()
                Else
                    ShowWaitCursor()
                    returnvalue = SwitchEnable(4, True)
                    If returnvalue <> "OK" Then
                        ShowDefaultCursor()
                        Exit Sub
                    End If
                    ShowDefaultCursor()
                End If
            End If
            TimerSwitch_Tick(Nothing, Nothing)
            ShowDefaultCursor()

            'set equipent status on on when manual change was made => disaster check will close roof
            pEquipmentStatus = "ON"

        Catch ex As Exception
            ShowDefaultCursor()
            LogSessionEntry("ERROR", "lblSwitch5Status_Click: " + Err.ToString(), "", "lblSwitch5Status_Click", "PROGRAM")
        End Try
    End Sub

    Private Sub LblSwitch6Status_Click(sender As Object, e As EventArgs) Handles lblSwitch6Status.Click
        Dim returnvalue As String

        Try
            If My.Settings.sSwitch6Warning <> "" Then
                If ShowMessage(My.Settings.sSwitch6Warning, "OKONLY", "Warning") = vbOK Then
                    If lblSwitch6Status.BackColor = ColorTranslator.FromHtml("#4cd137") Then 'green
                        ShowWaitCursor()
                        returnvalue = SwitchEnable(5, False)
                        If returnvalue <> "OK" Then
                            ShowDefaultCursor()
                            Exit Sub
                        End If
                        ShowDefaultCursor()
                    Else
                        ShowWaitCursor()
                        returnvalue = SwitchEnable(5, True)
                        If returnvalue <> "OK" Then
                            ShowDefaultCursor()
                            Exit Sub
                        End If
                        ShowDefaultCursor()
                    End If
                End If
            Else
                If lblSwitch6Status.BackColor = ColorTranslator.FromHtml("#4cd137") Then 'green
                    ShowWaitCursor()
                    returnvalue = SwitchEnable(5, False)
                    If returnvalue <> "OK" Then
                        ShowDefaultCursor()
                        Exit Sub
                    End If
                    ShowDefaultCursor()
                Else
                    ShowWaitCursor()
                    returnvalue = SwitchEnable(5, True)
                    If returnvalue <> "OK" Then
                        ShowDefaultCursor()
                        Exit Sub
                    End If
                    ShowDefaultCursor()
                End If
            End If
            TimerSwitch_Tick(Nothing, Nothing)
            ShowDefaultCursor()

            'set equipent status on on when manual change was made => disaster check will close roof
            pEquipmentStatus = "ON"

        Catch ex As Exception
            ShowDefaultCursor()
            LogSessionEntry("ERROR", "lblSwitch6Status_Click: " + Err.ToString(), "", "lblSwitch6Status_Click", "PROGRAM")
        End Try
    End Sub

    Private Sub LblSwitch7Status_Click(sender As Object, e As EventArgs) Handles lblSwitch7Status.Click
        Dim returnvalue As String

        Try
            If My.Settings.sSwitch7Warning <> "" Then
                If ShowMessage(My.Settings.sSwitch7Warning, "OKONLY", "Warning") = vbOK Then
                    If lblSwitch7Status.BackColor = ColorTranslator.FromHtml("#4cd137") Then 'green
                        ShowWaitCursor()
                        returnvalue = SwitchEnable(6, False)
                        If returnvalue <> "OK" Then
                            ShowDefaultCursor()
                            Exit Sub
                        End If
                        ShowDefaultCursor()
                    Else
                        ShowWaitCursor()
                        returnvalue = SwitchEnable(6, True)
                        If returnvalue <> "OK" Then
                            ShowDefaultCursor()
                            Exit Sub
                        End If
                        ShowDefaultCursor()
                    End If
                End If
            Else
                If lblSwitch7Status.BackColor = ColorTranslator.FromHtml("#4cd137") Then 'green
                    ShowWaitCursor()
                    returnvalue = SwitchEnable(6, False)
                    If returnvalue <> "OK" Then
                        ShowDefaultCursor()
                        Exit Sub
                    End If
                    ShowDefaultCursor()
                Else
                    ShowWaitCursor()
                    SwitchEnable(6, True)
                    ShowDefaultCursor()
                End If
            End If
            TimerSwitch_Tick(Nothing, Nothing)
            ShowDefaultCursor()

            'set equipent status on on when manual change was made => disaster check will close roof
            pEquipmentStatus = "ON"

        Catch ex As Exception
            ShowDefaultCursor()
            LogSessionEntry("ERROR", "lblSwitch7Status_Click: " + Err.ToString(), "", "lblSwitch7Status_Click", "PROGRAM")
        End Try
    End Sub

    Private Sub LblSwitch8Status_Click(sender As Object, e As EventArgs) Handles lblSwitch8Status.Click
        Dim returnvalue As String

        Try
            If My.Settings.sSwitch8Warning <> "" Then
                If ShowMessage(My.Settings.sSwitch8Warning, "OKONLY", "Warning") = vbOK Then
                    If lblSwitch8Status.BackColor = ColorTranslator.FromHtml("#4cd137") Then 'green
                        ShowWaitCursor()
                        returnvalue = SwitchEnable(7, False)
                        If returnvalue <> "OK" Then
                            ShowDefaultCursor()
                            Exit Sub
                        End If

                        ShowDefaultCursor()
                    Else
                        ShowWaitCursor()
                        returnvalue = SwitchEnable(7, True)
                        If returnvalue <> "OK" Then
                            ShowDefaultCursor()
                            Exit Sub
                        End If

                        ShowDefaultCursor()
                    End If
                End If
            Else
                If lblSwitch8Status.BackColor = ColorTranslator.FromHtml("#4cd137") Then 'green
                    ShowWaitCursor()
                    returnvalue = SwitchEnable(7, False)
                    If returnvalue <> "OK" Then
                        ShowDefaultCursor()
                        Exit Sub
                    End If
                    ShowDefaultCursor()
                Else
                    ShowWaitCursor()
                    returnvalue = SwitchEnable(7, True)
                    If returnvalue <> "OK" Then
                        ShowDefaultCursor()
                        Exit Sub
                    End If
                    ShowDefaultCursor()
                End If
            End If
            TimerSwitch_Tick(Nothing, Nothing)
            ShowDefaultCursor()

            'set equipent status on on when manual change was made => disaster check will close roof
            pEquipmentStatus = "ON"

        Catch ex As Exception
            ShowDefaultCursor()
            LogSessionEntry("ERROR", "lblSwitch8Status_Click: " + Err.ToString(), "", "lblSwitch8Status_Click", "PROGRAM")
        End Try
    End Sub

    Private Sub BtnStopAllEquipment_Click(sender As Object, e As EventArgs) Handles btnStopAllEquipment.Click
        Dim returnvalue As String
        Try
            'wait 3 seconds between turning equipment on/off
            ShowWaitCursor()
            returnvalue = SwitchEquipmentOff()
            If returnvalue <> "OK" Then
                ShowDefaultCursor()
                Exit Sub
            End If
            ShowDefaultCursor()

        Catch ex As Exception
            ShowDefaultCursor()
            LogSessionEntry("ERROR", "btnStopAllEquipment_Click: " + Err.ToString(), "", "btnStopAllEquipment_Click", "PROGRAM")
        End Try
    End Sub

    Private Sub BtnConnectCover_Click(sender As Object, e As EventArgs) Handles BtnConnectCover.Click
        Dim returnvalue As String
        Try
            'enable Cover
            If pCoverConnected = False Then

                ShowWaitCursor()
                returnvalue = CoverConnect()
                If returnvalue <> "OK" Then
                    ShowDefaultCursor()
                    Exit Sub
                End If
                TimerCover.Enabled = True
                FrmMain.TimerCover.Enabled = True

                'set equipent status on PARTIAL when manual change was made => disaster check will close roof
                pEquipmentStatus = "PARTIAL"

                TimerCover_Tick(Nothing, Nothing)
                ShowDefaultCursor()
            End If
        Catch ex As Exception
            ShowDefaultCursor()
            LogSessionEntry("ERROR", "BtnConnectCover_Click: " + Err.ToString(), "", "BtnConnectCover_Click", "PROGRAM")
        End Try
    End Sub

    Private Sub BtnDisconnectCover_Click(sender As Object, e As EventArgs) Handles BtnDisconnectCover.Click
        Dim returnvalue As String
        Try
            'enable Cover
            TimerCover.Enabled = False
            FrmMain.TimerCover.Enabled = False
            LblCover.BackColor = Color.Transparent
            FrmMain.LblCover.BackColor = Color.Transparent

            ShowWaitCursor()

            returnvalue = CoverDisconnect()
            If returnvalue <> "OK" Then
                ShowDefaultCursor()
                Exit Sub
            End If

            'set equipent status on PARTIAL when manual change was made => disaster check will close roof
            pEquipmentStatus = "PARTIAL"

            TimerCover_Tick(Nothing, Nothing)
            ShowDefaultCursor()
        Catch ex As Exception
            ShowDefaultCursor()
            LogSessionEntry("ERROR", "BtnDisconnectCover_Click: " + Err.ToString(), "", "BtnDisconnectCover_Click", "PROGRAM")
        End Try
    End Sub

    Private Sub BtnConnectCCD_Click(sender As Object, e As EventArgs) Handles BtnConnectCCD.Click
        Dim returnvalue As String
        Try
            ShowWaitCursor()
            If pTheSkyXEquipmentConnected = False Then
                returnvalue = ConnectTheSkyXEquipment(True)
                BtnTakeImage.Enabled = True
                If returnvalue <> "OK" Then
                    ShowDefaultCursor()
                    Exit Sub
                End If

                'check CCD
                returnvalue = CheckTheSkyXCCD()
                If returnvalue <> "OK" Then
                    ShowDefaultCursor()
                    Exit Sub
                End If
                TxtFocusserAbsolutePosition.Text = Format(pCurrentFocusserPosition)
            End If

            'set equipent status on PARTIAL when manual change was made => disaster check will close roof
            pEquipmentStatus = "PARTIAL"

            TimerFocusser_Tick(Nothing, Nothing)
            ShowDefaultCursor()

        Catch ex As Exception
            ShowDefaultCursor()
            LogSessionEntry("ERROR", "BtnConnectCCD_Click: " + Err.ToString(), "", "BtnConnectCCD_Click", "PROGRAM")
        End Try
    End Sub

    Private Sub BtnDisconnectCCD_Click(sender As Object, e As EventArgs) Handles BtnDisconnectCCD.Click
        Dim returnvalue As String
        Try
            If pTheSkyXEquipmentConnected = True Then
                ShowWaitCursor()
                returnvalue = DisconnectTheSkyXEquipment()
                BtnTakeImage.Enabled = False
                If returnvalue <> "OK" Then
                    ShowDefaultCursor()
                    Exit Sub
                End If
            End If
            'set equipent status on PARTIAL when manual change was made => disaster check will close roof
            pEquipmentStatus = "PARTIAL"

            FrmMain.LblCCDExposureStatus.Text = ""
            FrmMain.LblCCDTemp.Text = ""
            FrmMain.LblFocusserPosition.Text = ""
            FrmMain.LblFocusserTemperature.Text = ""
            FrmMain.LblLastFocusTemperature.Text = ""
            FrmMain.LblLastFocusDateTime.Text = ""
            FrmMain.LblCCDFilter.Text = ""

            TimerFocusser_Tick(Nothing, Nothing)
            ShowDefaultCursor()

        Catch ex As Exception
            ShowDefaultCursor()
            LogSessionEntry("ERROR", "BtnDisconnectCCD_Click: " + Err.ToString(), "", "BtnDisconnectCCD_Click", "PROGRAM")
        End Try
    End Sub

    Private Sub TimerFocusser_Tick(sender As Object, e As EventArgs) Handles TimerCCDFocusser.Tick
        'Dim returnvalue As String
        Try
            'check focusser / CCCD
            If pTheSkyXEquipmentConnected = True Then
                'returnvalue = GetTheSkyXFocusserPosition()
                'If returnvalue <> "OK" Then
                'ShowDefaultCursor()
                ' Exit Sub
                ' End If

                LblFocusserPosition.Text = Format(pCurrentFocusserPosition)

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

                LblCCDExposureStatus.Text = ExposureStatus  'pTheSkyXExposureStatus 'Exposing Light ( x left )
                BtnConnectCCD.BackColor = ColorTranslator.FromHtml("#4cd137") 'green
                BtnDisconnectCCD.BackColor = ColorTranslator.FromHtml("#0066CC") 'blue
                BtnConnectCCD.Enabled = False
                BtnDisconnectCCD.Enabled = True
                BtnFocusserIN.Enabled = True
                BtnFocusserOUT.Enabled = True
                BtnFocusserMoveTo.Enabled = True
                BtnFocusserAutofocus.Enabled = True
            Else
                LblFocusserPosition.Text = ""
                LblCCDExposureStatus.Text = ""
                BtnConnectCCD.BackColor = ColorTranslator.FromHtml("#0066CC") 'blue
                BtnDisconnectCCD.BackColor = ColorTranslator.FromHtml("#d63031") 'red
                BtnConnectCCD.Enabled = True
                BtnDisconnectCCD.Enabled = False
                BtnFocusserIN.Enabled = False
                BtnFocusserOUT.Enabled = False
                BtnFocusserMoveTo.Enabled = False
                BtnFocusserAutofocus.Enabled = False
            End If
        Catch ex As Exception
            LogSessionEntry("ERROR", "TimerFocusser_Tick: " + Err.ToString(), "", "TimerFocusser_Tick", "PROGRAM")
        End Try
    End Sub

    Private Sub BtnFocusserIN_Click(sender As Object, e As EventArgs) Handles BtnFocusserIN.Click
        Dim returnvalue As String
        Try
            If pTheSkyXEquipmentConnected = True Then
                ShowWaitCursor()
                returnvalue = MoveTheSkyXFocusser("IN", Convert.ToInt32(TxtRelPosition.Text))
                If returnvalue <> "OK" Then
                    ShowDefaultCursor()
                    Exit Sub
                End If
            Else
                LblFocusserPosition.Text = ""
            End If
            ShowDefaultCursor()

        Catch ex As Exception
            ShowDefaultCursor()
            LogSessionEntry("ERROR", "BtnFocusserIN_Click: " + Err.ToString(), "", "BtnFocusserIN_Click", "PROGRAM")
        End Try
    End Sub

    Private Sub BtnFocusserOUT_Click(sender As Object, e As EventArgs) Handles BtnFocusserOUT.Click
        Dim returnvalue As String
        Try
            If pTheSkyXEquipmentConnected = True Then
                ShowWaitCursor()
                returnvalue = MoveTheSkyXFocusser("OUT", Convert.ToInt32(TxtRelPosition.Text))
                If returnvalue <> "OK" Then
                    ShowDefaultCursor()
                    Exit Sub
                End If
            Else
                LblFocusserPosition.Text = ""
            End If
            ShowDefaultCursor()

        Catch ex As Exception
            ShowDefaultCursor()
            LogSessionEntry("ERROR", "BtnFocusserOUT_Click: " + Err.ToString(), "", "BtnFocusserOUT_Click", "PROGRAM")
        End Try
    End Sub

    Private Sub BtnFocusserAbsolute_Click(sender As Object, e As EventArgs) Handles BtnFocusserMoveTo.Click
        Dim returnvalue As String
        Try

            If TxtFocusserAbsolutePosition.Text = "" Then
                ShowMessage("Enter a focus position value first !", "CRITICAL", "Moving to focus postion.")
            Else
                If pTheSkyXEquipmentConnected = True Then
                    ShowWaitCursor()
                    returnvalue = MoveAbsoluteTheSkyXFocusser(Convert.ToInt32(TxtFocusserAbsolutePosition.Text))
                    If returnvalue <> "OK" Then
                        ShowDefaultCursor()
                        Exit Sub
                    End If
                Else
                    LblFocusserPosition.Text = ""
                End If
                ShowDefaultCursor()
            End If

        Catch ex As Exception
            ShowDefaultCursor()
            LogSessionEntry("ERROR", "BtnFocusserAbsolute_Click: " + Err.ToString(), "", "BtnFocusserAbsolute_Click", "PROGRAM")
        End Try
    End Sub

    Private Sub BtnTakeImage_Click(sender As Object, e As EventArgs) Handles BtnTakeImage.Click
        Dim returnvalue As String
        Try
            Dim FilterNumber, Binning As Integer

            If pTheSkyXEquipmentConnected = True And pTheSkyXTakingImage = False Then
                ShowWaitCursor()

                'retrieve filternumber
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
                BtnTakeImage.Enabled = False
                BtnAbortImage.Enabled = True

                returnvalue = TakeImageTheSkyX(Convert.ToInt32(TxtCCDExpsoure.Text), Binning, "IMAGE", False, False, FilterNumber, CmbTargetFilter.Text, "exposure", True, "LIGHT", "", "")
                If returnvalue <> "OK" And returnvalue <> "IMAGE_ABORTED" Then
                    ShowDefaultCursor()
                    Exit Sub
                End If
                BtnTakeImage.Enabled = True
                BtnAbortImage.Enabled = False
            Else
                LblFocusserPosition.Text = ""
            End If
            ShowDefaultCursor()

        Catch ex As Exception
            ShowDefaultCursor()
            LogSessionEntry("ERROR", "BtnTakeImage_Click: " + Err.ToString(), "", "BtnTakeImage_Click", "PROGRAM")
        End Try
    End Sub

    Private Sub BtnAbortImage_Click(sender As Object, e As EventArgs) Handles BtnAbortImage.Click
        Dim returnvalue As String
        Try
            If pTheSkyXEquipmentConnected = True Then
                returnvalue = AbortImageTheSkyX()
                If returnvalue <> "OK" Then
                    ShowDefaultCursor()
                    Exit Sub
                End If
                BtnTakeImage.Enabled = True
                BtnAbortImage.Enabled = False
            Else
                LblFocusserPosition.Text = ""
            End If

        Catch ex As Exception
            LogSessionEntry("ERROR", "BtnAbortImage_Click: " + Err.ToString(), "", "BtnAbortImage_Click", "PROGRAM")
        End Try
    End Sub

    Private Sub TimerMount_Tick(sender As Object, e As EventArgs) Handles TimerMount.Tick
        'Dim returnvalue As String
        Try

            If pMountConnected = True Then
                BtnMountEast.Enabled = True
                BtnMountWest.Enabled = True
                BtnMountSouth.Enabled = True
                BtnMountNorth.Enabled = True

                BtnConnectMount.BackColor = ColorTranslator.FromHtml("#4cd137") 'green
                BtnDisconnectMount.BackColor = ColorTranslator.FromHtml("#0066CC") 'blue
                BtnConnectMount.Enabled = False
                BtnDisconnectMount.Enabled = True
                BtnPark.Enabled = True
                BtnUnpark.Enabled = True

                If pClosedLoopSlew = "SLEW" Then
                    BtnTargetSlew.Enabled = False
                    BtnClosedLoopSlew.Enabled = False
                    BtnTargetAbortSlew.Enabled = True
                Else
                    BtnTargetSlew.Enabled = True
                    BtnClosedLoopSlew.Enabled = True
                    BtnTargetAbortSlew.Enabled = False
                End If

                If pStructMount.AtPark = True Then
                    BtnPark.Enabled = False
                    BtnUnpark.Enabled = True
                Else
                    BtnPark.Enabled = True
                    BtnUnpark.Enabled = False
                End If
            Else
                BtnConnectMount.BackColor = ColorTranslator.FromHtml("#0066CC") 'blue
                BtnDisconnectMount.BackColor = ColorTranslator.FromHtml("#d63031") 'red
                BtnConnectMount.Enabled = True
                BtnDisconnectMount.Enabled = False
                BtnClosedLoopSlew.Enabled = False
                BtnTargetSlew.Enabled = False
                BtnTargetAbortSlew.Enabled = False
                BtnPark.Enabled = False
                BtnUnpark.Enabled = False
                BtnMountEast.Enabled = False
                BtnMountWest.Enabled = False
                BtnMountSouth.Enabled = False
                BtnMountNorth.Enabled = False
            End If

        Catch ex As Exception
            LogSessionEntry("ERROR", "TimerMount_Tick: " + ex.Message, "", "TimerMount_Tick", "PROGRAM")
        End Try
    End Sub

    Private Sub BtnConnectMount_Click(sender As Object, e As EventArgs) Handles BtnConnectMount.Click
        Dim returnvalue As String
        Try
            If pMountConnected = False Then
                ShowWaitCursor()
                returnvalue = MountConnect()
                If returnvalue <> "OK" Then
                    ShowDefaultCursor()
                    Exit Sub
                End If
                BtnMountEast.Enabled = True
                BtnMountWest.Enabled = True
                BtnMountSouth.Enabled = True
                BtnMountNorth.Enabled = True
            End If
            'set equipent status on PARTIAL when manual change was made => disaster check will close roof
            pEquipmentStatus = "PARTIAL"

            TimerMount_Tick(Nothing, Nothing)
            ShowDefaultCursor()

        Catch ex As Exception
            ShowDefaultCursor()
            LogSessionEntry("ERROR", "BtnConnectMount_Click: " + Err.ToString(), "", "BtnConnectMount_Click", "PROGRAM")
        End Try
    End Sub

    Private Sub BtnDisconnectMount_Click(sender As Object, e As EventArgs) Handles BtnDisconnectMount.Click
        Dim returnvalue As String
        Try
            If pMountConnected = True Then
                ShowWaitCursor()
                returnvalue = MountDisconnect()
                If returnvalue <> "OK" Then
                    ShowDefaultCursor()
                    Exit Sub
                End If

                'clear textfields
                FrmMain.lblMountAlt.Text = ""
                FrmMain.lblMountAz.Text = ""
                FrmMain.LblMountStatus.Text = ""
                FrmMain.LblMountStatus.BackColor = Color.Transparent
                FrmMain.LblMountRADEC.Text = ""
                FrmMain.LblMountPierSide.Text = ""

                BtnMountEast.Enabled = False
                BtnMountWest.Enabled = False
                BtnMountSouth.Enabled = False
                BtnMountNorth.Enabled = False
            End If
            'set equipent status on PARTIAL when manual change was made => disaster check will close roof
            pEquipmentStatus = "PARTIAL"

            TimerMount_Tick(Nothing, Nothing)
            ShowDefaultCursor()

        Catch ex As Exception
            ShowDefaultCursor()
            LogSessionEntry("ERROR", "DisconnectMount_Click: " + Err.ToString(), "", "BtnDisconnectMount_Click", "PROGRAM")
        End Try
    End Sub

    Private Sub BtnMountNorth_MouseDown(sender As Object, e As MouseEventArgs) Handles BtnMountNorth.MouseDown
        Dim returnvalue As String
        Try
            If pMountConnected = True And pStructMount.AtPark = False Then
                returnvalue = MountMoveAxis("N", Convert.ToInt32(TxtMountSpeed.Text))
                If returnvalue <> "OK" Then
                    ShowDefaultCursor()
                    Exit Sub
                End If
            End If

        Catch ex As Exception
            LogSessionEntry("ERROR", "BtnMountNorth_MouseDown: " + Err.ToString(), "", "BtnMountNorth_MouseDown", "PROGRAM")
        End Try
    End Sub

    Private Sub BtnMountNorth_MouseUp(sender As Object, e As MouseEventArgs) Handles BtnMountNorth.MouseUp
        Dim returnvalue As String
        Try
            If pMountConnected = True And pStructMount.AtPark = False Then
                returnvalue = MountMoveAxis("N", 0)
                If returnvalue <> "OK" Then
                    ShowDefaultCursor()
                    Exit Sub
                End If
            End If

        Catch ex As Exception
            LogSessionEntry("ERROR", "BtnMountNorth_MouseUp: " + Err.ToString(), "", "BtnMountNorth_MouseUp", "PROGRAM")
        End Try
    End Sub

    Private Sub BtnMountSouth_MouseUp(sender As Object, e As MouseEventArgs) Handles BtnMountSouth.MouseUp
        Dim returnvalue As String
        Try
            If pMountConnected = True And pStructMount.AtPark = False Then
                returnvalue = MountMoveAxis("S", 0)
                If returnvalue <> "OK" Then
                    ShowDefaultCursor()
                    Exit Sub
                End If
            End If

        Catch ex As Exception
            LogSessionEntry("ERROR", "BtnMountSouth_MouseUp: " + Err.ToString(), "", "BtnMountSouth_MouseUp", "PROGRAM")
        End Try
    End Sub

    Private Sub BtnMountSouth_MouseDown(sender As Object, e As MouseEventArgs) Handles BtnMountSouth.MouseDown
        Dim returnvalue As String
        Try
            If pMountConnected = True And pStructMount.AtPark = False Then
                returnvalue = MountMoveAxis("S", Convert.ToInt32(TxtMountSpeed.Text))
                If returnvalue <> "OK" Then
                    ShowDefaultCursor()
                    Exit Sub
                End If
            End If

        Catch ex As Exception
            LogSessionEntry("ERROR", "BtnMountSouth_MouseDown: " + Err.ToString(), "", "BtnMountSouth_MouseDown", "PROGRAM")
        End Try
    End Sub


    Private Sub BtnMountWest_MouseUp(sender As Object, e As MouseEventArgs) Handles BtnMountWest.MouseUp
        Dim returnvalue As String
        Try
            If pMountConnected = True And pStructMount.AtPark = False Then
                returnvalue = MountMoveAxis("W", 0)
                If returnvalue <> "OK" Then
                    ShowDefaultCursor()
                    Exit Sub
                End If
            End If
        Catch ex As Exception
            LogSessionEntry("ERROR", "BtnMountWest_MouseUp: " + Err.ToString(), "", "BtnMountWest_MouseUp", "PROGRAM")
        End Try
    End Sub

    Private Sub BtnMountWest_MouseDown(sender As Object, e As MouseEventArgs) Handles BtnMountWest.MouseDown
        Dim returnvalue As String
        Try
            Dim ciClone As CultureInfo = CType(CultureInfo.InvariantCulture.Clone(), CultureInfo)
            ciClone.NumberFormat.NumberDecimalSeparator = "."

            If pMountConnected = True And pStructMount.AtPark = False Then
                returnvalue = MountMoveAxis("W", Double.Parse(TxtMountSpeed.Text, ciClone))
                If returnvalue <> "OK" Then
                    ShowDefaultCursor()
                    Exit Sub
                End If
            End If

        Catch ex As Exception
            LogSessionEntry("ERROR", "BtnMountWest_MouseDown: " + Err.ToString(), "", "BtnMountWest_MouseDown", "PROGRAM")
        End Try
    End Sub


    Private Sub BtnMountEast_MouseUp(sender As Object, e As MouseEventArgs) Handles BtnMountEast.MouseUp
        Dim returnvalue As String
        Try
            If pMountConnected = True And pStructMount.AtPark = False Then
                returnvalue = MountMoveAxis("E", 0)
                If returnvalue <> "OK" Then
                    ShowDefaultCursor()
                    Exit Sub
                End If
            End If
        Catch ex As Exception
            LogSessionEntry("ERROR", "BtnMountEast_MouseUp: " + Err.ToString(), "", "BtnMountEast_MouseUp", "PROGRAM")
        End Try
    End Sub

    Private Sub BtnMountEast_MouseDown(sender As Object, e As MouseEventArgs) Handles BtnMountEast.MouseDown
        Dim returnvalue As String
        Try
            Dim ciClone As CultureInfo = CType(CultureInfo.InvariantCulture.Clone(), CultureInfo)
            ciClone.NumberFormat.NumberDecimalSeparator = "."

            If pMountConnected = True And pStructMount.AtPark = False Then
                returnvalue = MountMoveAxis("E", Double.Parse(TxtMountSpeed.Text, ciClone))
                If returnvalue <> "OK" Then
                    ShowDefaultCursor()
                    Exit Sub
                End If
            End If
        Catch ex As Exception
            LogSessionEntry("ERROR", "BtnMountEast_MouseDown: " + Err.ToString(), "", "BtnMountEast_MouseDown", "PROGRAM")
        End Try
    End Sub

    Private Sub BtnTargetSlew_Click(sender As Object, e As EventArgs) Handles BtnTargetSlew.Click
        Dim returnvalue As String
        Try
            Dim RA2000, DEC2000 As Double
            Dim RATopocentric, DECTopocentric As Double
            Dim TopcentricRAString, TopocentricDECString As String

            pToolsAbort = False

            ShowWaitCursor()

            'RA2000 and DEC2000
            RA2000 = pAUtil.HMSToHours(TxtTargetRA2000HH.Text + " " + TxtTargetRA2000MM.Text + " " + TxtTargetRA2000SS.Text)
            DEC2000 = pAUtil.DMSToDegrees(TxtTargetDEC2000DG.Text + " " + TxtTargetDEC2000MM.Text + " " + TxtTargetDEC2000SS.Text)

            'convert to topographic coordinates
            ConvertTargetJ2000ToTopocentric(RA2000, DEC2000)
            RATopocentric = pRATargetTopocentric
            DECTopocentric = pDECTargetTopocentric

            TopcentricRAString = pAUtil.HoursToHMS(RATopocentric, "h ", "m ", "s ")
            TopocentricDECString = pAUtil.DegreesToDMS(DECTopocentric, "° ", "' ", """ ")
            pAbort = False

            If pMountConnected = True And TxtTargetDEC2000DG.Text <> "" And TxtTargetRA2000HH.Text <> "" Then
                returnvalue = MountSlewToTarget(TxtTargetName.Text, RATopocentric, DECTopocentric, TopcentricRAString, TopocentricDECString, TxtTargetRA2000HH.Text + " " + TxtTargetRA2000MM.Text + " " + TxtTargetRA2000SS.Text, TxtTargetDEC2000DG.Text + " " + TxtTargetDEC2000MM.Text + " " + TxtTargetDEC2000SS.Text, True, True)
                If returnvalue <> "OK" Then
                    ShowDefaultCursor()
                    Exit Sub
                End If
            Else
                ShowMessage("First enter target coordinates!", "CRITICAL", "Target slew error...")
            End If
            ShowDefaultCursor()
        Catch ex As Exception
            ShowDefaultCursor()
            LogSessionEntry("ERROR", "BtnTargetSlew_Click: " + Err.ToString(), "", "BtnTargetSlew_Click", "PROGRAM")
        End Try
    End Sub

    Private Sub BtnTargetAbortSlew_Click(sender As Object, e As EventArgs) Handles BtnTargetAbortSlew.Click
        Dim returnvalue As String
        Try

            pToolsAbort = True

            If pMountConnected = True Then
                ShowWaitCursor()
                returnvalue = MountAbortSlew()
                If returnvalue <> "OK" Then
                    ShowDefaultCursor()
                    Exit Sub
                End If
            End If
            ShowDefaultCursor()
        Catch ex As Exception
            ShowDefaultCursor()
            LogSessionEntry("ERROR", "BtnTargetAbortSlew_Click: " + Err.ToString(), "", "BtnTargetAbortSlew_Click", "PROGRAM")
        End Try
    End Sub

    Private Sub TxtTargetRA2000HH_TextChanged(sender As Object, e As EventArgs) Handles TxtTargetRA2000HH.TextChanged
        Try
            Dim ciClone As CultureInfo = CType(CultureInfo.InvariantCulture.Clone(), CultureInfo)
            ciClone.NumberFormat.NumberDecimalSeparator = "."

            If TxtTargetRA2000HH.Text <> "" Then
                If IsNumeric(TxtTargetRA2000HH.Text) = False Then
                    ShowMessage("Not a number between 0 and 24!", "CRITICAL", "Incorrect input...")
                    Exit Sub
                End If
                If Double.Parse(TxtTargetRA2000HH.Text, ciClone) < 0 Or Double.Parse(TxtTargetRA2000HH.Text, ciClone) > 24 Then
                    ShowMessage("Not a number between 0 and 24!", "CRITICAL", "Incorrect input...")
                    Exit Sub
                End If
            End If
        Catch ex As Exception
            ShowMessage("TxtTargetRA2000HH_TextChanged: " + ex.Message, "CRITICAL", "Error!")
        End Try
    End Sub

    Private Sub TxtTargetRA2000MM_TextChanged(sender As Object, e As EventArgs) Handles TxtTargetRA2000MM.TextChanged
        Try
            Dim ciClone As CultureInfo = CType(CultureInfo.InvariantCulture.Clone(), CultureInfo)
            ciClone.NumberFormat.NumberDecimalSeparator = "."

            If TxtTargetRA2000MM.Text <> "" Then
                If IsNumeric(TxtTargetRA2000MM.Text) = False Then
                    ShowMessage("Not a number between 0 and 60!", "CRITICAL", "Incorrect input...")
                    Exit Sub
                End If
                If Double.Parse(TxtTargetRA2000MM.Text, ciClone) < 0 Or Double.Parse(TxtTargetRA2000MM.Text, ciClone) > 60 Then
                    ShowMessage("Not a number between 0 and 60!", "CRITICAL", "Incorrect input...")
                    Exit Sub
                End If
            End If
        Catch ex As Exception
            ShowMessage("TxtTargetRA2000MM_TextChanged: " + ex.Message, "CRITICAL", "Error!")
        End Try
    End Sub

    Private Sub TxtTargetRA2000SS_TextChanged(sender As Object, e As EventArgs) Handles TxtTargetRA2000SS.TextChanged
        Try
            Dim ciClone As CultureInfo = CType(CultureInfo.InvariantCulture.Clone(), CultureInfo)
            ciClone.NumberFormat.NumberDecimalSeparator = "."

            If TxtTargetRA2000SS.Text <> "" Then
                If IsNumeric(TxtTargetRA2000SS.Text) = False Then
                    ShowMessage("Not a number between 0 and 60!", "CRITICAL", "Incorrect input...")
                    Exit Sub
                End If
                If Double.Parse(TxtTargetRA2000SS.Text, ciClone) < 0 Or Double.Parse(TxtTargetRA2000SS.Text, ciClone) > 60 Then
                    ShowMessage("Not a number between 0 and 60!", "CRITICAL", "Incorrect input...")
                    Exit Sub
                End If
            End If
        Catch ex As Exception
            ShowMessage("TxtTargetRA2000SS_TextChanged: " + ex.Message, "CRITICAL", "Error!")
        End Try
    End Sub

    Private Sub TxtTargetDEC2000DG_TextChanged(sender As Object, e As EventArgs) Handles TxtTargetDEC2000DG.TextChanged
        Try
            If TxtTargetDEC2000DG.Text <> "" Then
                If IsNumeric(TxtTargetDEC2000DG.Text) = False Then
                    ShowMessage("Not a number!", "CRITICAL", "Incorrect input...")
                    Exit Sub
                End If
            End If
        Catch ex As Exception
            ShowMessage("TxtTargetDEC2000DG_TextChanged: " + ex.Message, "CRITICAL", "Error!")
        End Try
    End Sub

    Private Sub TxtTargetDEC2000MM_TextChanged(sender As Object, e As EventArgs) Handles TxtTargetDEC2000MM.TextChanged
        Try
            Dim ciClone As CultureInfo = CType(CultureInfo.InvariantCulture.Clone(), CultureInfo)
            ciClone.NumberFormat.NumberDecimalSeparator = "."

            If TxtTargetDEC2000MM.Text <> "" Then
                If IsNumeric(TxtTargetDEC2000MM.Text) = False Then
                    ShowMessage("Not a number between 0 and 60!", "CRITICAL", "Incorrect input...")
                    Exit Sub
                End If
                If Double.Parse(TxtTargetDEC2000MM.Text, ciClone) < 0 Or Double.Parse(TxtTargetDEC2000MM.Text, ciClone) > 60 Then
                    ShowMessage("Not a number between 0 and 60!", "CRITICAL", "Incorrect input...")
                    Exit Sub
                End If
            End If
        Catch ex As Exception
            ShowMessage("TxtTargetDEC2000MM_TextChanged: " + ex.Message, "CRITICAL", "Error!")
        End Try
    End Sub

    Private Sub TxtTargetDEC2000SS_TextChanged(sender As Object, e As EventArgs) Handles TxtTargetDEC2000SS.TextChanged
        Try
            Dim ciClone As CultureInfo = CType(CultureInfo.InvariantCulture.Clone(), CultureInfo)
            ciClone.NumberFormat.NumberDecimalSeparator = "."

            If TxtTargetDEC2000SS.Text <> "" Then
                If IsNumeric(TxtTargetDEC2000SS.Text) = False Then
                    ShowMessage("Not a number between 0 and 60!", "CRITICAL", "Incorrect input...")
                    Exit Sub
                End If
                If Double.Parse(TxtTargetDEC2000SS.Text, ciClone) < 0 Or Double.Parse(TxtTargetDEC2000SS.Text, ciClone) > 60 Then
                    ShowMessage("Not a number between 0 and 60!", "CRITICAL", "Incorrect input...")
                    Exit Sub
                End If
            End If
        Catch ex As Exception
            ShowMessage("TxtTargetDEC2000SS_TextChanged: " + ex.Message, "CRITICAL", "Error!")
        End Try
    End Sub

    Private Sub BtnTargetFindTSX_Click(sender As Object, e As EventArgs) Handles BtnTargetFindTSX.Click
        Dim returnvalue As String

        Try
            If TxtTargetName.Text <> "" Then
                ShowWaitCursor()
                returnvalue = FindTheSkyXTarget(TxtTargetName.Text)

                Select Case returnvalue
                    Case "NOTFOUND"
                        ShowMessage("Target not found !", "CRITICAL", "The Sky X Application...")
                        ShowDefaultCursor()
                        Exit Sub
                    Case "OK"
                        'do nothing
                    Case Else
                        Exit Sub
                End Select

                'convert values to HH MM DD and DD MM SS
                Dim RA_string, DEC_string As String
                Dim index, old_index As Integer
                'Dim Alt_string, Az_string As String
                'Dim RAHH, RAMM, RASS
                RA_string = pAUtil.HoursToHMS(pStrucTargetObject.RightAscension)
                DEC_string = pAUtil.DegreesToDMS(pStrucTargetObject.Declination)
                '01:51:28
                '-10° 20' 06"
                '12345678901

                index = GetNthIndex(RA_string, ":"c, 1)
                TxtTargetRA2000HH.Text = RA_string.Substring(0, index)
                old_index = index
                index = GetNthIndex(RA_string, ":"c, 2)
                TxtTargetRA2000MM.Text = RA_string.Substring(old_index + 1, index - old_index - 1)
                TxtTargetRA2000SS.Text = RA_string.Substring(index + 1, 2)

                index = GetNthIndex(DEC_string, "°"c, 1)
                TxtTargetDEC2000DG.Text = DEC_string.Substring(0, index)
                TxtTargetDEC2000MM.Text = DEC_string.Substring(index + 2, 2)
                index = GetNthIndex(DEC_string, "'"c, 1)
                TxtTargetDEC2000SS.Text = DEC_string.Substring(index + 2, 2)
            End If
            ShowDefaultCursor()
        Catch ex As Exception
            ShowDefaultCursor()
            ShowMessage("TxtTargetDEC2000SS_TextChanged: " + ex.Message, "CRITICAL", "Error!")
        End Try
    End Sub

    Private Sub BtnClosedLoopSlew_Click(sender As Object, e As EventArgs) Handles BtnClosedLoopSlew.Click
        Dim returnvalue As String
        Try
            Dim RA2000, DEC2000 As Double
            Dim RATopocentric, DECTopocentric As Double
            Dim TopocentricRAString, TopocentricDECString As String
            Dim TopocentricRAStringTSX, TopocentricDECStringTSX As String
            ShowWaitCursor()

            pToolsAbort = False

            RA2000 = pAUtil.HMSToHours(TxtTargetRA2000HH.Text + " " + TxtTargetRA2000MM.Text + " " + TxtTargetRA2000SS.Text)
            DEC2000 = pAUtil.DMSToDegrees(TxtTargetDEC2000DG.Text + " " + TxtTargetDEC2000MM.Text + " " + TxtTargetDEC2000SS.Text)

            'convert to topographic coordinates
            ConvertTargetJ2000ToTopocentric(RA2000, DEC2000)
            RATopocentric = pRATargetTopocentric
            DECTopocentric = pDECTargetTopocentric

            TopocentricRAString = pAUtil.HoursToHMS(RATopocentric, "h ", "m ", "s ")
            TopocentricDECString = pAUtil.DegreesToDMS(DECTopocentric, "° ", "' ", """ ")
            TopocentricRAStringTSX = pAUtil.HoursToHMS(RATopocentric, " ", " ", " ")
            TopocentricDECStringTSX = pAUtil.DegreesToDMS(DECTopocentric, " ", " ", " ")

            'if target is below horizon: abort !
            returnvalue = CalculateObject(RA2000, DEC2000)
            If returnvalue <> "OK" Then
                ShowDefaultCursor()
                Exit Sub
            End If

            If pStructObject.ObjectAlt > 10 Then
                If pMountConnected = True And TxtTargetDEC2000DG.Text <> "" And TxtTargetRA2000HH.Text <> "" Then

                    'turn off buttons
                    BtnTakeImage.Enabled = False
                    BtnTargetSlew.Enabled = False

                    Dim FilterNumber As Integer
                    'retrieve filternumber
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


                    returnvalue = ClosedLoopSlew(RATopocentric, DECTopocentric, TopocentricRAString, TopocentricDECString, TxtTargetRA2000HH.Text + "h " + TxtTargetRA2000MM.Text + "m " + TxtTargetRA2000SS.Text + "s ", TxtTargetDEC2000DG.Text + "° " + TxtTargetDEC2000MM.Text + "' " + TxtTargetDEC2000SS.Text + """", TxtTargetName.Text, FilterNumber.ToString, True, TopocentricRAStringTSX, TopocentricDECStringTSX)
                    If returnvalue <> "OK" Then
                        ShowDefaultCursor()
                        Exit Sub
                    End If

                    'turn off buttons
                    BtnTakeImage.Enabled = False
                    BtnTargetSlew.Enabled = False

                Else
                    ShowMessage("First enter target coordinates!", "CRITICAL", "Target slew error...")
                End If
            Else
                LogSessionEntry("FULL", "Slew could not be completed! Object at : " + Format(pStructObject.ObjectAlt, "0.00") + "°", "", "BtnClosedLoopSlew_Click", "TSX")
            End If
            ShowDefaultCursor()
        Catch ex As Exception
            ShowDefaultCursor()
            LogSessionEntry("ERROR", "BtnClosedLoopSlew_Click: " + Err.ToString(), "", "BtnClosedLoopSlew_Click", "PROGRAM")
        End Try
    End Sub

    Private Sub BtnPark_Click(sender As Object, e As EventArgs) Handles BtnPark.Click
        Dim returnvalue As String
        Try
            If pMountConnected = True Then
                ShowWaitCursor()
                returnvalue = MountPark()
                If returnvalue <> "OK" Then
                    ShowDefaultCursor()
                    Exit Sub
                End If
            End If
            TimerMount_Tick(Nothing, Nothing)
            ShowDefaultCursor()
        Catch ex As Exception
            ShowDefaultCursor()
            LogSessionEntry("ERROR", "BtnPark_Click: " + Err.ToString(), "", "BtnPark_Click", "PROGRAM")
        End Try
    End Sub

    Private Sub BtnUnpark_Click(sender As Object, e As EventArgs) Handles BtnUnpark.Click
        Dim returnvalue As String
        Try
            If pMountConnected = True Then
                ShowWaitCursor()
                returnvalue = MountUnpark()
                If returnvalue <> "OK" Then
                    ShowDefaultCursor()
                    Exit Sub
                End If

                returnvalue = MountEnableTracking()
                If returnvalue <> "OK" Then
                    ShowDefaultCursor()
                    Exit Sub
                End If
            End If

            TimerMount_Tick(Nothing, Nothing)
            ShowDefaultCursor()
        Catch ex As Exception
            ShowDefaultCursor()
            LogSessionEntry("ERROR", "BtnPark_Click: " + Err.ToString(), "", "BtnPark_Click", "PROGRAM")
        End Try
    End Sub

    Private Sub BtnZipCopyImageSets_Click(sender As Object, e As EventArgs) Handles BtnZipCopyImageSets.Click
        Dim returnvalue As String
        Try
            ShowWaitCursor()
            returnvalue = Zip7AndCopyFolders(True)
            If returnvalue <> "OK" Then
                ShowDefaultCursor()
                Exit Sub
            End If
            ShowDefaultCursor()
        Catch ex As Exception
            ShowDefaultCursor()
            LogSessionEntry("ERROR", "BtnZipCopyImageSets_Click: " + Err.ToString(), "", "BtnZipCopyImageSets_Click", "PROGRAM")
        End Try
    End Sub

    Private Sub BtnStartEquipment_Click(sender As Object, e As EventArgs) Handles BtnStartEquipment.Click
        Dim returnvalue As String
        Try
            ShowWaitCursor()
            LogSessionEntry("ESSENTIAL", "STARTUP: turning on power... Sun Alt " + Format(pStructEventTimes.SunAlt, "#0.00") + "°", "", "CheckRun", "SEQUENCE")
            returnvalue = TurnOnEquipment(ChkOpenRoof.Checked)
            BtnTakeImage.Enabled = True
            If returnvalue <> "OK" Then
                ShowDefaultCursor()
                Exit Sub
            End If
            ShowDefaultCursor()
        Catch ex As Exception
            ShowDefaultCursor()
            LogSessionEntry("ERROR", "BtnStartEquipment_Click: " + Err.ToString(), "", "BtnStartEquipment_Click", "PROGRAM")
        End Try
    End Sub

    Private Sub BtnShutdownEquipment_Click(sender As Object, e As EventArgs) Handles BtnShutdownEquipment.Click
        Dim returnvalue As String
        Try
            If pEquipmentStatus = "ON" Or pEquipmentStatus = "PARTIAL" Or pEquipmentStatus = "PAUSED" Then
                ShowWaitCursor()
                returnvalue = TurnOffEquipment()
                BtnTakeImage.Enabled = False
                If returnvalue <> "OK" Then
                    ShowDefaultCursor()
                    Exit Sub
                End If
                ShowDefaultCursor()
            Else
                'first everything must be on !
                ShowMessage("First turn equipment on!", "CRITICAL", "Shutdown equipment...")
            End If

        Catch ex As Exception
            ShowDefaultCursor()
            LogSessionEntry("ERROR", "BtnShutdownEquipment_Click: " + Err.ToString(), "", "BtnShutdownEquipment_Click", "PROGRAM")
        End Try
    End Sub

    Private Sub ShowWaitCursor()
        Me.Cursor = Cursors.WaitCursor
        FrmMain.Cursor = Cursors.WaitCursor
        'FrmMain.BtnStart.Enabled = False
        'FrmMain.BtnStop.Enabled = True
        'FrmMain.ChkAutoStart.Enabled = False
    End Sub

    Private Sub ShowDefaultCursor()
        Me.Cursor = Cursors.Default
        FrmMain.Cursor = Cursors.Default
        'FrmMain.BtnStart.Enabled = True
        'FrmMain.BtnStop.Enabled = False gives problems with smart error
        'FrmMain.ChkAutoStart.Enabled = True
    End Sub

    Private Sub FrmTools_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        My.Settings.sToolsOpenRoof = ChkOpenRoof.Checked
    End Sub

    Private Sub BtnPauzeEquipment_Click(sender As Object, e As EventArgs) Handles BtnPauzeEquipment.Click
        Dim returnvalue As String
        Try
            ShowWaitCursor()
            returnvalue = PauseEquipment()
            BtnTakeImage.Enabled = False
            If returnvalue <> "OK" Then
                ShowDefaultCursor()
                Exit Sub
            End If
            ShowDefaultCursor()

        Catch ex As Exception
            ShowDefaultCursor()
            LogSessionEntry("ERROR", "BtnPauseEquipment_Click: " + Err.ToString(), "", "BtnPauseEquipment_Click", "PROGRAM")
        End Try
    End Sub

    Private Sub BtnFocusserAutofocus_Click(sender As Object, e As EventArgs) Handles BtnFocusserAutofocus.Click
        Dim returnvalue As String
        Try
            ShowWaitCursor()
            FrmMain.LblLastFocusDateTime.Text = "Last focus: " + Format(DateTime.UtcNow)
            FrmMain.LblLastFocusTemperature.Text = "Last temperature:" + Format(pCurrentFocusserTemperature, "0.00") + "°"

            'focus goto focusstar or object and focus
            If My.Settings.sSimulatorMode = True Then
                LogSessionEntry("ESSENTIAL", "Now focussing in debug mode... focussed.", "", "BtnFocusserAutofocus_Click", "SEQUENCE")
            Else
                returnvalue = FocusDeepsky(False)
                If returnvalue <> "OK" And returnvalue <> "FOCUS_ABORTED" Then
                    LogSessionEntry("ERROR", "Focussing failed!", "", "BtnFocusserAutofocus_Click", "SEQUENCE")
                Else
                    LogSessionEntry("ESSENTIAL", "Focussed.", "", "BtnFocusserAutofocus_Click", "SEQUENCE")
                End If
            End If
            ShowDefaultCursor()
        Catch ex As Exception
            ShowDefaultCursor()
            LogSessionEntry("ERROR", "BtnPauseEquipment_Click: " + Err.ToString(), "", "BtnPauseEquipment_Click", "PROGRAM")
        End Try
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub Panel8_Paint(sender As Object, e As PaintEventArgs) Handles Panel8.Paint

    End Sub
End Class