using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundBox : MonoBehaviour
{

	public AudioSource source;
    public AudioClip hitSound;
    
    void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    public void HitSFX()
	{
     	source.PlayOneShot(hitSound, 0.3F);
    }

  
}
