using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSounds : MonoBehaviour
{
	public AudioSource beepSource;
	public AudioClip beep;

    void playSound() 
    {
    	beepSource.PlayOneShot(beep);
    }
}
