using UnityEngine;

public class CS_PlayerShield : MonoBehaviour
{
    [SerializeField]
    [Tooltip("シールドの耐久値")]
    private float _shieldDurability = 50.0f;

    [SerializeField]
    [Tooltip("シールドの最大耐久値")]
    private float _maxShieldDurability = 50.0f;

    [Tooltip("一時的なシールドの耐久値")]
    private float _tempShieldDurability = 0.0f;

    [SerializeField]
    [Tooltip("シールドの再生時間")]
    private float _shieldRegenTime = 10.0f;

    [SerializeField]
    [Tooltip("前回ダメージを受けてから経過した時間")]
    private float _timeSinceLastDamage = 0.0f;

    private void Update()
    {
        // 前回ダメージを受けてから経過した時間を更新
        _timeSinceLastDamage += Time.deltaTime;

        // シールドの耐久値が最大耐久値未満で、再生時間が経過している場合
        if (_timeSinceLastDamage >= _shieldRegenTime)
        {
            // シールドを再生する
            _shieldDurability = _maxShieldDurability;
        }
    }

    public float TakeDamage(float damage)
    {
        // 一時的なシールドの耐久値を考慮して残りダメージを計算
        float remainingDamage = Mathf.Max(damage - _tempShieldDurability, 0.0f);
        // 一時的なシールドの耐久値を減少させる
        _tempShieldDurability = Mathf.Max(_tempShieldDurability - damage, 0.0f);

        // シールドの耐久値を考慮して残りダメージを計算
        remainingDamage = Mathf.Max(damage - _shieldDurability, 0.0f);
        // シールドの耐久値を減少させる
        _shieldDurability = Mathf.Max(_shieldDurability - damage, 0.0f);

        // ダメージを受けたので時間をリセット
        _timeSinceLastDamage = 0.0f;

        // 残りダメージを返す
        return remainingDamage;
    }

    public void SetTempShieldDurability(float tempDurability)
    {
        _tempShieldDurability = tempDurability;
    }

    public void SetShieldDurability(float durability)
    {
        _maxShieldDurability = durability;
        _shieldDurability = durability;
    }

    public void SetShieldRegenTime(float regenTime)
    {
        _shieldRegenTime = regenTime;
        _timeSinceLastDamage = 0.0f;
    }
}
