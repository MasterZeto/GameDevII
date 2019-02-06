using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechInstantiate : MonoBehaviour
{
    GameObject[] left_arm_parts  = new GameObject[PartTypes.NUM_TYPES];
    GameObject[] left_leg_parts  = new GameObject[PartTypes.NUM_TYPES];
    GameObject[] right_arm_parts = new GameObject[PartTypes.NUM_TYPES];
    GameObject[] right_leg_parts = new GameObject[PartTypes.NUM_TYPES];
    
    void Awake()
    {
        GrabParts(transform.Find("Left Arm"), left_arm_parts);
        GrabParts(transform.Find("Left Leg"), left_leg_parts);
        GrabParts(transform.Find("Right Arm"), right_arm_parts);
        GrabParts(transform.Find("Right Leg"), right_leg_parts);
    }

    void Start()
    {
        Instantiate(
            CustomizationManager.left_arm_part,
            CustomizationManager.left_leg_part,
            CustomizationManager.right_arm_part,
            CustomizationManager.right_leg_part
        );
    }

    void GrabParts(Transform container, GameObject[] parts)
    {
        for (int i = 0; i < container.childCount; ++i)
        {
            parts[i] = container.GetChild(i).gameObject;
        }
    }

    void SetActivePart(int part, GameObject[] parts)
    {
        for (int i = 0; i < parts.Length; ++i)
        {
            parts[i].SetActive( i == part );
        }
    }

    public void Instantiate(
        int left_arm_part, 
        int left_leg_part, 
        int right_arm_part,
        int right_leg_part
    )
    {
        SetActivePart(left_arm_part,  left_arm_parts);
        SetActivePart(left_leg_part,  left_leg_parts);
        SetActivePart(right_arm_part, right_arm_parts);
        SetActivePart(right_leg_part, right_leg_parts);
    }

}
