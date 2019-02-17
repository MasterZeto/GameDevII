using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Giga.AI.FSM;
using Giga.AI.Blackboard;

public class SawyerCharacter : AICharacter
{
    public CharacterController character { get; private set; }
    public Animator animator { get; private set; }
    public Hitbox hitbox { get; private set; }

    float t;

    public SawyerCharacter(CharacterController character, Hitbox h)
    {
        this.character = character;
        animator = character.transform.GetChild(0).GetComponent<Animator>();
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
    private class TowardPlayer : MachineState<SawyerCharacter>
    {
        public TowardPlayer() { name = "TowardPlayer"; }

        public override void Update(SawyerCharacter actor, float dt)
        {
            actor.character.SimpleMove(actor.character.transform.forward * 3f);            
        }
    }

    private class AttackPlayer : MachineState<SawyerCharacter>
    {
        public AttackPlayer() { name = "AttackPlayer"; }

        public override void Update(SawyerCharacter actor, float dt)
        {
            actor.Attack();
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

    SawyerCharacter actor;

    public SawyerFSM(SawyerCharacter actor) 
         : base(new TowardPlayer(), next_state) 
    {
        this.actor = actor;
    }
}

[RequireComponent(typeof(CharacterController))]
public class SawyerController : MonoBehaviour
{
    [SerializeField] Hitbox hitbox;
    
    SawyerCharacter ai;
    SawyerFSM fsm;

    void Awake()
    {
        ai = new SawyerCharacter(GetComponent<CharacterController>(), hitbox);
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
