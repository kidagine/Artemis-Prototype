using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    [SerializeField] private float timeUntilDestroy;

    void Start()
    {
        Destroy(gameObject, timeUntilDestroy);
    }
}
