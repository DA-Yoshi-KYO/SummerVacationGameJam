using UnityEngine;

/// <summary>
/// 右クリック等でADSに入った際、メインカメラをスコープの接眼位置へ寄せ、
/// レンズオブジェクト(Quad)を視界いっぱいに見せるための制御。
/// タルコフのように「レンズの外側は覗いていない武器のジオメトリが見える」
/// 演出にするため、メインカメラ自体は動かしつつスコープはRenderTextureで別描画する。
/// </summary>
public class CS_ADSController : MonoBehaviour
{
    [Header("参照")]
    public Camera mainCamera;
    public Transform adsAlignPoint;   // スコープの接眼位置(EyeRelief位置)に置く空オブジェクト
    public Transform hipFireHold;     // 通常構え時の位置

    [Header("挙動")]
    public float adsTransitionSpeed = 12f;
    public AnimationCurve easing = AnimationCurve.EaseInOut(0, 0, 1, 1);
    [Tooltip("レンズ面をMain Cameraの正面この距離に置く")]
    public float lensDistance = 1f;

    [Header("スコープ")]
    public CS_ScopeRig activeScope; // 現在装備中の武器が持つScopeRig
    public CS_ReticleOverlay reticle; // 中央の赤ドット/十字線オーバーレイ(任意)

    private bool isAiming;
    private float blend; // 0=ヒップファイア, 1=ADS

    void Update()
    {
        var player = CS_InputManager.readInstance.customInputSystem.Player;
        SetAiming(player.Aim.IsPressed());

        float target = isAiming ? 1f : 0f;
        blend = Mathf.MoveTowards(blend, target, Time.deltaTime * adsTransitionSpeed);
        float eased = easing.Evaluate(blend);

        if (adsAlignPoint != null && hipFireHold != null)
        {
            transform.position = Vector3.Lerp(hipFireHold.position, adsAlignPoint.position, eased);
            transform.rotation = Quaternion.Slerp(hipFireHold.rotation, adsAlignPoint.rotation, eased);
        }

        // スコープのレンズ映像はADSにほぼ入り切ってから有効化すると
        // 遷移中に不自然な歪みが見えずに済む
        if (activeScope != null)
        {
            bool shouldRender = eased > 0.95f;
            if (activeScope.enabled != shouldRender)
                activeScope.enabled = shouldRender;

            // ADS中だけレンズ面を表示する(非ADS時は顔の前を塞いでしまうため)
            if (activeScope.lensRenderer != null && activeScope.lensRenderer.enabled != shouldRender)
                activeScope.lensRenderer.enabled = shouldRender;

            if (reticle != null)
                reticle.SetVisible(shouldRender);

            // 覗いている間だけホイールで倍率切り替え(タルコフの倍率変更操作)
            if (isAiming)
            {
                float scroll = player.Zoom.ReadValue<float>();
                if (player.Zoom.WasPerformedThisFrame() && Mathf.Abs(scroll) > 0.01f)
                {
                    activeScope.CycleZoom(scroll > 0f ? 1 : -1);
                }
            }
        }
    }

    void LateUpdate()
    {
        // CS_PlayerLookのピッチ反映(Update)より後に呼ばれるLateUpdateで追従させることで
        // 1フレーム遅れによるレンズのズレ・ジッターを防ぐ
        if (activeScope != null && activeScope.lensRenderer != null && mainCamera != null)
        {
            Transform lensT = activeScope.lensRenderer.transform;
            Transform camT = mainCamera.transform;
            lensT.SetPositionAndRotation(camT.position + camT.forward * lensDistance, camT.rotation);
        }
    }

    public void SetAiming(bool aiming)
    {
        isAiming = aiming;
    }

    public bool IsFullyAimed => blend > 0.95f;

    /// <summary>
    /// ADS中はスコープ倍率に応じてマウス感度を下げる。CS_PlayerLookから参照する。
    /// 等倍(裸眼)扱いの非ADS時は1を返す。
    /// </summary>
    public float SensitivityMultiplier
    {
        get
        {
            if (activeScope == null) return 1f;
            float eased = easing.Evaluate(blend);
            float zoomedMultiplier = 1f / Mathf.Max(1f, activeScope.CurrentMagnification);
            return Mathf.Lerp(1f, zoomedMultiplier, eased);
        }
    }
}
