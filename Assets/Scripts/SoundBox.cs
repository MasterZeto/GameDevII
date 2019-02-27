using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundBox : MonoBehaviour
{

	public AudioSource source;
    public AudioLowPassFilter muffle;
    public AudioClip hitSound;
    public AudioClip missSound;
    public AudioClip slowSound;
    public AudioLowPassFilter low_pass;

    GameObject speaker;

    void Awake()
    {
        source = GetComponent<AudioSource>();
        low_pass = GameObject.Find("Center Light").GetComponent<AudioLowPassFilter>();
    }


    public void HitSFX()
	{
     	source.PlayOneShot(hitSound, 0.3F);
    }

    public void MissSFX()
    {
        source.PlayOneShot(missSound, 0.3F);
    }

    public void TimeSlowSFX()
    {
        source.PlayOneShot(slowSound, 0.3F);
        low_pass.enabled = true;    
    }

    public void TimeSlowStop()
    {
        low_pass.enabled = false;    
    }

  
}
