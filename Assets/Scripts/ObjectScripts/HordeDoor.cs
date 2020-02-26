using UnityEngine;

public class HordeDoor : MonoBehaviour
{
    [SerializeField] private Animator doorAnimator;
    [SerializeField] private int hordeAmount;
    [SerializeField] private int rounds;


    public void HordeNotify()
    {
        hordeAmount--;
        if (rounds != 0 && hordeAmount <= 1)
        {
            rounds--;
            SpawnRoundHorde();
        }
        else if (rounds == 0 && hordeAmount <= 0)
        {
            OpenDoor();
        }
    }

    private void OpenDoor()
    {
        AudioManager.Instance.Play("GateOpen");
        doorAnimator.SetTrigger("Open");
    }

    private void SpawnRoundHorde()
    {
        Debug.Log("new round");
    }
}
