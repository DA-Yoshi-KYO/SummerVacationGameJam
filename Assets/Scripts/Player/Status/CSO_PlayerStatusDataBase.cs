using UnityEngine;

[CreateAssetMenu(
    fileName = "DB_PlayerStatusDataBase", 
    menuName = "ScriptableObjects/Player/Status",
    order = 1
    )]
public class CSO_PlayerStatusDataBase : ScriptableObject
{
    [Header("===== 体力 =====")]

    [Tooltip("最大体力")]
    public float maxHealth = 100f;

    [Tooltip("初期体力")]
    public float initialHealth = 100f;

    [Header("===== レベル =====")]

    [Tooltip("初期レベル")]
    public int initialLevel = 1;

    [Tooltip("最大レベル")]
    public int maxLevel = 99;

    [Tooltip("初期必要経験値")]
    public float initialNextLevelExp = 5f;

    [Tooltip("レベルアップごとの必要経験値の上昇率")]
    public float expGrowthRate = 1.2f;
}
