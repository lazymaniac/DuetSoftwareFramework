namespace DuetControlServer.SPI.Communication.LinuxRequests
{
    /// <summary>
    /// Request indices for SPI transfers from the Linux board to the RepRapFirmware controller
    /// </summary>
    public enum Request : ushort
    {
        /// <summary>
        /// Perform an immediate emergency stop
        /// </summary>
        EmergencyStop = 0,
        
        /// <summary>
        /// Reset the controller
        /// </summary>
        Reset = 1,
        
        /// <summary>
        /// Request the execution of a G/M/T-code
        /// </summary>
        /// <seealso cref="CodeHeader"/>
        Code = 2,

        /// <summary>
        /// Request a part of the machine's object model
        /// </summary>
        /// <seealso cref="Shared.StringHeader"/>
        /// <remarks>
        /// In protocol version 1, this request was used to retrieve status responses
        /// </remarks>
        GetObjectModel = 3,

        /// <summary>
        /// Set a value in the machine's object model (reserved)
        /// </summary>
        /// <seealso cref="SetObjectModelHeader"/>
        SetObjectModel = 4,

        /// <summary>
        /// Tell the firmware a print has started and set information about the file being processed
        /// </summary>
        /// <seealso cref="PrintStartedHeader"/>
        PrintStarted = 5,

        /// <summary>
        /// Tell the firmware a print has been stopped and reset information about the file being processed
        /// </summary>
        /// <seealso cref="PrintStoppedHeader"/>
        PrintStopped = 6,

        /// <summary>
        /// Notify the firmware about the completion of a macro file
        /// </summary>
        /// <seealso cref="MacroCompleteHeader"/>
        MacroCompleted = 7,

        /// <summary>
        /// Request the heightmap coordinates as generated by G29 S0
        /// </summary>
        GetHeightMap = 8,

        /// <summary>
        /// Set the heightmap coordinates via G29 S1
        /// </summary>
        /// <seealso cref="Shared.HeightMapHeader"/>
        SetHeightMap = 9,

        /// <summary>
        /// Lock movement and wait for standstill
        /// </summary>
        /// <seealso cref="Shared.CodeChannelHeader"/>
        LockMovementAndWaitForStandstill = 10,

        /// <summary>
        /// Unlock everything again
        /// </summary>
        /// <seealso cref="Shared.CodeChannelHeader"/>
        Unlock = 11,

        /// <summary>
        /// Write another chunk of the IAP binary to the designated Flash area
        /// </summary>
        /// <remarks>
        /// There is no discrete header for this request but be aware that only multiples
        /// of IFLASH_PAGE_SIZE must be transmitted (except for the last sector)
        /// </remarks>
        WriteIap = 12,

        /// <summary>
        /// Launch the IAP binary
        /// </summary>
        StartIap = 13,

        /// <summary>
        /// Assign filament to a given extruder drive
        /// </summary>
        /// <seealso cref="AssignFilamentHeader"/>
        AssignFilament = 14,

        /// <summary>
        /// Response to a <see cref="FirmwareRequests.FileChunkHeader"/>
        /// </summary>
        /// <seealso cref="LinuxRequests.FileChunk"/>
        FileChunk = 15,

        /// <summary>
        /// Evaluate an arbitrary expression
        /// </summary>
        /// <seealso cref="Shared.CodeChannelHeader"/>
        EvaluateExpression = 16,

        /// <summary>
        /// Send an arbitrary RepRapFirmware message
        /// </summary>
        /// <seealso cref="Shared.MessageHeader"/>
        Message = 17
    }
}