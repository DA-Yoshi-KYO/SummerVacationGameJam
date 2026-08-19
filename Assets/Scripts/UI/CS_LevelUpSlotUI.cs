/* ＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
 *    高度のUI
 * ＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
 *    元浪梨緒
 * ----------------------------------------------------------
 * 2026-08-19 | 初回作成
 */
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CS_LevelUpSlotUI : MonoBehaviour
{
    [Header("フレームの画像")][SerializeField] private Image frameImage;
    [Header("フレームの画像(Back)")][SerializeField] private Image frameImageBack;
    [Header("フレームの画像のマテリアル(Back)")][SerializeField] private Material frameImageBackMaterial;
    [Header("フレームの画像のサイズ（起動中）")][SerializeField] private Vector2 frameImageSizeSet;
    [Header("フレームの画像のサイズ（未起動中）")][SerializeField] private Vector2 frameImageSizeUnSet;
    [Header("数字のデータが入った親を格納")][SerializeField]private RectTransform slotContent;
    [Header("回転する速度")][SerializeField]private float speed;
    [Header("回転する時間")][SerializeField]private float spinTime;
    [Header("スロットの数字を非表示にする時間")][SerializeField]private float hideNumbersTime;

    [HideInInspector]public bool isSpinning = false;//スロットが回転中かどうか
    private float time = 0.0f;//スロットが回転している時間

    [Header("セルの高さ")][SerializeField]private float cellHeight;
    [Header("数字の数")][SerializeField]private int cellCount;

    private float loopHeight;//スロットのループする高さ
    private float initY;//スロットの初期位置
    private float loopY;//スロットのループする位置

    [Header("スロットの数字")][SerializeField]private TMP_Text[] slotNumbers;

    private int levelUpCount;//レベルの上昇回数
    [Header("レベルのUI")][SerializeField]private CS_PlayerLevelUI playerLevelUI;
    [HideInInspector] public int slotRequestCount = 0;//スロットの起動要求回数


    void Start()
    {
        frameImage.GetComponent<RectTransform>().sizeDelta = new Vector2(frameImageSizeUnSet.x, frameImageSizeUnSet.y);

        loopHeight = cellHeight * cellCount;

        initY = slotContent.anchoredPosition.y;
        loopY = initY - loopHeight + cellHeight;

        frameImageBackMaterial.SetTexture("_MainTexture2D", frameImageBack.sprite.texture);

        slotContent.gameObject.SetActive(false);
    }

    void Update()
    {
        //スロットが回転中なら
        if (isSpinning)
        {
            time += Time.deltaTime;

            slotContent.anchoredPosition -= new Vector2(0.0f, speed * Time.deltaTime);

            if (slotContent.anchoredPosition.y <= loopY)
            {
                //スロットの数字をシャッフルする
                if (time < spinTime)
                {
                    ShuffleNumbers();
                }

                //ループ位置に到達したら初期位置に戻す
                slotContent.anchoredPosition = new Vector2(slotContent.anchoredPosition.x, initY);
            }

            //スロットの回転時間が経過したら停止する
            if (time >= spinTime)
            {
                time = 0.0f;
                StopAtRandomNumber();
            }
        }
    }

    //スロットを回転させる
    public void StartSlot()
    {
        //フレームの画像を起動中の画像に変更する
        frameImage.GetComponent<CS_ChangeUITexture>().ChangeTexture(true);
        frameImage.GetComponent<RectTransform>().sizeDelta = new Vector2(frameImageSizeSet.x, frameImageSizeSet.y);

        //スロットの数字を表示
        slotContent.gameObject.SetActive(true);

        //スロットの初期位置に戻す
        slotContent.anchoredPosition = new Vector2(
            slotContent.anchoredPosition.x,
            initY
        );

        isSpinning = true;
        time = 0.0f;
    }

    //スロットをランダムな数字で停止させる
    private void StopAtRandomNumber()
    {
        isSpinning = false;

        int result = Random.Range(1, 4);

        float targetY = -(result - 1) * cellHeight + initY;

        slotContent.anchoredPosition = new Vector2(
            slotContent.anchoredPosition.x,
            targetY
        );

        //スロットの数字を取得
        string raw = slotNumbers[result - 1].text;
        //数字だけ取り出す
        string numberOnly = System.Text.RegularExpressions.Regex.Replace(raw, @"\D", "");
        levelUpCount = int.Parse(numberOnly);

        Invoke("StopSlot", hideNumbersTime);
    }

    //スロットの数字をシャッフルする
    private void ShuffleNumbers()
    {
        for (int i = 0; i < slotNumbers.Length; i++)
        {
            int r = Random.Range(1, 4);
            slotNumbers[i].text = r.ToString() + "Up";
        }
    }

    //未起動中の画像に変更して、スロットの数字を非表示にする
    private void StopSlot()
    {
        //フレームの画像を未起動中の画像に変更する
        frameImage.GetComponent<CS_ChangeUITexture>().ChangeTexture(false);
        frameImage.GetComponent<RectTransform>().sizeDelta = new Vector2(frameImageSizeUnSet.x, frameImageSizeUnSet.y);

        slotContent.gameObject.SetActive(false);

        for(int i = 0; i < levelUpCount; i++)
        {
            playerLevelUI.LevelUp();
        }

        //スロットの起動要求があれば、再度スロットを回転させる
        if (slotRequestCount > 0)
        {
            slotRequestCount--;
            StartSlot();
        }
    }
}
