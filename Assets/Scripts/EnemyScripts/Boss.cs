using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour, IEnemy
{
    [SerializeField] private Slider healthSlider;
    [SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer;
    [SerializeField] private GameObject explosionPrefab;
    private bool isBattleTriggered;
    private int health = 2;
    private bool isFrozen;


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
        skinnedMeshRenderer.material.SetColor("_BaseColor", Color.gray);
        //animator.speed = 0.0f;
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
        isBattleTriggered = true;
    }
}
