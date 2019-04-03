using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void EndCallback();

public class PortraitDialogue : MonoBehaviour
{
    [System.Serializable]
    public struct PortraitDialogueBlock
    {
        public int index;
        public List<string> dialogue;
    }

    [SerializeField] List<PortraitDialogueBlock> text;
    [SerializeField] List<Typewriter> typewriters;

    bool printing = false;
    bool next = false;
    int index = 0;

    EndCallback callback = null;

    [ContextMenu("Start Dialogue")]
    public void StartDialogue()
    {
        printing = true;
        next = false;
        index = 0;

        DisplayBlock(index);
    }

    public void StartDialogue(EndCallback callback)
    {
        printing = true;
        next = false;
        index = 0;
        this.callback = callback;

        DisplayBlock(index);
    }

    void Update()
    {
        if (printing && next)
        {
            next = false;
            DisplayBlock(index);
        }
    }

    void DisplayBlock(int i)
    {
        typewriters[text[i].index].StartTypewriter(
            text[i].dialogue, 
            "", 
            null, 
            () => { IncrementDialogueBlock(); }
        );
    }

    void IncrementDialogueBlock()
    {
        index++;
        if (index >= text.Count)
        {
            next = false;
            printing = false;
            if (callback != null) callback();
        }
        else
        {
            next = true;
        }
    }
    
}
