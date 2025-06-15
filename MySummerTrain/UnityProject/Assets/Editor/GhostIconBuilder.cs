// Assets/Editor/GhostIconBuilder.cs
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using TMPro;

public static class GhostIconBuilder
{
    const int ICON_SIZE = 128;
    const string PREFAB_PATH = "Assets/Prefabs/BookletIconRenderer.prefab";
    const string OUTPUT_PATH = "Assets/Texture2D/GhostIcon.asset";

    [MenuItem("DetailedBookletIcons/Build Ghost Icon")]
    public static void BuildGhostIcon()
    {
        // 1) Load the prefab
        var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(PREFAB_PATH);
        if (prefab == null)
        {
            Debug.LogError($"Prefab not found at {PREFAB_PATH}");
            return;
        }

        // 2) Create a preview scene and instantiate
        var previewScene = EditorSceneManager.NewScene(
            NewSceneSetup.EmptyScene,
            NewSceneMode.Single   // unloads any open scene and makes this one active
        );
        var instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab, previewScene);

        try
        {
            // 3) Find the paper object
            var paperTf = instance.transform.Find("Paper");
            if (paperTf == null)
                throw new System.Exception("Could not find child named 'Paper'");
            var paperGO = paperTf.gameObject;

            // 4) Duplicate for the back silhouette
            var backGO = Object.Instantiate(paperGO, paperTf.parent);
            backGO.name = "paper_back";
            backGO.transform.localPosition += new Vector3(0f, -0.001f, 0f);
            backGO.transform.localScale *= 1.12f;

            // 5) Disable all TextMeshPro on the original
            foreach (var tmp in instance.GetComponentsInChildren<TextMeshPro>())
                tmp.gameObject.SetActive(false);

            // 6) Create two simple Unlit/Color mats
            var borderMat = new Material(Shader.Find("Custom/GhostShader"))
            { color = new Color(0.518f, 0.827f, .937f, 0.6f) };
            var fillMat   = new Material(Shader.Find("Custom/GhostShader"))
            { color = new Color(0.518f, 0.827f, .937f, 0.098f) };

            // Assign them
            backGO.GetComponent<MeshRenderer>().sharedMaterial = borderMat;
            paperGO.GetComponent<MeshRenderer>().sharedMaterial   = fillMat;

            // 7) Grab the camera
            var cam = instance.GetComponentInChildren<Camera>();
            if (cam == null)
                throw new System.Exception("No Camera found under the prefab");

            // Helper to render one pass into a Texture2D
            Texture2D RenderPass(System.Action prep)
            {
                // prep: enable/disable objects & assign targetTexture
                prep();

                var rt = new RenderTexture(ICON_SIZE, ICON_SIZE, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.sRGB)
                { antiAliasing = 1, useMipMap = false, autoGenerateMips = false };
                rt.Create();

                cam.targetTexture = rt;
                cam.Render();

                RenderTexture.active = rt;
                var tex = new Texture2D(ICON_SIZE, ICON_SIZE, TextureFormat.ARGB32, mipChain: false, linear: false);
                tex.ReadPixels(new Rect(0, 0, ICON_SIZE, ICON_SIZE), 0, 0);
                tex.Apply();

                cam.targetTexture = null;
                RenderTexture.active = null;
                Object.DestroyImmediate(rt);

                return tex;
            }

            // 8a) Back-only pass
            var texBack = RenderPass(() => {
                paperGO.SetActive(false);
                backGO.SetActive(true);
            });

            // 8b) Front-only pass
            var texFront = RenderPass(() => {
                backGO.SetActive(false);
                paperGO.SetActive(true);
            });

            // 9) Composite border + fill
            var backPixels  = texBack.GetPixels();
            var frontPixels = texFront.GetPixels();
            var finalPixels = new Color[backPixels.Length];

            for (int i = 0; i < backPixels.Length; i++)
            {
                if (frontPixels[i].a > 0f)
                {
                    // fill area
                    finalPixels[i] = frontPixels[i];
                }
                else if (backPixels[i].a > 0f)
                {
                    // border area
                    finalPixels[i] = backPixels[i];
                }
                else
                {
                    finalPixels[i] = Color.clear;
                }
            }

            var finalTex = new Texture2D(ICON_SIZE, ICON_SIZE, TextureFormat.ARGB32, mipChain: true, linear: false);
            finalTex.wrapMode = TextureWrapMode.Clamp;
            finalTex.SetPixels(finalPixels);
            finalTex.Apply();

            // Save as asset
            if (AssetDatabase.LoadAssetAtPath<Texture2D>(OUTPUT_PATH) != null)
            {
                AssetDatabase.DeleteAsset(OUTPUT_PATH);
            }

            AssetDatabase.CreateAsset(finalTex, OUTPUT_PATH);

            var sprite = Sprite.Create(
                finalTex,
                new Rect(0, 0, finalTex.width, finalTex.height),
                new Vector2(0.5f, 0.5f),
                100
            );
            AssetDatabase.AddObjectToAsset(sprite, OUTPUT_PATH);

            {  
                var guids = AssetDatabase.FindAssets("t:DetailedBookletIcons.AssetCatalog");
                if (guids.Length == 0)
                {
                    Debug.LogError("No AssetCatalog asset found in the project.");
                    return;
                }
                var catalogPath = AssetDatabase.GUIDToAssetPath(guids[0]);

                var catalog = AssetDatabase.LoadAssetAtPath<DetailedBookletIcons.AssetCatalog>(catalogPath);
                if (catalog == null)
                {
                    Debug.LogError($"Asset at {catalogPath} is not an AssetCatalog.");
                    return;
                }
                catalog.bookletIconGhostSprite = sprite;
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log($"Ghost icon written to {OUTPUT_PATH}");
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error building ghost icon: " + ex);
        }
        finally
        {
            // Cleanup
            EditorSceneManager.CloseScene(previewScene, true);
        }
    }
}
