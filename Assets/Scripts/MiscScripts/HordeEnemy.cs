using UnityEngine;

public class HordeEnemy : MonoBehaviour
{
    [SerializeField] private HordeDoor hordeDoor;

    private void OnDestroy()
    {
        hordeDoor.HordeNotify();
    }
}
