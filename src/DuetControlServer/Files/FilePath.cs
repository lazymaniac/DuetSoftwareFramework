﻿using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DuetControlServer.Files
{
    /// <summary>
    /// Static class used to provide functions for file path resolution
    /// </summary>
    public static class FilePath
    {
        /// <summary>
        /// Default name of the config file
        /// </summary>
        public const string ConfigFile = "config.g";

        /// <summary>
        /// Fallback file if the config file could not be found
        /// </summary>
        public const string ConfigFileFallback = "config.g.bak";

        /// <summary>
        /// Config override as generated by M500
        /// </summary>
        public const string ConfigOverrideFile = "config-override.g";

        /// <summary>
        /// Daemon file used to perform periodic tasks
        /// </summary>
        public const string DaemonFile = "daemon.g";

        /// <summary>
        /// Default heightmap file
        /// </summary>
        public const string DefaultHeightmapFile = "heightmap.csv";

        /// <summary>
        /// Fallback file if the probe-specific deploy probe file could not be found
        /// </summary>
        public const string DeployProbeFallbackFile = "deployprobe.g";

        /// <summary>
        /// Fallback file if the probe-specific retract probe file could not be found
        /// </summary>
        public const string RetractProbeFallbackFile = "retractprobe.g";

        /// <summary>
        /// Probe-specific deploy file name pattern
        /// </summary>
        public static Regex DeployProbePattern = new Regex(@"deployprobe\d+\.g");

        /// <summary>
        /// Probe-specific retract file name pattern
        /// </summary>
        public static Regex RetractProbePattern = new Regex(@"retractprobe\d+\.g");

        /// <summary>
        /// File holding the filaments mapping
        /// </summary>
        public const string FilamentsFile = "filaments.csv";

        /// <summary>
        /// Resolve a RepRapFirmware/FatFs-style file path to a physical file path asynchronously.
        /// The first drive (0:/) is reserved for usage with the base directory as specified in the settings
        /// </summary>
        /// <param name="filePath">File path to resolve</param>
        /// <param name="directory">Directory containing filePath if it is not absolute is specified</param>
        /// <returns>Resolved file path</returns>
        public static async Task<string> ToPhysicalAsync(string filePath, FileDirectory directory)
        {
            Match match = Regex.Match(filePath, "^(\\d+):?/?(.*)");
            if (match.Success && int.TryParse(match.Groups[1].Value, out int driveNumber))
            {
                if (driveNumber == 0)
                {
                    return Path.Combine(Path.GetFullPath(Settings.BaseDirectory), match.Groups[2].Value);
                }

                using (await Model.Provider.AccessReadOnlyAsync())
                {
                    if (driveNumber > 0 && driveNumber < Model.Provider.Get.Volumes.Count)
                    {
                        return Path.Combine(Model.Provider.Get.Volumes[driveNumber].Path, match.Groups[2].Value);
                    }
                }

                throw new ArgumentException("Invalid drive index");
            }

            if (!filePath.StartsWith('/'))
            {
                string directoryPath;
                using (await Model.Provider.AccessReadOnlyAsync())
                {
                    directoryPath = directory switch
                    {
                        FileDirectory.Filaments => Model.Provider.Get.Directories.Filaments,
                        FileDirectory.Firmware => Model.Provider.Get.Directories.Firmware,
                        FileDirectory.GCodes => Model.Provider.Get.Directories.GCodes,
                        FileDirectory.Macros => Model.Provider.Get.Directories.Macros,
                        FileDirectory.Menu => Model.Provider.Get.Directories.Menu,
                        FileDirectory.Scans => Model.Provider.Get.Directories.Scans,
                        FileDirectory.System => Model.Provider.Get.Directories.System,
                        FileDirectory.Web => Model.Provider.Get.Directories.Web,
                        _ => Model.Provider.Get.Directories.System,
                    };

                    match = Regex.Match(directoryPath, "^(\\d+):?/?(.*)");
                    if (match.Success && int.TryParse(match.Groups[1].Value, out driveNumber))
                    {
                        if (driveNumber == 0)
                        {
                            directoryPath = Path.Combine(Path.GetFullPath(Settings.BaseDirectory), match.Groups[2].Value);
                        }

                        if (driveNumber > 0 && driveNumber < Model.Provider.Get.Volumes.Count)
                        {
                            directoryPath = Path.Combine(Model.Provider.Get.Volumes[driveNumber].Path, match.Groups[2].Value);
                        }
                    }
                }
                return Path.Combine(Path.GetFullPath(Settings.BaseDirectory), directoryPath, filePath);
            }
            return Path.Combine(Path.GetFullPath(Settings.BaseDirectory), filePath.StartsWith('/') ? filePath.Substring(1) : filePath);
        }

        /// <summary>
        /// Resolve a RepRapFirmware/FatFs-style file path to a physical file path asynchronously.
        /// The first drive (0:/) is reserved for usage with the base directory as specified in the settings.
        /// </summary>
        /// <param name="filePath">File path to resolve</param>
        /// <param name="directory">Directory containing filePath if it is not absolute is specified</param>
        /// <returns>Resolved file path</returns>
        public static async Task<string> ToPhysicalAsync(string filePath, string directory = null)
        {
            Match match = Regex.Match(filePath, "^(\\d+):?/?(.*)");
            if (match.Success && int.TryParse(match.Groups[1].Value, out int driveNumber))
            {
                if (driveNumber == 0)
                {
                    return Path.Combine(Path.GetFullPath(Settings.BaseDirectory), match.Groups[2].Value);
                }

                using (await Model.Provider.AccessReadOnlyAsync())
                {
                    if (driveNumber > 0 && driveNumber < Model.Provider.Get.Volumes.Count)
                    {
                        return Path.Combine(Model.Provider.Get.Volumes[driveNumber].Path, match.Groups[2].Value);
                    }
                }

                throw new ArgumentException("Invalid drive index");
            }

            if (directory != null && !filePath.StartsWith('/'))
            {
                match = Regex.Match(directory, "^(\\d+):?/?(.*)");
                if (match.Success && int.TryParse(match.Groups[1].Value, out driveNumber))
                {
                    if (driveNumber == 0)
                    {
                        directory = Path.Combine(Path.GetFullPath(Settings.BaseDirectory), match.Groups[2].Value);
                    }

                    using (await Model.Provider.AccessReadOnlyAsync())
                    {
                        if (driveNumber > 0 && driveNumber < Model.Provider.Get.Volumes.Count)
                        {
                            directory = Path.Combine(Model.Provider.Get.Volumes[driveNumber].Path, match.Groups[2].Value);
                        }
                    }
                }

                return Path.Combine(Path.GetFullPath(Settings.BaseDirectory), directory, filePath);
            }
            return Path.Combine(Path.GetFullPath(Settings.BaseDirectory), filePath.StartsWith('/') ? filePath.Substring(1) : filePath);
        }

        /// <summary>
        /// Convert a physical ile path to a RRF-style file path asynchronously.
        /// The first drive (0:/) is reserved for usage with the base directory as specified in the settings.
        /// </summary>
        /// <param name="filePath">File path to convert</param>
        /// <returns>Resolved file path</returns>
        public static async Task<string> ToVirtualAsync(string filePath)
        {
            if (filePath.StartsWith(Settings.BaseDirectory))
            {
                filePath = filePath.Substring(Settings.BaseDirectory.EndsWith('/') ? Settings.BaseDirectory.Length : (Settings.BaseDirectory.Length + 1));
                return Path.Combine("0:/", filePath);
            }

            using (await Model.Provider.AccessReadOnlyAsync())
            {
                foreach (DuetAPI.Machine.Volume storage in Model.Provider.Get.Volumes)
                {
                    if (filePath.StartsWith(storage.Path))
                    {
                        return Path.Combine("0:/", filePath.Substring(storage.Path.Length));
                    }
                }
            }

            return Path.Combine("0:/", filePath);
        }
    }
}