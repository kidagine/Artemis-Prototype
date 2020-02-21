using UnityEngine;
using UnityEngine.UI;

public class EnemyUI : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;


    void Update()
    {
        Vector3 healthSliderPosition = Camera.main.WorldToScreenPoint(transform.position);
        healthSlider.transform.position = healthSliderPosition;
    }
}
