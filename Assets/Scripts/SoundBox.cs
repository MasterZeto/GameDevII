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

    void Awake()
    {
        source = GetComponent<AudioSource>();
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
        GameObject.Find("Center Light").GetComponent<AudioLowPassFilter>().enabled = true;    
    }

    public void TimeSlowStop()
    {
        GameObject.Find("Center Light").GetComponent<AudioLowPassFilter>().enabled = false;    
    }

  
}
