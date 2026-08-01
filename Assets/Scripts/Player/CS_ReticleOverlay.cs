using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// スコープのレティクル(赤ドット+十字線)を画面中央にオーバーレイ表示する。
/// シーン側にCanvasを用意しなくても済むよう、実行時に自前でUIを構築する。
/// ズーム倍率やレンズ形状に依存しないので、Shader Graphを触らずに済む。
/// </summary>
public class CS_ReticleOverlay : MonoBehaviour
{
    [Header("見た目")]
    public Color reticleColor = Color.red;
    public float dotSize = 6f;
    public float lineLength = 40f;
    public float lineThickness = 2f;
    public float gapFromCenter = 14f;

    private GameObject root;

    void Awake()
    {
        BuildReticle();
        SetVisible(false);
    }

    public void SetVisible(bool visible)
    {
        if (root != null)
            root.SetActive(visible);
    }

    private void BuildReticle()
    {
        var canvasGO = new GameObject("ReticleCanvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
        canvasGO.transform.SetParent(transform, false);

        var canvas = canvasGO.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 100;

        var scaler = canvasGO.GetComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);

        root = new GameObject("Reticle", typeof(RectTransform));
        root.transform.SetParent(canvasGO.transform, false);

        float halfSpan = gapFromCenter + lineLength * 0.5f;
        CreateBar("Dot", Vector2.zero, new Vector2(dotSize, dotSize));
        CreateBar("Top", new Vector2(0, halfSpan), new Vector2(lineThickness, lineLength));
        CreateBar("Bottom", new Vector2(0, -halfSpan), new Vector2(lineThickness, lineLength));
        CreateBar("Left", new Vector2(-halfSpan, 0), new Vector2(lineLength, lineThickness));
        CreateBar("Right", new Vector2(halfSpan, 0), new Vector2(lineLength, lineThickness));
    }

    private void CreateBar(string name, Vector2 anchoredPos, Vector2 size)
    {
        var go = new GameObject(name, typeof(Image));
        go.transform.SetParent(root.transform, false);

        var img = go.GetComponent<Image>();
        img.color = reticleColor;
        img.raycastTarget = false;

        var rt = img.rectTransform;
        rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.pivot = new Vector2(0.5f, 0.5f);
        rt.anchoredPosition = anchoredPos;
        rt.sizeDelta = size;
    }
}
