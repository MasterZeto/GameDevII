using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
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

        fighter.Move((fighter.transform.right * h) + (fighter.transform.forward * v));
    }
}
