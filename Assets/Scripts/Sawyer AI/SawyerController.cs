using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Giga.AI.FSM;
using Giga.AI.Blackboard;

public class SawyerCharacter : AICharacter
{
    public FighterController character { get; private set; }
    public Animator animator { get; private set; }
    public Hitbox hitbox { get; private set; }

    float t;

    //constructor
    public SawyerCharacter(FighterController character, Hitbox h)
    {
        this.character = character.GetComponent<FighterController>();
        animator = character.GetComponentInChildren<Animator>();
        hitbox = h;

 
    }

    //sawyer sweep stricks and overhead smash add here
    public void Attack()
    {
       character.RightPunch();
    }
    public void DashRight()
    {
        character.DashRight();
    }
    public void DashLeft()
    {
        character.DashLeft();
    }

}

public class SawyerFSM : FiniteStateMachine<SawyerCharacter>
{
    private class FirstZigZag : MachineState<SawyerCharacter>
    {
        public FirstZigZag() { name = "FirstZigZag"; }
        float zigZagTime = 1.0f;
        float currentTime = 1.1f;
        bool right = true;
        public override void Update(SawyerCharacter actor, float dt)
        {
            currentTime += Time.deltaTime;
      
            if (currentTime>zigZagTime)
            {
                right = !right;
                currentTime = 0;
                if (right)
                {
                    actor.DashRight();
                    Debug.Log("toward player");
                }
                else {
                    actor.DashLeft();
                    Debug.Log("toward player");
                }
            }
       
 
           // actor.character.transform.rotation = Quaternion.LookRotation(actor.character.RelativeMove(moveDirection));

        }
    }
    private class ZigZagAway : MachineState<SawyerCharacter>
    {
        public ZigZagAway() { name = "ZigZagAway"; }
        float zigZagTime = 0.8f;
        float currentTime = 1f;
        float P = 0;// a parameter that is supposed to affect the change of angle for each zigzag move
        float speed = 0;
        // float maxSpeed = 1.5f;
        Vector3 moveDirection;
        bool right = true;
        public override void Update(SawyerCharacter actor, float dt)
        {
            currentTime += Time.deltaTime;
            speed +=25f * Time.deltaTime;
            if (currentTime > zigZagTime)
            {
                right = !right;
                speed = 0;
                currentTime = 0;
                P = Random.Range(0.5f, 1);
                if (right)
                {
                    //tmp = Vector3.Cross(actor.character.transform.up, actor.character.transform.forward).normalized;
                    //calculate a new direction here
                    moveDirection = Vector3.Lerp(actor.character.transform.right, -actor.character.transform.forward, P);
                    Debug.Log("running away");
                }
                else
                {
                    // tmp = Vector3.Cross(actor.character.transform.forward, actor.character.transform.up).normalized;
                    moveDirection = Vector3.Lerp(-actor.character.transform.right, -actor.character.transform.forward, P);
                    Debug.Log("running away");
                }
            }
            //move here
            actor.character.RelativeMove(moveDirection * (speed + 1));
            actor.character.transform.rotation = Quaternion.LookRotation(actor.character.RelativeMove(moveDirection));
        }
    }

    private class DashToPlayer : MachineState<SawyerCharacter>
    {
        public  DashToPlayer () { name = "DashToPlayer"; }

        public override void Update(SawyerCharacter actor, float dt)
        {
            actor.character.Move(actor.character.transform.forward * 3f);
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
        bool isSeen = actor.character.GetOpponent().GetComponent<PlayerView>().FoundSawyer;//is it getting updated here?
        Debug.Log(isSeen);
        switch (state.name)
        {
            case "FirstZigZag": //only dash to player when get close and stay out of player sight 
                if (dist_to_player < 10f&&isSeen==false)
                {
                 //  actor.animator.SetBool("walk", false);
                    return new DashToPlayer();
                }//if get close to player but is within the sight, zigzag away
                else if(dist_to_player < 7f && isSeen == true)
                {
                    return new ZigZagAway();
                }
                break;
            case "ZigZagAway":
                if(dist_to_player > 20f)
                {
                    return new FirstZigZag();
                }
                break;
            case "DashToPlayer":
                if (dist_to_player > 7f)
                {
                    //actor.animator.SetBool("walk", true);
                    return new FirstZigZag();
                }
                else if(dist_to_player <3f)
                {

                    return new AttackPlayer();
                }
                break;
            case "AttackPlayer":
                if (isSeen== true&&dist_to_player > 5f)
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
        /* transform.forward = Vector3.ProjectOnPlane(
            (Blackboard.player_position - transform.position), 
             Vector3.up
         ).normalized;*/

        transform.forward = ai.character.GetOpponent().transform.position - transform.position;

        fsm.Update(ai, Time.deltaTime);




    }


}
