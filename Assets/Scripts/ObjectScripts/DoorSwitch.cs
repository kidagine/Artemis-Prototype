using UnityEngine;

public class DoorSwitch : MonoBehaviour, IInteractable
{
    [SerializeField] private Animator doorAnimator;
    [SerializeField] private MeshRenderer meshRenderer;


    public void Interact()
    {
        OpenDoor();
    }

    private void OpenDoor()
    {
        doorAnimator.SetTrigger("Open");
        meshRenderer.material.SetColor("_BaseColor", Color.red);
    }
}
