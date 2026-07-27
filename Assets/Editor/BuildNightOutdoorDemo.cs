#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEditor.Rendering.HighDefinition;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

// Adds a simple rolling-hill Terrain, a few demo props and night lighting
// to the currently open scene, to make HDRP's night-time features (physically
// based sky/moon, volumetric fog, bloom, SSR, SSAO) easy to see.
public static class BuildNightOutdoorDemo
{
    const string RootName = "Night Demo";
    const string MaterialFolder = "Assets/Materials";
    const string TerrainFolder = "Assets/Terrain";

    [MenuItem("Tools/HDRP Night Demo/Build Outdoor Night Scene")]
    static void Build()
    {
        if (GameObject.Find(RootName) != null)
        {
            EditorUtility.DisplayDialog("HDRP Night Demo",
                $"A '{RootName}' object already exists in this scene. Delete it first if you want to rebuild.", "OK");
            return;
        }

        if (!AssetDatabase.IsValidFolder(MaterialFolder))
            AssetDatabase.CreateFolder("Assets", "Materials");
        if (!AssetDatabase.IsValidFolder(TerrainFolder))
            AssetDatabase.CreateFolder("Assets", "Terrain");

        var root = new GameObject(RootName);
        Undo.RegisterCreatedObjectUndo(root, "Build Night Outdoor Demo");

        var terrain = BuildTerrain(root.transform);
        BuildProps(terrain, root.transform);
        BuildLamps(terrain, root.transform);
        ConvertSunToMoonlight();
        EnhanceVolumeProfile();

        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
        Selection.activeGameObject = root;
        Debug.Log("[HDRP Night Demo] Terrain, props and lamps added; Sun converted to moonlight; volume profile enhanced (Bloom/SSAO/SSR).");
    }

    // ---------- Terrain ----------

    static Terrain BuildTerrain(Transform parent)
    {
        const int res = 65; // must be 2^n + 1
        var size = new Vector3(60f, 4f, 60f);

        var data = new TerrainData();
        data.heightmapResolution = res;
        data.size = size;

        var heights = new float[res, res];
        for (int y = 0; y < res; y++)
        {
            for (int x = 0; x < res; x++)
            {
                float nx = (float)x / res;
                float ny = (float)y / res;
                heights[y, x] = Mathf.PerlinNoise(nx * 3f, ny * 3f) * 0.12f
                              + Mathf.PerlinNoise(nx * 8f + 50f, ny * 8f + 50f) * 0.03f;
            }
        }
        data.SetHeights(0, 0, heights);

        var grassLayer = CreateFlatColorTerrainLayer("NightDemo_Grass", new Color(0.09f, 0.15f, 0.08f));
        data.terrainLayers = new[] { grassLayer };

        var dataPath = AssetDatabase.GenerateUniqueAssetPath($"{TerrainFolder}/NightDemoTerrainData.asset");
        AssetDatabase.CreateAsset(data, dataPath);

        var terrainGo = Terrain.CreateTerrainGameObject(data);
        terrainGo.name = "Ground Terrain";
        Undo.RegisterCreatedObjectUndo(terrainGo, "Build Night Outdoor Demo");
        terrainGo.transform.SetParent(parent, false);
        terrainGo.transform.localPosition = new Vector3(-size.x * 0.5f, 0f, -size.z * 0.5f);

        return terrainGo.GetComponent<Terrain>();
    }

