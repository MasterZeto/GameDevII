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
    [SerializeField] float attack_distance;

    FighterController sawyer;
    Blackboard blackboard;
    BehaviorTree tree;

    Vector3 lateral_move_dir;
    Vector3 radial_move_dir;

    float t = 0;
    float switched_time = 0f;

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
                new ActionNode(sawyer.DashLeft)
            }
        );

        SequencerNode DashInRight = new SequencerNode(
            new List<Node>()
            {
                new ActionNode(sawyer.DashRight),
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
                    new ActionNode(SetMoveDirForward),
                    new ActionNode(SetMoveDirBackward),
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
                            new ActionNode(Nothing)
                        }
                    ),
                    new ActionNode(sawyer.RightPunch),
                    new SelectorNode(
                        ShouldSwitchDirection,
                        new List<Node>() 
                        {
                            new ActionNode(SwitchRadialDirection),
                            new ActionNode(Nothing)
                        }
                    )
                }
            )
        );
    }

    void Update()
    {
        Debug.Log(sawyer.IsActing());
        if (actions.Count == 0) actions = tree.Evaluate();

        if (!sawyer.IsActing()) (actions.Dequeue())();

        sawyer.RelativeMove((lateral_move_dir + radial_move_dir).normalized);

        t += Time.deltaTime;
        switched_time += Time.deltaTime;
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

        if (dist > movein_distance) return 0;
        if (dist < attack_distance) return 3;
        if (dist < backup_distance) return 1;
        if (Mathf.Abs(dist - dash_distance) < 1f) return 2;
        
        return 4;
    }

    int ShouldSwitchDirection()
    {
        float time_range_value = (switched_time - 2f) / 2f;
        return ((Random.Range(0f, 1f) < time_range_value) ? 0 : 1);
    }

    int ShouldAttack()
    {
        return ((t > 5f && Random.Range(0f, 1f) > 0.4f) ? 0 : 1);
    }

    int Random01()
    {
        return Random.Range(0, 2);
    }

}

/*
    I'm thinking it would be better to instead of use actions in the tree, but
    use a function call since everything gets routed through the fighter controller
    so we can just have function calls with "is done"... maybe do that...
 */