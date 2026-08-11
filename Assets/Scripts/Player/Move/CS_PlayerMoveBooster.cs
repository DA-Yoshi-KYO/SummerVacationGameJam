using UnityEngine;

public class CS_PlayerMoveBooster : MonoBehaviour
{
    [SerializeField]
    private SO_PlayerMoveStatus _stats;

    [SerializeField]
    private CS_PlayerMoveInput _input;

    [SerializeField]
    private CS_PlayerMoveEnergy _energy;

    public bool IsBoosting { get; private set; }

    /// <summary>
    /// 現在のブースト推進力
    /// </summary>
    public float CurrentBoostForce { get; private set; }

    /// <summary>
    /// ブースト開始からの時間
    /// </summary>
    private float _boostTimer;

    /// <summary>
    /// 前フレームのブースト入力
    /// </summary>
    private bool _previousBoostInput;

    /// <summary>
    /// ブースト入力を離してからの時間
    /// </summary>
    private float _releaseBoostInputTimer;

    private void Awake()
    {
        _releaseBoostInputTimer = _stats.boostCooldown;
    }

    private void FixedUpdate()
    {
        UpdateBoost();
    }

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
            float normalizedTime =
                _boostTimer /
                _stats.boostInitialDuration;

            float curveValue =
                _stats.boostInitialCurve.Evaluate(
                    normalizedTime
                );

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
    /// EN消費
    /// </summary>
    private void ConsumeEnergy()
    {
        float consumption =
            _stats.boostEnergyConsumption *
            Time.fixedDeltaTime;

        _energy.Consume(consumption);
    }
}
