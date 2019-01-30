using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleBoxFSM : MonoBehaviour
{
    private class RotateYState : MachineState
    {
        public override void Update(GameObject g, float dt)
        {
            g.transform.Rotate(0, 180 * dt, 0);
        }
    }

    private class OrbitState : MachineState
    {
        public override void Update(GameObject g, float dt)
        {
            g.transform.RotateAround(Vector3.zero, Vector3.up, 120 * dt);
        }
    }

    private class RotateZState : MachineState
    {
        public override void Update(GameObject g, float dt)
        {
            g.transform.Rotate(0, 0, 180 * dt);
        }
    }

    MachineState BoxStateTransition(MachineState s)
    {
        return null;
    }

    FiniteStateMachine fsm;

    void Start()
    {
        fsm = new FiniteStateMachine(new OrbitState(), BoxStateTransition);
    }

    void Update()
    {
        fsm.Update(this.gameObject, Time.deltaTime);
    }
}
