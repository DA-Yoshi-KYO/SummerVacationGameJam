using UnityEngine;

public abstract class CS_DamageBoostEffectBase
{
    [Tooltip("与えるダメージ増加率")]
    protected float _upDamageRate = 1.2f;

    /// <summary>
    /// 与えるダメージを増加させる抽象メソッド
    /// </summary>
    public abstract int DamageUp(int baseDamage, GameObject enemy);

    public void SetUpDamageRate(float upDamageRate)
    {
        _upDamageRate = upDamageRate;
    }
}