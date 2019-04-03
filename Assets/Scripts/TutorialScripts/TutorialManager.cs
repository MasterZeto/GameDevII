using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TutorialStage
{
    MOVEMENT,
    DASH,
    SIMPLE_ATTACK,
    DOUBLE_ATTACK,
    HEAT,
    PAUSE,
    COMBO,
    DODGING,
    CLOSING
}

public delegate void VoidFunc();

public class TutorialManager : MonoBehaviour
{
    [SerializeField] PortraitDialogue intro_dialogue;
    [SerializeField] PortraitDialogue moveDone_dialogue;
    [SerializeField] PortraitDialogue dashDone_dialogue;
    [SerializeField] PortraitDialogue attackDone_dialogue;
    [SerializeField] PortraitDialogue doubleAttackDone_dialogue;
    [SerializeField] PortraitDialogue heatDone_dialogue;
    [SerializeField] PortraitDialogue pauseDone_dialogue;    
    [SerializeField] PortraitDialogue comboDone_dialogue;
    [SerializeField] PortraitDialogue outro_dialogue;


    [SerializeField] Action dash_left_action;
    [SerializeField] Action dash_right_action;
    [SerializeField] Action dash_forward_action;
    [SerializeField] Action dash_back_action;

    [SerializeField] FighterController player;
    [SerializeField] InputHandler input;
    [SerializeField] DummyHurtbox dummy;
    [SerializeField] PauseUIManager pause_manager;

    [SerializeField] SceneTransition scene_transition;
    [SerializeField] SceneFade scene_fade;

    TutorialStage current_stage;
    bool notified;
    string last_box;

    bool should_check;
    
    bool left;
    bool right;
    bool forward;
    bool back;

    bool dash_left;
    bool dash_right;
    bool dash_forward;
    bool dash_back;

    bool right_punch;
    bool left_punch;
    bool right_kick;
    bool left_kick;

    bool double_punch;
    bool double_kick;

    bool paused;

    bool combo_entered;

    void Start() 
    { 
        StartCoroutine(ExecuteAfterDelay(StartFadeIn, 0.1f));
        StartCoroutine(ExecuteAfterDelay(StartTutorial, 1.5f)); 
    }

    void StartTutorial()
    {
        current_stage = TutorialStage.MOVEMENT;
        dummy.Initialize(DummyNotifyMethod);
        notified = false;
        should_check = false;
        intro_dialogue.StartDialogue(ResumePlay);
        input.SetControlActive(false);
    }

    void Update()
    {
        if (should_check) 
        {
            Check();
            UpdateStage();
        }
        if (combo_entered)
        {
            Debug.Log(pause_manager.PauseQueueIndex());
        }
    }

    void UpdateStage()
    {
        switch (current_stage)
        {
            case TutorialStage.MOVEMENT:
                // if stage is done...
                if (left && right && forward && back)
                {
                    Debug.Log("MOVEMENT PHASE DONE");
                    IncrementStage(TutorialStage.DASH, moveDone_dialogue);
                }
                break;
            case TutorialStage.DASH: 
                if (dash_left && dash_right && dash_forward && dash_back && !player.IsActing())
                {
                    Debug.Log("DASHING STAGE DONE");
                    IncrementStage(TutorialStage.SIMPLE_ATTACK, dashDone_dialogue);
                }
                break;
            case TutorialStage.SIMPLE_ATTACK: 
                if (right_punch && left_punch && right_kick && left_kick && !player.IsActing())
                {
                    Debug.Log("SIMPLE ATTACK STAGE DONE");
                    IncrementStage(TutorialStage.DOUBLE_ATTACK, attackDone_dialogue);
                }
                break;
            case TutorialStage.DOUBLE_ATTACK: 
                if (double_punch && double_kick && !player.IsActing())
                {
                    Debug.Log("DOUBLE ATTACK STAGE DONE");
                    IncrementStage(TutorialStage.HEAT, doubleAttackDone_dialogue);
                }
                break;
            case TutorialStage.HEAT: 
                // probably just a thing with 
                Debug.Log("HEAT STAGE DONE");
                IncrementStage(TutorialStage.PAUSE, heatDone_dialogue);
                break;
            case TutorialStage.PAUSE: 
                if (player.pause)
                {
                    Debug.Log("PAUSE STAGE DONE");
                    IncrementStage(TutorialStage.COMBO, pauseDone_dialogue);
                }
                break;
            case TutorialStage.COMBO:
                if (!player.pause && combo_entered && pause_manager.PauseQueueIndex() == pause_manager.PauseQueueCount())
                {
                    Debug.Log("COMBO STAGE DONE");
                    IncrementStage(TutorialStage.DODGING, comboDone_dialogue);
                }
                break;
            case TutorialStage.DODGING:
                IncrementStage(TutorialStage.CLOSING, outro_dialogue);
                break;
            case TutorialStage.CLOSING:
                scene_transition.Transition();
                break;
        }
    }

