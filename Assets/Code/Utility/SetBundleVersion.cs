#if UNITY_EDITOR
using TMPro;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace BuildPipeline
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class SetBundleVersion : MonoBehaviour, IPreprocessBuildWithReport
    {
        private TextMeshProUGUI versionText;

        [ContextMenu("Validate")]
        void OnValidate()
        {
            if (!versionText)
                versionText = GetComponent<TextMeshProUGUI>();

            IncrementVersionNumberNumber();
            versionText.text = $"Version: {PlayerSettings.bundleVersion}"; // Constants.VersionString}";
        }

        public int callbackOrder { get { return 0; } }

        public void OnPreprocessBuild(BuildReport report)
        {
            IncreasePatchNumber();

            if (versionText)
                versionText.text = $"Version: {PlayerSettings.bundleVersion}";
        }

        private static string IncrementVersionNumberNumber(bool major = false, bool minor = false, bool patch = true)
        {
            //int incrementUpAt = 9; //if this is set to 9, then 1.9.~ will become 2.0.~

            string versionText = PlayerSettings.bundleVersion;

            versionText = versionText.Trim(); //clean up whitespace if necessary
            var parts = versionText.Split('.', '-');

            int majorVersion = 0;
            int minorVersion = 0;
            string patchVersion = string.Empty;
            string releaseType = string.Empty;

            if (parts.Length > 0) majorVersion = int.Parse(parts[0]);
            if (parts.Length > 1) minorVersion = int.Parse(parts[1]);
            if (parts.Length > 2) patchVersion = parts[2];
            if (parts.Length > 3) releaseType = parts[3];

            if (patch)
                patchVersion = System.DateTime.UtcNow.ToString("yyMMddHHmm");

            if (minor)
            {
                minorVersion++;

                //if (minorVersion > incrementUpAt)
                //{
                //    majorVersion++;
                //    minorVersion = 0;
                //}
            }

            if (major)
                majorVersion++;

            versionText = $"{majorVersion:0}.{minorVersion:0}.{patchVersion:0}";

            if (releaseType != string.Empty)
                versionText = $"{versionText}-{releaseType}";

            Debug.LogWarning("Version number incremented to " + versionText);

            PlayerSettings.bundleVersion = versionText;
            //Constants.SetVersionString(versionText);

            return versionText;
        }

        [MenuItem("Build/Increase Patch Number", false, 800)]
        private static void IncreasePatchNumber() => IncrementVersionNumberNumber(false, false, true);

        [MenuItem("Build/Increase Minor Version Number", false, 801)]
        private static void IncreaseMinorNumber() => IncrementVersionNumberNumber(false, true, false);

        [MenuItem("Build/Increase Major Version Number", false, 802)]
        private static void IncreaseMajorNumber() => IncrementVersionNumberNumber(true, false, false);
    }
}
#endif