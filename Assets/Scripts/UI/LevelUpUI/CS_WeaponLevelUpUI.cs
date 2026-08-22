using TMPro;
using UnityEngine;

public class CS_WeaponLevelUpUI : MonoBehaviour
{
    [Header("レベルのテキスト")][SerializeField] private TextMeshProUGUI weaponLevelText;

    [Header("初期レベル")] public int initWeaponLevel;
    [Header("最大レベル")] public int maxWeaponLevel;

    [HideInInspector] public int currentWeaponLevel;//現在のレベル

    void Start()
    {
        weaponLevelText.text = initWeaponLevel.ToString();

        currentWeaponLevel = initWeaponLevel;
    }

    void Update()
    {
    }

    //レベルアップ
    public void LevelUp()
    {
        currentWeaponLevel++;
        currentWeaponLevel = Mathf.Clamp(currentWeaponLevel, initWeaponLevel, maxWeaponLevel);
        weaponLevelText.text = currentWeaponLevel.ToString();
    }

    //レベルダウン
    public void LevelDown()
    {
        currentWeaponLevel--;
        currentWeaponLevel = Mathf.Clamp(currentWeaponLevel, initWeaponLevel, maxWeaponLevel);
        weaponLevelText.text = currentWeaponLevel.ToString();
    }
}
