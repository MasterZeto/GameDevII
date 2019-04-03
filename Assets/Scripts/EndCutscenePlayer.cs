using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndCutscenePlayer : MonoBehaviour
{
    [SerializeField] PortraitDialogue dialogue;
    [SerializeField] SceneTransition transition;
    [SerializeField] SceneFade fade;

    void Start()
    {
        StartCoroutine(ExecAfterDelay(fade.FadeIn, 0.01f));
        StartCoroutine(ExecAfterDelay(PlayDialogue, 1.5f));
    }

    void PlayDialogue()
    {
        dialogue.StartDialogue(transition.Transition);
    }

    private IEnumerator ExecAfterDelay(VoidDelegate func, float delay)
    {
        yield return new WaitForSeconds(delay);
        func();
    }
}
