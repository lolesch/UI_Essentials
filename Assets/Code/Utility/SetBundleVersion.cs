#if UNITY_EDITOR
using TMPro;
using UI.Extensions;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.UI;

namespace BuildPipeline
{
    [RequireComponent(typeof(TextMeshProUGUI), typeof(ContentSizeFitter))]
    public class SetBundleVersion : MonoBehaviour, IPreprocessBuildWithReport
    {
        public int callbackOrder => 0;

        private TextMeshProUGUI versionText;

        [ContextMenu("UpdateText")]
        public void UpdateText() => OnPreprocessBuild(null);

        void OnValidate() => versionText = GetComponent<TextMeshProUGUI>();

        public void OnPreprocessBuild(BuildReport report)
        {
            if (versionText)
                versionText.text = $"Version: {IncreasePatchNumber()}";
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

            if (parts.Length > 0)
                majorVersion = int.Parse(parts[0]);
            if (parts.Length > 1)
                minorVersion = int.Parse(parts[1]);
            if (parts.Length > 2)
                patchVersion = parts[2];
            if (parts.Length > 3)
                releaseType = parts[3];

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

            PlayerSettings.bundleVersion = versionText;
            //Constants.SetVersionString(versionText);

            Debug.Log($"{"Version:".Colored(UIExtensions.Orange)}\tVersion incremented to " + versionText.Colored(UIExtensions.Orange));

            return versionText;
        }

        [MenuItem("Build/Increase Patch Number", false, 800)]
        private static string IncreasePatchNumber() => IncrementVersionNumberNumber(false, false, true);

        [MenuItem("Build/Increase Minor Version Number", false, 801)]
        private static string IncreaseMinorNumber() => IncrementVersionNumberNumber(false, true, false);

        [MenuItem("Build/Increase Major Version Number", false, 802)]
        private static string IncreaseMajorNumber() => IncrementVersionNumberNumber(true, false, false);
    }
}
#endif