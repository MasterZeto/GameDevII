using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* Do the component locking thing */
public class PartSelector : MonoBehaviour
{
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
    }

    [ContextMenu("Prev")]
    public void Prev()
    {
        parts[selected].SetActive(false);
        selected--;
        if (selected < 0) selected += parts.Length;
        parts[selected].SetActive(true);
    }
}
