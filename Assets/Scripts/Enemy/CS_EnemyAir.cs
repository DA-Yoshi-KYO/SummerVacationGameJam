using UnityEngine;

public class CS_EnemyAir : CS_EnemyBase
{
    [Header("浮遊")]
    [SerializeField] private float _floatHeight = 1.5f;
    [SerializeField] private float _floatSpeed = 2.0f;

    private float _startY;

    private void Start()
    {
        // 初期位置のY座標を保存
        _startY = transform.position.y;
    }

    // 上下に揺れる
    protected override void Idle()
    {
        // プレイヤーの方向を向く
        transform.LookAt(_playerTransform.position);

        // 上下に往復
        float y = _startY + Mathf.Sin(Time.time * _floatSpeed) * _floatHeight;

        Vector3 position = transform.position;
        position.y = y;
        transform.position = position;
    }

    // 空中でプレイヤーに向かってXZ平面上で進む
    protected override void Move()
    {
        Vector3 direction = _playerTransform.position - transform.position;
        // Y軸方向の移動を無効化
        direction.y = 0f;
        direction.Normalize();

        transform.position += direction * _moveSpeed * Time.deltaTime;
    }
}
