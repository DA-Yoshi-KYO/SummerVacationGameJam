/* ＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
 *   　弾丸クラス（ミサイル弾）
 * ＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
 *    元浪梨緒
 * ----------------------------------------------------------
 * 2026-08-12 | 初回作成
 */
using UnityEngine;

public class CS_MissileBullet : CS_BaseBullet
{
    private Transform target;//追尾するターゲット
    private bool isDummyTarget = false;//ダミーターゲットかどうか

    private float t = 0.0f;//エルミート曲線の進行度
    private Vector3 startPos;//発射位置
    private Vector3 prevPos;//前フレームの位置（向き計算用）

    [Header("始点の曲がり具合（接線の強さ）")][SerializeField] private float startCurvePower;
    [Header("終点の曲がり具合（接線の強さ）")][SerializeField] private float endCurvePower;

    void Start()
    {
        startPos = transform.position;
        prevPos = startPos;
    }

    void Update()
    {
        if (target == null)
            return;

        //エルミート曲線の進行度を増やす
        t += Time.deltaTime * (speed / 10.0f);

        //終点に到達
        if (t >= 1.0f)
        {
            t = 1.0f;
            Destroy(gameObject);
            return;
        }

        //エルミート曲線の制御点
        Vector3 p0 = startPos;
        Vector3 p1 = target.position;

        //始点の接線
        Vector3 t0 = (Vector3.right + Vector3.up).normalized * startCurvePower;
        // 終点の接線
        Vector3 t1 = (Vector3.right + Vector3.up).normalized * endCurvePower;

        //次の位置をエルミート曲線で計算
        Vector3 nextPos = CS_PhysicalMovement.Hermite(p0, p1, t0, t1, t);

        transform.position = nextPos;

        //移動方向に向けて回転
        Vector3 dir = (nextPos - prevPos).normalized;
        if (dir.sqrMagnitude > 0.001f)
        {
            transform.rotation = Quaternion.LookRotation(dir);
        }

        //次フレーム用に位置を保存
        prevPos = nextPos;
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);

        if (collision.gameObject == owner)
            return;

        //dummyなら消す
        if (isDummyTarget && target != null)
        {
            Destroy(target.gameObject);
        }
    }

    //Setter関数

    //追尾ターゲットを設定する
    public void SetTarget(Transform Target)
    {
        target = Target;
    }

    //ダミーターゲットであることを設定する
    public void SetDummyTarget()
    {
        isDummyTarget = true;
    }

}
