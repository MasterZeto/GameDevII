using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [ContextMenu("Start Dialogue")]
    void StartDialogue()
    {
        printing = true;
        next = false;
        index = 0;

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
        }
        else
        {
            next = true;
        }
    }
    
}
