/* ＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
 *    ウェーブUI
 * ＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
 *    元浪梨緒
 * ----------------------------------------------------------
 * 2026-08-17 | 初回作成
 */
using TMPro;
using UnityEngine;

public class CS_WaveUI : MonoBehaviour
{
    [Header("ウェーブのテキスト")][SerializeField] private TextMeshProUGUI waveText;

    [Header("初期ウェーブ")][SerializeField] private int initWave;
    [Header("最大ウェーブ")][SerializeField] private int maxWave  ;

    private int currentWave;//現在のウェーブ

    void Start()
    {
        waveText.text = initWave.ToString() + " / " + maxWave.ToString();

        currentWave = initWave;
    }

    void Update()
    {
    }

    //ウェーブを増やす
    public void AddWave()
    {
        currentWave++;
        currentWave = Mathf.Clamp(currentWave, initWave, maxWave);
        waveText.text = currentWave.ToString() + " / " + maxWave.ToString();
    }

    //ウェーブを減らす
    public void SubtractWave()
    {
        currentWave--;
        currentWave = Mathf.Clamp(currentWave, initWave, maxWave);
        waveText.text = currentWave.ToString() + " / " + maxWave.ToString() ;
    }
}
