using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundBox : MonoBehaviour
{

	public AudioSource source;
    public AudioClip hitSound;
    public AudioClip missSound;

    
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

  
}