    static TerrainLayer CreateFlatColorTerrainLayer(string name, Color color)
    {
        var tex = new Texture2D(4, 4, TextureFormat.RGBA32, false);
        var pixels = new Color[16];
        for (int i = 0; i < pixels.Length; i++) pixels[i] = color;
        tex.SetPixels(pixels);
        tex.Apply();

        var texPath = AssetDatabase.GenerateUniqueAssetPath($"{TerrainFolder}/{name}_Albedo.png");
        File.WriteAllBytes(texPath, tex.EncodeToPNG());
        Object.DestroyImmediate(tex);
        AssetDatabase.ImportAsset(texPath);

        var importedTex = AssetDatabase.LoadAssetAtPath<Texture2D>(texPath);

        var layer = new TerrainLayer();
        layer.diffuseTexture = importedTex;
        layer.tileSize = new Vector2(25f, 25f);

        var layerPath = AssetDatabase.GenerateUniqueAssetPath($"{TerrainFolder}/{name}.terrainlayer");
        AssetDatabase.CreateAsset(layer, layerPath);
        return layer;
    }

    // ---------- Materials & Props ----------

    static Material MakeLitMaterial(string name, Color baseColor, float metallic, float smoothness, Color? emissive = null)
    {
        var mat = new Material(Shader.Find("HDRP/Lit")) { name = name };
        mat.SetColor("_BaseColor", baseColor);
        mat.SetFloat("_Metallic", metallic);
        mat.SetFloat("_Smoothness", smoothness);
        if (emissive.HasValue)
            mat.SetColor("_EmissiveColor", emissive.Value);

        HDShaderUtils.ResetMaterialKeywords(mat);

        var path = AssetDatabase.GenerateUniqueAssetPath($"{MaterialFolder}/{name}.mat");
        AssetDatabase.CreateAsset(mat, path);
        return mat;
    }

    static GameObject MakePrimitive(PrimitiveType type, string name, Vector3 worldPos, Vector3 localScale, Material mat, Transform parent, Quaternion? rot = null)
    {
        var go = GameObject.CreatePrimitive(type);
        go.name = name;
        Undo.RegisterCreatedObjectUndo(go, "Build Night Outdoor Demo");
        go.transform.SetParent(parent, true);
        go.transform.position = worldPos;
        go.transform.rotation = rot ?? Quaternion.identity;
        go.transform.localScale = localScale;
        go.GetComponent<Renderer>().sharedMaterial = mat;
        return go;
    }

    static float GroundY(Terrain terrain, float worldX, float worldZ) =>
        terrain.SampleHeight(new Vector3(worldX, 0f, worldZ));

    static void BuildProps(Terrain terrain, Transform parent)
    {
        var mirror = MakeLitMaterial("M_Mirror_Sphere", Color.white, 1f, 0.96f);
        MakePrimitive(PrimitiveType.Sphere, "Mirror Sphere",
            new Vector3(-3.5f, GroundY(terrain, -3.5f, 4f) + 1f, 4f), Vector3.one * 2f, mirror, parent);

        var glow = MakeLitMaterial("M_Glow_Crystal", Color.black, 0f, 0.6f, new Color(0.3f, 3.5f, 4f));
        MakePrimitive(PrimitiveType.Cube, "Glow Crystal",
            new Vector3(3f, GroundY(terrain, 3f, 3f) + 0.75f, 3f), new Vector3(1f, 1.5f, 1f), glow, parent,
            Quaternion.Euler(18f, 32f, 12f));

        var stone = MakeLitMaterial("M_Stone", new Color(0.3f, 0.29f, 0.27f), 0f, 0.22f);
        MakePrimitive(PrimitiveType.Cylinder, "Stone Pillar A",
            new Vector3(0f, GroundY(terrain, 0f, 6.5f) + 1.5f, 6.5f), new Vector3(0.8f, 1.5f, 0.8f), stone, parent);
        MakePrimitive(PrimitiveType.Cylinder, "Stone Pillar B",
            new Vector3(2.2f, GroundY(terrain, 2.2f, 8f) + 1.1f, 8f), new Vector3(0.55f, 1.1f, 0.55f), stone, parent);
    }

