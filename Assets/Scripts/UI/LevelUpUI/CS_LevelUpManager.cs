using UnityEngine;

public class CS_LevelUpManager : MonoBehaviour
{
    public static CS_LevelUpManager Instance;

    // プレイヤーが装備している武器データ（6スロット）
    public CSO_WeaponLevelData.WeaponLevelData[] playerWeapons = new CSO_WeaponLevelData.WeaponLevelData[6];

    // 武器スロットの UI
    public CS_DropSelectUIData[] weaponSlots = new CS_DropSelectUIData[6];

    private void Awake()
    {
        Instance = this;
    }

    public void SetWeapon(int slotIndex, CS_SelectUISet selectUI)
    {
        Debug.Log($"slotIndex = {slotIndex}");
        Debug.Log($"weaponSlots.Length = {weaponSlots.Length}");
        Debug.Log($"weaponSlots[{slotIndex}] = {weaponSlots[slotIndex]}");

        var upgradeData = selectUI.GetData();

        if (!upgradeData.isWeapon)
        {
            Debug.Log("武器じゃないデータがドロップされたよ");
            return;
        }

        // 正しい型で受け取る
        var weapon = upgradeData.weapon;

        // プレイヤーの武器リストにセット
        playerWeapons[slotIndex] = weapon;

        // UI を更新
        weaponSlots[slotIndex].SetUI(weapon);

        Debug.Log($"武器 {weapon.weaponName} をスロット {slotIndex} にセットしました");
    }
}
