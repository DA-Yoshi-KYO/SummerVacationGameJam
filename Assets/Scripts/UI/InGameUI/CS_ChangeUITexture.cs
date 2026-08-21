/* ＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
 *    UIのテクスチャを切り替える
 * ＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
 *    元浪梨緒
 * ----------------------------------------------------------
 * 2026-08-16 | 初回作成
 */
using UnityEngine;
using UnityEngine.UI;

public class CS_ChangeUITexture : MonoBehaviour
{
    [Header("非設定時のテクスチャ")][SerializeField] Sprite offTexture;
    [Header("設定時のテクスチャ")][SerializeField] Sprite onTexture;
    private Image currentImage;

    void Awake()
    {
        currentImage = GetComponent<Image>();
        currentImage.sprite = offTexture;
    }

    public void ChangeTexture(bool isSelect)
    {
        if (currentImage != null)
            currentImage.sprite = isSelect ? onTexture : offTexture;
    }
}
