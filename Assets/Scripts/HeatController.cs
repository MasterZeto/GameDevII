using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeatController : MonoBehaviour
{
	public GameObject topHeatMask;
	public GameObject bottomHeatMask;
    
	private RectTransform topMaskSize;
	private RectTransform bottomMaskSize;
    void Start()
    {
        topMaskSize = topHeatMask.GetComponent<RectTransform>();
        bottomMaskSize = bottomHeatMask.GetComponent<RectTransform>();
    }

    public void Update() 
    {
    	if(topMaskSize.sizeDelta.y < 40)
    	{
			topMaskSize.sizeDelta = new Vector2(topMaskSize.sizeDelta.x, topMaskSize.sizeDelta.y + .1f);
		}
		if(bottomMaskSize.sizeDelta.y > .1)
		{
			bottomMaskSize.sizeDelta = new Vector2(bottomMaskSize.sizeDelta.x, bottomMaskSize.sizeDelta.y - .1f);
		}
    }

    public void UseHeat(float amount)
    {
    	topMaskSize.sizeDelta = new Vector2(topMaskSize.sizeDelta.x, topMaskSize.sizeDelta.y - ((amount / 100f) * 40f));
    	bottomMaskSize.sizeDelta = new Vector2(bottomMaskSize.sizeDelta.x, bottomMaskSize.sizeDelta.y - ((amount / 100f) * 40f));
    }

	public void SetHeat(float amount)
	{
		topMaskSize.sizeDelta = new Vector2(topMaskSize.sizeDelta.x, (1.0f - (amount / 100f)) * 40f);
    	bottomMaskSize.sizeDelta = new Vector2(bottomMaskSize.sizeDelta.x, ((amount / 100f)) * 40f);
	}
 
}
