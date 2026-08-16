/* ＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
 *    Hpゲージ
 * ＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
 *    元浪梨緒
 * ----------------------------------------------------------
 * 2026-08-16 | 初回作成
 */
using UnityEngine;
using UnityEngine.UI;

public class CS_PlayerHP : MonoBehaviour
{
    [Header("Hpゲージ")][SerializeField] private Image hpGauge;
    [Header("ダメージゲージ")][SerializeField] private Image damageGauge;

    [Header("最大HP値")][SerializeField] private float maxHp;
    [Header("現在HP値")][SerializeField] private float currentHp;

    [Header("ダメージの遅延時間")][SerializeField] private float damageDelay = 0.5f;
    [Header("ダメージの速度")][SerializeField] private float damageSpeed = 2.0f;

    private float damageTime = 0.0f;
    private float hpRate;//ブーストの表示割合

    void Start()
    {
        
    }

    void Update()
    {
        hpRate = currentHp / maxHp;

        hpGauge.fillAmount = hpRate;

        if (damageTime > damageDelay)
        {
            damageGauge.fillAmount = Mathf.Lerp(damageGauge.fillAmount, hpRate, Time.deltaTime * damageSpeed);
        }
        else
        {
            damageTime += Time.deltaTime;
        }
    }

    //ダメージを受ける
    public void Damage(float amount)
    {
        currentHp -= amount;
        currentHp = Mathf.Clamp(currentHp, 0.0f, maxHp);
        damageTime = 0.0f;
    }

    public void Heal(float amount)
    {
        currentHp += amount;
        currentHp = Mathf.Clamp(currentHp, 0.0f, maxHp);

        hpRate = currentHp / maxHp;
        damageGauge.fillAmount = hpRate;

        damageTime = 0.0f;
    }
}