    static void BuildLamps(Terrain terrain, Transform parent)
    {
        var poleMat = MakeLitMaterial("M_Lamp_Pole", new Color(0.05f, 0.05f, 0.05f), 0.6f, 0.4f);
        CreateLampPost("Lamp Post A", new Vector3(-2f, 0f, 1.5f), terrain, poleMat, parent);
        CreateLampPost("Lamp Post B", new Vector3(2.5f, 0f, -1f), terrain, poleMat, parent);
    }

    static void CreateLampPost(string name, Vector3 basePosXZ, Terrain terrain, Material poleMat, Transform parent)
    {
        float groundY = GroundY(terrain, basePosXZ.x, basePosXZ.z);

        var post = new GameObject(name);
        Undo.RegisterCreatedObjectUndo(post, "Build Night Outdoor Demo");
        post.transform.SetParent(parent, true);
        post.transform.position = new Vector3(basePosXZ.x, groundY, basePosXZ.z);

        MakePrimitive(PrimitiveType.Cylinder, "Pole",
            post.transform.position + new Vector3(0f, 1.25f, 0f), new Vector3(0.08f, 1.25f, 0.08f), poleMat, post.transform);

        var lampGo = new GameObject("Light");
        Undo.RegisterCreatedObjectUndo(lampGo, "Build Night Outdoor Demo");
        lampGo.transform.SetParent(post.transform, true);
        lampGo.transform.position = post.transform.position + new Vector3(0f, 2.5f, 0f);

        var light = lampGo.AddComponent<Light>();
        light.type = LightType.Point;
        light.color = new Color(1f, 0.76f, 0.48f);
        light.range = 9f;
        light.shadows = LightShadows.Soft;

        var hd = lampGo.AddComponent<HDAdditionalLightData>();
        HDAdditionalLightData.InitDefaultHDAdditionalLightData(hd);
        light.lightUnit = LightUnit.Lumen;
        light.intensity = 6000f;
        light.useColorTemperature = false;
    }

    // ---------- Lighting & Sky ----------

    static void ConvertSunToMoonlight()
    {
        var sun = GameObject.Find("Sun");
        if (sun == null)
        {
            Debug.LogWarning("[HDRP Night Demo] Could not find a 'Sun' GameObject to convert to moonlight.");
            return;
        }

        Undo.RecordObject(sun.transform, "Convert Sun To Moonlight");
        sun.transform.eulerAngles = new Vector3(150f, -30f, 0f);

        var light = sun.GetComponent<Light>();
        if (light != null)
        {
            Undo.RecordObject(light, "Convert Sun To Moonlight");
            light.lightUnit = LightUnit.Lux;
            light.intensity = 0.5f; // ~max full-moon brightness
            light.useColorTemperature = true;
            light.colorTemperature = 4100f;
        }

        sun.name = "Moon";
    }

    static void EnhanceVolumeProfile()
    {
        var volumeGo = GameObject.Find("Sky and Fog Volume");
        var volume = volumeGo != null ? volumeGo.GetComponent<Volume>() : null;
        if (volume == null || volume.sharedProfile == null)
        {
            Debug.LogWarning("[HDRP Night Demo] Could not find the scene's global Volume to enhance.");
            return;
        }

        var profile = volume.sharedProfile;

        if (!profile.TryGet<Bloom>(out var bloom))
            bloom = profile.Add<Bloom>(true);
        bloom.active = true;
        bloom.intensity.Override(0.35f);
        bloom.tint.Override(new Color(0.85f, 0.9f, 1f));

        if (!profile.TryGet<ScreenSpaceAmbientOcclusion>(out var ao))
            ao = profile.Add<ScreenSpaceAmbientOcclusion>(true);
        ao.active = true;
        ao.intensity.Override(1f);
        ao.radius.Override(1.2f);

        if (!profile.TryGet<ScreenSpaceReflection>(out var ssr))
            ssr = profile.Add<ScreenSpaceReflection>(true);
        ssr.active = true;
        ssr.enabled.Override(true);

        EditorUtility.SetDirty(profile);
    }
}
#endif
