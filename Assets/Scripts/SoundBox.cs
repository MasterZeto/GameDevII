using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundBox : MonoBehaviour
{

	public AudioSource playerSource;
    public AudioLowPassFilter muffle;
    public AudioClip hitSound;
    public AudioClip missSound;
    public AudioClip slowSound;

    public AudioSource musicSource;
    public AudioLowPassFilter low_pass;

    GameObject speaker;

    void Awake()
    {
        playerSource = GetComponent<AudioSource>();
        musicSource = GameObject.Find("Center Light").GetComponent<AudioSource>();
        low_pass = GameObject.Find("Center Light").GetComponent<AudioLowPassFilter>();
    }


    public void HitSFX()
	{
     	playerSource.PlayOneShot(hitSound, 0.3F);
    }

    public void MissSFX()
    {
        playerSource.PlayOneShot(missSound, 0.3F);
    }

    public void TimeSlowSFX()
    {
        playerSource.PlayOneShot(slowSound, 0.3F);
        low_pass.enabled = true;    
        musicSource.pitch = 0.5F;
    }

    public void TimeSlowStop()
    {
        low_pass.enabled = false;   
        musicSource.pitch = 1.0F; 
    }

  
}
