using UnityEngine;

public class CS_LifeStealEffect : MonoBehaviour
{
    [Tooltip("回復する割合")]
    private float _healRate = 0.1f;

    [Tooltip("プレイヤーのステータス参照")]
    private CS_PlayerStatus _playerStatus;

    public void ApplyEffect(float damage)
    {
        float healAmount = damage * _healRate;
        _playerStatus.Regenerate(healAmount);
    }

    public void SetHealRate(float healRate)
    {
        _healRate = healRate;
    }

    public void SetPlayerStatus(CS_PlayerStatus playerStatus)
    {
        _playerStatus = playerStatus;
    }
}