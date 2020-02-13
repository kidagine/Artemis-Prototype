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
		playerInputActions.PlayerControls.Crouch.performed += Crouch;
		playerInputActions.PlayerControls.Dash.performed += Dash;
		playerInputActions.PlayerControls.Draw.performed += DrawBow;
		playerInputActions.PlayerControls.Fire.performed += FireArrow;
		playerInputActions.PlayerControls.StartSummon.performed += SummonArrow;
		playerInputActions.PlayerControls.StopSummon.performed += StopSummonArrow;
	}

	private void SetMove(InputAction.CallbackContext context)
	{
		playerScript.movementInput = context.ReadValue<Vector2>();
	}

	private void SetCamera(InputAction.CallbackContext context)
	{
		playerCamera.cameraInput = context.ReadValue<Vector2>();
	}

	private void Crouch(InputAction.CallbackContext context)
	{
		if (context.performed)
		{
			playerScript.Crouch();
		}
	}

	private void Dash(InputAction.CallbackContext context)
	{
		if (context.performed)
		{
			playerScript.Dash();
		}
	}

	private void DrawBow(InputAction.CallbackContext context)
	{
		if (context.performed)
		{
			playerScript.DrawBow();
		}
	}

	private void FireArrow(InputAction.CallbackContext context)
	{
		if (context.performed)
		{
			playerScript.FireArrow();
		}
	}

	private void SummonArrow(InputAction.CallbackContext context)
	{
		if (context.performed)
		{
			playerScript.StartSummonArrow();
		}
	}

	private void StopSummonArrow(InputAction.CallbackContext context)
	{
		if (context.performed)
		{
			playerScript.StopSummonArrow();
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
