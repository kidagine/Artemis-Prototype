using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	[SerializeField] private GameObject uiEnterText;
	[SerializeField] private Text healthText;
	[SerializeField] private Text dashText;
	[SerializeField] private Text healthReceivedText;
	[SerializeField] private Text healthReceivedOutlineText;
	[SerializeField] private Slider healthSlider;
	[SerializeField] private Slider dashSlider;
	[SerializeField] private Animator playerDamagedAnimator;
	[SerializeField] private Animator healthReceivedAnimator;


	public static UIManager Instance { get; private set; }

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

	public void SetHealth(int health, int healthReceived = 0)
	{
		if (healthReceived != 0)
		{
			healthReceivedAnimator.SetTrigger("ReceivedHealth");
			healthReceivedText.text = $"+{healthReceived}HP";
			healthReceivedOutlineText.text = $"+{healthReceived}HP";
		}
		healthText.text = health.ToString();
		healthSlider.value = health;
	}

	public void SetDash(int dash)
	{
		dashText.text = dash.ToString();
		dashSlider.value = dash;
	}

	public void ShowEnter()
	{
		uiEnterText.SetActive(true);
	}

	public void HideEnter()
	{
		uiEnterText.SetActive(false);
	}

	public void SetEnemyHealth(float health, Slider healthSlider)
	{
		healthSlider.value = health;
	}

	public void PlayerDamaged()
	{
		playerDamagedAnimator.SetTrigger("Damage");
	}
}
