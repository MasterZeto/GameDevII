using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeatSystem : MonoBehaviour
{
    [SerializeField] HeatController heat;

    FighterController fighter;

    void Awake() 
    {
        fighter = GetComponent<FighterController>();
    }

    void Update()
    {
        heat.SetHeat(fighter.heat);
    }
}
