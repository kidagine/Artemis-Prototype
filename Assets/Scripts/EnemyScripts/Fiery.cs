using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Fiery : MonoBehaviour, IEnemy
{
	[SerializeField] private GameObject explosionPrefab;
	[SerializeField] private GameObject healthPickupPrefab;
	[SerializeField] private Slider healthSlider;
	[SerializeField] private NavMeshAgent navMeshAgent;
	[SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer;
	[SerializeField] private Animator animator;
	private Transform player;
	private readonly int attackPoints = 15;
	private int health = 1;
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
			Debug.Log(dist);
		}
	}

	public int Damage(Vector3 hitDirection)
    {
		animator.SetTrigger("TakeHit");
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
		navMeshAgent.velocity = Vector3.zero;
		isBattleTriggered = false;
		skinnedMeshRenderer.material.SetColor("_BaseColor", Color.gray);
		animator.speed = 0.0f;
	}

    public void Die()
    {
		AudioManager.Instance.Play("EnemyDeath");
		Instantiate(explosionPrefab, new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), Quaternion.identity);
		int dropHealth = Random.Range(0, 10);
		if (dropHealth == 1)
		{
			Instantiate(healthPickupPrefab, new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), Quaternion.identity);
		}
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
		isBattleTriggered = true;
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
			if (!AudioManager.Instance.IsPlaying("MinionAttack"))
			{
				AudioManager.Instance.Play("MinionAttack");
			}
			other.GetComponent<Player>().Damage(attackPoints);
		}
	}

	public bool GetFrozen()
	{
		return isFrozen;
	}
}
