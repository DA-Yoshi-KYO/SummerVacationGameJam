using UnityEngine;

public class CS_PlayerCameraController : MonoBehaviour
{
    [Header("===== 参照 =====")]

    [Tooltip("カメラが追従する対象")]
    [SerializeField]
    private Transform _cameraFollowTarget;

    [Tooltip("カメラのルートオブジェクト")]
    [SerializeField]
    private Transform _cameraRoot;

    [Tooltip("カメラのピボットオブジェクト")]
    [SerializeField]
    private Transform _cameraPivot;

    [Tooltip("カメラ")]
    [SerializeField]
    private Camera _camera;

    [Header("===== カメラ設定 =====")]

    [Tooltip("通常時の追従速度")]
    [SerializeField] 
    private float _followSmoothTime = 0.15f;
    
    [Tooltip("プレイヤーが画面端に近づいた時の追従速度")]
    [SerializeField]
    private float _limitFollowSmoothTime = 0.03f;
    
    [Tooltip("画面端に近づいたと判定する割合")]
    [Range(0.5f, 1.0f)]
    [SerializeField] 
    private float _screenEdgeThreshold = 0.75f; 
    
    [Tooltip("画面外に出る直前の強制追従")]
    [Range(0.8f, 1.0f)]
    [SerializeField] 
    private float _screenLimitThreshold = 0.9f;

    [Tooltip("カメラの回転感度")]
    [SerializeField]
    private float _sensitivity = 0.1f;

    [Tooltip("カメラの上下回転の最小角度")]
    [SerializeField]
    private float _minPitch = -40f;

    [Tooltip("カメラの上下回転の最大角度")]
    [SerializeField]
    private float _maxPitch = 60f;

    [Tooltip("カメラの距離")]
    [SerializeField]
    private float _distance = 15f;


    private float _yaw;
    private float _pitch;

    private Vector3 _followVelocity;


    private void Start()
    {
        Cursor.lockState =
            CursorLockMode.Locked;

        Cursor.visible = false;

        // カメラの初期位置
        _camera.transform.localPosition =
            new Vector3(
                0f,
                0f,
                -_distance
            );
    }


    private void FixedUpdate()
    {
        UpdateFollow();
        UpdateLook();
    }


    /// <summary>
    /// プレイヤーを遅れて追従する
    /// </summary>
    private void UpdateFollow()
    {
        if (_cameraFollowTarget == null) { return; }

        Vector3 targetPosition = _cameraFollowTarget.position; 

        // プレイヤーのスクリーン座標を取得
        Vector3 screenPosition = _camera.WorldToViewportPoint(targetPosition);

        float distanceFromCenterX = Mathf.Abs(screenPosition.x - 0.5f) * 2f;
        float distanceFromCenterY = Mathf.Abs(screenPosition.y - 0.5f) * 2f; 

        // X/Yのうち、端に近い方を使用
        float edgeDistance = Mathf.Max(distanceFromCenterX, distanceFromCenterY);

        // ================================ 
        // 通常追従 
        // ================================

        float smoothTime = _followSmoothTime;

        // ================================ 
        // 画面端に近づいた場合 
        // ================================

        if (edgeDistance > _screenEdgeThreshold)
        {
            float t = 
                Mathf.InverseLerp(
                    _screenEdgeThreshold,
                    _screenLimitThreshold, 
                    edgeDistance
                    );

            smoothTime = 
                Mathf.Lerp(
                    _followSmoothTime, 
                    _limitFollowSmoothTime,
                    t
                    );
        }

        // ================================ 
        // 画面外に出そうな場合 
        // ================================

        if (edgeDistance >= _screenLimitThreshold)
        {
            smoothTime = _limitFollowSmoothTime;
        }

        // ================================ 
        // 追従 
        // ================================

        transform.position = 
            Vector3.SmoothDamp(
                transform.position,
                targetPosition, 
                ref _followVelocity, 
                smoothTime
                );
    }

    /// <summary>
    /// カメラ視点を更新する
    /// </summary>
    private void UpdateLook()
    {
        Vector2 lookInput =
            CS_InputManager.readInstance.customInputSystem.Player.Look.ReadValue<Vector2>();

        _yaw +=
            lookInput.x *
            _sensitivity;

        _pitch -=
            lookInput.y *
            _sensitivity;

        _pitch =
            Mathf.Clamp(
                _pitch,
                _minPitch,
                _maxPitch
            );

        // 左右
        _cameraRoot.localRotation =
            Quaternion.Euler(
                0f,
                _yaw,
                0f
            );

        // 上下
        _cameraPivot.localRotation =
            Quaternion.Euler(
                _pitch,
                0f,
                0f
            );
    }
}