using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaraLeftAttack : Action
{
    public List<HitboxForGems> hitbox = new List<HitboxForGems>();
    [SerializeField] float hit_duration;
    [SerializeField] string anim_name;
    [SerializeField] GameObject[] gems = new GameObject[5];
    [SerializeField] Transform LeftCannon;
    GameObject tempForGem;
    HitboxForGems box;
    bool done = false;
    ParticleSystem[] particles;

    public override void StartAction(FighterController fighter)
    {
        this.fighter = fighter;

        particles = GameObject.FindGameObjectWithTag("FireL").GetComponentsInChildren<ParticleSystem>();
        fighter.SetTrigger(anim_name);
        foreach (ParticleSystem par in particles)
        {
            par.Play();
        }
        int rand = Random.Range(0, 5);
        tempForGem= Instantiate(gems[rand],LeftCannon.position,Quaternion.identity);
        hitbox.Add(tempForGem.GetComponent<HitboxForGems>());
        box = tempForGem.GetComponent<HitboxForGems>();
        hitbox.Add(box);
        done = true;




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
}
