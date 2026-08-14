/* ＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
 *    物理運動計算クラス
 * ＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
 *    元浪梨緒
 * ----------------------------------------------------------
 * 2026-08-12 | 初回作成
 */
using UnityEngine;

public class CS_PhysicalMovement
{
    //エルミート曲線の計算用の関数
    public static Vector3 Hermite(Vector3 p0, Vector3 p1, Vector3 t0, Vector3 t1, float t)
    {
        float tt = t * t;
        float ttt = tt * t;

        float h00 = 2.0f * ttt - 3.0f * tt + 1.0f;
        float h10 = ttt - 2.0f * tt + t;
        float h01 = -2.0f * ttt + 3.0f * tt;
        float h11 = ttt - tt;

        return h00 * p0 + h10 * t0 + h01 * p1 + h11 * t1;
    }
}
