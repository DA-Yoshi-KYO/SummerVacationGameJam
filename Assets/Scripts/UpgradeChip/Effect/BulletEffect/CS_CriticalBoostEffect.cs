using UnityEngine;

public class CS_CriticalBoostEffect : CS_DamageBoostEffectBase
{
    [Tooltip("クリティカル発生率")]
    private float _criticalRate = 0.2f;

    /// <summary>
    /// 与えるダメージを増加させるメソッド
    /// </summary>
    public override int DamageUp(int baseDamage, GameObject enemy)
    {
        // クリティカル判定
        if (Random.value < _criticalRate)
        {
            int newDamage = Mathf.RoundToInt(baseDamage * _upDamageRate);

            // クリティカル発生時のダメージ計算
            return newDamage - baseDamage; // クリティカル発生時の追加ダメージを返す
        }
        else
        {
            // 通常ダメージ
            return 0;
        }
    }

    public void SetCriticalRate(float criticalRate)
    {
        _criticalRate = criticalRate;
    }
}