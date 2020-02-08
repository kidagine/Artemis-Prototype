using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class Player : MonoBehaviour
{
	[SerializeField] private Animator animator;
	[SerializeField] private Bow bow;
	[SerializeField] private Arrow arrow;
	[SerializeField] private CharacterController characterController;
	[SerializeField] private GameObject reticleGameObject;
	[SerializeField] private Transform cameraTransform;
	[SerializeField] private Material material;
	[SerializeField] private MeshRenderer arrowMeshRenderer;
	[SerializeField] private LayerMask environmentLayerMask;
	private const float aimSpeed = 1.5f;
	private const float walkSpeed = 4f;
	private const float gravity = 0.1f;
	private const float pickableRange = 1.5f;
	private float moveSpeed = 4f;
	private float firePower;
	private Vector3 velocity;
	private bool isGrounded;
	private bool hasArrow = true;

	private float t;
	private Coroutine c;
	public Vector2 movementInput { get; set; }
	public bool isDrawingBow { get; set; }


    void Update()
    {
		CheckGround();
		Gravity();
		Move();
		SummonArrow();
		PickUpArrow();
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
			if (Input.GetMouseButtonDown(1))
			{
				AudioManager.Instance.Play("SummonArrow");
			}
			if (Input.GetMouseButton(1))
			{
				animator.SetBool("IsSummoning", true);
				t += 1f * Time.deltaTime;
				float glowValue = Mathf.Lerp(0, 0.05f, t);
				material.SetFloat("_GlowPower", glowValue); arrow.Summon(true, transform);
			}
			else if (Input.GetMouseButtonUp(1))
			{
				AudioManager.Instance.Stop("SummonArrow");
				t = 0;
				animator.SetBool("IsSummoning", false);
				material.SetFloat("_GlowPower", 0f);
				arrow.Summon(false, transform);
			}
		}
		else
		{
			animator.SetBool("IsSummoning", false);
		}
	}

	public void GrabArrow()
	{
		if (!hasArrow)
		{
			AudioManager.Instance.Stop("SummonArrow");
			AudioManager.Instance.Play("CatchArrow");
			arrowMeshRenderer.shadowCastingMode = ShadowCastingMode.Off;
			animator.SetTrigger("Catch");
			t = 0;
			material.SetFloat("_GlowPower", 0f);
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
			yield return new WaitForSeconds(0.01f);
		}
	}

	public void FireArrow()
	{
		if (hasArrow)
		{
			arrowMeshRenderer.shadowCastingMode = ShadowCastingMode.On;
			hasArrow = false;
			AudioManager.Instance.Play("FireBow");
			animator.SetBool("IsCharging", false);
			reticleGameObject.GetComponent<Animator>().SetBool("IsCharging", false);
			moveSpeed = walkSpeed;
			bow.FireArrow(firePower);

			StopCoroutine(c);
			firePower = 0f;
			animator.SetFloat("FirePower", firePower);
		}
	}

	private void PickUpArrow()
	{
		Physics.Raycast(cameraTransform.position, cameraTransform.forward, out RaycastHit hit, pickableRange);
		if (hit.collider != null)
		{
			Arrow arrow = hit.collider.GetComponent<Arrow>();
			if (arrow != null)
			{
				if (Input.GetKeyDown(KeyCode.F))
				{
					arrow.Pickup(GetComponent<Player>());
				}
			}
		}
	}
}
