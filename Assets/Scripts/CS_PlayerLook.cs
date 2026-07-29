using UnityEngine;
using UnityEngine.InputSystem;

public class CS_PlayerLook : MonoBehaviour
{
    [SerializeField] float sensitivity = 0.1f;
    [SerializeField] float minPitch = -85f;
    [SerializeField] float maxPitch = 85f;

    Transform body;
    float pitch;

    void Start()
    {
        body = transform.parent;
        LockCursor();
    }

    void Update()
    {
        bool altHeld = Keyboard.current != null && Keyboard.current.altKey.isPressed;

        if (altHeld)
        {
            UnlockCursor();
            return;
        }

        LockCursor();

        Vector2 look = CS_InputManager.readInstance.customInputSystem.Player.Look.ReadValue<Vector2>();

        body.Rotate(Vector3.up, look.x * sensitivity);

        pitch = Mathf.Clamp(pitch - look.y * sensitivity, minPitch, maxPitch);
        transform.localRotation = Quaternion.Euler(pitch, 0f, 0f);
    }

    void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
