/* ＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
 *    キル数UI
 * ＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
 *    元浪梨緒
 * ----------------------------------------------------------
 * 2026-08-17 | 初回作成
 */
using TMPro;
using UnityEngine;

public class CS_KillUI : MonoBehaviour
{
    [Header("キルのテキスト")][SerializeField] private TextMeshProUGUI killText;

    [Header("初期キル")][SerializeField] private int initKill;
    [Header("最大キル")][SerializeField] private int maxKill;

    private int currentKill;//現在のキル

    void Start()
    {
        killText.text = initKill.ToString();

        currentKill = initKill;
    }

    void Update()
    {
    }

    //キルを増やす
    public void AddKill()
    {
        currentKill++;
        currentKill = Mathf.Clamp(currentKill, initKill, maxKill);
        killText.text = currentKill.ToString();
    }

    //キルを減らす
    public void SubtractKill()
    {
        currentKill--;
        currentKill = Mathf.Clamp(currentKill, initKill, maxKill);
        killText.text = currentKill.ToString();
    }
}
