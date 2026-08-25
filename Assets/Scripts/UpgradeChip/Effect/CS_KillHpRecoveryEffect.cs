using UnityEngine;

public class CS_KillHpRecoveryEffect : MonoBehaviour
{
    [Tooltip("敵を倒した時に回復するHPの量")]
    private float _recoveryHpAmount = 5f;

    public void OnDestroy()
    {
        CS_PlayerStatus playerStatus = GameObject.FindAnyObjectByType<CS_PlayerStatus>();

        // プレイヤーの体力を回復する
        if (playerStatus != null)
            playerStatus.Regenerate(_recoveryHpAmount);
    }
}