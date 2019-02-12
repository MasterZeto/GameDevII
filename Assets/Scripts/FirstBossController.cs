using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Giga.AI.FSM;
using Giga.AI.Blackboard;

public class FirstBossCharacter : AICharacter
{
    public NavMeshAgent agent            { get; private set; }
    public CharacterController character { get; private set; }

    public FirstBossCharacter(NavMeshAgent agent, CharacterController character)
    {
        this.agent = agent;
        this.character = character;
    }
}

public class FirstBossFSM : FiniteStateMachine<FirstBossCharacter>
{
    private class TowardPlayer : MachineState<FirstBossCharacter>
    {
        public TowardPlayer() { name = "TowardPlayer"; }

        public override void Update(FirstBossCharacter actor, float dt)
        {
            actor.agent.SetDestination(Blackboard.player_position);
            actor.character.Move(actor.agent.desiredVelocity);
            
        }
    }

    private class AttackPlayer : MachineState<FirstBossCharacter>
    {
        public AttackPlayer() { name = "AttackPlayer"; }

        public override void Update(FirstBossCharacter actor, float dt)
        {
            // do nothing
        }
    }

    private static MachineState<FirstBossCharacter> next_state(
        FirstBossCharacter actor, 
        MachineState<FirstBossCharacter> state
    )
    {
        switch(state.name)
        {
            case "TowardPlayer": 
                if (actor.agent.remainingDistance < 10.0f) { Debug.Log("stop"); return new AttackPlayer(); }
                break;
            case "AttackPlayer": 
                if (Vector3.Distance(actor.agent.transform.position, Blackboard.player_position) > 20.0)
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

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(CharacterController))]
public class FirstBossController : MonoBehaviour
{
    FirstBossCharacter ai;
    FirstBossFSM fsm;

    void Awake()
    {
        ai = new FirstBossCharacter(GetComponent<NavMeshAgent>(), GetComponent<CharacterController>());
        fsm = new FirstBossFSM(ai);
    }

    void Update()
    {
        fsm.Update(ai, Time.deltaTime);
    }

}
