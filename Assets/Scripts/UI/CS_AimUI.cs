/* ＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
 *    照準UIクラス
 * ＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
 *    元浪梨緒
 * ----------------------------------------------------------
 * 2026-08-12 | 初回作成(仮で作ったため修正必要（アーモンドコアを基に作成）)
 */
using UnityEngine;
using UnityEngine.UI;

public class CS_AimUI : MonoBehaviour
{
    [Header("回転するリング")][SerializeField] private Image outRing;
    [Header("揺れるリング")][SerializeField] private Image inRing;
    [Header("ロックオン時の点滅")][SerializeField] private Image lockRing;

    [Header("外側リングの回転速度")][SerializeField] private float outRotateSpeed = 30f;
    [Header("内側リングの揺れ")][SerializeField] private float inPulseSpeed = 2f;
    [Header("揺れの強さ")][SerializeField] private float inPulseAmount = 0.05f;

    [Header("プレイヤーが使ってるカメラ")][SerializeField] private Camera playerCamera;
    [Header("プレイヤー本体")][SerializeField] private Transform player;
    [Header("照準の距離")][SerializeField] private float aimDistance;

    private bool isLocked = false;

    void Update()
    {
        Vector3 worldPoint = player.position + player.forward * aimDistance;
        Vector3 screenPos = playerCamera.WorldToScreenPoint(worldPoint);

        transform.position = screenPos;

        AimUI();
    }

    //UIの動き
    private void AimUI()
    {
        //外側リングを回転させる
        outRing.rectTransform.Rotate(0.0f, 0.0f, outRotateSpeed * Time.deltaTime);

        //内側リングを揺らす
        float scale = 1.0f + Mathf.Sin(Time.time * inPulseSpeed) * inPulseAmount;
        inRing.rectTransform.localScale = new Vector3(scale, scale, 1.0f);

        //ロックオン時の点滅
        if (isLocked)
        {
            float alpha = Mathf.Abs(Mathf.Sin(Time.time * 5.0f));
            lockRing.color = new Color(1.0f, 0.0f, 0.0f, alpha);
        }
        else
        {
            lockRing.color = new Color(1.0f, 0.0f, 0.0f, 0.0f);
        }
    }

    public void SetLocked(bool locked)
    {
        isLocked = locked;
    }

}
