using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Giga.AI.FSM;
using Giga.AI.Blackboard;


public class FirstBossCharacter : AICharacter
{
    public FighterController fighter;

    //constructor
    public FirstBossCharacter(FighterController fighter)
    {
        this.fighter = fighter;
    }
    //sawyer sweep stricks and overhead smash add here
    public void Attack()
    {
        fighter.RightPunch();
    }

    
}

public class FirstBossFSM : FiniteStateMachine<FirstBossCharacter>
{
    private class TowardPlayer : MachineState<FirstBossCharacter>
    {
        public TowardPlayer() { name = "TowardPlayer"; }

        public override void Update(FirstBossCharacter actor, float dt)
        {
            actor.fighter.RelativeMove(Vector3.forward * 3f);            
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
        float dist_to_player = Vector3.Distance(actor.fighter.transform.position, Blackboard.player_position);
        switch(state.name)
        {
            case "TowardPlayer": 
                if (dist_to_player < 2f) 
                { 
                    return new AttackPlayer(); 
                }
                break;
            case "AttackPlayer": 
                if (dist_to_player > 3f)
                {
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
    FirstBossCharacter ai;
    FirstBossFSM fsm;

    void Awake()
    {
        ai = new FirstBossCharacter(GetComponent<FighterController>());
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
