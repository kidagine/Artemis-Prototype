using UnityEngine;

public class BattleTrigger : MonoBehaviour
{
	[SerializeField] private Transform[] enemies;


	private void OnTriggerEnter(Collider other)
	{
		Player player = other.GetComponent<Player>();
		if (player != null)
		{
			foreach (Transform enemy in enemies)
			{
				if (enemy != null)
				{
					enemy.GetComponent<IEnemy>().TriggerBattle(other.transform);
				}
			}
		}
	}
}
