// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;

namespace Microsoft.DotNet.Interactive.PowerShell.Host
{
    using System.Management.Automation;
    using System.Text;

    internal static class DollarProfileHelper
    {
        private const string _profileName = "Microsoft.dotnet-interactive_profile.ps1";

        internal static string AllUsersCurrentHost => GetFullProfileFilePath(forCurrentUser: false);
        internal static string CurrentUserCurrentHost => GetFullProfileFilePath(forCurrentUser: true);

        private static string GetFullProfileFilePath(bool forCurrentUser)
        {
            if (!forCurrentUser)
            {
                string pshome = Path.GetDirectoryName(typeof(PSObject).Assembly.Location);
                return Path.Combine(pshome, _profileName);
            }

            string configPath = Platform.IsWindows
                ? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "PowerShell")
                : Platform.SelectProductNameForDirectory(Platform.XDG_Type.CONFIG);

            return Path.Combine(configPath, _profileName);
        }

        public static PSObject GetProfileValue()
        {
            PSObject dollarProfile = new PSObject(CurrentUserCurrentHost);
            dollarProfile.Properties.Add(new PSNoteProperty("AllUsersCurrentHost", AllUsersCurrentHost));
            dollarProfile.Properties.Add(new PSNoteProperty("CurrentUserCurrentHost", CurrentUserCurrentHost));
            // TODO: Decide on whether or not we want to support running the AllHosts profiles

            return dollarProfile;
        }

        public static string GetProfileScript()
        {
            var profileScript = new StringBuilder();
            if (File.Exists(AllUsersCurrentHost))
            {
                profileScript.AppendLine($"& '{AllUsersCurrentHost}'");
            }
            if (File.Exists(CurrentUserCurrentHost))
            {
                profileScript.AppendLine($"& '{CurrentUserCurrentHost}'");
            }

            return profileScript.ToString();
        }
    }
}
