using UnityEngine;

public class Elevator : MonoBehaviour
{
    [SerializeField] private Transform elevatorTransform;
    [SerializeField] private Animator elevatorObstacleAnimator;
    private int elevatorSpeed = 5;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            AudioManager.Instance.Play("Elevator");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            elevatorTransform.position += elevatorTransform.up * Time.deltaTime * elevatorSpeed;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            AudioManager.Instance.Stop("Elevator");
            elevatorObstacleAnimator.SetTrigger("Open");
        }
    }
}
