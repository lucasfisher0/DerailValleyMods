using System.Collections;
using System.IO;
using UnityEngine;
using DetailedBookletIcons;

public class AssetBundleTester : MonoBehaviour
{
    // Name of bundle file (no extension)
    public string bundleName = "detailedbookleticons";
    // The catalog ScriptableObject type
    public string catalogAssetName = "AssetCatalog";

    public static readonly Color HAUL_JOB_TYPE_COLOR = new Color(0.4117f, 0.847f, 0.4117f);
    public static readonly Color EMPTY_HAUL_JOB_TYPE_COLOR = new Color(0.847f, 0.76f, 0.4117f);
    public static readonly Color SHUNTING_LOAD_JOB_TYPE_COLOR = new Color(0.847f, 0.4117f, 0.4117f);
    public static readonly Color IMW_COLOR = new Color(0.6039216f, 0.3490196f, 0.2784314f);
    public static readonly Color SM_COLOR = new Color(0.482f, 0.514f, 0.58f);
    public static readonly Color HB_COLOR = new Color(0.506f, 0.424f, 0.58f);
    public static readonly Color MF_COLOR = new Color(0.863f, 0.533f, 0.353f);
    public static readonly Color GRAY_COLOR = new Color(0.9f, 0.9f, 0.9f);

    IEnumerator Start()
    {
        string bundlePath = Path.Combine(Application.streamingAssetsPath, bundleName);
        var bundle = AssetBundle.LoadFromFile(bundlePath);
        if (bundle == null)
        {
            Debug.LogError($"Failed to load AssetBundle at {bundlePath}");
            yield break;
        }

        foreach (var n in bundle.GetAllAssetNames())
            Debug.Log("Bundle contains: " + n);

        var catalog = bundle.LoadAsset<AssetCatalog>(catalogAssetName);
        if (catalog == null)
        {
            Debug.LogError("Failed to load AssetCatalog from bundle");
            yield break;
        }

        // Instantiate whatever root prefab you have
        var root = Instantiate(catalog.bookletIconRendererPrefab);
        root.name = "TEST_Instance";

        // assume `root` is the GameObject you instantiated:
        var renderer = root.GetComponent<FillInBookletIcon>();

        // prepare a jagged array: 3 sets of RowProperties (one per icon), each with 3 entries
        FillInBookletIcon.RowProperties[][] rows = new FillInBookletIcon.RowProperties[][]
        {
            new[]
            {
                new FillInBookletIcon.RowProperties { style = FillInBookletIcon.RowStyle.Solid,       bgcolor = HAUL_JOB_TYPE_COLOR,       color = "black", text = "FH-69" },
                new FillInBookletIcon.RowProperties { style = FillInBookletIcon.RowStyle.Start,       bgcolor = IMW_COLOR,                  color = "white", text = "IMW"   },
                new FillInBookletIcon.RowProperties { style = FillInBookletIcon.RowStyle.End,         bgcolor = SM_COLOR,                   color = "white", text = "SM"    }
            },
            new[]
            {
                new FillInBookletIcon.RowProperties { style = FillInBookletIcon.RowStyle.Solid,       bgcolor = EMPTY_HAUL_JOB_TYPE_COLOR,  color = "black", text = "LH-12" },
                new FillInBookletIcon.RowProperties { style = FillInBookletIcon.RowStyle.Start,       bgcolor = HB_COLOR,                   color = "white", text = "HB"    },
                new FillInBookletIcon.RowProperties { style = FillInBookletIcon.RowStyle.End,         bgcolor = MF_COLOR,                   color = "white", text = "MF"    }
            },
            new[]
            {
                new FillInBookletIcon.RowProperties { style = FillInBookletIcon.RowStyle.End,         bgcolor = IMW_COLOR,                  color = "white", text = "IMW"   },
                new FillInBookletIcon.RowProperties { style = FillInBookletIcon.RowStyle.Solid,       bgcolor = GRAY_COLOR,                 color = "black", text = "165t"  },
                new FillInBookletIcon.RowProperties { style = FillInBookletIcon.RowStyle.Solid,       bgcolor = GRAY_COLOR,                 color = "black", text = "C3I"   }
            },
            new[]
            {
                new FillInBookletIcon.RowProperties { style = FillInBookletIcon.RowStyle.Solid,       bgcolor = SHUNTING_LOAD_JOB_TYPE_COLOR, color = "black", text = "SL-17"   },
                new FillInBookletIcon.RowProperties { style = FillInBookletIcon.RowStyle.End,         bgcolor = HB_COLOR,                   color = "white", text = "HB"    },
                new FillInBookletIcon.RowProperties { style = FillInBookletIcon.RowStyle.Solid,       bgcolor = GRAY_COLOR,                 color = "black", text = "B4S"   }
            }
        };

        for (int i = 0; i < rows.Length; i++)
        {
            float stripeWidth = (i == rows.Length - 1) ? 0.4f : 0f;
            Sprite icon = null;
            renderer.RenderPaper(Color.white, new Color(0.7f, 0.7f, 0.7f), stripeWidth, rows[i], ref icon);

            var go = new GameObject($"icon_{i}");
            go.transform.position = new Vector3(1.8f*(float)i + 2, 0, 0);
            var sr = go.AddComponent<SpriteRenderer>();
            sr.sprite = icon;
        }

        {
            var go = new GameObject($"icon_chost");
            go.transform.position = new Vector3(1.8f + 2, 1.8f, 0);
            var sr = go.AddComponent<SpriteRenderer>();
            sr.sprite = catalog.bookletIconGhostSprite;
        }
        root.SetActive(false);
                
        bundle.Unload(false);
    }
}
