using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaraRightAttack : Action
{
    public List<HitboxForGems> hitbox = new List<HitboxForGems>();
    [SerializeField] float hit_duration;
    [SerializeField] string anim_name;
    [SerializeField] GameObject[] gems = new GameObject[5];
    [SerializeField] Transform RightCannon;
    GameObject tempForGem;
    HitboxForGems box;
    bool done = false;
    ParticleSystem[] particles = new ParticleSystem[2];
    int rand = 0;
 

    public override void StartAction(FighterController fighter)
    {
        this.fighter = fighter;
        particles = GameObject.FindGameObjectWithTag("FireR").GetComponentsInChildren<ParticleSystem>();
        fighter.SetTrigger(anim_name);
        foreach (ParticleSystem par in particles){
            par.Play();
        }
       
        //instantiate one gem 
        rand = Random.Range(0,5);
        tempForGem = Instantiate(gems[rand], RightCannon.position, Quaternion.identity);
        box = tempForGem.GetComponent<HitboxForGems>();
        hitbox.Add(box);
        done = true;
     //   foreach (HitboxForGems box in hitbox)
      //  { box.Fire(hit_duration); }
        gameObject.GetComponent<SoundBox>().HitSFX();
    }

    public override void Stop() {}
    public override void Pause() 
    {
        foreach (HitboxForGems box in hitbox)
        { box.Pause(); }
    }
    public override void Resume() 
    {
        foreach (HitboxForGems box in hitbox)
        { box.Resume(); }
    }
    
    public override bool IsDone()
    {
        return done;
    }
   /* private static bool isActive(HitboxForGems h)
    {
        return !h.active;
    }*/
}
