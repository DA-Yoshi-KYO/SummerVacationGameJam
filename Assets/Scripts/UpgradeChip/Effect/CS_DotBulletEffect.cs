using UnityEngine;

public class CS_DotBulletEffect : MonoBehaviour
{
    [Tooltip("ダメージ間隔の時間")]
    private float damageInterval = 1.0f;

    [Tooltip("継続ダメージの継続時間")]
    private float dotDuration = 3.0f;

    [Tooltip("元ダメージの割合")]
    private float damageRate = 0.1f;

    public void ApplyDotEffect(GameObject target, float baseDamage)
    {
        CS_DamageOverTimeEffect damageOverTimeEffect = target.AddComponent<CS_DamageOverTimeEffect>();

        damageOverTimeEffect.SetDamageInterval(damageInterval);
        damageOverTimeEffect.SetRemainingTime(dotDuration);
        damageOverTimeEffect.SetDamageAmount(baseDamage * damageRate);

        damageOverTimeEffect.SetTarget();
    }

    public void SetDamageInterval(float interval)
    {
        damageInterval = interval;
    }
    public void SetRemainingTime(float time)
    {
        dotDuration = time;
    }
    public void SetDamageRate(float rate)
    {
        damageRate = rate;
    }
}