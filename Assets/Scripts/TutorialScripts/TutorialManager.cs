using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

[System.Serializable]
public struct MovementChecklist
{
    public GameObject list;
    public Text move_forward;
    public Text move_backward;
    public Text move_right;
    public Text move_left;
}

[System.Serializable]
public struct DashChecklist
{
    public GameObject list;
    public Text dash_forward;
    public Text dash_backward;
    public Text dash_right;
    public Text dash_left;
}

[System.Serializable]
public struct AttackChecklist
{
    public GameObject list;
    public Text punch_left;
    public Text punch_right;
    public Text kick_left;
    public Text kick_right;
}

[System.Serializable]
public struct DoubleAttackChecklist
{
    public GameObject list;
    public Text punch_double;
    public Text kick_double;
}

[System.Serializable]
public struct PauseChecklist
{
    public GameObject list;
    public Text pause;
}

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

    [Space]
    [SerializeField] MovementChecklist movement_checklist;
    [SerializeField] DashChecklist dash_checklist;
    [SerializeField] AttackChecklist attack_checklist;
    [SerializeField] DoubleAttackChecklist double_checklist;
    [SerializeField] PauseChecklist pause_checklist;

    [Space]
    [SerializeField] Action dash_left_action;
    [SerializeField] Action dash_right_action;
    [SerializeField] Action dash_forward_action;
    [SerializeField] Action dash_back_action;

    [Space]
    [SerializeField] FighterController player;
    [SerializeField] InputHandler input;
    [SerializeField] DummyHurtbox dummy;
    [SerializeField] PauseUIManager pause_manager;

    [Space]
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
        intro_dialogue.StartDialogue(StartMovement);
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
                    movement_checklist.list.SetActive(false);
                    IncrementStage(TutorialStage.DASH, moveDone_dialogue, StartDash);
                }
                break;
            case TutorialStage.DASH: 
                if (dash_left && dash_right && dash_forward && dash_back && !player.IsActing())
                {
                    Debug.Log("DASHING STAGE DONE");
                    dash_checklist.list.SetActive(false);
                    IncrementStage(TutorialStage.SIMPLE_ATTACK, dashDone_dialogue, StartAttack);
                }
                break;
            case TutorialStage.SIMPLE_ATTACK: 
                if (right_punch && left_punch && right_kick && left_kick && !player.IsActing())
                {
                    Debug.Log("SIMPLE ATTACK STAGE DONE");
                    attack_checklist.list.SetActive(false);
                    IncrementStage(TutorialStage.DOUBLE_ATTACK, attackDone_dialogue, StartDoubleAttack);
                }
                break;
            case TutorialStage.DOUBLE_ATTACK: 
                if (double_punch && double_kick && !player.IsActing())
                {
                    Debug.Log("DOUBLE ATTACK STAGE DONE");
                    double_checklist.list.SetActive(false);
                    IncrementStage(TutorialStage.HEAT, doubleAttackDone_dialogue, ResumePlay);
                }
                break;
            case TutorialStage.HEAT: 
                // probably just a thing with 
                Debug.Log("HEAT STAGE DONE");
                IncrementStage(TutorialStage.PAUSE, heatDone_dialogue, StartPause);
                break;
            case TutorialStage.PAUSE: 
                if (player.pause)
                {
                    Debug.Log("PAUSE STAGE DONE");
                    pause_checklist.list.SetActive(false);
                    IncrementStage(TutorialStage.COMBO, pauseDone_dialogue, ResumePlay);
                }
                break;
            case TutorialStage.COMBO:
                if (!player.pause && combo_entered && pause_manager.PauseQueueIndex() == pause_manager.PauseQueueCount())
                {
                    Debug.Log("COMBO STAGE DONE");
                    IncrementStage(TutorialStage.DODGING, comboDone_dialogue, ResumePlay);
                }
                break;
            case TutorialStage.DODGING:
                IncrementStage(TutorialStage.CLOSING, outro_dialogue, ResumePlay);
                break;
            case TutorialStage.CLOSING:
                scene_transition.Transition();
                break;
        }
    }

    void IncrementStage(TutorialStage new_stage, PortraitDialogue dialogue, EndCallback callback)
    {
        should_check = false;
        current_stage = new_stage;
        // freeze player
        input.SetControlActive(false);
        dialogue.StartDialogue(callback);
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
        if (h < -0.1f) { left  = true; movement_checklist.move_left.color = Color.grey; }
        if (h > 0.1f)  { right = true; movement_checklist.move_right.color = Color.grey; }
        float v = Input.GetAxisRaw("Vertical");
        if (v < -0.1f) { back    = true; movement_checklist.move_backward.color = Color.grey; }
        if (v > 0.1f)  { forward = true; movement_checklist.move_forward.color = Color.grey; }
    }

    void CheckDash()
    {
        // the actual logic for IsDone is highkey jank, but it should work
        if (!dash_left_action.IsDone())    { dash_left    = true; dash_checklist.dash_left.color = Color.grey; }
        if (!dash_right_action.IsDone())   { dash_right   = true; dash_checklist.dash_right.color = Color.grey; }
        if (!dash_forward_action.IsDone()) { dash_forward = true; dash_checklist.dash_forward.color = Color.grey; }
        if (!dash_back_action.IsDone())    { dash_back    = true; dash_checklist.dash_backward.color = Color.grey; }
    }

    void CheckSimpleAttack()
    {
        if (notified)
        {
            switch (last_box)
            {
                case "RightArmHitbox": 
                    right_punch = true; 
                    notified = false; 
                    attack_checklist.punch_right.color = Color.grey; 
                    break;
                case "LeftArmHitbox":  
                    left_punch = true; 
                    notified = false; 
                    attack_checklist.punch_left.color = Color.grey; 
                    break;
                case "RightLegHitbox": 
                    right_kick = true; 
                    notified = false; 
                    attack_checklist.kick_right.color = Color.grey; 
                    break;
                case "LeftLegHitbox":  
                    left_kick = true; 
                    notified = false; 
                    attack_checklist.kick_left.color = Color.grey; 
                    break;
            }
        }
    }

    void CheckDoubleAttack()
    {
        if (notified)
        {
            switch (last_box)
            {
                case "DoubleArmHitbox": 
                    double_punch = true; 
                    notified = false; 
                    double_checklist.punch_double.color = Color.grey;
                    break;
                case "DoubleLegHitbox": 
                    double_kick = true; 
                    notified = false; 
                    double_checklist.kick_double.color = Color.grey;
                    break;
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

    void StartMovement()
    {
        ResumePlay();
        movement_checklist.list.SetActive(true);
    }

    void StartDash()
    {
        ResumePlay();
        dash_checklist.list.SetActive(true);
    }

    void StartAttack()
    {
        ResumePlay();
        attack_checklist.list.SetActive(true);
    }

    void StartDoubleAttack()
    {
        ResumePlay();
        double_checklist.list.SetActive(true);
    }

    void StartPause()
    {
        ResumePlay();
        pause_checklist.list.SetActive(true);
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
