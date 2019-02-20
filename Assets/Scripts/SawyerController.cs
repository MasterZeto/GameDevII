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

    public void Attack()
    {
        if (!hitbox.active)
        {
            animator.SetTrigger("punch");
            hitbox.Fire(0.75f);
        }
    }



}

public class SawyerFSM : FiniteStateMachine<SawyerCharacter>
{
    private class FirstZigZag : MachineState<SawyerCharacter>
    {
        public FirstZigZag() { name = "FirstZigZag"; }
        float zigZagTime = 1.0f;
        float currentTime = 1.1f;
        float P = 0;// a parameter that is supposed to affect the change of angle for each zigzag
        float speed = 0;
        // float maxSpeed = 1.5f;
        //Vector3 tmp;
        Vector3 moveDirection;
        bool right = true;
        public override void Update(SawyerCharacter actor, float dt)
        {
            currentTime += Time.deltaTime;
            speed += 4f*Time.deltaTime;
            if (currentTime>zigZagTime)
            {
                right = !right;
                speed = 0;
                currentTime = 0;
                P = Random.Range(0, 1);
                if (right)
                {   
                    //tmp = Vector3.Cross(actor.character.transform.up, actor.character.transform.forward).normalized;
                    //calculate a new direction here
                   
                   moveDirection = Vector3.Lerp(actor.character.transform.right, actor.character.transform.forward, 0.5f);
                    Debug.Log("one direction:right");
            
                }
                else {
                 
                   // tmp = Vector3.Cross(actor.character.transform.forward, actor.character.transform.up).normalized;
                    moveDirection = Vector3.Lerp(-actor.character.transform.right, actor.character.transform.forward, 0.5f);
                    Debug.Log("another direction");
                }
    
           
              
            }
            //move here
            actor.character.RelativeMove(moveDirection);

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
        switch(state.name)
        {
            case "FirstZigZag": 
                if (dist_to_player < 5f) 
                { 
                    actor.animator.SetBool("walk", false);
                    return new DashToPlayer(); 
                }
                break;
            case "DashToPlayer":
                if (dist_to_player > 5f)
                {
                    actor.animator.SetBool("walk", true);
                    return new FirstZigZag();
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
        transform.forward = Vector3.ProjectOnPlane(
           (Blackboard.player_position - transform.position), 
            Vector3.up
        ).normalized;
        fsm.Update(ai, Time.deltaTime);
    }


}
