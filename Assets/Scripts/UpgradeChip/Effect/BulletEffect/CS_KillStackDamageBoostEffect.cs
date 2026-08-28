using UnityEngine;

public class CS_KillStackDamageBoostEffect : CS_DamageBoostEffectBase
{
    [Tooltip("ダメージ上昇を適用するカウント区切り数")]
    private int _killStackThreshold = 10; // ダメージ上昇を適用するカウント区切り数

    [Tooltip("撃破数スタックごとのダメージ増加率")]
    private float _damageIncreasePerStack = 0.1f; // スタックごとのダメージ増加率

    private int _currentKillStack = 0; // 現在の撃破数スタック

    public override int DamageUp(int baseDamage, GameObject enemy)
    {
        // 撃破数スタックを回数に応じて増加させる
        int boostCount = _currentKillStack / _killStackThreshold;

        // ダメージ増加率を計算
        float damageMultiplier = 1 + (boostCount * _damageIncreasePerStack);

        if (boostCount > 0)
        {
            int newDamage = Mathf.RoundToInt(baseDamage * damageMultiplier);

            // 追加ダメージを返す
            return newDamage - baseDamage;

        }
        else
        {
            return 0;
        }
    }

    // 撃破数スタックを増加させるメソッド
    public void IncreaseKillStack()
    {
        _currentKillStack++;
    }

    public void SetKillStackThreshold(int threshold)
    {
        _killStackThreshold = threshold;
    }
}