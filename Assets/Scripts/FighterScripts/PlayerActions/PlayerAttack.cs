﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : Action
{
    [SerializeField] Hitbox hitbox;
    [SerializeField] float hit_duration;
    [SerializeField] string anim_name;

    public override void StartAction(FighterController fighter)
    {
        this.fighter = fighter;
        hitbox.Fire(hit_duration);
        fighter.SetTrigger(anim_name);
    }

    public override void Stop() {}
    public override void Pause() 
    {
        fighter.Pause();
        hitbox.Pause();
    }
    public override void Resume() 
    {
        fighter.Resume();
        hitbox.Resume();
    }
    
    public override bool IsDone()
    {
        return !hitbox.active;
    }
}
