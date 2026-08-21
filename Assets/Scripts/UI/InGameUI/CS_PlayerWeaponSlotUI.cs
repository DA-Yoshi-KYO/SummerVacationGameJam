/* ＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
 *    UIの武器スロット
 * ＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
 *    元浪梨緒
 * ----------------------------------------------------------
 * 2026-08-17 | 初回作成
 */
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CS_PlayerWeaponSlotUI : MonoBehaviour
{
    [Header("フレーム画像")][SerializeField] private Image frameImage;
    [Header("フレームの中の白画像")][SerializeField] private Image BackImage;
    [Header("アイコン画像")][SerializeField] private Image iconImage;
    [Header("弾数テキスト")][SerializeField] private TextMeshProUGUI bulletsText;
    [Header("リロード画像")][SerializeField] private Image reloadImage;

    private int maxBullets;//最大弾数
    private int currentBullets;//現在の弾数

    private bool hasWeapon = false;//武器がセットされているか

    private bool isBlinking = false;//点滅中かどうか
    [Header("点滅の速さ")][SerializeField]private float blinkSpeed;

    void Update()
    {
        if (isBlinking)
        {
            float alpha = (Mathf.Sin(Time.time * blinkSpeed) + 1.0f) * 0.5f;

            reloadImage.color = new Color(1.0f, 1.0f, 1.0f, alpha);
        }
    }

    //武器データをセット
    public void SetupWeapon(CSO_WeaponDataBase.WeaponDataBase data)
    {
        hasWeapon = true;

        iconImage.sprite = data.weaponIcon;
        iconImage.enabled = true;
        bulletsText.enabled = true;
        BackImage.enabled = false;
        frameImage.GetComponent<CS_ChangeUITexture>().ChangeTexture(true);

        maxBullets = data.bulletCount;
        currentBullets = data.bulletCount;

        bulletsText.text = currentBullets.ToString();

        StopBlink();
    }

    //弾を消費する
    public void UseBullets(int amount)
    {
        if (!hasWeapon) return;

        currentBullets -= amount;
        currentBullets = Mathf.Clamp(currentBullets, 0, maxBullets);

        bulletsText.text = currentBullets.ToString();

        //弾が0になったら点滅開始
        if (currentBullets <= 0)
        {
            bulletsText.enabled = false;
            StartBlink();
        }
    }

    //弾の数を追加する
    public void AddBullets(int amount)
    {
        if (!hasWeapon) return;

        currentBullets += amount;
        currentBullets = Mathf.Clamp(currentBullets, 0, maxBullets);

        bulletsText.text = currentBullets   .ToString();

        //弾が0以上になったら点滅停止
        if (currentBullets > 0)
        {
            bulletsText.enabled = true;
            StopBlink();
        }
    }

    //スロットを空にする
    public void ClearSlot()
    {
        hasWeapon = false;

        frameImage.GetComponent<CS_ChangeUITexture>().ChangeTexture(false);

        iconImage.enabled = false;
        bulletsText.enabled = false;
        BackImage.enabled = true;

        StopBlink();
    }

    //点滅開始
    private void StartBlink()
    {
        if (isBlinking) return;

        isBlinking = true;
    }

    //点滅停止
    private void StopBlink()
    {
        isBlinking = false;

        reloadImage.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
    }
}