using UnityEngine;
using UnityEngine.InputSystem;

public class CS_PlayerMoveInput : MonoBehaviour
{
    /// <summary>
    /// 移動入力
    /// X = 左右
    /// Y = 前後
    /// </summary>
    public Vector2 MoveInput { get; private set; }

    /// <summary>
    /// ブースト入力
    /// </summary>
    public bool BoostInput { get; private set; }

    /// <summary>
    /// ジャンプ入力
    /// </summary>
    public bool JumpInput { get; private set; }

    private void Update()
    {
        // 入力をリセット
        MoveInput = Vector2.zero;

        // キーボード入力を取得
        if (Keyboard.current != null)
        {
            float x = 0f;
            float y = 0f;

            // 移動の入力判定
            if (Keyboard.current.aKey.isPressed)
                x -= 1f;

            if (Keyboard.current.dKey.isPressed)
                x += 1f;

            if (Keyboard.current.sKey.isPressed)
                y -= 1f;

            if (Keyboard.current.wKey.isPressed)
                y += 1f;

            // 入力を正規化してMoveInputに格納
            MoveInput = Vector2.ClampMagnitude(
                new Vector2(x, y),
                1f
            );

            // ブーストの入力判定
            BoostInput =
                Keyboard.current.leftShiftKey.isPressed;

            // ジャンプの入力判定
            JumpInput =
                Keyboard.current.spaceKey.wasPressedThisFrame;
        }
    }
}
