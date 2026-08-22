using UnityEngine;

[CreateAssetMenu(fileName = "DB_UpgradeStatus", menuName = "ScriptableObjects/Upgrade")]
public class CSO_UpgradeStatus : ScriptableObject
{
    [Header("体力の増加量")]
    [Tooltip("プレイヤーの最大体力を増加させる量")]
    public int healthIncreaseAmount;

    [Header("ダメージ軽減率")]
    [Tooltip("プレイヤーが受けるダメージを軽減する割合（0.0～1.0）")]
    public float damageReductionRate;

    [Header("ブーストエネルギーの増加量")]
    [Tooltip("ブーストエネルギーの最大値を増加させる量")]
    public int boostEnergyIncreaseAmount;

    [Header("ブースト消費量の軽減率")]
    [Tooltip("ブースト使用時の消費量を軽減する割合（0.0～1.0）")]
    public float boostConsumptionReductionRate;

    [Header("ブースト速度の増加率")]
    [Tooltip("ブースト使用時の移動速度を増加させる割合（0.0～1.0）")]
    public float boostSpeedIncreaseRate;

    [Header("取得経験値の増加率")]
    [Tooltip("敵を倒した際に得られる経験値を増加させる割合（0.0～1.0）")]
    public float experienceGainIncreaseRate;

    [Header("プレイヤーの移動速度の増加率")]
    [Tooltip("プレイヤーの移動速度を増加させる割合（0.0～1.0）")]
    public float playerMovementSpeedIncreaseRate;

    [Header("ダメージ増加率")]
    [Tooltip("プレイヤーが与えるダメージを増加させる割合（0.0～1.0）")]
    public float damageIncreaseRate;

    [Header("弾数の増加率")]
    [Tooltip("弾数を増加させる割合（0.0～1.0）")]
    public float bulletCountIncreaseRate;

    [Header("連射速度の増加率")]
    [Tooltip("連射速度を増加させる割合（0.0～1.0）")]
    public float fireRateIncreaseRate;

    [Header("リロード速度の増加率")]
    [Tooltip("リロード速度を増加させる割合（0.0～1.0）")]
    public float reloadSpeedIncreaseRate;

    [Header("弾のサイズ増加率")]
    [Tooltip("弾のサイズを増加させる割合（0.0～1.0）")]
    public float bulletSizeIncreaseRate;

    [Header("弾の射程増加率")]
    [Tooltip("弾の射程を増加させる割合（0.0～1.0）")]
    public float bulletRangeIncreaseRate;

    [Header("弾の貫通力の増加量")]
    [Tooltip("弾の貫通力を増加させる量")]
    public int bulletPenetrationIncreaseAmount;

    [Header("弾速の増加率")]
    [Tooltip("弾速を増加させる割合（0.0～1.0）")]
    public float bulletSpeedIncreaseRate;
}
