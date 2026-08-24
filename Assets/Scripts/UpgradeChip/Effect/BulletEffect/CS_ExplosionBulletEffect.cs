using UnityEngine;

public class CS_ExplosionBulletEffect : MonoBehaviour
{
    [SerializeField]
    [Tooltip("ダメージを与える範囲")]
    private float _damageRadius = 5.0f;

    [SerializeField]
    [Tooltip("ダメージ量")]
    private float _damageAmount = 2.0f;

    protected virtual void OnTriggerEnter(Collider other)
    {
        // 範囲内の"Ebeny"タグを持つオブジェクトを取得
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, _damageRadius);

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy"))
            {
                // Enemyにダメージを与える処理を呼び出す
                CS_EnemyBase enemy = hitCollider.GetComponent<CS_EnemyBase>();
                if (enemy != null)
                {
                    // ダメージを与える処理を呼び出す
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        // ダメージ範囲を可視化するためのGizmosを描画
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _damageRadius);
    }
}