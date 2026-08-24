using UnityEngine;

public class CS_KillHpRecoveryEffect : MonoBehaviour
{
    [SerializeField]
    [Tooltip("敵を倒した時に回復するHPの量")]
    private float _recoveryHpAmount = 5f;

    [SerializeField]
    [Tooltip("プレイヤーのステータス参照")]
    private CS_PlayerStatus _playerStatus;

    public void OnDestroy()
    {
        // プレイヤーの体力を回復する
        _playerStatus.Regenerate(_recoveryHpAmount);
    }

    public void SetRecoveryHpAmount(float amount)
    {
        _recoveryHpAmount = amount;
    }

    public void SetPlayerStatus(CS_PlayerStatus playerStatus)
    {
        _playerStatus = playerStatus;
    }
}