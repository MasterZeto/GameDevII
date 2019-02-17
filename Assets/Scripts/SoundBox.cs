using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundBox : MonoBehaviour
{

	public AudioSource source;
    public AudioClip punchHitSound;
    
    void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    public void HitSFX()
	{
     	source.PlayOneShot(punchHitSound, 0.3F);
    }

  
}
