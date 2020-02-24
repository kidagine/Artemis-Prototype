using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class TurtleShell : MonoBehaviour, IEnemy
{
	[SerializeField] private GameObject explosionPrefab;
	[SerializeField] private Rigidbody enemyRigidbody;
	[SerializeField] private Slider healthSlider;
	[SerializeField] private NavMeshAgent navMeshAgent;
	[SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer;
	[SerializeField] private Animator animator;
	private Transform player;
	private readonly int attackPoints = 5;
	private int health = 2;
	private bool isBattleTriggered;
	private bool isFrozen;
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

	public int Damage(Vector3 hitDirection)
    {
		health--;
		UIManager.Instance.SetEnemyHealth(health, healthSlider);
		if (health <= 0)
		{
			Freeze();
		}
		else
		{
			StartCoroutine(DamageEffect());
		}
		return health;
	}

	private void Freeze()
	{
		isFrozen = true;
		navMeshAgent.isStopped = true;
		isBattleTriggered = false;
		skinnedMeshRenderer.material.SetColor("_BaseColor", Color.gray);
		animator.speed = 0.0f;
		enemyRigidbody.isKinematic = true;
	}

    public void Die()
    {
		AudioManager.Instance.Play("EnemyDeath");
		Instantiate(explosionPrefab, new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), Quaternion.identity);
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

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			other.GetComponent<Player>().Damage(attackPoints);
		}
	}

	public bool GetFrozen()
	{
		return isFrozen;
	}
}
