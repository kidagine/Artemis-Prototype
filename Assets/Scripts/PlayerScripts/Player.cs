using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class Player : MonoBehaviour
{
	[SerializeField] private Animator animator;
	[SerializeField] private Animator reticleAnimator;
	[SerializeField] private Bow bow;
	[SerializeField] private Arrow arrow;
	[SerializeField] private CharacterController characterController;
	[SerializeField] private Material material;
	[SerializeField] private MeshRenderer arrowMeshRenderer;
	[SerializeField] private LayerMask environmentLayerMask;
	private Coroutine drawBowCoroutine;
	private Coroutine summonArrowCoroutine;
	private const float aimSpeed = 1.5f;
	private const float walkSpeed = 4f;
	private const float gravity = 0.1f;
	private float moveSpeed = 4f;
	private float firePower;
	private Vector3 velocity;
	private bool isGrounded;
	private bool hasArrow = true;

	private float t;
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
		if (hasArrow)
		{
			drawBowCoroutine = StartCoroutine(DrawBowCoroutine());
		}
	}

	private IEnumerator DrawBowCoroutine()
	{
		AudioManager.Instance.Play("DrawBow");
		reticleAnimator.SetBool("IsCharging", true);
		animator.SetBool("IsCharging", true);
		moveSpeed = aimSpeed;

		float elapsedTime = 0f;
		float waitTime = 0.7f;
		float startValue = 0;
		float endValue = 1;
		while (elapsedTime < waitTime)
		{
			firePower = Mathf.Lerp(startValue, endValue, (elapsedTime / waitTime));
			animator.SetFloat("FirePower", firePower);
			elapsedTime += Time.deltaTime;
			yield return null;
		}
		firePower = endValue;
		yield return null;
	}

	public void FireArrow()
	{
		if (hasArrow)
		{
			StopCoroutine(drawBowCoroutine);
			AudioManager.Instance.Play("FireBow");
			reticleAnimator.SetBool("IsCharging", false);
			animator.SetBool("IsCharging", false);
			moveSpeed = walkSpeed;

			arrowMeshRenderer.shadowCastingMode = ShadowCastingMode.On;
			hasArrow = false;
			bow.FireArrow(firePower);

			firePower = 0f;
			animator.SetFloat("FirePower", firePower);
		}
	}

	public void StartSummonArrow()
	{
		if (!hasArrow)
		{
			summonArrowCoroutine = StartCoroutine(SummonArrowCoroutine());
		}
	}

	IEnumerator SummonArrowCoroutine()
	{
		AudioManager.Instance.Play("SummonArrow");
		animator.SetBool("IsSummoning", true);
		arrow.SetSummon(true);
		moveSpeed = walkSpeed;

		float elapsedTime = 0f;
		float waitTime = 1.2f;
		float startValue = 0;
		float endValue = 0.05f;
		while (elapsedTime < waitTime)
		{
			float glowValue = Mathf.Lerp(startValue, endValue, (elapsedTime / waitTime));
			material.SetFloat("_GlowPower", glowValue);
			elapsedTime += Time.deltaTime;
			yield return null;
		}
		yield return null;
	}

	public void StopSummonArrow()
	{
		if (!hasArrow)
		{
			StopCoroutine(summonArrowCoroutine);
			AudioManager.Instance.Stop("SummonArrow");
			animator.SetBool("IsSummoning", false);
			material.SetFloat("_GlowPower", 0f);
			arrow.SetSummon(false);
		}
	}

	public void GrabArrow()
	{
		if (!hasArrow)
		{
			StopSummonArrow();
			AudioManager.Instance.Play("CatchArrow");
			animator.SetTrigger("Catch");
			
			hasArrow = true;
			arrowMeshRenderer.shadowCastingMode = ShadowCastingMode.Off;
			bow.EquipArrow();
		}
	}
}