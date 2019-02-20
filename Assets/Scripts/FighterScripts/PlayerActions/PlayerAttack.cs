using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : Action
{
    public Hitbox hitbox;
    [SerializeField] float hit_duration;
    [SerializeField] string anim_name;

    public override void StartAction(FighterController fighter)
    {
        hitbox.Fire(hit_duration);
        fighter.SetTrigger(anim_name);
    }

    public override void Stop() {}
    public override void Pause() {}
    public override void Resume() {}
    
    public override bool IsDone()
    {
        return !hitbox.active;
    }
}
