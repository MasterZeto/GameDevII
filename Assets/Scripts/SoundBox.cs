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

    GameObject speaker = GameObject.Find("Center Light");

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
        if(speaker!=null){
            speaker.GetComponent<AudioLowPassFilter>().enabled = true;  
        }  
    }

    public void TimeSlowStop()
    {
        if(speaker!=null){
            speaker.GetComponent<AudioLowPassFilter>().enabled = false;  
        }  
    }

  
}
