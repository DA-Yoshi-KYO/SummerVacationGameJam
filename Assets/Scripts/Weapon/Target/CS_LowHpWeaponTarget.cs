using UnityEngine;

public class CS_LowHpWeaponTarget : CS_WeaponTargetBase
{
    // •W€UI‚Éû‚Ü‚Á‚Ä‚¢‚Ä‘Ì—Í‚ª’á‚¢“G‚ğ—Dæ‚µ‚Ä‘_‚¤
    public override GameObject FindTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject targetEnemy = null;
        float lowestHp = float.MaxValue;

        foreach (GameObject enemy in enemies)
        {
            // “G‚Ì‘Ì—Í‚ğæ“¾
            CS_EnemyBase enemyBase = enemy.GetComponent<CS_EnemyBase>();
            if (enemyBase != null && enemyBase.health < lowestHp)
            {
                // ‘Ì—Í‚ª’á‚¢“G‚ğ—Dæ‚µ‚Äƒ^[ƒQƒbƒg‚É‚·‚é
                lowestHp = enemyBase.health;
                targetEnemy = enemy;
            }
        }

        return targetEnemy;
    }
}