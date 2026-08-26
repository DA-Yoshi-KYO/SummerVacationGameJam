using UnityEngine;

public class CS_LowHpBoostEffect : CS_DamageBoostEffectBase
{
    [Tooltip("判定の体力割合")]
    private float _lowHpThreshold = 0.3f; // HPが30%以下のときにダメージ増加
    public float lowHpThreshold => _lowHpThreshold;

    /// <summary>
    /// 与えるダメージを増加させるメソッド
    /// </summary>
    public override int DamageUp(int baseDamage, GameObject enemy)
    {
        CS_EnemyBase enemyScript = enemy.GetComponent<CS_EnemyBase>();

        // HPが30%以下の場合、与えるダメージを増加させる
        if (enemyScript.health / enemyScript.maxHealth <= _lowHpThreshold)
        {
            int newDamage = Mathf.RoundToInt(baseDamage * _upDamageRate);

            // 追加ダメージを返す
            return newDamage - baseDamage;
        }
        else
        {
            return 0;
        }
    }
    
    public void SetLowHpThreshold(float threshold)
    {
        _lowHpThreshold = threshold;
    }
}