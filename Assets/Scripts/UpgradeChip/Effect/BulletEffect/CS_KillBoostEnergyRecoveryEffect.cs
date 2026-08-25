using UnityEngine;

public class CS_KillBoostEnergyRecoveryEffect : MonoBehaviour
{
    [SerializeField]
    [Tooltip("回復する量")]
    private float _recoveryBoostEnergyAmount;

    [SerializeField]
    [Tooltip("プレイヤーのステータス参照")]
    private CS_PlayerMoveBoostEnergy _playerMoveBoostEnergy;

    public void ApplyEffect()
    {
        // プレイヤーの体力を回復する
        _playerMoveBoostEnergy.Regenerate(_recoveryBoostEnergyAmount);
    }

    public void SetRecoveryBoostEnergyAmount(float amount)
    {
        _recoveryBoostEnergyAmount = amount;
    }

    public void SetPlayerMoveBoostEnergy(CS_PlayerMoveBoostEnergy playerMoveBoostEnergy)
    {
        _playerMoveBoostEnergy = playerMoveBoostEnergy;
    }
}