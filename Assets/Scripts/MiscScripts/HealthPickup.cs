using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    private readonly int healthAmount = 20;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<Player>().Heal(healthAmount);
            Destroy(gameObject);
        }
    }
}
