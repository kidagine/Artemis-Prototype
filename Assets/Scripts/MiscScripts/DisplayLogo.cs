using UnityEngine;

public class DisplayLogo : MonoBehaviour
{
    [SerializeField] private Animator displayLogoAnimator;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            AudioManager.Instance.Play("Music");
            displayLogoAnimator.SetTrigger("Display");
        }
    }
}
