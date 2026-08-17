using UnityEngine;

public class CS_PlayerMoveBooster : MonoBehaviour
{
    [Header("===== 参照 =====")]

    [Tooltip("プレイヤーのステータスSO")]
    [SerializeField]
    private CSO_PlayerMoveStatus _stats;

    [Tooltip("プレイヤーの入力情報")]
    [SerializeField]
    private CS_PlayerMoveInput _input;

    [Tooltip("プレイヤーのエネルギー管理")]
    [SerializeField]
    private CS_PlayerMoveEnergy _energy;

    /// <summary>
    /// ブースト中かどうか
    /// </summary>
    public bool IsBoosting { get; private set; }

    /// <summary>
    /// 現在のブースト推進力
    /// </summary>
    public float CurrentBoostForce { get; private set; }

    [Tooltip("ブースト開始からの時間")]
    private float _boostTimer;

    [Tooltip("前フレームのブースト入力")]
    private bool _previousBoostInput;

    [Tooltip("ブースト入力を離してからの時間")]
    private float _releaseBoostInputTimer;


    private void Awake()
    {
        _releaseBoostInputTimer = _stats.boostCooldown;
    }

    private void FixedUpdate()
    {
        UpdateBoost();
    }

    /// <summary>
    /// ブーストの状態を更新
    /// </summary>
    private void UpdateBoost()
    {
        bool boostInput = _input.BoostInput;

        // ========================================
        // ブースト入力を離してからの時間を更新
        // ========================================

        if (_releaseBoostInputTimer < _stats.boostCooldown)
        {
            StopBoost();

            _releaseBoostInputTimer += Time.fixedDeltaTime;

            if (_releaseBoostInputTimer >= _stats.boostCooldown)
            {
                _releaseBoostInputTimer = _stats.boostCooldown;
            }
            else return;
        }


        // ========================================
        // ブーストしていない
        // ========================================

        if (!boostInput)
        {
            StopBoost();

            if(_previousBoostInput) _releaseBoostInputTimer = 0;

            _previousBoostInput = false;

            return;
        }

        // ========================================
        // EN不足
        // ========================================

        if (!_energy.CanBoost())
        {
            StopBoost();

            _previousBoostInput = true;

            return;
        }

        // ========================================
        // ブースト開始
        // ========================================

        bool boostStarted =
            !_previousBoostInput;

        if (boostStarted)
        {
            StartBoost();
        }

        // ========================================
        // ブースト継続
        // ========================================

        UpdateBoostForce();

        ConsumeEnergy();

        _previousBoostInput = true;
    }

    /// <summary>
    /// ブースト開始
    /// </summary>
    private void StartBoost()
    {
        IsBoosting = true;

        _boostTimer = 0f;
    }

    /// <summary>
    /// ブースト終了
    /// </summary>
    private void StopBoost()
    {
        IsBoosting = false;

        CurrentBoostForce = 0f;

        _boostTimer = 0f;
    }

    /// <summary>
    /// 現在の推進力を計算
    /// </summary>
    private void UpdateBoostForce()
    {
        _boostTimer += Time.fixedDeltaTime;

        // 初動ブースト中
        if (_boostTimer < _stats.boostInitialDuration)
        {
            // 初動ブーストの時間を0～1に正規化
            float normalizedTime =
                _boostTimer /
                _stats.boostInitialDuration;

            // 初動ブーストのカーブ値を取得
            float curveValue =
                _stats.boostInitialCurve.Evaluate(
                    normalizedTime
                );

            // 初動ブーストのカーブ値を使って、初動ブースト力と継続ブースト力を補間
            CurrentBoostForce =
                Mathf.Lerp(
                    _stats.boostContinuousForce,
                    _stats.boostInitialForce,
                    curveValue
                );

            return;
        }

        // 初動終了後
        CurrentBoostForce =
            _stats.boostContinuousForce;
    }

    /// <summary>
    /// エネルギー消費
    /// </summary>
    private void ConsumeEnergy()
    {
        float consumption =
            _stats.boostEnergyConsumption *
            Time.fixedDeltaTime;

        _energy.Consume(consumption);
    }
}
