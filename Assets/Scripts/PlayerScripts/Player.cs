using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
	[SerializeField] private Animator animator;
	[SerializeField] private Animator reticleAnimator;
	[SerializeField] private Bow bow;
	[SerializeField] private Arrow arrow;
	[SerializeField] private CharacterController characterController;
	[SerializeField] private Material material;
	[SerializeField] private MeshRenderer arrowMeshRenderer;
	[SerializeField] private TrailRenderer dashTrailRenderer;
	[SerializeField] private Text dashAmountText;
	[SerializeField] private LayerMask environmentLayerMask;
	private Coroutine drawBowCoroutine;
	private Coroutine summonArrowCoroutine;
	private const float dashForce = 500f;
	private const float gravity = 20f;
	private const float aimSpeed = 1.5f;
	private const float summonArrowSpeed = 2f;
	private const float walkSpeed = 4f;
	private const float jumpForce = 1f;
	private const float crouchSpeedMultiplier = 0.7f;
	private const float standUpSpeedMultiplier = 1f;
	private float currentSpeedMultiplier = 1f;
	private float firePower;
	private float moveSpeed = 4f;
	private float dashCooldown = 3;
	private float footstepCooldown = 0.32f;
	private int dashAmount = 3;
	private Vector3 velocity;
	private bool isDashing;
	private bool isGrounded;
	private bool isInAir;
	private bool hasArrow = true;
	private bool isCrouching;

	public Vector2 movementInput { get; set; }


    void Update()
    {
		CheckGround();
		Gravity();
		Move();
		DashCooldown();
		Footsteps();
	}

    private void CheckGround()
    {
		Vector3 checkGround = new Vector3(transform.position.x, transform.position.y - characterController.height / 2, transform.position.z);
		isGrounded = Physics.Raycast(checkGround, Vector3.down, 0.5f, environmentLayerMask);
    }

    private void Gravity()
	{
		if (isGrounded && velocity.y < 0)
		{
			if (isInAir)
			{
				AudioManager.Instance.Play("Land");
				isInAir = false;
			}
			velocity.y = -2;
		}
		velocity.y -= gravity * Time.deltaTime;
		characterController.Move(velocity * Time.deltaTime);
    }

	private void Move()
	{
		if (!isDashing)
		{
			Vector3 move = transform.right * movementInput.x + transform.forward * movementInput.y;
			characterController.Move(move * moveSpeed * Time.deltaTime);
		}
	}

	public void Crouch()
	{
		if (isCrouching)
		{
			Vector3 checkCelling = new Vector3(transform.position.x, transform.position.y + characterController.height / 2, transform.position.z);
			bool cantStandUp = Physics.Raycast(checkCelling, Vector3.up, characterController.height, environmentLayerMask);
			if (!cantStandUp)
			{
				characterController.height = 2.0f;
				currentSpeedMultiplier = standUpSpeedMultiplier;

				moveSpeed = walkSpeed * currentSpeedMultiplier;
				isCrouching = !isCrouching;
			}
		}
		else
		{
			characterController.height = 1.0f;
			currentSpeedMultiplier = crouchSpeedMultiplier;

			moveSpeed = walkSpeed * currentSpeedMultiplier;
			isCrouching = !isCrouching;
		}
	}

	public void Jump()
	{
		if (isGrounded)
		{
			AudioManager.Instance.Play("Jump");
			velocity.y = Mathf.Sqrt(jumpForce * 2.0f * gravity);
			isInAir = true;
		}
	}

	public void Dash()
	{
		if (dashAmount > 0)
		{
			AudioManager.Instance.Play("Dash");
			animator.SetTrigger("Dash");
			isDashing = true;
			dashTrailRenderer.emitting = true;

			Vector3 move = transform.right * movementInput.x * dashForce + transform.forward * movementInput.y * dashForce;
			if (move == Vector3.zero)
			{
				move = transform.forward * dashForce;
			}
			characterController.Move(move * Time.deltaTime);
			dashAmount--;
			dashAmountText.text = dashAmount.ToString();
		}
	}

	public void DeactivateIsDashing()
	{
		isDashing = false;
		dashTrailRenderer.emitting = false;
	}

	private void DashCooldown()
	{
		if (dashAmount < 3)
		{
			dashCooldown -= Time.deltaTime;
			if (dashCooldown <= 0)
			{
				dashAmount++;
				AudioManager.Instance.Play("DashRecharge", 1f + (dashAmount / 5f));
				dashCooldown = 3f;
				dashAmountText.text = dashAmount.ToString();
			}
		}
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
		moveSpeed = aimSpeed * currentSpeedMultiplier;

		float elapsedTime = 0f;
		float waitTime = 0.5f;
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
			moveSpeed = walkSpeed * currentSpeedMultiplier;

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
		moveSpeed = summonArrowSpeed * currentSpeedMultiplier;

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
			moveSpeed = walkSpeed * currentSpeedMultiplier;

			hasArrow = true;
			arrowMeshRenderer.shadowCastingMode = ShadowCastingMode.Off;
			bow.EquipArrow();
		}
	}

	private void Footsteps()
	{
		if (movementInput.magnitude > 0 && isGrounded)
		{
			footstepCooldown-= Time.deltaTime;
			if (footstepCooldown <= 0)
			{
				AudioManager.Instance.PlayRandomFromSoundGroup("Footsteps");
				footstepCooldown = 0.32f;
			}
		}
	}
}