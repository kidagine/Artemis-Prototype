using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
	[SerializeField] private Animator animator;
	[SerializeField] private Bow bow;
	[SerializeField] private CharacterController characterController;
	[SerializeField] private LayerMask environmentLayerMask;
	private const float moveSpeed = 2f;
	private const float gravity = 0.1f;
	private float firePower;
	private Vector3 velocity;
	private bool isGrounded;
	private bool hasArrow = true;

	public Vector2 movementInput { get; set; }
	public bool isDrawingBow { get; set; }


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

	public void DrawBow()
	{
		StartCoroutine(DrawBowNum());
	}

	private IEnumerator DrawBowNum()
	{
		AudioManager.Instance.Play("DrawBow");
		float ratio = 0f;
		float startValue = 0;
		float endValue = 1;
		while (ratio <= 1)
		{
			ratio += 0.02f;
			firePower = Mathf.Lerp(startValue, endValue, ratio);
			animator.SetFloat("FirePower", firePower);
			yield return new WaitForSeconds(0.01f);
		}
	}

	public void FireArrow()
	{
		if (hasArrow)
		{
			hasArrow = false;
			AudioManager.Instance.Play("FireBow");
			bow.FireArrow(firePower);
			firePower = 0f;
		}
	}
}
