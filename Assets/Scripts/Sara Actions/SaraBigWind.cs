using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//big wind attack together with the particle effect for air push
//need to make player stunned 
public class SaraBigWind : Action
{
    public HitboxForSaraWind hitbox;
    [SerializeField] float hit_duration;
    [SerializeField] string anim_name;
    ParticleSystem particle;
    float chaseTime = 1f;

    public override void StartAction(FighterController fighter)
    {   
        this.fighter = fighter;
        fighter.SetTrigger(anim_name);
        particle = gameObject.GetComponentInChildren<ParticleSystem>();
        particle.Play();


        //should fire once here
        //hit_duration is set to be 0 over inspector
        hitbox.Fire(hit_duration);
 





        //      transform.gameObject.GetComponent<SoundBox>().MissSFX();
    }

    public override void Stop() {}
    public override void Pause() 
    {
        hitbox.Pause();
    }
    public override void Resume() 
    {
         hitbox.Resume();
    }
    
    public override bool IsDone()
    {
        return !hitbox.active;
    }

}
