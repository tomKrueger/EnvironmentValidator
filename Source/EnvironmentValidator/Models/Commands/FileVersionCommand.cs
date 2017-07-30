using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EnvironmentValidator.Models.Commands
{
    public class FileVersionCommand : Command
    {
        public FileVersionCommand()
            : base("FileVersion")
        {
        }

        public override Task<CommandResult> ExecuteAsync()
        {
            // Use Task.Run so that a Task is returned since
            // the code to check for existence is not Aysnc.
            return Task.Run<CommandResult>(() =>
            {
                var result = new CommandResult(this);

                try
                {
                    if (string.IsNullOrWhiteSpace(FilePath)) { throw new ArgumentException("FilePath not specified.", "FilePath"); }
                    if (string.IsNullOrWhiteSpace(ExpectedVersion)) { throw new ArgumentException("ExpectedVersion not specified.", "ExpectedVersion"); }

                    if (!File.Exists(FilePath))
                    {
                        throw new Exception($"File does not exist. File Expected:{FilePath}");
                    }

                    var fvi = FileVersionInfo.GetVersionInfo(FilePath);
                    var actualVersion = new Version(fvi.FileMajorPart, fvi.FileMinorPart, fvi.FileBuildPart, fvi.FilePrivatePart);

                    CompareVersions(ExpectedVersion, actualVersion);

                    // Exact 1.1.1.1
                    // Partial 1.1
                    // Greater than 1.1.1.>110 (1.1.1.99999 is ok, but 1.1.2.0 is not ok)
                    // Between 1.1.1.110, 1.1.2
                    // 

                    result.Status = ResultStatus.Success;
                }
                catch (Exception ex)
                {
                    result.Status = ResultStatus.Error;
                    result.Exception = ex;
                }

                return result;
            });

        }

        private void CompareVersions(string expectedVersionFormat, Version actualVersion)
        {
            if (expectedVersionFormat.StartsWith("BETWEEN", StringComparison.OrdinalIgnoreCase))
            {
                CompareBetweenVersions(expectedVersionFormat, actualVersion);
                return;
            }

            if (expectedVersionFormat.StartsWith(">=") || expectedVersionFormat.StartsWith("GREATER THAN OR EQUAL TO"))
            {
                expectedVersionFormat = expectedVersionFormat.Replace(">=", "");
                expectedVersionFormat = expectedVersionFormat.Replace("GREATER THAN OR EQUAL TO", "");
                expectedVersionFormat = expectedVersionFormat.Trim();

                CompareVersion(expectedVersionFormat, actualVersion, ComparisonType.GreaterThanOrEqualTo);
                return;
            }

            if (expectedVersionFormat.StartsWith(">") || expectedVersionFormat.StartsWith("GREATER THAN"))
            {
                expectedVersionFormat = expectedVersionFormat.Replace(">", "");
                expectedVersionFormat = expectedVersionFormat.Replace("GREATER THAN", "");
                expectedVersionFormat = expectedVersionFormat.Trim();

                CompareVersion(expectedVersionFormat, actualVersion, ComparisonType.GreaterThan);
                return;
            }

            if (expectedVersionFormat.StartsWith("<=") || expectedVersionFormat.StartsWith("LESS THAN OR EQUAL TO"))
            {
                expectedVersionFormat = expectedVersionFormat.Replace("<=", "");
                expectedVersionFormat = expectedVersionFormat.Replace("LESS THAN OR EQUAL TO", "");
                expectedVersionFormat = expectedVersionFormat.Trim();

                CompareVersion(expectedVersionFormat, actualVersion, ComparisonType.LessThanOrEqualTo);
                return;
            }

            if (expectedVersionFormat.StartsWith("<") || expectedVersionFormat.StartsWith("LESS THAN"))
            {
                expectedVersionFormat = expectedVersionFormat.Replace("<", "");
                expectedVersionFormat = expectedVersionFormat.Replace("LESS THAN", "");
                expectedVersionFormat = expectedVersionFormat.Trim();

                CompareVersion(expectedVersionFormat, actualVersion, ComparisonType.LessThan);
                return;
            }

            // Assume Equal To comparison if non of previous were found.
            // There may be no leading operator for equal to:
            // Examples:
            // x.x.x.x
            // = x.x.x.x
            // EQUAL TO x.x.x.x
            expectedVersionFormat = expectedVersionFormat.Replace("=", "");
            expectedVersionFormat = expectedVersionFormat.Replace("EQUAL TO", "");
            expectedVersionFormat = expectedVersionFormat.Trim();

            CompareVersion(expectedVersionFormat, actualVersion, ComparisonType.EqualTo);
        }

        /// <summary>
        /// Compares expected version BETWEEN format with actual version.
        /// 
        /// The BETWEEN operator is inclusive of minVersion and maxVersion.
        /// </summary>
        /// <param name="expectedVersionFormat"></param>
        /// <param name="actualVersion"></param>
        /// <example>
        /// Full versions specified.
        /// BETWEEN x.x.x.x AND x.x.x.x
        ///
        /// Partial versions specified.
        /// BETWEEN x.x AND x.x.x
        /// </example>
        private void CompareBetweenVersions(string expectedVersionFormat, Version actualVersion)
        {
            string[] parts = expectedVersionFormat.Split(' ');

            //
            // Validate expected version BETWEEN format.
            //
            if (!parts[0].Equals("BETWEEN", StringComparison.OrdinalIgnoreCase))
            {
                throw new Exception($"Unable to parse 'BETWEEN' keyword from Expected Version.  Expected: 'BETWEEN x.x.x.x AND x.x.x.x' Actual: '{expectedVersionFormat}'");
            }

            Version minVersion;
            if (!TryParseVersion(parts[1], out minVersion))
            {
                throw new Exception($"Unable to parse 'MinVersion' from Expected Version.  Expected: 'BETWEEN x.x.x.x AND x.x.x.x' Actual: '{expectedVersionFormat}'");
            }

            if (!parts[2].Equals("AND", StringComparison.OrdinalIgnoreCase))
            {
                throw new Exception($"Unable to parse 'AND' keyword from Expected Version.  Expected: 'BETWEEN x.x.x.x AND x.x.x.x' Actual: '{expectedVersionFormat}'");
            }

            Version maxVersion;
            if (!TryParseVersion(parts[3], out maxVersion))
            {
                throw new Exception($"Unable to parse 'MaxVersion' from Expected Version.  Expected: 'BETWEEN x.x.x.x AND x.x.x.x' Actual: '{expectedVersionFormat}'");
            }

            //
            // Ensure actual version is between min and max versions.
            //
            if (actualVersion < minVersion)
            {
                throw new ArgumentOutOfRangeException($"Actual version is less than expected minimum version.  Expected Min Version: '{minVersion}' Actual: '{actualVersion}'");
            }

            if (actualVersion > maxVersion)
            {
                throw new ArgumentOutOfRangeException($"Actual version is greater than expected maximum version.  Expected Max Version: '{maxVersion}' Actual: '{actualVersion}'");
            }
        }

        /// <summary>
        /// Compares expected version with actual version based on the given 
        /// compare type.
        /// </summary>
        /// <param name="expectedVersion"></param>
        /// <param name="actualVersion"></param>
        /// <example>
        /// </example>
        private void CompareVersion(string expectedVersionFormat, Version actualVersion, ComparisonType compareType)
        {
            Version expectedVersion;
            if (!Version.TryParse(expectedVersionFormat, out expectedVersion))
            {
                throw new Exception($"Unable to parse 'Version' from Expected Version.  Expected: '> x.x.x.x' Actual: '{expectedVersion}'");
            }

            string errorMsg = null;
            switch (compareType)
            {
                case ComparisonType.EqualTo:
                    if (!(actualVersion == expectedVersion))
                    {
                        errorMsg = "Equal To";
                    }
                    break;
                case ComparisonType.GreaterThan:
                    if (!(actualVersion > expectedVersion))
                    {
                        errorMsg = "Greater Than";
                    }
                    break;
                case ComparisonType.GreaterThanOrEqualTo:
                    if (!(actualVersion >= expectedVersion))
                    {
                        errorMsg = "Greater Than or Equal To";
                    }
                    break;
                case ComparisonType.LessThan:
                    if (!(actualVersion < expectedVersion))
                    {
                        errorMsg = "Less Than";
                    }
                    break;
                case ComparisonType.LessThanOrEqualTo:
                    if (!(actualVersion <= expectedVersion))
                    {
                        errorMsg = "Less Than or Equal To";
                    }
                    break;
                default:
                    throw new NotSupportedException($"Compare Type '{compareType}' is not supported.");

            }

            if (errorMsg != null)
            {
                throw new ArgumentOutOfRangeException($"Actual version is not '{errorMsg}' expected version.  Expected Version: '{expectedVersion}' Actual: '{actualVersion}'");
            }
        }

        private bool TryParseVersion(string value, out Version version)
        {
            if (string.IsNullOrWhiteSpace(value)) { throw new ArgumentNullException("value"); }
            
            // Ensure there is at least a major and minor version
            // otherwise the Version.TryParse will fail.
            if (value.IndexOf('.') < 0)
            {
                value = value + ".0";
            }

            return Version.TryParse(value, out version);
        }

        public string FilePath { get; set; }

        public string ExpectedVersion { get; set; }
    }

    public enum ComparisonType
    {
        NotSpecified,
        LessThan,
        LessThanOrEqualTo,
        EqualTo,
        GreaterThan,
        GreaterThanOrEqualTo
    }
}
