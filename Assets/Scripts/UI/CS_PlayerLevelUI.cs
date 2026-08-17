/* ＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
 *    レベルUI
 * ＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
 *    元浪梨緒
 * ----------------------------------------------------------
 * 2026-08-17 | 初回作成
 */
using TMPro;
using UnityEngine;

public class CS_PlayerLevelUI : MonoBehaviour
{
    [Header("レベルのテキスト")][SerializeField] private TextMeshProUGUI levelText;

    [Header("初期レベル")][SerializeField]private int initPlayerLevel;
    [Header("最大レベル")][SerializeField]private int　maxPlayerLevel;

    private int currentplayerLevel;//現在のレベル

    void Start()
    {
        levelText.text = initPlayerLevel.ToString();

        currentplayerLevel = initPlayerLevel;
    }

    void Update()
    {
    }

    //レベルアップ
    public void LevelUp()
    {
        currentplayerLevel++;
        currentplayerLevel = Mathf.Clamp(currentplayerLevel, initPlayerLevel, maxPlayerLevel);
        levelText.text = currentplayerLevel.ToString();
    }

    //レベルダウン
    public void LevelDown()
    {
        currentplayerLevel--;
        currentplayerLevel = Mathf.Clamp(currentplayerLevel, initPlayerLevel, maxPlayerLevel);
        levelText.text = currentplayerLevel.ToString();
    }
}
