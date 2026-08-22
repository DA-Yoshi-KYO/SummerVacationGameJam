/* ＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
 *   　強化回数用のスクリプト
 * ＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
 *    元浪梨緒
 * ----------------------------------------------------------
 * 2026-08-20 | 初回作成
 */
using TMPro;
using UnityEngine;

public class CS_UpGradeCountUI : MonoBehaviour
{
    [Header("アップグレードのテキスト")][SerializeField] private TextMeshProUGUI upGradeText;

    [HideInInspector] public int currentUpGradeCount;//現在のレベル
    [Header("アップグレードのテキスト文")][SerializeField] private string upGrandeTextSet;

    void Start()
    {
    }

    void Update()
    {
    }

    //レベルアップ
    public void LevelUp()
    {
        currentUpGradeCount++;
        upGradeText.text = currentUpGradeCount.ToString() + upGrandeTextSet;
    }

    //レベルダウン
    public void LevelDown()
    {
        currentUpGradeCount--;
        upGradeText.text = currentUpGradeCount.ToString() + upGrandeTextSet;
    }
}
