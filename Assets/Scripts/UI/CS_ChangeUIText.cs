/* ＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
 *    Textのカラーを切り替える
 * ＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
 *    元浪梨緒
 * ----------------------------------------------------------
 * 2026-08-23 | 初回作成
 */
using TMPro;
using UnityEngine;

public class CS_ChangeUIText : MonoBehaviour
{
    [Header("非設定時のテクスチャ")][SerializeField] private Color offColor;
    [Header("設定時のテクスチャ")][SerializeField] private Color onColor;

    private TextMeshProUGUI currentText;

    void Awake()
    {
        currentText = GetComponent<TextMeshProUGUI>();
        currentText.color = onColor;
    }

    public void ChangeTexture(bool isSelect)
    {
        if (currentText != null)
            currentText.color = isSelect ? offColor : onColor;
    }

    //選択状態を維持するため、ドラッグ開始時に呼ぶ
    public void SetDragTexture()
    {
        if (currentText != null)
            currentText.color = offColor;
    }

    //元に戻すため、ドラッグ終了時に呼ぶ
    public void ResetDragTexture()
    {
        if (currentText != null)
            currentText.color = onColor;
    }
}
