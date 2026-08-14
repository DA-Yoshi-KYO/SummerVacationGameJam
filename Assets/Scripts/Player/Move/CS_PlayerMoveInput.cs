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
    /// 上昇入力
    /// </summary>
    public bool AscendInput { get; private set; }


    private void FixedUpdate()
    {
        // 入力をリセット
        MoveInput = Vector2.zero;

        // キーボード入力を取得
        if (Keyboard.current != null)
        {
            // 移動の入力判定
            MoveInput = Vector2.ClampMagnitude(
                CS_InputManager.readInstance.customInputSystem.Player.Move.ReadValue<Vector2>(),
                1f
            );

            // ブーストの入力判定 
            BoostInput = CS_InputManager.readInstance.customInputSystem.Player.Boost.IsPressed();

            // ジャンプの入力判定
            AscendInput = CS_InputManager.readInstance.customInputSystem.Player.Jump.IsPressed();
        }
    }
}
