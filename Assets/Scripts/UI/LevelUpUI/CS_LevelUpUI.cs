/* ＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
 *   　UIのサイズや位置を変更
 * ＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
 *    元浪梨緒
 * ----------------------------------------------------------
 * 2026-08-20 | 初回作成
 */
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CS_LevelUpUI : MonoBehaviour
{
    [Header("上のバーの画像")][SerializeField] private RectTransform topBar;
    [Header("下のバーの画像")][SerializeField] private RectTransform bottomBar;

    [Header("最大スケール")][SerializeField] private float maxScaleY;
    [Header("最小スケール")][SerializeField] private float minScaleY;
    [Header("速度")][SerializeField] private float speed;
    [Header("閉じるまでの遅延時間")][SerializeField] private float delayBeforeClose;

    [Header("カーソルの画像")][SerializeField] private Texture2D cursorImage;

    private float targetScaleY;//現在のターゲットスケール

    //元の値を保存する辞書
    private Dictionary<RectTransform, float> originalHeights = new Dictionary<RectTransform, float>();
    private Dictionary<RectTransform, Vector2> originalPositions = new Dictionary<RectTransform, Vector2>();
    private Dictionary<TextMeshProUGUI, float> originalFontSizes = new Dictionary<TextMeshProUGUI, float>();

    private bool barsShouldShrink = false;//バーを縮めるタイミング管理

    [HideInInspector] public bool isLevelUpUIOpen;//レベルアップUIを開いているかどうか

    void Start()
    {
        //targetScaleY = minScaleY;
        //barsShouldShrink = false;

        //CacheOriginalValues();
        //ForceApplyClosedState();

        //カーソル表示
        isLevelUpUIOpen = true;
    }

    void Update()
    {
        CursorShow();

        //if (Input.GetKeyDown(KeyCode.UpArrow))
        //{
        //    OpenUI();
        //}

        //if (Input.GetKeyDown(KeyCode.DownArrow))
        //{
        //    CloseUIWithDelay();
        //}

        //UpdateCenterElements();
        //UpdateBarMovement();
        //CheckCenterGone();
        //UpdateBarShrink();
    }

    //UIを開く
    public void OpenUI()
    {
        isLevelUpUIOpen = true;

        targetScaleY = maxScaleY;
        barsShouldShrink = true;
    }

    //UIを閉じる
    public void CloseUI()
    {
        targetScaleY = minScaleY;
        barsShouldShrink = false;
    }

    //delayBeforeClose秒待つ
    private IEnumerator DelayClose()
    {
        isLevelUpUIOpen = false;

        yield return new WaitForSeconds(delayBeforeClose);
        CloseUI();
    }

    //delayBeforeClose秒待ってUIを閉じる
    public void CloseUIWithDelay()
    {
        StartCoroutine(DelayClose());
    }

    //初期値を保存する
    void CacheOriginalValues()
    {
        RectTransform[] rects = GetComponentsInChildren<RectTransform>(true);

        foreach (var rect in rects)
        {
            if (rect == transform) continue;

            originalHeights[rect] = rect.sizeDelta.y;
            originalPositions[rect] = rect.anchoredPosition;

            TextMeshProUGUI tmp = rect.GetComponent<TextMeshProUGUI>();
            if (tmp != null)
            {
                originalFontSizes[tmp] = tmp.fontSize;
            }
        }
    }

    //バー以外のUIを縮めて移動する
    void UpdateCenterElements()
    {
        foreach (var rect in originalHeights.Keys)
        {
            if (rect == topBar || rect == bottomBar)
                continue;

            ChangeSize(rect);
            AnimatePosition(rect);
        }
    }

    //バーの移動だけ連動させる
    void UpdateBarMovement()
    {
        AnimatePosition(topBar);
        AnimatePosition(bottomBar);
    }

    //中央UIが消えたか判定
    void CheckCenterGone()
    {
        bool allCenterGone = true;

        foreach (var rect in originalHeights.Keys)
        {
            if (rect == topBar || rect == bottomBar)
                continue;

            if (rect.sizeDelta.y > 1f)
            {
                allCenterGone = false;
                break;
            }
        }

        if (allCenterGone)
            barsShouldShrink = true;
    }

    //バーのサイズを縮める
    void UpdateBarShrink()
    {
        if (!barsShouldShrink) return;

        ChangeSize(topBar);
        ChangeSize(bottomBar);
    }

    //targetScaleYに応じて変化させる（拡大・縮小）
    void ChangeSize(RectTransform rect)
    {
        float originalY = originalHeights[rect];

        Vector2 size = rect.sizeDelta;
        size.y = Mathf.Lerp(size.y, originalY * targetScaleY, Time.deltaTime * speed);
        rect.sizeDelta = size;

        TextMeshProUGUI text = rect.GetComponent<TextMeshProUGUI>();
        if (text != null)
        {
            float originalFontSize = originalFontSizes[text];
            text.fontSize = Mathf.Lerp(text.fontSize, originalFontSize * targetScaleY, Time.deltaTime * speed);
        }
    }

    //targetScaleY に応じて位置を移動する
    void AnimatePosition(RectTransform rect)
    {
        Vector2 pos = rect.anchoredPosition;
        Vector2 originalPos = originalPositions[rect];

        pos.y = Mathf.Lerp(pos.y, originalPos.y * targetScaleY, Time.deltaTime * speed);
        rect.anchoredPosition = pos;
    }

    //UIを即座に閉じた状態にする
    private void ForceApplyClosedState()
    {
        isLevelUpUIOpen = false;

        foreach (var rect in originalHeights.Keys)
        {
            ApplySize(rect, 0.0f);
        }
    }

    //サイズを即座に適用する
    private void ApplySize(RectTransform rect, float height)
    {
        Vector2 size = rect.sizeDelta;
        size.y = height;
        rect.sizeDelta = size;

        TextMeshProUGUI text = rect.GetComponent<TextMeshProUGUI>();
        if (text != null)
        {
            text.fontSize = originalFontSizes[text] * (height == 0.0f ? 0.0f : 1.0f);
        }
    }

    //カーソルの表示・非表示する
    private void CursorShow()
    {
        if (isLevelUpUIOpen)
        {
            //カーソル表示
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            Vector2 hotspot = new Vector2(cursorImage.width / 2, cursorImage.height / 2);

            //カーソル画像を設定
            Cursor.SetCursor(cursorImage, hotspot, CursorMode.Auto);
        }
        else
        {
            //カーソル非表示
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            //カーソル画像を元に戻す
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }
    }
}
