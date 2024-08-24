Imports System.Data.SQLite


'---------------------------------------------------------------------------
' Logic database selection process
'---------------------------------------------------------------------------
' Favor objects in the WEST, until the WEST limit is reached
' Moon: check position of Moon and favor objects that are on the other side of the Moon.
' Will take into account My.Settings.sMoonIgnore property setting.
'---------------------------------------------------------------------------
' Triangle of death
'---------------------------------------------------------------------------

' sorting by azimuth is not the best way to: right above the pole star az becomes 359 and keeps dropping
' therefore every time a circumpolar objects crosses the meridian, the workflow is interrupted and targets are switched
' therefore it is better to seperate both hemnispheres and assign a priority
' if az < 180 => is still in the East = priority 2
' if az > 180 => meridian is crossed = priority 1
' next, objects should sorted to altitude ASC => lowest targets are always favored


'---------------------------------------------------------------------------
' Target Order
'---------------------------------------------------------------------------
'- Any target: whatever target is selected first: default sort is priority, already ongoing, az priority, altitude and targetname
'- Priority target: has priority (triangle of death not taken into account)
'- Normal Target: checks moon position and select target on other side, triangle of death is respected
'- Final selection: priority - normal - any (iow does not care about Moon or triangle of death)


Module ModDatabaseDeepsky

    Public Structure StructSLCurrentRecord
        Public ID As Integer
        Public TargetName As String
        Public TargetRA2000HH As String
        Public TargetRA2000MM As String
        Public TargetRA2000SS As String
        Public TargetDEC2000DG As String
        Public TargetDEC2000MM As String
        Public TargetDEC2000SS As String
        Public TargetExposure As Double
        Public TargetBin As String
        Public TargetFilter As String
        Public TargetDone As Boolean
        Public TargetNbrExposedFrames As Integer
        Public TargetNbrFrames As Integer
        Public TargetPriority As Boolean
        Public TargetIsComet As Boolean 'when comet: at observation time get coordinates from TSX
        Public TargetIgnoreMoon As Boolean
        Public FocusStarName As String
        Public FocusStarRA2000HH As String
        Public FocusStarRA2000MM As String
        Public FocusStarRA2000SS As String
        Public FocusStarDEC2000DG As String
        Public FocusStarDEC2000MM As String
        Public FocusStarDEC2000SS As String
        Public FocusStarExposure As Double
        Public GuideAuto As Boolean
        Public GuideStarXBMF As Integer
        Public GuideStarYBMF As Integer
        Public GuideStarXPMF As Integer
        Public GuideStarYPMF As Integer
        Public GuideStarExposure As Double
        Public TargetAltitude As Double
        Public TargetAzimuth As Double
        Public TargetCompassIndex As Integer
        Public TargetCompassDirection As String
        Public TargetCircumpolar As Boolean
        Public TargetCircumPriority As Integer
        Public ErrorObservingTarget As Boolean
        Public ErrorTextTarget As String
        Public TargetRemarks As String
        Public TargetLastObservedDate As String
        Public TargetMosaic As Boolean
        Public TargetMosaicType As String
        Public TargetMosaicFramesPerPanel As Integer
        Public TargetMosaicOverlap As Integer
        Public TargetPanel1RA2000HH As String
        Public TargetPanel2RA2000HH As String
        Public TargetPanel3RA2000HH As String
        Public TargetPanel4RA2000HH As String
        Public TargetPanel1RA2000MM As String
        Public TargetPanel2RA2000MM As String
        Public TargetPanel3RA2000MM As String
        Public TargetPanel4RA2000MM As String
        Public TargetPanel1RA2000SS As String
        Public TargetPanel2RA2000SS As String
        Public TargetPanel3RA2000SS As String
        Public TargetPanel4RA2000SS As String
        Public TargetPanel1DEC2000DG As String
        Public TargetPanel2DEC2000DG As String
        Public TargetPanel3DEC2000DG As String
        Public TargetPanel4DEC2000DG As String
        Public TargetPanel1DEC2000MM As String
        Public TargetPanel2DEC2000MM As String
        Public TargetPanel3DEC2000MM As String
        Public TargetPanel4DEC2000MM As String
        Public TargetPanel1DEC2000SS As String
        Public TargetPanel2DEC2000SS As String
        Public TargetPanel3DEC2000SS As String
        Public TargetPanel4DEC2000SS As String
        Public TargetPanel1NbrExposedFrames As Integer
        Public TargetPanel2NbrExposedFrames As Integer
        Public TargetPanel3NbrExposedFrames As Integer
        Public TargetPanel4NbrExposedFrames As Integer
    End Structure

    Public pStrucSLCurrentRecord As StructSLCurrentRecord

    Public pDtSLTarget As New DataTable
    Public pDSLTarget As StructSLCurrentRecord 'Deepsky target
    Public pDUSLTarget As StructSLCurrentRecord 'Deepsky target UNSAFE: deepsky targets that can be run when MOON is unsafe

    Public Function DatabaseSelectDeepSky(vCheckOnly As Boolean, vUnsafe As Boolean) As String
        Dim returnvalue As String
        Dim i As Integer
        Dim RA2000, DEC2000 As Double
        Dim NormalTarget, PriorityTarget, AnyTarget As StructSLCurrentRecord
        Dim TargetFound As Boolean = False
        Dim startExecution As Date
        Dim executionTime As TimeSpan

        DatabaseSelectDeepSky = "OK"
        Try
            startExecution = DateTime.UtcNow()
            FrmMain.Cursor = Cursors.WaitCursor

            PriorityTarget.ID = Nothing
            PriorityTarget.TargetName = Nothing
            PriorityTarget.TargetRA2000HH = Nothing
            PriorityTarget.TargetRA2000MM = Nothing
            PriorityTarget.TargetRA2000SS = Nothing
            PriorityTarget.TargetDEC2000DG = Nothing
            PriorityTarget.TargetDEC2000MM = Nothing
            PriorityTarget.TargetDEC2000SS = Nothing
            PriorityTarget.TargetExposure = Nothing
            PriorityTarget.TargetBin = Nothing
            PriorityTarget.TargetFilter = Nothing
            PriorityTarget.TargetDone = Nothing
            PriorityTarget.TargetNbrExposedFrames = Nothing
            PriorityTarget.TargetNbrFrames = Nothing
            PriorityTarget.TargetPriority = Nothing
            PriorityTarget.TargetIsComet = Nothing 'when comet: at observation time get coordinates from TSX
            PriorityTarget.TargetIgnoreMoon = Nothing
            PriorityTarget.FocusStarName = Nothing
            PriorityTarget.FocusStarRA2000HH = Nothing
            PriorityTarget.FocusStarRA2000MM = Nothing
            PriorityTarget.FocusStarRA2000SS = Nothing
            PriorityTarget.FocusStarDEC2000DG = Nothing
            PriorityTarget.FocusStarDEC2000MM = Nothing
            PriorityTarget.FocusStarDEC2000SS = Nothing
            PriorityTarget.FocusStarExposure = Nothing
            PriorityTarget.GuideAuto = Nothing
            PriorityTarget.GuideStarXBMF = Nothing
            PriorityTarget.GuideStarYBMF = Nothing
            PriorityTarget.GuideStarXPMF = Nothing
            PriorityTarget.GuideStarYPMF = Nothing
            PriorityTarget.GuideStarExposure = Nothing
            PriorityTarget.TargetAltitude = Nothing
            PriorityTarget.TargetAzimuth = Nothing
            PriorityTarget.TargetCompassIndex = Nothing
            PriorityTarget.TargetCompassDirection = Nothing
            PriorityTarget.TargetCircumpolar = Nothing
            PriorityTarget.TargetCircumPriority = Nothing
            PriorityTarget.ErrorObservingTarget = Nothing
            PriorityTarget.ErrorTextTarget = Nothing
            PriorityTarget.TargetRemarks = Nothing
            PriorityTarget.TargetLastObservedDate = Nothing
            PriorityTarget.TargetMosaic = Nothing
            PriorityTarget.TargetMosaicType = Nothing
            PriorityTarget.TargetMosaicFramesPerPanel = Nothing
            PriorityTarget.TargetPanel1RA2000HH = Nothing
            PriorityTarget.TargetPanel2RA2000HH = Nothing
            PriorityTarget.TargetPanel3RA2000HH = Nothing
            PriorityTarget.TargetPanel4RA2000HH = Nothing
            PriorityTarget.TargetPanel1RA2000MM = Nothing
            PriorityTarget.TargetPanel2RA2000MM = Nothing
            PriorityTarget.TargetPanel3RA2000MM = Nothing
            PriorityTarget.TargetPanel4RA2000MM = Nothing
            PriorityTarget.TargetPanel1RA2000SS = Nothing
            PriorityTarget.TargetPanel2RA2000SS = Nothing
            PriorityTarget.TargetPanel3RA2000SS = Nothing
            PriorityTarget.TargetPanel4RA2000SS = Nothing
            PriorityTarget.TargetPanel1DEC2000DG = Nothing
            PriorityTarget.TargetPanel2DEC2000DG = Nothing
            PriorityTarget.TargetPanel3DEC2000DG = Nothing
            PriorityTarget.TargetPanel4DEC2000DG = Nothing
            PriorityTarget.TargetPanel1DEC2000MM = Nothing
            PriorityTarget.TargetPanel2DEC2000MM = Nothing
            PriorityTarget.TargetPanel3DEC2000MM = Nothing
            PriorityTarget.TargetPanel4DEC2000MM = Nothing
            PriorityTarget.TargetPanel1DEC2000SS = Nothing
            PriorityTarget.TargetPanel2DEC2000SS = Nothing
            PriorityTarget.TargetPanel3DEC2000SS = Nothing
            PriorityTarget.TargetPanel4DEC2000SS = Nothing
            PriorityTarget.TargetPanel1NbrExposedFrames = Nothing
            PriorityTarget.TargetPanel2NbrExposedFrames = Nothing
            PriorityTarget.TargetPanel3NbrExposedFrames = Nothing
            PriorityTarget.TargetPanel4NbrExposedFrames = Nothing

            NormalTarget.ID = Nothing
            NormalTarget.TargetName = Nothing
            NormalTarget.TargetRA2000HH = Nothing
            NormalTarget.TargetRA2000MM = Nothing
            NormalTarget.TargetRA2000SS = Nothing
            NormalTarget.TargetDEC2000DG = Nothing
            NormalTarget.TargetDEC2000MM = Nothing
            NormalTarget.TargetDEC2000SS = Nothing
            NormalTarget.TargetExposure = Nothing
            NormalTarget.TargetBin = Nothing
            NormalTarget.TargetFilter = Nothing
            NormalTarget.TargetDone = Nothing
            NormalTarget.TargetNbrExposedFrames = Nothing
            NormalTarget.TargetNbrFrames = Nothing
            NormalTarget.TargetPriority = Nothing
            NormalTarget.TargetIsComet = Nothing 'when comet: at observation time get coordinates from TSX
            NormalTarget.TargetIgnoreMoon = Nothing
            NormalTarget.FocusStarName = Nothing
            NormalTarget.FocusStarRA2000HH = Nothing
            NormalTarget.FocusStarRA2000MM = Nothing
            NormalTarget.FocusStarRA2000SS = Nothing
            NormalTarget.FocusStarDEC2000DG = Nothing
            NormalTarget.FocusStarDEC2000MM = Nothing
            NormalTarget.FocusStarDEC2000SS = Nothing
            NormalTarget.FocusStarExposure = Nothing
            NormalTarget.GuideAuto = Nothing
            NormalTarget.GuideStarXBMF = Nothing
            NormalTarget.GuideStarYBMF = Nothing
            NormalTarget.GuideStarXPMF = Nothing
            NormalTarget.GuideStarYPMF = Nothing
            NormalTarget.GuideStarExposure = Nothing
            NormalTarget.TargetAltitude = Nothing
            NormalTarget.TargetAzimuth = Nothing
            NormalTarget.TargetCompassIndex = Nothing
            NormalTarget.TargetCompassDirection = Nothing
            NormalTarget.TargetCircumpolar = Nothing
            NormalTarget.TargetCircumPriority = Nothing
            NormalTarget.ErrorObservingTarget = Nothing
            NormalTarget.ErrorTextTarget = Nothing
            NormalTarget.TargetRemarks = Nothing
            NormalTarget.TargetLastObservedDate = Nothing
            NormalTarget.TargetMosaic = Nothing
            NormalTarget.TargetMosaicType = Nothing
            NormalTarget.TargetMosaicFramesPerPanel = Nothing
            NormalTarget.TargetPanel1RA2000HH = Nothing
            NormalTarget.TargetPanel2RA2000HH = Nothing
            NormalTarget.TargetPanel3RA2000HH = Nothing
            NormalTarget.TargetPanel4RA2000HH = Nothing
            NormalTarget.TargetPanel1RA2000MM = Nothing
            NormalTarget.TargetPanel2RA2000MM = Nothing
            NormalTarget.TargetPanel3RA2000MM = Nothing
            NormalTarget.TargetPanel4RA2000MM = Nothing
            NormalTarget.TargetPanel1RA2000SS = Nothing
            NormalTarget.TargetPanel2RA2000SS = Nothing
            NormalTarget.TargetPanel3RA2000SS = Nothing
            NormalTarget.TargetPanel4RA2000SS = Nothing
            NormalTarget.TargetPanel1DEC2000DG = Nothing
            NormalTarget.TargetPanel2DEC2000DG = Nothing
            NormalTarget.TargetPanel3DEC2000DG = Nothing
            NormalTarget.TargetPanel4DEC2000DG = Nothing
            NormalTarget.TargetPanel1DEC2000MM = Nothing
            NormalTarget.TargetPanel2DEC2000MM = Nothing
            NormalTarget.TargetPanel3DEC2000MM = Nothing
            NormalTarget.TargetPanel4DEC2000MM = Nothing
            NormalTarget.TargetPanel1DEC2000SS = Nothing
            NormalTarget.TargetPanel2DEC2000SS = Nothing
            NormalTarget.TargetPanel3DEC2000SS = Nothing
            NormalTarget.TargetPanel4DEC2000SS = Nothing
            NormalTarget.TargetPanel1NbrExposedFrames = Nothing
            NormalTarget.TargetPanel2NbrExposedFrames = Nothing
            NormalTarget.TargetPanel3NbrExposedFrames = Nothing
            NormalTarget.TargetPanel4NbrExposedFrames = Nothing

            AnyTarget.ID = Nothing
            AnyTarget.TargetName = Nothing
            AnyTarget.TargetRA2000HH = Nothing
            AnyTarget.TargetRA2000MM = Nothing
            AnyTarget.TargetRA2000SS = Nothing
            AnyTarget.TargetDEC2000DG = Nothing
            AnyTarget.TargetDEC2000MM = Nothing
            AnyTarget.TargetDEC2000SS = Nothing
            AnyTarget.TargetExposure = Nothing
            AnyTarget.TargetBin = Nothing
            AnyTarget.TargetFilter = Nothing
            AnyTarget.TargetDone = Nothing
            AnyTarget.TargetNbrExposedFrames = Nothing
            AnyTarget.TargetNbrFrames = Nothing
            AnyTarget.TargetPriority = Nothing
            AnyTarget.TargetIsComet = Nothing 'when comet: at observation time get coordinates from TSX
            AnyTarget.TargetIgnoreMoon = Nothing
            AnyTarget.FocusStarName = Nothing
            AnyTarget.FocusStarRA2000HH = Nothing
            AnyTarget.FocusStarRA2000MM = Nothing
            AnyTarget.FocusStarRA2000SS = Nothing
            AnyTarget.FocusStarDEC2000DG = Nothing
            AnyTarget.FocusStarDEC2000MM = Nothing
            AnyTarget.FocusStarDEC2000SS = Nothing
            AnyTarget.FocusStarExposure = Nothing
            AnyTarget.GuideAuto = Nothing
            AnyTarget.GuideStarXBMF = Nothing
            AnyTarget.GuideStarYBMF = Nothing
            AnyTarget.GuideStarXPMF = Nothing
            AnyTarget.GuideStarYPMF = Nothing
            AnyTarget.GuideStarExposure = Nothing
            AnyTarget.TargetAltitude = Nothing
            AnyTarget.TargetAzimuth = Nothing
            AnyTarget.TargetCompassIndex = Nothing
            AnyTarget.TargetCompassDirection = Nothing
            AnyTarget.TargetCircumpolar = Nothing
            AnyTarget.TargetCircumPriority = Nothing
            AnyTarget.ErrorObservingTarget = Nothing
            AnyTarget.ErrorTextTarget = Nothing
            AnyTarget.TargetRemarks = Nothing
            AnyTarget.TargetLastObservedDate = Nothing
            AnyTarget.TargetMosaic = Nothing
            AnyTarget.TargetMosaicType = Nothing
            AnyTarget.TargetMosaicFramesPerPanel = Nothing
            AnyTarget.TargetPanel1RA2000HH = Nothing
            AnyTarget.TargetPanel2RA2000HH = Nothing
            AnyTarget.TargetPanel3RA2000HH = Nothing
            AnyTarget.TargetPanel4RA2000HH = Nothing
            AnyTarget.TargetPanel1RA2000MM = Nothing
            AnyTarget.TargetPanel2RA2000MM = Nothing
            AnyTarget.TargetPanel3RA2000MM = Nothing
            AnyTarget.TargetPanel4RA2000MM = Nothing
            AnyTarget.TargetPanel1RA2000SS = Nothing
            AnyTarget.TargetPanel2RA2000SS = Nothing
            AnyTarget.TargetPanel3RA2000SS = Nothing
            AnyTarget.TargetPanel4RA2000SS = Nothing
            AnyTarget.TargetPanel1DEC2000DG = Nothing
            AnyTarget.TargetPanel2DEC2000DG = Nothing
            AnyTarget.TargetPanel3DEC2000DG = Nothing
            AnyTarget.TargetPanel4DEC2000DG = Nothing
            AnyTarget.TargetPanel1DEC2000MM = Nothing
            AnyTarget.TargetPanel2DEC2000MM = Nothing
            AnyTarget.TargetPanel3DEC2000MM = Nothing
            AnyTarget.TargetPanel4DEC2000MM = Nothing
            AnyTarget.TargetPanel1DEC2000SS = Nothing
            AnyTarget.TargetPanel2DEC2000SS = Nothing
            AnyTarget.TargetPanel3DEC2000SS = Nothing
            AnyTarget.TargetPanel4DEC2000SS = Nothing
            AnyTarget.TargetPanel1NbrExposedFrames = Nothing
            AnyTarget.TargetPanel2NbrExposedFrames = Nothing
            AnyTarget.TargetPanel3NbrExposedFrames = Nothing
            AnyTarget.TargetPanel4NbrExposedFrames = Nothing

            pDSLTarget.ID = Nothing
            pDSLTarget.TargetName = Nothing
            pDSLTarget.TargetRA2000HH = Nothing
            pDSLTarget.TargetRA2000MM = Nothing
            pDSLTarget.TargetRA2000SS = Nothing
            pDSLTarget.TargetDEC2000DG = Nothing
            pDSLTarget.TargetDEC2000MM = Nothing
            pDSLTarget.TargetDEC2000SS = Nothing
            pDSLTarget.TargetExposure = Nothing
            pDSLTarget.TargetBin = Nothing
            pDSLTarget.TargetFilter = Nothing
            pDSLTarget.TargetDone = Nothing
            pDSLTarget.TargetNbrExposedFrames = Nothing
            pDSLTarget.TargetNbrFrames = Nothing
            pDSLTarget.TargetPriority = Nothing
            pDSLTarget.TargetIsComet = Nothing 'when comet: at observation time get coordinates from TSX
            pDSLTarget.TargetIgnoreMoon = Nothing
            pDSLTarget.FocusStarName = Nothing
            pDSLTarget.FocusStarRA2000HH = Nothing
            pDSLTarget.FocusStarRA2000MM = Nothing
            pDSLTarget.FocusStarRA2000SS = Nothing
            pDSLTarget.FocusStarDEC2000DG = Nothing
            pDSLTarget.FocusStarDEC2000MM = Nothing
            pDSLTarget.FocusStarDEC2000SS = Nothing
            pDSLTarget.FocusStarExposure = Nothing
            pDSLTarget.GuideAuto = Nothing
            pDSLTarget.GuideStarXBMF = Nothing
            pDSLTarget.GuideStarYBMF = Nothing
            pDSLTarget.GuideStarXPMF = Nothing
            pDSLTarget.GuideStarYPMF = Nothing
            pDSLTarget.GuideStarExposure = Nothing
            pDSLTarget.TargetAltitude = Nothing
            pDSLTarget.TargetAzimuth = Nothing
            pDSLTarget.TargetCompassIndex = Nothing
            pDSLTarget.TargetCompassDirection = Nothing
            pDSLTarget.TargetCircumpolar = Nothing
            pDSLTarget.TargetCircumPriority = Nothing
            pDSLTarget.ErrorObservingTarget = Nothing
            pDSLTarget.ErrorTextTarget = Nothing
            pDSLTarget.TargetRemarks = Nothing
            pDSLTarget.TargetLastObservedDate = Nothing
            pDSLTarget.TargetMosaic = Nothing
            pDSLTarget.TargetMosaicType = Nothing
            pDSLTarget.TargetMosaicFramesPerPanel = Nothing
            pDSLTarget.TargetPanel1RA2000HH = Nothing
            pDSLTarget.TargetPanel2RA2000HH = Nothing
            pDSLTarget.TargetPanel3RA2000HH = Nothing
            pDSLTarget.TargetPanel4RA2000HH = Nothing
            pDSLTarget.TargetPanel1RA2000MM = Nothing
            pDSLTarget.TargetPanel2RA2000MM = Nothing
            pDSLTarget.TargetPanel3RA2000MM = Nothing
            pDSLTarget.TargetPanel4RA2000MM = Nothing
            pDSLTarget.TargetPanel1RA2000SS = Nothing
            pDSLTarget.TargetPanel2RA2000SS = Nothing
            pDSLTarget.TargetPanel3RA2000SS = Nothing
            pDSLTarget.TargetPanel4RA2000SS = Nothing
            pDSLTarget.TargetPanel1DEC2000DG = Nothing
            pDSLTarget.TargetPanel2DEC2000DG = Nothing
            pDSLTarget.TargetPanel3DEC2000DG = Nothing
            pDSLTarget.TargetPanel4DEC2000DG = Nothing
            pDSLTarget.TargetPanel1DEC2000MM = Nothing
            pDSLTarget.TargetPanel2DEC2000MM = Nothing
            pDSLTarget.TargetPanel3DEC2000MM = Nothing
            pDSLTarget.TargetPanel4DEC2000MM = Nothing
            pDSLTarget.TargetPanel1DEC2000SS = Nothing
            pDSLTarget.TargetPanel2DEC2000SS = Nothing
            pDSLTarget.TargetPanel3DEC2000SS = Nothing
            pDSLTarget.TargetPanel4DEC2000SS = Nothing
            pDSLTarget.TargetPanel1NbrExposedFrames = Nothing
            pDSLTarget.TargetPanel2NbrExposedFrames = Nothing
            pDSLTarget.TargetPanel3NbrExposedFrames = Nothing
            pDSLTarget.TargetPanel4NbrExposedFrames = Nothing


            Using pCon As New SQLiteConnection("Data Source=RoboticObservatory.db;Version=3;")
                Using cmd As SQLiteCommand = pCon.CreateCommand
                    Using da As New SQLiteDataAdapter
                        Try
                            da.SelectCommand = cmd

                            If vUnsafe = False Then
                                cmd.CommandText = "Select Id, TargetName, TargetRA2000HH, TargetRA2000MM, TargetRA2000SS, TargetDEC2000DG, TargetDEC2000MM, TargetDEC2000SS, " +
                                                  "TargetExposure, TargetBin, TargetFilter, TargetDone, TargetNbrFrames, TargetNbrExposedFrames, " +
                                                  "TargetPriority, TargetIsComet, TargetIgnoreMoon, " +
                                                  "FocusStarName, FocusStarRA2000HH, FocusStarRA2000MM, FocusStarRA2000SS, FocusStarDEC2000DG, FocusStarDEC2000MM, FocusStarDEC2000SS, FocusStarExposure, " +
                                                  "GuideAuto, GuidestarXBMF, GuidestarYBMF, GuidestarXPMF, GuidestarYPMF, GuideExposure, " +
                                                  "0.01 as Altitude, 0.01 as Azimuth, 0 as TargetCompassIndex, '' as TargetCompassDirection, False as Circumpolar, 0 as TargetCircumPriority,  0 as TargetOngoing, " +
                                                  "TargetMosaic, TargetMosaicType, TargetMosaicFramesPerPanel, " +
                                                  "TargetPanel1RA2000HH, TargetPanel1RA2000MM, TargetPanel1RA2000SS, TargetPanel1DEC2000DG, TargetPanel1DEC2000MM, TargetPanel1DEC2000SS, " +
                                                  "TargetPanel2RA2000HH, TargetPanel2RA2000MM, TargetPanel2RA2000SS, TargetPanel2DEC2000DG, TargetPanel2DEC2000MM, TargetPanel2DEC2000SS, " +
                                                  "TargetPanel3RA2000HH, TargetPanel3RA2000MM, TargetPanel3RA2000SS, TargetPanel3DEC2000DG, TargetPanel3DEC2000MM, TargetPanel3DEC2000SS, " +
                                                  "TargetPanel4RA2000HH, TargetPanel4RA2000MM, TargetPanel4RA2000SS, TargetPanel4DEC2000DG, TargetPanel4DEC2000MM, TargetPanel4DEC2000SS, " +
                                                  "TargetPanel1NbrExposedFrames, TargetPanel2NbrExposedFrames, TargetPanel3NbrExposedFrames, TargetPanel4NbrExposedFrames  " +
                                                  "From Target where TargetDone=False and TargetError=False order by TargetPriority desc" ' and TargetNbrExposedFrames < TargetNbrFrames order by TargetPriority desc"
                            Else
                                cmd.CommandText = "Select Id, TargetName, TargetRA2000HH, TargetRA2000MM, TargetRA2000SS, TargetDEC2000DG, TargetDEC2000MM, TargetDEC2000SS, " +
                                                  "TargetExposure, TargetBin, TargetFilter, TargetDone, TargetNbrFrames, TargetNbrExposedFrames, " +
                                                  "TargetPriority, TargetIsComet, TargetIgnoreMoon, " +
                                                  "FocusStarName, FocusStarRA2000HH, FocusStarRA2000MM, FocusStarRA2000SS, FocusStarDEC2000DG, FocusStarDEC2000MM, FocusStarDEC2000SS, FocusStarExposure, " +
                                                  "GuideAuto, GuidestarXBMF, GuidestarYBMF, GuidestarXPMF, GuidestarYPMF, GuideExposure, " +
                                                  "0.01 as Altitude, 0.01 as Azimuth, 0 as TargetCompassIndex, '' as TargetCompassDirection, False as Circumpolar, 0 as TargetCircumPriority, 0 as TargetOngoing, " +
                                                  "TargetMosaic, TargetMosaicType, TargetMosaicFramesPerPanel, " +
                                                  "TargetPanel1RA2000HH, TargetPanel1RA2000MM, TargetPanel1RA2000SS, TargetPanel1DEC2000DG, TargetPanel1DEC2000MM, TargetPanel1DEC2000SS, " +
                                                  "TargetPanel2RA2000HH, TargetPanel2RA2000MM, TargetPanel2RA2000SS, TargetPanel2DEC2000DG, TargetPanel2DEC2000MM, TargetPanel2DEC2000SS, " +
                                                  "TargetPanel3RA2000HH, TargetPanel3RA2000MM, TargetPanel3RA2000SS, TargetPanel3DEC2000DG, TargetPanel3DEC2000MM, TargetPanel3DEC2000SS, " +
                                                  "TargetPanel4RA2000HH, TargetPanel4RA2000MM, TargetPanel4RA2000SS, TargetPanel4DEC2000DG, TargetPanel4DEC2000MM, TargetPanel4DEC2000SS, " +
                                                  "TargetPanel1NbrExposedFrames, TargetPanel2NbrExposedFrames, TargetPanel3NbrExposedFrames, TargetPanel4NbrExposedFrames  " +
                                                  "From Target where TargetDone=False And TargetError=False And TargetIgnoreMoon = True order by TargetPriority desc" 'And TargetNbrExposedFrames < TargetNbrFrames And TargetIgnoreMoon = True order by TargetPriority desc"
                            End If

                            da.SelectCommand = cmd
                            pDtSLTarget.Clear()
                            da.Fill(pDtSLTarget)

                            For i = 0 To pDtSLTarget.Rows.Count - 1
                                'caculate actual object position                               
                                RA2000 = pAUtil.HMSToHours(Format(pDtSLTarget.Rows(i).Item(2)) + " " + Format(pDtSLTarget.Rows(i).Item(3)) + " " + Format(pDtSLTarget.Rows(i).Item(4)))
                                DEC2000 = pAUtil.DMSToDegrees(Format(pDtSLTarget.Rows(i).Item(5)) + " " + Format(pDtSLTarget.Rows(i).Item(6)) + " " + Format(pDtSLTarget.Rows(i).Item(7)))

                                returnvalue = CalculateObject(RA2000, DEC2000)
                                If returnvalue <> "OK" Then
                                    DatabaseSelectDeepSky = returnvalue
                                    Exit Function
                                End If
                                pDtSLTarget.Rows(i).Item(31) = pStructObject.ObjectAlt
                                pDtSLTarget.Rows(i).Item(32) = pStructObject.ObjectAz

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

                                Select Case pStructObject.ObjectAz
                                    Case 0.00# To 11.25#
                                        pDtSLTarget.Rows(i).Item(33) = 16
                                        pDtSLTarget.Rows(i).Item(34) = "N"
                                    Case 348.75# To 360.0#
                                        pDtSLTarget.Rows(i).Item(33) = 16
                                        pDtSLTarget.Rows(i).Item(34) = "N"
                                    Case 11.25# To 33.75#
                                        pDtSLTarget.Rows(i).Item(33) = 15
                                        pDtSLTarget.Rows(i).Item(34) = "NNE"
                                    Case 33.75# To 56.25#
                                        pDtSLTarget.Rows(i).Item(33) = 14
                                        pDtSLTarget.Rows(i).Item(34) = "NE"
                                    Case 56.25# To 78.75#
                                        pDtSLTarget.Rows(i).Item(33) = 13
                                        pDtSLTarget.Rows(i).Item(34) = "ENE"
                                    Case 78.75# To 101.25#
                                        pDtSLTarget.Rows(i).Item(33) = 12
                                        pDtSLTarget.Rows(i).Item(34) = "E"
                                    Case 101.25# To 123.75#
                                        pDtSLTarget.Rows(i).Item(33) = 11
                                        pDtSLTarget.Rows(i).Item(34) = "ESE"
                                    Case 123.75# To 146.25#
                                        pDtSLTarget.Rows(i).Item(33) = 10
                                        pDtSLTarget.Rows(i).Item(34) = "SE"
                                    Case 146.25# To 168.75#
                                        pDtSLTarget.Rows(i).Item(33) = 9
                                        pDtSLTarget.Rows(i).Item(34) = "SSE"
                                    Case 168.75# To 191.25#
                                        pDtSLTarget.Rows(i).Item(33) = 8
                                        pDtSLTarget.Rows(i).Item(34) = "S"
                                    Case 191.25# To 213.75#
                                        pDtSLTarget.Rows(i).Item(33) = 7
                                        pDtSLTarget.Rows(i).Item(34) = "SSW"
                                    Case 213.75# To 236.25#
                                        pDtSLTarget.Rows(i).Item(33) = 6
                                        pDtSLTarget.Rows(i).Item(34) = "SW"
                                    Case 236.25# To 258.75#
                                        pDtSLTarget.Rows(i).Item(33) = 5
                                        pDtSLTarget.Rows(i).Item(34) = "WSW"
                                    Case 258.75# To 281.25#
                                        pDtSLTarget.Rows(i).Item(33) = 4
                                        pDtSLTarget.Rows(i).Item(34) = "W"
                                    Case 281.25# To 303.75#
                                        pDtSLTarget.Rows(i).Item(33) = 3
                                        pDtSLTarget.Rows(i).Item(34) = "WNW"
                                    Case 303.75# To 326.25#
                                        pDtSLTarget.Rows(i).Item(33) = 2
                                        pDtSLTarget.Rows(i).Item(34) = "NW"
                                    Case 326.25# To 348.75#
                                        pDtSLTarget.Rows(i).Item(33) = 1
                                        pDtSLTarget.Rows(i).Item(34) = "NNW"
                                End Select

                                If My.Settings.sObserverLatitude + DEC2000 > 90 Then
                                    'circumpolar
                                    pDtSLTarget.Rows(i).Item(35) = True
                                Else
                                    'not circumpolar
                                    pDtSLTarget.Rows(i).Item(35) = False
                                End If

                                Dim sortorder As Double
                                'lowest sortorder will be selected first
                                If My.Settings.sObserverLatitude + DEC2000 > 90 Then
                                    'circumpolar, should be second choice
                                    If pStructObject.ObjectAz <= 180 Then
                                        'use azimuth desc as sortorder
                                        sortorder = 1000 + 500 - pStructObject.ObjectAz
                                    Else
                                        'sort by asc altitude, lowest first
                                        sortorder = 1000 + pStructObject.ObjectAlt
                                    End If
                                Else
                                    'not circumpolar, first choice, sort by az desc and number of frames to go: targets with a low number of frames to go will be favored
                                    Dim TargetNbrFramesToGo As Integer
                                    TargetNbrFramesToGo = Convert.ToInt32(pDtSLTarget.Rows(i).Item(11)) - Convert.ToInt32(pDtSLTarget.Rows(i).Item(12))
                                    sortorder = 500 - pStructObject.ObjectAz + TargetNbrFramesToGo
                                End If
                                pDtSLTarget.Rows(i).Item(36) = sortorder

                                'prefer targets that are already ongoing: normal and mosaic
                                If Convert.ToInt32(pDtSLTarget.Rows(i).Item(13)) > 0 Or
                                   Convert.ToInt32(pDtSLTarget.Rows(i).Item(65)) + Convert.ToInt32(pDtSLTarget.Rows(i).Item(66)) + Convert.ToInt32(pDtSLTarget.Rows(i).Item(67)) + Convert.ToInt32(pDtSLTarget.Rows(i).Item(68)) > 0 Then
                                    pDtSLTarget.Rows(i).Item(37) = 1
                                Else
                                    pDtSLTarget.Rows(i).Item(37) = 0
                                End If
                            Next

                            pDtSLTarget.DefaultView.Sort = "TargetPriority DESC, TargetOngoing DESC, TargetCircumPriority ASC, TargetNbrExposedFrames DESC, TargetName ASC"
                            pDtSLTarget = pDtSLTarget.DefaultView.ToTable

                            'for debug purposes print order
                            If vCheckOnly = False Then
                                i = 0
                                While TargetFound = False And i <= pDtSLTarget.Rows.Count - 1
                                    LogSessionEntry("FULL", "Target: " + pDtSLTarget.Rows(i).Item(1).ToString + " / Alt " + Format(pDtSLTarget.Rows(i).Item(31), "#0.00") + "° - Az " + Format(pDtSLTarget.Rows(i).Item(32), "#0.00") + "° / Compass: " + Format(pDtSLTarget.Rows(i).Item(33)) + " - " +
                                                     pDtSLTarget.Rows(i).Item(34).ToString + " DEC2000: " + Format(pAUtil.DMSToDegrees(Format(pDtSLTarget.Rows(i).Item(5)) + " " + Format(pDtSLTarget.Rows(i).Item(6)) + " " + Format(pDtSLTarget.Rows(i).Item(7))), "#0.00") + "° /  Circumpolar: " + Format(pDtSLTarget.Rows(i).Item(35)), "", "DatabaseSelectDeepSky", "PROGRAM")
                                    i += 1
                                End While
                            End If


                            i = 0
                            While TargetFound = False And i <= pDtSLTarget.Rows.Count - 1
                                If AnyTarget.ID = 0 And
                                   (Convert.ToDouble(pDtSLTarget.Rows(i).Item(31)) >= My.Settings.sObjectAltLimitWest And Convert.ToDouble(pDtSLTarget.Rows(i).Item(33)) < 8) Or 'target is in the West and above the altitude limit
                                   (Convert.ToDouble(pDtSLTarget.Rows(i).Item(31)) >= My.Settings.sObjectAltLimitEast And Convert.ToDouble(pDtSLTarget.Rows(i).Item(33)) >= 8) Then 'target is in the East and above the altitude limit

                                    AnyTarget.ID = Convert.ToInt32(pDtSLTarget.Rows(i).Item(0))
                                    AnyTarget.TargetName = pDtSLTarget.Rows(i).Item(1).ToString
                                    AnyTarget.TargetRA2000HH = pDtSLTarget.Rows(i).Item(2).ToString
                                    AnyTarget.TargetRA2000MM = pDtSLTarget.Rows(i).Item(3).ToString
                                    AnyTarget.TargetRA2000SS = pDtSLTarget.Rows(i).Item(4).ToString
                                    AnyTarget.TargetDEC2000DG = pDtSLTarget.Rows(i).Item(5).ToString
                                    AnyTarget.TargetDEC2000MM = pDtSLTarget.Rows(i).Item(6).ToString
                                    AnyTarget.TargetDEC2000SS = pDtSLTarget.Rows(i).Item(7).ToString
                                    AnyTarget.TargetExposure = Convert.ToDouble(pDtSLTarget.Rows(i).Item(8))
                                    AnyTarget.TargetBin = pDtSLTarget.Rows(i).Item(9).ToString
                                    AnyTarget.TargetFilter = pDtSLTarget.Rows(i).Item(10).ToString
                                    AnyTarget.TargetDone = Convert.ToBoolean(pDtSLTarget.Rows(i).Item(11))
                                    AnyTarget.TargetNbrFrames = Convert.ToInt32(pDtSLTarget.Rows(i).Item(12))
                                    AnyTarget.TargetNbrExposedFrames = Convert.ToInt32(pDtSLTarget.Rows(i).Item(13))
                                    AnyTarget.TargetPriority = Convert.ToBoolean(pDtSLTarget.Rows(i).Item(14))
                                    AnyTarget.TargetIsComet = Convert.ToBoolean(pDtSLTarget.Rows(i).Item(15))
                                    AnyTarget.TargetIgnoreMoon = Convert.ToBoolean(pDtSLTarget.Rows(i).Item(16))
                                    AnyTarget.FocusStarName = pDtSLTarget.Rows(i).Item(17).ToString
                                    AnyTarget.FocusStarRA2000HH = pDtSLTarget.Rows(i).Item(18).ToString
                                    AnyTarget.FocusStarRA2000MM = pDtSLTarget.Rows(i).Item(19).ToString
                                    AnyTarget.FocusStarRA2000SS = pDtSLTarget.Rows(i).Item(20).ToString
                                    AnyTarget.FocusStarDEC2000DG = pDtSLTarget.Rows(i).Item(21).ToString
                                    AnyTarget.FocusStarDEC2000MM = pDtSLTarget.Rows(i).Item(22).ToString
                                    AnyTarget.FocusStarDEC2000SS = pDtSLTarget.Rows(i).Item(23).ToString
                                    AnyTarget.FocusStarExposure = Convert.ToDouble(pDtSLTarget.Rows(i).Item(24))
                                    AnyTarget.GuideAuto = Convert.ToBoolean(pDtSLTarget.Rows(i).Item(25))
                                    AnyTarget.GuideStarXBMF = Convert.ToInt32(pDtSLTarget.Rows(i).Item(26))
                                    AnyTarget.GuideStarYBMF = Convert.ToInt32(pDtSLTarget.Rows(i).Item(27))
                                    AnyTarget.GuideStarXPMF = Convert.ToInt32(pDtSLTarget.Rows(i).Item(28))
                                    AnyTarget.GuideStarYPMF = Convert.ToInt32(pDtSLTarget.Rows(i).Item(29))
                                    AnyTarget.GuideStarExposure = Convert.ToDouble(pDtSLTarget.Rows(i).Item(30))
                                    AnyTarget.TargetAltitude = Convert.ToDouble(pDtSLTarget.Rows(i).Item(31))
                                    AnyTarget.TargetAzimuth = Convert.ToDouble(pDtSLTarget.Rows(i).Item(32))
                                    AnyTarget.TargetCompassIndex = Convert.ToInt32(pDtSLTarget.Rows(i).Item(33))
                                    AnyTarget.TargetCompassDirection = pDtSLTarget.Rows(i).Item(34).ToString
                                    AnyTarget.TargetCircumpolar = Convert.ToBoolean(pDtSLTarget.Rows(i).Item(35))
                                    AnyTarget.TargetMosaic = Convert.ToBoolean(pDtSLTarget.Rows(i).Item(38))
                                    AnyTarget.TargetMosaicType = pDtSLTarget.Rows(i).Item(39).ToString
                                    AnyTarget.TargetMosaicFramesPerPanel = Convert.ToInt32(pDtSLTarget.Rows(i).Item(40))
                                    AnyTarget.TargetPanel1RA2000HH = pDtSLTarget.Rows(i).Item(41).ToString
                                    AnyTarget.TargetPanel1RA2000MM = pDtSLTarget.Rows(i).Item(42).ToString
                                    AnyTarget.TargetPanel1RA2000SS = pDtSLTarget.Rows(i).Item(43).ToString
                                    AnyTarget.TargetPanel1DEC2000DG = pDtSLTarget.Rows(i).Item(44).ToString
                                    AnyTarget.TargetPanel1DEC2000MM = pDtSLTarget.Rows(i).Item(45).ToString
                                    AnyTarget.TargetPanel1DEC2000SS = pDtSLTarget.Rows(i).Item(46).ToString
                                    AnyTarget.TargetPanel2RA2000HH = pDtSLTarget.Rows(i).Item(47).ToString
                                    AnyTarget.TargetPanel2RA2000MM = pDtSLTarget.Rows(i).Item(48).ToString
                                    AnyTarget.TargetPanel2RA2000SS = pDtSLTarget.Rows(i).Item(49).ToString
                                    AnyTarget.TargetPanel2DEC2000DG = pDtSLTarget.Rows(i).Item(50).ToString
                                    AnyTarget.TargetPanel2DEC2000MM = pDtSLTarget.Rows(i).Item(51).ToString
                                    AnyTarget.TargetPanel2DEC2000SS = pDtSLTarget.Rows(i).Item(52).ToString
                                    AnyTarget.TargetPanel3RA2000HH = pDtSLTarget.Rows(i).Item(53).ToString
                                    AnyTarget.TargetPanel3RA2000MM = pDtSLTarget.Rows(i).Item(54).ToString
                                    AnyTarget.TargetPanel3RA2000SS = pDtSLTarget.Rows(i).Item(55).ToString
                                    AnyTarget.TargetPanel3DEC2000DG = pDtSLTarget.Rows(i).Item(56).ToString
                                    AnyTarget.TargetPanel3DEC2000MM = pDtSLTarget.Rows(i).Item(57).ToString
                                    AnyTarget.TargetPanel3DEC2000SS = pDtSLTarget.Rows(i).Item(58).ToString
                                    AnyTarget.TargetPanel4RA2000HH = pDtSLTarget.Rows(i).Item(59).ToString
                                    AnyTarget.TargetPanel4RA2000MM = pDtSLTarget.Rows(i).Item(60).ToString
                                    AnyTarget.TargetPanel4RA2000SS = pDtSLTarget.Rows(i).Item(61).ToString
                                    AnyTarget.TargetPanel4DEC2000DG = pDtSLTarget.Rows(i).Item(62).ToString
                                    AnyTarget.TargetPanel4DEC2000MM = pDtSLTarget.Rows(i).Item(63).ToString
                                    AnyTarget.TargetPanel4DEC2000SS = pDtSLTarget.Rows(i).Item(64).ToString
                                    AnyTarget.TargetPanel1NbrExposedFrames = Convert.ToInt32(pDtSLTarget.Rows(i).Item(65))
                                    AnyTarget.TargetPanel2NbrExposedFrames = Convert.ToInt32(pDtSLTarget.Rows(i).Item(66))
                                    AnyTarget.TargetPanel3NbrExposedFrames = Convert.ToInt32(pDtSLTarget.Rows(i).Item(67))
                                    AnyTarget.TargetPanel4NbrExposedFrames = Convert.ToInt32(pDtSLTarget.Rows(i).Item(68))
                                End If

                                'check position of moon 
                                '&&
                                If pStructEventTimes.MoonAlt < My.Settings.sMoonAltitudeLimitLow Or My.Settings.sMoonIgnore = True Or Convert.ToBoolean(pDtSLTarget.Rows(i).Item(30)) = True Then
                                    'Moon is below horizon or we do not care: get a target in normal order
                                    If (Convert.ToDouble(pDtSLTarget.Rows(i).Item(31)) >= My.Settings.sObjectAltLimitWest And Convert.ToDouble(pDtSLTarget.Rows(i).Item(35)) < 8) Or 'target is in the West and above the altitude limit
                                        (Convert.ToDouble(pDtSLTarget.Rows(i).Item(31)) >= My.Settings.sObjectAltLimitEast And Convert.ToDouble(pDtSLTarget.Rows(i).Item(35)) >= 8) Then 'target is in the East and above the altitude limit

                                        If Convert.ToBoolean(pDtSLTarget.Rows(i).Item(28)) = True Then 'is priority target ?
                                            'priority target
                                            PriorityTarget.ID = Convert.ToInt32(pDtSLTarget.Rows(i).Item(0))
                                            PriorityTarget.TargetName = pDtSLTarget.Rows(i).Item(1).ToString
                                            PriorityTarget.TargetRA2000HH = pDtSLTarget.Rows(i).Item(2).ToString
                                            PriorityTarget.TargetRA2000MM = pDtSLTarget.Rows(i).Item(3).ToString
                                            PriorityTarget.TargetRA2000SS = pDtSLTarget.Rows(i).Item(4).ToString
                                            PriorityTarget.TargetDEC2000DG = pDtSLTarget.Rows(i).Item(5).ToString
                                            PriorityTarget.TargetDEC2000MM = pDtSLTarget.Rows(i).Item(6).ToString
                                            PriorityTarget.TargetDEC2000SS = pDtSLTarget.Rows(i).Item(7).ToString
                                            PriorityTarget.TargetExposure = Convert.ToDouble(pDtSLTarget.Rows(i).Item(8))
                                            PriorityTarget.TargetBin = pDtSLTarget.Rows(i).Item(9).ToString
                                            PriorityTarget.TargetFilter = pDtSLTarget.Rows(i).Item(10).ToString
                                            PriorityTarget.TargetDone = Convert.ToBoolean(pDtSLTarget.Rows(i).Item(11))
                                            PriorityTarget.TargetNbrFrames = Convert.ToInt32(pDtSLTarget.Rows(i).Item(12))
                                            PriorityTarget.TargetNbrExposedFrames = Convert.ToInt32(pDtSLTarget.Rows(i).Item(13))
                                            PriorityTarget.TargetPriority = Convert.ToBoolean(pDtSLTarget.Rows(i).Item(14))
                                            PriorityTarget.TargetIsComet = Convert.ToBoolean(pDtSLTarget.Rows(i).Item(15))
                                            PriorityTarget.TargetIgnoreMoon = Convert.ToBoolean(pDtSLTarget.Rows(i).Item(16))
                                            PriorityTarget.FocusStarName = pDtSLTarget.Rows(i).Item(17).ToString
                                            PriorityTarget.FocusStarRA2000HH = pDtSLTarget.Rows(i).Item(18).ToString
                                            PriorityTarget.FocusStarRA2000MM = pDtSLTarget.Rows(i).Item(19).ToString
                                            PriorityTarget.FocusStarRA2000SS = pDtSLTarget.Rows(i).Item(20).ToString
                                            PriorityTarget.FocusStarDEC2000DG = pDtSLTarget.Rows(i).Item(21).ToString
                                            PriorityTarget.FocusStarDEC2000MM = pDtSLTarget.Rows(i).Item(22).ToString
                                            PriorityTarget.FocusStarDEC2000SS = pDtSLTarget.Rows(i).Item(23).ToString
                                            PriorityTarget.FocusStarExposure = Convert.ToDouble(pDtSLTarget.Rows(i).Item(24))
                                            PriorityTarget.GuideAuto = Convert.ToBoolean(pDtSLTarget.Rows(i).Item(25))
                                            PriorityTarget.GuideStarXBMF = Convert.ToInt32(pDtSLTarget.Rows(i).Item(26))
                                            PriorityTarget.GuideStarYBMF = Convert.ToInt32(pDtSLTarget.Rows(i).Item(27))
                                            PriorityTarget.GuideStarXPMF = Convert.ToInt32(pDtSLTarget.Rows(i).Item(28))
                                            PriorityTarget.GuideStarYPMF = Convert.ToInt32(pDtSLTarget.Rows(i).Item(29))
                                            PriorityTarget.GuideStarExposure = Convert.ToDouble(pDtSLTarget.Rows(i).Item(30))
                                            PriorityTarget.TargetAltitude = Convert.ToDouble(pDtSLTarget.Rows(i).Item(31))
                                            PriorityTarget.TargetAzimuth = Convert.ToDouble(pDtSLTarget.Rows(i).Item(32))
                                            PriorityTarget.TargetCompassIndex = Convert.ToInt32(pDtSLTarget.Rows(i).Item(33))
                                            PriorityTarget.TargetCompassDirection = pDtSLTarget.Rows(i).Item(34).ToString
                                            PriorityTarget.TargetCircumpolar = Convert.ToBoolean(pDtSLTarget.Rows(i).Item(35))
                                            PriorityTarget.TargetMosaic = Convert.ToBoolean(pDtSLTarget.Rows(i).Item(38))
                                            PriorityTarget.TargetMosaicType = pDtSLTarget.Rows(i).Item(39).ToString
                                            PriorityTarget.TargetMosaicFramesPerPanel = Convert.ToInt32(pDtSLTarget.Rows(i).Item(40))
                                            PriorityTarget.TargetPanel1RA2000HH = pDtSLTarget.Rows(i).Item(41).ToString
                                            PriorityTarget.TargetPanel1RA2000MM = pDtSLTarget.Rows(i).Item(42).ToString
                                            PriorityTarget.TargetPanel1RA2000SS = pDtSLTarget.Rows(i).Item(43).ToString
                                            PriorityTarget.TargetPanel1DEC2000DG = pDtSLTarget.Rows(i).Item(44).ToString
                                            PriorityTarget.TargetPanel1DEC2000MM = pDtSLTarget.Rows(i).Item(45).ToString
                                            PriorityTarget.TargetPanel1DEC2000SS = pDtSLTarget.Rows(i).Item(46).ToString
                                            PriorityTarget.TargetPanel2RA2000HH = pDtSLTarget.Rows(i).Item(47).ToString
                                            PriorityTarget.TargetPanel2RA2000MM = pDtSLTarget.Rows(i).Item(48).ToString
                                            PriorityTarget.TargetPanel2RA2000SS = pDtSLTarget.Rows(i).Item(49).ToString
                                            PriorityTarget.TargetPanel2DEC2000DG = pDtSLTarget.Rows(i).Item(50).ToString
                                            PriorityTarget.TargetPanel2DEC2000MM = pDtSLTarget.Rows(i).Item(51).ToString
                                            PriorityTarget.TargetPanel2DEC2000SS = pDtSLTarget.Rows(i).Item(52).ToString
                                            PriorityTarget.TargetPanel3RA2000HH = pDtSLTarget.Rows(i).Item(53).ToString
                                            PriorityTarget.TargetPanel3RA2000MM = pDtSLTarget.Rows(i).Item(54).ToString
                                            PriorityTarget.TargetPanel3RA2000SS = pDtSLTarget.Rows(i).Item(55).ToString
                                            PriorityTarget.TargetPanel3DEC2000DG = pDtSLTarget.Rows(i).Item(56).ToString
                                            PriorityTarget.TargetPanel3DEC2000MM = pDtSLTarget.Rows(i).Item(57).ToString
                                            PriorityTarget.TargetPanel3DEC2000SS = pDtSLTarget.Rows(i).Item(58).ToString
                                            PriorityTarget.TargetPanel4RA2000HH = pDtSLTarget.Rows(i).Item(59).ToString
                                            PriorityTarget.TargetPanel4RA2000MM = pDtSLTarget.Rows(i).Item(60).ToString
                                            PriorityTarget.TargetPanel4RA2000SS = pDtSLTarget.Rows(i).Item(61).ToString
                                            PriorityTarget.TargetPanel4DEC2000DG = pDtSLTarget.Rows(i).Item(62).ToString
                                            PriorityTarget.TargetPanel4DEC2000MM = pDtSLTarget.Rows(i).Item(63).ToString
                                            PriorityTarget.TargetPanel4DEC2000SS = pDtSLTarget.Rows(i).Item(64).ToString
                                            PriorityTarget.TargetPanel1NbrExposedFrames = Convert.ToInt32(pDtSLTarget.Rows(i).Item(65))
                                            PriorityTarget.TargetPanel2NbrExposedFrames = Convert.ToInt32(pDtSLTarget.Rows(i).Item(66))
                                            PriorityTarget.TargetPanel3NbrExposedFrames = Convert.ToInt32(pDtSLTarget.Rows(i).Item(67))
                                            PriorityTarget.TargetPanel4NbrExposedFrames = Convert.ToInt32(pDtSLTarget.Rows(i).Item(68))

                                            If vCheckOnly = False Then
                                                LogSessionEntry("FULL", "PRIORITY TARGET - No Moon: " + pDtSLTarget.Rows(i).Item(1).ToString + " (" + Format(pDtSLTarget.Rows(i).Item(12)) + "/" + Format(pDtSLTarget.Rows(i).Item(11)) + " frames) Alt " + Format(pDtSLTarget.Rows(i).Item(31), "#0.00") + "° - Az " + Format(pDtSLTarget.Rows(i).Item(32), "#0.00") + "° Target: " + pDtSLTarget.Rows(i).Item(34).ToString + " Moon: " + pStructEventTimes.MoonCompassDirection + " Sortorder: " + Format(pDtSLTarget.Rows(i).Item(36)), "", "DatabaseSelectDeepSky", "PROGRAM")
                                            End If
                                            TargetFound = True
                                        Else
                                            'normal target
                                            NormalTarget.ID = Convert.ToInt32(pDtSLTarget.Rows(i).Item(0))
                                            NormalTarget.TargetName = pDtSLTarget.Rows(i).Item(1).ToString
                                            NormalTarget.TargetRA2000HH = pDtSLTarget.Rows(i).Item(2).ToString
                                            NormalTarget.TargetRA2000MM = pDtSLTarget.Rows(i).Item(3).ToString
                                            NormalTarget.TargetRA2000SS = pDtSLTarget.Rows(i).Item(4).ToString
                                            NormalTarget.TargetDEC2000DG = pDtSLTarget.Rows(i).Item(5).ToString
                                            NormalTarget.TargetDEC2000MM = pDtSLTarget.Rows(i).Item(6).ToString
                                            NormalTarget.TargetDEC2000SS = pDtSLTarget.Rows(i).Item(7).ToString
                                            NormalTarget.TargetExposure = Convert.ToDouble(pDtSLTarget.Rows(i).Item(8))
                                            NormalTarget.TargetBin = pDtSLTarget.Rows(i).Item(9).ToString
                                            NormalTarget.TargetFilter = pDtSLTarget.Rows(i).Item(10).ToString
                                            NormalTarget.TargetDone = Convert.ToBoolean(pDtSLTarget.Rows(i).Item(11))
                                            NormalTarget.TargetNbrFrames = Convert.ToInt32(pDtSLTarget.Rows(i).Item(12))
                                            NormalTarget.TargetNbrExposedFrames = Convert.ToInt32(pDtSLTarget.Rows(i).Item(13))
                                            NormalTarget.TargetPriority = Convert.ToBoolean(pDtSLTarget.Rows(i).Item(14))
                                            NormalTarget.TargetIsComet = Convert.ToBoolean(pDtSLTarget.Rows(i).Item(15))
                                            NormalTarget.TargetIgnoreMoon = Convert.ToBoolean(pDtSLTarget.Rows(i).Item(16))
                                            NormalTarget.FocusStarName = pDtSLTarget.Rows(i).Item(17).ToString
                                            NormalTarget.FocusStarRA2000HH = pDtSLTarget.Rows(i).Item(18).ToString
                                            NormalTarget.FocusStarRA2000MM = pDtSLTarget.Rows(i).Item(19).ToString
                                            NormalTarget.FocusStarRA2000SS = pDtSLTarget.Rows(i).Item(20).ToString
                                            NormalTarget.FocusStarDEC2000DG = pDtSLTarget.Rows(i).Item(21).ToString
                                            NormalTarget.FocusStarDEC2000MM = pDtSLTarget.Rows(i).Item(22).ToString
                                            NormalTarget.FocusStarDEC2000SS = pDtSLTarget.Rows(i).Item(23).ToString
                                            NormalTarget.FocusStarExposure = Convert.ToDouble(pDtSLTarget.Rows(i).Item(24))
                                            NormalTarget.GuideAuto = Convert.ToBoolean(pDtSLTarget.Rows(i).Item(25))
                                            NormalTarget.GuideStarXBMF = Convert.ToInt32(pDtSLTarget.Rows(i).Item(26))
                                            NormalTarget.GuideStarYBMF = Convert.ToInt32(pDtSLTarget.Rows(i).Item(27))
                                            NormalTarget.GuideStarXPMF = Convert.ToInt32(pDtSLTarget.Rows(i).Item(28))
                                            NormalTarget.GuideStarYPMF = Convert.ToInt32(pDtSLTarget.Rows(i).Item(29))
                                            NormalTarget.GuideStarExposure = Convert.ToDouble(pDtSLTarget.Rows(i).Item(30))
                                            NormalTarget.TargetAltitude = Convert.ToDouble(pDtSLTarget.Rows(i).Item(31))
                                            NormalTarget.TargetAzimuth = Convert.ToDouble(pDtSLTarget.Rows(i).Item(32))
                                            NormalTarget.TargetCompassIndex = Convert.ToInt32(pDtSLTarget.Rows(i).Item(33))
                                            NormalTarget.TargetCompassDirection = pDtSLTarget.Rows(i).Item(34).ToString
                                            NormalTarget.TargetCircumpolar = Convert.ToBoolean(pDtSLTarget.Rows(i).Item(35))
                                            NormalTarget.TargetMosaic = Convert.ToBoolean(pDtSLTarget.Rows(i).Item(38))
                                            NormalTarget.TargetMosaicType = pDtSLTarget.Rows(i).Item(39).ToString
                                            NormalTarget.TargetMosaicFramesPerPanel = Convert.ToInt32(pDtSLTarget.Rows(i).Item(40))
                                            NormalTarget.TargetPanel1RA2000HH = pDtSLTarget.Rows(i).Item(41).ToString
                                            NormalTarget.TargetPanel1RA2000MM = pDtSLTarget.Rows(i).Item(42).ToString
                                            NormalTarget.TargetPanel1RA2000SS = pDtSLTarget.Rows(i).Item(43).ToString
                                            NormalTarget.TargetPanel1DEC2000DG = pDtSLTarget.Rows(i).Item(44).ToString
                                            NormalTarget.TargetPanel1DEC2000MM = pDtSLTarget.Rows(i).Item(45).ToString
                                            NormalTarget.TargetPanel1DEC2000SS = pDtSLTarget.Rows(i).Item(46).ToString
                                            NormalTarget.TargetPanel2RA2000HH = pDtSLTarget.Rows(i).Item(47).ToString
                                            NormalTarget.TargetPanel2RA2000MM = pDtSLTarget.Rows(i).Item(48).ToString
                                            NormalTarget.TargetPanel2RA2000SS = pDtSLTarget.Rows(i).Item(49).ToString
                                            NormalTarget.TargetPanel2DEC2000DG = pDtSLTarget.Rows(i).Item(50).ToString
                                            NormalTarget.TargetPanel2DEC2000MM = pDtSLTarget.Rows(i).Item(51).ToString
                                            NormalTarget.TargetPanel2DEC2000SS = pDtSLTarget.Rows(i).Item(52).ToString
                                            NormalTarget.TargetPanel3RA2000HH = pDtSLTarget.Rows(i).Item(53).ToString
                                            NormalTarget.TargetPanel3RA2000MM = pDtSLTarget.Rows(i).Item(54).ToString
                                            NormalTarget.TargetPanel3RA2000SS = pDtSLTarget.Rows(i).Item(55).ToString
                                            NormalTarget.TargetPanel3DEC2000DG = pDtSLTarget.Rows(i).Item(56).ToString
                                            NormalTarget.TargetPanel3DEC2000MM = pDtSLTarget.Rows(i).Item(57).ToString
                                            NormalTarget.TargetPanel3DEC2000SS = pDtSLTarget.Rows(i).Item(58).ToString
                                            NormalTarget.TargetPanel4RA2000HH = pDtSLTarget.Rows(i).Item(59).ToString
                                            NormalTarget.TargetPanel4RA2000MM = pDtSLTarget.Rows(i).Item(60).ToString
                                            NormalTarget.TargetPanel4RA2000SS = pDtSLTarget.Rows(i).Item(61).ToString
                                            NormalTarget.TargetPanel4DEC2000DG = pDtSLTarget.Rows(i).Item(62).ToString
                                            NormalTarget.TargetPanel4DEC2000MM = pDtSLTarget.Rows(i).Item(63).ToString
                                            NormalTarget.TargetPanel4DEC2000SS = pDtSLTarget.Rows(i).Item(64).ToString
                                            NormalTarget.TargetPanel1NbrExposedFrames = Convert.ToInt32(pDtSLTarget.Rows(i).Item(65))
                                            NormalTarget.TargetPanel2NbrExposedFrames = Convert.ToInt32(pDtSLTarget.Rows(i).Item(66))
                                            NormalTarget.TargetPanel3NbrExposedFrames = Convert.ToInt32(pDtSLTarget.Rows(i).Item(67))
                                            NormalTarget.TargetPanel4NbrExposedFrames = Convert.ToInt32(pDtSLTarget.Rows(i).Item(68))

                                            If vCheckOnly = False Then
                                                LogSessionEntry("FULL", "Normal target - No Moon: " + pDtSLTarget.Rows(i).Item(1).ToString + " (" + Format(pDtSLTarget.Rows(i).Item(12)) + "/" + Format(pDtSLTarget.Rows(i).Item(11)) + " frames) Alt " + Format(pDtSLTarget.Rows(i).Item(31), "#0.00") + "° - Az " + Format(pDtSLTarget.Rows(i).Item(32), "#0.00") + "° Target: " + pDtSLTarget.Rows(i).Item(34).ToString + " Moon: " + pStructEventTimes.MoonCompassDirection + " Sortorder: " + Format(pDtSLTarget.Rows(i).Item(36)), "", "DatabaseSelectDeepSky", "PROGRAM")
                                            End If

                                            TargetFound = True
                                        End If
                                    Else
                                        'target is to low, do nothing
                                        If vCheckOnly = False Then
                                            LogSessionEntry("FULL", "Not Observable - No Moon: " + pDtSLTarget.Rows(i).Item(1).ToString + " (" + Format(pDtSLTarget.Rows(i).Item(12)) + "/" + Format(pDtSLTarget.Rows(i).Item(11)) + " frames) Alt " + Format(pDtSLTarget.Rows(i).Item(31), "#0.00") + "° - Az " + Format(pDtSLTarget.Rows(i).Item(32), "#0.00") + "° Target: " + pDtSLTarget.Rows(i).Item(32).ToString + " Moon: " + pStructEventTimes.MoonCompassDirection + " Sortorder: " + Format(pDtSLTarget.Rows(i).Item(36)), "", "DatabaseSelectDeepSky", "PROGRAM")
                                        End If
                                    End If
                                Else
                                    'moon above horizon
                                    'change target in function of Moon
                                    If pStructEventTimes.MoonCompassIndex < 8 Then
                                        'Moon is above the horizon in the West, prefer a target from the East
                                        If Convert.ToDouble(pDtSLTarget.Rows(i).Item(33)) >= 8 And Convert.ToDouble(pDtSLTarget.Rows(i).Item(31)) >= My.Settings.sObjectAltLimitEast Then
                                            If Convert.ToBoolean(pDtSLTarget.Rows(i).Item(28)) = True Then
                                                'priority target
                                                PriorityTarget.ID = Convert.ToInt32(pDtSLTarget.Rows(i).Item(0))
                                                PriorityTarget.TargetName = pDtSLTarget.Rows(i).Item(1).ToString
                                                PriorityTarget.TargetRA2000HH = pDtSLTarget.Rows(i).Item(2).ToString
                                                PriorityTarget.TargetRA2000MM = pDtSLTarget.Rows(i).Item(3).ToString
                                                PriorityTarget.TargetRA2000SS = pDtSLTarget.Rows(i).Item(4).ToString
                                                PriorityTarget.TargetDEC2000DG = pDtSLTarget.Rows(i).Item(5).ToString
                                                PriorityTarget.TargetDEC2000MM = pDtSLTarget.Rows(i).Item(6).ToString
                                                PriorityTarget.TargetDEC2000SS = pDtSLTarget.Rows(i).Item(7).ToString
                                                PriorityTarget.TargetExposure = Convert.ToDouble(pDtSLTarget.Rows(i).Item(8))
                                                PriorityTarget.TargetBin = pDtSLTarget.Rows(i).Item(9).ToString
                                                PriorityTarget.TargetFilter = pDtSLTarget.Rows(i).Item(10).ToString
                                                PriorityTarget.TargetDone = Convert.ToBoolean(pDtSLTarget.Rows(i).Item(11))
                                                PriorityTarget.TargetNbrFrames = Convert.ToInt32(pDtSLTarget.Rows(i).Item(12))
                                                PriorityTarget.TargetNbrExposedFrames = Convert.ToInt32(pDtSLTarget.Rows(i).Item(13))
                                                PriorityTarget.TargetPriority = Convert.ToBoolean(pDtSLTarget.Rows(i).Item(14))
                                                PriorityTarget.TargetIsComet = Convert.ToBoolean(pDtSLTarget.Rows(i).Item(15))
                                                PriorityTarget.TargetIgnoreMoon = Convert.ToBoolean(pDtSLTarget.Rows(i).Item(16))
                                                PriorityTarget.FocusStarName = pDtSLTarget.Rows(i).Item(17).ToString
                                                PriorityTarget.FocusStarRA2000HH = pDtSLTarget.Rows(i).Item(18).ToString
                                                PriorityTarget.FocusStarRA2000MM = pDtSLTarget.Rows(i).Item(19).ToString
                                                PriorityTarget.FocusStarRA2000SS = pDtSLTarget.Rows(i).Item(20).ToString
                                                PriorityTarget.FocusStarDEC2000DG = pDtSLTarget.Rows(i).Item(21).ToString
                                                PriorityTarget.FocusStarDEC2000MM = pDtSLTarget.Rows(i).Item(22).ToString
                                                PriorityTarget.FocusStarDEC2000SS = pDtSLTarget.Rows(i).Item(23).ToString
                                                PriorityTarget.FocusStarExposure = Convert.ToDouble(pDtSLTarget.Rows(i).Item(24))
                                                PriorityTarget.GuideAuto = Convert.ToBoolean(pDtSLTarget.Rows(i).Item(25))
                                                PriorityTarget.GuideStarXBMF = Convert.ToInt32(pDtSLTarget.Rows(i).Item(26))
                                                PriorityTarget.GuideStarYBMF = Convert.ToInt32(pDtSLTarget.Rows(i).Item(27))
                                                PriorityTarget.GuideStarXPMF = Convert.ToInt32(pDtSLTarget.Rows(i).Item(28))
                                                PriorityTarget.GuideStarYPMF = Convert.ToInt32(pDtSLTarget.Rows(i).Item(29))
                                                PriorityTarget.GuideStarExposure = Convert.ToDouble(pDtSLTarget.Rows(i).Item(30))
                                                PriorityTarget.TargetAltitude = Convert.ToDouble(pDtSLTarget.Rows(i).Item(31))
                                                PriorityTarget.TargetAzimuth = Convert.ToDouble(pDtSLTarget.Rows(i).Item(32))
                                                PriorityTarget.TargetCompassIndex = Convert.ToInt32(pDtSLTarget.Rows(i).Item(33))
                                                PriorityTarget.TargetCompassDirection = pDtSLTarget.Rows(i).Item(34).ToString
                                                PriorityTarget.TargetCircumpolar = Convert.ToBoolean(pDtSLTarget.Rows(i).Item(35))
                                                PriorityTarget.TargetMosaic = Convert.ToBoolean(pDtSLTarget.Rows(i).Item(38))
                                                PriorityTarget.TargetMosaicType = pDtSLTarget.Rows(i).Item(39).ToString
                                                PriorityTarget.TargetMosaicFramesPerPanel = Convert.ToInt32(pDtSLTarget.Rows(i).Item(40))
                                                PriorityTarget.TargetPanel1RA2000HH = pDtSLTarget.Rows(i).Item(41).ToString
                                                PriorityTarget.TargetPanel1RA2000MM = pDtSLTarget.Rows(i).Item(42).ToString
                                                PriorityTarget.TargetPanel1RA2000SS = pDtSLTarget.Rows(i).Item(43).ToString
                                                PriorityTarget.TargetPanel1DEC2000DG = pDtSLTarget.Rows(i).Item(44).ToString
                                                PriorityTarget.TargetPanel1DEC2000MM = pDtSLTarget.Rows(i).Item(45).ToString
                                                PriorityTarget.TargetPanel1DEC2000SS = pDtSLTarget.Rows(i).Item(46).ToString
                                                PriorityTarget.TargetPanel2RA2000HH = pDtSLTarget.Rows(i).Item(47).ToString
                                                PriorityTarget.TargetPanel2RA2000MM = pDtSLTarget.Rows(i).Item(48).ToString
                                                PriorityTarget.TargetPanel2RA2000SS = pDtSLTarget.Rows(i).Item(49).ToString
                                                PriorityTarget.TargetPanel2DEC2000DG = pDtSLTarget.Rows(i).Item(50).ToString
                                                PriorityTarget.TargetPanel2DEC2000MM = pDtSLTarget.Rows(i).Item(51).ToString
                                                PriorityTarget.TargetPanel2DEC2000SS = pDtSLTarget.Rows(i).Item(52).ToString
                                                PriorityTarget.TargetPanel3RA2000HH = pDtSLTarget.Rows(i).Item(53).ToString
                                                PriorityTarget.TargetPanel3RA2000MM = pDtSLTarget.Rows(i).Item(54).ToString
                                                PriorityTarget.TargetPanel3RA2000SS = pDtSLTarget.Rows(i).Item(55).ToString
                                                PriorityTarget.TargetPanel3DEC2000DG = pDtSLTarget.Rows(i).Item(56).ToString
                                                PriorityTarget.TargetPanel3DEC2000MM = pDtSLTarget.Rows(i).Item(57).ToString
                                                PriorityTarget.TargetPanel3DEC2000SS = pDtSLTarget.Rows(i).Item(58).ToString
                                                PriorityTarget.TargetPanel4RA2000HH = pDtSLTarget.Rows(i).Item(59).ToString
                                                PriorityTarget.TargetPanel4RA2000MM = pDtSLTarget.Rows(i).Item(60).ToString
                                                PriorityTarget.TargetPanel4RA2000SS = pDtSLTarget.Rows(i).Item(61).ToString
                                                PriorityTarget.TargetPanel4DEC2000DG = pDtSLTarget.Rows(i).Item(62).ToString
                                                PriorityTarget.TargetPanel4DEC2000MM = pDtSLTarget.Rows(i).Item(63).ToString
                                                PriorityTarget.TargetPanel4DEC2000SS = pDtSLTarget.Rows(i).Item(64).ToString
                                                PriorityTarget.TargetPanel1NbrExposedFrames = Convert.ToInt32(pDtSLTarget.Rows(i).Item(65))
                                                PriorityTarget.TargetPanel2NbrExposedFrames = Convert.ToInt32(pDtSLTarget.Rows(i).Item(66))
                                                PriorityTarget.TargetPanel3NbrExposedFrames = Convert.ToInt32(pDtSLTarget.Rows(i).Item(67))
                                                PriorityTarget.TargetPanel4NbrExposedFrames = Convert.ToInt32(pDtSLTarget.Rows(i).Item(68))
                                                If vCheckOnly = False Then
                                                    LogSessionEntry("FULL", "PRIORITY TARGET - Moon West/Target East: " + pDtSLTarget.Rows(i).Item(1).ToString + " (" + Format(pDtSLTarget.Rows(i).Item(12)) + "/" + Format(pDtSLTarget.Rows(i).Item(11)) + " frames) Alt " + Format(pDtSLTarget.Rows(i).Item(31), "#0.00") + "° - Az " + Format(pDtSLTarget.Rows(i).Item(32), "#0.00") + "° Target: " + pDtSLTarget.Rows(i).Item(34).ToString + " Moon: " + pStructEventTimes.MoonCompassDirection + " Sortorder: " + Format(pDtSLTarget.Rows(i).Item(36)), "", "DatabaseSelectDeepSky", "PROGRAM")
                                                End If
                                                TargetFound = True
                                            Else
                                                'normal target
                                                NormalTarget.ID = Convert.ToInt32(pDtSLTarget.Rows(i).Item(0))
                                                NormalTarget.TargetName = pDtSLTarget.Rows(i).Item(1).ToString
                                                NormalTarget.TargetRA2000HH = pDtSLTarget.Rows(i).Item(2).ToString
                                                NormalTarget.TargetRA2000MM = pDtSLTarget.Rows(i).Item(3).ToString
                                                NormalTarget.TargetRA2000SS = pDtSLTarget.Rows(i).Item(4).ToString
                                                NormalTarget.TargetDEC2000DG = pDtSLTarget.Rows(i).Item(5).ToString
                                                NormalTarget.TargetDEC2000MM = pDtSLTarget.Rows(i).Item(6).ToString
                                                NormalTarget.TargetDEC2000SS = pDtSLTarget.Rows(i).Item(7).ToString
                                                NormalTarget.TargetExposure = Convert.ToDouble(pDtSLTarget.Rows(i).Item(8))
                                                NormalTarget.TargetBin = pDtSLTarget.Rows(i).Item(9).ToString
                                                NormalTarget.TargetFilter = pDtSLTarget.Rows(i).Item(10).ToString
                                                NormalTarget.TargetDone = Convert.ToBoolean(pDtSLTarget.Rows(i).Item(11))
                                                NormalTarget.TargetNbrFrames = Convert.ToInt32(pDtSLTarget.Rows(i).Item(12))
                                                NormalTarget.TargetNbrExposedFrames = Convert.ToInt32(pDtSLTarget.Rows(i).Item(13))
                                                NormalTarget.TargetPriority = Convert.ToBoolean(pDtSLTarget.Rows(i).Item(14))
                                                NormalTarget.TargetIsComet = Convert.ToBoolean(pDtSLTarget.Rows(i).Item(15))
                                                NormalTarget.TargetIgnoreMoon = Convert.ToBoolean(pDtSLTarget.Rows(i).Item(16))
                                                NormalTarget.FocusStarName = pDtSLTarget.Rows(i).Item(17).ToString
                                                NormalTarget.FocusStarRA2000HH = pDtSLTarget.Rows(i).Item(18).ToString
                                                NormalTarget.FocusStarRA2000MM = pDtSLTarget.Rows(i).Item(19).ToString
                                                NormalTarget.FocusStarRA2000SS = pDtSLTarget.Rows(i).Item(20).ToString
                                                NormalTarget.FocusStarDEC2000DG = pDtSLTarget.Rows(i).Item(21).ToString
                                                NormalTarget.FocusStarDEC2000MM = pDtSLTarget.Rows(i).Item(22).ToString
                                                NormalTarget.FocusStarDEC2000SS = pDtSLTarget.Rows(i).Item(23).ToString
                                                NormalTarget.FocusStarExposure = Convert.ToDouble(pDtSLTarget.Rows(i).Item(24))
                                                NormalTarget.GuideAuto = Convert.ToBoolean(pDtSLTarget.Rows(i).Item(25))
                                                NormalTarget.GuideStarXBMF = Convert.ToInt32(pDtSLTarget.Rows(i).Item(26))
                                                NormalTarget.GuideStarYBMF = Convert.ToInt32(pDtSLTarget.Rows(i).Item(27))
                                                NormalTarget.GuideStarXPMF = Convert.ToInt32(pDtSLTarget.Rows(i).Item(28))
                                                NormalTarget.GuideStarYPMF = Convert.ToInt32(pDtSLTarget.Rows(i).Item(29))
                                                NormalTarget.GuideStarExposure = Convert.ToDouble(pDtSLTarget.Rows(i).Item(30))
                                                NormalTarget.TargetAltitude = Convert.ToDouble(pDtSLTarget.Rows(i).Item(31))
                                                NormalTarget.TargetAzimuth = Convert.ToDouble(pDtSLTarget.Rows(i).Item(32))
                                                NormalTarget.TargetCompassIndex = Convert.ToInt32(pDtSLTarget.Rows(i).Item(33))
                                                NormalTarget.TargetCompassDirection = pDtSLTarget.Rows(i).Item(34).ToString
                                                NormalTarget.TargetCircumpolar = Convert.ToBoolean(pDtSLTarget.Rows(i).Item(35))
                                                NormalTarget.TargetMosaic = Convert.ToBoolean(pDtSLTarget.Rows(i).Item(38))
                                                NormalTarget.TargetMosaicType = pDtSLTarget.Rows(i).Item(39).ToString
                                                NormalTarget.TargetMosaicFramesPerPanel = Convert.ToInt32(pDtSLTarget.Rows(i).Item(40))
                                                NormalTarget.TargetPanel1RA2000HH = pDtSLTarget.Rows(i).Item(41).ToString
                                                NormalTarget.TargetPanel1RA2000MM = pDtSLTarget.Rows(i).Item(42).ToString
                                                NormalTarget.TargetPanel1RA2000SS = pDtSLTarget.Rows(i).Item(43).ToString
                                                NormalTarget.TargetPanel1DEC2000DG = pDtSLTarget.Rows(i).Item(44).ToString
                                                NormalTarget.TargetPanel1DEC2000MM = pDtSLTarget.Rows(i).Item(45).ToString
                                                NormalTarget.TargetPanel1DEC2000SS = pDtSLTarget.Rows(i).Item(46).ToString
                                                NormalTarget.TargetPanel2RA2000HH = pDtSLTarget.Rows(i).Item(47).ToString
                                                NormalTarget.TargetPanel2RA2000MM = pDtSLTarget.Rows(i).Item(48).ToString
                                                NormalTarget.TargetPanel2RA2000SS = pDtSLTarget.Rows(i).Item(49).ToString
                                                NormalTarget.TargetPanel2DEC2000DG = pDtSLTarget.Rows(i).Item(50).ToString
                                                NormalTarget.TargetPanel2DEC2000MM = pDtSLTarget.Rows(i).Item(51).ToString
                                                NormalTarget.TargetPanel2DEC2000SS = pDtSLTarget.Rows(i).Item(52).ToString
                                                NormalTarget.TargetPanel3RA2000HH = pDtSLTarget.Rows(i).Item(53).ToString
                                                NormalTarget.TargetPanel3RA2000MM = pDtSLTarget.Rows(i).Item(54).ToString
                                                NormalTarget.TargetPanel3RA2000SS = pDtSLTarget.Rows(i).Item(55).ToString
                                                NormalTarget.TargetPanel3DEC2000DG = pDtSLTarget.Rows(i).Item(56).ToString
                                                NormalTarget.TargetPanel3DEC2000MM = pDtSLTarget.Rows(i).Item(57).ToString
                                                NormalTarget.TargetPanel3DEC2000SS = pDtSLTarget.Rows(i).Item(58).ToString
                                                NormalTarget.TargetPanel4RA2000HH = pDtSLTarget.Rows(i).Item(59).ToString
                                                NormalTarget.TargetPanel4RA2000MM = pDtSLTarget.Rows(i).Item(60).ToString
                                                NormalTarget.TargetPanel4RA2000SS = pDtSLTarget.Rows(i).Item(61).ToString
                                                NormalTarget.TargetPanel4DEC2000DG = pDtSLTarget.Rows(i).Item(62).ToString
                                                NormalTarget.TargetPanel4DEC2000MM = pDtSLTarget.Rows(i).Item(63).ToString
                                                NormalTarget.TargetPanel4DEC2000SS = pDtSLTarget.Rows(i).Item(64).ToString
                                                NormalTarget.TargetPanel1NbrExposedFrames = Convert.ToInt32(pDtSLTarget.Rows(i).Item(65))
                                                NormalTarget.TargetPanel2NbrExposedFrames = Convert.ToInt32(pDtSLTarget.Rows(i).Item(66))
                                                NormalTarget.TargetPanel3NbrExposedFrames = Convert.ToInt32(pDtSLTarget.Rows(i).Item(67))
                                                NormalTarget.TargetPanel4NbrExposedFrames = Convert.ToInt32(pDtSLTarget.Rows(i).Item(68))
                                                If vCheckOnly = False Then
                                                    LogSessionEntry("FULL", "Normal target - Moon West/Target East: " + pDtSLTarget.Rows(i).Item(1).ToString + " (" + Format(pDtSLTarget.Rows(i).Item(12)) + "/" + Format(pDtSLTarget.Rows(i).Item(11)) + " frames) Alt " + Format(pDtSLTarget.Rows(i).Item(31), "#0.00") + "° - Az " + Format(pDtSLTarget.Rows(i).Item(32), "#0.00") + "° Target: " + pDtSLTarget.Rows(i).Item(34).ToString + " Moon: " + pStructEventTimes.MoonCompassDirection + " Sortorder: " + Format(pDtSLTarget.Rows(i).Item(36)), "", "DatabaseSelectDeepSky", "PROGRAM")
                                                End If
                                                TargetFound = True
                                            End If
                                        Else
                                            'target is too low, do nothing
                                            If vCheckOnly = False Then
                                                LogSessionEntry("FULL", "Not Observable - Moon West/Target East: " + pDtSLTarget.Rows(i).Item(1).ToString + " (" + Format(pDtSLTarget.Rows(i).Item(12)) + "/" + Format(pDtSLTarget.Rows(i).Item(11)) + " frames) Alt " + Format(pDtSLTarget.Rows(i).Item(31), "#0.00") + "° - Az " + Format(pDtSLTarget.Rows(i).Item(32), "#0.00") + "° Target: " + pDtSLTarget.Rows(i).Item(34).ToString + " Moon: " + pStructEventTimes.MoonCompassDirection + " Sortorder: " + Format(pDtSLTarget.Rows(i).Item(36)), "", "DatabaseSelectDeepSky", "PROGRAM")
                                            End If

                                        End If
                                    Else
                                        'the Moon is above the horizon in the East, prefer a target in the West
                                        If Convert.ToDouble(pDtSLTarget.Rows(i).Item(33)) < 8 And Convert.ToDouble(pDtSLTarget.Rows(i).Item(31)) >= My.Settings.sObjectAltLimitWest Then
                                            If Convert.ToBoolean(pDtSLTarget.Rows(i).Item(28)) = True Then
                                                'priority target
                                                PriorityTarget.ID = Convert.ToInt32(pDtSLTarget.Rows(i).Item(0))
                                                PriorityTarget.TargetName = pDtSLTarget.Rows(i).Item(1).ToString
                                                PriorityTarget.TargetRA2000HH = pDtSLTarget.Rows(i).Item(2).ToString
                                                PriorityTarget.TargetRA2000MM = pDtSLTarget.Rows(i).Item(3).ToString
                                                PriorityTarget.TargetRA2000SS = pDtSLTarget.Rows(i).Item(4).ToString
                                                PriorityTarget.TargetDEC2000DG = pDtSLTarget.Rows(i).Item(5).ToString
                                                PriorityTarget.TargetDEC2000MM = pDtSLTarget.Rows(i).Item(6).ToString
                                                PriorityTarget.TargetDEC2000SS = pDtSLTarget.Rows(i).Item(7).ToString
                                                PriorityTarget.TargetExposure = Convert.ToDouble(pDtSLTarget.Rows(i).Item(8))
                                                PriorityTarget.TargetBin = pDtSLTarget.Rows(i).Item(9).ToString
                                                PriorityTarget.TargetFilter = pDtSLTarget.Rows(i).Item(10).ToString
                                                PriorityTarget.TargetDone = Convert.ToBoolean(pDtSLTarget.Rows(i).Item(11))
                                                PriorityTarget.TargetNbrFrames = Convert.ToInt32(pDtSLTarget.Rows(i).Item(12))
                                                PriorityTarget.TargetNbrExposedFrames = Convert.ToInt32(pDtSLTarget.Rows(i).Item(13))
                                                PriorityTarget.TargetPriority = Convert.ToBoolean(pDtSLTarget.Rows(i).Item(14))
                                                PriorityTarget.TargetIsComet = Convert.ToBoolean(pDtSLTarget.Rows(i).Item(15))
                                                PriorityTarget.TargetIgnoreMoon = Convert.ToBoolean(pDtSLTarget.Rows(i).Item(16))
                                                PriorityTarget.FocusStarName = pDtSLTarget.Rows(i).Item(17).ToString
                                                PriorityTarget.FocusStarRA2000HH = pDtSLTarget.Rows(i).Item(18).ToString
                                                PriorityTarget.FocusStarRA2000MM = pDtSLTarget.Rows(i).Item(19).ToString
                                                PriorityTarget.FocusStarRA2000SS = pDtSLTarget.Rows(i).Item(20).ToString
                                                PriorityTarget.FocusStarDEC2000DG = pDtSLTarget.Rows(i).Item(21).ToString
                                                PriorityTarget.FocusStarDEC2000MM = pDtSLTarget.Rows(i).Item(22).ToString
                                                PriorityTarget.FocusStarDEC2000SS = pDtSLTarget.Rows(i).Item(23).ToString
                                                PriorityTarget.FocusStarExposure = Convert.ToDouble(pDtSLTarget.Rows(i).Item(24))
                                                PriorityTarget.GuideAuto = Convert.ToBoolean(pDtSLTarget.Rows(i).Item(25))
                                                PriorityTarget.GuideStarXBMF = Convert.ToInt32(pDtSLTarget.Rows(i).Item(26))
                                                PriorityTarget.GuideStarYBMF = Convert.ToInt32(pDtSLTarget.Rows(i).Item(27))
                                                PriorityTarget.GuideStarXPMF = Convert.ToInt32(pDtSLTarget.Rows(i).Item(28))
                                                PriorityTarget.GuideStarYPMF = Convert.ToInt32(pDtSLTarget.Rows(i).Item(29))
                                                PriorityTarget.GuideStarExposure = Convert.ToDouble(pDtSLTarget.Rows(i).Item(30))
                                                PriorityTarget.TargetAltitude = Convert.ToDouble(pDtSLTarget.Rows(i).Item(31))
                                                PriorityTarget.TargetAzimuth = Convert.ToDouble(pDtSLTarget.Rows(i).Item(32))
                                                PriorityTarget.TargetCompassIndex = Convert.ToInt32(pDtSLTarget.Rows(i).Item(33))
                                                PriorityTarget.TargetCompassDirection = pDtSLTarget.Rows(i).Item(34).ToString
                                                PriorityTarget.TargetCircumpolar = Convert.ToBoolean(pDtSLTarget.Rows(i).Item(35))
                                                PriorityTarget.TargetMosaic = Convert.ToBoolean(pDtSLTarget.Rows(i).Item(38))
                                                PriorityTarget.TargetMosaicType = pDtSLTarget.Rows(i).Item(39).ToString
                                                PriorityTarget.TargetMosaicFramesPerPanel = Convert.ToInt32(pDtSLTarget.Rows(i).Item(40))
                                                PriorityTarget.TargetPanel1RA2000HH = pDtSLTarget.Rows(i).Item(41).ToString
                                                PriorityTarget.TargetPanel1RA2000MM = pDtSLTarget.Rows(i).Item(42).ToString
                                                PriorityTarget.TargetPanel1RA2000SS = pDtSLTarget.Rows(i).Item(43).ToString
                                                PriorityTarget.TargetPanel1DEC2000DG = pDtSLTarget.Rows(i).Item(44).ToString
                                                PriorityTarget.TargetPanel1DEC2000MM = pDtSLTarget.Rows(i).Item(45).ToString
                                                PriorityTarget.TargetPanel1DEC2000SS = pDtSLTarget.Rows(i).Item(46).ToString
                                                PriorityTarget.TargetPanel2RA2000HH = pDtSLTarget.Rows(i).Item(47).ToString
                                                PriorityTarget.TargetPanel2RA2000MM = pDtSLTarget.Rows(i).Item(48).ToString
                                                PriorityTarget.TargetPanel2RA2000SS = pDtSLTarget.Rows(i).Item(49).ToString
                                                PriorityTarget.TargetPanel2DEC2000DG = pDtSLTarget.Rows(i).Item(50).ToString
                                                PriorityTarget.TargetPanel2DEC2000MM = pDtSLTarget.Rows(i).Item(51).ToString
                                                PriorityTarget.TargetPanel2DEC2000SS = pDtSLTarget.Rows(i).Item(52).ToString
                                                PriorityTarget.TargetPanel3RA2000HH = pDtSLTarget.Rows(i).Item(53).ToString
                                                PriorityTarget.TargetPanel3RA2000MM = pDtSLTarget.Rows(i).Item(54).ToString
                                                PriorityTarget.TargetPanel3RA2000SS = pDtSLTarget.Rows(i).Item(55).ToString
                                                PriorityTarget.TargetPanel3DEC2000DG = pDtSLTarget.Rows(i).Item(56).ToString
                                                PriorityTarget.TargetPanel3DEC2000MM = pDtSLTarget.Rows(i).Item(57).ToString
                                                PriorityTarget.TargetPanel3DEC2000SS = pDtSLTarget.Rows(i).Item(58).ToString
                                                PriorityTarget.TargetPanel4RA2000HH = pDtSLTarget.Rows(i).Item(59).ToString
                                                PriorityTarget.TargetPanel4RA2000MM = pDtSLTarget.Rows(i).Item(60).ToString
                                                PriorityTarget.TargetPanel4RA2000SS = pDtSLTarget.Rows(i).Item(61).ToString
                                                PriorityTarget.TargetPanel4DEC2000DG = pDtSLTarget.Rows(i).Item(62).ToString
                                                PriorityTarget.TargetPanel4DEC2000MM = pDtSLTarget.Rows(i).Item(63).ToString
                                                PriorityTarget.TargetPanel4DEC2000SS = pDtSLTarget.Rows(i).Item(64).ToString
                                                PriorityTarget.TargetPanel1NbrExposedFrames = Convert.ToInt32(pDtSLTarget.Rows(i).Item(65))
                                                PriorityTarget.TargetPanel2NbrExposedFrames = Convert.ToInt32(pDtSLTarget.Rows(i).Item(66))
                                                PriorityTarget.TargetPanel3NbrExposedFrames = Convert.ToInt32(pDtSLTarget.Rows(i).Item(67))
                                                PriorityTarget.TargetPanel4NbrExposedFrames = Convert.ToInt32(pDtSLTarget.Rows(i).Item(68))
                                                If vCheckOnly = False Then
                                                    LogSessionEntry("FULL", "PRIORITY TARGET - Moon East/Target West: " + pDtSLTarget.Rows(i).Item(1).ToString + " (" + Format(pDtSLTarget.Rows(i).Item(12)) + "/" + Format(pDtSLTarget.Rows(i).Item(11)) + " frames) Alt " + Format(pDtSLTarget.Rows(i).Item(31), "#0.00") + "° - Az " + Format(pDtSLTarget.Rows(i).Item(32), "#0.00") + "° Target: " + pDtSLTarget.Rows(i).Item(34).ToString + " Moon: " + pStructEventTimes.MoonCompassDirection + " Sortorder: " + Format(pDtSLTarget.Rows(i).Item(36)), "", "DatabaseSelectDeepSky", "PROGRAM")
                                                End If
                                                TargetFound = True
                                            Else
                                                'normal target
                                                NormalTarget.ID = Convert.ToInt32(pDtSLTarget.Rows(i).Item(0))
                                                NormalTarget.TargetName = pDtSLTarget.Rows(i).Item(1).ToString
                                                NormalTarget.TargetRA2000HH = pDtSLTarget.Rows(i).Item(2).ToString
                                                NormalTarget.TargetRA2000MM = pDtSLTarget.Rows(i).Item(3).ToString
                                                NormalTarget.TargetRA2000SS = pDtSLTarget.Rows(i).Item(4).ToString
                                                NormalTarget.TargetDEC2000DG = pDtSLTarget.Rows(i).Item(5).ToString
                                                NormalTarget.TargetDEC2000MM = pDtSLTarget.Rows(i).Item(6).ToString
                                                NormalTarget.TargetDEC2000SS = pDtSLTarget.Rows(i).Item(7).ToString
                                                NormalTarget.TargetExposure = Convert.ToDouble(pDtSLTarget.Rows(i).Item(8))
                                                NormalTarget.TargetBin = pDtSLTarget.Rows(i).Item(9).ToString
                                                NormalTarget.TargetFilter = pDtSLTarget.Rows(i).Item(10).ToString
                                                NormalTarget.TargetDone = Convert.ToBoolean(pDtSLTarget.Rows(i).Item(11))
                                                NormalTarget.TargetNbrFrames = Convert.ToInt32(pDtSLTarget.Rows(i).Item(12))
                                                NormalTarget.TargetNbrExposedFrames = Convert.ToInt32(pDtSLTarget.Rows(i).Item(13))
                                                NormalTarget.TargetPriority = Convert.ToBoolean(pDtSLTarget.Rows(i).Item(14))
                                                NormalTarget.TargetIsComet = Convert.ToBoolean(pDtSLTarget.Rows(i).Item(15))
                                                NormalTarget.TargetIgnoreMoon = Convert.ToBoolean(pDtSLTarget.Rows(i).Item(16))
                                                NormalTarget.FocusStarName = pDtSLTarget.Rows(i).Item(17).ToString
                                                NormalTarget.FocusStarRA2000HH = pDtSLTarget.Rows(i).Item(18).ToString
                                                NormalTarget.FocusStarRA2000MM = pDtSLTarget.Rows(i).Item(19).ToString
                                                NormalTarget.FocusStarRA2000SS = pDtSLTarget.Rows(i).Item(20).ToString
                                                NormalTarget.FocusStarDEC2000DG = pDtSLTarget.Rows(i).Item(21).ToString
                                                NormalTarget.FocusStarDEC2000MM = pDtSLTarget.Rows(i).Item(22).ToString
                                                NormalTarget.FocusStarDEC2000SS = pDtSLTarget.Rows(i).Item(23).ToString
                                                NormalTarget.FocusStarExposure = Convert.ToDouble(pDtSLTarget.Rows(i).Item(24))
                                                NormalTarget.GuideAuto = Convert.ToBoolean(pDtSLTarget.Rows(i).Item(25))
                                                NormalTarget.GuideStarXBMF = Convert.ToInt32(pDtSLTarget.Rows(i).Item(26))
                                                NormalTarget.GuideStarYBMF = Convert.ToInt32(pDtSLTarget.Rows(i).Item(27))
                                                NormalTarget.GuideStarXPMF = Convert.ToInt32(pDtSLTarget.Rows(i).Item(28))
                                                NormalTarget.GuideStarYPMF = Convert.ToInt32(pDtSLTarget.Rows(i).Item(29))
                                                NormalTarget.GuideStarExposure = Convert.ToDouble(pDtSLTarget.Rows(i).Item(30))
                                                NormalTarget.TargetAltitude = Convert.ToDouble(pDtSLTarget.Rows(i).Item(31))
                                                NormalTarget.TargetAzimuth = Convert.ToDouble(pDtSLTarget.Rows(i).Item(32))
                                                NormalTarget.TargetCompassIndex = Convert.ToInt32(pDtSLTarget.Rows(i).Item(33))
                                                NormalTarget.TargetCompassDirection = pDtSLTarget.Rows(i).Item(34).ToString
                                                NormalTarget.TargetCircumpolar = Convert.ToBoolean(pDtSLTarget.Rows(i).Item(35))
                                                NormalTarget.TargetMosaic = Convert.ToBoolean(pDtSLTarget.Rows(i).Item(38))
                                                NormalTarget.TargetMosaicType = pDtSLTarget.Rows(i).Item(39).ToString
                                                NormalTarget.TargetMosaicFramesPerPanel = Convert.ToInt32(pDtSLTarget.Rows(i).Item(40))
                                                NormalTarget.TargetPanel1RA2000HH = pDtSLTarget.Rows(i).Item(41).ToString
                                                NormalTarget.TargetPanel1RA2000MM = pDtSLTarget.Rows(i).Item(42).ToString
                                                NormalTarget.TargetPanel1RA2000SS = pDtSLTarget.Rows(i).Item(43).ToString
                                                NormalTarget.TargetPanel1DEC2000DG = pDtSLTarget.Rows(i).Item(44).ToString
                                                NormalTarget.TargetPanel1DEC2000MM = pDtSLTarget.Rows(i).Item(45).ToString
                                                NormalTarget.TargetPanel1DEC2000SS = pDtSLTarget.Rows(i).Item(46).ToString
                                                NormalTarget.TargetPanel2RA2000HH = pDtSLTarget.Rows(i).Item(47).ToString
                                                NormalTarget.TargetPanel2RA2000MM = pDtSLTarget.Rows(i).Item(48).ToString
                                                NormalTarget.TargetPanel2RA2000SS = pDtSLTarget.Rows(i).Item(49).ToString
                                                NormalTarget.TargetPanel2DEC2000DG = pDtSLTarget.Rows(i).Item(50).ToString
                                                NormalTarget.TargetPanel2DEC2000MM = pDtSLTarget.Rows(i).Item(51).ToString
                                                NormalTarget.TargetPanel2DEC2000SS = pDtSLTarget.Rows(i).Item(52).ToString
                                                NormalTarget.TargetPanel3RA2000HH = pDtSLTarget.Rows(i).Item(53).ToString
                                                NormalTarget.TargetPanel3RA2000MM = pDtSLTarget.Rows(i).Item(54).ToString
                                                NormalTarget.TargetPanel3RA2000SS = pDtSLTarget.Rows(i).Item(55).ToString
                                                NormalTarget.TargetPanel3DEC2000DG = pDtSLTarget.Rows(i).Item(56).ToString
                                                NormalTarget.TargetPanel3DEC2000MM = pDtSLTarget.Rows(i).Item(57).ToString
                                                NormalTarget.TargetPanel3DEC2000SS = pDtSLTarget.Rows(i).Item(58).ToString
                                                NormalTarget.TargetPanel4RA2000HH = pDtSLTarget.Rows(i).Item(59).ToString
                                                NormalTarget.TargetPanel4RA2000MM = pDtSLTarget.Rows(i).Item(60).ToString
                                                NormalTarget.TargetPanel4RA2000SS = pDtSLTarget.Rows(i).Item(61).ToString
                                                NormalTarget.TargetPanel4DEC2000DG = pDtSLTarget.Rows(i).Item(62).ToString
                                                NormalTarget.TargetPanel4DEC2000MM = pDtSLTarget.Rows(i).Item(63).ToString
                                                NormalTarget.TargetPanel4DEC2000SS = pDtSLTarget.Rows(i).Item(64).ToString
                                                NormalTarget.TargetPanel1NbrExposedFrames = Convert.ToInt32(pDtSLTarget.Rows(i).Item(65))
                                                NormalTarget.TargetPanel2NbrExposedFrames = Convert.ToInt32(pDtSLTarget.Rows(i).Item(66))
                                                NormalTarget.TargetPanel3NbrExposedFrames = Convert.ToInt32(pDtSLTarget.Rows(i).Item(67))
                                                NormalTarget.TargetPanel4NbrExposedFrames = Convert.ToInt32(pDtSLTarget.Rows(i).Item(68))
                                                If vCheckOnly = False Then
                                                    LogSessionEntry("FULL", "Normal target - Moon East/Target West: " + pDtSLTarget.Rows(i).Item(1).ToString + " (" + Format(pDtSLTarget.Rows(i).Item(12)) + "/" + Format(pDtSLTarget.Rows(i).Item(11)) + " frames) Alt " + Format(pDtSLTarget.Rows(i).Item(31), "#0.00") + "° - Az " + Format(pDtSLTarget.Rows(i).Item(32), "#0.00") + "° Target: " + pDtSLTarget.Rows(i).Item(34).ToString + " Moon: " + pStructEventTimes.MoonCompassDirection + " Sortorder: " + Format(pDtSLTarget.Rows(i).Item(36)), "", "DatabaseSelectDeepSky", "PROGRAM")
                                                End If
                                                TargetFound = True
                                            End If
                                        Else
                                            'target too low, do nothing
                                            If vCheckOnly = False Then
                                                LogSessionEntry("FULL", "Not observable - Moon East/Target West: " + pDtSLTarget.Rows(i).Item(1).ToString + " (" + Format(pDtSLTarget.Rows(i).Item(12)) + "/" + Format(pDtSLTarget.Rows(i).Item(11)) + " frames) Alt " + Format(pDtSLTarget.Rows(i).Item(31), "#0.00") + "° - Az " + Format(pDtSLTarget.Rows(i).Item(32), "#0.00") + "° Target: " + pDtSLTarget.Rows(i).Item(34).ToString + " Moon: " + pStructEventTimes.MoonCompassDirection + " Sortorder: " + Format(pDtSLTarget.Rows(i).Item(36)), "", "DatabaseSelectDeepSky", "PROGRAM")
                                            End If
                                        End If
                                    End If
                                End If

                                i += 1
                            End While

                            DatabaseClearDeepSky()

                            If PriorityTarget.ID <> 0 Then
                                pDSLTarget.ID = PriorityTarget.ID
                                pDSLTarget.TargetName = PriorityTarget.TargetName
                                pDSLTarget.TargetRA2000HH = PriorityTarget.TargetRA2000HH
                                pDSLTarget.TargetRA2000MM = PriorityTarget.TargetRA2000MM
                                pDSLTarget.TargetRA2000SS = PriorityTarget.TargetRA2000SS
                                pDSLTarget.TargetDEC2000DG = PriorityTarget.TargetDEC2000DG
                                pDSLTarget.TargetDEC2000MM = PriorityTarget.TargetDEC2000MM
                                pDSLTarget.TargetDEC2000SS = PriorityTarget.TargetDEC2000SS
                                pDSLTarget.TargetExposure = PriorityTarget.TargetExposure
                                pDSLTarget.TargetBin = PriorityTarget.TargetBin
                                pDSLTarget.TargetFilter = PriorityTarget.TargetFilter
                                pDSLTarget.TargetDone = PriorityTarget.TargetDone
                                pDSLTarget.TargetNbrExposedFrames = PriorityTarget.TargetNbrExposedFrames
                                pDSLTarget.TargetPriority = PriorityTarget.TargetPriority
                                pDSLTarget.TargetIsComet = PriorityTarget.TargetIsComet
                                pDSLTarget.TargetIgnoreMoon = PriorityTarget.TargetIgnoreMoon
                                pDSLTarget.TargetNbrFrames = PriorityTarget.TargetNbrFrames
                                pDSLTarget.FocusStarName = PriorityTarget.FocusStarName
                                pDSLTarget.FocusStarRA2000HH = PriorityTarget.FocusStarRA2000HH
                                pDSLTarget.FocusStarRA2000MM = PriorityTarget.FocusStarRA2000MM
                                pDSLTarget.FocusStarRA2000SS = PriorityTarget.FocusStarRA2000SS
                                pDSLTarget.FocusStarDEC2000DG = PriorityTarget.FocusStarDEC2000DG
                                pDSLTarget.FocusStarDEC2000MM = PriorityTarget.FocusStarDEC2000MM
                                pDSLTarget.FocusStarDEC2000SS = PriorityTarget.FocusStarDEC2000SS
                                pDSLTarget.FocusStarExposure = PriorityTarget.FocusStarExposure
                                pDSLTarget.GuideAuto = PriorityTarget.GuideAuto
                                pDSLTarget.GuideStarXBMF = PriorityTarget.GuideStarXBMF
                                pDSLTarget.GuideStarYBMF = PriorityTarget.GuideStarYBMF
                                pDSLTarget.GuideStarXPMF = PriorityTarget.GuideStarXPMF
                                pDSLTarget.GuideStarYPMF = PriorityTarget.GuideStarYPMF
                                pDSLTarget.GuideStarExposure = PriorityTarget.GuideStarExposure
                                pDSLTarget.TargetAltitude = PriorityTarget.TargetAltitude
                                pDSLTarget.TargetAzimuth = PriorityTarget.TargetAzimuth
                                pDSLTarget.TargetCompassIndex = PriorityTarget.TargetCompassIndex
                                pDSLTarget.TargetCompassDirection = PriorityTarget.TargetCompassDirection
                                pDSLTarget.TargetCircumpolar = PriorityTarget.TargetCircumpolar
                                pDSLTarget.TargetMosaic = PriorityTarget.TargetMosaic
                                pDSLTarget.TargetMosaicType = PriorityTarget.TargetMosaicType
                                pDSLTarget.TargetMosaicFramesPerPanel = PriorityTarget.TargetMosaicFramesPerPanel

                                pDSLTarget.TargetPanel1RA2000HH = PriorityTarget.TargetPanel1RA2000HH
                                pDSLTarget.TargetPanel1RA2000MM = PriorityTarget.TargetPanel1RA2000MM
                                pDSLTarget.TargetPanel1RA2000SS = PriorityTarget.TargetPanel1RA2000SS
                                pDSLTarget.TargetPanel1DEC2000DG = PriorityTarget.TargetPanel1DEC2000DG
                                pDSLTarget.TargetPanel1DEC2000MM = PriorityTarget.TargetPanel1DEC2000MM
                                pDSLTarget.TargetPanel1DEC2000SS = PriorityTarget.TargetPanel1DEC2000SS

                                pDSLTarget.TargetPanel2RA2000HH = PriorityTarget.TargetPanel2RA2000HH
                                pDSLTarget.TargetPanel2RA2000MM = PriorityTarget.TargetPanel2RA2000MM
                                pDSLTarget.TargetPanel2RA2000SS = PriorityTarget.TargetPanel2RA2000SS
                                pDSLTarget.TargetPanel2DEC2000DG = PriorityTarget.TargetPanel2DEC2000DG
                                pDSLTarget.TargetPanel2DEC2000MM = PriorityTarget.TargetPanel2DEC2000MM
                                pDSLTarget.TargetPanel2DEC2000SS = PriorityTarget.TargetPanel2DEC2000SS

                                pDSLTarget.TargetPanel3RA2000HH = PriorityTarget.TargetPanel3RA2000HH
                                pDSLTarget.TargetPanel3RA2000MM = PriorityTarget.TargetPanel3RA2000MM
                                pDSLTarget.TargetPanel3RA2000SS = PriorityTarget.TargetPanel3RA2000SS
                                pDSLTarget.TargetPanel3DEC2000DG = PriorityTarget.TargetPanel3DEC2000DG
                                pDSLTarget.TargetPanel3DEC2000MM = PriorityTarget.TargetPanel3DEC2000MM
                                pDSLTarget.TargetPanel3DEC2000SS = PriorityTarget.TargetPanel3DEC2000SS

                                pDSLTarget.TargetPanel4RA2000HH = PriorityTarget.TargetPanel4RA2000HH
                                pDSLTarget.TargetPanel4RA2000MM = PriorityTarget.TargetPanel4RA2000MM
                                pDSLTarget.TargetPanel4RA2000SS = PriorityTarget.TargetPanel4RA2000SS
                                pDSLTarget.TargetPanel4DEC2000DG = PriorityTarget.TargetPanel4DEC2000DG
                                pDSLTarget.TargetPanel4DEC2000MM = PriorityTarget.TargetPanel4DEC2000MM
                                pDSLTarget.TargetPanel4DEC2000SS = PriorityTarget.TargetPanel4DEC2000SS

                                pDSLTarget.TargetPanel1NbrExposedFrames = PriorityTarget.TargetPanel1NbrExposedFrames
                                pDSLTarget.TargetPanel2NbrExposedFrames = PriorityTarget.TargetPanel2NbrExposedFrames
                                pDSLTarget.TargetPanel3NbrExposedFrames = PriorityTarget.TargetPanel3NbrExposedFrames
                                pDSLTarget.TargetPanel4NbrExposedFrames = PriorityTarget.TargetPanel4NbrExposedFrames
                            ElseIf NormalTarget.ID <> 0 Then
                                pDSLTarget.ID = NormalTarget.ID
                                pDSLTarget.TargetName = NormalTarget.TargetName
                                pDSLTarget.TargetRA2000HH = NormalTarget.TargetRA2000HH
                                pDSLTarget.TargetRA2000MM = NormalTarget.TargetRA2000MM
                                pDSLTarget.TargetRA2000SS = NormalTarget.TargetRA2000SS
                                pDSLTarget.TargetDEC2000DG = NormalTarget.TargetDEC2000DG
                                pDSLTarget.TargetDEC2000MM = NormalTarget.TargetDEC2000MM
                                pDSLTarget.TargetDEC2000SS = NormalTarget.TargetDEC2000SS
                                pDSLTarget.TargetExposure = NormalTarget.TargetExposure
                                pDSLTarget.TargetBin = NormalTarget.TargetBin
                                pDSLTarget.TargetFilter = NormalTarget.TargetFilter
                                pDSLTarget.TargetDone = NormalTarget.TargetDone
                                pDSLTarget.TargetNbrExposedFrames = NormalTarget.TargetNbrExposedFrames
                                pDSLTarget.TargetPriority = NormalTarget.TargetPriority
                                pDSLTarget.TargetIsComet = NormalTarget.TargetIsComet
                                pDSLTarget.TargetIgnoreMoon = NormalTarget.TargetIgnoreMoon
                                pDSLTarget.TargetNbrFrames = NormalTarget.TargetNbrFrames
                                pDSLTarget.FocusStarName = NormalTarget.FocusStarName
                                pDSLTarget.FocusStarRA2000HH = NormalTarget.FocusStarRA2000HH
                                pDSLTarget.FocusStarRA2000MM = NormalTarget.FocusStarRA2000MM
                                pDSLTarget.FocusStarRA2000SS = NormalTarget.FocusStarRA2000SS
                                pDSLTarget.FocusStarDEC2000DG = NormalTarget.FocusStarDEC2000DG
                                pDSLTarget.FocusStarDEC2000MM = NormalTarget.FocusStarDEC2000MM
                                pDSLTarget.FocusStarDEC2000SS = NormalTarget.FocusStarDEC2000SS
                                pDSLTarget.FocusStarExposure = NormalTarget.FocusStarExposure
                                pDSLTarget.GuideAuto = NormalTarget.GuideAuto
                                pDSLTarget.GuideStarXBMF = NormalTarget.GuideStarXBMF
                                pDSLTarget.GuideStarYBMF = NormalTarget.GuideStarYBMF
                                pDSLTarget.GuideStarXPMF = NormalTarget.GuideStarXPMF
                                pDSLTarget.GuideStarYPMF = NormalTarget.GuideStarYPMF
                                pDSLTarget.GuideStarExposure = NormalTarget.GuideStarExposure
                                pDSLTarget.TargetAltitude = NormalTarget.TargetAltitude
                                pDSLTarget.TargetAzimuth = NormalTarget.TargetAzimuth
                                pDSLTarget.TargetCompassIndex = NormalTarget.TargetCompassIndex
                                pDSLTarget.TargetCompassDirection = NormalTarget.TargetCompassDirection
                                pDSLTarget.TargetCircumpolar = NormalTarget.TargetCircumpolar
                                pDSLTarget.TargetMosaic = NormalTarget.TargetMosaic
                                pDSLTarget.TargetMosaicType = NormalTarget.TargetMosaicType
                                pDSLTarget.TargetMosaicFramesPerPanel = NormalTarget.TargetMosaicFramesPerPanel

                                pDSLTarget.TargetPanel1RA2000HH = NormalTarget.TargetPanel1RA2000HH
                                pDSLTarget.TargetPanel1RA2000MM = NormalTarget.TargetPanel1RA2000MM
                                pDSLTarget.TargetPanel1RA2000SS = NormalTarget.TargetPanel1RA2000SS
                                pDSLTarget.TargetPanel1DEC2000DG = NormalTarget.TargetPanel1DEC2000DG
                                pDSLTarget.TargetPanel1DEC2000MM = NormalTarget.TargetPanel1DEC2000MM
                                pDSLTarget.TargetPanel1DEC2000SS = NormalTarget.TargetPanel1DEC2000SS

                                pDSLTarget.TargetPanel2RA2000HH = NormalTarget.TargetPanel2RA2000HH
                                pDSLTarget.TargetPanel2RA2000MM = NormalTarget.TargetPanel2RA2000MM
                                pDSLTarget.TargetPanel2RA2000SS = NormalTarget.TargetPanel2RA2000SS
                                pDSLTarget.TargetPanel2DEC2000DG = NormalTarget.TargetPanel2DEC2000DG
                                pDSLTarget.TargetPanel2DEC2000MM = NormalTarget.TargetPanel2DEC2000MM
                                pDSLTarget.TargetPanel2DEC2000SS = NormalTarget.TargetPanel2DEC2000SS

                                pDSLTarget.TargetPanel3RA2000HH = NormalTarget.TargetPanel3RA2000HH
                                pDSLTarget.TargetPanel3RA2000MM = NormalTarget.TargetPanel3RA2000MM
                                pDSLTarget.TargetPanel3RA2000SS = NormalTarget.TargetPanel3RA2000SS
                                pDSLTarget.TargetPanel3DEC2000DG = NormalTarget.TargetPanel3DEC2000DG
                                pDSLTarget.TargetPanel3DEC2000MM = NormalTarget.TargetPanel3DEC2000MM
                                pDSLTarget.TargetPanel3DEC2000SS = NormalTarget.TargetPanel3DEC2000SS

                                pDSLTarget.TargetPanel4RA2000HH = NormalTarget.TargetPanel4RA2000HH
                                pDSLTarget.TargetPanel4RA2000MM = NormalTarget.TargetPanel4RA2000MM
                                pDSLTarget.TargetPanel4RA2000SS = NormalTarget.TargetPanel4RA2000SS
                                pDSLTarget.TargetPanel4DEC2000DG = NormalTarget.TargetPanel4DEC2000DG
                                pDSLTarget.TargetPanel4DEC2000MM = NormalTarget.TargetPanel4DEC2000MM
                                pDSLTarget.TargetPanel4DEC2000SS = NormalTarget.TargetPanel4DEC2000SS

                                pDSLTarget.TargetPanel1NbrExposedFrames = NormalTarget.TargetPanel1NbrExposedFrames
                                pDSLTarget.TargetPanel2NbrExposedFrames = NormalTarget.TargetPanel2NbrExposedFrames
                                pDSLTarget.TargetPanel3NbrExposedFrames = NormalTarget.TargetPanel3NbrExposedFrames
                                pDSLTarget.TargetPanel4NbrExposedFrames = NormalTarget.TargetPanel4NbrExposedFrames
                            ElseIf AnyTarget.ID <> 0 Then
                                pDSLTarget.ID = AnyTarget.ID
                                pDSLTarget.TargetName = AnyTarget.TargetName
                                pDSLTarget.TargetRA2000HH = AnyTarget.TargetRA2000HH
                                pDSLTarget.TargetRA2000MM = AnyTarget.TargetRA2000MM
                                pDSLTarget.TargetRA2000SS = AnyTarget.TargetRA2000SS
                                pDSLTarget.TargetDEC2000DG = AnyTarget.TargetDEC2000DG
                                pDSLTarget.TargetDEC2000MM = AnyTarget.TargetDEC2000MM
                                pDSLTarget.TargetDEC2000SS = AnyTarget.TargetDEC2000SS
                                pDSLTarget.TargetExposure = AnyTarget.TargetExposure
                                pDSLTarget.TargetBin = AnyTarget.TargetBin
                                pDSLTarget.TargetFilter = AnyTarget.TargetFilter
                                pDSLTarget.TargetDone = AnyTarget.TargetDone
                                pDSLTarget.TargetNbrExposedFrames = AnyTarget.TargetNbrExposedFrames
                                pDSLTarget.TargetPriority = AnyTarget.TargetPriority
                                pDSLTarget.TargetIsComet = AnyTarget.TargetIsComet
                                pDSLTarget.TargetIgnoreMoon = AnyTarget.TargetIgnoreMoon
                                pDSLTarget.TargetNbrFrames = AnyTarget.TargetNbrFrames
                                pDSLTarget.FocusStarName = AnyTarget.FocusStarName
                                pDSLTarget.FocusStarRA2000HH = AnyTarget.FocusStarRA2000HH
                                pDSLTarget.FocusStarRA2000MM = AnyTarget.FocusStarRA2000MM
                                pDSLTarget.FocusStarRA2000SS = AnyTarget.FocusStarRA2000SS
                                pDSLTarget.FocusStarDEC2000DG = AnyTarget.FocusStarDEC2000DG
                                pDSLTarget.FocusStarDEC2000MM = AnyTarget.FocusStarDEC2000MM
                                pDSLTarget.FocusStarDEC2000SS = AnyTarget.FocusStarDEC2000SS
                                pDSLTarget.FocusStarExposure = AnyTarget.FocusStarExposure
                                pDSLTarget.GuideAuto = AnyTarget.GuideAuto
                                pDSLTarget.GuideStarXBMF = AnyTarget.GuideStarXBMF
                                pDSLTarget.GuideStarYBMF = AnyTarget.GuideStarYBMF
                                pDSLTarget.GuideStarXPMF = AnyTarget.GuideStarXPMF
                                pDSLTarget.GuideStarYPMF = AnyTarget.GuideStarYPMF
                                pDSLTarget.GuideStarExposure = AnyTarget.GuideStarExposure
                                pDSLTarget.TargetAltitude = AnyTarget.TargetAltitude
                                pDSLTarget.TargetAzimuth = AnyTarget.TargetAzimuth
                                pDSLTarget.TargetCompassIndex = AnyTarget.TargetCompassIndex
                                pDSLTarget.TargetCompassDirection = AnyTarget.TargetCompassDirection
                                pDSLTarget.TargetCircumpolar = AnyTarget.TargetCircumpolar
                                pDSLTarget.TargetMosaic = AnyTarget.TargetMosaic
                                pDSLTarget.TargetMosaicType = AnyTarget.TargetMosaicType
                                pDSLTarget.TargetMosaicFramesPerPanel = AnyTarget.TargetMosaicFramesPerPanel

                                pDSLTarget.TargetPanel1RA2000HH = NormalTarget.TargetPanel1RA2000HH
                                pDSLTarget.TargetPanel1RA2000MM = NormalTarget.TargetPanel1RA2000MM
                                pDSLTarget.TargetPanel1RA2000SS = NormalTarget.TargetPanel1RA2000SS
                                pDSLTarget.TargetPanel1DEC2000DG = NormalTarget.TargetPanel1DEC2000DG
                                pDSLTarget.TargetPanel1DEC2000MM = NormalTarget.TargetPanel1DEC2000MM
                                pDSLTarget.TargetPanel1DEC2000SS = NormalTarget.TargetPanel1DEC2000SS

                                pDSLTarget.TargetPanel2RA2000HH = NormalTarget.TargetPanel2RA2000HH
                                pDSLTarget.TargetPanel2RA2000MM = NormalTarget.TargetPanel2RA2000MM
                                pDSLTarget.TargetPanel2RA2000SS = NormalTarget.TargetPanel2RA2000SS
                                pDSLTarget.TargetPanel2DEC2000DG = NormalTarget.TargetPanel2DEC2000DG
                                pDSLTarget.TargetPanel2DEC2000MM = NormalTarget.TargetPanel2DEC2000MM
                                pDSLTarget.TargetPanel2DEC2000SS = NormalTarget.TargetPanel2DEC2000SS

                                pDSLTarget.TargetPanel3RA2000HH = NormalTarget.TargetPanel3RA2000HH
                                pDSLTarget.TargetPanel3RA2000MM = NormalTarget.TargetPanel3RA2000MM
                                pDSLTarget.TargetPanel3RA2000SS = NormalTarget.TargetPanel3RA2000SS
                                pDSLTarget.TargetPanel3DEC2000DG = NormalTarget.TargetPanel3DEC2000DG
                                pDSLTarget.TargetPanel3DEC2000MM = NormalTarget.TargetPanel3DEC2000MM
                                pDSLTarget.TargetPanel3DEC2000SS = NormalTarget.TargetPanel3DEC2000SS

                                pDSLTarget.TargetPanel4RA2000HH = NormalTarget.TargetPanel4RA2000HH
                                pDSLTarget.TargetPanel4RA2000MM = NormalTarget.TargetPanel4RA2000MM
                                pDSLTarget.TargetPanel4RA2000SS = NormalTarget.TargetPanel4RA2000SS
                                pDSLTarget.TargetPanel4DEC2000DG = NormalTarget.TargetPanel4DEC2000DG
                                pDSLTarget.TargetPanel4DEC2000MM = NormalTarget.TargetPanel4DEC2000MM
                                pDSLTarget.TargetPanel4DEC2000SS = NormalTarget.TargetPanel4DEC2000SS

                                pDSLTarget.TargetPanel1NbrExposedFrames = AnyTarget.TargetPanel1NbrExposedFrames
                                pDSLTarget.TargetPanel2NbrExposedFrames = AnyTarget.TargetPanel2NbrExposedFrames
                                pDSLTarget.TargetPanel3NbrExposedFrames = AnyTarget.TargetPanel3NbrExposedFrames
                                pDSLTarget.TargetPanel4NbrExposedFrames = AnyTarget.TargetPanel4NbrExposedFrames
                            End If

                            If vCheckOnly = False Then
                                LogSessionEntry("FULL", "Any target: " + AnyTarget.TargetName + " (Exposed: " + Format(AnyTarget.TargetNbrExposedFrames) + "/" + Format(AnyTarget.TargetNbrFrames) + " frames). Alt " + Format(AnyTarget.TargetAltitude, "#0.00") + "° - Az " + Format(AnyTarget.TargetAzimuth, "#0.00") + "° Target: " + AnyTarget.TargetCompassDirection + " Moon: " + pStructEventTimes.MoonCompassDirection, "", "DatabaseSelectDeepSky", "PROGRAM")
                                LogSessionEntry("FULL", "Priority target: " + PriorityTarget.TargetName + " (Exposed: " + Format(PriorityTarget.TargetNbrExposedFrames) + "/" + Format(PriorityTarget.TargetNbrFrames) + " frames). Alt " + Format(PriorityTarget.TargetAltitude, "#0.00") + "° - Az " + Format(PriorityTarget.TargetAzimuth, "#0.00") + "° Target: " + PriorityTarget.TargetCompassDirection + " Moon: " + pStructEventTimes.MoonCompassDirection, "", "DatabaseSelectDeepSky", "PROGRAM")
                                LogSessionEntry("FULL", "Normal target: " + NormalTarget.TargetName + " (Exposed: " + Format(NormalTarget.TargetNbrExposedFrames) + "/" + Format(NormalTarget.TargetNbrFrames) + " frames). Alt " + Format(NormalTarget.TargetAltitude, "#0.00") + "° - Az " + Format(NormalTarget.TargetAzimuth, "#0.00") + "° Target: " + NormalTarget.TargetCompassDirection + " Moon: " + pStructEventTimes.MoonCompassDirection, "", "DatabaseSelectDeepSky", "PROGRAM")
                                LogSessionEntry("ESSENTIAL", pDSLTarget.TargetName + " selected (Exposed: " + Format(pDSLTarget.TargetNbrExposedFrames) + "/" + Format(pDSLTarget.TargetNbrFrames) + " frames). Alt " + Format(pDSLTarget.TargetAltitude, "#0.00") + "° - Az " + Format(pDSLTarget.TargetAzimuth, "#0.00") + "° Target: " + pDSLTarget.TargetCompassDirection + " Moon: " + pStructEventTimes.MoonCompassDirection, "", "DatabaseSelectDeepSky", "PROGRAM")
                            End If
                        Catch ex As Exception
                            DatabaseSelectDeepSky = "DatabaseSelectDeepSky: " + ex.Message
                            LogSessionEntry("ERROR", "DatabaseSelectDeepSky: " + ex.Message, "", "DatabaseSelectDeepSky", "PROGRAM")
                        End Try
                    End Using
                End Using
            End Using
            FrmMain.Cursor = Cursors.Default
            executionTime = DateTime.UtcNow() - startExecution
            LogSessionEntry("DEBUG", "  DatabaseSelectDeepSky: " + executionTime.ToString, "", "DatabaseSelectDeepSky", "PROGRAM")

        Catch ex As Exception
            DatabaseSelectDeepSky = "DatabaseSelectDeepSky: " + ex.Message
            ' DatabaseDisconnect()
            LogSessionEntry("ERROR", "DatabaseSelectDeepSky: " + ex.Message, "", "DatabaseSelectDeepSky", "PROGRAM")
        End Try
    End Function

    Private Sub DatabaseClearDeepSky()
        pDSLTarget.ID = Nothing
        pDSLTarget.TargetName = Nothing
        pDSLTarget.TargetRA2000HH = Nothing
        pDSLTarget.TargetRA2000MM = Nothing
        pDSLTarget.TargetRA2000SS = Nothing
        pDSLTarget.TargetDEC2000DG = Nothing
        pDSLTarget.TargetDEC2000MM = Nothing
        pDSLTarget.TargetDEC2000SS = Nothing
        pDSLTarget.TargetExposure = Nothing
        pDSLTarget.TargetBin = Nothing
        pDSLTarget.TargetFilter = Nothing
        pDSLTarget.TargetDone = Nothing
        pDSLTarget.TargetNbrExposedFrames = Nothing
        pDSLTarget.TargetNbrFrames = Nothing
        pDSLTarget.TargetPriority = Nothing
        pDSLTarget.TargetIsComet = Nothing 'when comet: at observation time get coordinates from TSX
        pDSLTarget.TargetIgnoreMoon = Nothing
        pDSLTarget.FocusStarName = Nothing
        pDSLTarget.FocusStarRA2000HH = Nothing
        pDSLTarget.FocusStarRA2000MM = Nothing
        pDSLTarget.FocusStarRA2000SS = Nothing
        pDSLTarget.FocusStarDEC2000DG = Nothing
        pDSLTarget.FocusStarDEC2000MM = Nothing
        pDSLTarget.FocusStarDEC2000SS = Nothing
        pDSLTarget.FocusStarExposure = Nothing
        pDSLTarget.GuideAuto = Nothing
        pDSLTarget.GuideStarXBMF = Nothing
        pDSLTarget.GuideStarYBMF = Nothing
        pDSLTarget.GuideStarXPMF = Nothing
        pDSLTarget.GuideStarYPMF = Nothing
        pDSLTarget.GuideStarExposure = Nothing
        pDSLTarget.TargetAltitude = Nothing
        pDSLTarget.TargetAzimuth = Nothing
        pDSLTarget.TargetCompassIndex = Nothing
        pDSLTarget.TargetCompassDirection = Nothing
        pDSLTarget.ErrorObservingTarget = Nothing
        pDSLTarget.TargetCircumpolar = Nothing
        pDSLTarget.TargetCircumPriority = Nothing
        pDSLTarget.ErrorTextTarget = Nothing
        pDSLTarget.TargetRemarks = Nothing
        pDSLTarget.TargetLastObservedDate = Nothing
        pDSLTarget.TargetMosaic = Nothing
        pDSLTarget.TargetMosaicType = Nothing
        pDSLTarget.TargetMosaicFramesPerPanel = Nothing
        pDSLTarget.TargetPanel1RA2000HH = Nothing
        pDSLTarget.TargetPanel2RA2000HH = Nothing
        pDSLTarget.TargetPanel3RA2000HH = Nothing
        pDSLTarget.TargetPanel4RA2000HH = Nothing
        pDSLTarget.TargetPanel1RA2000MM = Nothing
        pDSLTarget.TargetPanel2RA2000MM = Nothing
        pDSLTarget.TargetPanel3RA2000MM = Nothing
        pDSLTarget.TargetPanel4RA2000MM = Nothing
        pDSLTarget.TargetPanel1RA2000SS = Nothing
        pDSLTarget.TargetPanel2RA2000SS = Nothing
        pDSLTarget.TargetPanel3RA2000SS = Nothing
        pDSLTarget.TargetPanel4RA2000SS = Nothing
        pDSLTarget.TargetPanel1DEC2000DG = Nothing
        pDSLTarget.TargetPanel2DEC2000DG = Nothing
        pDSLTarget.TargetPanel3DEC2000DG = Nothing
        pDSLTarget.TargetPanel4DEC2000DG = Nothing
        pDSLTarget.TargetPanel1DEC2000MM = Nothing
        pDSLTarget.TargetPanel2DEC2000MM = Nothing
        pDSLTarget.TargetPanel3DEC2000MM = Nothing
        pDSLTarget.TargetPanel4DEC2000MM = Nothing
        pDSLTarget.TargetPanel1DEC2000SS = Nothing
        pDSLTarget.TargetPanel2DEC2000SS = Nothing
        pDSLTarget.TargetPanel3DEC2000SS = Nothing
        pDSLTarget.TargetPanel4DEC2000SS = Nothing
        pDSLTarget.TargetPanel1NbrExposedFrames = Nothing
        pDSLTarget.TargetPanel2NbrExposedFrames = Nothing
        pDSLTarget.TargetPanel3NbrExposedFrames = Nothing
        pDSLTarget.TargetPanel4NbrExposedFrames = Nothing
    End Sub

    Public Function DatabaseMarkErrorDeepSky(vReason As String) As String
        Dim i As Integer
        Dim startExecution As Date
        Dim executionTime As TimeSpan

        DatabaseMarkErrorDeepSky = "OK"
        Try
            startExecution = DateTime.UtcNow()

            LogSessionEntry("ERROR", "Marking " + pDSLTarget.TargetName + " as unusable: " + vReason, "", "DatabaseMarkErrorDeepSky", "PROGRAM")


            Using pCon As New SQLiteConnection("Data Source=RoboticObservatory.db;Version=3;")
                Using cmd As SQLiteCommand = pCon.CreateCommand
                    Using da As New SQLiteDataAdapter
                        Try
                            pCon.Open()
                            da.SelectCommand = cmd
                            cmd.CommandText = "UPDATE Target SET TargetError=1, TargetErrorText = '" + vReason + "' WHERE Id = " + Format(pDSLTarget.ID)
                            da.SelectCommand = cmd
                            i = cmd.ExecuteNonQuery
                            pCon.Close()

                        Catch ex As Exception
                            DatabaseMarkErrorDeepSky = "DatabaseMarkErrorDeepSky: " + ex.Message
                            LogSessionEntry("ERROR", "DatabaseMarkErrorDeepSky: " + ex.Message, "", "DatabaseMarkErrorDeepSky", "PROGRAM")
                        End Try
                    End Using
                End Using
            End Using

            executionTime = DateTime.UtcNow() - startExecution
            LogSessionEntry("DEBUG", "  DatabaseMarkErrorDeepSky: " + executionTime.ToString, "", "DatabaseMarkErrorDeepSky", "PROGRAM")

        Catch ex As Exception
            DatabaseMarkErrorDeepSky = "DatabaseMarkErrorDeepSky: " + ex.Message
            LogSessionEntry("ERROR", "DatabaseMarkErrorDeepSky: " + ex.Message, "", "DatabaseMarkErrorDeepSky", "PROGRAM")
        End Try
    End Function


    Public Function DatabaseUpdateNbrExpDeepSky(vNumberOfExposures As Integer, vPanelNumber As Integer, vTotalNumberOfExposures As Integer, vMosaicType As String) As String
        'updates the number of exposure of the deepsky target
        Dim i As Integer
        Dim startExecution As Date
        Dim executionTime As TimeSpan

        DatabaseUpdateNbrExpDeepSky = "OK"
        Try
            startExecution = DateTime.UtcNow()

            Using pCon As New SQLiteConnection("Data Source=RoboticObservatory.db;Version=3;")
                Using cmd As SQLiteCommand = pCon.CreateCommand
                    Using da As New SQLiteDataAdapter
                        Try
                            If vMosaicType = "NONE" Then
                                'normal flow
                                pCon.Open()
                                da.SelectCommand = cmd
                                cmd.CommandText = "UPDATE Target SET TargetNbrExposedFrames = " + Format(vNumberOfExposures) + " WHERE Id = " + Format(pDSLTarget.ID)
                                da.SelectCommand = cmd
                                i = cmd.ExecuteNonQuery
                            Else
                                'mosaic flow
                                pCon.Open()
                                da.SelectCommand = cmd
                                If vPanelNumber = 1 Then
                                    cmd.CommandText = "UPDATE Target SET TargetPanel1NbrExposedFrames = " + Format(vNumberOfExposures) + " WHERE Id = " + Format(pDSLTarget.ID)
                                ElseIf vPanelNumber = 2 Then
                                    cmd.CommandText = "UPDATE Target SET TargetPanel2NbrExposedFrames = " + Format(vNumberOfExposures) + " WHERE Id = " + Format(pDSLTarget.ID)
                                ElseIf vPanelNumber = 3 Then
                                    cmd.CommandText = "UPDATE Target SET TargetPanel3NbrExposedFrames= " + Format(vNumberOfExposures) + " WHERE Id = " + Format(pDSLTarget.ID)
                                ElseIf vPanelNumber = 4 Then
                                    cmd.CommandText = "UPDATE Target SET TargetPanel4NbrExposedFrames = " + Format(vNumberOfExposures) + " WHERE Id = " + Format(pDSLTarget.ID)
                                End If

                                da.SelectCommand = cmd
                                i = cmd.ExecuteNonQuery

                                cmd.CommandText = "UPDATE Target SET TargetNbrExposedFrames = " + Format(vTotalNumberOfExposures) + " WHERE Id = " + Format(pDSLTarget.ID)
                                da.SelectCommand = cmd
                                i = cmd.ExecuteNonQuery

                            End If

                            da.SelectCommand = cmd
                            cmd.CommandText = "UPDATE Target SET TargetLastObservedDate = """ + Format(Now) + """ WHERE Id = " + Format(pDSLTarget.ID)
                            da.SelectCommand = cmd
                            i = cmd.ExecuteNonQuery

                            If vMosaicType = "NONE" Then
                                'normal flow
                                If vNumberOfExposures >= pDSLTarget.TargetNbrFrames Then
                                    da.SelectCommand = cmd
                                    cmd.CommandText = "UPDATE Target SET TargetDone = True WHERE Id = " + Format(pDSLTarget.ID)
                                    da.SelectCommand = cmd
                                    i = cmd.ExecuteNonQuery
                                End If
                                pCon.Close()
                            Else
                                'mosaic flow
                                Dim Multiplier As Integer
                                If vMosaicType = "1x2" Then
                                    Multiplier = 2
                                ElseIf vMosaicType = "2x1" Then
                                    Multiplier = 2
                                ElseIf vMosaicType = "2x2" Then
                                    Multiplier = 4
                                End If

                                If vTotalNumberOfExposures >= (pDSLTarget.TargetMosaicFramesPerPanel * Multiplier) Then
                                    da.SelectCommand = cmd
                                    cmd.CommandText = "UPDATE Target SET TargetDone = True WHERE Id = " + Format(pDSLTarget.ID)
                                    da.SelectCommand = cmd
                                    i = cmd.ExecuteNonQuery
                                End If
                                pCon.Close()
                                End If


                        Catch ex As Exception
                            DatabaseUpdateNbrExpDeepSky = "DatabaseUpdateNbrExpDeepSky: " + ex.Message
                            LogSessionEntry("ERROR", "DatabaseUpdateNbrExpDeepSky: " + ex.Message, "", "DatabaseUpdateNbrExpDeepSky", "PROGRAM")
                        End Try
                    End Using
                End Using
            End Using

            executionTime = DateTime.UtcNow() - startExecution
            LogSessionEntry("DEBUG", "  DatabaseUpdateNbrExpDeepSky: " + executionTime.ToString, "", "DatabaseUpdateNbrExpDeepSky", "PROGRAM")

        Catch ex As Exception
            DatabaseUpdateNbrExpDeepSky = "DatabaseUpdateNbrExpDeepSky: " + ex.Message
            LogSessionEntry("ERROR", "DatabaseUpdateNbrExpDeepSky: " + ex.Message, "", "DatabaseUpdateNbrExpDeepSky", "PROGRAM")
        End Try
    End Function
End Module