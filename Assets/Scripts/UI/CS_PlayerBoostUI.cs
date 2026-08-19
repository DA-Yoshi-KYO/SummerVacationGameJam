/* ＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
 *    Boostゲージ
 * ＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
 *    元浪梨緒
 * ----------------------------------------------------------
 * 2026-08-16 | 初回作成
 * 2026-08-17 | 自動回復を追加
 */
using UnityEngine;
using UnityEngine.UI;

public class CS_PlayerBoostUI : MonoBehaviour
{
    [Header("最大ブースト値")][SerializeField] private float maxBoost;
    [Header("現在のブースト値")][SerializeField] private float currentBoost;
    [Header("自動回復速度")][SerializeField] private float autoRecoverSpeed;
    [Header("自動回復する一定値")][SerializeField] private float autoRecoverBoost;

    [Header("白のゲージ")][SerializeField] private Image fullGauge;
    [Header("赤のゲージ")][SerializeField] private Image fewGauge;

    [Header("空の警告UI")][SerializeField] private Image emptyImage;
    [Header("点滅間隔")][SerializeField] private float blinkInterval;

    [Header("赤のゲージに切り替えする閾値")][SerializeField] private float fewThreshold;

    [Header("滑らかに変化する速度")][SerializeField] private float smoothSpeed;

    private float displayBoostRate;//ブーストの表示割合
    private float targetRate;//ブーストの目標割合

    private float blinkTime = 0.0f;

    void Start()
    {
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            UseBoost(10.0f);
        }

        AutoRecoverBoost();

        targetRate = currentBoost / maxBoost;

        displayBoostRate = Mathf.Lerp(displayBoostRate, targetRate, Time.deltaTime * smoothSpeed);

        fullGauge.fillAmount = displayBoostRate;
        fewGauge.fillAmount = displayBoostRate;

        //閾値によって色を切り替える
        if (displayBoostRate <= fewThreshold)
        {
            fullGauge.enabled = false;
            fewGauge.enabled = true;
        }
        else
        {
            fullGauge.enabled = true;
            fewGauge.enabled = false;
        }

        //ブーストが0なら点滅開始
        if (currentBoost <= 0.0f)
        {
            BlinkWarning();
        }
        else
        {
            //ブーストが回復したら非表示に戻す
            emptyImage.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
            blinkTime = 0.0f;
        }
    }

    //点滅処理
    private void BlinkWarning()
    {
        blinkTime += Time.deltaTime;

        float t = Mathf.PingPong(blinkTime, blinkInterval) / blinkInterval;

        float alpha = Mathf.Lerp(0.0f, 1.0f, t);

        emptyImage.color = new Color(1.0f, 1.0f, 1.0f, alpha);
    }

    //ブーストの消費
    public void UseBoost(float amount)
    {
        currentBoost -= amount;
        currentBoost = Mathf.Clamp(currentBoost, 0.0f, maxBoost);
    }

    //ブーストの回復
    public void RecoverBoost(float amount)
    {
        currentBoost += amount;
        currentBoost = Mathf.Clamp(currentBoost, 0.0f, maxBoost);
    }

    //ブーストの自動回復
    private void AutoRecoverBoost()
    {
        if (currentBoost < autoRecoverBoost)
        {
            currentBoost += autoRecoverSpeed * Time.deltaTime;

            if (currentBoost > autoRecoverBoost)
            {
                currentBoost = autoRecoverBoost;
            }
        }
    }

}
