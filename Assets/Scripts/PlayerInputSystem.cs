using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputSystem : MonoBehaviour
{
	[SerializeField] Player playerScript;
	private PlayerInputActions playerInputActions;


	private void Awake()
	{
		playerInputActions = new PlayerInputActions();
		playerInputActions.PlayerControls.Move.performed += SetMove;
	}

	public void SetMove(InputAction.CallbackContext context)
	{
		playerScript.movementInput = context.ReadValue<Vector2>();
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
