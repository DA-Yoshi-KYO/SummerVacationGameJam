using UnityEngine;

public class CS_PlayerMove : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float jumpHeight = 1.5f;
    [SerializeField] float gravity = -9.81f;

    CharacterController controller;
    Vector3 vel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = GetComponent<CharacterController>();
        vel = Vector3.zero;
    }

    // Update is called once per frame
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
                vel.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        vel.y += gravity * Time.deltaTime;

        controller.Move((move * moveSpeed + vel) * Time.deltaTime);
    }
}
