using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    private const float mouseSensitivity = 20f;
    private float rotationX;

    public Vector2 cameraInput { get; set; }


    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        RotateCamera();
    }

    private void RotateCamera()
    {
        float mouseX = cameraInput.x * mouseSensitivity * Time.deltaTime;
        float mouseY = cameraInput.y * mouseSensitivity * Time.deltaTime;

        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -85f, 85f);

        transform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);
        playerTransform.Rotate(Vector3.up * mouseX);
    }
}