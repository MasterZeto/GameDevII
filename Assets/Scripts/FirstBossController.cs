using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Giga.AI.FSM;
using Giga.AI.Blackboard;


public class FirstBossCharacter : AICharacter
{
    public CharacterController character { get; private set; }
    public Animator animator { get; private set; }
    public Hitbox hitbox { get; private set; }
    

    float t;
    //constructor
    public FirstBossCharacter(CharacterController character, Hitbox h)
    {
        this.character = character;
        animator = character.transform.GetChild(0).GetComponent<Animator>();
        hitbox = h;
    }
    //sawyer sweep stricks and overhead smash add here
    public void Attack()
    {
        if (!hitbox.active)
        {
            animator.SetTrigger("punch");
            hitbox.Fire(0.75f);
        }
    }

    
}

public class FirstBossFSM : FiniteStateMachine<FirstBossCharacter>
{
    private class TowardPlayer : MachineState<FirstBossCharacter>
    {
        public TowardPlayer() { name = "TowardPlayer"; }

        public override void Update(FirstBossCharacter actor, float dt)
        {
            actor.character.SimpleMove(actor.character.transform.forward * 3f);            
        }
    }

    private class AttackPlayer : MachineState<FirstBossCharacter>
    {
        public AttackPlayer() { name = "AttackPlayer"; }

        public override void Update(FirstBossCharacter actor, float dt)
        {
            actor.Attack();
        }
    }

    private static MachineState<FirstBossCharacter> next_state(
        FirstBossCharacter actor, 
        MachineState<FirstBossCharacter> state
    )
    {
        float dist_to_player = Vector3.Distance(actor.character.transform.position, Blackboard.player_position);
        switch(state.name)
        {
            case "TowardPlayer": 
                if (dist_to_player < 2f) 
                { 
                    actor.animator.SetBool("walk", false);
                    return new AttackPlayer(); 
                }
                break;
            case "AttackPlayer": 
                if (dist_to_player > 3f)
                {
                    actor.animator.SetBool("walk", true);
                    return new TowardPlayer();
                }
                break;
        }
        return null;
    }

    FirstBossCharacter actor;

    public FirstBossFSM(FirstBossCharacter actor) 
         : base(new TowardPlayer(), next_state) 
    {
        this.actor = actor;
    }
}

[RequireComponent(typeof(CharacterController))]
public class FirstBossController : MonoBehaviour
{
    [SerializeField] Hitbox hitbox;
    
    FirstBossCharacter ai;
    FirstBossFSM fsm;

    void Awake()
    {
        ai = new FirstBossCharacter(GetComponent<CharacterController>(), hitbox);
        fsm = new FirstBossFSM(ai);
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
