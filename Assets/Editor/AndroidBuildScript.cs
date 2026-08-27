#if UNITY_EDITOR
using System;
using System.IO;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace Bussigo.Editor
{
    public static class AndroidBuildScript
    {
        private static readonly string[] BuildScenes = new string[]
        {
            "Assets/Scenes/MainMenu.unity",
            "Assets/Scenes/VijayawadaHyderabadPlayableRoute.unity"
        };

        [MenuItem("BUSSIGO/Android/Build Development APK")]
        public static void BuildAndroidDevelopmentAPK()
        {
            PerformAndroidBuild(isDevelopment: true);
        }

        [MenuItem("BUSSIGO/Android/Build Release APK")]
        public static void BuildAndroidReleaseAPK()
        {
            PerformAndroidBuild(isDevelopment: false);
        }

        public static void PerformAndroidBuild(bool isDevelopment = true)
        {
            string outputDirectory = Path.Combine(Application.dataPath, "..", "Build", "Android");
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            string apkFileName = isDevelopment ? "BUSSIGO_v1.0.0_Dev.apk" : "BUSSIGO_v1.0.0_Release.apk";
            string targetApkPath = Path.Combine(outputDirectory, apkFileName);

            Debug.Log($"[BUSSIGO Android Build] Starting {(isDevelopment ? "Development" : "Release")} APK build to: {targetApkPath}");

            // Configure Player Settings for Android
            PlayerSettings.SetApplicationIdentifier(UnityEditor.Build.NamedBuildTarget.Android, "com.bussigo.southindiatravels");
            PlayerSettings.bundleVersion = "1.0.0";
            PlayerSettings.Android.bundleVersionCode = 1;
            PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel26;
            PlayerSettings.Android.targetSdkVersion = AndroidSdkVersions.AndroidApiLevel34;
            PlayerSettings.defaultInterfaceOrientation = UIOrientation.LandscapeLeft;
            PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARMv7 | AndroidArchitecture.ARM64;

            BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions
            {
                scenes = BuildScenes,
                locationPathName = targetApkPath,
                target = BuildTarget.Android,
                options = isDevelopment ? (BuildOptions.Development | BuildOptions.AllowDebugging) : BuildOptions.None
            };

            BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
            BuildSummary summary = report.summary;

            if (summary.result == BuildResult.Succeeded)
            {
                Debug.Log($"[BUSSIGO Android Build] SUCCESS! Output size: {summary.totalSize / (1024 * 1024)} MB at: {targetApkPath}");
            }
            else if (summary.result == BuildResult.Failed)
            {
                Debug.LogError($"[BUSSIGO Android Build] FAILED with {summary.totalErrors} errors.");
            }
        }
    }
}
#endif
