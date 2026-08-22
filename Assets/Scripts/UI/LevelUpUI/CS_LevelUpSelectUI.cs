using System.Collections.Generic;
using UnityEngine;

public class CS_LevelUpSelectUI : MonoBehaviour
{
    [SerializeField] private CSO_WeaponLevelData weaponData;
    //[SerializeField] private CSO_ChipLevelData chipData;

    [SerializeField] private GameObject weaponSlotPrefab;
    [SerializeField] private GameObject chipSlotPrefab;
    [SerializeField] private Transform slotParent;

    [SerializeField] private Vector2 leftslotPos;
    [SerializeField] private Vector2 slotSpce;


    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GenerateUpgradeSlots();
        }
    }

    public class UpgradeWrapper
    {
        public bool isWeapon;
        public CSO_WeaponLevelData.WeaponLevelData weapon;
        //public CSO_ChipLevelData.ChipLevelData chip;
    }

    private List<UpgradeWrapper> GetRandomUpgrades()
    {
        List<UpgradeWrapper> all = new List<UpgradeWrapper>();

        // 武器を追加
        foreach (var w in weaponData.weaponDatas.Values)
        {
            all.Add(new UpgradeWrapper { isWeapon = true, weapon = w });
        }

        // チップを追加
        //foreach (var c in chipData.chipList)
        //{
        //    all.Add(new UpgradeWrapper { isWeapon = false, chip = c });
        //}

        // ランダムで3つ選ぶ
        List<UpgradeWrapper> result = new List<UpgradeWrapper>();
        for (int i = 0; i < 3; i++)
        {
            result.Add(all[Random.Range(0, all.Count)]);
        }

        return result;
    }

    public void GenerateUpgradeSlots()
    {
        ClearSlots();

        var selected = GetRandomUpgrades();

        for (int i = 0; i < selected.Count; i++)
        {
            var upgrade = selected[i];

            GameObject prefab = upgrade.isWeapon
                ? weaponSlotPrefab
                : chipSlotPrefab;

            GameObject slot = Instantiate(prefab, slotParent);

            // 位置をずらす
            slot.transform.localPosition = new Vector3(leftslotPos.x + slotSpce.x * i, leftslotPos.y + slotSpce.y * i, 0);

            slot.GetComponent<CS_SelectUISet>().SetData(upgrade);
        }
    }


    private void ClearSlots()
    {
        foreach (Transform child in slotParent)
        {
            Destroy(child.gameObject);
        }
    }

}
