// Configures XR Plug-in Management to use OpenXR (instead of the deprecated Oculus XR Plugin)
// for Meta Quest (Android) and Quest Link (Windows editor). Run from the menu, or in batch mode:
//   Unity.exe -batchmode -quit -projectPath <path> -executeMethod QuestOpenXRSetup.Run
using System.Linq;
using UnityEditor;
using UnityEditor.XR.Management;
using UnityEditor.XR.Management.Metadata;
using UnityEngine;
using UnityEngine.XR.OpenXR;

public static class QuestOpenXRSetup
{
    const string OpenXRLoader = "UnityEngine.XR.OpenXR.OpenXRLoader";

    // OpenXR features enabled per platform, matched by type name.
    static readonly string[] AndroidFeatures =
    {
        "MetaQuestFeature",                    // Meta Quest support (Android)
        "MetaXRFeature",                       // Meta XR SDK (OVRPlugin) on OpenXR
        "OculusTouchControllerProfile",
        "MetaQuestTouchPlusControllerProfile", // Quest 3 / 3S controllers
    };
    static readonly string[] StandaloneFeatures =
    {
        "MetaXRFeature",
        "OculusTouchControllerProfile",
        "MetaQuestTouchPlusControllerProfile",
    };

    [MenuItem("Tools/Quest Setup/Use OpenXR for Quest + Link")]
    public static void Run()
    {
        Configure(BuildTargetGroup.Android, AndroidFeatures);
        Configure(BuildTargetGroup.Standalone, StandaloneFeatures);

        // Remove leftovers from the old Oculus XR Plugin.
        EditorBuildSettings.RemoveConfigObject("Unity.XR.Oculus.Settings");
        AssetDatabase.DeleteAsset("Assets/XR/Loaders/Oculus Loader.asset");
        AssetDatabase.DeleteAsset("Assets/XR/Settings/Oculus Settings.asset");

        AssetDatabase.SaveAssets();
        Debug.Log("[QuestSetup] Done.");
    }

    // Builds the enabled scenes to Builds/VRParkour.apk (batch: -executeMethod QuestOpenXRSetup.BuildApk).
    [MenuItem("Tools/Quest Setup/Build APK")]
    public static void BuildApk()
    {
        var scenes = EditorBuildSettings.scenes.Where(s => s.enabled).Select(s => s.path).ToArray();
        var report = BuildPipeline.BuildPlayer(scenes, "Builds/VRParkour.apk", BuildTarget.Android, BuildOptions.None);
        Debug.Log($"[QuestSetup] Build result: {report.summary.result}, size {report.summary.totalSize / (1024 * 1024)} MB, errors {report.summary.totalErrors}");
        if (Application.isBatchMode && report.summary.result != UnityEditor.Build.Reporting.BuildResult.Succeeded)
            EditorApplication.Exit(1);
    }

    static void Configure(BuildTargetGroup group, string[] featureNames)
    {
        var perTarget = XRGeneralSettingsPerBuildTarget.XRGeneralSettingsForBuildTarget(group);
        if (perTarget == null || perTarget.Manager == null)
        {
            Debug.LogError($"[QuestSetup] No XR settings for {group}. Open Project Settings > XR Plug-in Management once, then rerun.");
            return;
        }

        var manager = perTarget.Manager;

        // Drop loaders whose script no longer exists (the removed OculusLoader).
        var so = new SerializedObject(manager);
        var loaders = so.FindProperty("m_Loaders");
        for (int i = loaders.arraySize - 1; i >= 0; i--)
        {
            if (loaders.GetArrayElementAtIndex(i).objectReferenceValue == null)
                loaders.DeleteArrayElementAtIndex(i);
        }
        so.ApplyModifiedPropertiesWithoutUndo();

        if (!manager.activeLoaders.Any(l => l != null && l.GetType().FullName == OpenXRLoader))
        {
            bool ok = XRPackageMetadataStore.AssignLoader(manager, OpenXRLoader, group);
            Debug.Log($"[QuestSetup] {group}: assign OpenXR loader -> {ok}");
        }
        perTarget.InitManagerOnStart = true;
        EditorUtility.SetDirty(perTarget);
        EditorUtility.SetDirty(manager);

        // Make sure feature assets exist (they are normally created when the settings UI is first opened).
        UnityEditor.XR.OpenXR.Features.FeatureHelpers.RefreshFeatures(group);

        var openxr = OpenXRSettings.GetSettingsForBuildTargetGroup(group);
        if (openxr == null)
        {
            Debug.LogError($"[QuestSetup] No OpenXR settings for {group}.");
            return;
        }
        foreach (var feature in openxr.GetFeatures())
        {
            if (featureNames.Contains(feature.GetType().Name) && !feature.enabled)
            {
                feature.enabled = true;
                EditorUtility.SetDirty(feature);
                Debug.Log($"[QuestSetup] {group}: enabled {feature.GetType().Name}");
            }
        }
        EditorUtility.SetDirty(openxr);
    }
}
