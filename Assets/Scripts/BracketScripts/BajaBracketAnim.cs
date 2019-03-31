using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BajaBracketAnim : BracketAnim
{
    void Awake()
    {
        transition_function = bracket_transition.TriggerBajaAnimation;
    }
}
