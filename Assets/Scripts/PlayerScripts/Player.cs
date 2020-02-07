using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
	[SerializeField] private Animator animator;
	[SerializeField] private Bow bow;
	[SerializeField] private Arrow arrow;
	[SerializeField] private CharacterController characterController;
	[SerializeField] private GameObject reticleGameObject;
	[SerializeField] private LayerMask environmentLayerMask;
	private const float aimSpeed = 1.5f;
	private const float walkSpeed = 4f;
	private const float gravity = 0.1f;
	private float moveSpeed = 4f;
	private float firePower;
	private Vector3 velocity;
	private bool isGrounded;
	private bool hasArrow = true;

	private Coroutine c;
	public Vector2 movementInput { get; set; }
	public bool isDrawingBow { get; set; }


    void Update()
    {
		CheckGround();
		Gravity();
		Move();
		SummonArrow();
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

	private void SummonArrow()
	{
		if (!hasArrow)
		{
			if (Input.GetMouseButton(1))
			{
				arrow.Summon(true, transform);
			}
			else if (Input.GetMouseButtonUp(1))
			{
				arrow.Summon(false, transform);
			}
		}
	}

	public void GrabArrow()
	{
		if (!hasArrow)
		{
			hasArrow = true;
			bow.EquipArrow();
		}
	}

	public void DrawBow()
	{
		if (hasArrow)
		{
			c = StartCoroutine(DrawBowNum());
		}
	}

	private IEnumerator DrawBowNum()
	{
		AudioManager.Instance.Play("DrawBow");
		reticleGameObject.GetComponent<Animator>().SetBool("IsCharging", true);
		moveSpeed = aimSpeed;
		animator.SetBool("IsCharging", true);

		float ratio = 0f;
		float startValue = 0;
		float endValue = 1;
		while (ratio <= 1)
		{
			ratio += 0.02f;
			firePower = Mathf.Lerp(startValue, endValue, ratio);
			animator.SetFloat("FirePower", firePower);
			Debug.Log("s");
			yield return new WaitForSeconds(0.01f);
		}
	}

	public void FireArrow()
	{
		if (hasArrow)
		{
			hasArrow = false;
			AudioManager.Instance.Play("FireBow");
			animator.SetBool("IsCharging", false);
			reticleGameObject.GetComponent<Animator>().SetBool("IsCharging", false);
			moveSpeed = walkSpeed;
			bow.FireArrow(firePower);

			Debug.Log("stop");
			StopCoroutine(c);
			firePower = 0f;
			animator.SetFloat("FirePower", firePower);
		}
	}
}
