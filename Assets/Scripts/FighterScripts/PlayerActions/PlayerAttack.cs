using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : Action
{
    [SerializeField] Hitbox hitbox;
    [SerializeField] float hit_duration;

    public override void StartAction(FighterController fighter) 
    {
        Debug.Log("hit started on " + hitbox.gameObject.name);
        hitbox.Fire(hit_duration);
    }
    public override void Stop() {}
    public override void Pause() {}
    public override void Resume() {}

    public override bool IsDone() 
    {
        return !hitbox.active;
    }
    
}
