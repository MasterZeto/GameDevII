using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void VoidDelegate();

public class FightManager : MonoBehaviour
{
    [SerializeField] PortraitDialogue intro_dialogue;
    [SerializeField] PortraitDialogue lose_dialogue;
    [SerializeField] PortraitDialogue final_dialogue;

    [SerializeField] SceneFade fade;
    [SerializeField] SceneTransition transition;

    [SerializeField] HealthSystem player_health_system;
    [SerializeField] HealthSystem opponent_health_system;

    [SerializeField] InputHandler player;
    [SerializeField] SawyerController sawyer;
    [SerializeField] BTSaraAI sara;
    [SerializeField] BahaController baha;
    /* Put the other controllers in here */

    [SerializeField] GameObject loss_screen;

    void Start()
    {
        // instantiate the loss_screen and disable it
        loss_screen.SetActive(false);
        // freeze the boys
        Freeze();
        // fade in
        fade.FadeIn();
        // set death callbacks
        player_health_system.SetDeathCallback(LossCallback);
        if (sawyer != null) opponent_health_system.SetDeathCallback(WinCallback);
        if (sara != null) opponent_health_system.SetDeathCallback(WinCallback);
        if (baha != null) opponent_health_system.SetDeathCallback(WinCallback);
        // start the dialogue with the StartGame() callback
        StartCoroutine(ExecAfterDelay(StartDialogue, 1.5f));
    }

    void StartDialogue()
    {
        intro_dialogue.StartDialogue(StartGame);
    }

    void StartGame()
    {
        // do the 321 start and then unfreeze the boys
        Unfreeze(); // probably call after delay
    }

    void WinCallback()
    {
        // freeze the boys and play the ending dialogue 
        Freeze();
        final_dialogue.StartDialogue(transition.Transition);
    }

    void LossCallback()
    {
        // freeze the boys and open the lose menu
        Freeze();
        lose_dialogue.StartDialogue(BringUpLoseScreen);
    }

    void BringUpLoseScreen()
    {
        loss_screen.SetActive(true);
    }

    void Freeze() 
    {
        player.SetControlActive(false);
        if (sawyer != null) sawyer.SetFreezed(true);
        if (sara != null) sara.SetFreezed(true);
        if (baha != null) baha.SetFreezed(true);
    }

    void Unfreeze()
    {
        player.SetControlActive(true);
        if (sawyer != null) sawyer.SetFreezed(false);
        if (sara != null) sara.SetFreezed(false);
        if (baha!= null) baha.SetFreezed(false);
    }

    private IEnumerator ExecAfterDelay(VoidFunction func, float delay)
    {
        yield return new WaitForSeconds(delay);
        func();
    }
}
