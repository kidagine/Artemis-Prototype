using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    private const float mouseSensitivity = 20f;
    private Player player;
    private float rotationX;


    public Vector2 cameraInput { get; set; }


    void Start()
    {
        player = playerTransform.GetComponent<Player>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        RotateCamera();
        if (!player.HasBow)
        {
            RaycastBow();
        }
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


    private void RaycastBow()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 2))
        {
            if (hit.collider.CompareTag("Raycast"))
            {
                UIManager.Instance.ShowEnter();
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    UIManager.Instance.HideEnter();
                    Destroy(hit.collider.gameObject);
                    player.GrabBow();
                }
            }
        }
        else
        {
            UIManager.Instance.HideEnter();
        }
    }
}