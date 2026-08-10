using UnityEngine;

/// <summary>
/// タルコフ風スコープの「覗き込み」レンダリングを制御するコンポーネント。
/// スコープの接眼レンズ位置に置いた専用カメラをRenderTextureへ出力し、
/// レンズ面(Quad等)のマテリアルにそのテクスチャを反映する。
/// </summary>
[RequireComponent(typeof(Camera))]
public class CS_ScopeRig : MonoBehaviour
{
    [Header("参照")]
    [Tooltip("スコープのレンズ面(Quad/Cylinder)に付いているMeshRenderer")]
    public MeshRenderer lensRenderer;
    [Tooltip("プレイヤーのメインカメラ(FOV同期用)")]
    public Camera mainCamera;

    [Header("ズーム設定")]
    [Tooltip("スコープの倍率一覧(例: 4倍, 8倍)")]
    public float[] zoomLevels = { 4f, 8f };
    private int zoomIndex = 0;

    [Tooltip("裸眼相当のFOV。ここから倍率で割って算出する")]
    public float baseFov = 60f;

    [Header("レンダーテクスチャ")]
    [Tooltip("実行時に生成する解像度(正方形推奨)")]
    public int renderTextureSize = 1024;
    [Tooltip("負荷が気になる場合はここでMSAAを制御")]
    public int msaaSamples = 1;

    private Camera scopeCamera;
    private RenderTexture rt;
    private static readonly int mainTexId = Shader.PropertyToID("_ScopeTex");

    void Awake()
    {
        scopeCamera = GetComponent<Camera>();
        // ---- RenderTextureを動的生成 ----
        rt = new RenderTexture(renderTextureSize, renderTextureSize, 16, RenderTextureFormat.DefaultHDR)
        {
            antiAliasing = Mathf.Max(1, msaaSamples),
            filterMode = FilterMode.Bilinear,
            useMipMap = false,
            name = "ScopeRT_" + gameObject.name
        };
        rt.Create();

        scopeCamera.targetTexture = rt;
        scopeCamera.enabled = false; // 手動でRenderさせる(常時Updateで回さない=負荷軽減)

        if (lensRenderer != null)
        {
            // マテリアルインスタンスを作ってRTを割り当て
            lensRenderer.material.SetTexture(mainTexId, rt);
        }

        ApplyZoom();
    }

    void LateUpdate()
    {
        // 撮影内容がMain Cameraの向き(ピッチ含む)と一致するよう、毎フレーム位置・向きを合わせる
        if (mainCamera != null)
        {
            transform.SetPositionAndRotation(mainCamera.transform.position, mainCamera.transform.rotation);
        }

        // Camera.onPreCullはHDRP等のSRPでは発火しないため、直接毎フレーム描画する
        // (このコンポーネント自体がADS中だけenabledになるので、負荷は自然に抑えられる)
        if (scopeCamera != null)
        {
            scopeCamera.Render();
        }
    }

    /// <summary>ズーム段階を切り替える(マウスホイール等から呼ぶ)</summary>
    public void CycleZoom(int direction)
    {
        if (zoomLevels.Length == 0) return;
        zoomIndex = (zoomIndex + direction + zoomLevels.Length) % zoomLevels.Length;
        ApplyZoom();
    }

    private void ApplyZoom()
    {
        float mag = zoomLevels[zoomIndex];
        scopeCamera.fieldOfView = baseFov / mag;
    }

    /// <summary>現在の倍率。マウス感度をズームに応じて下げる際に使う。</summary>
    public float CurrentMagnification => zoomLevels.Length > 0 ? zoomLevels[zoomIndex] : 1f;

    /// <summary>
    /// スコープの位置を武器のマウント位置に追従させる。
    /// 別途、視差(パララックス)が欲しい場合はここで
    /// 視点オフセットに応じてレティクルオブジェクトの位置もズラす。
    /// </summary>
    public void SyncTransform(Transform mount)
    {
        transform.position = mount.position;
        transform.rotation = mount.rotation;
    }

    void OnDestroy()
    {
        if (rt != null)
        {
            scopeCamera.targetTexture = null;
            rt.Release();
        }
    }
}
