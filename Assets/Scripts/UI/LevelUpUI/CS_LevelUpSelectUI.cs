/* ＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
 *   レベルアップ時に表示される武器・チップの選択UIを生成する
 * ＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
 *    元浪梨緒
 * ----------------------------------------------------------
 * 2026-08-23 | 初回作成
 */
using System.Collections.Generic;
using UnityEngine;

public class CS_LevelUpSelectUI : MonoBehaviour
{
    [Header("武器データ")][SerializeField] private CSO_WeaponLevelData weaponData;
    //[SerializeField] private CSO_ChipLevelData chipData;

    [Header("武器用のスロットPrefab")][SerializeField] private GameObject weaponSlotPrefab;
    [Header("チップ用のスロットPrefab")][SerializeField] private GameObject chipSlotPrefab;
    [Header("生成したスロットの親オブジェクト")][SerializeField] private Transform slotParent;

    [Header("左端のスロット位置")][SerializeField] private Vector2 leftslotPos;

    [Header("スロット間の間隔")][SerializeField] private Vector2 slotSpce;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GenerateUpgradeSlots();
        }
    }

    //レベルアップ候補をまとめるためのラッパークラス
    //武器かチップかを判定しつつデータを保持する
    public class UpgradeWrapper
    {
        public bool isWeapon;
        public CSO_WeaponLevelData.WeaponLevelData weapon;
        //public CSO_ChipLevelData.ChipLevelData chip;
    }

    //武器・チップの中からランダムで3つ選ぶ
    private List<UpgradeWrapper> GetRandomUpgrades()
    {
        List<UpgradeWrapper> all = new List<UpgradeWrapper>();

        //武器を追加
        foreach (var w in weaponData.weaponDatas.Values)
        {
            all.Add(new UpgradeWrapper { isWeapon = true, weapon = w });
        }

        //チップを追加
        //foreach (var c in chipData.chipList)
        //{
        //    all.Add(new UpgradeWrapper { isWeapon = false, chip = c });
        //}

        //ランダムで3つ選ぶ
        List<UpgradeWrapper> result = new List<UpgradeWrapper>();
        for (int i = 0; i < 3; i++)
        {
            result.Add(all[Random.Range(0, all.Count)]);
        }

        return result;
    }

    //レベルアップ候補のUIスロットを生成する
    public void GenerateUpgradeSlots()
    {
        ClearSlots();

        var selected = GetRandomUpgrades();

        for (int i = 0; i < selected.Count; i++)
        {
            var upgrade = selected[i];

            //武器かチップかでプレハブを選択
            GameObject prefab = upgrade.isWeapon ? weaponSlotPrefab : chipSlotPrefab;

            //スロット生成
            GameObject slot = Instantiate(prefab, slotParent);

            //位置をずらす
            slot.transform.localPosition = new Vector3(leftslotPos.x + slotSpce.x * i, leftslotPos.y + slotSpce.y * i, 0);

            //UIにデータをセット
            slot.GetComponent<CS_SelectUISet>().SetData(upgrade);
        }
    }

    //既存のスロットを削除する
    public void ClearSlots()
    {
        foreach (Transform child in slotParent)
        {
            Destroy(child.gameObject);
        }
    }
}
