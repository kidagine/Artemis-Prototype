using UnityEngine;

public class Elevator : MonoBehaviour
{
    [SerializeField] private Transform elevatorTransform;
    private int elevatorSpeed = 5;


    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            elevatorTransform.position += elevatorTransform.up * Time.deltaTime * elevatorSpeed;
        }
    }
}
