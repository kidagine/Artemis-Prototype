using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
	[SerializeField] private GameObject projectileExplosionPrefab;
    private readonly int speed = 1400;
	private readonly int attackPoints = 20;

		
    void Start()
    {
        transform.GetComponent<Rigidbody>().AddForce(transform.forward * speed);
    }

	private void OnCollisionEnter(Collision other)
	{
		if (!other.gameObject.CompareTag("Enemy"))
		{
			AudioManager.Instance.Play("ProjectileExplosion");
			if (other.gameObject.CompareTag("Player"))
			{
				if (!AudioManager.Instance.IsPlaying("MinionAttack"))
				{
					AudioManager.Instance.Play("MinionAttack");
				}
				other.collider.GetComponent<Player>().Damage(attackPoints);
			}
			Instantiate(projectileExplosionPrefab, transform.position, Quaternion.identity);
			Destroy(gameObject);
		}
	}
}
