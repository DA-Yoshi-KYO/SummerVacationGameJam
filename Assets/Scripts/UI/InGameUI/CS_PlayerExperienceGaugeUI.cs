/* ＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
 *    高度のUI
 * ＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
 *    元浪梨緒
 * ----------------------------------------------------------
 * 2026-08-19 | 初回作成
 */
using UnityEngine;
using UnityEngine.UI;

public class CS_PlayerExperienceGaugeUI : MonoBehaviour
{
    [Header("経験値ゲージ")][SerializeField] private Image experienceGauge;

    [Header("現在の経験値")][SerializeField] private float currentExperience;
    [Header("次のレベルに必要な経験値")][SerializeField] private int experienceToNextLevel;

    [Header("レベルUI")][SerializeField] private CS_PlayerLevelUI playerLevelUI;

    private float experienceRate;//経験値の表示割合

    [Header("レベルアップスロットUI")][SerializeField]private CS_LevelUpSlotUI levelUpSlotUI;

    void Start()
    {
        if (playerLevelUI == null)
        {
            Debug.Log("レベルUIがありません");
            return;
        }

        currentExperience = 0;
    }

    void Update()
    {
        experienceRate = currentExperience / experienceToNextLevel;

        experienceGauge.fillAmount = experienceRate;
    }

    public void AddExperience(int amount)
    {
        currentExperience += amount;
        currentExperience = Mathf.Clamp(currentExperience, 0, experienceToNextLevel);

        if(currentExperience >= experienceToNextLevel)
        {
            //レベルアップ処理
            if (levelUpSlotUI.isSpinning)
            {
                levelUpSlotUI.slotRequestCount++;
            }
            else
            {
                levelUpSlotUI.StartSlot();
            }

            currentExperience = 0;

            if (playerLevelUI.currentplayerLevel == playerLevelUI.maxPlayerLevel)
            {
                currentExperience = experienceToNextLevel;
                return;
            }
        }
    }

    public void SubtractExperience(int amount)
    {
        currentExperience -= amount;

        if (currentExperience < 0)
        {
            if (playerLevelUI.currentplayerLevel == playerLevelUI.initPlayerLevel)
            {
                currentExperience = 0;
                return;
            }

            playerLevelUI.LevelDown();
            currentExperience = experienceToNextLevel + currentExperience;
            return;
        }

        currentExperience = Mathf.Clamp(currentExperience, 0, experienceToNextLevel);
    }
}
