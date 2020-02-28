using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour, IEnemy
{
    [SerializeField] private Slider healthSlider;
    [SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer1, skinnedMeshRenderer2, skinnedMeshRenderer3;
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private GameObject outro;
    [SerializeField] private GameObject enemyProjectilePrefab;
    [SerializeField] private Transform eyePoint;
    [SerializeField] private Transform parentPivot;
    [SerializeField] private Animator animator;
    private Transform player;
    private const float attackCooldown = 0.6f;
    private float currentAttackCooldown = 2f;
    private bool isBattleTriggered;
    private int health = 15;
    private bool isFrozen;


    private void Update()
    {
        if (isBattleTriggered)
        {
            currentAttackCooldown -= Time.deltaTime;
            if (currentAttackCooldown <= 0)
            {
                Attack();
                currentAttackCooldown = attackCooldown;
            }
            parentPivot.LookAt(player);
        }
    }

    private void Attack()
    {
        GameObject enemyProjectile = Instantiate(enemyProjectilePrefab, eyePoint.position, Quaternion.identity);
        enemyProjectile.transform.LookAt(player);
    }

    public int Damage(Vector3 hitDirection)
    {
        //animator.SetTrigger("TakeHit");
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
        isBattleTriggered = false;
        skinnedMeshRenderer1.material.SetColor("_BaseColor", Color.gray);
        skinnedMeshRenderer2.material.SetColor("_BaseColor", Color.gray);
        skinnedMeshRenderer3.material.SetColor("_BaseColor", Color.gray);
        animator.speed = 0.0f;
    }

    IEnumerator DamageEffect()
    {
        Color defaultColor = skinnedMeshRenderer1.material.color;
        skinnedMeshRenderer1.material.SetColor("_BaseColor", Color.red);
        skinnedMeshRenderer2.material.SetColor("_BaseColor", Color.red);
        skinnedMeshRenderer3.material.SetColor("_BaseColor", Color.red);
        yield return new WaitForSeconds(0.3f);
        skinnedMeshRenderer1.material.SetColor("_BaseColor", defaultColor);
        skinnedMeshRenderer2.material.SetColor("_BaseColor", defaultColor);
        skinnedMeshRenderer3.material.SetColor("_BaseColor", defaultColor);
    }

    public void Die()
    {
        StartCoroutine(BossDeathExplosions());
    }

    IEnumerator BossDeathExplosions()
    {
        AudioManager.Instance.Play("EnemyDeath");
        Instantiate(explosionPrefab, new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), Quaternion.identity);
        yield return new WaitForSeconds(0.3f);
        AudioManager.Instance.Play("EnemyDeath");
        Instantiate(explosionPrefab, new Vector3(transform.position.x + 5, transform.position.y + 2.5f, transform.position.z), Quaternion.identity);
        yield return new WaitForSeconds(0.3f);
        AudioManager.Instance.Play("EnemyDeath");
        Instantiate(explosionPrefab, new Vector3(transform.position.x - 2.5f, transform.position.y + 3.2f, transform.position.z + 0.4f), Quaternion.identity);
        AudioManager.Instance.Play("EnemyDeath");
        Instantiate(explosionPrefab, new Vector3(transform.position.x - 3.5f, transform.position.y + 1.5f, transform.position.z - 1), Quaternion.identity);
        yield return new WaitForSeconds(0.3f);
        AudioManager.Instance.Play("EnemyDeath");
        Instantiate(explosionPrefab, new Vector3(transform.position.x + 4.3f, transform.position.y + 2.2f, transform.position.z + 0.8f), Quaternion.identity);
        AudioManager.Instance.Play("EnemyDeath");
        Instantiate(explosionPrefab, new Vector3(transform.position.x, transform.position.y + 2, transform.position.z + 0.1f), Quaternion.identity);
        AudioManager.Instance.Play("EnemyDeath");
        Instantiate(explosionPrefab, new Vector3(transform.position.x - 3, transform.position.y + 1.5f, transform.position.z), Quaternion.identity);

        healthSlider.enabled = false;
        Destroy(gameObject);
        outro.SetActive(true);
        yield return new WaitForSeconds(1f);
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
        this.player = player;
        animator.SetBool("HasDetectedPlayer", true);
        isBattleTriggered = true;
    }
}
