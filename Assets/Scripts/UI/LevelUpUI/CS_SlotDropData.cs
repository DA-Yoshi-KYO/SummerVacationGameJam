/* ＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
 *   ドラッグとドロップで武器をスロットに入れる処理
 * ＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
 *    元浪梨緒
 * ----------------------------------------------------------
 * 2026-08-23 | 初回作成
 */
using UnityEngine;
using UnityEngine.EventSystems;

public class CS_SlotDropData : MonoBehaviour, IDropHandler
{
    [Header("スロットの情報")][SerializeField] private CS_WeaponSet mySlot;

    [Header("このスロットの番号（0〜5）")][SerializeField] private int slotIndex;

    [Header("強化テキスト")][SerializeField] CS_UpGradeCountUI upGradeCountUI;

    //ドロップされたときに呼ばれる処理
    public void OnDrop(PointerEventData eventData)
    {
        var data = eventData.pointerDrag.GetComponentInParent<CS_SelectUISet>();
        if (data == null) return;

        //スロットが空なら入れる
        if (mySlot.currentWeapon == null)
        {
            //WeaponLevelDataを取得
            var weaponLevelData = data.GetData().weapon;

            //初期レベルを設定
            weaponLevelData.currentLevel = weaponLevelData.minLevel;

            //UIのレベル表示も初期化
            mySlot.weaponLevelUpUI.currentWeaponLevel = weaponLevelData.currentLevel;
            mySlot.weaponLevelUpUI.weaponLevelText.text = weaponLevelData.currentLevel.ToString();

            //スロットにセット
            mySlot.SetUI(data);

            //プレイ中の武器スロットも更新
            CS_LevelUpManager.Instance.SetWeapon(slotIndex, data);

            upGradeCountUI.LevelDown();

            return;
        }

        //スロットに武器が入っている場合
        string slotWeaponName = mySlot.currentWeapon.GetData().weapon.weaponName;
        string dragWeaponName = data.GetData().weapon.weaponName;

        //同じ武器ならレベルアップ
        if (slotWeaponName == dragWeaponName)
        {
            mySlot.weaponLevelUpUI.LevelUp();

            //レベルアップ後にPlayerWeaponSlotも更新する
            CS_LevelUpManager.Instance.SetWeapon(slotIndex, data);

            upGradeCountUI.LevelDown();

            return;
        }
    }
}