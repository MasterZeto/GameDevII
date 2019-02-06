using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Parts
{
    LEFT_ARM,
    RIGHT_ARM,
    LEFT_LEG,
    RIGHT_LEG
};

/* Do the component locking thing */
public class PartSelector : MonoBehaviour
{
    [SerializeField] Parts part;
    [SerializeField] Button prev_button;
    [SerializeField] Button next_button;
    GameObject[] parts;
    int selected = 0;

    void Start()
    {
        prev_button.onClick.AddListener(() => { Prev(); });
        next_button.onClick.AddListener(() => { Next(); });
        
        parts = new GameObject[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            parts[i] = transform.GetChild(i).gameObject;
            if (i != 0) parts[i].SetActive(false);
        }
    }

    [ContextMenu("Next")]
    public void Next() 
    {
        parts[selected].SetActive(false);
        selected++;
        if (selected >= parts.Length) selected -= parts.Length;
        parts[selected].SetActive(true);
        UpdateCustomizationManager(selected);
    }

    [ContextMenu("Prev")]
    public void Prev()
    {
        parts[selected].SetActive(false);
        selected--;
        if (selected < 0) selected += parts.Length;
        parts[selected].SetActive(true);
        UpdateCustomizationManager(selected);
    }

    void UpdateCustomizationManager(int type)
    {
        switch (part)
        {
            case Parts.LEFT_ARM: 
                CustomizationManager.left_arm_part = type;
                break;
            case Parts.LEFT_LEG: 
                CustomizationManager.left_leg_part = type;
                break;
            case Parts.RIGHT_ARM: 
                CustomizationManager.right_arm_part = type;
                break;
            case Parts.RIGHT_LEG: 
                CustomizationManager.right_leg_part = type;
                break;
        }
    }
}
