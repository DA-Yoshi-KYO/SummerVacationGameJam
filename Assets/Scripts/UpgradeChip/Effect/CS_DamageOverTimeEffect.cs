using UnityEngine;

public class CS_DamageOverTimeEffect : MonoBehaviour
{
    [Tooltip("ダメージを与える間隔")]
    private float _damageInterval = 1f;
    [Tooltip("残り時間")]
    private float _remainingTime = 3f;

    [Tooltip("与えるダメージ量")]
    private float _damageAmount = 5f;

    private float _timer = 0f;

    CS_EnemyBase _enemyBase;

    public void SetTarget()
    {
        _enemyBase = transform.GetComponent<CS_EnemyBase>();
    }

    private void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= _damageInterval)
        {
            // ダメージを与える
            _enemyBase.TakeDamage(_damageAmount);

            _timer = 0f;
        }

        _remainingTime -= Time.deltaTime;
        if (_remainingTime <= 0f)
        {
            Destroy(this);
        }
    }

    public void SetDamageInterval(float interval)
    {
        _damageInterval = interval;
    }

    public void SetDamageAmount(float amount)
    {
        _damageAmount = amount;
    }

    public void SetRemainingTime(float time)
    {
        _remainingTime = time;
    }
}