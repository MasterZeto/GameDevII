using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DashAction : Action
{
    [SerializeField] protected float dash_speed;
    [SerializeField] protected float dash_duration;
    public virtual Vector3 Predictor(FighterController fighter, ref Vector3 DashPredictorForward, Vector3 currentPosition){return Vector3.zero;}
}
