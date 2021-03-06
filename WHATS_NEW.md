Summary of important changes in recent versions
===============================================

Version 3.1.1
==============

Compatible files:
- RepRapFirmware 3.1.1
- DuetWebControl 3.1.1

Changed behaviour:
- Increased API level due to new object model fields
- Code replies from the firmware are now trimmed at the end right after receipt

Bug fixes:
- Final replies from system macros were discarded
- Substituted macro filenames were incorrect in the DCS log
- Codes requesting message boxes could be executed twice
- Message boxes could be closed internally in DCS when not supposed to
- Codes from pause.g were cancelled under certain circumstances

Version 3.1.0
==============

Compatible files:
- RepRapFirmware 3.1.0
- DuetWebControl 3.1.0

Changed behaviour:
- Duplicate code parameters are now ignored
- If M122 cannot obtain locks in DCS within 2s, the lock is ignored
- SPI poll delay is skipped during updates

Bug fixes:
- When pausing a print at the end of a file, the file was closed on resume
- M500 P10 did not work
- DCS parameter for updates (-u) was broken if another instance was not started
- Whole-line comments were not truncated before they were sent to RRF
- Only the first filament usage was parsed from files generated by Prusa Slicer
- M505 did not return the current sys directory when invoked without parameters
- Whole-line comments preceding a code that requests a macro file could cause the code to be executed twice

Version 2.2.0
==============

Compatible files:
- RepRapFirmware 3.01-RC12
- DuetWebControl 2.1.7

Changed behaviour:
- Changed letter of unprecedented parameters from '\0' to '@'
- Increased default and minimum API version number to 7
- Whole line comments are now sent to RepRapFirmware

Known issues:
- Print/Simulation times are not written to G-code files
- Codes with invalid expressions may not instantly terminate a macro or job file

Bug fixes:
- Expressions in square brackets were not evaluatated
- M500 wrote workplace coordinates without offsetting the indices by 1

Version 2.1.3
==============

Compatible files:
- RepRapFirmware 3.01-RC11
- DuetWebControl 2.1.6

Changed behaviour:
- Warning message is shown in the DCS log when API clients with an old version number connect

Known issues:
- Print/Simulation times are not written to G-code files
- Comments for object cancellation detection are not parsed (work-around is to use M486 directly)
- Codes with invalid expressions may not instantly terminate a macro or job file

Bug fixes:
- Unchanged arrays could be reported in Patch subscription mode
- Initial query in Patch mode was not working
- Web server did not clear HTTP endpoints under certain circumstances
- echo expressions were not parsed correctly if strings contained commas
- Changing the system time just before a controller reset could lead to an abnormal program termination

Version 2.1.2
==============

Compatible files:
- RepRapFirmware 3.01-RC10
- DuetWebControl 2.1.5

Known issues:
- Print/Simulation times are not written to G-code files
- Comments for object cancellation detection are not parsed (work-around is to use M486 directly)
- Codes with invalid expressions may not instantly terminate a macro or job file

Bug fixes:
- Leading G53 wasn't added to string representations of parsed codes
- Starting DCS with the fifo CPU scheduler via systemd could lead to maximum CPU usage
- Some nullable RRF OM fields were not declared as such in the DSF OM

Version 2.1.1
==============

Compatible files:
- RepRapFirmware 3.01-RC10
- DuetWebControl 2.1.5

Changed behaviour:
- If DCS cannot establish a connection to RRF, the error message is always printed
- Code parser exceptions report the filename
- File info parser scans parsed comments in the file footer like in the file header
- Increased priority in systemd service for DCS and start it at `basic.target` instead of `multi-user.target`

Known issues:
- Print/Simulation times are not written to G-code files
- Comments for object cancellation detection are not parsed (work-around is to use M486 directly)
- Codes with invalid expressions may not instantly terminate a macro or job file

Bug fixes:
- Expression code parameters were not properly printed in the log
- Double quotes were incorrectly parsed
- limits key was not updated in the object model
- Height map file was overwritten by the RepRapFirmware object model
- G29 S1/M375 didn't print an offset warning when a heightmap was loaded without homing Z first
- Order of M0/M1 and notification about the print being cancelled was wrong
- Some internal fields of the Code object were incorrectly serialized
- Codes could finish in the wrong order
- PrusaSlicer print time and layer height were not parsed correctly
- Expression fields were always evaluated from the DSF object model

Version 2.1.0
==============

Compatible files:
- RepRapFirmware 3.01-RC9
- DuetWebControl 2.1.4

Changed behaviour:
- Implemented conditional G-code according to https://duet3d.dozuki.com/Wiki/GCode_Meta_Commands (same command set as supported by RRF)
- DuetAPI version number has been increased, however the previous one is still accepted
- DuetAPI uses relaxed JSON escaping like in the DCS settings file
- Added new fields stepsPerMm and microstepping to Axis amd Extruder items to DuetAPI
- Increased maximum size of messages being sent to the firmware from 256 bytes to 4KiB
- Removed SpiPollDelaySimulating and renamed SpiPollDelaySimulating to FileBufferSize in the DCS settings (the latter is now used for code files, too)
- Simple text-based codes no longer report when they are cancelled

Known issues:
- Print/Simulation times are not written to G-code files
- Comments for object cancellation detection are not parsed (work-around is to use M486 directly)

Bug fixes:
- DuetControlServer could sporadically hang when printing a file
- Fixed deadlock that could occur when the SPI task tried to resolve pending requests
- M20 was not fully compatible with RRF
- Concatenating code parser exception caused the line to be appended multiple times
- Filament sensors and move.kinematics were neither properly updated nor serialized
- Codes of macros being cancelled were sometimes aborted with a wrong exception

Version 2.0.0
==============
Compatible files:
- RepRapFirmware 3.01-RC8
- DuetWebControl 2.1.3

Changed behaviour:
- M999 stops DCS. This behaviour can be changed by starting it with the `-r` command-line argument or by changing the config value `NoTerminateOnReset` to `true`
- Plugins using prior API versions are no longer compatible and require new versions of the API libraries
- Codes M21+M22 are not supported and will throw an error
- Code expressions are now preparsed and Linux object model fields are substituted before the final evaluation

Known issues:
- Conditional G-codes (aka meta commands) except for echo are not supported yet
- Print/Simulation times are not written to G-code files
- Comments for object cancellation detection are not parsed (work-around is to use M486 directly)

Bug fixes:
- Added compatibility for G-code meta expressions
- When all macros were aborted the messages were not properly propagated to the start code(s)
- Some codes were incorrectly sent when aborting all files
- Some macro codes could be executed in the wrong order when multiple macros were invoked
- Code requests from the firmware could cause a deadlock
