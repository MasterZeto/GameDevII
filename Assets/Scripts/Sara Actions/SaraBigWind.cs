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
    FighterController player;
    HealthSystem health;
    [SerializeField] GameObject[] diamonds = new GameObject[7];
    public override void StartAction(FighterController fighter)
    {   
        this.fighter = fighter;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<FighterController>();
        health = gameObject.GetComponent<HealthSystem>();
        fighter.SetTrigger(anim_name);
        player.SetTrigger("Stunned");
        particle = GameObject.FindGameObjectWithTag("Wind").GetComponent<ParticleSystem>();
        // particle = gameObject.GetComponentInChildren<ParticleSystem>();

        if (health.getHitPoints() > 100)
        { particle.Play(); }
        else {//diamond projectile
            foreach (GameObject diamond in diamonds)
            {
                diamond.SetActive(true);
            }

        }


        //should fire once here
        //hit_duration is set to be 0 over inspector
        hitbox.Fire(hit_duration);
 





        gameObject.GetComponent<SoundBox>().MissSFX();
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
