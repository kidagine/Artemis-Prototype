using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Lich : MonoBehaviour, IEnemy
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform staffProjectile;
    [SerializeField] private GameObject enemyProjectilePrefab;
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private GameObject healthPickupPrefab;
    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] private Animator animator;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer;
    private bool isBattleTriggered;
    private int health = 2;
    private float dist;
    private float fireDist;
    private bool isFrozen;


    void Update()
    {
        if (isBattleTriggered)
        {
            navMeshAgent.SetDestination(player.position);
            dist = navMeshAgent.remainingDistance;

            if (dist != Mathf.Infinity && dist < fireDist)
            {
                isBattleTriggered = false;
                navMeshAgent.isStopped = true;
                navMeshAgent.velocity = Vector3.zero;
                animator.SetTrigger("Attack");
            }
        }
        else
        {
            transform.LookAt(player);
        }
    }

    public void Attack()
    {
        animator.SetBool("HasDetectedPlayer", false);
        if (!AudioManager.Instance.IsPlaying("MinionAttack"))
        {
            AudioManager.Instance.Play("MinionAttack");
        }
        GameObject enemyProjectile = Instantiate(enemyProjectilePrefab, staffProjectile.position, Quaternion.identity);
        enemyProjectile.transform.LookAt(player);
        fireDist = Random.Range(8, 22);
        StartCoroutine(RandomlyTriggerBattle());
    }

    IEnumerator RandomlyTriggerBattle()
    {
        float randomAwaitTime = Random.Range(1, 2.5f);
        yield return new WaitForSeconds(randomAwaitTime);
        animator.SetBool("HasDetectedPlayer", true);
        navMeshAgent.isStopped = false;
        isBattleTriggered = true;
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

    IEnumerator DamageEffect()
    {
        Color defaultColor = skinnedMeshRenderer.material.color;
        skinnedMeshRenderer.material.SetColor("_BaseColor", Color.red);
        yield return new WaitForSeconds(0.3f);
        skinnedMeshRenderer.material.SetColor("_BaseColor", defaultColor);
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

    public void Enraged()
    {
        throw new System.NotImplementedException();
    }

    public bool GetFrozen()
    {
        return isFrozen;
    }

    public void TriggerBattle(Transform player)
    {
        fireDist = Random.Range(8, 22);
        this.player = player;
        isBattleTriggered = true;
        animator.SetBool("HasDetectedPlayer", true);
    }
}
