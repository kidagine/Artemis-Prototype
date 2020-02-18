using UnityEngine;

public interface IEnemy
{
	void Enraged();
	void TriggerBattle(Transform player);
    void Damage(Vector3 hitDirection);
}
