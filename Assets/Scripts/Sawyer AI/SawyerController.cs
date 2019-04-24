using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Giga.AI.FSM;
using Giga.AI.Blackboard;
using Giga.AI.BehaviorTree;

[RequireComponent(typeof(FighterController))]
public class SawyerController : MonoBehaviour
{
    [SerializeField] float dash_distance;
    [SerializeField] float backup_distance;
    [SerializeField] float movein_distance;
    [SerializeField] float rest_distance;

    FighterController sawyer;
    Blackboard blackboard;
    BehaviorTree tree;

    Vector3 lateral_move_dir;
    Vector3 radial_move_dir;

    float t = 0;
    float attack_delay = 5f;
    float switched_time = 0f;
    float rest_time = 3f;
    float rest_timer = 0f;

    bool should_rest = true;
    bool freezed = false;

    
    [SerializeField, Range(0f,1f)] float nothing_percent    = 0.4f;
    [SerializeField, Range(0f,1f)] float switch_percent     = 0.1f;
    [SerializeField, Range(0f,1f)] float dash_left_percent  = 0.1f;
    [SerializeField, Range(0f,1f)] float dash_right_percent = 0.1f;

    [Space]
    [SerializeField] PauseScript pause;

    Queue<ActionDelegate> actions;

    void Awake()
    {  
        sawyer = GetComponent<FighterController>();
        blackboard = GameObject.FindGameObjectWithTag("Blackboard").GetComponent<Blackboard>();
        actions = new Queue<ActionDelegate>();
        lateral_move_dir = Vector3.zero;
        radial_move_dir = (Random01() == 0) ? Vector3.right : Vector3.left;

    }

    void Start()
    {
        // should probably have a dash routine that's like

        /* 
            should circle the player...
            so I should have some functions that set a local "move" variable
            and do some actions that adjust this function. Then when I eval
            the tree, do the action, then try to move in the direction

            AttackL = Seq(AttackL, AttackRL)
            AttackR = Seq(AttackR, AttackRL)
            DashInL = Seq(DashL, DashR, DashL)
            DashInR = Seq(DashR, DashL, DashR)

            Retreat = DashBack

            Select : DistFromPlayer
                Select : WhereToMove (basically an else for the rest of the tree)
                    ---
                    ---
                    ---
                MoveForward  (if too far)
                MoveBackward (if too close)
                Select : Random01 (if in dash range)
                    DashL
                    DashR
                Select : Random01 (if in attack range)
                    AttackL
                    AttackR

         */

        SequencerNode DashInLeft = new SequencerNode(
            new List<Node>()
            {
                new ActionNode(sawyer.DashLeft),
                new ActionNode(sawyer.DashRight),
                new ActionNode(sawyer.DashForward),
                new ActionNode(sawyer.RightPunch),
                new ActionNode(GoRest)
            }
        );

        SequencerNode DashInRight = new SequencerNode(
            new List<Node>()
            {
                new ActionNode(sawyer.DashRight),
                new ActionNode(sawyer.DashLeft),
                new ActionNode(sawyer.DashForward),
                new ActionNode(sawyer.LeftPunch),
                new ActionNode(GoRest)
            }
        );

        SelectorNode Idle = new SelectorNode(
            IdleSelect,
            new List<Node>()
            {
                new ActionNode(Nothing),
                new ActionNode(SwitchRadialDirection),
                new ActionNode(sawyer.DashLeft),
                new ActionNode(sawyer.DashRight)
            }
        );

        /* if (dist > movein_distance) return 0;
        if (dist < attack_distance) return 3;
        if (dist < backup_distance) return 1;
        if (Mathf.Abs(dist - dash_distance) < 1f) return 2;
        
        return 4; */

        tree = new BehaviorTree(
            new SelectorNode(
                DistanceToPlayer,
                new List<Node>()
                {
                    new SelectorNode(
                        Random01,
                        new List<Node>()
                        {
                            new ActionNode(sawyer.DashLeft),
                            new ActionNode(sawyer.DashRight)
                        }    
                    ),
                    new SelectorNode(
                        Random01,
                        new List<Node>()
                        {
                            new ActionNode(sawyer.DashBackLeft),
                            new ActionNode(sawyer.DashBackRight)
                        }    
                    ),
                    new SelectorNode(
                        ShouldAttack,
                        new List<Node>()
                        {
                            new SelectorNode(
                                Random01,
                                new List<Node>()
                                {
                                    DashInLeft,
                                    DashInRight
                                }
                            ),
                            Idle
                        }
                    ),
                    new SelectorNode(
                        Random01,
                        new List<Node>()
                        {
                            new ActionNode(sawyer.DashBackLeft),
                            new ActionNode(sawyer.DashBackRight)
                        }    
                    ),
                    new ActionNode(Nothing)
                }
            )
        );
    }

    void Update()
    {
        Debug.Log(sawyer.current_action == null);
        if (sawyer.current_action != null && sawyer.current_action.IsPaused() && !pause.IsPaused()) 
        { 
            sawyer.current_action.Resume(); 
        }
        if (!freezed) 
        {
            if (actions.Count == 0) actions = tree.Evaluate();

            if (!sawyer.IsActing()) (actions.Dequeue())();

            if (!sawyer.IsActing()) sawyer.RelativeMove((lateral_move_dir + radial_move_dir).normalized);

            t += Time.deltaTime;
            switched_time += Time.deltaTime;

            if (should_rest) rest_timer += Time.deltaTime;
            if (rest_timer >= rest_time) should_rest = false;
        }
    }

    void SetMoveDirForward()
    {
        lateral_move_dir = Vector3.forward;
    }

    void SetMoveDirBackward()
    {
        lateral_move_dir = Vector3.back;
    }

    void SwitchRadialDirection()
    {
        radial_move_dir = -radial_move_dir;
        switched_time = 0f;
    }

    void Nothing() {}

    int DistanceToPlayer()
    {
        float dist = blackboard.DistanceFromPlayerToOpponent();

        /* check distance against stuff */

        if (dist < rest_distance && should_rest) return 3;
        if (should_rest) return 2;
        if (dist > movein_distance) return 0;
        if (dist < backup_distance) return 1;
        return 2;
    }

    int ShouldSwitchDirection()
    {
        float time_range_value = (switched_time - 2f) / 2f;
        return ((Random.Range(0f, 1f) < time_range_value) ? 0 : 1);
    }

    int ShouldAttack()
    {
        if (should_rest) return 1;
        if (t > attack_delay) 
        {
            t = 0; 
            attack_delay = Random.Range(1f, 3f);
            return 0;
        }
        else 
        {
            return 1;
        };
    }

    int IdleSelect()
    {
        float r = Random.Range(0f, 1f);

        if (r < switch_percent)     return 1; r -= switch_percent;
        if (r < dash_left_percent)  return 2; r -= dash_left_percent;
        if (r < dash_right_percent) return 3; r -= dash_right_percent;
        if (r < nothing_percent)    return 0; r -= nothing_percent;
        
        return 0;
    }

    int Random01()
    {
        return Random.Range(0, 2);
    }

    public void SetFreezed(bool f)
    {
        freezed = f;
        //if (f) { sawyer.Pause(); } else { sawyer.Resume(); }
    }

    void GoRest()
    {
        should_rest = true;
        rest_time = Random.Range(2f, 4f);
        rest_timer = 0f;
    }

}

/*
    I'm thinking it would be better to instead of use actions in the tree, but
    use a function call since everything gets routed through the fighter controller
    so we can just have function calls with "is done"... maybe do that...
 */