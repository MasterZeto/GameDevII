using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaraBracketAnim : BracketAnim
{
    void Awake()
    {
        transition_function = bracket_transition.TriggerSaraAnimation;
    }
}
