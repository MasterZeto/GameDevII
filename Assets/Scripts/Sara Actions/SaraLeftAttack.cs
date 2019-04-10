using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaraLeftAttack : Action
{
    public List<HitboxForGems> hitbox = new List<HitboxForGems>();
    [SerializeField] float hit_duration;
    [SerializeField] string anim_name;
    [SerializeField] GameObject[] gems = new GameObject[4];
    [SerializeField] Transform LeftCannon;
    GameObject tempForGem;
    HitboxForGems box;
    bool done = false;

    public override void StartAction(FighterController fighter)
    {
        this.fighter = fighter;
      
        
        fighter.SetTrigger(anim_name);
        int rand = Random.Range(0, 4);
       tempForGem= Instantiate(gems[3],LeftCannon.position,Quaternion.identity);
        hitbox.Add(tempForGem.GetComponent<HitboxForGems>());
        box = tempForGem.GetComponent<HitboxForGems>();
        hitbox.Add(box);
        done = true;




        //      transform.gameObject.GetComponent<SoundBox>().MissSFX();
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
