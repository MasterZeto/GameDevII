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
        float zigZagTime = 2;
        float currentTime = 0;
        float P = 0;
        float speed = 0;
        Vector3 tmp;
        Vector3 moveDirection;
        bool right = true;

        public override void Update(SawyerCharacter actor, float dt)
        {
            currentTime += Time.deltaTime;
           if(currentTime>zigZagTime)
            {  //calculate a new direction here
                currentTime = 0;
                P = Random.Range(0, 1);
                speed = 2.0f;
                if (right)
                {
                    tmp = Vector3.Cross(actor.character.transform.up, actor.character.transform.forward).normalized;
                    moveDirection = Vector3.Lerp(tmp, actor.character.transform.forward, P);
                }
                else {
                    tmp = Vector3.Cross(-actor.character.transform.up, actor.character.transform.forward).normalized;
                    moveDirection = Vector3.Lerp(tmp, actor.character.transform.forward, P);
                }
                Debug.Log("zig1");
                right = !right;
              
            }
            //move here
            actor.character.Move(moveDirection * speed);



            /*  //if the direction is relative to AI, can not use update, maybe coroutine?
              //if the direction is relative to the world, can use update
              float P = Random.Range(0, 1);
              float speed = 2.0f;
              //find the vector that is perpendicular to the transform.forward vector on the xz plane
              Vector3 tmp = Vector3.Cross(actor.character.transform.up, actor.character.transform.forward).normalized;
              Vector3 moveDirection =Vector3.Lerp(tmp, actor.character.transform.forward, P);
              //Vector3 moveDirection = Vector3.Lerp(actor.character.transform.right, actor.character.transform.forward, P);
              actor.character.Move(moveDirection*speed);
              Debug.Log("zig1"); */
        }
    }
    private class SecondZigZag : MachineState<SawyerCharacter>
    {
        public SecondZigZag() { name = "SecondZigZag"; }

        public override void Update(SawyerCharacter actor, float dt)
        {  //if the direction is relative to AI, can not use update, maybe coroutine?
            //if the direction is relative to the world, can use update
            float P = Random.Range(0, 1);
            float speed = 2.0f;
            Vector3 moveDirection = Vector3.Lerp(new Vector3(0, 0, 1), actor.character.transform.forward, P);
            //Vector3 moveDirection = Vector3.Lerp(actor.character.transform.right, actor.character.transform.forward, P);
            actor.character.Move(moveDirection * speed);
            Debug.Log("zig2");
        }
    }
    private class DashToPlayer : MachineState<SawyerCharacter>
    {
        public  DashToPlayer () { name = "DashToPlayer"; }

        public override void Update(SawyerCharacter actor, float dt)
        {
            actor.character.Move(actor.character.transform.forward * 10f);
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
                if (dist_to_player < 2f&& dist_to_player>1f) 
                { 
                    actor.animator.SetBool("walk", false);
                    return new SecondZigZag(); 
                }
                break;
            case "SecondZigZag": 
                if (dist_to_player < 1f)
                {
                    actor.animator.SetBool("walk", true);
                    return new DashToPlayer();
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
