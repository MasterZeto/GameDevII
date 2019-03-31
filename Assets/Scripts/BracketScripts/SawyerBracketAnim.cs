using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawyerBracketAnim : BracketAnim
{
    void Awake()
    {
        transition_function = bracket_transition.TriggerSawyerAnimation;
    }
}
