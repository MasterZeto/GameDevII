using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFlash : MonoBehaviour
{

	public float flashSpeed = 1f;
	public Color flashColor = new Color(1f, 1f, 1f, 1f);
	public bool flashing = false;
	public Image flashImage;

	void Update ()
	{
		if(flashing) 
		{
			flashImage.color = flashColor;
		} else {
			flashImage.color = Color.Lerp (flashImage.color, Color.clear, flashSpeed * Time.deltaTime);
		}
		flashing = false;
	}

	public void Flash()
	{
		flashing = true;
	}
}
