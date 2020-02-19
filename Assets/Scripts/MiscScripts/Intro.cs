using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intro : MonoBehaviour
{
    [SerializeField] private PlayerInputSystem playerInputSystem;


    void Awake()
    {
        playerInputSystem.enabled = false;
    }

    public void FadeInSound()
    {
        AudioManager.Instance.FadeIn("IntroLogo");
    }

    public void FadeOutSound()
    {
        AudioManager.Instance.FadeOut("IntroLogo");
    }

    public void EnablePlayerInputSystem()
    {
        playerInputSystem.enabled = true;
    }
}
