using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Sound
{
   Lobby_BGM,
   Game_BGM,
   Gover_BGM,
   BtnClick,
   Pet_pop,
   Eat_Reverse,
   T_in,
   T_Out,
   Wall,
   Walk1,
   Walk2,
   Devil_wing
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        bgm.volume = PlayerPrefs.GetFloat("bgmV");
        source.volume = PlayerPrefs.GetFloat("effectV");
        wing.volume = PlayerPrefs.GetFloat("effectV");
    }

   public AudioSource source;
    public AudioSource bgm;
    public AudioSource wing;
    public AudioClip[] clips;

    public void SetBGMVolume(float bgmV)
    {
        bgm.volume = bgmV;
    }

    public void SetEffectVolume(float effectV)
    {
        source.volume = effectV;
        wing.volume = effectV;
    }



    public void ClickBtnSound()
    {
        source.PlayOneShot(clips[(int)Sound.BtnClick]);
    }

    public void BGM(int num)
    {
        bgm.clip = clips[num];
        bgm.Play();
    }

   public void SoundOneShot(Sound sound)
    {
        source.PlayOneShot(clips[(int)sound]);
    }

    public void WalkSound1()
    {
        source.PlayOneShot(clips[(int)Sound.Walk2]);

    }

    public void WalkSound2()
    {

        source.PlayOneShot(clips[(int)Sound.Walk2]);
    }

    public void WingSound(float volume)
    {
        wing.volume = volume;
        wing.PlayOneShot(clips[(int)Sound.Devil_wing]);
    }
}
