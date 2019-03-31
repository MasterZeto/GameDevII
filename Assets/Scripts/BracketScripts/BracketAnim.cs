using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void BracketTransitionFunction(float delay);

public class BracketAnim : MonoBehaviour
{
    [SerializeField] protected BracketTransition bracket_transition;
    [SerializeField] SceneFade fade;
    [SerializeField] float fade_time;

    protected BracketTransitionFunction transition_function;

    void Start()
    {
        fade.FadeIn(fade_time);
        transition_function(fade_time);
    }

}
