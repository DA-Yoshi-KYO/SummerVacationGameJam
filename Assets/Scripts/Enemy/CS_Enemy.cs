using System;
using System.Security.Cryptography;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // プレイヤーのtransform
    // スポーン時にマネージャーから取得する
    [SerializeField] private Transform player;

    // 移動速度
    [Header("ステータス")]
    [SerializeField] private float _health = 100f;
    [SerializeField] private float _moveSpeed = 3.0f;

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

    // Update is called once per frame
    private void Update()
    {
        // 死亡判定
        if (_health <= 0f)
        {
            Destroy(gameObject);
            return;
        }

        if (player == null)
            return;

        // プレイヤーの方向を向く
        transform.LookAt(player.position);

        // プレイヤーとの距離
        float distToPlayer = Vector3.Distance(transform.position, player.position);

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

    // プレイヤーの方向に移動する
    private void Move()
    {
        Vector3 direction = player.position - transform.position;

        // Y軸方向の移動を無効化
        direction.y = 0f;
        direction.Normalize();

        transform.position += direction * _moveSpeed * Time.deltaTime;
    }

    private void Attack()
    {
        if (player == null || _bulletPrefab == null)
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
        Vector3 direction = (player.position - transform.position).normalized;

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
