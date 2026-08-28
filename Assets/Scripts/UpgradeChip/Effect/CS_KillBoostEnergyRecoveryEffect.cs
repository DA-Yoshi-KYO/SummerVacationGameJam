using UnityEngine;

public class CS_KillBoostEnergyRecoveryEffect : MonoBehaviour
{
    [Tooltip("回復する量")]
    private float _recoveryBoostEnergyAmount = 5f;

    public void OnDestroy()
    {
        CS_PlayerMoveBoostEnergy playerMoveBoostEnergy = GameObject.FindAnyObjectByType<CS_PlayerMoveBoostEnergy>();

        // プレイヤーの体力を回復する
        if (playerMoveBoostEnergy != null)
            playerMoveBoostEnergy.Regenerate(_recoveryBoostEnergyAmount);
    }
}