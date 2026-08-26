#if UNITY_EDITOR
using System;
using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace Bussigo.Editor
{
    public static class WebGLBuildScript
    {
        private static readonly string[] BuildScenes = new string[]
        {
            "Assets/Scenes/MainMenu.unity",
            "Assets/Scenes/VijayawadaHyderabadPlayableRoute.unity"
        };

        [MenuItem("BUSSIGO/WebGL/Build WebGL Local Playable")]
        public static void BuildWebGL()
        {
            string projectRoot = Path.GetFullPath(Path.Combine(Application.dataPath, ".."));
            string outputDirectory = Path.Combine(projectRoot, "Build", "WebGL");

            if (Directory.Exists(outputDirectory))
            {
                try
                {
                    Directory.Delete(outputDirectory, true);
                }
                catch (Exception ex)
                {
                    Debug.LogWarning($"[BUSSIGO WebGL] Warning cleaning output dir: {ex.Message}");
                }
            }
            Directory.CreateDirectory(outputDirectory);

            Debug.Log($"[BUSSIGO WebGL] Starting WebGL Local Playable Build to: {outputDirectory}");

            PlayerSettings.SetApplicationIdentifier(NamedBuildTarget.WebGL, "com.bussigo.southindiatravels");
            PlayerSettings.companyName = "BussigoStudios";
            PlayerSettings.productName = "BUSSIGO - South India Bus & Travel Empire Simulator";
            PlayerSettings.bundleVersion = "1.0.0";
            PlayerSettings.defaultScreenWidth = 1280;
            PlayerSettings.defaultScreenHeight = 720;
            
            // Set uncompressed so Python simple HTTP server works without Brotli/Gzip header issues
            PlayerSettings.WebGL.compressionFormat = WebGLCompressionFormat.Disabled;
            PlayerSettings.WebGL.decompressionFallback = false;
            PlayerSettings.WebGL.dataCaching = false;
            PlayerSettings.WebGL.exceptionSupport = WebGLExceptionSupport.FullWithStacktrace;

            BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions
            {
                scenes = BuildScenes,
                locationPathName = outputDirectory,
                target = BuildTarget.WebGL,
                options = BuildOptions.Development
            };

            BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
            BuildSummary summary = report.summary;

            if (summary.result == BuildResult.Succeeded)
            {
                Debug.Log($"[BUSSIGO WebGL] SUCCESS! WebGL Build generated ({summary.totalSize / (1024 * 1024)} MB) at: {outputDirectory}");
            }
            else if (summary.result == BuildResult.Failed)
            {
                Debug.LogError($"[BUSSIGO WebGL] FAILED with {summary.totalErrors} errors.");
                EditorApplication.Exit(1);
            }
        }
    }
}
#endif
