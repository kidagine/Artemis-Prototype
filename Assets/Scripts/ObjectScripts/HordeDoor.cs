using UnityEngine;

public class HordeDoor : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Animator doorAnimator;
    [SerializeField] private GameObject spawnRound;
    [SerializeField] private int hordeAmount;
    [SerializeField] private int rounds;


    public void HordeNotify()
    {
        hordeAmount--;
        if (rounds != 0 && hordeAmount <= 8)
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
        spawnRound.SetActive(true);
        for (int i = 0; i < spawnRound.transform.childCount; i++)
        {
            spawnRound.transform.GetChild(i).GetComponent<IEnemy>().TriggerBattle(player);
        }
        Debug.Log("new round");
    }
}
