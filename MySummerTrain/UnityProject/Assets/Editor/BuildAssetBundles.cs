using UnityEngine;
using UnityEditor;

public static class MyModBundleBuilder
{
    [MenuItem("DetailedBookletIcons/Build AssetBundles")]
    public static void BuildAllBundles()
    {
        BuildPipeline.BuildAssetBundles(
            "Assets/AssetBundles",
            BuildAssetBundleOptions.None,
            EditorUserBuildSettings.activeBuildTarget);
    }
}