    void IncrementStage(TutorialStage new_stage, PortraitDialogue dialogue)
    {
        should_check = false;
        current_stage = new_stage;
        // freeze player
        input.SetControlActive(false);
        dialogue.StartDialogue(ResumePlay);
    }

    void Check()
    {
        switch (current_stage)
        {
            case TutorialStage.MOVEMENT:
                CheckMovement();
                break;
            case TutorialStage.DASH:
                CheckDash(); 
                break;
            case TutorialStage.SIMPLE_ATTACK: 
                CheckSimpleAttack();
                break;
            case TutorialStage.DOUBLE_ATTACK: 
                CheckDoubleAttack();
                break;
            case TutorialStage.HEAT: 
                break;
            case TutorialStage.PAUSE:
                break;
            case TutorialStage.COMBO: 
                CheckPauseModeCombo();
                break;
            case TutorialStage.DODGING: 
                break;
        }
    }

    void CheckMovement()
    {
        float h = Input.GetAxisRaw("Horizontal");
        if (h < -0.1f) left  = true;
        if (h > 0.1f)  right = true;
        float v = Input.GetAxisRaw("Vertical");
        if (v < -0.1f) back    = true;
        if (v > 0.1f)  forward = true;
    }

    void CheckDash()
    {
        // the actual logic for IsDone is highkey jank, but it should work
        if (!dash_left_action.IsDone())    dash_left    = true;
        if (!dash_right_action.IsDone())   dash_right   = true;
        if (!dash_forward_action.IsDone()) dash_forward = true;
        if (!dash_back_action.IsDone())    dash_back    = true;
    }

    void CheckSimpleAttack()
    {
        if (notified)
        {
            switch (last_box)
            {
                case "RightArmHitbox": right_punch = true; notified = false; break;
                case "LeftArmHitbox":  left_punch  = true; notified = false; break;
                case "RightLegHitbox": right_kick  = true; notified = false; break;
                case "LeftLegHitbox":  left_kick   = true; notified = false; break;
            }
        }
    }

    void CheckDoubleAttack()
    {
        if (notified)
        {
            switch (last_box)
            {
                case "DoubleArmHitbox": double_punch = true; notified = false; break;
                case "DoubleLegHitbox": double_kick  = true; notified = false; break;
            }
        }
    }

    void CheckPauseModeCombo()
    {
        if (!player.pause && pause_manager.PauseQueueCount() > 0)
        {
            Debug.Log("COMBO ENTERED!!!");
            combo_entered = true;
        }
    }

    void DummyNotifyMethod(string box)
    {
        last_box = box;
        notified = true;
        Debug.Log(last_box);
    }

    void ResumePlay()
    {
        should_check = true;
        input.SetControlActive(true);
        // give control back to player
    }

    void StartFadeIn() { scene_fade.FadeIn(1f); }

    private IEnumerator ExecuteAfterDelay(VoidFunc func, float delay)
    {
        yield return new WaitForSeconds(delay);
        func();
    }
}
