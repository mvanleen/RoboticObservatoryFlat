Module ModTODO
    ' all times are in UTC !
    ' module for aquiring flats
    ' think of module for sky flats ?

    ' use pressure and temperature off sensor Pegasus (switch) or MGBox (environmental device)

    ' implement guiding
    ' variable star observing


    'version history
    '-------------------
    '1.56:  added comet support: when comet is enabled, at each time the comet is selected to image, the current coordinates are refetched from TSX
    '       bug: when starting run when Sun at flat time: nothing happens (euipmentstatus was set to 'error')
    '       bug: when / in target name, TSX throws error while creating file (remove / from name before saving file)
    '       bug: during run no more targets => hangs instead of pause
    '1.57:  bug target window had no name
    '       added moon support: switch enables per target to ignore moon settings
    '       bug: disaster check would switch everything off and not only the switches that are needed
    '       change: deprecated pTheskXRASCOMTele6 32 bit and replaced by 64 bit library as per instruction Software Bisque
    '       bug: will now verify equipment at startup => hard shutdown no longer possible at program restart
    '       bug: with an ASI-camera, TSX says there is no temperature control: This command is not supported. Error = 11000.
    '1.58:  added additional comet moon support part 2
    '       added logic to take into account observation program in check run in preparation of variables
    '       bug: in frmTools: when equipment is at paused, the shutdown button will first ask to start equipmment: added paused to acceptable statusses
    '       bug: added messages in WaitForSeconds + added more time to Baader remote power switch + removed some uneccesary delays
    '1.59   change: modified target selection: for non circumpolar targets will look at azimuth and number of frames needed: will prevent switching rapidly fe between M13 and LBN248
    '       bug: calibration window: number of bias was used instead of number of darks
    '       change: target marked unusable = essential in stead of brief => visual warning
    '       change: if Sun already dropped below flat height - 4: issue message that flats cannot be taken
    '       change: flats: added message when not enough light to continue
    '       change: waitseconds changed: removed doevents
    '1.60   change: changed splashscreen
    '       change: waitseconds changed: added extra parameter
    '       bug: TurnOffEquipment now tests if Snapcap is already closed before closing => no mount error if mount is not active but only the Pegasus
    '       change: change in startup sequence to increase responsivness: check equimpent is now done post load + added extra messages
    '       change: when no deepsky left: added error message !
    '       change: complete HADS target screen
    '       change: renaming in ModDatabaseDeepSky
    '1.61   change: removed abort in WaitSeconds: function aborted prematurely
    '       change: added function to select HADS
    '       change: added RunHADS / FocusHADS 
    '       change: rename some functions
    '1.62   change: when no roof, autorun is not possible and aborts in the morning
    '       change: at startup, depending on program, download the HADS-list
    '       change: check the correct database depending on program
    '       bug: when mount freezes, endless loop and roof will not close on smart error
    '1.63   change: added variables to CheckRun
    '1.64   bug: estimate HADS-exposure does not work
    '       bug: ChekRun Deepsky + Moon unsafe handled incorrectly
    '       change: HADS add table truncate and delete inactive records
    '1.65   change: added HADS declination cutoff
    '       change: error management on hardware errors: properly abort
    '       change: removed double code compass direction / index
    '       change: LastObserved filled at every exposure
    '1.66   bug: no snapcap: buttons on tool windows not disabled
    '       change: changed timers to fire every +-5 seconds in stead of one second: frees up resources
    '       change: when disconnected mount not all fields are cleared
    '       change: when disconnected CCD not all fields are cleared
    '       change: add tab to debug message for readability
    '1.67   change: slew timeout / TSX timeout: added sleep + doevents + extra message in CheckSwitchStatus + added 150 seconds extra to timeout slew and TSX image
    '       bug: SMART Error did not take moon settings into account
    '1.68   change: only update / insert HADS records when needed
    '       change: finished focusstar HADS
    '1.69   change: added filter to main screen
    '       change: multiple dark exposure times calibration
    '       bug: will update all records HADS: datatype was not correct at EYEMAOBS
    '1.70   change: never do select * from
    '       change: explicitly add RA/DEC to image + other FITS-keywords needed for Pinpoint plate solving
    '       bug: HADSActive not filled correctly
    '       change: notation RA h m s and DEC ° ' "
    '       change: naming conventions variables functions: v
    '1.71:  change: added autofocus to tool form
    '       bug: GetNthIndex format RA / DEC strings correct to : in stead of hms
    '       bug: Tool window: focus position must be entered before moving focusser
    '1.72:  bug: Target form delete throws SQL error
    '       change: target adding last data observed and remarks
    '       change: HADS / Target some form redesign
    '1.73:  changing cstr / cint / cdbl / cbool visual basic functions to .NET functions
    '1.74:  change: support The Sky X 64-bit
    '1.80:  change: DEC2000DG to DEC2000DG
    '       change: added mosaic to forms
    '       change: adding mosaic features
    '               supported: 2x1, 1x2, 2x2
    '               panel numbering:
    '               |1|2|   |1|   |12|  |123|
    '                       |2|   |34|  |456|
    '1.81 change: extra message when starting HADS
    '     change: when stopped observation HADS, reselect the same target when continuing
    '     bug: HADS shows 0 as first exposure
    '     change: added error column to HADS-form + layout datagrid
    '1.82 change: sort order Target / HADS changed in form: error shown on top
    '     change: fix object initialisation new -> using / unused variables
    '     bug: did not cool at "DEEPSKY ELSE VARIABLES": spelled incorrectly in CheckRun
    '1.83 change: HADS variables: refocus after x exposures
    '1.84 bug: will pick the same HADS the day after due to changes in 1.82
    '1.85 bug: DEEPSKY else VARIABLES does not takes moon UNSAFE into account
    '     bug: error when loglocation does not exists
    '     bug: several bugs in CheckRun: revised and documented
    '     bug: mosaic not doing a closed loop slew: use focus star to do closed loop slew
    '     bug: refocus < in stead of <= number of images
    '     bug: mosaic incorrectly setting target done
    '1.86 change: when updating HADS-record do not reload grid => will effectively go to next record in stead of all the way up
    '1.87 bug: in temperature comparison cooldown CCD calibration / flats
    '1.89 change: add HADSactive colun to overview HADS-form
    '     change: naming TALON to Roof / Dragonfly to Switch / Snapcap to cover
    '     change: added My.Settings.sMountStartupLink: http://user:secret@172.20.7.49/?cmd=5&p=1&a1=1&a2=0&s=2
    '     change: use ASCOM choosers - at first run cleanup all the error messages
    '             https://www.youtube.com/watch?v=SfFg5xoVKhg
    '               CCD, filterwheel, autoguider = TSX - no change needed
    '               Folder Locations: ok
    '               Roof: OK
    '               Mount: OK
    '               AAG: OK
    '               Snapcap: OK
    '               Dragonfly: OK
    '               UPS
    '1.90 bug: while waiting for dusk, SMART ERROR timeout when short spell of clouds: added unsafe to condition "SMART ERROR: Weather is safe, run is hanging!"
    '     bug: ignore moon switch general / target dit not work properly
    '1.91 change: added sun altitude when taking flats
    '     change: added switch status in a background worker: disconnect freezes UI and causes timeout errors
    '1.92 change: added (Position = " + Format(pTheSkyXCamera.focPosition) + ")" when running @focus3
    '1.93 change: when safe, add message before run starts
    '     change: for weather to be safe, switch and sensor must be safe
    '     change: Marking " + deepsky / var + " as unusable: is now an error
    '     change: added lunar altitude when switching to variables
    '1.94 bug: previous change is causing repetitive messages
    '1.95 bug: typos + errors not played on Telegram, added button to play messages
    '1.96 change: when # expsosures is changed during run, the number was not changed dynamically
    '     bug: focus temperature format unified + bug in amount for refocus fe 1.6 became 2 which is not correct => too much refocus
    '     bug: Sentinel closes roof: when restarting run, roof is not reopend
    '1.97 change: DUSK FLATS: not enough light => park mount (sometimes mount is not parked and nothing happens
    '     bug: pmount.tracking = true does not work on Paramount
    '     change: added moon alt on all "moving mount to"
    '1.98 bug: number of exposures improperly calculated due to 1.96
    '     bug pmount.tracking = true needed for 10 micron: build in check
    '1.99 change: some messages changed
    '2.00 change: before opening roof, check mount is parked !
    '2.01 change: added 60 seconds to The Sky X timeout
    '2.02 change: target form Mosaic features: several bugs
    '     change: Mosaic run: several bugs
    '2.03 change: target selection: only look at done and not at exposed frames: conflict with mosiacs
    '     change: update number of frames also for mosaics
    '     change: added parameter for closed loop slew to focus stars
    '     change: added closed loop slew to HADS-flow
    '     bug: while running flats when not enough light changed order so only 1 park command is issued (avoids: AsyncMountPark: Not parked after trying to park)
    '     change: no mosaic possible when target is comet
    '     bug: mosaic target slews were not correct
    '2.04 bug: introduced bug in normal deepsky procedure due to unclear naming of solving / actual positions
    '     bug: Switching to variables removed: was repeated in inappropriate places: has no use
    '2.05 bug: another bug causing jumps after a 2de closed loop slew is done
    '2.06 change: mosaic 2x2 order changed: due to meridian flip images can be mirrored
    '     change: save mosaic overlap in database for future reference
    '     change: remove message when moving mount between exposures
    '     change: dithering: take into account cos(declination)
    '2.07 bug: replaced hardcoded link for starting mount in switching procedures
    '     change: clariefied some property fields
    '2.08 bug: frmCalibration cooling took in stead of 0.5 => changed to integer 1
    '2.09 bug: Total pointing error RA: 84.3 DEC: 4.8 added "arcsec", removed excess space
    '     bug: Correcting error: added .
    '     change: "Set pRunStatus to: changed to DEBUG
    '     bug: frmCalibration cooling changed calculation method do while
    '     change: mosaic flow: added panel numbers to filenames images
    '     change: add panel numbers in mosaic images
    '     change: added pMountIsParking to prevent multiple park commands: to test by running safety measures
    '     change: HADS update database: removed parameter
    '     change: frmProperties when not 10micron mount, disable the startup link text
    '     bug: several bugs in frmTarget
    '2.10 change: Moon safety calculation is not optimal: sometimes dark skies are deemed unsafe and bright skies are deemed safe
    '             New calculation:
    '             -----------------
    '             if altitude < x fe 0° => always safe
    '             else
    '               if phase <= a fe 5 => always safe
    '               elseif phase <= b fe 25 => we can start at altitude y fe 20°
    '               else phase > b 25 we start at altitude z fe 5°
    '               New properties:
    '               -------------------
    '                 sMoonAltitudeAlwaysSafe
    '                 sMoonPhaseAlwaysSafe
    '                 sMoonPhaseLimitLow
    '                 sMoonAltitudeLimitLow
    '                 sMoonStartCooldownLow
    '                 sMoonAltitudeLimitHigh
    '                 sMoonStartCooldownHigh
    '3.00 change: Flat UI design
    '               https://icon-icons.com/pack/Iconsax-Vol2---bulk/3868&page=4
    '               https://flatuicolors.com/palette/fr
    '               https://editor.method.ac/
    '       change: standard messagebox replace by form
    '3.01   bug: park aborts using Paramount => added park condition to abort slew
    '3.02   bug: changed coloring of AAG data as certain values were not visible
    '       changed: added option to use someone elses AAG skywatcher if mine breaks down
    '       bug: RunHADS: Index was outside the bounds of the array
    '       change: AAG timestamp timeout verification
    '3.03   bug: error in insert procedure deepsky
    '3.04   changed: added instructions for HADS yearly refresh
    '3.05   bug: Paramount does not know side of pier before homing
    '3.06   change: added sMountTimeout / sCoverTimeOut / sCameraTimeout
    '3.07   bug: change cover color when no cover
    '       bug: RunHADS: Index was outside the bounds of the array. 
    '       change: added uniform debug reporting in modules
    '       change: added message when slewing to focus star / object
    '3.08   change: added switch sEnableDebugLogging: full debug text logging
    '       bug: when CLOUDED NO ROOF no error message is played 
    '3.09   bug: modified number of columns in HADS array due to error
    '       bug: number format . , when estimating HADS magnitude
    '3.10   change: removed duplicate button in target window
    '3.20   change: better explanations concerning debugging
    '               added logging concerning cover
    '               removed topmost windows: annoying
    '               added extra checks for cover so serial is not opened all the time
    '3.21   bug: related to Paramount
    '3.22   change: removed possibility to type in error box
    '       change: using id to insert/update records in deepsky
    '3.23   change: SAFETY CHECK NIGHT as error !
    '       change: DUSK FLATS: not enough light. Waiting for sun to set. => park the mount, changed if tot <> true as status flag is not always set correctly
    '       change: Marking V377 Boo as unusable: will only give error following x HADS variables failures
    '       change: "Focus slew failed! => made essential in stead of error






    '1/05/2025 
    '- volgorde variabelen ? lijkt nergens op


    '-- bugs in forms
    ' kip of ei: eerst plate solve en dan focus of omgekeerd ?

    'RGB filters



    '&& homing voor unpark maar enkel indien nodig
    '&& geeft geen alarm bij BEWOLKING EN GEEN DAK !!!
    '&& last observed hads is sorted as text and not as date
    '&& maan opsplitsen < 40% => vanaf 10° en anders 0° / < 10% gewoon altijd


    ' HOLD
    ' change: calculate number of stars in frame as support for aag cloudwatcher
    ' change: guiding: needs implementation



    'KNOWN EQUIPMENT ISSUES
    '- @Focus3: Error code = 5
    '- PC looses connection to mount:
    '   --------------------------------------------
    '   22:18:35: Exposure completed :  NGC 772_L_Exp180s_Bin1_20231007_221835.fit
    '   22:18:36: Moving mount To: RA 01:58:58 - DEC 19° 07' 27" / Alt 46.18° - Az 102.40°
    '   22:18:48: CheckMountStatus: CheckDotNetExceptions ASCOM.tenmicron_mount.Telescope SideOfPier Get System.Exception: Can't read bytes from socket: System.IO.IOException Unable to read data from the transport connection: A connection attempt failed because the connected party did not properly respond after a period of time, or established connection failed because connected host has failed to respond. (See Inner Exception for details)
    '   22:18:48: Aborting run  due To mount Error...
    '   22:18:48: Pausing: parking mount And closing roof.
    '   22:18:52: MountPark: The remote procedure Call failed. (Exception from HRESULT: 0x800706BE)
    '   22:18:52: Closing roof...
    '   22:18:52: CheckMountStatus: The RPC server Is unavailable. (Exception from HRESULT: 0x800706BA)
    '   22:18:52: ABORTING RUN


End Module
