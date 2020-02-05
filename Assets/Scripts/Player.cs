using UnityEngine;

public class Player : MonoBehaviour
{
	[SerializeField] private CharacterController characterController;

	public Vector2 movementInput { get; set; }
	private const float moveSpeed = 2;

    void Update()
    {
		Move();   
    }

	private void Move()
	{
		Vector3 move = transform.right * movementInput.x + transform.forward * movementInput.y;
		characterController.Move(move * moveSpeed * Time.deltaTime);
	}
}
