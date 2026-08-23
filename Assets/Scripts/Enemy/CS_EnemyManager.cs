using System.IO.Pipes;
using UnityEngine;
using UnityEditor;
using System.Security.Cryptography;

public class CS_EnemyManager : MonoBehaviour
{
    [Header("プレイヤー")]
    [SerializeField] private Transform _playerTransform; // プレイヤーのTransform

    [Header("ウェーブ")]
    [SerializeField] private int _currentWave = 0; // 現在のウェーブ
    [SerializeField] private int _maxWave = 5;     // 最大ウェーブ数

    [Header("スポーン設定")]
    [SerializeField] private float _spawnInterval = 5.0f;   // スポーン間隔
    [SerializeField] private float _spawnRangeMin = 20.0f;  // スポーン範囲(最小)   // ←プレイヤーを中心にした範囲
    [SerializeField] private float _spawnRangeMax = 40.0f;  // スポーン範囲(最大)

    // 敵のスポーン情報を格納する構造体
    [System.Serializable]
    struct EnemySpawnInfo
    {
        public GameObject _enemyPrefab;
        public AnimationCurve _curve;
        public float _spawnHeight;
    }

    [Header("敵設定")]
    [SerializeField] private EnemySpawnInfo[] _enemySpawnInfos; // 敵のスポーン情報

    // gizmos表示用
    [Header("デバッグ")]
    [SerializeField] private bool _showDebugRange = true;

    private float _spawnTimer = 0f; // スポーンタイマー

    // Update is called once per frame
    void Update()
    {
        if (_spawnTimer > 0f)
        {
            _spawnTimer -= Time.deltaTime;
        }
        else
        {
            SpawnEnemy();
            _currentWave++;
            _spawnTimer = _spawnInterval;
        }
    }

    void SpawnEnemy()
    {
        if (_currentWave >= _maxWave)
        {
            Debug.Log("最大ウェーブに到達しました。");
            return;
        }

        for (int i = 0; i < _enemySpawnInfos.Length; i++)
        {
            EnemySpawnInfo spawnInfo = _enemySpawnInfos[i];
            // カーブからスポーン数を取得
            int spawnCount = Mathf.RoundToInt(spawnInfo._curve.Evaluate((float)_currentWave / _maxWave));
            // スポーン
            for (int j = 0; j < spawnCount; j++)
            {
                // ランダムな位置を生成
                Vector3 spawnPosition = _playerTransform.position;
                float randomAngle = Random.Range(0f, 360f) * Mathf.Deg2Rad; // ランダムな角度を取得
                float randomDistance = Random.Range(_spawnRangeMin, _spawnRangeMax); // ランダムな距離を取得
                spawnPosition.x += Mathf.Cos(randomAngle) * randomDistance; // X座標を計算
                spawnPosition.z += Mathf.Sin(randomAngle) * randomDistance; // Z座標を計算

                // 敵をスポーン
                GameObject enemy = Instantiate(spawnInfo._enemyPrefab, spawnPosition, Quaternion.identity);
                // プレイヤーのtransformを設定
                CS_EnemyBase enemyBase = enemy.GetComponent<CS_EnemyBase>();
                if (enemyBase != null)
                {
                    enemyBase.PlayerTransform = _playerTransform;
                }
            }
        }       
    }

    private void OnDrawGizmosSelected()
    {
        if (!_showDebugRange)
            return;

        // スポーン範囲を表示
        Handles.color = Color.red;
        Handles.DrawWireDisc(_playerTransform.position, Vector3.up, _spawnRangeMin);
        Handles.DrawWireDisc(_playerTransform.position, Vector3.up, _spawnRangeMax);
    }
}
