//using UnityEngine;
//using UnityEngine.InputSystem;

//public class CS_PlayerLook : MonoBehaviour
//{
//    [SerializeField] float sensitivity = 0.1f;
//    [SerializeField] float minPitch = -85f;
//    [SerializeField] float maxPitch = 85f;
//    [Tooltip("ADS中はスコープ倍率に応じて感度を下げる。未設定なら等倍のまま")]
//    [SerializeField] CS_ADSController adsController;

//    Transform body;
//    float pitch;

//    void Start()
//    {
//        body = transform.parent;
//        LockCursor();
//    }

//    void Update()
//    {
//        bool altHeld = Keyboard.current != null && Keyboard.current.altKey.isPressed;

//        if (altHeld)
//        {
//            UnlockCursor();
//            return;
//        }

//        LockCursor();

//        Vector2 look = CS_InputManager.readInstance.customInputSystem.Player.Look.ReadValue<Vector2>();

//        float effectiveSensitivity = sensitivity;
//        if (adsController != null)
//            effectiveSensitivity *= adsController.SensitivityMultiplier;

//        body.Rotate(Vector3.up, look.x * effectiveSensitivity);

//        pitch = Mathf.Clamp(pitch - look.y * effectiveSensitivity, minPitch, maxPitch);
//        transform.localRotation = Quaternion.Euler(pitch, 0f, 0f);
//    }

//    void LockCursor()
//    {
//        Cursor.lockState = CursorLockMode.Locked;
//        Cursor.visible = false;
//    }

//    void UnlockCursor()
//    {
//        Cursor.lockState = CursorLockMode.None;
//        Cursor.visible = true;
//    }
//}
