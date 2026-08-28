/* ＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
 *    高度のUI
 * ＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
 *    元浪梨緒
 * ----------------------------------------------------------
 * 2026-08-17 | 初回作成
 */
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CS_AltUI : MonoBehaviour
{
    [Header("高度のテキスト")][SerializeField] private TextMeshProUGUI altText;

    [Header("初期高度")][SerializeField] private int initAlt;

    [Header("高度のメーター画像")][SerializeField] private RawImage altMetarImage;

    [Header("メーターのメモリの数")][SerializeField]private int meterCount;
    [Header("1メモリの数値")][SerializeField] private float meterValue;
    [Header("現在のメータ値")][SerializeField]private float currentMeter;//現在のメーター位置

    private int currentAlt;//現在の高度

    void Start()
    {
        altText.text = initAlt.ToString();

        currentAlt = initAlt;
    }

    void Update()
    {
        //メーターのUV座標を更新
        float uvStep = (1.0f / meterValue) / meterCount;
        float uvOffset = currentMeter * uvStep;

        Rect r = altMetarImage.uvRect;
        r.y = uvOffset;
        altMetarImage.uvRect = r;
    }

    //高度を増やす
    public void AddAlt(float amount)
    {
        currentMeter += amount;
        currentAlt = Mathf.FloorToInt(currentMeter);
        altText.text = currentAlt.ToString();
    }

    //高度を減らす
    public void SubtractAlt(float amount)
    {
        currentMeter -= amount;
        currentAlt = Mathf.FloorToInt(currentMeter);
        altText.text = currentAlt.ToString();
    }
}
