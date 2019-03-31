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

    public override void StartAction(FighterController fighter)
    {   
        this.fighter = fighter;
        particle = gameObject.GetComponentInChildren<ParticleSystem>();
 
        
        fighter.SetTrigger(anim_name);
        //should fire once here 
        hitbox.Fire(hit_duration);


        particle.Play();
      //  hitbox.gameObject.SetActive(true);
     
     

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
