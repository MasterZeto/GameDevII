using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Giga.AI.FSM;
using Giga.AI.Blackboard;

public class SawyerCharacter : AICharacter
{

    public FighterController character;

    public Animator animator { get; private set; }
    public Hitbox hitbox { get; private set; }

    float t;

    //constructor
    public SawyerCharacter(FighterController character, Hitbox h)
    {
        this.character = character;
        animator = character.GetComponentInChildren<Animator>();
        hitbox = h;


    }

    //sawyer sweep stricks and overhead smash add here
    public void Attack()
    {

        character.RightPunch();
    }
    public void RightDash()
    {
        character.DashRight();
    }
    public void LeftDash()
    {
        character.DashLeft();
    }
    public void BackRightDash()
    {
        character.DashBackRight();
    }
    public void BackLeftDash()
    {
        character.DashBackLeft();
    }
    public void DashForward()
    {
        character.DashForward();
    }

}

public class SawyerFSM : FiniteStateMachine<SawyerCharacter>
{
    private class FirstZigZag : MachineState<SawyerCharacter>
    {
        public FirstZigZag() { name = "FirstZigZag"; }
        float zigZagTime = 0.3f;
        float currentTime = 1f;
        bool right = true;
        public override void Update(SawyerCharacter actor, float dt)
        {
            currentTime += Time.deltaTime;

            if (currentTime > zigZagTime)
            {

                currentTime = 0;
                if (right)
                {
                    actor.RightDash();
                    right = !right;
                    Debug.Log("right dash to player");
                }
                else
                {
                    actor.LeftDash();
                    right = !right;
                    Debug.Log("left dash to player");
                }
            }


            // actor.character.transform.rotation = Quaternion.LookRotation(actor.character.RelativeMove(moveDirection));

        }
    }
    private class ZigZagAway : MachineState<SawyerCharacter>
    {
        public ZigZagAway() { name = "ZigZagAway"; }
        float zigZagTime = 0.3f;
        float currentTime = 1f;
        float P = 0;// a parameter that is supposed to affect the change of angle for each zigzag move
        bool right = true;
        public override void Update(SawyerCharacter actor, float dt)
        {
            currentTime += Time.deltaTime;


            if (currentTime > zigZagTime)
            {

                currentTime = 0;
                if (right)
                {
                    actor.BackRightDash();
                    right = !right;
                    Debug.Log("right dash away player");
                }
                else
                {
                    actor.BackLeftDash();
                    right = !right;
                    Debug.Log("left dash away player");
                }
            }

            //actor.character.transform.rotation = Quaternion.LookRotation(actor.character.RelativeMove(moveDirection));
        }
    }

    private class DashToPlayer : MachineState<SawyerCharacter>
    {
        public DashToPlayer() { name = "DashToPlayer"; }

        public override void Update(SawyerCharacter actor, float dt)
        {
            actor.DashForward();
            Debug.Log("DashToPlayer");
        }
    }

    private class AttackPlayer : MachineState<SawyerCharacter>
    {
        public AttackPlayer() { name = "AttackPlayer"; }

        public override void Update(SawyerCharacter actor, float dt)
        {
            actor.Attack();

            Debug.Log("should attack now");
        }
    }

    private static MachineState<SawyerCharacter> next_state(
        SawyerCharacter actor,
        MachineState<SawyerCharacter> state
    )
    {
        float dist_to_player = Vector3.Distance(actor.character.transform.position, Blackboard.player_position);

        switch (state.name)
        {
            case "FirstZigZag": 
                if (dist_to_player < 15f)
                {   //only dash to player when get close
                    return new DashToPlayer();
                }
                break;
            case "ZigZagAway":
                if (dist_to_player > 25f)
                {
                    return new FirstZigZag();
                }
                break;
            case "DashToPlayer":
                 if (dist_to_player <10f)
                {
                    return new AttackPlayer();
                }
                break;
            case "AttackPlayer":
                if (dist_to_player < 3f)
                {
                    return new ZigZagAway();
                }
                break;
        }
        return null;
    }
    SawyerCharacter actor;

    public SawyerFSM(SawyerCharacter actor)
         : base(new FirstZigZag(), next_state)
    {
        this.actor = actor;
    }
}

[RequireComponent(typeof(FighterController))]
public class SawyerController : MonoBehaviour
{
    [SerializeField] Hitbox hitbox;
    SawyerCharacter ai;
    SawyerFSM fsm;


    void Awake()
    {  //fightercontroller
        ai = new SawyerCharacter(GetComponent<FighterController>(), hitbox);
        fsm = new SawyerFSM(ai);
    }

    void Update()
    {
        /*  transform.forward = Vector3.ProjectOnPlane(
             (Blackboard.player_position - transform.position), 
              Vector3.up
          ).normalized;*/

        if(ai.character.animator.enabled) transform.forward = (ai.character.GetOpponent().transform.position - transform.position).normalized;

        fsm.Update(ai, Time.deltaTime);




    }


}