using UnityEngine;

public class DoorSwitch : MonoBehaviour, IInteractable
{
    [SerializeField] private Animator doorAnimator;
    [SerializeField] private MeshRenderer meshRenderer;
    private bool isOpen;


    public void Interact()
    {
        if (!isOpen)
        {
            AudioManager.Instance.Play("DoorOn");
            OpenDoor();
            isOpen = true;
        }
    }

    private void OpenDoor()
    {
        AudioManager.Instance.Play("GateOpen");
        doorAnimator.SetTrigger("Open");
        meshRenderer.material.SetColor("_BaseColor", Color.red);
    }
}
