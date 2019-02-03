using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate MachineState GetNextState(MachineState s);

public abstract class MachineState
{
    public abstract void Update(GameObject g, float dt);
}

public class FiniteStateMachine
{
    GetNextState next_state;
    MachineState current_state;

    /* ALWAYS STARTS ON STATE 0 */
    public FiniteStateMachine(MachineState start_state, GetNextState state_transition_function)
    {
        current_state = start_state;
        this.next_state = state_transition_function;
    }

    public void Update(GameObject g, float dt)
    {
        current_state.Update(g, dt);
        MachineState s = next_state(current_state);
        //Debug.Log(s);
        if (s != null) current_state = s;
    }
}
