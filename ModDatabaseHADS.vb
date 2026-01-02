Imports System.Data.Entity
Imports System.Data.SqlClient
Imports System.Data.SQLite
Imports System.Diagnostics.Eventing
Imports System.Globalization
Imports System.Net
Imports System.Runtime.InteropServices.WindowsRuntime
Imports System.Windows.Forms.VisualStyles.VisualStyleElement.Button

Module ModDatabaseHADS
    'https://docs.google.com/spreadsheets/d/e/2PACX-1vSyphtUrAM-tldhj0d4EH6BAbyPrMbfPq4UZ5AANyacd9OL8WAY4Ef0mZ_xbpIC7Bj3GhJuv6oSMyAM/pub?gid=316386585&single=true&output=csv
    'https://docs.google.com/spreadsheets/d/e/2PACX-1vSyphtUrAM-tldhj0d4EH6BAbyPrMbfPq4UZ5AANyacd9OL8WAY4Ef0mZ_xbpIC7Bj3GhJuv6oSMyAM/pub?gid=316386585&single=true&output=tsv
    'strategy: load tsv from internet
    'load csv into database
    'change GID for the spreadsheet of the correct year !

    Public Structure StructHADSRecord
        Public ID As Integer
        Public HADSName As String
        Public HADSName2 As String
        Public HADSName3 As String
        Public HADSName4 As String
        Public HADSName5 As String
        Public HADSActive As Integer
        Public HADSMin As String
        Public HADSMax As String
        Public HADSPeriod As String
        Public HADSRA2000HH As String
        Public HADSRA2000MM As String
        Public HADSRA2000SS As String
        Public HADSDEC2000DG As String
        Public HADSDEC2000MM As String
        Public HADSDEC2000SS As String
        Public HADSExposure As Double
        Public HADSBin As String
        Public HADSFilter As String
        Public HADSJan As String
        Public HADSFeb As String
        Public HADSMar As String
        Public HADSApr As String
        Public HADSMay As String
        Public HADSJun As String
        Public HADSJul As String
        Public HADSAug As String
        Public HADSSep As String
        Public HADSOct As String
        Public HADSNov As String
        Public HADSDec As String
        Public HADSLastObserved As String
        Public FocusStarName As String
        Public FocusStarRA2000HH As String
        Public FocusStarRA2000MM As String
        Public FocusStarRA2000SS As String
        Public FocusStarDEC2000DG As String
        Public FocusStarDEC2000MM As String
        Public FocusStarDEC2000SS As String
        Public FocusStarExposure As Double
        Public HADSPriority As Integer
        Public HADSError As Integer
        Public HADSErrorText As String
        Public HADSAltitude As Double
        Public HADSAzimuth As Double
        Public HADSCompassIndex As Integer
        Public HADSCompassDirection As String
        Public HADSCircumpolar As String
        Public HADSCircumPriority As String
    End Structure

    Public pDtVSLTarget As New DataTable
    Public pStrucHADSRecord As StructHADSRecord
    Public pStrucOldHADSRecord As StructHADSRecord
    Public pVSLTarget As StructHADSRecord 'Deepsky target
    Public pHADSFailedAttempts As Integer

    Private Sub ClearHADSRecord()
        Dim startExecution As Date
        Dim executionTime As TimeSpan


        startExecution = DateTime.UtcNow()
        LogSessionEntry("DEBUG", "  ClearHADSRecord...", "", "ClearHADSRecord", "PROGRAM")
        pStrucHADSRecord.ID = Nothing
        pStrucHADSRecord.HADSName = Nothing
        pStrucHADSRecord.HADSName2 = Nothing
        pStrucHADSRecord.HADSName3 = Nothing
        pStrucHADSRecord.HADSName4 = Nothing
        pStrucHADSRecord.HADSName5 = Nothing
        pStrucHADSRecord.HADSActive = Nothing
        pStrucHADSRecord.HADSMin = Nothing
        pStrucHADSRecord.HADSMax = Nothing
        pStrucHADSRecord.HADSPeriod = Nothing
        pStrucHADSRecord.HADSRA2000HH = Nothing
        pStrucHADSRecord.HADSRA2000MM = Nothing
        pStrucHADSRecord.HADSRA2000SS = Nothing
        pStrucHADSRecord.HADSDEC2000DG = Nothing
        pStrucHADSRecord.HADSDEC2000MM = Nothing
        pStrucHADSRecord.HADSDEC2000SS = Nothing
        pStrucHADSRecord.HADSExposure = Nothing
        pStrucHADSRecord.HADSBin = Nothing
        pStrucHADSRecord.HADSFilter = Nothing
        pStrucHADSRecord.HADSJan = Nothing
        pStrucHADSRecord.HADSFeb = Nothing
        pStrucHADSRecord.HADSMar = Nothing
        pStrucHADSRecord.HADSApr = Nothing
        pStrucHADSRecord.HADSMay = Nothing
        pStrucHADSRecord.HADSJun = Nothing
        pStrucHADSRecord.HADSJul = Nothing
        pStrucHADSRecord.HADSAug = Nothing
        pStrucHADSRecord.HADSSep = Nothing
        pStrucHADSRecord.HADSOct = Nothing
        pStrucHADSRecord.HADSNov = Nothing
        pStrucHADSRecord.HADSDec = Nothing
        pStrucHADSRecord.HADSLastObserved = Nothing
        pStrucHADSRecord.FocusStarName = Nothing
        pStrucHADSRecord.FocusStarRA2000HH = Nothing
        pStrucHADSRecord.FocusStarRA2000MM = Nothing
        pStrucHADSRecord.FocusStarRA2000SS = Nothing
        pStrucHADSRecord.FocusStarDEC2000DG = Nothing
        pStrucHADSRecord.FocusStarDEC2000MM = Nothing
        pStrucHADSRecord.FocusStarDEC2000SS = Nothing
        pStrucHADSRecord.FocusStarExposure = Nothing
        pStrucHADSRecord.HADSPriority = Nothing
        pStrucHADSRecord.HADSError = Nothing
        pStrucHADSRecord.HADSErrorText = Nothing
        pStrucHADSRecord.HADSAltitude = Nothing
        pStrucHADSRecord.HADSAzimuth = Nothing
        pStrucHADSRecord.HADSCompassIndex = Nothing
        pStrucHADSRecord.HADSCompassDirection = Nothing
        pStrucHADSRecord.HADSCircumpolar = Nothing
        pStrucHADSRecord.HADSCircumPriority = Nothing

        'clear old record
        pStrucOldHADSRecord.ID = Nothing
        pStrucOldHADSRecord.HADSName = Nothing
        pStrucOldHADSRecord.HADSName2 = Nothing
        pStrucOldHADSRecord.HADSName3 = Nothing
        pStrucOldHADSRecord.HADSName4 = Nothing
        pStrucOldHADSRecord.HADSName5 = Nothing
        pStrucOldHADSRecord.HADSActive = Nothing
        pStrucOldHADSRecord.HADSMin = Nothing
        pStrucOldHADSRecord.HADSMax = Nothing
        pStrucOldHADSRecord.HADSPeriod = Nothing
        pStrucOldHADSRecord.HADSRA2000HH = Nothing
        pStrucOldHADSRecord.HADSRA2000MM = Nothing
        pStrucOldHADSRecord.HADSRA2000SS = Nothing
        pStrucOldHADSRecord.HADSDEC2000DG = Nothing
        pStrucOldHADSRecord.HADSDEC2000MM = Nothing
        pStrucOldHADSRecord.HADSDEC2000SS = Nothing
        pStrucOldHADSRecord.HADSExposure = Nothing
        pStrucOldHADSRecord.HADSBin = Nothing
        pStrucOldHADSRecord.HADSFilter = Nothing
        pStrucOldHADSRecord.HADSJan = Nothing
        pStrucOldHADSRecord.HADSFeb = Nothing
        pStrucOldHADSRecord.HADSMar = Nothing
        pStrucOldHADSRecord.HADSApr = Nothing
        pStrucOldHADSRecord.HADSMay = Nothing
        pStrucOldHADSRecord.HADSJun = Nothing
        pStrucOldHADSRecord.HADSJul = Nothing
        pStrucOldHADSRecord.HADSAug = Nothing
        pStrucOldHADSRecord.HADSSep = Nothing
        pStrucOldHADSRecord.HADSOct = Nothing
        pStrucOldHADSRecord.HADSNov = Nothing
        pStrucOldHADSRecord.HADSDec = Nothing
        pStrucOldHADSRecord.HADSLastObserved = Nothing
        pStrucOldHADSRecord.FocusStarName = Nothing
        pStrucOldHADSRecord.FocusStarRA2000HH = Nothing
        pStrucOldHADSRecord.FocusStarRA2000MM = Nothing
        pStrucOldHADSRecord.FocusStarRA2000SS = Nothing
        pStrucOldHADSRecord.FocusStarDEC2000DG = Nothing
        pStrucOldHADSRecord.FocusStarDEC2000MM = Nothing
        pStrucOldHADSRecord.FocusStarDEC2000SS = Nothing
        pStrucOldHADSRecord.FocusStarExposure = Nothing
        pStrucOldHADSRecord.HADSPriority = Nothing
        pStrucOldHADSRecord.HADSError = Nothing
        pStrucOldHADSRecord.HADSErrorText = Nothing
        pStrucOldHADSRecord.HADSAltitude = Nothing
        pStrucOldHADSRecord.HADSAzimuth = Nothing
        pStrucOldHADSRecord.HADSCompassIndex = Nothing
        pStrucOldHADSRecord.HADSCompassDirection = Nothing
        pStrucOldHADSRecord.HADSCircumpolar = Nothing
        pStrucOldHADSRecord.HADSCircumPriority = Nothing

        executionTime = DateTime.UtcNow() - startExecution
        LogSessionEntry("DEBUG", "  ClearHADSRecord: " + executionTime.ToString, "", "ClearHADSRecord", "PROGRAM")

    End Sub

    Public Function DownloadHADSFile() As String
        Dim startExecution As Date
        Dim executionTime As TimeSpan

        DownloadHADSFile = "OK"
        Try
            startExecution = DateTime.UtcNow()
            LogSessionEntry("DEBUG", "  DownloadHADSFile...", "", "DownloadHADSFile", "PROGRAM")

            Dim remoteUri As String = My.Settings.SHADSURL
            Dim fileName As String = My.Settings.sLogLocation + "HADS.tsv"
            'Dim password As String = "..."
            'Dim username As String = "..."

            Using client As New WebClient()

                '   client.Credentials = New NetworkCredential(username, password)
                client.DownloadFile(remoteUri, fileName)
            End Using

            executionTime = DateTime.UtcNow() - startExecution
            LogSessionEntry("DEBUG", "  DownloadHADSFile: " + executionTime.ToString, "", "DownloadHADSFile", "PROGRAM")

        Catch ex As Exception
            LogSessionEntry("ERROR", "DownloadHADSFile: " + ex.Message, "", "DownloadHADSFile", "PROGRAM")
            DownloadHADSFile = ex.Message
        End Try
    End Function

    Public Function ReadHADSFile() As String
        Dim currentRow As String()
        Dim currentRecord(25) As String
        Dim returnvalue As String
        Dim i As Integer
        Dim HADSNbrOfInsertedRecords As Integer
        Dim HADSNbrOfUpdatedRecords As Integer
        Dim startExecution As Date
        Dim executionTime As TimeSpan


        ReadHADSFile = "OK"
        Try
            startExecution = DateTime.UtcNow()
            LogSessionEntry("DEBUG", "  ReadHADSFile...", "", "ReadHADSFile", "PROGRAM")

            HADSNbrOfInsertedRecords = 0
            HADSNbrOfUpdatedRecords = 0

            DownloadHADSFile()

            Using MyReader As New Microsoft.VisualBasic.
                FileIO.TextFieldParser(My.Settings.sLogLocation + "HADS.tsv")
                MyReader.TextFieldType = FileIO.FieldType.Delimited
                MyReader.SetDelimiters(vbTab)
                i = 0

                'set all records inactive
                SetAllInactiveHADSRecord()

                While Not MyReader.EndOfData
                    Try
                        ClearHADSRecord()
                        currentRow = MyReader.ReadFields()
                        Dim currentField As String
                        i = 0
                        For Each currentField In currentRow
                            currentRecord(i) = currentField
                            i += 1
                        Next

                        'construct record, but only real records
                        If currentRecord(3) <> "" And currentRecord(0) <> "Star" Then
                            Dim temp As String
                            temp = currentRecord(0)
                            temp = temp.Replace(";", "")
                            temp = temp.Replace("(", "")
                            temp = temp.Replace(")", "")
                            temp = temp.Replace("?", "")
                            temp = temp.Replace(",", "")
                            temp = temp.Replace("multiperiodic", "")
                            Dim split As String() = temp.Split("="c)
                            If split.Count > 0 Then
                                pStrucHADSRecord.HADSName = split(0).Trim
                            End If
                            If split.Count > 1 Then
                                pStrucHADSRecord.HADSName2 = split(1).Trim
                            End If
                            If split.Count > 2 Then
                                pStrucHADSRecord.HADSName3 = split(2).Trim
                            End If
                            If split.Count > 3 Then
                                pStrucHADSRecord.HADSName4 = split(3).Trim
                            End If
                            If split.Count > 4 Then
                                pStrucHADSRecord.HADSName5 = split(4).Trim
                            End If

                            temp = currentRecord(1)
                            Dim splitRA As String() = temp.Split(" "c)
                            If splitRA.Count > 0 Then
                                pStrucHADSRecord.HADSRA2000HH = splitRA(0).Trim
                            End If
                            If splitRA.Count > 1 Then
                                pStrucHADSRecord.HADSRA2000MM = splitRA(1).Trim
                            End If
                            If splitRA.Count > 2 Then
                                pStrucHADSRecord.HADSRA2000SS = splitRA(2).Trim
                            End If

                            temp = currentRecord(2)
                            Dim splitDEC As String() = temp.Split(" "c)
                            If splitDEC.Count > 0 Then
                                pStrucHADSRecord.HADSDEC2000DG = splitDEC(0).Trim
                            End If
                            If splitDEC.Count > 1 Then
                                pStrucHADSRecord.HADSDEC2000MM = splitDEC(1).Trim
                            End If
                            If splitDEC.Count > 2 Then
                                pStrucHADSRecord.HADSDEC2000SS = splitDEC(2).Trim
                            End If

                            pStrucHADSRecord.HADSMax = currentRecord(3)
                            pStrucHADSRecord.HADSMin = currentRecord(4)
                            pStrucHADSRecord.HADSPeriod = currentRecord(5)
                            Dim ciClone As CultureInfo = CType(CultureInfo.InvariantCulture.Clone(), CultureInfo)
                            ciClone.NumberFormat.NumberDecimalSeparator = "."

                            pStrucHADSRecord.HADSExposure = EstimateExposureTime(Double.Parse(pStrucHADSRecord.HADSMax, ciClone))

                            pStrucHADSRecord.HADSJan = currentRecord(6)
                            pStrucHADSRecord.HADSFeb = currentRecord(7)
                            pStrucHADSRecord.HADSMar = currentRecord(8)
                            pStrucHADSRecord.HADSApr = currentRecord(9)
                            pStrucHADSRecord.HADSMay = currentRecord(10)
                            pStrucHADSRecord.HADSJun = currentRecord(11)
                            pStrucHADSRecord.HADSJul = currentRecord(12)
                            pStrucHADSRecord.HADSAug = currentRecord(13)
                            pStrucHADSRecord.HADSSep = currentRecord(14)
                            pStrucHADSRecord.HADSOct = currentRecord(15)
                            pStrucHADSRecord.HADSNov = currentRecord(16)
                            pStrucHADSRecord.HADSDec = currentRecord(17)

                            'check if record exists
                            returnvalue = CheckHADSRecord(pStrucHADSRecord.HADSName)
                            'insert or update
                            If returnvalue <> "0" And returnvalue <> "1" And returnvalue <> "2" Then
                                '0=new record, 1=record exists and needs update, 2=record exists but no update required, other value=error
                                'error occurred, do not exit but continue with next record
                                LogSessionEntry("ERROR", "ReadHADSFile: " + returnvalue, "", "ReadHADSFile", "PROGRAM")
                                ReadHADSFile = returnvalue
                            Else
                                If returnvalue = "1" Then
                                    'update required
                                    returnvalue = UpdateHADSRecord()
                                    If returnvalue <> "OK" Then
                                        LogSessionEntry("Error", "UpdateHADSRecord: " + returnvalue, "", "UpdateHADSRecord", "PROGRAM")
                                    End If
                                    HADSNbrOfUpdatedRecords += 1
                                ElseIf returnvalue = "0" Then
                                    'return value=0: new record
                                    'insert, but only when declination is higher then cutoff
                                    If Convert.ToDouble(pStrucHADSRecord.HADSDEC2000DG) > My.Settings.sHADSDECCutOff Then
                                        returnvalue = InsertHADSRecord()
                                        If returnvalue <> "OK" Then
                                            LogSessionEntry("ERROR", "InsertHADSRecord: " + returnvalue, "", "InsertHADSRecord", "PROGRAM")
                                        End If
                                        HADSNbrOfInsertedRecords += 1
                                    Else
                                        'do nothing
                                        returnvalue = ""
                                    End If
                                Else
                                    'do nothing
                                    returnvalue = ""
                                End If
                            End If

                            'update the active flag
                            returnvalue = UpdateHADSRecordActiveFlag()
                            If returnvalue <> "OK" Then
                                LogSessionEntry("Error", "UpdateHADSRecord: " + returnvalue, "", "UpdateHADSRecord", "PROGRAM")
                            End If

                        End If
                    Catch ex As Microsoft.VisualBasic.FileIO.MalformedLineException
                        ShowMessage("Line " & ex.Message & "is not valid and will be skipped.", "CRITICAL", "Incorrect input...")
                    End Try
                End While

                returnvalue = UpdateHADSFocusLink()
                returnvalue = CheckHADSEmptyFocusStar()

                If HADSNbrOfUpdatedRecords > 0 Then
                    LogSessionEntry("BRIEF", Format(HADSNbrOfUpdatedRecords) + " HADS records were updated...", "", "ReadHADSFile", "PROGRAM")
                End If

                If HADSNbrOfInsertedRecords > 0 Then
                    LogSessionEntry("BRIEF", Format(HADSNbrOfInsertedRecords) + " HADS records were inserted...", "", "ReadHADSFile", "PROGRAM")
                End If

            End Using
            executionTime = DateTime.UtcNow() - startExecution
            LogSessionEntry("DEBUG", "  ReadHADSFile: " + executionTime.ToString, "", "ReadHADSFile", "PROGRAM")

        Catch ex As Exception
            LogSessionEntry("ERROR", "ReadHADSFile: " + ex.Message, "ReadHADSFile", "", "PROGRAM")
            ReadHADSFile = ex.Message
            'reset all records active
            SetAllActiveHADSRecord()
        End Try
    End Function


    Public Function CheckHADSRecord(vHADSName As String) As String
        'check if a HADS-record already exists
        Dim reader As SQLiteDataReader
        Dim startExecution As Date
        Dim executionTime As TimeSpan

        CheckHADSRecord = "0"
        Try
            startExecution = DateTime.UtcNow()
            LogSessionEntry("DEBUG", "  CheckHADSRecord...", "", "CheckHADSRecord", "PROGRAM")

            Using pCon As New SQLiteConnection("Data Source=RoboticObservatory.db;Version=3;")
                Using cmd As SQLiteCommand = pCon.CreateCommand
                    Try
                        pCon.Open()
                        cmd.Connection = pCon
                        cmd.CommandText = "select ID, HADSName2, HADSName3, HADSName4, HADSName5, HADSActive, " +
                                          "HADSRA2000HH, HADSRA2000MM, HADSRA2000SS, " +
                                          "HADSDEC2000DG, HADSDEC2000MM, HADSDEC2000SS, " +
                                          "HADSMin, HADSMax, HADSPeriod, " +
                                          "HADSJan, HADSFeb, HADSMar, HADSApr, HADSMay, HADSJun, " +
                                          "HADSJul, HADSAug, HADSSep, HADSOct, HADSNov, HADSDec, " +
                                          "HADSExposure " +
                                          "FROM HADS WHERE HADSName = '" + vHADSName + "'"
                        reader = cmd.ExecuteReader()
                        While reader.Read()
                            If Convert.ToInt32(reader.Item(0)) > 0 Then
                                'record already exists, compare with new version
                                'set record as not updated, will be changed in following code if needed
                                CheckHADSRecord = "2"

                                If reader.Item(1).ToString <> pStrucHADSRecord.HADSName2 Then
                                    CheckHADSRecord = "1"
                                End If
                                If reader.Item(2).ToString <> pStrucHADSRecord.HADSName3 Then
                                    CheckHADSRecord = "1"
                                End If
                                If reader.Item(3).ToString <> pStrucHADSRecord.HADSName4 Then
                                    CheckHADSRecord = "1"
                                End If
                                If reader.Item(4).ToString <> pStrucHADSRecord.HADSName5 Then
                                    CheckHADSRecord = "1"
                                End If
                                If Convert.ToDouble(reader.Item(5)) <> pStrucHADSRecord.HADSActive Then
                                    CheckHADSRecord = "1"
                                End If
                                If Convert.ToDouble(reader.Item(6)) <> Convert.ToDouble(pStrucHADSRecord.HADSRA2000HH) Then
                                    CheckHADSRecord = "1"
                                End If
                                If Convert.ToDouble(reader.Item(7)) <> Convert.ToDouble(pStrucHADSRecord.HADSRA2000MM) Then
                                    CheckHADSRecord = "1"
                                End If
                                If Convert.ToDouble(reader.Item(8)) <> Convert.ToDouble(pStrucHADSRecord.HADSRA2000SS) Then
                                    CheckHADSRecord = "1"
                                End If
                                If Convert.ToDouble(reader.Item(9)) <> Convert.ToDouble(pStrucHADSRecord.HADSDEC2000DG) Then
                                    CheckHADSRecord = "1"
                                End If
                                If Convert.ToDouble(reader.Item(10)) <> Convert.ToDouble(pStrucHADSRecord.HADSDEC2000MM) Then
                                    CheckHADSRecord = "1"
                                End If
                                If Convert.ToDouble(reader.Item(11)) <> Convert.ToDouble(pStrucHADSRecord.HADSDEC2000SS) Then
                                    CheckHADSRecord = "1"
                                End If
                                Dim ciClone As CultureInfo = CType(CultureInfo.InvariantCulture.Clone(), CultureInfo)
                                ciClone.NumberFormat.NumberDecimalSeparator = "."

                                'pStrucHADSRecord.HADSExposure = EstimateExposureTime(Double.Parse(pStrucHADSRecord.HADSMax, ciClone))

                                If Convert.ToDouble(reader.Item(12)) <> Double.Parse(pStrucHADSRecord.HADSMin, ciClone) Then
                                    CheckHADSRecord = "1"
                                End If
                                If Convert.ToDouble(reader.Item(13)) <> Double.Parse(pStrucHADSRecord.HADSMax, ciClone) Then
                                    CheckHADSRecord = "1"
                                End If
                                If Convert.ToDouble(reader.Item(14)) <> Double.Parse(pStrucHADSRecord.HADSPeriod, ciClone) Then
                                    CheckHADSRecord = "1"
                                End If
                                If reader.Item(15).ToString <> pStrucHADSRecord.HADSJan Then
                                    CheckHADSRecord = "1"
                                End If
                                If reader.Item(16).ToString <> pStrucHADSRecord.HADSFeb Then
                                    CheckHADSRecord = "1"
                                End If
                                If reader.Item(17).ToString <> pStrucHADSRecord.HADSMar Then
                                    CheckHADSRecord = "1"
                                End If
                                If reader.Item(18).ToString <> pStrucHADSRecord.HADSApr Then
                                    CheckHADSRecord = "1"
                                End If
                                If reader.Item(19).ToString <> pStrucHADSRecord.HADSMay Then
                                    CheckHADSRecord = "1"
                                End If
                                If reader.Item(20).ToString <> pStrucHADSRecord.HADSJun Then
                                    CheckHADSRecord = "1"
                                End If
                                If reader.Item(21).ToString <> pStrucHADSRecord.HADSJul Then
                                    CheckHADSRecord = "1"
                                End If
                                If reader.Item(22).ToString <> pStrucHADSRecord.HADSAug Then
                                    CheckHADSRecord = "1"
                                End If
                                If reader.Item(23).ToString <> pStrucHADSRecord.HADSSep Then
                                    CheckHADSRecord = "1"
                                End If
                                If reader.Item(24).ToString <> pStrucHADSRecord.HADSOct Then
                                    CheckHADSRecord = "1"
                                End If
                                If reader.Item(25).ToString <> pStrucHADSRecord.HADSNov Then
                                    CheckHADSRecord = "1"
                                End If
                                If reader.Item(26).ToString <> pStrucHADSRecord.HADSDec Then
                                    CheckHADSRecord = "1"
                                End If
                                If Convert.ToDouble(reader.Item(27)) <> pStrucHADSRecord.HADSExposure Then
                                    CheckHADSRecord = "1"
                                End If
                            Else
                                'new record
                                CheckHADSRecord = "0"
                            End If
                        End While
                        pCon.Close()

                    Catch ex As Exception
                        CheckHADSRecord = "CheckHADSRecord: " + ex.Message
                        LogSessionEntry("ERROR", "CheckHADSRecord: " + ex.Message, "", "CheckHADSRecord", "PROGRAM")
                    End Try
                End Using
            End Using

            executionTime = DateTime.UtcNow() - startExecution
            LogSessionEntry("DEBUG", "  CheckHADSRecord: " + executionTime.ToString, "", "CheckHADSRecord", "PROGRAM")

        Catch ex As Exception
            LogSessionEntry("ERROR", "CheckHADSRecord: " + ex.Message, "", "CheckHADSRecord", "PROGRAM")
            CheckHADSRecord = ex.Message
        End Try
    End Function

    Public Function CheckHADSEmptyFocusStar() As String
        'check if all the HADS-records have focusstars
        Dim reader As SQLiteDataReader
        Dim startExecution As Date
        Dim executionTime As TimeSpan

        CheckHADSEmptyFocusStar = "OK"
        Try
            startExecution = DateTime.UtcNow()
            LogSessionEntry("DEBUG", "  CheckHADSEmptyFocusStar...", "", "CheckHADSEmptyFocusStar", "PROGRAM")


            Using pCon As New SQLiteConnection("Data Source=RoboticObservatory.db;Version=3;")
                Using cmd As SQLiteCommand = pCon.CreateCommand
                    Try
                        pCon.Open()
                        cmd.Connection = pCon
                        cmd.CommandText = "select ID, HADSName " +
                                          "FROM HADS WHERE FocusStarName = ''"
                        reader = cmd.ExecuteReader()
                        While reader.Read()
                            If Convert.ToInt32(reader.Item(0)) > 0 Then
                                LogSessionEntry("FULL", "Focus star missing for : " + reader.Item(1).ToString, "", "CheckHADSEmptyFocusStar", "PROGRAM")
                            End If
                        End While
                        pCon.Close()

                    Catch ex As Exception
                        CheckHADSEmptyFocusStar = "CheckHADSEmptyFocusStar: " + ex.Message
                        LogSessionEntry("ERROR", "CheckHADSEmptyFocusStar: " + ex.Message, "", "CheckHADSEmptyFocusStar", "PROGRAM")
                    End Try
                End Using
            End Using
            executionTime = DateTime.UtcNow() - startExecution
            LogSessionEntry("DEBUG", "  CheckHADSEmptyFocusStar: " + executionTime.ToString, "", "CheckHADSEmptyFocusStar", "PROGRAM")

        Catch ex As Exception
            LogSessionEntry("ERROR", "CheckHADSEmptyFocusStar: " + ex.Message, "", "CheckHADSEmptyFocusStar", "PROGRAM")
            CheckHADSEmptyFocusStar = ex.Message
        End Try
    End Function

    Public Function InsertHADSRecord() As String
        Dim i As Integer
        Dim reader As SQLiteDataReader
        Dim returnvalue As String
        Dim startExecution As Date
        Dim executionTime As TimeSpan

        InsertHADSRecord = "OK"
        Try
            startExecution = DateTime.UtcNow()
            LogSessionEntry("DEBUG", "  InsertHADSRecord...", "", "InsertHADSRecord", "PROGRAM")

            'find if a focusstar was defined
            Using pCon As New SQLiteConnection("Data Source=RoboticObservatory.db;Version=3;")
                Using cmd As SQLiteCommand = pCon.CreateCommand
                    Try
                        pCon.Open()
                        cmd.Connection = pCon
                        cmd.CommandText = "select ID, HADSName, FocusName FROM  HADSFocus where HADSNAME = '" + pStrucHADSRecord.HADSName + "'"
                        reader = cmd.ExecuteReader()
                        While reader.Read()
                            If Convert.ToInt32(reader.Item(0)) > 0 Then
                                'find the coordinates
                                returnvalue = FindTheSkyXTarget(reader.Item(2).ToString)

                                'convert values to HH MM DD and DD MM SS
                                Dim RA_string, DEC_string As String
                                Dim index, old_index As Integer
                                'Dim Alt_string, Az_string As String
                                'Dim RAHH, RAMM, RASS
                                pStrucHADSRecord.FocusStarName = reader.Item(2).ToString
                                RA_string = pAUtil.HoursToHMS(pStrucTargetObject.RightAscension, "h ", "m ", "s ")
                                DEC_string = pAUtil.DegreesToDMS(pStrucTargetObject.Declination, "° ", "' ", """ ")
                                '01:51:28
                                '-10° 20' 06"
                                '12345678901

                                index = GetNthIndex(RA_string, ":"c, 1)
                                pStrucHADSRecord.FocusStarRA2000HH = RA_string.Substring(0, index)
                                old_index = index
                                index = GetNthIndex(RA_string, ":"c, 2)
                                pStrucHADSRecord.FocusStarRA2000MM = RA_string.Substring(old_index + 1, index - old_index - 1)
                                pStrucHADSRecord.FocusStarRA2000SS = RA_string.Substring(index + 1, 2)

                                index = GetNthIndex(DEC_string, "°"c, 1)
                                pStrucHADSRecord.FocusStarDEC2000DG = DEC_string.Substring(0, index)
                                pStrucHADSRecord.FocusStarDEC2000MM = DEC_string.Substring(index + 2, 2)
                                index = GetNthIndex(DEC_string, "'"c, 1)
                                pStrucHADSRecord.FocusStarDEC2000SS = DEC_string.Substring(index + 2, 2)
                            End If
                        End While
                        pCon.Close()
                    Catch ex As Exception
                        InsertHADSRecord = "InsertHADSRecord: " + ex.Message
                        LogSessionEntry("ERROR", "InsertHADSRecord: " + ex.Message, "", "InsertHADSRecord", "PROGRAM")
                    End Try
                End Using
            End Using

            ' insert the record
            Using pCon As New SQLiteConnection("Data Source=RoboticObservatory.db;Version=3;")
                Using cmd As SQLiteCommand = pCon.CreateCommand
                    Using daSL As New SQLiteDataAdapter
                        Try
                            pCon.Open()
                            daSL.SelectCommand = cmd
                            cmd.CommandText = "INSERT into HADS (HADSName, HADSName2, HADSName3, HADSName4, HADSName5, HADSActive, " +
                                            "HADSRA2000HH, HADSRA2000MM, HADSRA2000SS, HADSDEC2000DG, HADSDEC2000MM, HADSDEC2000SS, HADSMin, HADSMax, HADSPeriod, HADSExposure, " +
                                            "HADSBin, HADSFilter, HADSJan, HADSFeb, HADSMar, HADSApr, HADSMay, HADSJun, HADSJul, HADSAug, HADSSep, HADSOct, HADSNov, HADSDec, " +
                                            "HADSLastObserved, FocusStarName, FocusStarRA2000HH, FocusStarRA2000MM, FocusStarRA2000SS, " +
                                            "FocusStarDEC2000DG, FocusStarDEC2000MM, FocusStarDEC2000SS, " +
                                            "FocusStarExposure, HADSPriority, HADSError) VALUES ('" +
                                                pStrucHADSRecord.HADSName + "','" + pStrucHADSRecord.HADSName2 + "','" + pStrucHADSRecord.HADSName3 + "','" + pStrucHADSRecord.HADSName4 + "','" +
                                                pStrucHADSRecord.HADSName5 + "','1', '" + pStrucHADSRecord.HADSRA2000HH + "','" + pStrucHADSRecord.HADSRA2000MM + "','" +
                                                pStrucHADSRecord.HADSRA2000SS + "','" + pStrucHADSRecord.HADSDEC2000DG + "','" + pStrucHADSRecord.HADSDEC2000MM + "','" +
                                                pStrucHADSRecord.HADSDEC2000SS + "','" + pStrucHADSRecord.HADSMin + "','" + pStrucHADSRecord.HADSMax + "','" + pStrucHADSRecord.HADSPeriod + "'," +
                                                pStrucHADSRecord.HADSExposure.ToString + ", '2x2','V','" + pStrucHADSRecord.HADSJan + "','" + pStrucHADSRecord.HADSFeb + "','" + pStrucHADSRecord.HADSMar + "','" +
                                                pStrucHADSRecord.HADSApr + "','" + pStrucHADSRecord.HADSMay + "','" + pStrucHADSRecord.HADSJun + "','" + pStrucHADSRecord.HADSJul + "','" +
                                                pStrucHADSRecord.HADSAug + "','" + pStrucHADSRecord.HADSSep + "','" + pStrucHADSRecord.HADSOct + "','" + pStrucHADSRecord.HADSNov + "','" +
                                                pStrucHADSRecord.HADSDec + "','','" + pStrucHADSRecord.FocusStarName + "','" +
                                                pStrucHADSRecord.FocusStarRA2000HH + "','" + pStrucHADSRecord.FocusStarRA2000MM + "','" + pStrucHADSRecord.FocusStarRA2000SS + "','" +
                                                pStrucHADSRecord.FocusStarDEC2000DG + "','" + pStrucHADSRecord.FocusStarDEC2000MM + "','" + pStrucHADSRecord.FocusStarDEC2000SS + "','5','0','0'" + ")"

                            daSL.SelectCommand = cmd
                            i = cmd.ExecuteNonQuery
                            pCon.Close()

                        Catch ex As Exception
                            InsertHADSRecord = "InsertHADSRecord: " + ex.Message
                            LogSessionEntry("ERROR", "InsertHADSRecord: " + ex.Message, "", "InsertHADSRecord", "PROGRAM")
                        End Try
                    End Using
                End Using
            End Using
            executionTime = DateTime.UtcNow() - startExecution
            LogSessionEntry("DEBUG", "  InsertHADSRecord: " + executionTime.ToString, "", "InsertHADSRecord", "PROGRAM")

        Catch ex As Exception
            LogSessionEntry("ERROR", "InsertHADSRecord: " + ex.Message, "", "InsertHADSRecord", "PROGRAM")
            InsertHADSRecord = ex.Message
        End Try
    End Function


    Public Function UpdateHADSRecord() As String
        Dim i As Integer
        Dim startExecution As Date
        Dim executionTime As TimeSpan

        UpdateHADSRecord = "OK"
        Try
            startExecution = DateTime.UtcNow()
            LogSessionEntry("DEBUG", "  UpdateHADSRecord...", "", "UpdateHADSRecord", "PROGRAM")


            Using pCon As New SQLiteConnection("Data Source=RoboticObservatory.db;Version=3;")
                Using cmd As SQLiteCommand = pCon.CreateCommand
                    Using daSL As New SQLiteDataAdapter
                        Try
                            pCon.Open()
                            daSL.SelectCommand = cmd
                            cmd.CommandText = "UPDATE HADS set " +
                                " HADSName2='" + pStrucHADSRecord.HADSName2 + "'," +
                                " HADSName3='" + pStrucHADSRecord.HADSName3 + "'," +
                                " HADSName4='" + pStrucHADSRecord.HADSName4 + "'," +
                                " HADSName5='" + pStrucHADSRecord.HADSName5 + "'," +
                                " HADSActive='1'," +
                                " HADSRA2000HH='" + pStrucHADSRecord.HADSRA2000HH + "'," +
                                " HADSRA2000MM='" + pStrucHADSRecord.HADSRA2000MM + "'," +
                                " HADSRA2000SS='" + pStrucHADSRecord.HADSRA2000SS + "'," +
                                " HADSDEC2000DG='" + pStrucHADSRecord.HADSDEC2000DG + "'," +
                                " HADSDEC2000MM='" + pStrucHADSRecord.HADSDEC2000MM + "'," +
                                " HADSDEC2000SS='" + pStrucHADSRecord.HADSDEC2000SS + "'," +
                                " HADSMin='" + pStrucHADSRecord.HADSMin + "'," +
                                " HADSMax='" + pStrucHADSRecord.HADSMax + "'," +
                                " HADSPeriod='" + pStrucHADSRecord.HADSPeriod + "'," +
                                " HADSJan='" + pStrucHADSRecord.HADSJan + "'," +
                                " HADSFeb='" + pStrucHADSRecord.HADSFeb + "'," +
                                " HADSMar='" + pStrucHADSRecord.HADSMar + "'," +
                                " HADSApr='" + pStrucHADSRecord.HADSApr + "'," +
                                " HADSMay='" + pStrucHADSRecord.HADSMay + "'," +
                                " HADSJun='" + pStrucHADSRecord.HADSJun + "'," +
                                " HADSJul='" + pStrucHADSRecord.HADSJul + "'," +
                                " HADSAug='" + pStrucHADSRecord.HADSAug + "'," +
                                " HADSSep='" + pStrucHADSRecord.HADSSep + "'," +
                                " HADSOct='" + pStrucHADSRecord.HADSOct + "'," +
                                " HADSNov='" + pStrucHADSRecord.HADSNov + "'," +
                                " HADSDec='" + pStrucHADSRecord.HADSDec + "'," +
                                " HADSExposure='" + pStrucHADSRecord.HADSExposure.ToString + "'" +
                            " WHERE HADSName = '" + pStrucHADSRecord.HADSName + "'"

                            daSL.SelectCommand = cmd
                            i = cmd.ExecuteNonQuery
                            pCon.Close()

                        Catch ex As Exception
                            UpdateHADSRecord = "UpdateHADSRecord: " + ex.Message
                            LogSessionEntry("ERROR", "UpdateHADSRecord: " + ex.Message, "", "UpdateHADSRecord", "PROGRAM")
                        End Try
                    End Using
                End Using
            End Using
            executionTime = DateTime.UtcNow() - startExecution
            LogSessionEntry("DEBUG", "  UpdateHADSRecord: " + executionTime.ToString, "", "UpdateHADSRecord", "PROGRAM")


        Catch ex As Exception
            LogSessionEntry("ERROR", "UpdateHADSRecord: " + ex.Message, "", "UpdateHADSRecord", "PROGRAM")
            UpdateHADSRecord = ex.Message
        End Try
    End Function



    Public Function SetAllInactiveHADSRecord() As String
        Dim i As Integer
        Dim startExecution As Date
        Dim executionTime As TimeSpan

        SetAllInactiveHADSRecord = "OK"
        Try
            startExecution = DateTime.UtcNow()
            LogSessionEntry("DEBUG", "  SetAllInactiveHADSRecord...", "", "SetAllInactiveHADSRecord", "PROGRAM")

            Using pCon As New SQLiteConnection("Data Source=RoboticObservatory.db;Version=3;")
                Using cmd As SQLiteCommand = pCon.CreateCommand
                    Using daSL As New SQLiteDataAdapter
                        Try
                            pCon.Open()
                            daSL.SelectCommand = cmd
                            cmd.CommandText = "UPDATE HADS set HADSActive='0'"

                            daSL.SelectCommand = cmd
                            i = cmd.ExecuteNonQuery
                            pCon.Close()

                        Catch ex As Exception
                            SetAllInactiveHADSRecord = "SetAllInactiveHADSRecord: " + ex.Message
                            LogSessionEntry("ERROR", "SetAllInactiveHADSRecord: " + ex.Message, "", "SetAllInactiveHADSRecord", "PROGRAM")
                        End Try
                    End Using
                End Using
            End Using
            executionTime = DateTime.UtcNow() - startExecution
            LogSessionEntry("DEBUG", "  SetAllInactiveHADSRecord: " + executionTime.ToString, "", "SetAllInactiveHADSRecord", "PROGRAM")

        Catch ex As Exception
            LogSessionEntry("ERROR", "SetAllInactiveHADSRecord: " + ex.Message, "", "SetAllInactiveHADSRecord", "PROGRAM")
            SetAllInactiveHADSRecord = ex.Message
        End Try
    End Function


    Public Function SetAllActiveHADSRecord() As String
        Dim i As Integer
        Dim startExecution As Date
        Dim executionTime As TimeSpan

        SetAllActiveHADSRecord = "OK"
        Try
            startExecution = DateTime.UtcNow()
            LogSessionEntry("DEBUG", "  SetAllActiveHADSRecord...", "", "SetAllActiveHADSRecord", "PROGRAM")

            Using pCon As New SQLiteConnection("Data Source=RoboticObservatory.db;Version=3;")
                Using cmd As SQLiteCommand = pCon.CreateCommand
                    Using daSL As New SQLiteDataAdapter
                        Try
                            pCon.Open()
                            daSL.SelectCommand = cmd
                            cmd.CommandText = "UPDATE HADS set HADSActive='1'"

                            daSL.SelectCommand = cmd
                            i = cmd.ExecuteNonQuery
                            pCon.Close()

                        Catch ex As Exception
                            SetAllActiveHADSRecord = "SetAllActiveHADSRecord: " + ex.Message
                            LogSessionEntry("ERROR", "SetAllActiveHADSRecord: " + ex.Message, "", "SetAllActiveHADSRecord", "PROGRAM")
                        End Try
                    End Using
                End Using
            End Using
            executionTime = DateTime.UtcNow() - startExecution
            LogSessionEntry("DEBUG", "  SetAllActiveHADSRecord: " + executionTime.ToString, "", "SetAllActiveHADSRecord", "PROGRAM")

        Catch ex As Exception
            LogSessionEntry("ERROR", "SetAllActiveHADSRecord: " + ex.Message, "", "SetAllActiveHADSRecord", "PROGRAM")
            SetAllActiveHADSRecord = ex.Message
        End Try
    End Function



    Public Function UpdateHADSRecordActiveFlag() As String
        Dim i As Integer
        Dim startExecution As Date
        Dim executionTime As TimeSpan

        UpdateHADSRecordActiveFlag = "OK"
        Try
            startExecution = DateTime.UtcNow()
            LogSessionEntry("DEBUG", "  UpdateHADSRecordActiveFlag...", "", "UpdateHADSRecordActiveFlag", "PROGRAM")

            Using pCon As New SQLiteConnection("Data Source=RoboticObservatory.db;Version=3;")
                Using cmd As SQLiteCommand = pCon.CreateCommand
                    Using daSL As New SQLiteDataAdapter
                        Try
                            pCon.Open()
                            daSL.SelectCommand = cmd
                            cmd.CommandText = "UPDATE HADS set " +
                                " HADSActive='1' " +
                                " WHERE HADSName = '" + pStrucHADSRecord.HADSName + "'"

                            daSL.SelectCommand = cmd
                            i = cmd.ExecuteNonQuery
                            pCon.Close()

                        Catch ex As Exception
                            UpdateHADSRecordActiveFlag = "UpdateHADSRecordActiveFlag: " + ex.Message
                            LogSessionEntry("ERROR", "UpdateHADSRecordActiveFlag: " + ex.Message, "", "UpdateHADSRecordActiveFlag", "PROGRAM")
                        End Try
                    End Using
                End Using
            End Using
            executionTime = DateTime.UtcNow() - startExecution
            LogSessionEntry("DEBUG", "  UpdateHADSRecordActiveFlag: " + executionTime.ToString, "", "UpdateHADSRecordActiveFlag", "PROGRAM")

        Catch ex As Exception
            LogSessionEntry("ERROR", "UpdateHADSRecordActiveFlag: " + ex.Message, "", "UpdateHADSRecordActiveFlag", "PROGRAM")
            UpdateHADSRecordActiveFlag = ex.Message
        End Try
    End Function


    Public Function UpdateHADSFocusLink() As String
        'first truncate the table with all the links, next refill, based on current HADS-table
        Dim reader As SQLiteDataReader
        Dim i As Integer
        Dim startExecution As Date
        Dim executionTime As TimeSpan


        UpdateHADSFocusLink = "OK"
        Try
            startExecution = DateTime.UtcNow()
            LogSessionEntry("DEBUG", "  UpdateHADSFocusLink...", "", "UpdateHADSFocusLink", "PROGRAM")

            'truncate the HADSFocus table
            Using pCon As New SQLiteConnection("Data Source=RoboticObservatory.db;Version=3;")
                Using cmd As SQLiteCommand = pCon.CreateCommand
                    Using daSL As New SQLiteDataAdapter
                        Try
                            pCon.Open()
                            daSL.SelectCommand = cmd
                            cmd.CommandText = "DELETE from HADSFocus"

                            daSL.SelectCommand = cmd
                            i = cmd.ExecuteNonQuery
                            pCon.Close()

                        Catch ex As Exception
                            UpdateHADSFocusLink = "UpdateHADSFocusLink: " + ex.Message
                            LogSessionEntry("ERROR", "UpdateHADSFocusLink: " + ex.Message, "", "UpdateHADSFocusLink", "PROGRAM")
                        End Try
                    End Using
                End Using
            End Using

            'fill the HADSFocus table
            Using pCon As New SQLiteConnection("Data Source=RoboticObservatory.db;Version=3;")
                Using cmd As SQLiteCommand = pCon.CreateCommand
                    Try
                        pCon.Open()
                        cmd.Connection = pCon
                        cmd.CommandText = "select ID, HADSName, FocusStarName FROM HADS WHERE FocusStarName <> ''"
                        reader = cmd.ExecuteReader()
                        While reader.Read()
                            If Convert.ToInt32(reader.Item(0)) > 0 Then
                                'insert the record
                                'Using pConInsert As New SQLiteConnection("Data Source=RoboticObservatory.db;Version=3;")
                                Using cmdInsert As SQLiteCommand = pCon.CreateCommand
                                    Using daSLInsert As New SQLiteDataAdapter
                                        Try
                                            'pConInsert.Open()
                                            daSLInsert.SelectCommand = cmdInsert
                                            cmdInsert.CommandText = "INSERT into HADSFocus (HADSName, FocusName) VALUES ('" + reader.Item(1).ToString + "','" + reader.Item(2).ToString + "')"
                                            daSLInsert.SelectCommand = cmdInsert
                                            i = cmdInsert.ExecuteNonQuery
                                            'pConInsert.Close()

                                        Catch ex As Exception
                                            UpdateHADSFocusLink = "UpdateHADSFocusLink: " + ex.Message
                                            LogSessionEntry("ERROR", "UpdateHADSFocusLink: " + ex.Message, "", "UpdateHADSFocusLink", "PROGRAM")
                                        End Try
                                    End Using
                                End Using
                                'End Using
                            End If
                        End While
                        pCon.Close()

                    Catch ex As Exception
                        UpdateHADSFocusLink = "UpdateHADSFocusLink: " + ex.Message
                        LogSessionEntry("ERROR", "UpdateHADSFocusLink: " + ex.Message, "", "UpdateHADSFocusLink", "PROGRAM")
                    End Try
                End Using
            End Using
            executionTime = DateTime.UtcNow() - startExecution
            LogSessionEntry("DEBUG", "  UpdateHADSFocusLink: " + executionTime.ToString, "", "UpdateHADSFocusLink", "PROGRAM")

        Catch ex As Exception
            LogSessionEntry("ERROR", "UpdateHADSFocusLink: " + ex.Message, "", "UpdateHADSFocusLink", "PROGRAM")
            UpdateHADSFocusLink = ex.Message
        End Try
    End Function

    Public Function EstimateExposureTime(vMinMag As Double) As Double
        Dim startExecution As Date
        Dim executionTime As TimeSpan

        Try
            startExecution = DateTime.UtcNow()
            LogSessionEntry("DEBUG", "  EstimateExposureTime...", "", "EstimateExposureTime", "PROGRAM")

            EstimateExposureTime = 120

            If vMinMag < 9 Then
                EstimateExposureTime = Convert.ToDouble(My.Settings.sHADSMag8)
            ElseIf vMinMag >= 9 And vMinMag < 10 Then
                EstimateExposureTime = Convert.ToDouble(My.Settings.sHADSMag9)
            ElseIf vMinMag >= 10 And vMinMag < 11 Then
                EstimateExposureTime = Convert.ToDouble(My.Settings.sHADSMag10)
            ElseIf vMinMag >= 11 And vMinMag < 12 Then
                EstimateExposureTime = Convert.ToDouble(My.Settings.sHADSMag11)
            ElseIf vMinMag >= 12 And vMinMag < 13 Then
                EstimateExposureTime = Convert.ToDouble(My.Settings.sHADSMag12)
            ElseIf vMinMag >= 13 And vMinMag < 14 Then
                EstimateExposureTime = Convert.ToDouble(My.Settings.sHADSMag13)
            ElseIf vMinMag >= 14 And vMinMag < 15 Then
                EstimateExposureTime = Convert.ToDouble(My.Settings.sHADSMag14)
            ElseIf vMinMag >= 15 Then
                EstimateExposureTime = Convert.ToDouble(My.Settings.sHADSMag15)
            End If

            executionTime = DateTime.UtcNow() - startExecution
            LogSessionEntry("DEBUG", "  EstimateExposureTime: " + executionTime.ToString, "", "EstimateExposureTime", "PROGRAM")

        Catch ex As Exception
            LogSessionEntry("ERROR", "EstimateExposureTime: " + ex.Message, "", "EstimateExposureTime", "PROGRAM")
            'if error set default to 120
            EstimateExposureTime = 120
        End Try
    End Function

    Public Function DatabaseSelectHADS(vCheckOnly As Boolean) As String
        Dim returnvalue As String
        Dim i As Integer
        Dim RA2000, DEC2000 As Double
        Dim TargetFound As Boolean = False

        Dim startExecution As Date
        Dim executionTime As TimeSpan

        DatabaseSelectHADS = "OK"
        Try
            startExecution = DateTime.UtcNow()
            LogSessionEntry("DEBUG", "  DatabaseSelectHADS...", "", "DatabaseSelectHADS", "PROGRAM")

            FrmMain.Cursor = Cursors.WaitCursor

            Using pCon As New SQLiteConnection("Data Source=RoboticObservatory.db;Version=3;")
                Using cmd As SQLiteCommand = pCon.CreateCommand
                    Using da As New SQLiteDataAdapter
                        Try
                            da.SelectCommand = cmd
                            'select all active records
                            cmd.CommandText =
                            "SELECT ID, HADSName, HADSName2, HADSName3, HADSName4, HADSName5, HADSMin, HADSMax, " +
                            "HADSPeriod, HADSRA2000HH, HADSRA2000MM, HADSRA2000SS, HADSDEC2000DG, HADSDEC2000MM, HADSDEC2000SS, " +
                            "HADSExposure, HADSBin, HADSFilter, HADSJan, HADSFeb, HADSMar, HADSApr, HADSMay, HADSJun, " +
                            "HADSJul, HADSAug, HADSSep, HADSOct, HADSNov, HADSDec, FocusStarName, " +
                            "FocusStarRA2000HH, FocusStarRA2000MM, FocusStarRA2000SS, FocusStarDEC2000DG, FocusStarDEC2000MM, " +
                            "FocusStarDEC2000SS, FocusStarExposure, HADSPriority, HADSError, HADSError, HADSLastObserved, HADSActive, " +
                            "0.01 as Altitude, 0.01 as Azimuth, 0 as CompassIndex, '' as CompassDirection, False as Circumpolar, 0 as SortOrder " +
                            "FROM HADS WHERE HADSActive = True and HADSError=False"

                            da.SelectCommand = cmd
                            pDtVSLTarget.Clear()
                            da.Fill(pDtVSLTarget)

                            For i = 0 To pDtVSLTarget.Rows.Count - 1
                                'caculate actual position
                                'eerst omzetten naar double
                                RA2000 = pAUtil.HMSToHours(Format(pDtVSLTarget.Rows(i).Item(9)) + " " + Format(pDtVSLTarget.Rows(i).Item(10)) + " " + Format(pDtVSLTarget.Rows(i).Item(11)))
                                DEC2000 = pAUtil.DMSToDegrees(Format(pDtVSLTarget.Rows(i).Item(12)) + " " + Format(pDtVSLTarget.Rows(i).Item(13)) + " " + Format(pDtVSLTarget.Rows(i).Item(14)))

                                returnvalue = CalculateObject(RA2000, DEC2000)
                                If returnvalue <> "OK" Then
                                    DatabaseSelectHADS = returnvalue
                                    Exit Function
                                End If
                                pDtVSLTarget.Rows(i).Item(43) = pStructObject.ObjectAlt
                                pDtVSLTarget.Rows(i).Item(44) = pStructObject.ObjectAz
                                pDtVSLTarget.Rows(i).Item(45) = pStructObject.ObjectCompasIndex
                                pDtVSLTarget.Rows(i).Item(46) = pStructObject.ObjectCompasDirection

                                If My.Settings.sObserverLatitude + DEC2000 > 90 Then
                                    'circumpolar
                                    pDtVSLTarget.Rows(i).Item(47) = True
                                Else
                                    'not circumpolar
                                    pDtVSLTarget.Rows(i).Item(47) = False
                                End If

                                Dim sortorder As Double
                                'lowest sortorder will be selected first
                                If My.Settings.sObserverLatitude + DEC2000 > 90 Then
                                    'circumpolar, should be second choice
                                    If pStructObject.ObjectAz <= 180 Then
                                        'use azimuth desc as sortorder
                                        sortorder = 500 + pStructObject.ObjectAz
                                    Else
                                        'sort by asc azimuth, lowest first
                                        sortorder = pStructObject.ObjectAz
                                    End If
                                Else
                                    'not circumpolar, first choice, sort by az desc and number of frames to go: targets with a low number of frames to go will be favored
                                    sortorder = pStructObject.ObjectAz
                                End If

                                '------------------------------------------------------
                                'bring in the already observed this month factor
                                ' - last observed by me: big penalty
                                '------------------------------------------------------
                                Dim currentMonth, lastObserved As Integer

                                currentMonth = DatePart("m", Now)
                                If pDtVSLTarget.Rows(i).Item(41) Is "" Then
                                    lastObserved = 0
                                Else
                                    'if observed this year
                                    If DatePart("yyyy", pDtVSLTarget.Rows(i).Item(41)) = DatePart("yyyy", Now) Then
                                        lastObserved = DatePart("m", pDtVSLTarget.Rows(i).Item(41))
                                    Else
                                        lastObserved = 0
                                    End If

                                    If Convert.ToDateTime(pDtVSLTarget.Rows(i).Item(41)).ToShortDateString = Now.ToShortDateString And
                                        DateDiff(DateInterval.Hour, Convert.ToDateTime(pDtVSLTarget.Rows(i).Item(41)), Now) < 12 Then
                                        'was observing earlier today, loose weight
                                        sortorder -= 1000
                                    ElseIf currentMonth = lastObserved Then
                                        'already observed this month, add some weight
                                        sortorder += 1000
                                    End If
                                End If


                                '------------------------------------------------------
                                ' - last observed by someone else: the penalty is less
                                '------------------------------------------------------
                                If currentMonth = 1 Then
                                    If pDtVSLTarget.Rows(i).Item(18) IsNot "" Then
                                        sortorder += 750
                                    End If
                                ElseIf currentMonth = 2 Then
                                    If pDtVSLTarget.Rows(i).Item(19) IsNot "" Then
                                        sortorder += 750
                                    End If
                                ElseIf currentMonth = 3 Then
                                    If pDtVSLTarget.Rows(i).Item(20) IsNot "" Then
                                        sortorder += 750
                                    End If
                                ElseIf currentMonth = 4 Then
                                    If pDtVSLTarget.Rows(i).Item(21) IsNot "" Then
                                        sortorder += 750
                                    End If
                                ElseIf currentMonth = 5 Then
                                    If pDtVSLTarget.Rows(i).Item(22) IsNot "" Then
                                        sortorder += 750
                                    End If
                                ElseIf currentMonth = 6 Then
                                    If pDtVSLTarget.Rows(i).Item(23) IsNot "" Then
                                        sortorder += 750
                                    End If
                                ElseIf currentMonth = 7 Then
                                    If pDtVSLTarget.Rows(i).Item(24) IsNot "" Then
                                        sortorder += 750
                                    End If
                                ElseIf currentMonth = 8 Then
                                    If pDtVSLTarget.Rows(i).Item(25) IsNot "" Then
                                        sortorder += 750
                                    End If
                                ElseIf currentMonth = 9 Then
                                    If pDtVSLTarget.Rows(i).Item(26) IsNot "" Then
                                        sortorder += 750
                                    End If
                                ElseIf currentMonth = 10 Then
                                    If pDtVSLTarget.Rows(i).Item(27) IsNot "" Then
                                        sortorder += 750
                                    End If
                                ElseIf currentMonth = 11 Then
                                    If pDtVSLTarget.Rows(i).Item(28) IsNot "" Then
                                        sortorder += 750
                                    End If
                                ElseIf currentMonth = 12 Then
                                    If pDtVSLTarget.Rows(i).Item(29) IsNot "" Then
                                        sortorder += 750
                                    End If
                                End If
                                pDtVSLTarget.Rows(i).Item(48) = sortorder
                            Next

                            pDtVSLTarget.DefaultView.Sort = "HADSPriority DESC, SortOrder ASC, HADSName ASC"
                            pDtVSLTarget = pDtVSLTarget.DefaultView.ToTable

                            i = 0
                            DatabaseClearHADS()

                            While TargetFound = False And i <= pDtVSLTarget.Rows.Count - 1
                                If (Convert.ToDouble(pDtVSLTarget.Rows(i).Item(43)) >= My.Settings.sObjectAltLimitEast) Then 'target is in order and first one above the altitude limit

                                    pVSLTarget.ID = Convert.ToInt32(pDtVSLTarget.Rows(i).Item(0))
                                    pVSLTarget.HADSName = pDtVSLTarget.Rows(i).Item(1).ToString

                                    pVSLTarget.HADSRA2000HH = pDtVSLTarget.Rows(i).Item(9).ToString
                                    pVSLTarget.HADSRA2000MM = pDtVSLTarget.Rows(i).Item(10).ToString
                                    pVSLTarget.HADSRA2000SS = pDtVSLTarget.Rows(i).Item(11).ToString
                                    pVSLTarget.HADSDEC2000DG = pDtVSLTarget.Rows(i).Item(12).ToString
                                    pVSLTarget.HADSDEC2000MM = pDtVSLTarget.Rows(i).Item(13).ToString
                                    pVSLTarget.HADSDEC2000SS = pDtVSLTarget.Rows(i).Item(14).ToString

                                    pVSLTarget.HADSAltitude = Convert.ToDouble(pDtVSLTarget.Rows(i).Item(43))
                                    pVSLTarget.HADSAzimuth = Convert.ToDouble(pDtVSLTarget.Rows(i).Item(44))

                                    pVSLTarget.HADSExposure = Convert.ToDouble(pDtVSLTarget.Rows(i).Item(15))
                                    pVSLTarget.HADSBin = pDtVSLTarget.Rows(i).Item(16).ToString
                                    pVSLTarget.HADSFilter = pDtVSLTarget.Rows(i).Item(17).ToString
                                    pVSLTarget.HADSCompassDirection = pDtVSLTarget.Rows(i).Item(46).ToString

                                    pVSLTarget.FocusStarName = pDtVSLTarget.Rows(i).Item(30).ToString
                                    pVSLTarget.FocusStarRA2000HH = pDtVSLTarget.Rows(i).Item(31).ToString
                                    pVSLTarget.FocusStarRA2000MM = pDtVSLTarget.Rows(i).Item(32).ToString
                                    pVSLTarget.FocusStarRA2000SS = pDtVSLTarget.Rows(i).Item(33).ToString
                                    pVSLTarget.FocusStarDEC2000DG = pDtVSLTarget.Rows(i).Item(34).ToString
                                    pVSLTarget.FocusStarDEC2000MM = pDtVSLTarget.Rows(i).Item(35).ToString
                                    pVSLTarget.FocusStarDEC2000SS = pDtVSLTarget.Rows(i).Item(36).ToString
                                    pVSLTarget.FocusStarExposure = Convert.ToDouble(pDtVSLTarget.Rows(i).Item(37))

                                    Exit While
                                End If
                                i += 1
                            End While

                            If vCheckOnly = False Then
                                LogSessionEntry("ESSENTIAL", pVSLTarget.HADSName + " selected. Alt " + Format(pVSLTarget.HADSAltitude, "#0.00") + "° - Az " + Format(pVSLTarget.HADSAzimuth, "#0.00") + "° Target: " + pVSLTarget.HADSCompassDirection + " Moon: " + pStructEventTimes.MoonCompassDirection, "", "DatabaseSLSelectDeepSky", "PROGRAM")
                            End If
                        Catch ex As Exception
                            DatabaseSelectHADS = "DatabaseSelectHADS: " + ex.Message
                            LogSessionEntry("ERROR", "DatabaseSelectHADS: " + ex.Message, "", "DatabaseSelectHADS", "PROGRAM")
                        End Try
                    End Using
                End Using
            End Using
            FrmMain.Cursor = Cursors.Default

            executionTime = DateTime.UtcNow() - startExecution
            LogSessionEntry("DEBUG", "  DatabaseSelectHADS: " + executionTime.ToString, "", "DatabaseSelectHADS", "PROGRAM")

        Catch ex As Exception
            LogSessionEntry("ERROR", "DatabaseSelectHADS: " + ex.Message, "", "DatabaseSelectHADS", "PROGRAM")
        End Try

    End Function

    Private Sub DatabaseClearHADS()
        Dim startExecution As Date
        Dim executionTime As TimeSpan

        startExecution = DateTime.UtcNow()
        LogSessionEntry("DEBUG", "  DatabaseClearHADS...", "", "DatabaseClearHADS", "PROGRAM")

        pVSLTarget.ID = Nothing
        pVSLTarget.HADSName = Nothing
        pVSLTarget.HADSName2 = Nothing
        pVSLTarget.HADSName3 = Nothing
        pVSLTarget.HADSName4 = Nothing
        pVSLTarget.HADSName5 = Nothing
        pVSLTarget.HADSActive = Nothing
        pVSLTarget.HADSMin = Nothing
        pVSLTarget.HADSMax = Nothing
        pVSLTarget.HADSPeriod = Nothing
        pVSLTarget.HADSRA2000HH = Nothing
        pVSLTarget.HADSRA2000MM = Nothing
        pVSLTarget.HADSRA2000SS = Nothing
        pVSLTarget.HADSDEC2000DG = Nothing
        pVSLTarget.HADSDEC2000MM = Nothing
        pVSLTarget.HADSDEC2000SS = Nothing
        pVSLTarget.HADSExposure = Nothing
        pVSLTarget.HADSBin = Nothing
        pVSLTarget.HADSFilter = Nothing
        pVSLTarget.HADSJan = Nothing
        pVSLTarget.HADSFeb = Nothing
        pVSLTarget.HADSMar = Nothing
        pVSLTarget.HADSApr = Nothing
        pVSLTarget.HADSMay = Nothing
        pVSLTarget.HADSJun = Nothing
        pVSLTarget.HADSJul = Nothing
        pVSLTarget.HADSAug = Nothing
        pVSLTarget.HADSSep = Nothing
        pVSLTarget.HADSOct = Nothing
        pVSLTarget.HADSNov = Nothing
        pVSLTarget.HADSDec = Nothing
        pVSLTarget.HADSLastObserved = Nothing
        pVSLTarget.FocusStarName = Nothing
        pVSLTarget.FocusStarRA2000HH = Nothing
        pVSLTarget.FocusStarRA2000MM = Nothing
        pVSLTarget.FocusStarRA2000SS = Nothing
        pVSLTarget.FocusStarDEC2000DG = Nothing
        pVSLTarget.FocusStarDEC2000MM = Nothing
        pVSLTarget.FocusStarDEC2000SS = Nothing
        pVSLTarget.FocusStarExposure = Nothing
        pVSLTarget.HADSPriority = Nothing
        pVSLTarget.HADSError = Nothing
        pVSLTarget.HADSErrorText = Nothing
        pVSLTarget.HADSAltitude = Nothing
        pVSLTarget.HADSAzimuth = Nothing
        pVSLTarget.HADSCompassIndex = Nothing
        pVSLTarget.HADSCompassDirection = Nothing
        pVSLTarget.HADSCircumpolar = Nothing
        pVSLTarget.HADSCircumPriority = Nothing

        executionTime = DateTime.UtcNow() - startExecution
        LogSessionEntry("DEBUG", "  DatabaseClearHADS: " + executionTime.ToString, "", "DatabaseClearHADS", "PROGRAM")
    End Sub

    Public Function DatabaseMarkErrorHADS(vReason As String) As String
        Dim i As Integer
        Dim startExecution As Date
        Dim executionTime As TimeSpan

        DatabaseMarkErrorHADS = "OK"
        Try
            startExecution = DateTime.UtcNow()

            If pHADSFailedAttempts >= My.Settings.sHADSFailedAttempts Then
                LogSessionEntry("ERROR", "MULTIPLE FAILED HADS! Marking " + pVSLTarget.HADSName + " as unusable: " + vReason, "", "DatabaseMarkErrorHADS", "PROGRAM")
                pHADSFailedAttempts = 0
            Else
                LogSessionEntry("ESSENTIAL", "Marking " + pVSLTarget.HADSName + " as unusable:  " + vReason, "", "DatabaseMarkErrorHADS", "PROGRAM")
                pHADSFailedAttempts += 1
            End If

            Using pCon As New SQLiteConnection("Data Source=RoboticObservatory.db;Version=3;")
                Using cmd As SQLiteCommand = pCon.CreateCommand
                    Using da As New SQLiteDataAdapter
                        Try
                            pCon.Open()
                            da.SelectCommand = cmd
                            cmd.CommandText = "UPDATE HADS SET HADSError=1, HADSErrorText = '" + vReason + "' WHERE Id = " + Format(pVSLTarget.ID)
                            da.SelectCommand = cmd
                            i = cmd.ExecuteNonQuery
                            pCon.Close()

                        Catch ex As Exception
                            DatabaseMarkErrorHADS = "DatabaseMarkErrorHADS: " + ex.Message
                            LogSessionEntry("ERROR", "DatabaseMarkErrorHADS: " + ex.Message, "", "DatabaseMarkErrorHADS", "PROGRAM")
                        End Try
                    End Using
                End Using
            End Using

            executionTime = DateTime.UtcNow() - startExecution
            LogSessionEntry("DEBUG", "  DatabaseMarkErrorHADS: " + executionTime.ToString, "", "DatabaseMarkErrorHADS", "PROGRAM")

        Catch ex As Exception
            DatabaseMarkErrorHADS = "DatabaseMarkErrorHADS: " + ex.Message
            LogSessionEntry("ERROR", "DatabaseMarkErrorHADS: " + ex.Message, "", "DatabaseMarkErrorHADS", "PROGRAM")
        End Try
    End Function

    Public Function DatabaseSetLastObservedHADS(vID As Integer) As String
        Dim i As Integer
        Dim startExecution As Date
        Dim executionTime As TimeSpan

        DatabaseSetLastObservedHADS = "OK"
        Try
            startExecution = DateTime.UtcNow()
            LogSessionEntry("DEBUG", "  DatabaseSetLastObservedHADS...", "", "DatabaseSetLastObservedHADS", "PROGRAM")

            Using pCon As New SQLiteConnection("Data Source=RoboticObservatory.db;Version=3;")
                Using cmd As SQLiteCommand = pCon.CreateCommand
                    Using daSL As New SQLiteDataAdapter
                        Try
                            pCon.Open()
                            daSL.SelectCommand = cmd
                            cmd.CommandText = "UPDATE HADS set HADSLastObserved='" + DateTime.UtcNow().ToString + "'" + " WHERE Id = " + Format(vID)

                            daSL.SelectCommand = cmd
                            i = cmd.ExecuteNonQuery
                            pCon.Close()

                        Catch ex As Exception
                            DatabaseSetLastObservedHADS = "DatabaseSetLastObservedHADS: " + ex.Message
                            LogSessionEntry("ERROR", "DatabaseSetLastObservedHADS: " + ex.Message, "", "DatabaseSetLastObservedHADS", "PROGRAM")
                        End Try
                    End Using
                End Using
            End Using
            executionTime = DateTime.UtcNow() - startExecution
            LogSessionEntry("DEBUG", "  DatabaseSetLastObservedHADS: " + executionTime.ToString, "", "DatabaseSetLastObservedHADS", "PROGRAM")

        Catch ex As Exception
            LogSessionEntry("ERROR", "DatabaseSetLastObservedHADS: " + ex.Message, "", "DatabaseSetLastObservedHADS", "PROGRAM")
            DatabaseSetLastObservedHADS = ex.Message
        End Try
    End Function

    Public Function DatabaseTruncateHADS() As String
        Dim i As Integer
        Dim startExecution As Date
        Dim executionTime As TimeSpan

        DatabaseTruncateHADS = "OK"
        Try
            startExecution = DateTime.UtcNow()
            LogSessionEntry("DEBUG", "  DatabaseTruncateHADS...", "", "DatabaseTruncateHADS", "PROGRAM")

            Using pCon As New SQLiteConnection("Data Source=RoboticObservatory.db;Version=3;")
                Using cmd As SQLiteCommand = pCon.CreateCommand
                    Using daSL As New SQLiteDataAdapter
                        Try
                            pCon.Open()
                            daSL.SelectCommand = cmd
                            cmd.CommandText = "DELETE from HADS"

                            daSL.SelectCommand = cmd
                            i = cmd.ExecuteNonQuery
                            pCon.Close()

                        Catch ex As Exception
                            DatabaseTruncateHADS = "DatabaseTruncateHADS: " + ex.Message
                            LogSessionEntry("ERROR", "DatabaseTruncateHADS: " + ex.Message, "", "DatabaseTruncateHADS", "PROGRAM")
                        End Try
                    End Using
                End Using
            End Using
            executionTime = DateTime.UtcNow() - startExecution
            LogSessionEntry("DEBUG", "  DatabaseTruncateHADS: " + executionTime.ToString, "", "DatabaseTruncateHADS", "PROGRAM")

        Catch ex As Exception
            LogSessionEntry("ERROR", "DatabaseTruncateHADS: " + ex.Message, "", "DatabaseTruncateHADS", "PROGRAM")
            DatabaseTruncateHADS = ex.Message
        End Try
    End Function


    Public Function DatabaseDeleteInactiveHADS() As String
        Dim i As Integer
        Dim startExecution As Date
        Dim executionTime As TimeSpan

        DatabaseDeleteInactiveHADS = "OK"
        Try
            startExecution = DateTime.UtcNow()
            LogSessionEntry("DEBUG", "  DatabaseDeleteInactiveHADS...", "", "DatabaseDeleteInactiveHADS", "PROGRAM")

            Using pCon As New SQLiteConnection("Data Source=RoboticObservatory.db;Version=3;")
                Using cmd As SQLiteCommand = pCon.CreateCommand
                    Using daSL As New SQLiteDataAdapter
                        Try
                            pCon.Open()
                            daSL.SelectCommand = cmd
                            cmd.CommandText = "DELETE from HADS where HADSActive = 0"

                            daSL.SelectCommand = cmd
                            i = cmd.ExecuteNonQuery
                            pCon.Close()

                        Catch ex As Exception
                            DatabaseDeleteInactiveHADS = "DatabaseDeleteInactiveHADS: " + ex.Message
                            LogSessionEntry("ERROR", "DatabaseDeleteInactiveHADS: " + ex.Message, "", "DatabaseDeleteInactiveHADS", "PROGRAM")
                        End Try
                    End Using
                End Using
            End Using
            executionTime = DateTime.UtcNow() - startExecution
            LogSessionEntry("DEBUG", "  DatabaseDeleteInactiveHADS: " + executionTime.ToString, "", "DatabaseDeleteInactiveHADS", "PROGRAM")

        Catch ex As Exception
            LogSessionEntry("ERROR", "DatabaseDeleteInactiveHADS: " + ex.Message, "", "DatabaseDeleteInactiveHADS", "PROGRAM")
            DatabaseDeleteInactiveHADS = ex.Message
        End Try
    End Function
End Module
