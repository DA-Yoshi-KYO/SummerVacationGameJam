using UnityEngine;

public class CS_EnemyLand : CS_EnemyBase
{
    protected override void Idle()
    {
        Vector3 direction = _playerTransform.position - transform.position;
        // Y軸方向の移動を無効化
        direction.y = 0f;
        direction.Normalize();

        // プレイヤーの方向を向く
        transform.rotation = Quaternion.LookRotation(direction);
    }

    // 地上でプレイヤーに向かって直進
    protected override void Move()
    {
        Vector3 direction = _playerTransform.position - transform.position;
        // Y軸方向の移動を無効化
        direction.y = 0f;
        direction.Normalize();

        transform.position += direction * _moveSpeed * Time.deltaTime;
    }
}
