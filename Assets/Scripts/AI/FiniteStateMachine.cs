using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Okay, so I want to do something more specific instead of just pushing game objects into it.
    I think that just means genericizing the thing for all machine states, then they can take it
    as reference in update instead of passed as a constructor var
 */

namespace Giga.AI.FSM
{
    public abstract class AICharacter { }    

    public abstract class MachineState<T>
    {
        public string name { get; protected set; }
        public abstract void Update(T actor, float dt);
    }

    public delegate MachineState<T> GetNextState<T>(T actor, MachineState<T> s);

    public class FiniteStateMachine<T>
    {
        GetNextState<T> next_state;
        MachineState<T> current_state;

        /* ALWAYS STARTS ON STATE 0 */
        public FiniteStateMachine(MachineState<T> start_state, GetNextState<T> state_transition_function)
        {
            current_state = start_state;
            this.next_state = state_transition_function;
        }

        public void Update(T actor, float dt)
        {
            current_state.Update(actor, dt);
            MachineState<T> s = next_state(actor, current_state);
            //Debug.Log(s);
            if (s != null) current_state = s;
        }
    }
}