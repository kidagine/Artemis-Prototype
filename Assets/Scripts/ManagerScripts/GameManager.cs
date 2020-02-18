using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	[SerializeField] BattleTrigger[] battleTriggers;

	public static GameManager Instance { get; private set; }

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
