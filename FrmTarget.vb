Imports System.ComponentModel
Imports System.Data.SqlClient
Imports System.Data.SQLite
Imports System.Data.SqlTypes
Imports System.Globalization
Imports System.Net.Mime.MediaTypeNames

Public Class FrmTarget

    Public dtSL_master As New DataTable
    Public daSL_master As New OleDb.OleDbDataAdapter
    Public dtSL_detail As New DataTable
    Public daSL_detail As New OleDb.OleDbDataAdapter

    Private Sub FrmTarget_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim returnvalue As String

        Try
            'add combobox options
            ComboBoxTargetFilter.Items.Add(My.Settings.sCCDFilter1)
            ComboBoxTargetFilter.Items.Add(My.Settings.sCCDFilter2)
            ComboBoxTargetFilter.Items.Add(My.Settings.sCCDFilter3)
            ComboBoxTargetFilter.Items.Add(My.Settings.sCCDFilter4)
            ComboBoxTargetFilter.Items.Add(My.Settings.sCCDFilter5)

            LabelMosaic1.Text = ""
            LabelMosaic2.Text = ""
            LabelMosaic3.Text = ""
            LabelMosaic4.Text = ""
            LabelMosaic1.BorderStyle = BorderStyle.None
            LabelMosaic2.BorderStyle = BorderStyle.None
            LabelMosaic3.BorderStyle = BorderStyle.None
            LabelMosaic4.BorderStyle = BorderStyle.None

            ChkTargetMosaic_CheckedChanged(Nothing, Nothing)

            returnvalue = LoadDataGrid()
            If returnvalue <> "OK" Then
                Exit Sub
            End If

        Catch ex As Exception
            ShowMessage("FrmTarget_Load: " + ex.Message, "CRITICAL", "Error!")
        End Try
    End Sub

    Public Function LoadDataGrid() As String
        Dim returnvalue As String
        LoadDataGrid = "OK"
        Try
            dtSL_master.Clear()

            Using pCon As New SQLiteConnection("Data Source=RoboticObservatory.db;Version=3;")
                Using cmd As SQLiteCommand = pCon.CreateCommand
                    Using daSL_master As New SQLiteDataAdapter
                        Try
                            pCon.Open()
                            daSL_master.SelectCommand = cmd
                            cmd.CommandText = "Select TargetName as Name, TargetFilter as Filter, TargetDone as Done, TargetNbrFrames - TargetNbrExposedFrames as Todo, TargetPriority as Priority, " +
                                              "TargetError as Error, TargetLastObservedDate as LastObserved, TargetMosaic as Mosaic from Target order by 6 desc, 3 asc, 5 desc, 1 asc "
                            daSL_master.SelectCommand = cmd

                            'Add the columns
                            If dtSL_master.Columns.Count = 8 Then
                                'do nothing
                            Else
                                Using colName As New DataColumn("Name")
                                    colName.DataType = System.Type.GetType("System.String")
                                    dtSL_master.Columns.Add(colName)
                                End Using

                                Using colFilter As New DataColumn("Filter")
                                    colFilter.DataType = System.Type.GetType("System.String")
                                    dtSL_master.Columns.Add(colFilter)
                                End Using

                                Using colDone As New DataColumn("Done")
                                    colDone.DataType = System.Type.GetType("System.Boolean")
                                    dtSL_master.Columns.Add(colDone)
                                End Using

                                Using colTodo As New DataColumn("Todo")
                                    colTodo.DataType = System.Type.GetType("System.Int32")
                                    dtSL_master.Columns.Add(colTodo)
                                End Using

                                Using colPriority As New DataColumn("Priority")
                                    colPriority.DataType = System.Type.GetType("System.Boolean")
                                    dtSL_master.Columns.Add(colPriority)
                                End Using

                                Using colError As New DataColumn("Error")
                                    colError.DataType = System.Type.GetType("System.Boolean")
                                    dtSL_master.Columns.Add(colError)
                                End Using

                                Using colLastObserved As New DataColumn("LastObserved")
                                    colLastObserved.DataType = System.Type.GetType("System.String")
                                    dtSL_master.Columns.Add(colLastObserved)
                                End Using

                                Using colMosaic As New DataColumn("Mosaic")
                                    colMosaic.DataType = System.Type.GetType("System.Boolean")
                                    dtSL_master.Columns.Add(colMosaic)
                                End Using

                            End If

                            daSL_master.Fill(dtSL_master)
                            pCon.Close()

                        Catch ex As Exception
                            LoadDataGrid = "LoadDataGrid:     " + ex.Message
                            LogSessionEntry("ERROR", "LoadDataGrid " + ex.Message, "", "LoadDataGrid", "PROGRAM")
                        End Try
                    End Using
                End Using
            End Using

            'load first record
            If dtSL_master.Rows.Count > 0 Then
                returnvalue = LoadRecord(dtSL_master.Rows(0).Item(0).ToString, dtSL_master.Rows(0).Item(1).ToString)
                If returnvalue <> "OK" Then
                    LoadDataGrid = "OK"
                    Exit Function
                End If
            End If

            DataGridViewTarget.DataSource = dtSL_master

            DataGridViewTarget.Columns(0).Width = 100
            DataGridViewTarget.Columns(1).Width = 50
            DataGridViewTarget.Columns(2).Width = 50 'checkbox
            DataGridViewTarget.Columns(3).Width = 50
            DataGridViewTarget.Columns(4).Width = 50
            DataGridViewTarget.Columns(5).Width = 30 'checkbox
            DataGridViewTarget.Columns(6).Width = 120
            DataGridViewTarget.Columns(7).Width = 50 'checkbox
            DataGridViewTarget.Width = 525
            DataGridViewTarget.BackgroundColor = Color.Silver

        Catch ex As Exception
            ShowMessage("LoadDataGrid " + ex.Message, "CRITICAL", "Error!")
            LoadDataGrid = "NOK"
        End Try
    End Function

    Public Function LoadRecord(vTargetName As String, vTargetFilter As String) As String
        Dim returnvalue As String
        Dim ciClone As CultureInfo = CType(CultureInfo.InvariantCulture.Clone(), CultureInfo)
        ciClone.NumberFormat.NumberDecimalSeparator = "."

        LoadRecord = "OK"
        Try
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
                            'cmd.CommandText = "Select * from Target where TargetName=""" + vTargetName + """ And TargetFilter =""" + vTargetFilter + """"

                            cmd.CommandText = "Select ID, TargetName, TargetRA2000HH, TargetRA2000MM, TargetRA2000SS, " +
                                               "TargetDEC2000DG, TargetDEC2000MM, TargetDEC2000SS, " +
                                               "TargetExposure, TargetBin, TargetFilter, TargetDone, TargetPriority, " +
                                               "TargetIsComet, TargetIgnoreMoon, TargetNbrExposedFrames, TargetNbrFrames, " +
                                               "FocusStarName, FocusStarRA2000HH, FocusStarRA2000MM, FocusStarRA2000SS, " +
                                               "FocusStarDEC2000DG, FocusStarDEC2000MM, FocusStarDEC2000SS, FocusStarExposure, " +
                                               "GuideAuto, GuideExposure, GuideStarXBMF, GuideStarXPMF, GuideStarYBMF, GuideStarYPMF, " +
                                               "TargetError, TargetErrorText, TargetRemarks, TargetLastObservedDate, " +
                                               "TargetMosaic, TargetMosaicType, TargetMosaicFramesPerPanel, " +
                                               "TargetPanel1RA2000HH, TargetPanel2RA2000HH, TargetPanel3RA2000HH, TargetPanel4RA2000HH, " +
                                               "TargetPanel1RA2000MM, TargetPanel2RA2000MM, TargetPanel3RA2000MM, TargetPanel4RA2000MM, " +
                                               "TargetPanel1RA2000SS, TargetPanel2RA2000SS, TargetPanel3RA2000SS, TargetPanel4RA2000SS, " +
                                               "TargetPanel1DEC2000DG, TargetPanel2DEC2000DG, TargetPanel3DEC2000DG, TargetPanel4DEC2000DG, " +
                                               "TargetPanel1DEC2000MM, TargetPanel2DEC2000MM, TargetPanel3DEC2000MM, TargetPanel4DEC2000MM, " +
                                               "TargetPanel1DEC2000SS, TargetPanel2DEC2000SS, TargetPanel3DEC2000SS, TargetPanel4DEC2000SS, " +
                                               "TargetPanel1NbrExposedFrames, TargetPanel2NbrExposedFrames, TargetPanel3NbrExposedFrames, TargetPanel4NbrExposedFrames, TargetMosaicOverlap " +
                                               "from Target where TargetName=""" + vTargetName + """ And TargetFilter =""" + vTargetFilter + """"

                            daSL_detail.SelectCommand = cmd
                            dtSL_detail.Clear()

                            daSL_detail.Fill(dtSL_detail)

                            If dtSL_detail.Rows.Count > 1 Then
                                'error
                            Else
                                Dim dv As New DataView(dtSL_detail)

                                TxtTargetName.DataBindings.Add("text", dv, "TargetName")
                                TxtID.DataBindings.Add("text", dv, "ID")
                                TxtTargetRA2000HH.DataBindings.Add("text", dv, "TargetRA2000HH")
                                TxtTargetRA2000MM.DataBindings.Add("text", dv, "TargetRA2000MM")
                                TxtTargetRA2000SS.DataBindings.Add("text", dv, "TargetRA2000SS")
                                TxtTargetDEC2000DG.DataBindings.Add("text", dv, "TargetDEC2000DG")
                                TxtTargetDEC2000MM.DataBindings.Add("text", dv, "TargetDEC2000MM")
                                TxtTargetDEC2000SS.DataBindings.Add("text", dv, "TargetDEC2000SS")
                                TxtTargetExposure.DataBindings.Add("text", dv, "TargetExposure")
                                ComboBoxTargetBinning.DataBindings.Add("text", dv, "TargetBin")
                                ComboBoxTargetFilter.DataBindings.Add("text", dv, "TargetFilter")
                                ChkTargetDone.DataBindings.Add("checked", dv, "TargetDone")
                                ChkPriorityTarget.DataBindings.Add("checked", dv, "TargetPriority")
                                ChkTargetIsComet.DataBindings.Add("checked", dv, "TargetIsComet")
                                ChkTargetIgnoreMoon.DataBindings.Add("checked", dv, "TargetIgnoreMoon")
                                TxtTargetNbrExposedFrames.DataBindings.Add("text", dv, "TargetNbrExposedFrames")
                                TxtTargetNbrFrames.DataBindings.Add("text", dv, "TargetNbrFrames")
                                TxtFocusStarName.DataBindings.Add("text", dv, "FocusStarName")
                                TxtFocusStarRA2000HH.DataBindings.Add("text", dv, "FocusStarRA2000HH")
                                TxtFocusStarRA2000MM.DataBindings.Add("text", dv, "FocusStarRA2000MM")
                                TxtFocusStarRA2000SS.DataBindings.Add("text", dv, "FocusStarRA2000SS")
                                TxtFocusStarDEC2000DG.DataBindings.Add("text", dv, "FocusStarDEC2000DG")
                                TxtFocusStarDEC2000MM.DataBindings.Add("text", dv, "FocusStarDEC2000MM")
                                TxtFocusStarDEC2000SS.DataBindings.Add("text", dv, "FocusStarDEC2000SS")
                                TxtFocusStarExposure.DataBindings.Add("text", dv, "FocusStarExposure")
                                ChkGuideAuto.DataBindings.Add("checked", dv, "GuideAuto")
                                TxtGuideStarExposure.DataBindings.Add("text", dv, "GuideExposure")
                                TxtGuideStarXBMF.DataBindings.Add("text", dv, "GuideStarXBMF")
                                TxtGuideStarXPMF.DataBindings.Add("text", dv, "GuideStarXPMF")
                                TxtGuideStarYBMF.DataBindings.Add("text", dv, "GuideStarYBMF")
                                TxtGuideStarYPMF.DataBindings.Add("text", dv, "GuideStarYPMF")
                                ChkErrorObservingTarget.DataBindings.Add("checked", dv, "TargetError")
                                TxtErrorTextTarget.DataBindings.Add("text", dv, "TargetErrorText")
                                TxtTargetRemarks.DataBindings.Add("text", dv, "TargetRemarks")
                                TxtTargetLastObservedDate.DataBindings.Add("text", dv, "TargetLastObservedDate")
                                ChkTargetMosaic.DataBindings.Add("checked", dv, "TargetMosaic")
                                CmbTargetMosaicType.DataBindings.Add("text", dv, "TargetMosaicType")
                                TxtTargetMosaicFramesPerPanel.DataBindings.Add("text", dv, "TargetMosaicFramesPerPanel")
                                TxtTargetPanel1RA2000HH.DataBindings.Add("text", dv, "TargetPanel1RA2000HH")
                                TxtTargetPanel2RA2000HH.DataBindings.Add("text", dv, "TargetPanel2RA2000HH")
                                TxtTargetPanel3RA2000HH.DataBindings.Add("text", dv, "TargetPanel3RA2000HH")
                                TxtTargetPanel4RA2000HH.DataBindings.Add("text", dv, "TargetPanel4RA2000HH")
                                TxtTargetPanel1RA2000MM.DataBindings.Add("text", dv, "TargetPanel1RA2000MM")
                                TxtTargetPanel2RA2000MM.DataBindings.Add("text", dv, "TargetPanel2RA2000MM")
                                TxtTargetPanel3RA2000MM.DataBindings.Add("text", dv, "TargetPanel3RA2000MM")
                                TxtTargetPanel4RA2000MM.DataBindings.Add("text", dv, "TargetPanel4RA2000MM")
                                TxtTargetPanel1RA2000SS.DataBindings.Add("text", dv, "TargetPanel1RA2000SS")
                                TxtTargetPanel2RA2000SS.DataBindings.Add("text", dv, "TargetPanel2RA2000SS")
                                TxtTargetPanel3RA2000SS.DataBindings.Add("text", dv, "TargetPanel3RA2000SS")
                                TxtTargetPanel4RA2000SS.DataBindings.Add("text", dv, "TargetPanel4RA2000SS")
                                TxtTargetPanel1DEC2000DG.DataBindings.Add("text", dv, "TargetPanel1DEC2000DG")
                                TxtTargetPanel2DEC2000DG.DataBindings.Add("text", dv, "TargetPanel2DEC2000DG")
                                TxtTargetPanel3DEC2000DG.DataBindings.Add("text", dv, "TargetPanel3DEC2000DG")
                                TxtTargetPanel4DEC2000DG.DataBindings.Add("text", dv, "TargetPanel4DEC2000DG")
                                TxtTargetPanel1DEC2000MM.DataBindings.Add("text", dv, "TargetPanel1DEC2000MM")
                                TxtTargetPanel2DEC2000MM.DataBindings.Add("text", dv, "TargetPanel2DEC2000MM")
                                TxtTargetPanel3DEC2000MM.DataBindings.Add("text", dv, "TargetPanel3DEC2000MM")
                                TxtTargetPanel4DEC2000MM.DataBindings.Add("text", dv, "TargetPanel4DEC2000MM")
                                TxtTargetPanel1DEC2000SS.DataBindings.Add("text", dv, "TargetPanel1DEC2000SS")
                                TxtTargetPanel2DEC2000SS.DataBindings.Add("text", dv, "TargetPanel2DEC2000SS")
                                TxtTargetPanel3DEC2000SS.DataBindings.Add("text", dv, "TargetPanel3DEC2000SS")
                                TxtTargetPanel4DEC2000SS.DataBindings.Add("text", dv, "TargetPanel4DEC2000SS")
                                TxtTargetPanel1NbrExposedFrames.DataBindings.Add("text", dv, "TargetPanel1NbrExposedFrames")
                                TxtTargetPanel2NbrExposedFrames.DataBindings.Add("text", dv, "TargetPanel2NbrExposedFrames")
                                TxtTargetPanel3NbrExposedFrames.DataBindings.Add("text", dv, "TargetPanel3NbrExposedFrames")
                                TxtTargetPanel4NbrExposedFrames.DataBindings.Add("text", dv, "TargetPanel4NbrExposedFrames")
                                TxtTargetMosaicOverlap.DataBindings.Add("text", dv, "TargetMosaicOverlap")

                                'set the current record
                                With pStrucSLCurrentRecord
                                    .TargetName = TxtTargetName.Text
                                    .ID = Convert.ToInt32(TxtID.Text)
                                    .TargetRA2000HH = TxtTargetRA2000HH.Text
                                    .TargetRA2000MM = TxtTargetRA2000MM.Text
                                    .TargetRA2000SS = TxtTargetRA2000SS.Text
                                    .TargetDEC2000DG = TxtTargetDEC2000DG.Text
                                    .TargetDEC2000MM = TxtTargetDEC2000MM.Text
                                    .TargetDEC2000SS = TxtTargetDEC2000SS.Text
                                    .TargetExposure = Double.Parse(TxtTargetExposure.Text, ciClone)
                                    .TargetBin = ComboBoxTargetBinning.Text
                                    .TargetFilter = ComboBoxTargetFilter.Text
                                    .TargetDone = ChkTargetDone.Checked
                                    .TargetPriority = ChkPriorityTarget.Checked
                                    .TargetIsComet = ChkTargetIsComet.Checked
                                    .TargetIgnoreMoon = ChkTargetIgnoreMoon.Checked
                                    .TargetNbrExposedFrames = Convert.ToInt32(TxtTargetNbrExposedFrames.Text)
                                    .TargetNbrFrames = Convert.ToInt32(TxtTargetNbrFrames.Text)
                                    .FocusStarName = TxtFocusStarName.Text
                                    .FocusStarRA2000HH = TxtFocusStarRA2000HH.Text
                                    .FocusStarRA2000MM = TxtFocusStarRA2000MM.Text
                                    .FocusStarRA2000SS = TxtFocusStarRA2000SS.Text
                                    .FocusStarDEC2000DG = TxtFocusStarDEC2000DG.Text
                                    .FocusStarDEC2000MM = TxtFocusStarDEC2000MM.Text
                                    .FocusStarDEC2000SS = TxtFocusStarDEC2000SS.Text
                                    .FocusStarExposure = Double.Parse(TxtFocusStarExposure.Text, ciClone)
                                    .GuideAuto = ChkGuideAuto.Checked
                                    .GuideStarExposure = Double.Parse(TxtGuideStarExposure.Text, ciClone)
                                    .GuideStarXBMF = Convert.ToInt32(TxtGuideStarXBMF.Text)
                                    .GuideStarXPMF = Convert.ToInt32(TxtGuideStarXPMF.Text)
                                    .GuideStarYBMF = Convert.ToInt32(TxtGuideStarYBMF.Text)
                                    .GuideStarYPMF = Convert.ToInt32(TxtGuideStarYPMF.Text)
                                    .ErrorObservingTarget = ChkErrorObservingTarget.Checked
                                    .ErrorTextTarget = TxtErrorTextTarget.Text
                                    .TargetRemarks = TxtTargetRemarks.Text
                                    .TargetLastObservedDate = TxtTargetLastObservedDate.Text
                                    .TargetMosaic = ChkTargetMosaic.Checked
                                    .TargetMosaicType = CmbTargetMosaicType.Text
                                    .TargetMosaicFramesPerPanel = Convert.ToInt32(TxtTargetMosaicFramesPerPanel.Text)
                                    .TargetPanel1RA2000HH = TxtTargetPanel1RA2000HH.Text
                                    .TargetPanel2RA2000HH = TxtTargetPanel2RA2000HH.Text
                                    .TargetPanel3RA2000HH = TxtTargetPanel3RA2000HH.Text
                                    .TargetPanel4RA2000HH = TxtTargetPanel4RA2000HH.Text
                                    .TargetPanel1RA2000MM = TxtTargetPanel1RA2000MM.Text
                                    .TargetPanel2RA2000MM = TxtTargetPanel2RA2000MM.Text
                                    .TargetPanel3RA2000MM = TxtTargetPanel3RA2000MM.Text
                                    .TargetPanel4RA2000MM = TxtTargetPanel4RA2000MM.Text
                                    .TargetPanel1RA2000SS = TxtTargetPanel1RA2000SS.Text
                                    .TargetPanel2RA2000SS = TxtTargetPanel2RA2000SS.Text
                                    .TargetPanel3RA2000SS = TxtTargetPanel3RA2000SS.Text
                                    .TargetPanel4RA2000SS = TxtTargetPanel4RA2000SS.Text
                                    .TargetPanel1DEC2000DG = TxtTargetPanel1DEC2000DG.Text
                                    .TargetPanel2DEC2000DG = TxtTargetPanel2DEC2000DG.Text
                                    .TargetPanel3DEC2000DG = TxtTargetPanel3DEC2000DG.Text
                                    .TargetPanel4DEC2000DG = TxtTargetPanel4DEC2000DG.Text
                                    .TargetPanel1DEC2000MM = TxtTargetPanel1DEC2000MM.Text
                                    .TargetPanel2DEC2000MM = TxtTargetPanel2DEC2000MM.Text
                                    .TargetPanel3DEC2000MM = TxtTargetPanel3DEC2000MM.Text
                                    .TargetPanel4DEC2000MM = TxtTargetPanel4DEC2000MM.Text
                                    .TargetPanel1DEC2000SS = TxtTargetPanel1DEC2000SS.Text
                                    .TargetPanel2DEC2000SS = TxtTargetPanel2DEC2000SS.Text
                                    .TargetPanel3DEC2000SS = TxtTargetPanel3DEC2000SS.Text
                                    .TargetPanel4DEC2000SS = TxtTargetPanel4DEC2000SS.Text
                                    .TargetPanel1NbrExposedFrames = Convert.ToInt32(TxtTargetPanel1NbrExposedFrames.Text)
                                    .TargetPanel2NbrExposedFrames = Convert.ToInt32(TxtTargetPanel2NbrExposedFrames.Text)
                                    .TargetPanel3NbrExposedFrames = Convert.ToInt32(TxtTargetPanel3NbrExposedFrames.Text)
                                    .TargetPanel4NbrExposedFrames = Convert.ToInt32(TxtTargetPanel4NbrExposedFrames.Text)
                                    .TargetMosaicOverlap = Convert.ToInt32(TxtTargetMosaicOverlap.Text)
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

        Catch ex As Exception
            ShowMessage("LoadRecord " + ex.Message, "CRITICAL", "Error!")
            LoadRecord = "NOK"
        End Try
    End Function

    Public Function ClearDetailRecord() As String

        ClearDetailRecord = "OK"
        Try
            'clear the databindings
            TxtTargetName.DataBindings.Clear()
            TxtID.DataBindings.Clear()
            TxtTargetRA2000HH.DataBindings.Clear()
            TxtTargetRA2000MM.DataBindings.Clear()
            TxtTargetRA2000SS.DataBindings.Clear()
            TxtTargetDEC2000DG.DataBindings.Clear()
            TxtTargetDEC2000MM.DataBindings.Clear()
            TxtTargetDEC2000SS.DataBindings.Clear()
            TxtTargetExposure.DataBindings.Clear()
            ComboBoxTargetBinning.DataBindings.Clear()
            ComboBoxTargetFilter.DataBindings.Clear()
            ChkTargetDone.DataBindings.Clear()
            ChkPriorityTarget.DataBindings.Clear()
            ChkTargetIsComet.DataBindings.Clear()
            ChkTargetIgnoreMoon.DataBindings.Clear()
            TxtTargetNbrExposedFrames.DataBindings.Clear()
            TxtTargetNbrFrames.DataBindings.Clear()
            TxtFocusStarName.DataBindings.Clear()
            TxtFocusStarRA2000HH.DataBindings.Clear()
            TxtFocusStarRA2000MM.DataBindings.Clear()
            TxtFocusStarRA2000SS.DataBindings.Clear()
            TxtFocusStarDEC2000DG.DataBindings.Clear()
            TxtFocusStarDEC2000MM.DataBindings.Clear()
            TxtFocusStarDEC2000SS.DataBindings.Clear()
            TxtFocusStarExposure.DataBindings.Clear()
            ChkGuideAuto.DataBindings.Clear()
            TxtGuideStarExposure.DataBindings.Clear()
            TxtGuideStarXBMF.DataBindings.Clear()
            TxtGuideStarXPMF.DataBindings.Clear()
            TxtGuideStarYBMF.DataBindings.Clear()
            TxtGuideStarYPMF.DataBindings.Clear()
            ChkErrorObservingTarget.DataBindings.Clear()
            TxtErrorTextTarget.DataBindings.Clear()
            TxtTargetRemarks.DataBindings.Clear()
            TxtTargetLastObservedDate.DataBindings.Clear()
            ChkTargetMosaic.DataBindings.Clear()
            CmbTargetMosaicType.DataBindings.Clear()
            TxtTargetMosaicFramesPerPanel.DataBindings.Clear()
            TxtTargetPanel1RA2000HH.DataBindings.Clear()
            TxtTargetPanel2RA2000HH.DataBindings.Clear()
            TxtTargetPanel3RA2000HH.DataBindings.Clear()
            TxtTargetPanel4RA2000HH.DataBindings.Clear()
            TxtTargetPanel1RA2000MM.DataBindings.Clear()
            TxtTargetPanel2RA2000MM.DataBindings.Clear()
            TxtTargetPanel3RA2000MM.DataBindings.Clear()
            TxtTargetPanel4RA2000MM.DataBindings.Clear()
            TxtTargetPanel1RA2000SS.DataBindings.Clear()
            TxtTargetPanel2RA2000SS.DataBindings.Clear()
            TxtTargetPanel3RA2000SS.DataBindings.Clear()
            TxtTargetPanel4RA2000SS.DataBindings.Clear()
            TxtTargetPanel1DEC2000DG.DataBindings.Clear()
            TxtTargetPanel2DEC2000DG.DataBindings.Clear()
            TxtTargetPanel3DEC2000DG.DataBindings.Clear()
            TxtTargetPanel4DEC2000DG.DataBindings.Clear()
            TxtTargetPanel1DEC2000MM.DataBindings.Clear()
            TxtTargetPanel2DEC2000MM.DataBindings.Clear()
            TxtTargetPanel3DEC2000MM.DataBindings.Clear()
            TxtTargetPanel4DEC2000MM.DataBindings.Clear()
            TxtTargetPanel1DEC2000SS.DataBindings.Clear()
            TxtTargetPanel2DEC2000SS.DataBindings.Clear()
            TxtTargetPanel3DEC2000SS.DataBindings.Clear()
            TxtTargetPanel4DEC2000SS.DataBindings.Clear()
            TxtTargetPanel1NbrExposedFrames.DataBindings.Clear()
            TxtTargetPanel2NbrExposedFrames.DataBindings.Clear()
            TxtTargetPanel3NbrExposedFrames.DataBindings.Clear()
            TxtTargetPanel4NbrExposedFrames.DataBindings.Clear()
            TxtTargetMosaicOverlap.DataBindings.Clear()

            'clear the textfields
            TxtTargetName.Text = Nothing
            TxtID.Text = Nothing
            TxtTargetRA2000HH.Text = Nothing
            TxtTargetRA2000MM.Text = Nothing
            TxtTargetRA2000SS.Text = Nothing
            TxtTargetDEC2000DG.Text = Nothing
            TxtTargetDEC2000MM.Text = Nothing
            TxtTargetDEC2000SS.Text = Nothing
            TxtTargetExposure.Text = Nothing
            ComboBoxTargetBinning.Text = Nothing
            ComboBoxTargetFilter.Text = Nothing
            ChkTargetDone.Checked = Nothing
            ChkPriorityTarget.Checked = Nothing
            ChkTargetIsComet.Checked = Nothing
            ChkTargetIgnoreMoon.Checked = Nothing
            TxtTargetNbrExposedFrames.Text = Nothing
            TxtTargetNbrFrames.Text = Nothing
            TxtFocusStarName.Text = Nothing
            TxtFocusStarRA2000HH.Text = Nothing
            TxtFocusStarRA2000MM.Text = Nothing
            TxtFocusStarRA2000SS.Text = Nothing
            TxtFocusStarDEC2000DG.Text = Nothing
            TxtFocusStarDEC2000MM.Text = Nothing
            TxtFocusStarDEC2000SS.Text = Nothing
            TxtFocusStarExposure.Text = Nothing
            ChkGuideAuto.Checked = Nothing
            TxtGuideStarExposure.Text = Nothing
            TxtGuideStarXBMF.Text = Nothing
            TxtGuideStarXPMF.Text = Nothing
            TxtGuideStarYBMF.Text = Nothing
            TxtGuideStarYPMF.Text = Nothing
            ChkErrorObservingTarget.Checked = Nothing
            TxtErrorTextTarget.Text = Nothing
            TxtTargetRemarks.Text = Nothing
            TxtTargetLastObservedDate.Text = Nothing
            ChkTargetMosaic.Checked = Nothing
            CmbTargetMosaicType.Text = Nothing
            TxtTargetMosaicFramesPerPanel.Text = Nothing
            TxtTargetPanel1RA2000HH.Text = Nothing
            TxtTargetPanel2RA2000HH.Text = Nothing
            TxtTargetPanel3RA2000HH.Text = Nothing
            TxtTargetPanel4RA2000HH.Text = Nothing
            TxtTargetPanel1RA2000MM.Text = Nothing
            TxtTargetPanel2RA2000MM.Text = Nothing
            TxtTargetPanel3RA2000MM.Text = Nothing
            TxtTargetPanel4RA2000MM.Text = Nothing
            TxtTargetPanel1RA2000SS.Text = Nothing
            TxtTargetPanel2RA2000SS.Text = Nothing
            TxtTargetPanel3RA2000SS.Text = Nothing
            TxtTargetPanel4RA2000SS.Text = Nothing
            TxtTargetPanel1DEC2000DG.Text = Nothing
            TxtTargetPanel2DEC2000DG.Text = Nothing
            TxtTargetPanel3DEC2000DG.Text = Nothing
            TxtTargetPanel4DEC2000DG.Text = Nothing
            TxtTargetPanel1DEC2000MM.Text = Nothing
            TxtTargetPanel2DEC2000MM.Text = Nothing
            TxtTargetPanel3DEC2000MM.Text = Nothing
            TxtTargetPanel4DEC2000MM.Text = Nothing
            TxtTargetPanel1DEC2000SS.Text = Nothing
            TxtTargetPanel2DEC2000SS.Text = Nothing
            TxtTargetPanel3DEC2000SS.Text = Nothing
            TxtTargetPanel4DEC2000SS.Text = Nothing
            TxtTargetPanel1NbrExposedFrames.Text = Nothing
            TxtTargetPanel2NbrExposedFrames.Text = Nothing
            TxtTargetPanel3NbrExposedFrames.Text = Nothing
            TxtTargetPanel4NbrExposedFrames.Text = Nothing
            TxtTargetMosaicOverlap.Text = Nothing

            'clear structure
            With pStrucSLCurrentRecord
                .TargetName = Nothing
                .ID = Nothing
                .TargetRA2000HH = Nothing
                .TargetRA2000MM = Nothing
                .TargetRA2000SS = Nothing
                .TargetDEC2000DG = Nothing
                .TargetDEC2000MM = Nothing
                .TargetDEC2000SS = Nothing
                .TargetExposure = Nothing
                .TargetBin = Nothing
                .TargetFilter = Nothing
                .TargetDone = Nothing
                .TargetNbrExposedFrames = Nothing
                .TargetNbrFrames = Nothing
                .TargetPriority = Nothing
                .TargetIsComet = Nothing
                .TargetIgnoreMoon = Nothing
                .FocusStarName = Nothing
                .FocusStarRA2000HH = Nothing
                .FocusStarRA2000MM = Nothing
                .FocusStarRA2000SS = Nothing
                .FocusStarDEC2000DG = Nothing
                .FocusStarDEC2000MM = Nothing
                .FocusStarDEC2000SS = Nothing
                .FocusStarExposure = Nothing
                .GuideAuto = Nothing
                .GuideStarExposure = Nothing
                .GuideStarXBMF = Nothing
                .GuideStarXPMF = Nothing
                .GuideStarYBMF = Nothing
                .GuideStarYPMF = Nothing
                .ErrorObservingTarget = Nothing
                .ErrorObservingTarget = Nothing
                .ErrorTextTarget = Nothing
                .TargetRemarks = Nothing
                .TargetLastObservedDate = Nothing
                .TargetMosaic = Nothing
                .TargetMosaicType = Nothing
                .TargetMosaicFramesPerPanel = Nothing
                .TargetPanel1RA2000HH = Nothing
                .TargetPanel2RA2000HH = Nothing
                .TargetPanel3RA2000HH = Nothing
                .TargetPanel4RA2000HH = Nothing
                .TargetPanel1RA2000MM = Nothing
                .TargetPanel2RA2000MM = Nothing
                .TargetPanel3RA2000MM = Nothing
                .TargetPanel4RA2000MM = Nothing
                .TargetPanel1RA2000SS = Nothing
                .TargetPanel2RA2000SS = Nothing
                .TargetPanel3RA2000SS = Nothing
                .TargetPanel4RA2000SS = Nothing
                .TargetPanel1DEC2000DG = Nothing
                .TargetPanel2DEC2000DG = Nothing
                .TargetPanel3DEC2000DG = Nothing
                .TargetPanel4DEC2000DG = Nothing
                .TargetPanel1DEC2000MM = Nothing
                .TargetPanel2DEC2000MM = Nothing
                .TargetPanel3DEC2000MM = Nothing
                .TargetPanel4DEC2000MM = Nothing
                .TargetPanel1DEC2000SS = Nothing
                .TargetPanel2DEC2000SS = Nothing
                .TargetPanel3DEC2000SS = Nothing
                .TargetPanel4DEC2000SS = Nothing
                .TargetPanel1NbrExposedFrames = Nothing
                .TargetPanel2NbrExposedFrames = Nothing
                .TargetPanel3NbrExposedFrames = Nothing
                .TargetPanel4NbrExposedFrames = Nothing
                .TargetMosaicOverlap = Nothing
            End With
        Catch ex As Exception
            ShowMessage("ClearDetailRecord " + ex.Message, "CRITICAL", "Error!")
            ClearDetailRecord = "NOK"
        End Try
    End Function

    Private Sub BtnSave_Click(sender As Object, e As EventArgs) Handles BtnSave.Click
        InsertUpdateRecord()
    End Sub
    Private Sub DataGridViewTarget_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridViewTarget.CellClick
        Dim TargetName, TargetFilter As String
        Dim returnvalue As String
        Dim cmd As New OleDb.OleDbCommand

        Try
            '-------------------------------------------------------------------------------------------------------------
            'check for changes
            '-------------------------------------------------------------------------------------------------------------
            InsertUpdateRecord()

            '-------------------------------------------------------------------------------------------------------------
            'change the row
            '-------------------------------------------------------------------------------------------------------------
            TargetName = DataGridViewTarget.CurrentRow.Cells(0).Value.ToString
            TargetFilter = DataGridViewTarget.CurrentRow.Cells(1).Value.ToString


            returnvalue = LoadRecord(TargetName, TargetFilter)
            If returnvalue <> "OK" Then
                Exit Sub
            End If

        Catch ex As Exception
            ShowMessage("DataGridViewTarget_CellClick: " + ex.Message, "CRITICAL", "Error!")
        End Try
    End Sub

    Private Sub BtnDelete_Click(sender As Object, e As EventArgs) Handles BtnDelete.Click
        Dim returnvalue As String
        Try

            returnvalue = DeleteRecord()
            If returnvalue <> "OK" Then
                Exit Sub
            End If

        Catch ex As Exception
            ShowMessage("BtnDelete_Click: " + ex.Message, "CRITICAL", "Error!")
        End Try

    End Sub
    Private Sub BtnNew_Click(sender As Object, e As EventArgs) Handles BtnNew.Click
        Dim returnvalue As String
        Try

            returnvalue = ClearDetailRecord()
            If returnvalue <> "OK" Then
                Exit Sub
            End If

            'add row to datagrid
            dtSL_master.Rows.Add()
            DataGridViewTarget.Refresh()
            If DataGridViewTarget.Rows.Count > 0 Then
                Dim i = DataGridViewTarget.Rows.Count - 1
                DataGridViewTarget.Rows(i).Cells(0).Selected = True
            End If
            DataGridViewTarget.CurrentRow.Cells(0).Selected = True

            'set default values
            TxtTargetName.Text = "New target"
            TxtTargetRA2000HH.Text = "0"
            TxtTargetRA2000MM.Text = "0"
            TxtTargetRA2000SS.Text = "0"
            TxtTargetDEC2000DG.Text = "0"
            TxtTargetDEC2000MM.Text = "0"
            TxtTargetDEC2000SS.Text = "0"
            TxtTargetExposure.Text = "180"
            ComboBoxTargetBinning.Text = "1x1"
            ComboBoxTargetFilter.Text = My.Settings.sCCDFilter1
            ChkTargetDone.Checked = False
            ChkPriorityTarget.Checked = False
            ChkTargetIsComet.Checked = False
            ChkTargetIgnoreMoon.Checked = False
            TxtTargetNbrExposedFrames.Text = "0"
            TxtTargetNbrFrames.Text = "500"
            TxtFocusStarName.Text = ""
            TxtFocusStarRA2000HH.Text = "0"
            TxtFocusStarRA2000MM.Text = "0"
            TxtFocusStarRA2000SS.Text = "0"
            TxtFocusStarDEC2000DG.Text = "0"
            TxtFocusStarDEC2000MM.Text = "0"
            TxtFocusStarDEC2000SS.Text = "0"
            TxtFocusStarExposure.Text = My.Settings.sCCDFocusDefExp.ToString
            ChkGuideAuto.Checked = False
            TxtGuideStarExposure.Text = "2"
            TxtGuideStarXBMF.Text = "0"
            TxtGuideStarXPMF.Text = "0"
            TxtGuideStarYBMF.Text = "0"
            TxtGuideStarYPMF.Text = "0"
            TxtTargetRemarks.Text = ""
            TxtTargetLastObservedDate.Text = ""
            ChkTargetMosaic.Checked = False
            CmbTargetMosaicType.Text = ""
            TxtTargetMosaicFramesPerPanel.Text = "500"
            TxtTargetPanel1RA2000HH.Text = ""
            TxtTargetPanel2RA2000HH.Text = ""
            TxtTargetPanel3RA2000HH.Text = ""
            TxtTargetPanel4RA2000HH.Text = ""
            TxtTargetPanel1RA2000MM.Text = ""
            TxtTargetPanel2RA2000MM.Text = ""
            TxtTargetPanel3RA2000MM.Text = ""
            TxtTargetPanel4RA2000MM.Text = ""
            TxtTargetPanel1RA2000SS.Text = ""
            TxtTargetPanel2RA2000SS.Text = ""
            TxtTargetPanel3RA2000SS.Text = ""
            TxtTargetPanel4RA2000SS.Text = ""
            TxtTargetPanel1DEC2000DG.Text = ""
            TxtTargetPanel2DEC2000DG.Text = ""
            TxtTargetPanel3DEC2000DG.Text = ""
            TxtTargetPanel4DEC2000DG.Text = ""
            TxtTargetPanel1DEC2000MM.Text = ""
            TxtTargetPanel2DEC2000MM.Text = ""
            TxtTargetPanel3DEC2000MM.Text = ""
            TxtTargetPanel4DEC2000MM.Text = ""
            TxtTargetPanel1DEC2000SS.Text = ""
            TxtTargetPanel2DEC2000SS.Text = ""
            TxtTargetPanel3DEC2000SS.Text = ""
            TxtTargetPanel4DEC2000SS.Text = ""
            TxtTargetPanel1NbrExposedFrames.Text = "0"
            TxtTargetPanel2NbrExposedFrames.Text = "0"
            TxtTargetPanel3NbrExposedFrames.Text = "0"
            TxtTargetPanel4NbrExposedFrames.Text = "0"
            TxtTargetMosaicOverlap.Text = "25"
        Catch ex As Exception
            ShowMessage("BtnNew_Click: " + ex.Message, "CRITICAL", "Error!")
        End Try

    End Sub

    Public Function UpdateRecord() As String
        Dim returnvalue As String
        Dim UpdateString As String
        Dim i As Integer

        UpdateRecord = "OK"
        Try
            Dim ciClone As CultureInfo = CType(CultureInfo.InvariantCulture.Clone(), CultureInfo)
            ciClone.NumberFormat.NumberDecimalSeparator = "."

            UpdateString = ""
            'check for changes
            If pStrucSLCurrentRecord.TargetName <> TxtTargetName.Text Then
                UpdateString = "TargetName='" + TxtTargetName.Text + "'"
            End If

            If pStrucSLCurrentRecord.TargetRA2000HH <> TxtTargetRA2000HH.Text Then
                If UpdateString <> "" Then
                    UpdateString += ","
                End If
                UpdateString += "TargetRA2000HH='" + TxtTargetRA2000HH.Text + "'"
            End If

            If pStrucSLCurrentRecord.TargetRA2000MM <> TxtTargetRA2000MM.Text Then
                If UpdateString <> "" Then
                    UpdateString += ","
                End If
                UpdateString += "TargetRA2000MM='" + TxtTargetRA2000MM.Text + "'"
            End If

            If pStrucSLCurrentRecord.TargetRA2000SS <> TxtTargetRA2000SS.Text Then
                If UpdateString <> "" Then
                    UpdateString += ","
                End If
                UpdateString += "TargetRA2000SS='" + TxtTargetRA2000SS.Text + "'"
            End If

            If pStrucSLCurrentRecord.TargetDEC2000DG <> TxtTargetDEC2000DG.Text Then
                If UpdateString <> "" Then
                    UpdateString += ","
                End If
                UpdateString += "TargetDEC2000DG='" + TxtTargetDEC2000DG.Text + "'"
            End If

            If pStrucSLCurrentRecord.TargetDEC2000MM <> TxtTargetDEC2000MM.Text Then
                If UpdateString <> "" Then
                    UpdateString += ","
                End If
                UpdateString += "TargetDEC2000MM='" + TxtTargetDEC2000MM.Text + "'"
            End If

            If pStrucSLCurrentRecord.TargetDEC2000SS <> TxtTargetDEC2000SS.Text Then
                If UpdateString <> "" Then
                    UpdateString += ","
                End If
                UpdateString += "TargetDEC2000SS='" + TxtTargetDEC2000SS.Text + "'"
            End If

            If pStrucSLCurrentRecord.TargetExposure <> Double.Parse(TxtTargetExposure.Text, ciClone) Then
                If UpdateString <> "" Then
                    UpdateString += ","
                End If
                UpdateString += "TargetExposure=" + TxtTargetExposure.Text
            End If

            If pStrucSLCurrentRecord.TargetBin <> ComboBoxTargetBinning.Text Then
                If UpdateString <> "" Then
                    UpdateString += ","
                End If
                UpdateString += "TargetBin='" + ComboBoxTargetBinning.Text + "'"
            End If

            If pStrucSLCurrentRecord.TargetFilter <> ComboBoxTargetFilter.Text Then
                If UpdateString <> "" Then
                    UpdateString += ","
                End If
                UpdateString += "TargetFilter='" + ComboBoxTargetFilter.Text + "'"
            End If

            If pStrucSLCurrentRecord.TargetDone <> ChkTargetDone.Checked Then
                If UpdateString <> "" Then
                    UpdateString += ","
                End If
                If ChkTargetDone.Checked = True Then
                    UpdateString += "TargetDone=1"
                Else
                    UpdateString += "TargetDone=0"
                End If
            End If

            If pStrucSLCurrentRecord.TargetPriority <> ChkPriorityTarget.Checked Then
                If UpdateString <> "" Then
                    UpdateString += ","
                End If
                If ChkPriorityTarget.Checked = True Then
                    UpdateString += "TargetPriority=1"
                Else
                    UpdateString += "TargetPriority=0"
                End If
            End If

            If pStrucSLCurrentRecord.TargetIsComet <> ChkTargetIsComet.Checked Then
                If UpdateString <> "" Then
                    UpdateString += ","
                End If
                If ChkTargetIsComet.Checked = True Then
                    UpdateString += "TargetIsComet=1"
                Else
                    UpdateString += "TargetIsComet=0"
                End If
            End If

            If pStrucSLCurrentRecord.TargetIgnoreMoon <> ChkTargetIgnoreMoon.Checked Then
                If UpdateString <> "" Then
                    UpdateString += ","
                End If
                If ChkTargetIgnoreMoon.Checked = True Then
                    UpdateString += "TargetIgnoreMoon=1"
                Else
                    UpdateString += "TargetIgnoreMoon=0"
                End If
            End If

            If pStrucSLCurrentRecord.TargetNbrExposedFrames <> Convert.ToInt32(TxtTargetNbrExposedFrames.Text) Then
                If UpdateString <> "" Then
                    UpdateString += ","
                End If
                UpdateString += "TargetNbrExposedFrames=" + TxtTargetNbrExposedFrames.Text
            End If

            If pStrucSLCurrentRecord.TargetNbrFrames <> Convert.ToInt32(TxtTargetNbrFrames.Text) Then
                If UpdateString <> "" Then
                    UpdateString += ","
                End If
                UpdateString += "TargetNbrFrames=" + TxtTargetNbrFrames.Text
            End If

            If pStrucSLCurrentRecord.FocusStarName <> TxtFocusStarName.Text Then
                If UpdateString <> "" Then
                    UpdateString += ","
                End If
                UpdateString += "FocusStarName='" + TxtFocusStarName.Text + "'"
            End If

            If pStrucSLCurrentRecord.FocusStarRA2000HH <> TxtFocusStarRA2000HH.Text Then
                If UpdateString <> "" Then
                    UpdateString += ","
                End If
                UpdateString += "FocusStarRA2000HH='" + TxtFocusStarRA2000HH.Text + "'"
            End If

            If pStrucSLCurrentRecord.FocusStarRA2000MM <> TxtFocusStarRA2000MM.Text Then
                If UpdateString <> "" Then
                    UpdateString += ","
                End If
                UpdateString += "FocusStarRA2000MM='" + TxtFocusStarRA2000MM.Text + "'"
            End If

            If pStrucSLCurrentRecord.FocusStarRA2000SS <> TxtFocusStarRA2000SS.Text Then
                If UpdateString <> "" Then
                    UpdateString += ","
                End If
                UpdateString += "FocusStarRA2000SS='" + TxtFocusStarRA2000SS.Text + "'"
            End If

            If pStrucSLCurrentRecord.FocusStarDEC2000DG <> TxtFocusStarDEC2000DG.Text Then
                If UpdateString <> "" Then
                    UpdateString += ","
                End If
                UpdateString += "FocusStarDEC2000DG='" + TxtFocusStarDEC2000DG.Text + "'"
            End If

            If pStrucSLCurrentRecord.FocusStarDEC2000MM <> TxtFocusStarDEC2000MM.Text Then
                If UpdateString <> "" Then
                    UpdateString += ","
                End If
                UpdateString += "FocusStarDEC2000MM='" + TxtFocusStarDEC2000MM.Text + "'"
            End If

            If pStrucSLCurrentRecord.FocusStarDEC2000SS <> TxtFocusStarDEC2000SS.Text Then
                If UpdateString <> "" Then
                    UpdateString += ","
                End If
                UpdateString += "FocusStarDEC2000SS='" + TxtFocusStarDEC2000SS.Text + "'"
            End If

            If pStrucSLCurrentRecord.FocusStarExposure <> Double.Parse(TxtFocusStarExposure.Text, ciClone) Then
                If UpdateString <> "" Then
                    UpdateString += ","
                End If
                UpdateString += "FocusStarExposure=" + TxtFocusStarExposure.Text
            End If

            If pStrucSLCurrentRecord.GuideAuto <> ChkGuideAuto.Checked Then
                If UpdateString <> "" Then
                    UpdateString += ","
                End If
                If ChkTargetDone.Checked = True Then
                    UpdateString += "GuideAuto=1"
                Else
                    UpdateString += "GuideAuto=0"
                End If
            End If

            If pStrucSLCurrentRecord.GuideStarXBMF <> Convert.ToInt32(TxtGuideStarXBMF.Text) Then
                If UpdateString <> "" Then
                    UpdateString += ","
                End If
                UpdateString += "GuideStarXBMF=" + TxtGuideStarXBMF.Text
            End If

            If pStrucSLCurrentRecord.GuideStarYBMF <> Convert.ToInt32(TxtGuideStarYBMF.Text) Then
                If UpdateString <> "" Then
                    UpdateString += ","
                End If
                UpdateString += "GuideStaryYBMF=" + TxtGuideStarYBMF.Text
            End If

            If pStrucSLCurrentRecord.GuideStarXPMF <> Convert.ToInt32(TxtGuideStarXPMF.Text) Then
                If UpdateString <> "" Then
                    UpdateString += ","
                End If
                UpdateString += "GuideStarXPMF=" + TxtGuideStarXPMF.Text
            End If

            If pStrucSLCurrentRecord.GuideStarYPMF <> Convert.ToInt32(TxtGuideStarYPMF.Text) Then
                If UpdateString <> "" Then
                    UpdateString += ","
                End If
                UpdateString += "GuideStarYPMF=" + TxtGuideStarYPMF.Text
            End If

            If pStrucSLCurrentRecord.GuideStarExposure <> Convert.ToInt32(TxtGuideStarExposure.Text) Then
                If UpdateString <> "" Then
                    UpdateString += ","
                End If
                UpdateString += "GuideExposure=" + TxtGuideStarExposure.Text
            End If

            If pStrucSLCurrentRecord.ErrorObservingTarget <> ChkErrorObservingTarget.Checked Then
                If UpdateString <> "" Then
                    UpdateString += ","
                End If
                If ChkErrorObservingTarget.Checked = True Then
                    UpdateString += "TargetError=1"
                Else
                    UpdateString += "TargetError=0"
                End If
            End If

            If pStrucSLCurrentRecord.ErrorTextTarget <> TxtErrorTextTarget.Text Then
                If UpdateString <> "" Then
                    UpdateString += ","
                End If
                UpdateString += "TargetErrorText='" + TxtErrorTextTarget.Text + "'"
            End If

            If pStrucSLCurrentRecord.TargetRemarks <> TxtTargetRemarks.Text Then
                If UpdateString <> "" Then
                    UpdateString += ","
                End If
                UpdateString += "TargetRemarks='" + TxtTargetRemarks.Text + "'"
            End If

            If pStrucSLCurrentRecord.TargetLastObservedDate <> TxtTargetLastObservedDate.Text Then
                If UpdateString <> "" Then
                    UpdateString += ","
                End If
                UpdateString += "TargetLastObservedDate='" + TxtTargetLastObservedDate.Text + "'"
            End If

            If pStrucSLCurrentRecord.TargetMosaic <> ChkTargetMosaic.Checked Then
                If UpdateString <> "" Then
                    UpdateString += ","
                End If

                If ChkTargetMosaic.Checked = True Then
                    UpdateString += "TargetMosaic=1"
                Else
                    UpdateString += "TargetMosaic=0"
                End If
            End If

            If pStrucSLCurrentRecord.TargetMosaicType <> CmbTargetMosaicType.Text Then
                If UpdateString <> "" Then
                    UpdateString += ","
                End If
                UpdateString += "TargetMosaicType='" + CmbTargetMosaicType.Text + "'"
            End If

            If pStrucSLCurrentRecord.TargetMosaicFramesPerPanel <> Convert.ToInt32(TxtTargetMosaicFramesPerPanel.Text) Then
                If UpdateString <> "" Then
                    UpdateString += ","
                End If
                UpdateString += "TargetMosaicFramesPerPanel='" + TxtTargetMosaicFramesPerPanel.Text + "'"
            End If

            If pStrucSLCurrentRecord.TargetPanel1RA2000HH <> TxtTargetPanel1RA2000HH.Text Then
                If UpdateString <> "" Then
                    UpdateString += ","
                End If
                UpdateString += "TargetPanel1RA2000HH='" + TxtTargetPanel1RA2000HH.Text + "'"
            End If

            If pStrucSLCurrentRecord.TargetPanel2RA2000HH <> TxtTargetPanel2RA2000HH.Text Then
                If UpdateString <> "" Then
                    UpdateString += ","
                End If
                UpdateString += "TargetPanel2RA2000HH='" + TxtTargetPanel2RA2000HH.Text + "'"
            End If

            If pStrucSLCurrentRecord.TargetPanel3RA2000HH <> TxtTargetPanel3RA2000HH.Text Then
                If UpdateString <> "" Then
                    UpdateString += ","
                End If
                UpdateString += "TargetPanel3RA2000HH='" + TxtTargetPanel3RA2000HH.Text + "'"
            End If

            If pStrucSLCurrentRecord.TargetPanel4RA2000HH <> TxtTargetPanel4RA2000HH.Text Then
                If UpdateString <> "" Then
                    UpdateString += ","
                End If
                UpdateString += "TargetPanel4RA2000HH='" + TxtTargetPanel4RA2000HH.Text + "'"
            End If

            If pStrucSLCurrentRecord.TargetPanel1RA2000MM <> TxtTargetPanel1RA2000MM.Text Then
                If UpdateString <> "" Then
                    UpdateString += ","
                End If
                UpdateString += "TargetPanel1RA2000MM='" + TxtTargetPanel1RA2000MM.Text + "'"
            End If

            If pStrucSLCurrentRecord.TargetPanel2RA2000MM <> TxtTargetPanel2RA2000MM.Text Then
                If UpdateString <> "" Then
                    UpdateString += ","
                End If
                UpdateString += "TargetPanel2RA2000MM='" + TxtTargetPanel2RA2000MM.Text + "'"
            End If

            If pStrucSLCurrentRecord.TargetPanel3RA2000MM <> TxtTargetPanel3RA2000MM.Text Then
                If UpdateString <> "" Then
                    UpdateString += ","
                End If
                UpdateString += "TargetPanel3RA2000MM='" + TxtTargetPanel3RA2000MM.Text + "'"
            End If

            If pStrucSLCurrentRecord.TargetPanel4RA2000MM <> TxtTargetPanel4RA2000MM.Text Then
                If UpdateString <> "" Then
                    UpdateString += ","
                End If
                UpdateString += "TargetPanel4RA2000MM='" + TxtTargetPanel4RA2000MM.Text + "'"
            End If

            If pStrucSLCurrentRecord.TargetPanel1RA2000SS <> TxtTargetPanel1RA2000SS.Text Then
                If UpdateString <> "" Then
                    UpdateString += ","
                End If
                UpdateString += "TargetPanel1RA2000SS='" + TxtTargetPanel1RA2000SS.Text + "'"
            End If

            If pStrucSLCurrentRecord.TargetPanel2RA2000SS <> TxtTargetPanel2RA2000SS.Text Then
                If UpdateString <> "" Then
                    UpdateString += ","
                End If
                UpdateString += "TargetPanel2RA2000SS='" + TxtTargetPanel2RA2000SS.Text + "'"
            End If

            If pStrucSLCurrentRecord.TargetPanel3RA2000SS <> TxtTargetPanel3RA2000SS.Text Then
                If UpdateString <> "" Then
                    UpdateString += ","
                End If
                UpdateString += "TargetPanel3RA2000SS='" + TxtTargetPanel3RA2000SS.Text + "'"
            End If

            If pStrucSLCurrentRecord.TargetPanel4RA2000SS <> TxtTargetPanel4RA2000SS.Text Then
                If UpdateString <> "" Then
                    UpdateString += ","
                End If
                UpdateString += "TargetPanel4RA2000SS='" + TxtTargetPanel4RA2000SS.Text + "'"
            End If

            If pStrucSLCurrentRecord.TargetPanel1DEC2000DG <> TxtTargetPanel1DEC2000DG.Text Then
                If UpdateString <> "" Then
                    UpdateString += ","
                End If
                UpdateString += "TargetPanel1DEC2000DG='" + TxtTargetPanel1DEC2000DG.Text + "'"
            End If

            If pStrucSLCurrentRecord.TargetPanel2DEC2000DG <> TxtTargetPanel2DEC2000DG.Text Then
                If UpdateString <> "" Then
                    UpdateString += ","
                End If
                UpdateString += "TargetPanel2DEC2000DG='" + TxtTargetPanel2DEC2000DG.Text + "'"
            End If

            If pStrucSLCurrentRecord.TargetPanel3DEC2000DG <> TxtTargetPanel3DEC2000DG.Text Then
                If UpdateString <> "" Then
                    UpdateString += ","
                End If
                UpdateString += "TargetPanel3DEC2000DG='" + TxtTargetPanel3DEC2000DG.Text + "'"
            End If

            If pStrucSLCurrentRecord.TargetPanel4DEC2000DG <> TxtTargetPanel4DEC2000DG.Text Then
                If UpdateString <> "" Then
                    UpdateString += ","
                End If
                UpdateString += "TargetPanel4DEC2000DG='" + TxtTargetPanel4DEC2000DG.Text + "'"
            End If

            If pStrucSLCurrentRecord.TargetPanel1DEC2000MM <> TxtTargetPanel1DEC2000MM.Text Then
                If UpdateString <> "" Then
                    UpdateString += ","
                End If
                UpdateString += "TargetPanel1DEC2000MM='" + TxtTargetPanel1DEC2000MM.Text + "'"
            End If

            If pStrucSLCurrentRecord.TargetPanel2DEC2000MM <> TxtTargetPanel2DEC2000MM.Text Then
                If UpdateString <> "" Then
                    UpdateString += ","
                End If
                UpdateString += "TargetPanel2DEC2000MM='" + TxtTargetPanel2DEC2000MM.Text + "'"
            End If

            If pStrucSLCurrentRecord.TargetPanel3DEC2000MM <> TxtTargetPanel3DEC2000MM.Text Then
                If UpdateString <> "" Then
                    UpdateString += ","
                End If
                UpdateString += "TargetPanel3DEC2000MM='" + TxtTargetPanel3DEC2000MM.Text + "'"
            End If

            If pStrucSLCurrentRecord.TargetPanel4DEC2000MM <> TxtTargetPanel4DEC2000MM.Text Then
                If UpdateString <> "" Then
                    UpdateString += ","
                End If
                UpdateString += "TargetPanel4DEC2000MM='" + TxtTargetPanel4DEC2000MM.Text + "'"
            End If

            If pStrucSLCurrentRecord.TargetPanel1DEC2000SS <> TxtTargetPanel1DEC2000SS.Text Then
                If UpdateString <> "" Then
                    UpdateString += ","
                End If
                UpdateString += "TargetPanel1DEC2000SS='" + TxtTargetPanel1DEC2000SS.Text + "'"
            End If

            If pStrucSLCurrentRecord.TargetPanel2DEC2000SS <> TxtTargetPanel2DEC2000SS.Text Then
                If UpdateString <> "" Then
                    UpdateString += ","
                End If
                UpdateString += "TargetPanel2DEC2000SS='" + TxtTargetPanel2DEC2000SS.Text + "'"
            End If

            If pStrucSLCurrentRecord.TargetPanel3DEC2000SS <> TxtTargetPanel3DEC2000SS.Text Then
                If UpdateString <> "" Then
                    UpdateString += ","
                End If
                UpdateString += "TargetPanel3DEC2000SS='" + TxtTargetPanel3DEC2000SS.Text + "'"
            End If

            If pStrucSLCurrentRecord.TargetPanel4DEC2000SS <> TxtTargetPanel4DEC2000SS.Text Then
                If UpdateString <> "" Then
                    UpdateString += ","
                End If
                UpdateString += "TargetPanel4DEC2000SS='" + TxtTargetPanel4DEC2000SS.Text + "'"
            End If

            If pStrucSLCurrentRecord.TargetPanel1NbrExposedFrames <> Convert.ToInt32(TxtTargetPanel1NbrExposedFrames.Text) Then
                If UpdateString <> "" Then
                    UpdateString += ","
                End If
                UpdateString += "TargetPanel1NbrExposedFrames='" + TxtTargetPanel1NbrExposedFrames.Text + "'"
            End If

            If pStrucSLCurrentRecord.TargetPanel2NbrExposedFrames <> Convert.ToInt32(TxtTargetPanel2NbrExposedFrames.Text) Then
                If UpdateString <> "" Then
                    UpdateString += ","
                End If
                UpdateString += "TargetPanel2NbrExposedFrames='" + TxtTargetPanel2NbrExposedFrames.Text + "'"
            End If

            If pStrucSLCurrentRecord.TargetPanel3NbrExposedFrames <> Convert.ToInt32(TxtTargetPanel3NbrExposedFrames.Text) Then
                If UpdateString <> "" Then
                    UpdateString += ","
                End If
                UpdateString += "TargetPanel3NbrExposedFrames='" + TxtTargetPanel3NbrExposedFrames.Text + "'"
            End If

            If pStrucSLCurrentRecord.TargetPanel4NbrExposedFrames <> Convert.ToInt32(TxtTargetPanel4NbrExposedFrames.Text) Then
                If UpdateString <> "" Then
                    UpdateString += ","
                End If
                UpdateString += "TargetPanel4NbrExposedFrames='" + TxtTargetPanel4NbrExposedFrames.Text + "'"
            End If

            If pStrucSLCurrentRecord.TargetMosaicOverlap <> Convert.ToInt32(TxtTargetMosaicOverlap.Text) Then
                If UpdateString <> "" Then
                    UpdateString += ","
                End If
                UpdateString += "TargetMosaicOverlap='" + TxtTargetMosaicOverlap.Text + "'"
            End If

            If UpdateString <> "" Then
                If ShowMessage("Do you want to update the record?", "YESNO", "Update record?") = vbYes Then
                    Using pCon As New SQLiteConnection("Data Source=RoboticObservatory.db;Version=3;")
                        Using cmd As SQLiteCommand = pCon.CreateCommand
                            Using da As New SQLiteDataAdapter
                                Try
                                    pCon.Open()
                                    da.SelectCommand = cmd
                                    cmd.CommandText = "UPDATE Target SET " + UpdateString + " WHERE Id =" + TxtID.Text
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
                    returnvalue = LoadDataGrid()
                    If returnvalue <> "OK" Then
                        UpdateRecord = "NOK"
                        Exit Function
                    End If

                End If
            End If


        Catch ex As Exception
            ShowMessage("UpdateRecord: " + ex.Message, "CRITICAL", "Error!")
            UpdateRecord = "NOK"
        End Try
    End Function

    Public Function InsertRecord() As String
        Dim returnvalue As String
        'Dim sql As String
        'Dim cmd As New OleDb.OleDbCommand
        Dim i As Integer

        InsertRecord = "OK"
        Try
            If ShowMessage("Do you want to insert the record?", "YESNO", "Insert record ?") = vbYes Then
                'check if all necessary fields are filled
                If TxtTargetName.Text = "" Or
                   TxtTargetRA2000HH.Text = "" Or
                   TxtTargetRA2000MM.Text = "" Or
                   TxtTargetRA2000SS.Text = "" Or
                   TxtTargetDEC2000DG.Text = "" Or
                   TxtTargetDEC2000MM.Text = "" Or
                   TxtTargetDEC2000SS.Text = "" Or
                   ComboBoxTargetBinning.Text = "" Or
                   ComboBoxTargetFilter.Text = "" Or
                   TxtTargetNbrFrames.Text = "" Then
                    InsertRecord = "All target fields need to be filled !"
                    Exit Function
                End If

                Dim CheckB, CheckA, CheckTargetPriority, CheckErrorObservingTarget, CheckTargetIsComet, CheckTargetIgnoreMoon, CheckMosaic As String
                If ChkTargetDone.Checked = True Then
                    CheckB = "1"
                Else
                    CheckB = "0"
                End If

                If ChkGuideAuto.Checked = True Then
                    CheckA = "1"
                Else
                    CheckA = "0"
                End If

                If ChkPriorityTarget.Checked = True Then
                    CheckTargetPriority = "1"
                Else
                    CheckTargetPriority = "0"
                End If

                If ChkTargetIsComet.Checked = True Then
                    CheckTargetIsComet = "1"
                Else
                    CheckTargetIsComet = "0"
                End If

                If ChkTargetIgnoreMoon.Checked = True Then
                    CheckTargetIgnoreMoon = "1"
                Else
                    CheckTargetIgnoreMoon = "0"
                End If

                If ChkErrorObservingTarget.Checked = True Then
                    CheckErrorObservingTarget = "1"
                Else
                    CheckErrorObservingTarget = "0"
                End If

                If ChkTargetMosaic.Checked = True Then
                    CheckMosaic = "1"
                Else
                    CheckMosaic = "0"
                End If


                Using pCon As New SQLiteConnection("Data Source=RoboticObservatory.db;Version=3;")
                    Using cmd As SQLiteCommand = pCon.CreateCommand
                        Using daSL As New SQLiteDataAdapter
                            Try
                                pCon.Open()
                                daSL.SelectCommand = cmd
                                cmd.CommandText = "INSERT into Target (TargetName, TargetRA2000HH, TargetRA2000MM, TargetRA2000SS," +
                                               "TargetDEC2000DG, TargetDEC2000MM, TargetDEC2000SS," +
                                               "TargetExposure, TargetBin, TargetFilter, TargetNbrFrames, TargetNbrExposedFrames, TargetPriority," +
                                               "TargetIsComet, TargetIgnoreMoon," +
                                               "TargetDone, FocusStarName, FocusStarRA2000HH, FocusStarRA2000MM, FocusStarRA2000SS," +
                                               "FocusStarDEC2000DG, FocusStarDEC2000MM, FocusStarDEC2000SS, FocusStarExposure," +
                                               "GuideExposure, GuideStarXBMF, GuideStarXPMF, GuideStarYBMF, GuideStarYPMF, GuideAuto, TargetError, TargetErrorText," +
                                               "TargetRemarks, TargetLastObservedDate, " +
                                               "TargetMosaic, TargetMosaicType, TargetMosaicFramesPerPanel, " +
                                               "TargetPanel1RA2000HH, TargetPanel2RA2000HH, TargetPanel3RA2000HH, TargetPanel4RA2000HH, " +
                                               "TargetPanel1RA2000MM, TargetPanel2RA2000MM, TargetPanel3RA2000MM, TargetPanel4RA2000MM, " +
                                               "TargetPanel1RA2000SS, TargetPanel2RA2000SS, TargetPanel3RA2000SS, TargetPanel4RA2000SS, " +
                                               "TargetPanel1DEC2000DG, TargetPanel2DEC2000DG, TargetPanel3DEC2000DG, TargetPanel4DEC2000DG, " +
                                               "TargetPanel1DEC2000MM, TargetPanel2DEC2000MM, TargetPanel3DEC2000MM, TargetPanel4DEC2000MM, " +
                                               "TargetPanel1DEC2000SS, TargetPanel2DEC2000SS, TargetPanel3DEC2000SS, TargetPanel4DEC2000SS, " +
                                               "TargetPanel1NbrExposedFrames, TargetPanel2NbrExposedFrames, TargetPanel3NbrExposedFrames, TargetPanel4NbrExposedFrames, TargetMosaicOverlap " +
                                               ") VALUES ('" _
                                                + TxtTargetName.Text + "','" + TxtTargetRA2000HH.Text + "','" + TxtTargetRA2000MM.Text + "','" + TxtTargetRA2000SS.Text + "','" _
                                                + TxtTargetDEC2000DG.Text + "','" + TxtTargetDEC2000MM.Text + "','" + TxtTargetDEC2000SS.Text + "','" _
                                                + TxtTargetExposure.Text + "','" + ComboBoxTargetBinning.Text + "','" + ComboBoxTargetFilter.Text + "','" + TxtTargetNbrFrames.Text + "','" _
                                                + TxtTargetNbrExposedFrames.Text + "','" + CheckTargetPriority + "','" + CheckTargetIsComet + "','" + CheckTargetIgnoreMoon + "','" _
                                                + CheckB + "','" + TxtFocusStarName.Text + "','" + TxtFocusStarRA2000HH.Text + "','" + TxtFocusStarRA2000MM.Text + "','" + TxtFocusStarRA2000SS.Text + "','" _
                                                + TxtFocusStarDEC2000DG.Text + "','" + TxtFocusStarDEC2000MM.Text + "','" + TxtFocusStarDEC2000SS.Text + "','" + TxtFocusStarExposure.Text + "','" _
                                                + TxtGuideStarExposure.Text + "','" + TxtGuideStarXBMF.Text + "','" + TxtGuideStarXPMF.Text + "','" + TxtGuideStarYBMF.Text + "','" + TxtGuideStarYPMF.Text + "','" _
                                                + CheckA + "','" + CheckErrorObservingTarget + "','" + TxtErrorTextTarget.Text + "','" + TxtTargetRemarks.Text + "','" + TxtTargetLastObservedDate.Text + "','" _
                                                + CheckMosaic + "','" + CmbTargetMosaicType.Text + "','" + TxtTargetMosaicFramesPerPanel.Text + "','" _
                                                + TxtTargetPanel1RA2000HH.Text + "','" + TxtTargetPanel2RA2000HH.Text + "','" + TxtTargetPanel3RA2000HH.Text + "','" + TxtTargetPanel4RA2000HH.Text + "','" _
                                                + TxtTargetPanel1RA2000MM.Text + "','" + TxtTargetPanel2RA2000MM.Text + "','" + TxtTargetPanel3RA2000MM.Text + "','" + TxtTargetPanel4RA2000MM.Text + "','" _
                                                + TxtTargetPanel1RA2000SS.Text + "','" + TxtTargetPanel2RA2000SS.Text + "','" + TxtTargetPanel3RA2000SS.Text + "','" + TxtTargetPanel4RA2000SS.Text + "','" _
                                                + TxtTargetPanel1DEC2000DG.Text + "','" + TxtTargetPanel2DEC2000DG.Text + "','" + TxtTargetPanel3DEC2000DG.Text + "','" + TxtTargetPanel4DEC2000DG.Text + "','" _
                                                + TxtTargetPanel1DEC2000MM.Text + "','" + TxtTargetPanel2DEC2000MM.Text + "','" + TxtTargetPanel3DEC2000MM.Text + "','" + TxtTargetPanel4DEC2000MM.Text + "','" _
                                                + TxtTargetPanel1DEC2000SS.Text + "','" + TxtTargetPanel2DEC2000SS.Text + "','" + TxtTargetPanel3DEC2000SS.Text + "','" + TxtTargetPanel4DEC2000SS.Text + "','" _
                                                + TxtTargetPanel1NbrExposedFrames.Text + "','" + TxtTargetPanel2NbrExposedFrames.Text + "','" + TxtTargetPanel3NbrExposedFrames.Text + "','" + TxtTargetPanel4NbrExposedFrames.Text + TxtTargetMosaicOverlap.Text + "','" + "')"

                                daSL.SelectCommand = cmd
                                i = cmd.ExecuteNonQuery
                                pCon.Close()
                            Catch ex As Exception
                                InsertRecord = "InsertRecord: " + ex.Message
                                LogSessionEntry("ERROR", "InsertRecord: " + ex.Message, "", "InsertRecord", "PROGRAM")
                            End Try
                        End Using
                    End Using
                End Using
            End If

            'reload datagrid
            returnvalue = LoadDataGrid()
            If returnvalue <> "OK" Then
                InsertRecord = "NOK"
                Exit Function
            End If
        Catch ex As Exception
            ShowMessage("InsertRecord: " + ex.Message, "CRITICAL", "Error!")
            InsertRecord = "NOK"

        End Try
    End Function


    Public Function DeleteRecord() As String
        Dim returnvalue As String
        Dim i As Integer

        DeleteRecord = "OK"
        Try
            Dim ciClone As CultureInfo = CType(CultureInfo.InvariantCulture.Clone(), CultureInfo)
            ciClone.NumberFormat.NumberDecimalSeparator = "."

            If ShowMessage("Are you sure to delete the record?", "YESNO", "Delete record?") = vbYes Then

                Using pCon As New SQLiteConnection("Data Source=RoboticObservatory.db;Version=3;")
                    Using cmd As SQLiteCommand = pCon.CreateCommand
                        Using da As New SQLiteDataAdapter
                            Try
                                pCon.Open()
                                da.SelectCommand = cmd
                                cmd.CommandText = "DELETE FROM Target where ID = " + TxtID.Text
                                da.SelectCommand = cmd
                                i = cmd.ExecuteNonQuery

                                If i > 0 Then
                                    'set the current record
                                    With pStrucSLCurrentRecord
                                        .TargetName = TxtTargetName.Text
                                        .ID = Convert.ToInt32(TxtID.Text)
                                        .TargetRA2000HH = TxtTargetRA2000HH.Text
                                        .TargetRA2000MM = TxtTargetRA2000MM.Text
                                        .TargetRA2000SS = TxtTargetRA2000SS.Text
                                        .TargetDEC2000DG = TxtTargetDEC2000DG.Text
                                        .TargetDEC2000MM = TxtTargetDEC2000MM.Text
                                        .TargetDEC2000SS = TxtTargetDEC2000SS.Text
                                        .TargetExposure = Double.Parse(TxtTargetExposure.Text, ciClone)
                                        .TargetBin = ComboBoxTargetBinning.Text
                                        .TargetFilter = ComboBoxTargetFilter.Text
                                        .TargetDone = ChkTargetDone.Checked
                                        .TargetPriority = ChkPriorityTarget.Checked
                                        .TargetIsComet = ChkTargetIsComet.Checked
                                        .TargetIgnoreMoon = ChkTargetIgnoreMoon.Checked
                                        .TargetNbrExposedFrames = Convert.ToInt32(TxtTargetNbrExposedFrames.Text)
                                        .TargetNbrFrames = Convert.ToInt32(TxtTargetNbrFrames.Text)
                                        .FocusStarName = TxtFocusStarName.Text
                                        .FocusStarRA2000HH = TxtFocusStarRA2000HH.Text
                                        .FocusStarRA2000MM = TxtFocusStarRA2000MM.Text
                                        .FocusStarRA2000SS = TxtFocusStarRA2000SS.Text
                                        .FocusStarDEC2000DG = TxtFocusStarDEC2000DG.Text
                                        .FocusStarDEC2000MM = TxtFocusStarDEC2000MM.Text
                                        .FocusStarDEC2000SS = TxtFocusStarDEC2000SS.Text
                                        .FocusStarExposure = Double.Parse(TxtFocusStarExposure.Text, ciClone)
                                        .GuideAuto = ChkGuideAuto.Checked
                                        .GuideStarExposure = Double.Parse(TxtGuideStarExposure.Text, ciClone)
                                        .GuideStarXBMF = Convert.ToInt32(TxtGuideStarXBMF.Text)
                                        .GuideStarXPMF = Convert.ToInt32(TxtGuideStarXPMF.Text)
                                        .GuideStarYBMF = Convert.ToInt32(TxtGuideStarYBMF.Text)
                                        .GuideStarYPMF = Convert.ToInt32(TxtGuideStarYPMF.Text)
                                        .ErrorObservingTarget = ChkErrorObservingTarget.Checked
                                        .ErrorTextTarget = TxtErrorTextTarget.Text
                                        .TargetRemarks = TxtTargetRemarks.Text
                                        .TargetLastObservedDate = TxtTargetLastObservedDate.Text
                                        .TargetMosaic = ChkTargetMosaic.Checked
                                        .TargetMosaicType = CmbTargetMosaicType.Text
                                        .TargetMosaicFramesPerPanel = Convert.ToInt32(TxtTargetMosaicFramesPerPanel.Text)
                                        .TargetPanel1RA2000HH = TxtTargetPanel1RA2000HH.Text
                                        .TargetPanel2RA2000HH = TxtTargetPanel2RA2000HH.Text
                                        .TargetPanel3RA2000HH = TxtTargetPanel3RA2000HH.Text
                                        .TargetPanel4RA2000HH = TxtTargetPanel4RA2000HH.Text
                                        .TargetPanel1RA2000MM = TxtTargetPanel1RA2000MM.Text
                                        .TargetPanel2RA2000MM = TxtTargetPanel2RA2000MM.Text
                                        .TargetPanel3RA2000MM = TxtTargetPanel3RA2000MM.Text
                                        .TargetPanel4RA2000MM = TxtTargetPanel4RA2000MM.Text
                                        .TargetPanel1RA2000SS = TxtTargetPanel1RA2000SS.Text
                                        .TargetPanel2RA2000SS = TxtTargetPanel2RA2000SS.Text
                                        .TargetPanel3RA2000SS = TxtTargetPanel3RA2000SS.Text
                                        .TargetPanel4RA2000SS = TxtTargetPanel4RA2000SS.Text
                                        .TargetPanel1DEC2000DG = TxtTargetPanel1DEC2000DG.Text
                                        .TargetPanel2DEC2000DG = TxtTargetPanel2DEC2000DG.Text
                                        .TargetPanel3DEC2000DG = TxtTargetPanel3DEC2000DG.Text
                                        .TargetPanel4DEC2000DG = TxtTargetPanel4DEC2000DG.Text
                                        .TargetPanel1DEC2000MM = TxtTargetPanel1DEC2000MM.Text
                                        .TargetPanel2DEC2000MM = TxtTargetPanel2DEC2000MM.Text
                                        .TargetPanel3DEC2000MM = TxtTargetPanel3DEC2000MM.Text
                                        .TargetPanel4DEC2000MM = TxtTargetPanel4DEC2000MM.Text
                                        .TargetPanel1DEC2000SS = TxtTargetPanel1DEC2000SS.Text
                                        .TargetPanel2DEC2000SS = TxtTargetPanel2DEC2000SS.Text
                                        .TargetPanel3DEC2000SS = TxtTargetPanel3DEC2000SS.Text
                                        .TargetPanel4DEC2000SS = TxtTargetPanel4DEC2000SS.Text
                                        .TargetPanel1NbrExposedFrames = Convert.ToInt32(TxtTargetPanel1NbrExposedFrames.Text)
                                        .TargetPanel2NbrExposedFrames = Convert.ToInt32(TxtTargetPanel2NbrExposedFrames.Text)
                                        .TargetPanel3NbrExposedFrames = Convert.ToInt32(TxtTargetPanel3NbrExposedFrames.Text)
                                        .TargetPanel4NbrExposedFrames = Convert.ToInt32(TxtTargetPanel4NbrExposedFrames.Text)
                                        .TargetMosaicOverlap = Convert.ToInt32(TxtTargetMosaicOverlap.Text)
                                    End With
                                Else
                                    ShowMessage("No record was DELETED!", "CRITICAL", "Incorrect input...")
                                End If
                                pCon.Close()

                            Catch ex As Exception
                                DeleteRecord = "DeleteRecord: " + ex.Message
                                LogSessionEntry("ERROR", "DeleteRecord: " + ex.Message, "", "DeleteRecord", "PROGRAM")
                            End Try
                        End Using
                    End Using
                End Using

                'reload datagrid
                returnvalue = LoadDataGrid()
                If returnvalue <> "OK" Then
                    DeleteRecord = "NOK"
                    Exit Function
                End If
            End If


        Catch ex As Exception
            ShowMessage("DeleteRecord: " + ex.Message, "CRITICAL", "Error!")
            DeleteRecord = "NOK"
        End Try

    End Function



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


    Private Sub ComboBoxTargetFilter_TextChanged(sender As Object, e As EventArgs) Handles ComboBoxTargetFilter.TextChanged
        If ComboBoxTargetFilter.Text <> "" And ComboBoxTargetFilter.Items.Count > 0 Then

            Dim doesContain As Boolean = False
            For Each item As Object In ComboBoxTargetFilter.Items
                If item.ToString() = ComboBoxTargetFilter.Text Then
                    doesContain = True
                    Exit For
                End If
            Next

            If doesContain = False Then
                ShowMessage("Text not in list !", "CRITICAL", "Correct user input...")
            End If
        End If
    End Sub

    Private Sub ComboBoxTargetBinning_TextChanged(sender As Object, e As EventArgs) Handles ComboBoxTargetBinning.TextChanged
        If ComboBoxTargetBinning.Text <> "" And ComboBoxTargetBinning.Items.Count > 0 Then

            Dim doesContain As Boolean = False
            For Each item As Object In ComboBoxTargetBinning.Items
                If item.ToString() = ComboBoxTargetBinning.Text Then
                    doesContain = True
                    Exit For
                End If
            Next

            If doesContain = False Then
                ShowMessage("Text not in list !", "CRITICAL", "Incorrect input...")
            End If
        End If
    End Sub

    Private Sub TxtTargetExposure_TextChanged(sender As Object, e As EventArgs) Handles TxtTargetExposure.TextChanged
        Try
            Dim ciClone As CultureInfo = CType(CultureInfo.InvariantCulture.Clone(), CultureInfo)
            ciClone.NumberFormat.NumberDecimalSeparator = "."

            If TxtTargetExposure.Text <> "" Then
                If IsNumeric(TxtTargetExposure.Text) = False Then
                    ShowMessage("Not a number between 0 and 9999!", "CRITICAL", "Incorrect input...")
                    Exit Sub
                End If
                If Double.Parse(TxtTargetExposure.Text, ciClone) < 0 Or Double.Parse(TxtTargetExposure.Text, ciClone) > 9999 Then
                    ShowMessage("Not a number between 0 and 9999!", "CRITICAL", "Incorrect input...")
                    Exit Sub
                End If
            End If

        Catch ex As Exception
            ShowMessage("TxtTargetExposure_TextChanged: " + ex.Message, "CRITICAL", "Error!")
        End Try
    End Sub

    Private Sub TxtTargetNbrFrames_TextChanged(sender As Object, e As EventArgs) Handles TxtTargetNbrFrames.TextChanged
        Try
            Dim ciClone As CultureInfo = CType(CultureInfo.InvariantCulture.Clone(), CultureInfo)
            ciClone.NumberFormat.NumberDecimalSeparator = "."

            If TxtTargetNbrFrames.Text <> "" Then
                If IsNumeric(TxtTargetNbrFrames.Text) = False Then
                    ShowMessage("Not a number between 0 and 9999!", "CRITICAL", "Incorrect input...")
                    Exit Sub
                End If
                If Double.Parse(TxtTargetNbrFrames.Text, ciClone) < 0 Or Double.Parse(TxtTargetNbrFrames.Text, ciClone) > 9999 Then
                    ShowMessage("Not a number between 0 and 9999!", "CRITICAL", "Incorrect input...")
                    Exit Sub
                End If
            End If
        Catch ex As Exception
            ShowMessage("TxtTargetNbrFrames_TextChanged: " + ex.Message, "CRITICAL", "Error!")
        End Try
    End Sub

    Private Sub TxtTargetNbrExposedFrames_TextChanged(sender As Object, e As EventArgs) Handles TxtTargetNbrExposedFrames.TextChanged
        Try
            If TxtTargetNbrExposedFrames.Text <> "" Then
                If IsNumeric(TxtTargetNbrExposedFrames.Text) = False Then
                    ShowMessage("Not a number between 0 and 9999!", "CRITICAL", "Incorrect input...")
                    Exit Sub
                End If
                If Convert.ToInt32(TxtTargetNbrExposedFrames.Text) < 0 Or Convert.ToInt32(TxtTargetNbrExposedFrames.Text) > 9999 Then
                    ShowMessage("Not a number between 0 and 9999!", "CRITICAL", "Incorrect user input...")
                    Exit Sub
                End If
            End If
        Catch ex As Exception
            ShowMessage("TxtTargetNbrExposedFrames_TextChanged: " + ex.Message, "CRITICAL", "Error!")
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


    Private Sub TxtTargetName_TextChanged(sender As Object, e As EventArgs) Handles TxtTargetName.TextChanged
        Try
            If TxtTargetName.Text = "" And TxtTargetName.DataBindings.Count > 0 Then
                ShowMessage("Target name cannot be empty!", "CRITICAL", "Incorrect input...")
                Exit Sub
            End If
        Catch ex As Exception
            ShowMessage("TxtTargetName_TextChanged: " + ex.Message, "CRITICAL", "Error!")
        End Try
    End Sub

    Private Sub TxtGuideStarExposure_TextChanged(sender As Object, e As EventArgs) Handles TxtGuideStarExposure.TextChanged
        Try
            If TxtGuideStarExposure.Text <> "" Then
                If IsNumeric(TxtGuideStarExposure.Text) = False Then
                    ShowMessage("Not a number between 0 and 9999!", "CRITICAL", "Incorrect input...")
                    Exit Sub
                End If
                If Convert.ToInt32(TxtGuideStarExposure.Text) < 0 Or Convert.ToInt32(TxtGuideStarExposure.Text) > 9999 Then
                    ShowMessage("Not a number between 0 and 9999!", "CRITICAL", "Incorrect input...")
                    Exit Sub
                End If
            End If
        Catch ex As Exception
            ShowMessage("TxtGuideStarExposure_TextChanged: " + ex.Message, "CRITICAL", "Error!")
        End Try
    End Sub

    Private Sub TxtGuideStarXBMF_TextChanged(sender As Object, e As EventArgs) Handles TxtGuideStarXBMF.TextChanged
        Try
            If TxtGuideStarXBMF.Text <> "" Then
                If IsNumeric(TxtGuideStarXBMF.Text) = False Then
                    ShowMessage("Not a number between 0 and 9999!", "CRITICAL", "Incorrect input...")
                    Exit Sub
                End If
                If Convert.ToInt32(TxtGuideStarXBMF.Text) < 0 Or Convert.ToInt32(TxtGuideStarXBMF.Text) > 9999 Then
                    ShowMessage("Not a number between 0 and 9999!", "CRITICAL", "Incorrect input...")
                    Exit Sub
                End If
            End If
        Catch ex As Exception
            ShowMessage("TxtGuideStarXBMF_TextChanged: " + ex.Message, "CRITICAL", "Error!")
        End Try
    End Sub

    Private Sub TxtGuideStarXPMF_TextChanged(sender As Object, e As EventArgs) Handles TxtGuideStarXPMF.TextChanged
        Try
            If TxtGuideStarXPMF.Text <> "" Then
                If IsNumeric(TxtGuideStarXPMF.Text) = False Then
                    ShowMessage("Not a number between 0 and 9999!", "CRITICAL", "Incorrect input...")
                    Exit Sub
                End If
                If Convert.ToInt32(TxtGuideStarXPMF.Text) < 0 Or Convert.ToInt32(TxtGuideStarXPMF.Text) > 9999 Then
                    ShowMessage("Not a number between 0 and 9999!", "CRITICAL", "Incorrect input...")
                    Exit Sub
                End If
            End If
        Catch ex As Exception
            ShowMessage("TxtGuideStarXPMF_TextChanged: " + ex.Message, "CRITICAL", "Error!")
        End Try
    End Sub

    Private Sub TxtGuideStarYBMF_TextChanged(sender As Object, e As EventArgs) Handles TxtGuideStarYBMF.TextChanged
        Try
            If TxtGuideStarYBMF.Text <> "" Then
                If IsNumeric(TxtGuideStarYBMF.Text) = False Then
                    ShowMessage("Not a number between 0 and 9999!", "CRITICAL", "Incorrect input...")
                    Exit Sub
                End If
                If Convert.ToInt32(TxtGuideStarYBMF.Text) < 0 Or Convert.ToInt32(TxtGuideStarYBMF.Text) > 9999 Then
                    ShowMessage("Not a number between 0 and 9999!", "CRITICAL", "Incorrect input...")
                    Exit Sub
                End If
            End If
        Catch ex As Exception
            ShowMessage("TxtGuideStarYBMF_TextChanged: " + ex.Message, "CRITICAL", "Error!")
        End Try
    End Sub

    Private Sub TxtGuideStarYPMF_TextChanged(sender As Object, e As EventArgs) Handles TxtGuideStarYPMF.TextChanged
        Try
            If TxtGuideStarYPMF.Text <> "" Then
                If IsNumeric(TxtGuideStarYPMF.Text) = False Then
                    ShowMessage("Not a number between 0 and 9999!", "CRITICAL", "Incorrect input...")
                    Exit Sub
                End If
                If Convert.ToInt32(TxtGuideStarYPMF.Text) < 0 Or Convert.ToInt32(TxtGuideStarYPMF.Text) > 9999 Then
                    ShowMessage("Not a number between 0 and 9999!", "CRITICAL", "Incorrect input...")
                    Exit Sub
                End If
            End If
        Catch ex As Exception
            ShowMessage("TxtGuideStarYPMF_TextChanged: " + ex.Message, "CRITICAL", "Error!")
        End Try
    End Sub

    Private Sub BtnFindObject_Click(sender As Object, e As EventArgs) Handles BtnFindObject.Click
        Dim returnvalue As String

        Try
            If TxtTargetName.Text <> "" Then

                returnvalue = FindTheSkyXTarget(TxtTargetName.Text)

                Select Case returnvalue
                    Case "NOTFOUND"
                        ShowMessage("Target not found !", "CRITICAL", "The Sky X Application...")
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
        Catch ex As Exception
            ShowMessage("BtnFindObject_Click: " + ex.Message, "CRITICAL", "Error!")
        End Try
    End Sub

    Private Sub DataGridViewTarget_KeyDown(sender As Object, e As KeyEventArgs) Handles DataGridViewTarget.KeyDown
        Dim TargetName As String, TargetFilter As String
        Dim returnvalue As String
        Dim cmd As New OleDb.OleDbCommand

        Try
            '-------------------------------------------------------------------------------------------------------------
            'check for changes
            '-------------------------------------------------------------------------------------------------------------
            InsertUpdateRecord()

            '-------------------------------------------------------------------------------------------------------------
            'change the row
            '-------------------------------------------------------------------------------------------------------------
            TargetName = DataGridViewTarget.CurrentRow.Cells(0).Value.ToString
            TargetFilter = DataGridViewTarget.CurrentRow.Cells(1).Value.ToString

            returnvalue = LoadRecord(TargetName, TargetFilter)
            If returnvalue <> "OK" Then
                Exit Sub
            End If

        Catch ex As Exception
            ShowMessage("DataGridViewTarget_CellClick: " + ex.Message, "CRITICAL", "Error!")
        End Try
    End Sub

    Private Sub DataGridViewTarget_KeyUp(sender As Object, e As KeyEventArgs) Handles DataGridViewTarget.KeyUp
        Dim TargetName, TargetFilter As String
        Dim returnvalue As String
        Dim cmd As New OleDb.OleDbCommand

        Try
            '-------------------------------------------------------------------------------------------------------------
            'check for changes
            '-------------------------------------------------------------------------------------------------------------
            InsertUpdateRecord()

            '-------------------------------------------------------------------------------------------------------------
            'change the row
            '-------------------------------------------------------------------------------------------------------------
            TargetName = DataGridViewTarget.CurrentRow.Cells(0).Value.ToString
            TargetFilter = DataGridViewTarget.CurrentRow.Cells(1).Value.ToString

            returnvalue = LoadRecord(TargetName, TargetFilter)
            If returnvalue <> "OK" Then
                Exit Sub
            End If

        Catch ex As Exception
            ShowMessage("DataGridViewTarget_CellClick: " + ex.Message, "CRITICAL", "Error!")
        End Try
    End Sub

    Private Sub BrnDuplicate_Click(sender As Object, e As EventArgs) Handles BrnDuplicate.Click
        Try
            'add row to datagrid
            dtSL_master.Rows.Add()
            DataGridViewTarget.Refresh()
            If DataGridViewTarget.Rows.Count > 0 Then
                Dim i = DataGridViewTarget.Rows.Count - 1
                DataGridViewTarget.Rows(i).Cells(0).Selected = True
            End If
            DataGridViewTarget.CurrentRow.Cells(0).Selected = True

            TxtID.Text = ""
            TxtTargetName.DataBindings.Clear()

        Catch ex As Exception
            ShowMessage("BtnNew_Click: " + ex.Message, "CRITICAL", "Error!")
        End Try
    End Sub

    Private Sub BtnFindFocusStar_Click(sender As Object, e As EventArgs) Handles BtnFindFocusStar.Click
        Dim returnvalue As String

        Try
            If TxtFocusStarName.Text <> "" Then

                returnvalue = FindTheSkyXTarget(TxtFocusStarName.Text)

                Select Case returnvalue
                    Case "NOTFOUND"
                        ShowMessage("Focus star not found !", "OKONLY", "The Sky X Application...")
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

    Private Sub ChkErrorObservingTarget_CheckedChanged(sender As Object, e As EventArgs) Handles ChkErrorObservingTarget.CheckedChanged
        TxtErrorTextTarget.Text = ""
    End Sub


    Private Sub InsertUpdateRecord()
        Try
            Dim returnvalue As String
            Dim ciClone As CultureInfo = CType(CultureInfo.InvariantCulture.Clone(), CultureInfo)
            ciClone.NumberFormat.NumberDecimalSeparator = "."

            'check if target and focusstar are not too far of
            If TxtFocusStarName.Text <> "" Then
                If Math.Abs(Double.Parse(TxtFocusStarRA2000HH.Text, ciClone) - Double.Parse(TxtTargetRA2000HH.Text, ciClone)) > 2 Or
                        Math.Abs(Double.Parse(TxtFocusStarDEC2000DG.Text, ciClone) - Double.Parse(TxtTargetDEC2000DG.Text, ciClone)) > 10 Then
                    ShowMessage("Are you sure the focusstar Is correct? It should be maximum within 2 hours RA And 10° DEC from the target!", "CRITICAL", "Check the focusstar ...")
                    Exit Sub
                End If
            End If

            '-------------------------------------------------------------------------------------------------------------
            'check for changes
            '-------------------------------------------------------------------------------------------------------------
            'If TxtTargetName.DataBindings.Count = 1 Then
            If pStrucSLCurrentRecord.TargetName = TxtTargetName.Text And TxtTargetName.Text <> "" Then
                'update
                returnvalue = UpdateRecord()
                If returnvalue <> "OK" Then
                    ShowMessage("Update failed!", "CRITICAL", "Update record...")
                    Exit Sub
                End If
            ElseIf TxtTargetName.Text <> "" Then
                'insert
                returnvalue = InsertRecord()
                If returnvalue <> "OK" Then
                    ShowMessage("Insert failed!", "CRITICAL", "Insert record...")
                    Exit Sub
                End If
            End If

        Catch ex As Exception
            ShowMessage("InsertUpdateRecord: " + ex.Message, "CRITICAL", "Error!")
        End Try
    End Sub


    Private Sub FrmTarget_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        InsertUpdateRecord()
    End Sub

    Private Sub BtnCancel_Click(sender As Object, e As EventArgs) Handles BtnCancel.Click
        Me.Close()
    End Sub


    Private Sub ChkTargetMosaic_CheckedChanged(sender As Object, e As EventArgs) Handles ChkTargetMosaic.CheckedChanged
        If ChkTargetMosaic.Checked = True Then
            'enable mosaic fields
            CmbTargetMosaicType.Enabled = True
            If CmbTargetMosaicType.Text = "" Then
                CmbTargetMosaicType.Text = "2x2"
            End If
            BtnCalculateMosaic.Enabled = True

            If TxtTargetMosaicFramesPerPanel.Text = "0" Or TxtTargetMosaicFramesPerPanel.Text = "" Then
                TxtTargetMosaicFramesPerPanel.Text = "500"
            End If

            TxtTargetMosaicFramesPerPanel.Enabled = True
            TxtTargetPanel1RA2000HH.Enabled = True
            TxtTargetPanel2RA2000HH.Enabled = True
            TxtTargetPanel3RA2000HH.Enabled = True
            TxtTargetPanel4RA2000HH.Enabled = True
            TxtTargetPanel1RA2000MM.Enabled = True
            TxtTargetPanel2RA2000MM.Enabled = True
            TxtTargetPanel3RA2000MM.Enabled = True
            TxtTargetPanel4RA2000MM.Enabled = True
            TxtTargetPanel1RA2000SS.Enabled = True
            TxtTargetPanel2RA2000SS.Enabled = True
            TxtTargetPanel3RA2000SS.Enabled = True
            TxtTargetPanel4RA2000SS.Enabled = True
            TxtTargetPanel1DEC2000DG.Enabled = True
            TxtTargetPanel2DEC2000DG.Enabled = True
            TxtTargetPanel3DEC2000DG.Enabled = True
            TxtTargetPanel4DEC2000DG.Enabled = True
            TxtTargetPanel1DEC2000MM.Enabled = True
            TxtTargetPanel2DEC2000MM.Enabled = True
            TxtTargetPanel3DEC2000MM.Enabled = True
            TxtTargetPanel4DEC2000MM.Enabled = True
            TxtTargetPanel1DEC2000SS.Enabled = True
            TxtTargetPanel2DEC2000SS.Enabled = True
            TxtTargetPanel3DEC2000SS.Enabled = True
            TxtTargetPanel4DEC2000SS.Enabled = True
            TxtTargetPanel1NbrExposedFrames.Enabled = True
            TxtTargetPanel2NbrExposedFrames.Enabled = True
            TxtTargetPanel3NbrExposedFrames.Enabled = True
            TxtTargetPanel4NbrExposedFrames.Enabled = True
            TxtTargetMosaicOverlap.Enabled = True

            'TxtTargetNbrFrames.Text = "0"
            'TxtTargetNbrExposedFrames.Text = "0"

            'never do panels on a comet
            ChkTargetIsComet.Enabled = False
            ChkTargetIsComet.Checked = False
            TxtTargetNbrFrames.Enabled = False
            TxtTargetNbrExposedFrames.Enabled = False

        Else
            'disable mosaic fields
            CmbTargetMosaicType.Enabled = False
            CmbTargetMosaicType.Text = ""
            TxtTargetMosaicFramesPerPanel.Enabled = False
            TxtTargetPanel1RA2000HH.Enabled = False
            TxtTargetPanel2RA2000HH.Enabled = False
            TxtTargetPanel3RA2000HH.Enabled = False
            TxtTargetPanel4RA2000HH.Enabled = False
            TxtTargetPanel1RA2000MM.Enabled = False
            TxtTargetPanel2RA2000MM.Enabled = False
            TxtTargetPanel3RA2000MM.Enabled = False
            TxtTargetPanel4RA2000MM.Enabled = False
            TxtTargetPanel1RA2000SS.Enabled = False
            TxtTargetPanel2RA2000SS.Enabled = False
            TxtTargetPanel3RA2000SS.Enabled = False
            TxtTargetPanel4RA2000SS.Enabled = False
            TxtTargetPanel1DEC2000DG.Enabled = False
            TxtTargetPanel2DEC2000DG.Enabled = False
            TxtTargetPanel3DEC2000DG.Enabled = False
            TxtTargetPanel4DEC2000DG.Enabled = False
            TxtTargetPanel1DEC2000MM.Enabled = False
            TxtTargetPanel2DEC2000MM.Enabled = False
            TxtTargetPanel3DEC2000MM.Enabled = False
            TxtTargetPanel4DEC2000MM.Enabled = False
            TxtTargetPanel1DEC2000SS.Enabled = False
            TxtTargetPanel2DEC2000SS.Enabled = False
            TxtTargetPanel3DEC2000SS.Enabled = False
            TxtTargetPanel4DEC2000SS.Enabled = False
            TxtTargetPanel1NbrExposedFrames.Enabled = False
            TxtTargetPanel2NbrExposedFrames.Enabled = False
            TxtTargetPanel3NbrExposedFrames.Enabled = False
            TxtTargetPanel4NbrExposedFrames.Enabled = False
            TxtTargetMosaicOverlap.Enabled = False
            BtnCalculateMosaic.Enabled = False

            ChkTargetIsComet.Enabled = True
            TxtTargetNbrFrames.Enabled = True
            TxtTargetNbrFrames.Text = "500"
            TxtTargetNbrExposedFrames.Enabled = True
        End If

        If CmbTargetMosaicType.Text = "2x2" Then
            LabelMosaic1.Text = "1"
            LabelMosaic2.Text = "2"
            LabelMosaic3.Text = "3"
            LabelMosaic4.Text = "4"
            LabelMosaic1.BorderStyle = BorderStyle.FixedSingle
            LabelMosaic2.BorderStyle = BorderStyle.FixedSingle
            LabelMosaic3.BorderStyle = BorderStyle.FixedSingle
            LabelMosaic4.BorderStyle = BorderStyle.FixedSingle
        ElseIf CmbTargetMosaicType.Text = "1x2" Then
            LabelMosaic1.Text = "1"
            LabelMosaic2.Text = ""
            LabelMosaic3.Text = "2"
            LabelMosaic4.Text = ""
            LabelMosaic1.BorderStyle = BorderStyle.FixedSingle
            LabelMosaic2.BorderStyle = BorderStyle.None
            LabelMosaic3.BorderStyle = BorderStyle.FixedSingle
            LabelMosaic4.BorderStyle = BorderStyle.None
        ElseIf CmbTargetMosaicType.Text = "2x1" Then
            LabelMosaic1.Text = "1"
            LabelMosaic2.Text = "2"
            LabelMosaic3.Text = ""
            LabelMosaic4.Text = ""
            LabelMosaic1.BorderStyle = BorderStyle.FixedSingle
            LabelMosaic2.BorderStyle = BorderStyle.FixedSingle
            LabelMosaic3.BorderStyle = BorderStyle.None
            LabelMosaic4.BorderStyle = BorderStyle.None
        ElseIf CmbTargetMosaicType.Text = "" Then
            LabelMosaic1.Text = ""
            LabelMosaic2.Text = ""
            LabelMosaic3.Text = ""
            LabelMosaic4.Text = ""
            LabelMosaic1.BorderStyle = BorderStyle.None
            LabelMosaic2.BorderStyle = BorderStyle.None
            LabelMosaic3.BorderStyle = BorderStyle.None
            LabelMosaic4.BorderStyle = BorderStyle.None
        End If
    End Sub

    Private Sub TxtTargetPanel1RA2000HH_TextChanged(sender As Object, e As EventArgs) Handles TxtTargetPanel1RA2000HH.TextChanged
        Try
            Dim ciClone As CultureInfo = CType(CultureInfo.InvariantCulture.Clone(), CultureInfo)
            ciClone.NumberFormat.NumberDecimalSeparator = "."

            If TxtTargetPanel1RA2000HH.Text <> "" Then
                If IsNumeric(TxtTargetPanel1RA2000HH.Text) = False Then
                    ShowMessage("Not a number between 0 and 24!", "CRITICAL", "Incorrect input...")
                    Exit Sub
                End If
                If Double.Parse(TxtTargetPanel1RA2000HH.Text, ciClone) < 0 Or Double.Parse(TxtTargetPanel1RA2000HH.Text, ciClone) > 24 Then
                    ShowMessage("Not a number between 0 and 24!", "CRITICAL", "Incorrect input...")
                    Exit Sub
                End If
            End If
        Catch ex As Exception
            ShowMessage("TxtTargetPanel1RA2000HH_TextChanged: " + ex.Message, "CRITICAL", "Error!")
        End Try
    End Sub

    Private Sub TxtTargetPanel2RA2000HH_TextChanged(sender As Object, e As EventArgs) Handles TxtTargetPanel2RA2000HH.TextChanged
        Try
            Dim ciClone As CultureInfo = CType(CultureInfo.InvariantCulture.Clone(), CultureInfo)
            ciClone.NumberFormat.NumberDecimalSeparator = "."

            If TxtTargetPanel2RA2000HH.Text <> "" Then
                If IsNumeric(TxtTargetPanel2RA2000HH.Text) = False Then
                    ShowMessage("Not a number between 0 and 24!", "CRITICAL", "Incorrect input...")
                    Exit Sub
                End If
                If Double.Parse(TxtTargetPanel2RA2000HH.Text, ciClone) < 0 Or Double.Parse(TxtTargetPanel2RA2000HH.Text, ciClone) > 24 Then
                    ShowMessage("Not a number between 0 and 24!", "CRITICAL", "Incorrect input...")
                    Exit Sub
                End If
            End If
        Catch ex As Exception
            ShowMessage("TxtTargetPanel2RA2000HH_TextChanged: " + ex.Message, "CRITICAL", "Error!")
        End Try
    End Sub

    Private Sub TxtTargetPanel3RA2000HH_TextChanged(sender As Object, e As EventArgs) Handles TxtTargetPanel3RA2000HH.TextChanged
        Try
            Dim ciClone As CultureInfo = CType(CultureInfo.InvariantCulture.Clone(), CultureInfo)
            ciClone.NumberFormat.NumberDecimalSeparator = "."

            If TxtTargetPanel3RA2000HH.Text <> "" Then
                If IsNumeric(TxtTargetPanel3RA2000HH.Text) = False Then
                    ShowMessage("Not a number between 0 and 24!", "CRITICAL", "Incorrect input...")
                    Exit Sub
                End If
                If Double.Parse(TxtTargetPanel3RA2000HH.Text, ciClone) < 0 Or Double.Parse(TxtTargetPanel3RA2000HH.Text, ciClone) > 24 Then
                    ShowMessage("Not a number between 0 and 24!", "CRITICAL", "Incorrect input...")
                    Exit Sub
                End If
            End If
        Catch ex As Exception
            ShowMessage("TxtTargetPanel3RA2000HH_TextChanged: " + ex.Message, "CRITICAL", "Error!")
        End Try
    End Sub

    Private Sub TxtTargetPanel4RA2000HH_TextChanged(sender As Object, e As EventArgs) Handles TxtTargetPanel4RA2000HH.TextChanged
        Try
            Dim ciClone As CultureInfo = CType(CultureInfo.InvariantCulture.Clone(), CultureInfo)
            ciClone.NumberFormat.NumberDecimalSeparator = "."

            If TxtTargetPanel4RA2000HH.Text <> "" Then
                If IsNumeric(TxtTargetPanel4RA2000HH.Text) = False Then
                    ShowMessage("Not a number between 0 and 24!", "CRITICAL", "Incorrect input...")
                    Exit Sub
                End If
                If Double.Parse(TxtTargetPanel4RA2000HH.Text, ciClone) < 0 Or Double.Parse(TxtTargetPanel4RA2000HH.Text, ciClone) > 24 Then
                    ShowMessage("Not a number between 0 and 24!", "CRITICAL", "Incorrect input...")
                    Exit Sub
                End If
            End If
        Catch ex As Exception
            ShowMessage("TxtTargetPanel4RA2000HH_TextChanged: " + ex.Message, "CRITICAL", "Error!")
        End Try
    End Sub

    Private Sub TxtTargetPanel1RA2000MM_TextChanged(sender As Object, e As EventArgs) Handles TxtTargetPanel1RA2000MM.TextChanged
        Try
            Dim ciClone As CultureInfo = CType(CultureInfo.InvariantCulture.Clone(), CultureInfo)
            ciClone.NumberFormat.NumberDecimalSeparator = "."

            If TxtTargetPanel1RA2000MM.Text <> "" Then
                If IsNumeric(TxtTargetPanel1RA2000MM.Text) = False Then
                    ShowMessage("Not a number between 0 and 60!", "CRITICAL", "Incorrect input...")
                    Exit Sub
                End If
                If Double.Parse(TxtTargetPanel1RA2000MM.Text, ciClone) < 0 Or Double.Parse(TxtTargetPanel1RA2000MM.Text, ciClone) > 60 Then
                    ShowMessage("Not a number between 0 and 60!", "CRITICAL", "Incorrect input...")
                    Exit Sub
                End If
            End If
        Catch ex As Exception
            ShowMessage("TxtTargetPanel1RA2000MM_TextChanged: " + ex.Message, "CRITICAL", "Error!")
        End Try
    End Sub

    Private Sub TxtTargetPanel2RA2000MM_TextChanged(sender As Object, e As EventArgs) Handles TxtTargetPanel2RA2000MM.TextChanged
        Try
            Dim ciClone As CultureInfo = CType(CultureInfo.InvariantCulture.Clone(), CultureInfo)
            ciClone.NumberFormat.NumberDecimalSeparator = "."

            If TxtTargetPanel2RA2000MM.Text <> "" Then
                If IsNumeric(TxtTargetPanel2RA2000MM.Text) = False Then
                    ShowMessage("Not a number between 0 and 60!", "CRITICAL", "Incorrect input...")
                    Exit Sub
                End If
                If Double.Parse(TxtTargetPanel2RA2000MM.Text, ciClone) < 0 Or Double.Parse(TxtTargetPanel2RA2000MM.Text, ciClone) > 60 Then
                    ShowMessage("Not a number between 0 and 60!", "CRITICAL", "Incorrect input...")
                    Exit Sub
                End If
            End If
        Catch ex As Exception
            ShowMessage("TxtTargetPanel2RA2000MM_TextChanged: " + ex.Message, "CRITICAL", "Error!")
        End Try
    End Sub

    Private Sub TxtTargetPanel3RA2000MM_TextChanged(sender As Object, e As EventArgs) Handles TxtTargetPanel3RA2000MM.TextChanged
        Try
            Dim ciClone As CultureInfo = CType(CultureInfo.InvariantCulture.Clone(), CultureInfo)
            ciClone.NumberFormat.NumberDecimalSeparator = "."

            If TxtTargetPanel3RA2000MM.Text <> "" Then
                If IsNumeric(TxtTargetPanel3RA2000MM.Text) = False Then
                    ShowMessage("Not a number between 0 and 60!", "CRITICAL", "Incorrect input...")
                    Exit Sub
                End If
                If Double.Parse(TxtTargetPanel3RA2000MM.Text, ciClone) < 0 Or Double.Parse(TxtTargetPanel3RA2000MM.Text, ciClone) > 60 Then
                    ShowMessage("Not a number between 0 and 60!", "CRITICAL", "Incorrect input...")
                    Exit Sub
                End If
            End If
        Catch ex As Exception
            ShowMessage("TTxtTargetPanel3RA2000MM_TextChanged: " + ex.Message, "CRITICAL", "Error!")
        End Try
    End Sub

    Private Sub TxtTargetPanel4RA2000MM_TextChanged(sender As Object, e As EventArgs) Handles TxtTargetPanel4RA2000MM.TextChanged
        Try
            Dim ciClone As CultureInfo = CType(CultureInfo.InvariantCulture.Clone(), CultureInfo)
            ciClone.NumberFormat.NumberDecimalSeparator = "."

            If TxtTargetPanel4RA2000MM.Text <> "" Then
                If IsNumeric(TxtTargetPanel4RA2000MM.Text) = False Then
                    ShowMessage("Not a number between 0 and 60!", "CRITICAL", "Incorrect input...")
                    Exit Sub
                End If
                If Double.Parse(TxtTargetPanel4RA2000MM.Text, ciClone) < 0 Or Double.Parse(TxtTargetPanel4RA2000MM.Text, ciClone) > 60 Then
                    ShowMessage("Not a number between 0 and 60!", "CRITICAL", "Incorrect input...")
                    Exit Sub
                End If
            End If
        Catch ex As Exception
            ShowMessage("TxtTargetPanel4RA2000MM_TextChanged: " + ex.Message, "CRITICAL", "Error!")
        End Try
    End Sub

    Private Sub TxtTargetPanel1RA2000SS_TextChanged(sender As Object, e As EventArgs) Handles TxtTargetPanel1RA2000SS.TextChanged
        Try
            Dim ciClone As CultureInfo = CType(CultureInfo.InvariantCulture.Clone(), CultureInfo)
            ciClone.NumberFormat.NumberDecimalSeparator = "."

            If TxtTargetPanel1RA2000SS.Text <> "" Then
                If IsNumeric(TxtTargetPanel1RA2000SS.Text) = False Then
                    ShowMessage("Not a number between 0 and 60!", "CRITICAL", "Incorrect input...")
                    Exit Sub
                End If
                If Double.Parse(TxtTargetPanel1RA2000SS.Text, ciClone) < 0 Or Double.Parse(TxtTargetPanel1RA2000SS.Text, ciClone) > 60 Then
                    ShowMessage("Not a number between 0 and 60!", "CRITICAL", "Incorrect input...")
                    Exit Sub
                End If
            End If
        Catch ex As Exception
            ShowMessage("TxtTargetPanel1RA2000SS_TextChanged: " + ex.Message, "CRITICAL", "Error!")
        End Try
    End Sub

    Private Sub TxtTargetPanel2RA2000SS_TextChanged(sender As Object, e As EventArgs) Handles TxtTargetPanel2RA2000SS.TextChanged
        Try
            Dim ciClone As CultureInfo = CType(CultureInfo.InvariantCulture.Clone(), CultureInfo)
            ciClone.NumberFormat.NumberDecimalSeparator = "."

            If TxtTargetPanel2RA2000SS.Text <> "" Then
                If IsNumeric(TxtTargetPanel2RA2000SS.Text) = False Then
                    ShowMessage("Not a number between 0 and 60!", "CRITICAL", "Incorrect input...")
                    Exit Sub
                End If
                If Double.Parse(TxtTargetPanel2RA2000SS.Text, ciClone) < 0 Or Double.Parse(TxtTargetPanel2RA2000SS.Text, ciClone) > 60 Then
                    ShowMessage("Not a number between 0 and 60!", "CRITICAL", "Incorrect input...")
                    Exit Sub
                End If
            End If
        Catch ex As Exception
            ShowMessage("TxtTargetPanel2RA2000SS_TextChanged: " + ex.Message, "CRITICAL", "Error!")
        End Try
    End Sub

    Private Sub TxtTargetPanel3RA2000SS_TextChanged(sender As Object, e As EventArgs) Handles TxtTargetPanel3RA2000SS.TextChanged
        Try
            Dim ciClone As CultureInfo = CType(CultureInfo.InvariantCulture.Clone(), CultureInfo)
            ciClone.NumberFormat.NumberDecimalSeparator = "."

            If TxtTargetPanel3RA2000SS.Text <> "" Then
                If IsNumeric(TxtTargetPanel3RA2000SS.Text) = False Then
                    ShowMessage("Not a number between 0 and 60!", "CRITICAL", "Incorrect input...")
                    Exit Sub
                End If
                If Double.Parse(TxtTargetPanel3RA2000SS.Text, ciClone) < 0 Or Double.Parse(TxtTargetPanel3RA2000SS.Text, ciClone) > 60 Then
                    ShowMessage("Not a number between 0 and 60!", "CRITICAL", "Incorrect input...")
                    Exit Sub
                End If
            End If
        Catch ex As Exception
            ShowMessage("TxtTargetPanel3RA2000SS_TextChanged: " + ex.Message, "CRITICAL", "Error!")
        End Try
    End Sub

    Private Sub TxtTargetPanel4RA2000SS_TextChanged(sender As Object, e As EventArgs) Handles TxtTargetPanel4RA2000SS.TextChanged
        Try
            Dim ciClone As CultureInfo = CType(CultureInfo.InvariantCulture.Clone(), CultureInfo)
            ciClone.NumberFormat.NumberDecimalSeparator = "."

            If TxtTargetPanel4RA2000SS.Text <> "" Then
                If IsNumeric(TxtTargetPanel4RA2000SS.Text) = False Then
                    ShowMessage("Not a number between 0 and 60!", "CRITICAL", "Incorrect input...")
                    Exit Sub
                End If
                If Double.Parse(TxtTargetPanel4RA2000SS.Text, ciClone) < 0 Or Double.Parse(TxtTargetPanel4RA2000SS.Text, ciClone) > 60 Then
                    ShowMessage("Not a number between 0 and 60!", "CRITICAL", "Incorrect input...")
                    Exit Sub
                End If
            End If
        Catch ex As Exception
            ShowMessage("TxtTargetPanel4RA2000SS_TextChanged: " + ex.Message, "CRITICAL", "Error!")
        End Try
    End Sub

    Private Sub TxtTargetPanel1DEC2000DG_TextChanged(sender As Object, e As EventArgs) Handles TxtTargetPanel1DEC2000DG.TextChanged
        Try
            If TxtTargetPanel1DEC2000DG.Text <> "" Then
                If IsNumeric(TxtTargetPanel1DEC2000DG.Text) = False Then
                    ShowMessage("Not a number!", "CRITICAL", "Incorrect input...")
                    Exit Sub
                End If
            End If
        Catch ex As Exception
            ShowMessage("TxtTargetPanel1DEC2000DG_TextChanged: " + ex.Message, "CRITICAL", "Error!")
        End Try
    End Sub

    Private Sub TxtTargetPanel2DEC2000DG_TextChanged(sender As Object, e As EventArgs) Handles TxtTargetPanel2DEC2000DG.TextChanged
        Try
            If TxtTargetPanel2DEC2000DG.Text <> "" Then
                If IsNumeric(TxtTargetPanel2DEC2000DG.Text) = False Then
                    ShowMessage("Not a number!", "CRITICAL", "Incorrect input...")
                    Exit Sub
                End If
            End If
        Catch ex As Exception
            ShowMessage("TxtTargetPanel2DEC2000DG_TextChanged: " + ex.Message, "CRITICAL", "Error!")
        End Try
    End Sub

    Private Sub TxtTargetPanel3DEC2000DG_TextChanged(sender As Object, e As EventArgs) Handles TxtTargetPanel3DEC2000DG.TextChanged
        Try
            If TxtTargetPanel3DEC2000DG.Text <> "" Then
                If IsNumeric(TxtTargetPanel3DEC2000DG.Text) = False Then
                    ShowMessage("Not a number!", "CRITICAL", "Incorrect input...")
                    Exit Sub
                End If
            End If
        Catch ex As Exception
            ShowMessage("TxtTargetPanel3DEC2000DG_TextChanged: " + ex.Message, "CRITICAL", "Error!")
        End Try
    End Sub

    Private Sub TxtTargetPanel4DEC2000DG_TextChanged(sender As Object, e As EventArgs) Handles TxtTargetPanel4DEC2000DG.TextChanged
        Try
            If TxtTargetPanel4DEC2000DG.Text <> "" Then
                If IsNumeric(TxtTargetPanel4DEC2000DG.Text) = False Then
                    ShowMessage("Not a number!", "CRITICAL", "Incorrect input...")
                    Exit Sub
                End If
            End If
        Catch ex As Exception
            ShowMessage("TxtTargetPanel4DEC2000DG_TextChanged: " + ex.Message, "CRITICAL", "Error!")
        End Try
    End Sub

    Private Sub TxtTargetPanel1DEC2000MM_TextChanged(sender As Object, e As EventArgs) Handles TxtTargetPanel1DEC2000MM.TextChanged
        Try
            Dim ciClone As CultureInfo = CType(CultureInfo.InvariantCulture.Clone(), CultureInfo)
            ciClone.NumberFormat.NumberDecimalSeparator = "."

            If TxtTargetPanel1DEC2000MM.Text <> "" Then
                If IsNumeric(TxtTargetPanel1DEC2000MM.Text) = False Then
                    ShowMessage("Not a number between 0 and 60!", "CRITICAL", "Incorrect input...")
                    Exit Sub
                End If
                If Double.Parse(TxtTargetPanel1DEC2000MM.Text, ciClone) < 0 Or Double.Parse(TxtTargetPanel1DEC2000MM.Text, ciClone) > 60 Then
                    ShowMessage("Not a number between 0 and 60!", "CRITICAL", "Incorrect input...")
                    Exit Sub
                End If
            End If
        Catch ex As Exception
            ShowMessage("TxtTargetPanel1DEC2000MM_TextChanged: " + ex.Message, "CRITICAL", "Error!")
        End Try
    End Sub

    Private Sub TxtTargetPanel2DEC2000MM_TextChanged(sender As Object, e As EventArgs) Handles TxtTargetPanel2DEC2000MM.TextChanged
        Try
            Dim ciClone As CultureInfo = CType(CultureInfo.InvariantCulture.Clone(), CultureInfo)
            ciClone.NumberFormat.NumberDecimalSeparator = "."

            If TxtTargetPanel2DEC2000MM.Text <> "" Then
                If IsNumeric(TxtTargetPanel2DEC2000MM.Text) = False Then
                    ShowMessage("Not a number between 0 and 60!", "CRITICAL", "Incorrect input...")
                    Exit Sub
                End If
                If Double.Parse(TxtTargetPanel2DEC2000MM.Text, ciClone) < 0 Or Double.Parse(TxtTargetPanel2DEC2000MM.Text, ciClone) > 60 Then
                    ShowMessage("Not a number between 0 and 60!", "CRITICAL", "Incorrect input...")
                    Exit Sub
                End If
            End If
        Catch ex As Exception
            ShowMessage("TxtTargetDEC2000MM_TextChanged: " + ex.Message, "CRITICAL", "Error!")
        End Try
    End Sub

    Private Sub TxtTargetPanel3DEC2000MM_TextChanged(sender As Object, e As EventArgs) Handles TxtTargetPanel3DEC2000MM.TextChanged
        Try
            Dim ciClone As CultureInfo = CType(CultureInfo.InvariantCulture.Clone(), CultureInfo)
            ciClone.NumberFormat.NumberDecimalSeparator = "."

            If TxtTargetPanel3DEC2000MM.Text <> "" Then
                If IsNumeric(TxtTargetPanel3DEC2000MM.Text) = False Then
                    ShowMessage("Not a number between 0 and 60!", "CRITICAL", "Incorrect input...")
                    Exit Sub
                End If
                If Double.Parse(TxtTargetPanel3DEC2000MM.Text, ciClone) < 0 Or Double.Parse(TxtTargetPanel3DEC2000MM.Text, ciClone) > 60 Then
                    ShowMessage("Not a number between 0 and 60!", "CRITICAL", "Incorrect input...")
                    Exit Sub
                End If
            End If
        Catch ex As Exception
            ShowMessage("TxtTargetPanel3DEC2000MM_TextChanged: " + ex.Message, "CRITICAL", "Error!")
        End Try
    End Sub

    Private Sub TxtTargetPanel4DEC2000MM_TextChanged(sender As Object, e As EventArgs) Handles TxtTargetPanel4DEC2000MM.TextChanged
        Try
            Dim ciClone As CultureInfo = CType(CultureInfo.InvariantCulture.Clone(), CultureInfo)
            ciClone.NumberFormat.NumberDecimalSeparator = "."

            If TxtTargetPanel4DEC2000MM.Text <> "" Then
                If IsNumeric(TxtTargetPanel4DEC2000MM.Text) = False Then
                    ShowMessage("Not a number between 0 and 60!", "CRITICAL", "Incorrect input...")
                    Exit Sub
                End If
                If Double.Parse(TxtTargetPanel4DEC2000MM.Text, ciClone) < 0 Or Double.Parse(TxtTargetPanel4DEC2000MM.Text, ciClone) > 60 Then
                    ShowMessage("Not a number between 0 and 60!", "CRITICAL", "Incorrect input...")
                    Exit Sub
                End If
            End If
        Catch ex As Exception
            ShowMessage("TxtTargetPanel4DEC2000MM_TextChanged: " + ex.Message, "CRITICAL", "Error!")
        End Try
    End Sub

    Private Sub TxtTargetPanel1DEC2000SS_TextChanged(sender As Object, e As EventArgs) Handles TxtTargetPanel1DEC2000SS.TextChanged
        Try
            Dim ciClone As CultureInfo = CType(CultureInfo.InvariantCulture.Clone(), CultureInfo)
            ciClone.NumberFormat.NumberDecimalSeparator = "."

            If TxtTargetPanel1DEC2000SS.Text <> "" Then
                If IsNumeric(TxtTargetPanel1DEC2000SS.Text) = False Then
                    ShowMessage("Not a number between 0 and 60!", "CRITICAL", "Incorrect input...")
                    Exit Sub
                End If
                If Double.Parse(TxtTargetPanel1DEC2000SS.Text, ciClone) < 0 Or Double.Parse(TxtTargetPanel1DEC2000SS.Text, ciClone) > 60 Then
                    ShowMessage("Not a number between 0 and 60!", "CRITICAL", "Incorrect input...")
                    Exit Sub
                End If
            End If
        Catch ex As Exception
            ShowMessage("TxtTargetPanel1DEC2000SS_TextChanged: " + ex.Message, "CRITICAL", "Error!")
        End Try
    End Sub

    Private Sub TxtTargetPanel2DEC2000SS_TextChanged(sender As Object, e As EventArgs) Handles TxtTargetPanel2DEC2000SS.TextChanged
        Try
            Dim ciClone As CultureInfo = CType(CultureInfo.InvariantCulture.Clone(), CultureInfo)
            ciClone.NumberFormat.NumberDecimalSeparator = "."

            If TxtTargetPanel2DEC2000SS.Text <> "" Then
                If IsNumeric(TxtTargetPanel2DEC2000SS.Text) = False Then
                    ShowMessage("Not a number between 0 and 60!", "CRITICAL", "Incorrect input...")
                    Exit Sub
                End If
                If Double.Parse(TxtTargetPanel2DEC2000SS.Text, ciClone) < 0 Or Double.Parse(TxtTargetPanel2DEC2000SS.Text, ciClone) > 60 Then
                    ShowMessage("Not a number between 0 and 60!", "CRITICAL", "Incorrect input...")
                    Exit Sub
                End If
            End If
        Catch ex As Exception
            ShowMessage("TxtTargetPanel2DEC2000SS_TextChanged: " + ex.Message, "CRITICAL", "Error!")
        End Try
    End Sub

    Private Sub TxtTargetPanel3DEC2000SS_TextChanged(sender As Object, e As EventArgs) Handles TxtTargetPanel3DEC2000SS.TextChanged
        Try
            Dim ciClone As CultureInfo = CType(CultureInfo.InvariantCulture.Clone(), CultureInfo)
            ciClone.NumberFormat.NumberDecimalSeparator = "."

            If TxtTargetPanel3DEC2000SS.Text <> "" Then
                If IsNumeric(TxtTargetPanel3DEC2000SS.Text) = False Then
                    ShowMessage("Not a number between 0 and 60!", "CRITICAL", "Incorrect input...")
                    Exit Sub
                End If
                If Double.Parse(TxtTargetPanel3DEC2000SS.Text, ciClone) < 0 Or Double.Parse(TxtTargetPanel3DEC2000SS.Text, ciClone) > 60 Then
                    ShowMessage("Not a number between 0 and 60!", "CRITICAL", "Incorrect input...")
                    Exit Sub
                End If
            End If
        Catch ex As Exception
            ShowMessage("TxtTargetPanel3DEC2000SS_TextChanged: " + ex.Message, "CRITICAL", "Error!")
        End Try
    End Sub

    Private Sub TxtTargetPanel4DEC2000SS_TextChanged(sender As Object, e As EventArgs) Handles TxtTargetPanel4DEC2000SS.TextChanged
        Try
            Dim ciClone As CultureInfo = CType(CultureInfo.InvariantCulture.Clone(), CultureInfo)
            ciClone.NumberFormat.NumberDecimalSeparator = "."

            If TxtTargetPanel4DEC2000SS.Text <> "" Then
                If IsNumeric(TxtTargetPanel4DEC2000SS.Text) = False Then
                    ShowMessage("Not a number between 0 and 60!", "CRITICAL", "Incorrect input...")
                    Exit Sub
                End If
                If Double.Parse(TxtTargetPanel4DEC2000SS.Text, ciClone) < 0 Or Double.Parse(TxtTargetPanel4DEC2000SS.Text, ciClone) > 60 Then
                    ShowMessage("Not a number between 0 and 60!", "CRITICAL", "Incorrect input...")
                    Exit Sub
                End If
            End If
        Catch ex As Exception
            ShowMessage("TxtTargetPanel4DEC2000SS_TextChanged: " + ex.Message, "CRITICAL", "Error!")
        End Try
    End Sub

    Private Sub TxtTargetPanel1NbrExposedFrames_TextChanged(sender As Object, e As EventArgs) Handles TxtTargetPanel1NbrExposedFrames.TextChanged
        Try
            Dim ciClone As CultureInfo = CType(CultureInfo.InvariantCulture.Clone(), CultureInfo)
            ciClone.NumberFormat.NumberDecimalSeparator = "."

            If TxtTargetPanel1NbrExposedFrames.Text <> "" Then
                If IsNumeric(TxtTargetPanel1NbrExposedFrames.Text) = False Then
                    ShowMessage("Not a number between 0 and 9999!", "CRITICAL", "Incorrect input...")
                    Exit Sub
                End If
                If Double.Parse(TxtTargetPanel1NbrExposedFrames.Text, ciClone) < 0 Or Double.Parse(TxtTargetPanel1NbrExposedFrames.Text, ciClone) > 9999 Then
                    ShowMessage("Not a number between 0 and 9999!", "CRITICAL", "Incorrect input...")
                    Exit Sub
                End If


                If ChkTargetMosaic.Checked = True Then
                    Dim panel1, panel2, panel3, panel4 As Integer

                    If (TxtTargetPanel1NbrExposedFrames.Text) = "" Then
                        panel1 = 0
                    Else
                        panel1 = CInt(TxtTargetPanel1NbrExposedFrames.Text)
                    End If

                    If (TxtTargetPanel2NbrExposedFrames.Text) = "" Then
                        panel2 = 0
                    Else
                        panel2 = CInt(TxtTargetPanel2NbrExposedFrames.Text)
                    End If

                    If (TxtTargetPanel3NbrExposedFrames.Text) = "" Then
                        panel3 = 0
                    Else
                        panel3 = CInt(TxtTargetPanel3NbrExposedFrames.Text)
                    End If

                    If (TxtTargetPanel4NbrExposedFrames.Text) = "" Then
                        panel4 = 0
                    Else
                        panel4 = CInt(TxtTargetPanel4NbrExposedFrames.Text)
                    End If

                    TxtTargetNbrExposedFrames.Text = CStr(panel1 + panel2 + panel3 + panel4)
                End If
            End If

        Catch ex As Exception
            ShowMessage("TxtTargetPanel1NbrExposedFrames_TextChanged: " + ex.Message, "CRITICAL", "Error!")
        End Try
    End Sub

    Private Sub TxtTargetPanel2NbrExposedFrames_TextChanged(sender As Object, e As EventArgs) Handles TxtTargetPanel2NbrExposedFrames.TextChanged
        Try
            Dim ciClone As CultureInfo = CType(CultureInfo.InvariantCulture.Clone(), CultureInfo)
            ciClone.NumberFormat.NumberDecimalSeparator = "."

            If TxtTargetPanel2NbrExposedFrames.Text <> "" Then
                If IsNumeric(TxtTargetPanel2NbrExposedFrames.Text) = False Then
                    ShowMessage("Not a number between 0 and 9999!", "CRITICAL", "Incorrect input...")
                    Exit Sub
                End If
                If Double.Parse(TxtTargetPanel2NbrExposedFrames.Text, ciClone) < 0 Or Double.Parse(TxtTargetPanel2NbrExposedFrames.Text, ciClone) > 9999 Then
                    ShowMessage("Not a number between 0 and 9999!", "CRITICAL", "Incorrect input...")
                    Exit Sub
                End If


                If ChkTargetMosaic.Checked = True Then
                    Dim panel1, panel2, panel3, panel4 As Integer

                    If (TxtTargetPanel1NbrExposedFrames.Text) = "" Then
                        panel1 = 0
                    Else
                        panel1 = CInt(TxtTargetPanel1NbrExposedFrames.Text)
                    End If

                    If (TxtTargetPanel2NbrExposedFrames.Text) = "" Then
                        panel2 = 0
                    Else
                        panel2 = CInt(TxtTargetPanel2NbrExposedFrames.Text)
                    End If

                    If (TxtTargetPanel3NbrExposedFrames.Text) = "" Then
                        panel3 = 0
                    Else
                        panel3 = CInt(TxtTargetPanel3NbrExposedFrames.Text)
                    End If

                    If (TxtTargetPanel4NbrExposedFrames.Text) = "" Then
                        panel4 = 0
                    Else
                        panel4 = CInt(TxtTargetPanel4NbrExposedFrames.Text)
                    End If

                    TxtTargetNbrExposedFrames.Text = CStr(panel1 + panel2 + panel3 + panel4)
                End If

            End If
        Catch ex As Exception
            ShowMessage("TxtTargetPanel2NbrExposedFrames_TextChanged: " + ex.Message, "CRITICAL", "Error!")
        End Try
    End Sub

    Private Sub TxtTargetPanel3NbrExposedFrames_TextChanged(sender As Object, e As EventArgs) Handles TxtTargetPanel3NbrExposedFrames.TextChanged
        Try
            Dim ciClone As CultureInfo = CType(CultureInfo.InvariantCulture.Clone(), CultureInfo)
            ciClone.NumberFormat.NumberDecimalSeparator = "."

            If TxtTargetPanel3NbrExposedFrames.Text <> "" Then
                If IsNumeric(TxtTargetPanel3NbrExposedFrames.Text) = False Then
                    ShowMessage("Not a number between 0 and 9999!", "CRITICAL", "Incorrect input...")
                    Exit Sub
                End If
                If Double.Parse(TxtTargetPanel3NbrExposedFrames.Text, ciClone) < 0 Or Double.Parse(TxtTargetPanel3NbrExposedFrames.Text, ciClone) > 9999 Then
                    ShowMessage("Not a number between 0 and 9999!", "CRITICAL", "Incorrect input...")
                    Exit Sub
                End If

                If ChkTargetMosaic.Checked = True Then
                    Dim panel1, panel2, panel3, panel4 As Integer

                    If (TxtTargetPanel1NbrExposedFrames.Text) = "" Then
                        panel1 = 0
                    Else
                        panel1 = CInt(TxtTargetPanel1NbrExposedFrames.Text)
                    End If

                    If (TxtTargetPanel2NbrExposedFrames.Text) = "" Then
                        panel2 = 0
                    Else
                        panel2 = CInt(TxtTargetPanel2NbrExposedFrames.Text)
                    End If

                    If (TxtTargetPanel3NbrExposedFrames.Text) = "" Then
                        panel3 = 0
                    Else
                        panel3 = CInt(TxtTargetPanel3NbrExposedFrames.Text)
                    End If

                    If (TxtTargetPanel4NbrExposedFrames.Text) = "" Then
                        panel4 = 0
                    Else
                        panel4 = CInt(TxtTargetPanel4NbrExposedFrames.Text)
                    End If

                    TxtTargetNbrExposedFrames.Text = CStr(panel1 + panel2 + panel3 + panel4)
                End If
            End If
        Catch ex As Exception
            ShowMessage("TxtTargetPanel3NbrExposedFrames: " + ex.Message, "CRITICAL", "Error!")
        End Try
    End Sub

    Private Sub TxtTargetPanel4NbrExposedFrames_TextChanged(sender As Object, e As EventArgs) Handles TxtTargetPanel4NbrExposedFrames.TextChanged
        Try
            Dim ciClone As CultureInfo = CType(CultureInfo.InvariantCulture.Clone(), CultureInfo)
            ciClone.NumberFormat.NumberDecimalSeparator = "."

            If TxtTargetPanel4NbrExposedFrames.Text <> "" Then
                If IsNumeric(TxtTargetPanel4NbrExposedFrames.Text) = False Then
                    ShowMessage("Not a number between 0 and 9999!", "CRITICAL", "Incorrect input...")
                    Exit Sub
                End If
                If Double.Parse(TxtTargetPanel4NbrExposedFrames.Text, ciClone) < 0 Or Double.Parse(TxtTargetPanel4NbrExposedFrames.Text, ciClone) > 9999 Then
                    ShowMessage("Not a number between 0 and 9999!", "CRITICAL", "Incorrect input...")
                    Exit Sub
                End If


                If ChkTargetMosaic.Checked = True Then
                    Dim panel1, panel2, panel3, panel4 As Integer

                    If (TxtTargetPanel1NbrExposedFrames.Text) = "" Then
                        panel1 = 0
                    Else
                        panel1 = CInt(TxtTargetPanel1NbrExposedFrames.Text)
                    End If

                    If (TxtTargetPanel2NbrExposedFrames.Text) = "" Then
                        panel2 = 0
                    Else
                        panel2 = CInt(TxtTargetPanel2NbrExposedFrames.Text)
                    End If

                    If (TxtTargetPanel3NbrExposedFrames.Text) = "" Then
                        panel3 = 0
                    Else
                        panel3 = CInt(TxtTargetPanel3NbrExposedFrames.Text)
                    End If

                    If (TxtTargetPanel4NbrExposedFrames.Text) = "" Then
                        panel4 = 0
                    Else
                        panel4 = CInt(TxtTargetPanel4NbrExposedFrames.Text)
                    End If

                    TxtTargetNbrExposedFrames.Text = CStr(panel1 + panel2 + panel3 + panel4)
                End If
            End If
        Catch ex As Exception
            ShowMessage("TxtTargetPanel4NbrExposedFrames_TextChanged: " + ex.Message, "CRITICAL", "Error!")
        End Try
    End Sub

    Private Sub TxtTargetMosaicFramesPerPanel_TextChanged(sender As Object, e As EventArgs) Handles TxtTargetMosaicFramesPerPanel.TextChanged
        Try
            Dim ciClone As CultureInfo = CType(CultureInfo.InvariantCulture.Clone(), CultureInfo)
            ciClone.NumberFormat.NumberDecimalSeparator = "."

            If TxtTargetMosaicFramesPerPanel.Text <> "" Then
                If IsNumeric(TxtTargetMosaicFramesPerPanel.Text) = False Then
                    ShowMessage("Not a number between 0 and 9999!", "CRITICAL", "Incorrect input...")
                    Exit Sub
                End If
                If Double.Parse(TxtTargetMosaicFramesPerPanel.Text, ciClone) < 0 Or Double.Parse(TxtTargetMosaicFramesPerPanel.Text, ciClone) > 9999 Then
                    ShowMessage("Not a number between 0 and 9999!", "CRITICAL", "Incorrect input...")
                    Exit Sub
                End If
            End If

        Catch ex As Exception
            ShowMessage("TxtTargetMosaicFramesPerPanel_TextChanged: " + ex.Message, "CRITICAL", "Error!")
        End Try
    End Sub

    Private Sub TxtTargetMosaicOverlap_TextChanged(sender As Object, e As EventArgs) Handles TxtTargetMosaicOverlap.TextChanged
        Try
            Dim ciClone As CultureInfo = CType(CultureInfo.InvariantCulture.Clone(), CultureInfo)
            ciClone.NumberFormat.NumberDecimalSeparator = "."

            If TxtTargetMosaicOverlap.Text <> "" Then
                If IsNumeric(TxtTargetMosaicOverlap.Text) = False Then
                    ShowMessage("Not a number between 0 and 9999!", "CRITICAL", "Incorrect input...")
                    Exit Sub
                End If
                If Double.Parse(TxtTargetMosaicOverlap.Text, ciClone) < 0 Or Double.Parse(TxtTargetMosaicOverlap.Text, ciClone) > 9999 Then
                    ShowMessage("Not a number between 0 and 9999!", "CRITICAL", "Incorrect input...")
                    Exit Sub
                End If
            End If
        Catch ex As Exception
            ShowMessage("TxtTargetMosaicOverlap_TextChanged: " + ex.Message, "CRITICAL", "Error!")
        End Try
    End Sub

    Private Sub BtnClear_Click(sender As Object, e As EventArgs) Handles BtnClear.Click
        TxtTargetPanel1RA2000HH.Text = ""
        TxtTargetPanel2RA2000HH.Text = ""
        TxtTargetPanel3RA2000HH.Text = ""
        TxtTargetPanel4RA2000HH.Text = ""
        TxtTargetPanel1RA2000MM.Text = ""
        TxtTargetPanel2RA2000MM.Text = ""
        TxtTargetPanel3RA2000MM.Text = ""
        TxtTargetPanel4RA2000MM.Text = ""
        TxtTargetPanel1RA2000SS.Text = ""
        TxtTargetPanel2RA2000SS.Text = ""
        TxtTargetPanel3RA2000SS.Text = ""
        TxtTargetPanel4RA2000SS.Text = ""
        TxtTargetPanel1DEC2000DG.Text = ""
        TxtTargetPanel2DEC2000DG.Text = ""
        TxtTargetPanel3DEC2000DG.Text = ""
        TxtTargetPanel4DEC2000DG.Text = ""
        TxtTargetPanel1DEC2000MM.Text = ""
        TxtTargetPanel2DEC2000MM.Text = ""
        TxtTargetPanel3DEC2000MM.Text = ""
        TxtTargetPanel4DEC2000MM.Text = ""
        TxtTargetPanel1DEC2000SS.Text = ""
        TxtTargetPanel2DEC2000SS.Text = ""
        TxtTargetPanel3DEC2000SS.Text = ""
        TxtTargetPanel4DEC2000SS.Text = ""
        TxtTargetPanel1NbrExposedFrames.Text = "0"
        TxtTargetPanel2NbrExposedFrames.Text = "0"
        TxtTargetPanel3NbrExposedFrames.Text = "0"
        TxtTargetPanel4NbrExposedFrames.Text = "0"
    End Sub



    Private Sub BtnCalculateMosaic_Click(sender As Object, e As EventArgs) Handles BtnCalculateMosaic.Click
        Dim returnvalue As String
        Dim RA2000, DEC2000 As Double
        Dim RA_string, DEC_string As String
        Dim index, old_index As Integer

        Try
            TxtTargetPanel1RA2000HH.Text = ""
            TxtTargetPanel2RA2000HH.Text = ""
            TxtTargetPanel3RA2000HH.Text = ""
            TxtTargetPanel4RA2000HH.Text = ""
            TxtTargetPanel1RA2000MM.Text = ""
            TxtTargetPanel2RA2000MM.Text = ""
            TxtTargetPanel3RA2000MM.Text = ""
            TxtTargetPanel4RA2000MM.Text = ""
            TxtTargetPanel1RA2000SS.Text = ""
            TxtTargetPanel2RA2000SS.Text = ""
            TxtTargetPanel3RA2000SS.Text = ""
            TxtTargetPanel4RA2000SS.Text = ""
            TxtTargetPanel1DEC2000DG.Text = ""
            TxtTargetPanel2DEC2000DG.Text = ""
            TxtTargetPanel3DEC2000DG.Text = ""
            TxtTargetPanel4DEC2000DG.Text = ""
            TxtTargetPanel1DEC2000MM.Text = ""
            TxtTargetPanel2DEC2000MM.Text = ""
            TxtTargetPanel3DEC2000MM.Text = ""
            TxtTargetPanel4DEC2000MM.Text = ""
            TxtTargetPanel1DEC2000SS.Text = ""
            TxtTargetPanel2DEC2000SS.Text = ""
            TxtTargetPanel3DEC2000SS.Text = ""
            TxtTargetPanel4DEC2000SS.Text = ""

            If CmbTargetMosaicType.Text = "" Then
                ShowMessage("Mosaic type is not filled in!", "CRITICAL", "Incorrect input...")
            Else
                If TxtTargetMosaicOverlap.Text = "" Then
                    ShowMessage("Mosaic overlap is not filled in!", "CRITICAL", "Incorrect input...")
                Else
                    'calculate the numbers
                    If CmbTargetMosaicType.Text = "2x2" Then
                        TxtTargetNbrFrames.Text = CStr(CInt(TxtTargetMosaicFramesPerPanel.Text) * 4)
                    ElseIf CmbTargetMosaicType.Text = "1x2" Then
                        TxtTargetNbrFrames.Text = CStr(CInt(TxtTargetMosaicFramesPerPanel.Text) * 2)
                    ElseIf CmbTargetMosaicType.Text = "2x1" Then
                        TxtTargetNbrFrames.Text = CStr(CInt(TxtTargetMosaicFramesPerPanel.Text) * 2)
                    ElseIf CmbTargetMosaicType.Text = "" Then
                        TxtTargetNbrFrames.Text = "500"
                    End If
                    TxtTargetNbrExposedFrames.Text = "0"

                    'RA2000 and DEC2000
                    RA2000 = pAUtil.HMSToHours(TxtTargetRA2000HH.Text + " " + TxtTargetRA2000MM.Text + " " + TxtTargetRA2000SS.Text)
                    DEC2000 = pAUtil.DMSToDegrees(TxtTargetDEC2000DG.Text + " " + TxtTargetDEC2000MM.Text + " " + TxtTargetDEC2000SS.Text)

                    Dim ciClone As CultureInfo = CType(CultureInfo.InvariantCulture.Clone(), CultureInfo)
                    ciClone.NumberFormat.NumberDecimalSeparator = "."

                    returnvalue = CalculateMosaic(RA2000, DEC2000, CmbTargetMosaicType.Text, Convert.ToInt32(TxtTargetMosaicOverlap.Text))

                    'Panel1: convert values to HH MM DD and DD MM SS
                    If pStructMosaic.Panel1_RA2000 <> 0 Then
                        RA_string = pAUtil.HoursToHMS(pStructMosaic.Panel1_RA2000)
                        DEC_string = pAUtil.DegreesToDMS(pStructMosaic.Panel1_DEC2000)

                        index = GetNthIndex(RA_string, ":"c, 1)
                        TxtTargetPanel1RA2000HH.Text = RA_string.Substring(0, index)
                        old_index = index
                        index = GetNthIndex(RA_string, ":"c, 2)
                        TxtTargetPanel1RA2000MM.Text = RA_string.Substring(old_index + 1, index - old_index - 1)
                        TxtTargetPanel1RA2000SS.Text = RA_string.Substring(index + 1, 2)

                        index = GetNthIndex(DEC_string, "°"c, 1)
                        TxtTargetPanel1DEC2000DG.Text = DEC_string.Substring(0, index)
                        TxtTargetPanel1DEC2000MM.Text = DEC_string.Substring(index + 2, 2)
                        index = GetNthIndex(DEC_string, "'"c, 1)
                        TxtTargetPanel1DEC2000SS.Text = DEC_string.Substring(index + 2, 2)
                    End If

                    'Panel2: convert values to HH MM DD and DD MM SS
                    If pStructMosaic.Panel2_RA2000 <> 0 Then
                        RA_string = pAUtil.HoursToHMS(pStructMosaic.Panel2_RA2000)
                        DEC_string = pAUtil.DegreesToDMS(pStructMosaic.Panel2_DEC2000)

                        index = GetNthIndex(RA_string, ":"c, 1)
                        TxtTargetPanel2RA2000HH.Text = RA_string.Substring(0, index)
                        old_index = index
                        index = GetNthIndex(RA_string, ":"c, 2)
                        TxtTargetPanel2RA2000MM.Text = RA_string.Substring(old_index + 1, index - old_index - 1)
                        TxtTargetPanel2RA2000SS.Text = RA_string.Substring(index + 1, 2)

                        index = GetNthIndex(DEC_string, "°"c, 1)
                        TxtTargetPanel2DEC2000DG.Text = DEC_string.Substring(0, index)
                        TxtTargetPanel2DEC2000MM.Text = DEC_string.Substring(index + 2, 2)
                        index = GetNthIndex(DEC_string, "'"c, 1)
                        TxtTargetPanel2DEC2000SS.Text = DEC_string.Substring(index + 2, 2)
                    End If


                    'Panel3: convert values to HH MM DD and DD MM SS
                    If pStructMosaic.Panel3_RA2000 <> 0 Then
                        RA_string = pAUtil.HoursToHMS(pStructMosaic.Panel3_RA2000)
                        DEC_string = pAUtil.DegreesToDMS(pStructMosaic.Panel3_DEC2000)

                        index = GetNthIndex(RA_string, ":"c, 1)
                        TxtTargetPanel3RA2000HH.Text = RA_string.Substring(0, index)
                        old_index = index
                        index = GetNthIndex(RA_string, ":"c, 2)
                        TxtTargetPanel3RA2000MM.Text = RA_string.Substring(old_index + 1, index - old_index - 1)
                        TxtTargetPanel3RA2000SS.Text = RA_string.Substring(index + 1, 2)

                        index = GetNthIndex(DEC_string, "°"c, 1)
                        TxtTargetPanel3DEC2000DG.Text = DEC_string.Substring(0, index)
                        TxtTargetPanel3DEC2000MM.Text = DEC_string.Substring(index + 2, 2)
                        index = GetNthIndex(DEC_string, "'"c, 1)
                        TxtTargetPanel3DEC2000SS.Text = DEC_string.Substring(index + 2, 2)
                    End If

                    'Panel4: convert values to HH MM DD and DD MM SS
                    If pStructMosaic.Panel4_RA2000 <> 0 Then
                        RA_string = pAUtil.HoursToHMS(pStructMosaic.Panel4_RA2000)
                        DEC_string = pAUtil.DegreesToDMS(pStructMosaic.Panel4_DEC2000)

                        index = GetNthIndex(RA_string, ":"c, 1)
                        TxtTargetPanel4RA2000HH.Text = RA_string.Substring(0, index)
                        old_index = index
                        index = GetNthIndex(RA_string, ":"c, 2)
                        TxtTargetPanel4RA2000MM.Text = RA_string.Substring(old_index + 1, index - old_index - 1)
                        TxtTargetPanel4RA2000SS.Text = RA_string.Substring(index + 1, 2)

                        index = GetNthIndex(DEC_string, "°"c, 1)
                        TxtTargetPanel4DEC2000DG.Text = DEC_string.Substring(0, index)
                        TxtTargetPanel4DEC2000MM.Text = DEC_string.Substring(index + 2, 2)
                        index = GetNthIndex(DEC_string, "'"c, 1)
                        TxtTargetPanel4DEC2000SS.Text = DEC_string.Substring(index + 2, 2)
                    End If


                End If
            End If
        Catch ex As Exception
            ShowMessage("BtnCalculateMosaic_Click: " + ex.Message, "CRITICAL", "Error!")
        End Try
    End Sub

    Private Sub CmbTargetMosaicType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CmbTargetMosaicType.SelectedIndexChanged
        If CmbTargetMosaicType.Text = "2x2" Then
            LabelMosaic1.Text = "1"
            LabelMosaic2.Text = "2"
            LabelMosaic3.Text = "3"
            LabelMosaic4.Text = "4"
            LabelMosaic1.BorderStyle = BorderStyle.FixedSingle
            LabelMosaic2.BorderStyle = BorderStyle.FixedSingle
            LabelMosaic3.BorderStyle = BorderStyle.FixedSingle
            LabelMosaic4.BorderStyle = BorderStyle.FixedSingle
        ElseIf CmbTargetMosaicType.Text = "1x2" Then
            LabelMosaic1.Text = "1"
            LabelMosaic2.Text = ""
            LabelMosaic3.Text = "2"
            LabelMosaic4.Text = ""
            LabelMosaic1.BorderStyle = BorderStyle.FixedSingle
            LabelMosaic2.BorderStyle = BorderStyle.None
            LabelMosaic3.BorderStyle = BorderStyle.FixedSingle
            LabelMosaic4.BorderStyle = BorderStyle.None
        ElseIf CmbTargetMosaicType.Text = "2x1" Then
            LabelMosaic1.Text = "1"
            LabelMosaic2.Text = "2"
            LabelMosaic3.Text = ""
            LabelMosaic4.Text = ""
            LabelMosaic1.BorderStyle = BorderStyle.FixedSingle
            LabelMosaic2.BorderStyle = BorderStyle.FixedSingle
            LabelMosaic3.BorderStyle = BorderStyle.None
            LabelMosaic4.BorderStyle = BorderStyle.None
        ElseIf CmbTargetMosaicType.Text = "" Then
            LabelMosaic1.Text = ""
            LabelMosaic2.Text = ""
            LabelMosaic3.Text = ""
            LabelMosaic4.Text = ""
            LabelMosaic1.BorderStyle = BorderStyle.None
            LabelMosaic2.BorderStyle = BorderStyle.None
            LabelMosaic3.BorderStyle = BorderStyle.None
            LabelMosaic4.BorderStyle = BorderStyle.None
        End If
    End Sub

    Private Sub TxtTargetMosaicFramesPerPanel_Validated(sender As Object, e As EventArgs) Handles TxtTargetMosaicFramesPerPanel.Validated

        'recalculate total number of frames needed
        If CmbTargetMosaicType.Text = "2x2" Then
            TxtTargetNbrFrames.Text = CStr(CInt(TxtTargetMosaicFramesPerPanel.Text) * 4)
        ElseIf CmbTargetMosaicType.Text = "1x2" Then
            TxtTargetNbrFrames.Text = CStr(CInt(TxtTargetMosaicFramesPerPanel.Text) * 2)
        ElseIf CmbTargetMosaicType.Text = "2x1" Then
            TxtTargetNbrFrames.Text = CStr(CInt(TxtTargetMosaicFramesPerPanel.Text) * 2)
        ElseIf CmbTargetMosaicType.Text = "" Then
            TxtTargetNbrFrames.Text = ""
        End If
    End Sub

    Private Sub ChkTargetIsComet_CheckedChanged(sender As Object, e As EventArgs) Handles ChkTargetIsComet.CheckedChanged
        'no mosaic possible when target is comet
        If ChkTargetIsComet.Checked = True Then
            ChkTargetMosaic.Enabled = False
        Else
            ChkTargetMosaic.Enabled = True
        End If
    End Sub

    Private Sub DataGridViewTarget_Scroll(sender As Object, e As ScrollEventArgs) Handles DataGridViewTarget.Scroll
        Dim TargetName, TargetFilter As String
        Dim returnvalue As String
        Dim cmd As New OleDb.OleDbCommand

        Try
            '-------------------------------------------------------------------------------------------------------------
            'check for changes
            '-------------------------------------------------------------------------------------------------------------
            InsertUpdateRecord()

            '-------------------------------------------------------------------------------------------------------------
            'change the row
            '-------------------------------------------------------------------------------------------------------------
            TargetName = DataGridViewTarget.CurrentRow.Cells(0).Value.ToString
            TargetFilter = DataGridViewTarget.CurrentRow.Cells(1).Value.ToString


            returnvalue = LoadRecord(TargetName, TargetFilter)
            If returnvalue <> "OK" Then
                Exit Sub
            End If

        Catch ex As Exception
            ShowMessage("DataGridViewTarget_CellClick: " + ex.Message, "CRITICAL", "Error!")
        End Try
    End Sub

End Class
