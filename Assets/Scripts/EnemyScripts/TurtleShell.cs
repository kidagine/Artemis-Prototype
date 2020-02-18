using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class TurtleShell : MonoBehaviour, IEnemy
{
	[SerializeField] private NavMeshAgent navMeshAgent;
	[SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer;
	[SerializeField] private Animator animator;
	private Transform player;
	private const float damagedForce = 300f;
	private int health = 2;
	private bool isBattleTriggered;
	private float dist;

	private void Update()
	{
		if (isBattleTriggered)
		{
			navMeshAgent.SetDestination(player.position);
			dist = navMeshAgent.remainingDistance;

			if (dist != Mathf.Infinity && dist < 1.5f)
			{
				animator.SetTrigger("Attack");
			}
		}
	}

	public void Damage(Vector3 hitDirection)
    {
		StartCoroutine(DamageEffect());
		GetComponent<Rigidbody>().AddForce(hitDirection * damagedForce * 2);
		health--;
		if (health <= 0)
		{
			Die(hitDirection);
		}
	}

    private void Die(Vector3 hitDirection)
    {
		Destroy(gameObject);
    }

	IEnumerator DamageEffect()
	{
		Color defaultColor = skinnedMeshRenderer.material.color;
		skinnedMeshRenderer.material.SetColor("_BaseColor", Color.red);
		yield return new WaitForSeconds(0.3f);
		skinnedMeshRenderer.material.SetColor("_BaseColor", defaultColor);
	}

	public void TriggerBattle(Transform player)
	{
		this.player = player;
		animator.SetBool("HasDetectedPlayer", true);
	}

	public void Enraged()
	{
		isBattleTriggered = true;
	}
}
