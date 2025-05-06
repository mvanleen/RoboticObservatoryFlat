Imports System.ComponentModel
Imports System.Data.SqlClient
Imports System.Data.SQLite
Imports System.Data.SqlTypes
Imports System.Globalization

Public Class FrmHADS
    Public dtSL_master As New DataTable
    Public daSL_master As New OleDb.OleDbDataAdapter
    Public dtSL_detail As New DataTable
    Public daSL_detail As New OleDb.OleDbDataAdapter

    Private Sub FrmVariables_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim returnvalue As String

        Try
            returnvalue = LoadDataGrid()
            If returnvalue <> "OK" Then
                Exit Sub
            End If

            'add combobox options
            ComboBoxHADSFilter.Items.Add(My.Settings.sCCDFilter1)
            ComboBoxHADSFilter.Items.Add(My.Settings.sCCDFilter2)
            ComboBoxHADSFilter.Items.Add(My.Settings.sCCDFilter3)
            ComboBoxHADSFilter.Items.Add(My.Settings.sCCDFilter4)
            ComboBoxHADSFilter.Items.Add(My.Settings.sCCDFilter5)

        Catch ex As Exception
            ShowMessage("FrmVariables_Load: " + ex.Message, "CRITICAL", "Error!")
        End Try
    End Sub


    Public Function LoadDataGrid() As String
        Dim returnvalue As String

        LoadDataGrid = "OK"
        Try
            Me.Cursor = Cursors.WaitCursor
            dtSL_master.Clear()

            Using pCon As New SQLiteConnection("Data Source=RoboticObservatory.db;Version=3;")
                Using cmd As SQLiteCommand = pCon.CreateCommand
                    Using daSL_master As New SQLiteDataAdapter
                        Try
                            pCon.Open()
                            daSL_master.SelectCommand = cmd
                            cmd.CommandText = "select HADSName as Name, HADSMin as Minimum, HADSMax as Maximum, HADSPeriod as Period, " +
                                              " HADSExposure as Exposure, HADSFilter as Filter, HADSPriority as Priority, " +
                                              " HADSLastObserved as LastObserved, HADSError as Error, FocusStarName, HADSActive as Active" +
                                              " from HADS order by 9 desc, 1 asc"
                            daSL_master.SelectCommand = cmd

                            'Add the columns
                            If dtSL_master.Columns.Count = 11 Then
                                'do nothing
                            Else
                                Using colName As New DataColumn("Name")
                                    colName.DataType = System.Type.GetType("System.String")
                                    dtSL_master.Columns.Add(colName)
                                End Using

                                Using colMinimum As New DataColumn("Minimum")
                                    colMinimum.DataType = System.Type.GetType("System.Double")
                                    dtSL_master.Columns.Add(colMinimum)
                                End Using

                                Using colMaximum As New DataColumn("Maximum")
                                    colMaximum.DataType = System.Type.GetType("System.Double")
                                    dtSL_master.Columns.Add(colMaximum)
                                End Using

                                Using colPeriod As New DataColumn("Period")
                                    colPeriod.DataType = System.Type.GetType("System.Double")
                                    dtSL_master.Columns.Add(colPeriod)
                                End Using

                                Using colExposure As New DataColumn("Exposure")
                                    colExposure.DataType = System.Type.GetType("System.Double")
                                    dtSL_master.Columns.Add(colExposure)
                                End Using

                                Using colFilter As New DataColumn("Filter")
                                    colFilter.DataType = System.Type.GetType("System.String")
                                    dtSL_master.Columns.Add(colFilter)
                                End Using

                                Using colPriority As New DataColumn("Priority")
                                    colPriority.DataType = System.Type.GetType("System.Boolean")
                                    dtSL_master.Columns.Add(colPriority)
                                End Using

                                Using colLastObserved As New DataColumn("LastObserved")
                                    colLastObserved.DataType = System.Type.GetType("System.String")
                                    dtSL_master.Columns.Add(colLastObserved)
                                End Using

                                Using colError As New DataColumn("Error")
                                    colError.DataType = System.Type.GetType("System.Boolean")
                                    dtSL_master.Columns.Add(colError)
                                End Using

                                Using colFocusStarName As New DataColumn("FocusStarName")
                                    colFocusStarName.DataType = System.Type.GetType("System.String")
                                    dtSL_master.Columns.Add(colFocusStarName)
                                End Using

                                Using colActive As New DataColumn("Active")
                                    colActive.DataType = System.Type.GetType("System.Boolean")
                                    dtSL_master.Columns.Add(colActive)
                                End Using

                            End If

                            daSL_master.Fill(dtSL_master)

                            DataGridViewHADS.DataSource = dtSL_master

                            DataGridViewHADS.Columns(0).Width = 100
                            DataGridViewHADS.Columns(1).Width = 60
                            DataGridViewHADS.Columns(2).Width = 60
                            DataGridViewHADS.Columns(3).Width = 50
                            DataGridViewHADS.Columns(4).Width = 60
                            DataGridViewHADS.Columns(5).Width = 30
                            DataGridViewHADS.Columns(6).Width = 30
                            DataGridViewHADS.Columns(7).Width = 100
                            DataGridViewHADS.Columns(8).Width = 50
                            DataGridViewHADS.Columns(9).Width = 100
                            'DataGridViewHADS.Width = 725


                            If dtSL_master.Rows.Count > 0 Then
                                'load first record
                                returnvalue = LoadRecord(dtSL_master.Rows(0).Item(0).ToString, dtSL_master.Rows(0).Item(5).ToString)
                                If returnvalue <> "OK" Then
                                    LoadDataGrid = "OK"
                                    '    Exit Function
                                End If
                            Else
                                'no records, clear text fields
                                TxtID.Text = ""
                                TxtHADSName.Text = ""
                                TxtHADSName2.Text = ""
                                TxtHADSName3.Text = ""
                                TxtHADSName4.Text = ""
                                TxtHADSName5.Text = ""
                                ChkHADSActive.Checked = False

                                TxtHADSMin.Text = ""
                                TxtHADSMax.Text = ""
                                TxtHADSPeriod.Text = ""
                                TxtHADSLastObserved.Text = ""

                                TxtHADSRA2000HH.Text = ""
                                TxtHADSRA2000MM.Text = ""
                                TxtHADSRA2000SS.Text = ""
                                TxtHADSDEC2000DG.Text = ""
                                TxtHADSDEC2000MM.Text = ""
                                TxtHADSDEC2000SS.Text = ""
                                TxtHADSExposure.Text = ""
                                ComboBoxHADSBinning.Text = ""
                                ComboBoxHADSFilter.Text = ""


                                TxtHADSJan.Text = ""
                                TxtHADSFeb.Text = ""
                                TxtHADSMar.Text = ""
                                TxtHADSApr.Text = ""
                                TxtHADSMay.Text = ""
                                TxtHADSJun.Text = ""
                                TxtHADSJul.Text = ""
                                TxtHADSAug.Text = ""
                                TxtHADSSep.Text = ""
                                TxtHADSOct.Text = ""
                                TxtHADSNov.Text = ""
                                TxtHADSDec.Text = ""

                                ChkHADSPriority.Checked = False
                                TxtFocusStarName.Text = ""
                                TxtFocusStarRA2000HH.Text = ""
                                TxtFocusStarRA2000MM.Text = ""
                                TxtFocusStarRA2000SS.Text = ""
                                TxtFocusStarDEC2000DG.Text = ""
                                TxtFocusStarDEC2000MM.Text = ""
                                TxtFocusStarDEC2000SS.Text = ""
                                TxtFocusStarExposure.Text = ""
                                ChkHADSError.Checked = False
                                TxtHADSErrorText.Text = ""
                            End If
                            pCon.Close()

                        Catch ex As Exception
                            LoadDataGrid = "LoadDataGrid:     " + ex.Message
                            LogSessionEntry("ERROR", "LoadDataGrid " + ex.Message, "", "LoadDataGrid", "PROGRAM")
                        End Try
                    End Using
                End Using
            End Using

            DataGridViewHADS.DataSource = dtSL_master
            Me.Cursor = Cursors.Default
        Catch ex As Exception
            ShowMessage("LoadDataGrid " + ex.Message, "CRITICAL", "Error!")
            Me.Cursor = Cursors.Default
            LoadDataGrid = "NOK"
        End Try
    End Function

    Public Function LoadRecord(vHADSName As String, vHADSFilter As String) As String
        Dim returnvalue As String
        Dim ciClone As CultureInfo = CType(CultureInfo.InvariantCulture.Clone(), CultureInfo)
        ciClone.NumberFormat.NumberDecimalSeparator = "."

        LoadRecord = "OK"
        Try
            Me.Cursor = Cursors.WaitCursor
            returnvalue = ClearDetailRecord()
            If returnvalue <> "OK" Then
                LoadRecord = "NOK"
                Exit Function
            End If


            Using pCon As New SQLiteConnection("Data Source=RoboticObservatory.db;Version=3;")
                Using cmd As SQLiteCommand = pCon.CreateCommand
                    Using daSL_detail As New SQLiteDataAdapter
                        Try
                            pCon.Open()
                            daSL_detail.SelectCommand = cmd
                            'cmd.CommandText = "Select * from HADS where HADSName=""" + vHADSName + """ And HADSFilter =""" + vHADSFilter + """"

                            cmd.CommandText = "Select ID, HADSName, HADSName2, HADSName3, HADSName4, HADSName5, " +
                                               "HADSActive, HADSMin, HADSMax, HADSPeriod, HADSLastObserved," +
                                               "HADSRA2000HH, HADSRA2000MM, HADSRA2000SS, HADSDEC2000DG, HADSDEC2000MM, HADSDEC2000SS," +
                                               "HADSExposure, HADSBin, HADSFilter," +
                                               "HADSJan, HADSFeb, HADSMar, HADSApr, HADSMay, HADSJun, HADSJul, HADSAug, HADSSep, HADSOct," +
                                               "HADSNov, HADSDec, HADSPriority," +
                                               "FocusStarName, FocusStarRA2000HH, FocusStarRA2000MM, FocusStarRA2000SS, FocusStarDEC2000DG," +
                                               "FocusStarDEC2000MM, FocusStarDEC2000SS, FocusStarExposure, HADSError, HADSErrorText " +
                                               "from HADS where HADSName=""" + vHADSName + """ And HADSFilter =""" + vHADSFilter + """"

                            daSL_detail.SelectCommand = cmd
                            dtSL_detail.Clear()

                            daSL_detail.Fill(dtSL_detail)

                            If dtSL_detail.Rows.Count > 1 Then
                                'error
                            Else
                                Dim dv As New DataView(dtSL_detail)

                                TxtID.DataBindings.Add("text", dv, "ID")
                                TxtHADSName.DataBindings.Add("text", dv, "HADSName")
                                TxtHADSName2.DataBindings.Add("text", dv, "HADSName2")
                                TxtHADSName3.DataBindings.Add("text", dv, "HADSName3")
                                TxtHADSName4.DataBindings.Add("text", dv, "HADSName4")
                                TxtHADSName5.DataBindings.Add("text", dv, "HADSName5")
                                ChkHADSActive.DataBindings.Add("checked", dv, "HADSActive")

                                TxtHADSMin.DataBindings.Add("text", dv, "HADSMin")
                                TxtHADSMax.DataBindings.Add("text", dv, "HADSMax")
                                TxtHADSPeriod.DataBindings.Add("text", dv, "HADSPeriod")
                                TxtHADSLastObserved.DataBindings.Add("text", dv, "HADSLastObserved")

                                TxtHADSRA2000HH.DataBindings.Add("text", dv, "HADSRA2000HH")
                                TxtHADSRA2000MM.DataBindings.Add("text", dv, "HADSRA2000MM")
                                TxtHADSRA2000SS.DataBindings.Add("text", dv, "HADSRA2000SS")
                                TxtHADSDEC2000DG.DataBindings.Add("text", dv, "HADSDEC2000DG")
                                TxtHADSDEC2000MM.DataBindings.Add("text", dv, "HADSDEC2000MM")
                                TxtHADSDEC2000SS.DataBindings.Add("text", dv, "HADSDEC2000SS")
                                TxtHADSExposure.DataBindings.Add("text", dv, "HADSExposure")
                                ComboBoxHADSBinning.DataBindings.Add("text", dv, "HADSBin")
                                ComboBoxHADSFilter.DataBindings.Add("text", dv, "HADSFilter")


                                TxtHADSJan.DataBindings.Add("text", dv, "HADSJan")
                                TxtHADSFeb.DataBindings.Add("text", dv, "HADSFeb")
                                TxtHADSMar.DataBindings.Add("text", dv, "HADSMar")
                                TxtHADSApr.DataBindings.Add("text", dv, "HADSApr")
                                TxtHADSMay.DataBindings.Add("text", dv, "HADSMay")
                                TxtHADSJun.DataBindings.Add("text", dv, "HADSJun")
                                TxtHADSJul.DataBindings.Add("text", dv, "HADSJul")
                                TxtHADSAug.DataBindings.Add("text", dv, "HADSAug")
                                TxtHADSSep.DataBindings.Add("text", dv, "HADSSep")
                                TxtHADSOct.DataBindings.Add("text", dv, "HADSOct")
                                TxtHADSNov.DataBindings.Add("text", dv, "HADSNov")
                                TxtHADSDec.DataBindings.Add("text", dv, "HADSDec")

                                ChkHADSPriority.DataBindings.Add("checked", dv, "HADSPriority")
                                TxtFocusStarName.DataBindings.Add("text", dv, "FocusStarName")
                                TxtFocusStarRA2000HH.DataBindings.Add("text", dv, "FocusStarRA2000HH")
                                TxtFocusStarRA2000MM.DataBindings.Add("text", dv, "FocusStarRA2000MM")
                                TxtFocusStarRA2000SS.DataBindings.Add("text", dv, "FocusStarRA2000SS")
                                TxtFocusStarDEC2000DG.DataBindings.Add("text", dv, "FocusStarDEC2000DG")
                                TxtFocusStarDEC2000MM.DataBindings.Add("text", dv, "FocusStarDEC2000MM")
                                TxtFocusStarDEC2000SS.DataBindings.Add("text", dv, "FocusStarDEC2000SS")
                                TxtFocusStarExposure.DataBindings.Add("text", dv, "FocusStarExposure")
                                ChkHADSError.DataBindings.Add("checked", dv, "HADSError")
                                TxtHADSErrorText.DataBindings.Add("text", dv, "HADSErrorText")

                                'set the current record
                                With pStrucHADSRecord
                                    .ID = Integer.Parse(TxtID.Text)
                                    .HADSName = TxtHADSName.Text
                                    .HADSName2 = TxtHADSName2.Text
                                    .HADSName3 = TxtHADSName3.Text
                                    .HADSName4 = TxtHADSName4.Text
                                    .HADSName5 = TxtHADSName5.Text
                                    .HADSActive = Convert.ToInt16(ChkHADSActive.Checked)

                                    .HADSMin = TxtHADSMin.Text
                                    .HADSMax = TxtHADSMax.Text
                                    .HADSPeriod = TxtHADSPeriod.Text
                                    .HADSLastObserved = TxtHADSLastObserved.Text

                                    .HADSRA2000HH = TxtHADSRA2000HH.Text
                                    .HADSRA2000MM = TxtHADSRA2000MM.Text
                                    .HADSRA2000SS = TxtHADSRA2000SS.Text
                                    .HADSDEC2000DG = TxtHADSDEC2000DG.Text
                                    .HADSDEC2000MM = TxtHADSDEC2000MM.Text
                                    .HADSDEC2000SS = TxtHADSDEC2000SS.Text

                                    .HADSExposure = Double.Parse(TxtHADSExposure.Text, ciClone)
                                    .HADSBin = ComboBoxHADSBinning.Text
                                    .HADSFilter = ComboBoxHADSFilter.Text

                                    .HADSJan = TxtHADSJan.Text
                                    .HADSFeb = TxtHADSFeb.Text
                                    .HADSMar = TxtHADSMar.Text
                                    .HADSApr = TxtHADSApr.Text
                                    .HADSMay = TxtHADSMay.Text
                                    .HADSJun = TxtHADSJun.Text
                                    .HADSJul = TxtHADSJul.Text
                                    .HADSAug = TxtHADSAug.Text
                                    .HADSSep = TxtHADSSep.Text
                                    .HADSOct = TxtHADSOct.Text
                                    .HADSNov = TxtHADSNov.Text
                                    .HADSDec = TxtHADSDec.Text

                                    .HADSPriority = Convert.ToInt16(ChkHADSPriority.Checked)
                                    .FocusStarName = TxtFocusStarName.Text
                                    .FocusStarRA2000HH = TxtFocusStarRA2000HH.Text
                                    .FocusStarRA2000MM = TxtFocusStarRA2000MM.Text
                                    .FocusStarRA2000SS = TxtFocusStarRA2000SS.Text
                                    .FocusStarDEC2000DG = TxtFocusStarDEC2000DG.Text
                                    .FocusStarDEC2000MM = TxtFocusStarDEC2000MM.Text
                                    .FocusStarDEC2000SS = TxtFocusStarDEC2000SS.Text
                                    .FocusStarExposure = Double.Parse(TxtFocusStarExposure.Text, ciClone)
                                    .HADSError = Convert.ToInt16(ChkHADSError.Checked)
                                    .HADSErrorText = TxtHADSErrorText.Text
                                End With
                            End If
                            pCon.Close()

                        Catch ex As Exception
                            LoadRecord = "LoadRecord " + ex.Message
                            LogSessionEntry("ERROR", "LoadRecord " + ex.Message, "", "LoadRecord", "PROGRAM")
                        End Try
                    End Using
                End Using
            End Using
            Me.Cursor = Cursors.Default
        Catch ex As Exception
            ShowMessage("LoadRecord " + ex.Message, "CRITICAL", "Error!")
            Me.Cursor = Cursors.Default
            LoadRecord = "NOK"
        End Try
    End Function

    Public Function ClearDetailRecord() As String

        ClearDetailRecord = "OK"
        Try
            'clear the databindings
            TxtID.DataBindings.Clear()
            TxtHADSName.DataBindings.Clear()
            TxtHADSName2.DataBindings.Clear()
            TxtHADSName3.DataBindings.Clear()
            TxtHADSName4.DataBindings.Clear()
            TxtHADSName5.DataBindings.Clear()
            ChkHADSActive.DataBindings.Clear()

            TxtHADSMin.DataBindings.Clear()
            TxtHADSMax.DataBindings.Clear()
            TxtHADSPeriod.DataBindings.Clear()
            TxtHADSLastObserved.DataBindings.Clear()

            TxtHADSRA2000HH.DataBindings.Clear()
            TxtHADSRA2000MM.DataBindings.Clear()
            TxtHADSRA2000SS.DataBindings.Clear()
            TxtHADSDEC2000DG.DataBindings.Clear()
            TxtHADSDEC2000MM.DataBindings.Clear()
            TxtHADSDEC2000SS.DataBindings.Clear()

            TxtHADSExposure.DataBindings.Clear()
            ComboBoxHADSBinning.DataBindings.Clear()
            ComboBoxHADSFilter.DataBindings.Clear()

            TxtHADSJan.DataBindings.Clear()
            TxtHADSFeb.DataBindings.Clear()
            TxtHADSMar.DataBindings.Clear()
            TxtHADSApr.DataBindings.Clear()
            TxtHADSMay.DataBindings.Clear()
            TxtHADSJun.DataBindings.Clear()
            TxtHADSJul.DataBindings.Clear()
            TxtHADSAug.DataBindings.Clear()
            TxtHADSSep.DataBindings.Clear()
            TxtHADSOct.DataBindings.Clear()
            TxtHADSNov.DataBindings.Clear()
            TxtHADSDec.DataBindings.Clear()

            ChkHADSPriority.DataBindings.Clear()
            TxtFocusStarName.DataBindings.Clear()
            TxtFocusStarRA2000HH.DataBindings.Clear()
            TxtFocusStarRA2000MM.DataBindings.Clear()
            TxtFocusStarRA2000SS.DataBindings.Clear()
            TxtFocusStarDEC2000DG.DataBindings.Clear()
            TxtFocusStarDEC2000MM.DataBindings.Clear()
            TxtFocusStarDEC2000SS.DataBindings.Clear()
            TxtFocusStarExposure.DataBindings.Clear()
            ChkHADSError.DataBindings.Clear()
            TxtHADSErrorText.DataBindings.Clear()

            '---------------------------------------------
            'clear the textfields
            TxtID.Text = Nothing
            TxtHADSName.Text = Nothing
            TxtHADSName2.Text = Nothing
            TxtHADSName3.Text = Nothing
            TxtHADSName4.Text = Nothing
            TxtHADSName5.Text = Nothing
            ChkHADSActive.Checked = Nothing

            TxtHADSMin.Text = Nothing
            TxtHADSMax.Text = Nothing
            TxtHADSPeriod.Text = Nothing
            TxtHADSLastObserved.Text = Nothing

            TxtHADSRA2000HH.Text = Nothing
            TxtHADSRA2000MM.Text = Nothing
            TxtHADSRA2000SS.Text = Nothing
            TxtHADSDEC2000DG.Text = Nothing
            TxtHADSDEC2000MM.Text = Nothing
            TxtHADSDEC2000SS.Text = Nothing

            TxtHADSExposure.Text = Nothing
            ComboBoxHADSBinning.Text = Nothing
            ComboBoxHADSFilter.Text = Nothing

            TxtHADSJan.Text = Nothing
            TxtHADSFeb.Text = Nothing
            TxtHADSMar.Text = Nothing
            TxtHADSApr.Text = Nothing
            TxtHADSMay.Text = Nothing
            TxtHADSJun.Text = Nothing
            TxtHADSJul.Text = Nothing
            TxtHADSAug.Text = Nothing
            TxtHADSSep.Text = Nothing
            TxtHADSOct.Text = Nothing
            TxtHADSNov.Text = Nothing
            TxtHADSDec.Text = Nothing

            ChkHADSPriority.Checked = Nothing
            TxtFocusStarName.Text = Nothing
            TxtFocusStarRA2000HH.Text = Nothing
            TxtFocusStarRA2000MM.Text = Nothing
            TxtFocusStarRA2000SS.Text = Nothing
            TxtFocusStarDEC2000DG.Text = Nothing
            TxtFocusStarDEC2000MM.Text = Nothing
            TxtFocusStarDEC2000SS.Text = Nothing
            TxtFocusStarExposure.Text = Nothing
            ChkHADSError.Checked = Nothing
            TxtHADSErrorText.Text = Nothing

            'clear structure
            With pStrucHADSRecord
                .ID = Nothing
                .HADSName = Nothing
                .HADSName2 = Nothing
                .HADSName3 = Nothing
                .HADSName4 = Nothing
                .HADSName5 = Nothing
                .HADSActive = Nothing

                .HADSMin = Nothing
                .HADSMax = Nothing
                .HADSPeriod = Nothing
                .HADSLastObserved = Nothing

                .HADSRA2000HH = Nothing
                .HADSRA2000MM = Nothing
                .HADSRA2000SS = Nothing
                .HADSDEC2000DG = Nothing
                .HADSDEC2000MM = Nothing
                .HADSDEC2000SS = Nothing

                .HADSExposure = Nothing
                .HADSBin = Nothing
                .HADSFilter = Nothing

                .HADSJan = Nothing
                .HADSFeb = Nothing
                .HADSMar = Nothing
                .HADSApr = Nothing
                .HADSMay = Nothing
                .HADSJun = Nothing
                .HADSJul = Nothing
                .HADSAug = Nothing
                .HADSSep = Nothing
                .HADSOct = Nothing
                .HADSNov = Nothing
                .HADSDec = Nothing

                .HADSPriority = Nothing
                .FocusStarName = Nothing
                .FocusStarRA2000HH = Nothing
                .FocusStarRA2000MM = Nothing
                .FocusStarRA2000SS = Nothing
                .FocusStarDEC2000DG = Nothing
                .FocusStarDEC2000MM = Nothing
                .FocusStarDEC2000SS = Nothing
                .FocusStarExposure = Nothing
                .HADSError = Nothing
                .HADSErrorText = Nothing
            End With
        Catch ex As Exception
            ShowMessage("ClearDetailRecord " + ex.Message, "CRITICAL", "Error!")
            ClearDetailRecord = "NOK"
        End Try
    End Function

    Private Sub BtnFindObject_Click(sender As Object, e As EventArgs) Handles BtnDownloadHADSList.Click
        Dim returnvalue, returnvalue2 As String
        Me.Cursor = Cursors.WaitCursor

        returnvalue = ReadHADSFile()
        returnvalue2 = LoadDataGrid()

        If returnvalue <> "OK" Then
            ShowMessage("HADS-file Is Not read correctly.", "CRITICAL", "Read HADS-file")
        Else
            ShowMessage("HADS-file Is read correctly.", "OKONLY", "Read HADS-file")
        End If
        Me.Cursor = Cursors.Default
    End Sub

    Private Sub DataGridViewHADS_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridViewHADS.CellClick
        Dim HADSName, HADSFilter As String
        Dim returnvalue As String
        Dim cmd As New OleDb.OleDbCommand

        Try
            Me.Cursor = Cursors.WaitCursor
            '-------------------------------------------------------------------------------------------------------------
            'check for changes
            '-------------------------------------------------------------------------------------------------------------
            'update
            returnvalue = UpdateRecord()
            If returnvalue <> "OK" Then
                Exit Sub
            End If

            '-------------------------------------------------------------------------------------------------------------
            'change the row
            '-------------------------------------------------------------------------------------------------------------
            HADSName = DataGridViewHADS.CurrentRow.Cells(0).Value.ToString
            HADSFilter = DataGridViewHADS.CurrentRow.Cells(5).Value.ToString

            returnvalue = UpdateHADSFocusLink()
            If returnvalue <> "OK" Then
                Exit Sub
            End If

            returnvalue = LoadRecord(HADSName, HADSFilter)
            If returnvalue <> "OK" Then
                Exit Sub
            End If
            Me.Cursor = Cursors.Default
        Catch ex As Exception
            ShowMessage("DataGridViewHADS_CellClick: " + ex.Message, "CRITICAL", "Error!")
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub DataGridViewHADS_KeyDown(sender As Object, e As KeyEventArgs) Handles DataGridViewHADS.KeyDown
        Dim HADSName As String, HADSFilter As String
        Dim returnvalue As String
        Dim cmd As New OleDb.OleDbCommand

        Try
            Me.Cursor = Cursors.WaitCursor
            '-------------------------------------------------------------------------------------------------------------
            'check for changes
            '-------------------------------------------------------------------------------------------------------------
            'update
            returnvalue = UpdateRecord()
            If returnvalue <> "OK" Then
                Exit Sub
            End If

            returnvalue = UpdateHADSFocusLink()
            If returnvalue <> "OK" Then
                Exit Sub
            End If

            '-------------------------------------------------------------------------------------------------------------
            'change the row
            '-------------------------------------------------------------------------------------------------------------
            HADSName = DataGridViewHADS.CurrentRow.Cells(0).Value.ToString
            HADSFilter = DataGridViewHADS.CurrentRow.Cells(5).Value.ToString

            returnvalue = LoadRecord(HADSName, HADSFilter)
            If returnvalue <> "OK" Then
                Exit Sub
            End If
            Me.Cursor = Cursors.Default

        Catch ex As Exception
            ShowMessage("DataGridViewHADS_CellClick: " + ex.Message, "CRITICAL", "Error!")
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub DataGridViewHADS_KeyUp(sender As Object, e As KeyEventArgs) Handles DataGridViewHADS.KeyUp
        Dim HADSName, HADSFilter As String
        Dim returnvalue As String
        Dim cmd As New OleDb.OleDbCommand

        Try
            Me.Cursor = Cursors.WaitCursor
            '-------------------------------------------------------------------------------------------------------------
            'check for changes
            '-------------------------------------------------------------------------------------------------------------
            'update
            returnvalue = UpdateRecord()
            If returnvalue <> "OK" Then
                Exit Sub
            End If

            returnvalue = UpdateHADSFocusLink()
            If returnvalue <> "OK" Then
                Exit Sub
            End If

            '-------------------------------------------------------------------------------------------------------------
            'change the row
            '-------------------------------------------------------------------------------------------------------------
            HADSName = DataGridViewHADS.CurrentRow.Cells(0).Value.ToString
            HADSFilter = DataGridViewHADS.CurrentRow.Cells(5).Value.ToString

            returnvalue = LoadRecord(HADSName, HADSFilter)
            If returnvalue <> "OK" Then
                Exit Sub
            End If
            Me.Cursor = Cursors.Default
        Catch ex As Exception
            ShowMessage("DataGridViewHADS_CellClick: " + ex.Message, "CRITICAL", "Error!")
            Me.Cursor = Cursors.Default
        End Try
    End Sub
    Public Function UpdateRecord() As String
        Dim UpdateString As String
        Dim i As Integer
        Dim ciClone As CultureInfo = CType(CultureInfo.InvariantCulture.Clone(), CultureInfo)
        ciClone.NumberFormat.NumberDecimalSeparator = "."

        UpdateRecord = "OK"
        Try
            Me.Cursor = Cursors.WaitCursor
            UpdateString = ""
            'check for changes
            If pStrucHADSRecord.HADSFilter <> ComboBoxHADSFilter.Text Then
                If UpdateString <> "" Then
                    UpdateString += ","
                End If
                UpdateString += "HADSFilter='" + ComboBoxHADSFilter.Text + "'"
            End If

            If pStrucHADSRecord.HADSBin <> ComboBoxHADSBinning.Text Then
                If UpdateString <> "" Then
                    UpdateString += ","
                End If
                UpdateString += "HADSBin='" + ComboBoxHADSBinning.Text + "'"
            End If

            If pStrucHADSRecord.HADSExposure <> Double.Parse(TxtHADSExposure.Text, ciClone) Then
                If UpdateString <> "" Then
                    UpdateString += ","
                End If
                UpdateString += "HADSExposure=" + TxtHADSExposure.Text
            End If

            If pStrucHADSRecord.HADSLastObserved <> TxtHADSLastObserved.Text Then
                If UpdateString <> "" Then
                    UpdateString += ","
                End If
                UpdateString += "HADSLastObserved='" + TxtHADSLastObserved.Text + "'"
            End If

            If pStrucHADSRecord.HADSPriority <> Convert.ToInt16(ChkHADSPriority.Checked) Then
                If UpdateString <> "" Then
                    UpdateString += ","
                End If
                If ChkHADSPriority.Checked = True Then
                    UpdateString += "HADSPriority=1"
                Else
                    UpdateString += "HADSPriority=0"
                End If
            End If

            If pStrucHADSRecord.HADSError <> Convert.ToInt16(ChkHADSError.Checked) Then
                If UpdateString <> "" Then
                    UpdateString += ","
                End If
                If ChkHADSError.Checked = True Then
                    UpdateString += "HADSError=1"
                Else
                    UpdateString += "HADSError=0"
                End If
            End If

            If pStrucHADSRecord.HADSErrorText <> TxtHADSErrorText.Text Then
                If UpdateString <> "" Then
                    UpdateString += ","
                End If
                UpdateString += "HADSErrorText='" + TxtHADSErrorText.Text + "'"
            End If

            If pStrucHADSRecord.FocusStarName <> TxtFocusStarName.Text Then
                If UpdateString <> "" Then
                    UpdateString += ","
                End If
                UpdateString += "FocusStarName='" + TxtFocusStarName.Text + "'"
            End If

            If pStrucHADSRecord.FocusStarRA2000HH <> TxtFocusStarRA2000HH.Text Then
                If UpdateString <> "" Then
                    UpdateString += ","
                End If
                UpdateString += "FocusStarRA2000HH='" + TxtFocusStarRA2000HH.Text + "'"
            End If

            If pStrucHADSRecord.FocusStarRA2000MM <> TxtFocusStarRA2000MM.Text Then
                If UpdateString <> "" Then
                    UpdateString += ","
                End If
                UpdateString += "FocusStarRA2000MM='" + TxtFocusStarRA2000MM.Text + "'"
            End If

            If pStrucHADSRecord.FocusStarRA2000SS <> TxtFocusStarRA2000SS.Text Then
                If UpdateString <> "" Then
                    UpdateString += ","
                End If
                UpdateString += "FocusStarRA2000SS='" + TxtFocusStarRA2000SS.Text + "'"
            End If

            If pStrucHADSRecord.FocusStarDEC2000DG <> TxtFocusStarDEC2000DG.Text Then
                If UpdateString <> "" Then
                    UpdateString += ","
                End If
                UpdateString += "FocusStarDEC2000DG='" + TxtFocusStarDEC2000DG.Text + "'"
            End If

            If pStrucHADSRecord.FocusStarDEC2000MM <> TxtFocusStarDEC2000MM.Text Then
                If UpdateString <> "" Then
                    UpdateString += ","
                End If
                UpdateString += "FocusStarDEC2000MM='" + TxtFocusStarDEC2000MM.Text + "'"
            End If

            If pStrucHADSRecord.FocusStarDEC2000SS <> TxtFocusStarDEC2000SS.Text Then
                If UpdateString <> "" Then
                    UpdateString += ","
                End If
                UpdateString += "FocusStarDEC2000SS='" + TxtFocusStarDEC2000SS.Text + "'"
            End If

            If pStrucHADSRecord.FocusStarExposure <> Double.Parse(TxtFocusStarExposure.Text, ciClone) Then
                If UpdateString <> "" Then
                    UpdateString += ","
                End If
                UpdateString += "FocusStarExposure=" + TxtFocusStarExposure.Text
            End If

            If UpdateString <> "" Then
                Using pCon As New SQLiteConnection("Data Source=RoboticObservatory.db;Version=3;")
                    Using cmd As SQLiteCommand = pCon.CreateCommand
                        Using da As New SQLiteDataAdapter
                            Try
                                pCon.Open()
                                da.SelectCommand = cmd
                                cmd.CommandText = "UPDATE HADS SET " + UpdateString + " WHERE Id =" + TxtID.Text
                                da.SelectCommand = cmd
                                i = cmd.ExecuteNonQuery
                                pCon.Close()

                            Catch ex As Exception
                                UpdateRecord = "UpdateRecord: " + ex.Message
                                LogSessionEntry("ERROR", "UpdateRecord: " + ex.Message, "", "UpdateRecord", "PROGRAM")
                            End Try

                        End Using
                    End Using
                End Using



                'reload datagrid
                'returnvalue = LoadDataGrid()
                'If returnvalue <> "OK" Then
                'UpdateRecord = "NOK"
                'Exit Function
                'End If

            End If
            Me.Cursor = Cursors.Default
        Catch ex As Exception
            ShowMessage("UpdateRecord: " + ex.Message, "CRITICAL", "Error!")
            Me.Cursor = Cursors.Default
            UpdateRecord = "NOK"
        End Try
    End Function

    Private Sub FrmHADS_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        Dim returnvalue As String
        Try
            '-------------------------------------------------------------------------------------------------------------
            'check for changes
            '-------------------------------------------------------------------------------------------------------------
            'update
            If TxtID.Text <> "" Then
                returnvalue = UpdateRecord()
                If returnvalue <> "OK" Then
                    Exit Sub
                End If
            End If

        Catch ex As Exception
            ShowMessage(" FrmHADS_Closing: " + ex.Message, "CRITICAL", "Error!")
        End Try
    End Sub

    Private Sub BtnCancel_Click(sender As Object, e As EventArgs) Handles BtnCancel.Click
        Me.Close()
    End Sub

    Private Sub TxtFocusStarRA2000HH_TextChanged(sender As Object, e As EventArgs) Handles TxtFocusStarRA2000HH.TextChanged
        Try
            Dim ciClone As CultureInfo = CType(CultureInfo.InvariantCulture.Clone(), CultureInfo)
            ciClone.NumberFormat.NumberDecimalSeparator = "."

            If TxtFocusStarRA2000HH.Text <> "" Then
                If IsNumeric(TxtFocusStarRA2000HH.Text) = False Then
                    ShowMessage("Not a number between 0 and 24!", "CRITICAL", "Incorrect input...")
                    Exit Sub
                End If
                If Double.Parse(TxtFocusStarRA2000HH.Text, ciClone) < 0 Or Double.Parse(TxtFocusStarRA2000HH.Text, ciClone) > 24 Then
                    ShowMessage("Not a number between 0 and 24!", "CRITICAL", "Incorrect input...")
                    Exit Sub
                End If
            End If
        Catch ex As Exception
            ShowMessage("TxtFocusStarRA2000HH_TextChanged: " + ex.Message, "CRITICAL", "Error!")
        End Try
    End Sub

    Private Sub TxtFocusStarRA2000MM_TextChanged(sender As Object, e As EventArgs) Handles TxtFocusStarRA2000MM.TextChanged
        Try
            Dim ciClone As CultureInfo = CType(CultureInfo.InvariantCulture.Clone(), CultureInfo)
            ciClone.NumberFormat.NumberDecimalSeparator = "."

            If TxtFocusStarRA2000MM.Text <> "" Then
                If IsNumeric(TxtFocusStarRA2000MM.Text) = False Then
                    ShowMessage("Not a number between 0 and 60!", "CRITICAL", "Incorrect input...")
                    Exit Sub
                End If
                If Double.Parse(TxtFocusStarRA2000MM.Text, ciClone) < 0 Or Double.Parse(TxtFocusStarRA2000MM.Text, ciClone) > 60 Then
                    ShowMessage("Not a number between 0 and 60!", "CRITICAL", "Incorrect input...")
                    Exit Sub
                End If
            End If
        Catch ex As Exception
            ShowMessage("TxtFocusStarRA2000MM_TextChanged: " + ex.Message, "CRITICAL", "Error!")
        End Try
    End Sub

    Private Sub TxtFocusStarRA2000SS_TextChanged(sender As Object, e As EventArgs) Handles TxtFocusStarRA2000SS.TextChanged
        Try
            Dim ciClone As CultureInfo = CType(CultureInfo.InvariantCulture.Clone(), CultureInfo)
            ciClone.NumberFormat.NumberDecimalSeparator = "."

            If TxtFocusStarRA2000SS.Text <> "" Then
                If IsNumeric(TxtFocusStarRA2000SS.Text) = False Then
                    ShowMessage("Not a number between 0 and 60!", "CRITICAL", "Incorrect input...")
                    Exit Sub
                End If
                If Double.Parse(TxtFocusStarRA2000SS.Text, ciClone) < 0 Or Double.Parse(TxtFocusStarRA2000SS.Text, ciClone) > 60 Then
                    ShowMessage("Not a number between 0 and 60!", "CRITICAL", "Incorrect input...")
                    Exit Sub
                End If
            End If
        Catch ex As Exception
            ShowMessage("TxtFocusStarRA2000SS_TextChanged: " + ex.Message, "CRITICAL", "Error!")
        End Try
    End Sub

    Private Sub TxtFocusStarDEC2000DG_TextChanged(sender As Object, e As EventArgs) Handles TxtFocusStarDEC2000DG.TextChanged
        Try
            If TxtFocusStarDEC2000DG.Text <> "" Then
                If IsNumeric(TxtFocusStarDEC2000DG.Text) = False Then
                    ShowMessage("Not a number!", "CRITICAL", "Incorrect input...")
                    Exit Sub
                End If
            End If
        Catch ex As Exception
            ShowMessage("TxtFocusStarDEC2000DG_TextChanged: " + ex.Message, "CRITICAL", "Error!")
        End Try
    End Sub

    Private Sub TxtFocusStarDEC2000MM_TextChanged(sender As Object, e As EventArgs) Handles TxtFocusStarDEC2000MM.TextChanged
        Try
            Dim ciClone As CultureInfo = CType(CultureInfo.InvariantCulture.Clone(), CultureInfo)
            ciClone.NumberFormat.NumberDecimalSeparator = "."

            If TxtFocusStarDEC2000MM.Text <> "" Then
                If IsNumeric(TxtFocusStarDEC2000MM.Text) = False Then
                    ShowMessage("Not a number between 0 and 60!", "CRITICAL", "Incorrect input...")
                    Exit Sub
                End If
                If Double.Parse(TxtFocusStarDEC2000MM.Text, ciClone) < 0 Or Double.Parse(TxtFocusStarDEC2000MM.Text, ciClone) > 60 Then
                    ShowMessage("Not a number between 0 and 60!", "CRITICAL", "Incorrect input...")
                    Exit Sub
                End If
            End If
        Catch ex As Exception
            ShowMessage("TxtFocusStarDEC2000MM_TextChanged: " + ex.Message, "CRITICAL", "Error!")
        End Try
    End Sub

    Private Sub TxtFocusStarDEC2000SS_TextChanged(sender As Object, e As EventArgs) Handles TxtFocusStarDEC2000SS.TextChanged
        Try
            Dim ciClone As CultureInfo = CType(CultureInfo.InvariantCulture.Clone(), CultureInfo)
            ciClone.NumberFormat.NumberDecimalSeparator = "."

            If TxtFocusStarDEC2000SS.Text <> "" Then
                If IsNumeric(TxtFocusStarDEC2000SS.Text) = False Then
                    ShowMessage("Not a number between 0 and 60!", "CRITICAL", "Incorrect input...")
                    Exit Sub
                End If
                If Double.Parse(TxtFocusStarDEC2000SS.Text, ciClone) < 0 Or Double.Parse(TxtFocusStarDEC2000SS.Text, ciClone) > 60 Then
                    ShowMessage("Not a number between 0 and 60!", "CRITICAL", "Incorrect input...")
                    Exit Sub
                End If
            End If
        Catch ex As Exception
            ShowMessage("TxtFocusStarDEC2000SS_TextChanged: " + ex.Message, "CRITICAL", "Error!")
        End Try
    End Sub

    Private Sub TxtFocusStarExposure_TextChanged(sender As Object, e As EventArgs) Handles TxtFocusStarExposure.TextChanged
        Try
            Dim ciClone As CultureInfo = CType(CultureInfo.InvariantCulture.Clone(), CultureInfo)
            ciClone.NumberFormat.NumberDecimalSeparator = "."

            If TxtFocusStarExposure.Text <> "" Then
                If IsNumeric(TxtFocusStarExposure.Text) = False Then
                    ShowMessage("Not a number between 0 and 9999!", "CRITICAL", "Incorrect input...")
                    Exit Sub
                End If
                If Double.Parse(TxtFocusStarExposure.Text, ciClone) < 0 Or Double.Parse(TxtFocusStarExposure.Text, ciClone) > 9999 Then
                    ShowMessage("Not a number between 0 and 9999!", "CRITICAL", "Incorrect input...")
                    Exit Sub
                End If
            End If
        Catch ex As Exception
            ShowMessage("TxtFocusStarExposure_TextChanged: " + ex.Message, "CRITICAL", "Error!")
        End Try
    End Sub

    Private Sub TxtHADSExposure_TextChanged(sender As Object, e As EventArgs) Handles TxtHADSExposure.TextChanged
        Try
            Dim ciClone As CultureInfo = CType(CultureInfo.InvariantCulture.Clone(), CultureInfo)
            ciClone.NumberFormat.NumberDecimalSeparator = "."

            If TxtHADSExposure.Text <> "" Then
                If IsNumeric(TxtHADSExposure.Text) = False Then
                    ShowMessage("Not a number between 0 and 9999!", "CRITICAL", "Incorrect input...")
                    Exit Sub
                End If
                If Double.Parse(TxtHADSExposure.Text, ciClone) < 0 Or Double.Parse(TxtHADSExposure.Text, ciClone) > 9999 Then
                    ShowMessage("Not a number between 0 and 9999!", "CRITICAL", "Incorrect input...")
                    Exit Sub
                End If
            End If

        Catch ex As Exception
            ShowMessage("TxtTargetExposure_TextChanged: " + ex.Message, "CRITICAL", "Error!")
        End Try
    End Sub

    Private Sub ChkHADSError_CheckedChanged(sender As Object, e As EventArgs) Handles ChkHADSError.CheckedChanged
        TxtHADSErrorText.Text = ""
    End Sub

    Private Sub BtnSave_Click(sender As Object, e As EventArgs) Handles BtnSave.Click
        Dim returnvalue As String
        Try
            Dim ciClone As CultureInfo = CType(CultureInfo.InvariantCulture.Clone(), CultureInfo)
            ciClone.NumberFormat.NumberDecimalSeparator = "."

            'check if target and focusstar are not too far of
            If TxtFocusStarName.Text <> "" Then
                If Math.Abs(Double.Parse(TxtFocusStarRA2000HH.Text, ciClone) - Double.Parse(TxtHADSRA2000HH.Text, ciClone)) > 2 Or
                        Math.Abs(Double.Parse(TxtFocusStarDEC2000DG.Text, ciClone) - Double.Parse(TxtHADSDEC2000DG.Text, ciClone)) > 10 Then

                    ShowMessage("Are you sure the focusstar Is correct? It should be maximum within 2 hours RA And 10° DEC from the target!", "CRITICAL", "Check the focusstar ...")
                    Exit Sub
                End If
            End If

            '-------------------------------------------------------------------------------------------------------------
            'check for changes
            '-------------------------------------------------------------------------------------------------------------
            'update
            returnvalue = UpdateRecord()
            If returnvalue <> "OK" Then
                Exit Sub
            End If

            returnvalue = UpdateHADSFocusLink()
            If returnvalue <> "OK" Then
                Exit Sub
            End If

        Catch ex As Exception
            ShowMessage("BtnSave_Click: " + ex.Message, "CRITICAL", "Error!")
        End Try
    End Sub

    Private Sub BtnFindFocusStar_Click(sender As Object, e As EventArgs) Handles BtnFindFocusStar.Click
        Dim returnvalue As String

        Try
            If TxtFocusStarName.Text <> "" Then

                returnvalue = FindTheSkyXTarget(TxtFocusStarName.Text)

                Select Case returnvalue
                    Case "NOTFOUND"
                        ShowMessage("Focus star not found !", "CRITICAL", "The Sky X Application")
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
                TxtFocusStarRA2000HH.Text = RA_string.Substring(0, index)
                old_index = index
                index = GetNthIndex(RA_string, ":"c, 2)
                TxtFocusStarRA2000MM.Text = RA_string.Substring(old_index + 1, index - old_index - 1)
                TxtFocusStarRA2000SS.Text = RA_string.Substring(index + 1, 2)

                index = GetNthIndex(DEC_string, "°"c, 1)
                TxtFocusStarDEC2000DG.Text = DEC_string.Substring(0, index)
                TxtFocusStarDEC2000MM.Text = DEC_string.Substring(index + 2, 2)
                index = GetNthIndex(DEC_string, "'"c, 1)
                TxtFocusStarDEC2000SS.Text = DEC_string.Substring(index + 2, 2)
            End If
        Catch ex As Exception
            ShowMessage("BtnFindObject_Click: " + ex.Message, "CRITICAL", "Error!")
        End Try
    End Sub

    Private Sub BtnEstimate_Click(sender As Object, e As EventArgs) Handles BtnEstimate.Click
        'estimate the exposure of the variable
        Dim ciClone As CultureInfo = CType(CultureInfo.InvariantCulture.Clone(), CultureInfo)
        ciClone.NumberFormat.NumberDecimalSeparator = ","

        TxtHADSExposure.Text = EstimateExposureTime(Double.Parse(TxtHADSMax.Text, ciClone)).ToString
    End Sub

    Private Sub BtnTruncateHADS_Click(sender As Object, e As EventArgs) Handles BtnTruncateHADS.Click
        Dim returnvalue, returnvalue2 As String
        Me.Cursor = Cursors.WaitCursor

        returnvalue = DatabaseTruncateHADS()
        returnvalue2 = LoadDataGrid()

        If returnvalue <> "OK" Then
            ShowMessage("HADS-table was not truncated correctly.", "CRITICAL", "Read HADS-file")
        Else
            ShowMessage("HADS-file truncated correctly.", "OKONLY", "Read HADS-file")
        End If
        Me.Cursor = Cursors.Default
    End Sub

    Private Sub BtnDeleteInactiveHADS_Click(sender As Object, e As EventArgs) Handles BtnDeleteInactiveHADS.Click
        Dim returnvalue, returnvalue2 As String
        Me.Cursor = Cursors.WaitCursor

        returnvalue = DatabaseDeleteInactiveHADS()
        returnvalue2 = LoadDataGrid()

        If returnvalue <> "OK" Then
            ShowMessage("Inactive records HADS-table were NOT deleted correctly.", "CRITICAL", "Read HADS-file")
        Else
            ShowMessage("Inactive records HADS-table were deleted correctly.", "OKONLY", "Read HADS-file")
        End If
        Me.Cursor = Cursors.Default
    End Sub

    Private Sub BtnTSXCenter_Click(sender As Object, e As EventArgs) Handles BtnTSXCenter.Click
        Dim returnvalue As String
        Dim RA2000, DEC2000 As Double
        Try
            'RA2000 and DEC2000
            RA2000 = pAUtil.HMSToHours(TxtHADSRA2000HH.Text + " " + TxtHADSRA2000MM.Text + " " + TxtHADSRA2000SS.Text)
            DEC2000 = pAUtil.DMSToDegrees(TxtHADSDEC2000DG.Text + " " + TxtHADSDEC2000MM.Text + " " + TxtHADSDEC2000SS.Text)

            returnvalue = FindTheSkyXRADEC(RA2000, DEC2000)

        Catch ex As Exception
            ShowMessage("BtnTSXCenter_Click: " + ex.Message, "CRITICAL", "Error!")
        End Try
    End Sub


End Class