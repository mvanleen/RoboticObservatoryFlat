Imports SnmpSharpNet

Module ModUPS
    Public pUPSStatusNbr As String
    Public pUPSStatusTxt As String
    Public pLastKnownUPSConnected As DateTime


    Public Function GetUPSData() As String
        Dim startExecution As Date
        Dim executionTime As TimeSpan

        GetUPSData = "OK"
        Try
            startExecution = DateTime.UtcNow()

            If My.Settings.sSimulatorMode = True Then
                If My.Settings.sDebugKillUPS = False Then
                    pUPSStatusNbr = "2"
                    pUPSStatusTxt = "OK"
                    pLastKnownUPSConnected = DateTime.UtcNow
                Else
                    pUPSStatusNbr = "3"
                    pUPSStatusTxt = "ONBATTERY"
                    LogSessionEntry("FULL", "UPS: DEBUG NO POWER", "", "CheckTimeoutUPS", "UPS")
                End If

            Else
                Dim host As String = My.Settings.sUPSHost
                Dim community As String = My.Settings.sUPSCommunity
                Dim requestOid() As String
                Dim result As Dictionary(Of Oid, AsnType)
                requestOid = New String() {My.Settings.sUPSrequestOid}

                Dim snmp As New SimpleSnmp(host, community)

                If Not snmp.Valid Then
                    GetUPSData = "Invalid hostname/community."
                    Exit Function
                End If
                snmp.Timeout = 200

                result = snmp.Get(SnmpVersion.Ver1, requestOid)

                If result IsNot Nothing Then
                    Dim kvp As KeyValuePair(Of Oid, AsnType)
                    For Each kvp In result
                        Dim str As String
                        str = "{0}: ({1}) {2}" + " " + kvp.Key.ToString() + " " + SnmpConstants.GetTypeName(kvp.Value.Type) + " " + kvp.Value.ToString()
                        pUPSStatusNbr = kvp.Value.ToString()
                        Select Case pUPSStatusNbr
                            Case "1"
                                pUPSStatusTxt = "UNKNOWN"
                            Case "2"
                                pUPSStatusTxt = "OK"
                                pLastKnownUPSConnected = DateTime.UtcNow
                            Case "3"
                                pUPSStatusTxt = "ONBATTERY"
                            Case "4"
                                pUPSStatusTxt = "ONBOOST"
                            Case "5"
                                pUPSStatusTxt = "SLEEP"
                            Case "6"
                                pUPSStatusTxt = "OFF"
                            Case "7"
                                pUPSStatusTxt = "REBOOTING"
                        End Select

                    Next
                Else
                    pUPSStatusNbr = "1"
                    pUPSStatusTxt = "Unknown"
                End If
            End If

            executionTime = DateTime.UtcNow() - startExecution
            LogSessionEntry("DEBUG", "  GetUPSData: " + executionTime.ToString, "", "GetUPSData", "UPS")

        Catch ex As Exception
            GetUPSData = "GetUPSData: " + ex.Message
            LogSessionEntry("BRIEF", "GetUPSData: " + ex.Message, "", "GetUPSData", "UPS")
        End Try

    End Function

    Public Function CheckTimeoutUPS() As String
        'wrapper that takes into account a delay before the alarm is raised
        Dim returnvalue As String
        Dim i As Long
        Dim startExecution As Date
        Dim executionTime As TimeSpan

        CheckTimeoutUPS = "OK"
        Try
            startExecution = DateTime.UtcNow()

            If My.Settings.sUPSNotPresent = True Then
                FrmMain.LblUPSCyberPower.BackColor = Color.Transparent
                FrmMain.LblUPSCyberPower.Text = "NO UPS"
            Else
                returnvalue = GetUPSData()
                If returnvalue <> "OK" Then
                    CheckTimeoutUPS = "NOK"
                    'DO NOT EXIT FUNCTION BUT CONTINUE TO SET STATUS CORRECTLY
                    LogSessionEntry("FULL", "UPS: " + returnvalue, "", "CheckTimeoutUPS", "UPS")
                End If
                i = DateDiff(DateInterval.Second, pLastKnownUPSConnected, DateTime.UtcNow)

                'UPS is not connected
                If i >= My.Settings.sUPSTimeout Then 'Or pUPSStatusNbr <> 2
                    FrmMain.LblUPSCyberPower.BackColor = ColorTranslator.FromHtml("#d63031") 'red
                    FrmMain.LblUPSCyberPower.Text = "NO POWER"
                    CheckTimeoutUPS = "NOK"
                    LogSessionEntry("ERROR", "UPS: power down !", "", "CheckTimeoutUPS", "UPS")
                Else
                    FrmMain.LblUPSCyberPower.BackColor = ColorTranslator.FromHtml("#4cd137") 'green
                    FrmMain.LblUPSCyberPower.Text = "UPS"
                    LogSessionEntry("DEBUG", "  UPS OK!", "", "CheckTimeoutUPS", "UPS")
                End If
            End If

            executionTime = DateTime.UtcNow() - startExecution
            LogSessionEntry("DEBUG", "  CheckTimeoutUPS: " + executionTime.ToString, "", "CheckTimeoutUPS", "UPS")

        Catch ex As Exception
            CheckTimeoutUPS = "CheckTimeoutUPS: " + ex.Message
            LogSessionEntry("ERROR", "CheckTimeoutUPS: " + ex.Message, "", "CheckTimeoutUPS", "UPS")
        End Try
    End Function


End Module
