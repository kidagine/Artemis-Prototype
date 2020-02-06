using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputSystem : MonoBehaviour
{
	[SerializeField] private Player playerScript;
	[SerializeField] private PlayerCamera playerCamera;
	private PlayerInputActions playerInputActions;


	private void Awake()
	{
		playerInputActions = new PlayerInputActions();
		playerInputActions.PlayerControls.Move.performed += SetMove;
		playerInputActions.PlayerControls.Camera.performed += SetCamera;
		playerInputActions.PlayerControls.Fire.performed += FireArrow;
	}

	private void SetMove(InputAction.CallbackContext context)
	{
		playerScript.movementInput = context.ReadValue<Vector2>();
	}

	private void SetCamera(InputAction.CallbackContext context)
	{
		playerCamera.cameraInput = context.ReadValue<Vector2>();
	}

	private void FireArrow(InputAction.CallbackContext context)
	{
		if (context.performed)
		{
			playerScript.FireArrow();
		}
	}

	private void OnEnable()
	{
		playerInputActions.Enable();
	}

	private void OnDisable()
	{
		playerInputActions.Disable();	
	}
}
