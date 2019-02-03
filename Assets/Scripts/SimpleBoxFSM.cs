using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleBoxFSM : MonoBehaviour
{
    private class RotateYState : MachineState
    {
        public RotateYState() { name = "RotateYState"; }
        
        public override void Update(GameObject g, float dt)
        {
            g.transform.Rotate(0, 180 * dt, 0);
        }
    }

    private class OrbitState : MachineState
    {
        public OrbitState() { name = "OrbitState"; }

        public override void Update(GameObject g, float dt)
        {
            g.transform.RotateAround(Vector3.zero, Vector3.up, 120 * dt);
        }
    }

    private class RotateZState : MachineState
    {
        public RotateZState() { name = "RotateZState"; }

        public override void Update(GameObject g, float dt)
        {
            g.transform.Rotate(0, 0, 180 * dt);
        }
    }

    MachineState BoxStateTransition(MachineState s)
    {
        switch (s.name)
        {
            case "OrbitState": 
                if (Input.GetKeyDown(KeyCode.Q)) { return new RotateYState(); }
                if (Input.GetKeyDown(KeyCode.W)) { return new RotateZState(); }
                break;
            case "RotateYState": 
                if (Input.GetKeyDown(KeyCode.E)) { return new OrbitState(); }
                if (Input.GetKeyDown(KeyCode.R)) { return new RotateZState(); }
                break;
            case "RotateZState": 
                if (Input.GetKeyDown(KeyCode.T)) { return new OrbitState(); }
                if (Input.GetKeyDown(KeyCode.Y)) { return new RotateYState(); }
                break;

        }
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
