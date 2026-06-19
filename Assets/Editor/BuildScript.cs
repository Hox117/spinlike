using UnityEditor;
using UnityEngine;
using System.Linq;
public class BuildScript
{
    [MenuItem("Build/Build Linux")]
    public static void BuildLinux()
    {
        string[] scenes = EditorBuildSettings.scenes
            .Where(s => s.enabled)
            .Select(s => s.path)
            .ToArray();

        BuildPlayerOptions options = new BuildPlayerOptions
        {
            scenes = scenes,
            locationPathName = "Builds/Linux/MiJuego",
            target = BuildTarget.StandaloneLinux64,
            options = BuildOptions.None
        };

        BuildPipeline.BuildPlayer(options);
    }
}