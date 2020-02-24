using UnityEngine;

public interface IEnemy
{
	void Enraged();
	void TriggerBattle(Transform player);
    int Damage(Vector3 hitDirection);
	void Die();
	bool GetFrozen();
}
