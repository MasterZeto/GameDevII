using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeatSystem : MonoBehaviour
{
    [SerializeField] Image heat_bar_cover;

    FighterController fighter;

    void Awake() 
    {
        fighter = GetComponent<FighterController>();
    }

    void Update()
    {
        heat_bar_cover.rectTransform.localScale = new Vector3(
            (fighter.max_heat - fighter.heat) / fighter.max_heat, 1f, 1f
        );
    }
}
