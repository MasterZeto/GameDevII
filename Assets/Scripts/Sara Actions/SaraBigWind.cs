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
    GameObject[] diamonds = new GameObject[7];
    public override void StartAction(FighterController fighter)
    {   
        this.fighter = fighter;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<FighterController>();
        health = gameObject.GetComponent<HealthSystem>();
        diamonds[0] = Resources.Load("DiamondF0") as GameObject;
        diamonds[1] = Resources.Load("DiamondF1") as GameObject;
        diamonds[2] = Resources.Load("DiamondF2") as GameObject;
        diamonds[3] = Resources.Load("DiamondF3") as GameObject;
        diamonds[4] = Resources.Load("DiamondF4") as GameObject;
        diamonds[5] = Resources.Load("DiamondF5") as GameObject;
        diamonds[6] = Resources.Load("DiamondF6") as GameObject;
        fighter.SetTrigger(anim_name);
        player.SetTrigger("Stunned");
        particle = GameObject.FindGameObjectWithTag("Wind").GetComponent<ParticleSystem>();
        // particle = gameObject.GetComponentInChildren<ParticleSystem>();

        if (health.getHitPoints() > 30)
        { particle.Play(); }
        else {//diamond projectile
            foreach (GameObject diamond in diamonds)
            {
                Instantiate(diamond, gameObject.transform, false);
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
