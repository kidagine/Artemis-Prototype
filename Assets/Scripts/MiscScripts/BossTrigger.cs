using UnityEngine;

public class BossTrigger : MonoBehaviour
{
    [SerializeField] private GameObject bossHealth;
    [SerializeField] private Transform boss;


    private void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponent<Player>();
        if (player != null)
        {
            bossHealth.SetActive(true);
            boss.GetComponent<IEnemy>().TriggerBattle(other.transform);
            gameObject.SetActive(false);
        }
    }
}
