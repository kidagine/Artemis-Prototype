using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    [SerializeField] private GameObject pickUpExplosionPrefab;
    private readonly int healthAmount = 20;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            bool isHealed = other.gameObject.GetComponent<Player>().Heal(healthAmount);
            if (isHealed)
            {
                Instantiate(pickUpExplosionPrefab, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }
    }
}
