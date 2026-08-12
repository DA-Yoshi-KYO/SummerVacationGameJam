using UnityEngine;

public class CS_PlayerMove : MonoBehaviour
{
    [SerializeField]
    [Header("プレイヤーの基礎移動速度")]
    private float _baseMoveSpeed = 5f;

    [SerializeField]
    [Header("ジャンプの高さ")]
    private float _jumpHeight = 2f;

    [SerializeField] 
    private float _gravity = -9.81f;

    CharacterController controller;
    Vector3 vel;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        vel = Vector3.zero;
    }

    void Update()
    {
        var player = CS_InputManager.readInstance.customInputSystem.Player;
        Vector2 input = player.Move.ReadValue<Vector2>();
        Vector3 move = transform.right * input.x + transform.forward * input.y;

        if (controller.isGrounded)
        {
            if (vel.y < 0f)
                vel.y = -2f;

            if (player.Jump.WasPressedThisFrame())
                vel.y = Mathf.Sqrt(_jumpHeight * -2f * _gravity);
        }

        // 重力を適用
        vel.y += _gravity * Time.deltaTime;

        controller.Move((move * _baseMoveSpeed + vel) * Time.deltaTime);
    }
}
