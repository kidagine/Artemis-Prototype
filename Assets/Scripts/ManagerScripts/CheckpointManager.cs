using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
	public static CheckpointManager Instance { get; private set; }
	public Vector3 SpawnPosition { get; set; }

	void Awake()
	{
		CheckInstance();
	}

	private void CheckInstance()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(gameObject);
		}
		else
		{
			Instance = this;
		}
	}
}
