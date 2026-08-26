using System;
using System.Security.Cryptography;
using UnityEngine;

public class CS_EnemyBase : MonoBehaviour
{
    // プレイヤーのtransform
    // スポーン時にマネージャーから取得する
    [SerializeField] protected Transform _playerTransform;

    public Transform PlayerTransform
    {
        set { _playerTransform = value; }
    }

    // 移動速度
    [Header("ステータス")]
    [SerializeField] private float _health = 100f;
    public float health => _health;
    private float _maxHealth = 100f;
    public float maxHealth => _maxHealth;
    [SerializeField] protected float _moveSpeed = 3.0f;

    // 攻撃関連
    [Header("攻撃")]
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private float _bulletSpeed = 10f;
    [SerializeField] private float _attackCoolDown = 1.0f;
    private float _attackTimer = 0f;

    // 範囲
    [Header("範囲")]
    [SerializeField] private float _attackRange = 15f;
    [SerializeField] private float _stopRange = 5f;

    // gizmos表示用
    [Header("デバッグ")]
    [SerializeField] private bool _showDebugRange = true;

    private void Start()
    {
        _maxHealth = _health;
    }

    // Update is called once per frame
    private void Update()
    {
        // 死亡判定
        if (_health <= 0f)
        {
            Destroy(gameObject);
            return;
        }

        if (_playerTransform == null)
            return;

        // プレイヤーとの距離
        float distToPlayer = Vector3.Distance(transform.position, _playerTransform.position);

        Idle();

        // プレイヤーが範囲外にいる場合、移動する
        if (distToPlayer > _stopRange)
        {
            Move();
        }
        // プレイヤーが射程内にいる場合、攻撃する
        if (distToPlayer < _attackRange)
        {
            Attack();
        }
    }

    private void OnDestroy()
    {
        // 撃破時にダメージ増加エフェクトがあれば、撃破数を加算する
        CS_UpgradeChipManager chipManager = GameObject.FindAnyObjectByType<CS_UpgradeChipManager>();

        if (chipManager != null)
        {
            foreach (var effect in chipManager.damageBoostEffects)
            {
                if (effect is CS_KillStackDamageBoostEffect damageBoostEffect)
                {
                    damageBoostEffect.IncreaseKillStack();
                    break;
                }
            }
        }
    }

    // 待機中の動き(常に呼び出される)
    protected virtual void Idle()
    {
    }
    
    // 移動
    protected virtual void Move()
    {
    }

    private void Attack()
    {
        if (_playerTransform == null || _bulletPrefab == null)
            return;

        if (_attackTimer > 0f)
        {
            _attackTimer -= Time.deltaTime;
            return;
        }
        else
        {
            _attackTimer = _attackCoolDown;
        }

        // プレイヤーへの方向
        Vector3 direction = (_playerTransform.position - transform.position).normalized;

        // 弾を生成
        GameObject bullet = Instantiate(
            _bulletPrefab,
            transform.position,
            Quaternion.LookRotation(direction)
        );

        // 弾をプレイヤー方向へ飛ばす
        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.linearVelocity = direction * _bulletSpeed;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (!_showDebugRange)
            return;

        // 停止範囲
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _stopRange);

        // 攻撃範囲
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _attackRange);
    }
}
