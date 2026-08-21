/* ＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
 *    照準UIクラス
 * ＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
 *    元浪梨緒
 * ----------------------------------------------------------
 * 2026-08-12 | 初回作成(仮で作ったため修正必要（アーマードコアを基に作成）)
 * 2026-08-15 | UIアニメーションの変更
 */
using UnityEngine;

public class CS_AimUI : MonoBehaviour
{
    [Header("InRing")][SerializeField] private RectTransform inRing;
    [Header("InRingの拡大の速度")][SerializeField] private float inExpandSpeed;
    [Header("InRingの縮小の速度")][SerializeField] private float inShrinkSpeed;
    [Header("InRingの最大スケール")][SerializeField] private float inMaxScale;
    [Header("InRingの最小スケール")][SerializeField] private float inMinScale;
    [Header("InRingの拡大時の回転角度")][SerializeField] private float inRotateExpand;
    [Header("InRingの縮小時の回転角度")][SerializeField] private float inRotateShrink;

    [Header("OutRing")][SerializeField] private RectTransform outRing;
    [Header("OutRingの拡大の速度")][SerializeField] private float outExpandSpeed;
    [Header("OutRingの縮小の速度")][SerializeField] private float outShrinkSpeed;
    [Header("OutRingの最大スケール")][SerializeField] private float outMaxScale;
    [Header("OutRingの最小スケール")][SerializeField] private float outMinScale;
    [Header("OutRingの拡大時の回転角度")][SerializeField] private float outRotateExpand;
    [Header("OutRingの縮小時の回転角度")][SerializeField] private float outRotateShrink;

    [Header("待機時間")][SerializeField] private float waitTime;

    //リングの状態
    private enum RingState
    { 
        Expand,//拡大中
        WaitAfterExpand,//拡大後の待機中
        Shrink,//縮小中
        WaitAfterShrink,//縮小後の待機中
    }

    private RingState currentRingState = RingState.Expand;//現在のリングの状態

    private float time = 0.0f;

    private bool isLocked = false;//ロックオンしてるかどうか

    void Update()
    {
        AimUI();
    }

    //UIの動き
    private void AimUI()
    {
        switch (currentRingState)
        {
            case RingState.Expand:
                //inRingの拡大処理
                UpdateRingExpand(inRing, inMaxScale, inExpandSpeed, inRotateExpand);
                //outRingの拡大処理
                UpdateRingExpand(outRing, outMaxScale, outExpandSpeed, outRotateExpand);

                //リングが最大スケールに到達したら、次の状態に遷移する
                if (CheckReached(inRing.localScale.x, inMaxScale) && CheckReached(outRing.localScale.x, outMaxScale))
                {
                    currentRingState = RingState.WaitAfterExpand;
                    time = 0.0f;
                }
                break;

            case RingState.WaitAfterExpand:
                time += Time.deltaTime;

                //待機時間が経過したら、次の状態に遷移する
                if (time >= waitTime)
                {
                    currentRingState = RingState.Shrink;
                }
                break;

            case RingState.Shrink:
                //inRingの縮小処理
                UpdateRingShrink(inRing, inMinScale, inShrinkSpeed, inRotateShrink);
                //outRingの縮小処理
                UpdateRingShrink(outRing, outMinScale, outShrinkSpeed, outRotateShrink);

                //リングが最小スケールに到達したら、次の状態に遷移する
                if (CheckReached(inRing.localScale.x, inMinScale) && CheckReached(outRing.localScale.x, outMinScale))
                {
                    currentRingState = RingState.WaitAfterShrink;
                    time = 0.0f;
                }
                break;

            case RingState.WaitAfterShrink:
                time += Time.deltaTime;

                //待機時間が経過したら、次の状態に遷移する
                if (time >= waitTime)
                {
                    currentRingState = RingState.Expand;
                }
                break;
        }
    }

    //リングの拡大処理
    private void UpdateRingExpand(RectTransform ring, float maxScale, float speed, float rotate)
    {
        ring.Rotate(0.0f, 0.0f, rotate * Time.deltaTime);
        float scale = Mathf.Lerp(ring.localScale.x, maxScale, Time.deltaTime * speed);
        ring.localScale = new Vector3(scale, scale, 1.0f);
    }

    //リングを縮小する関数
    private void UpdateRingShrink(RectTransform ring, float minScale, float speed, float rotate)
    {
        ring.Rotate(0.0f, 0.0f, rotate * Time.deltaTime);
        float scale = Mathf.Lerp(ring.localScale.x, minScale, Time.deltaTime * speed);
        ring.localScale = new Vector3(scale, scale, 1.0f);
    }

    //目標値に到達したかを確認する関数
    private bool CheckReached(float current, float target)
    {
        return Mathf.Abs(current - target) < 0.01f;
    }

    //ロックオン状態を設定する関数
    public void SetLocked(bool locked)
    {
        isLocked = locked;
    }

}
