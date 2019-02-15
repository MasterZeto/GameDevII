using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    [SerializeField] float dash_window;

    FighterController fighter;

    float h, v;

    void Awake()
    {
        fighter = GetComponent<FighterController>();
    }

    void Update()
    {
        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");

        CheckDoubleTap();

        fighter.Move((fighter.transform.right * h) + (fighter.transform.forward * v));
    }

    void CheckDoubleTap()
    {
        if (Input.GetKeyDown(KeyCode.F)) fighter.DashLeft();
        if (Input.GetKeyDown(KeyCode.H)) fighter.DashRight();
        if (Input.GetKeyDown(KeyCode.T)) fighter.DashForward();
        if (Input.GetKeyDown(KeyCode.G)) fighter.DashBackward();
    }
}
