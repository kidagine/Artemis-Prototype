using UnityEngine;

public class Player : MonoBehaviour
{
	[SerializeField] private CharacterController characterController;
	[SerializeField] private Transform arrowTransform;
	[SerializeField] private LayerMask environmentLayerMask;
	public Vector3 arrowImpulse;
	private const float moveSpeed = 2;
	private const float gravity = 0.1f;
	private Vector3 velocity;
	private bool isGrounded;
	private bool hasArrow = true;

	public Vector2 movementInput { get; set; }
	

    void Update()
    {
		CheckGround();
		Gravity();
		Move();
	}

    private void CheckGround()
    {
		Vector3 checkGround = new Vector3(transform.position.x,
            transform.position.y - characterController.height / 2,
            transform.position.z);
		isGrounded = Physics.Raycast(checkGround, Vector3.down, 0.1f, environmentLayerMask);
    }

    private void Gravity()
	{
		if (isGrounded)
        {	
			velocity.y = -2;
        }
		velocity.y -= gravity;

		characterController.Move(velocity * Time.deltaTime);
    }

	private void Move()
	{
		Vector3 move = transform.right * movementInput.x + transform.forward * movementInput.y;
		characterController.Move(move * moveSpeed * Time.deltaTime);
	}

	public void FireArrow()
	{
		if (hasArrow)
		{
			hasArrow = false;
			arrowTransform.SetParent(null);
			Rigidbody rigidbody = arrowTransform.GetComponent<Rigidbody>();
			rigidbody.isKinematic = false;
			rigidbody.AddForce(transform.forward * arrowImpulse.z + transform.up * arrowImpulse.y, ForceMode.Impulse);
		}
	}
}
