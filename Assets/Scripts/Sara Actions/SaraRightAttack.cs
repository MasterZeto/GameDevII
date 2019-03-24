using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaraRightAttack : Action
{
    public List<Hitbox> hitbox = new List<Hitbox>();
    [SerializeField] float hit_duration;
    [SerializeField] string anim_name;
    [SerializeField] GameObject[] gems = new GameObject[4];
    [SerializeField] Transform RightCannon;
    GameObject tempForGem;

    public override void StartAction(FighterController fighter)
    {
        this.fighter = fighter;
        foreach (Hitbox box in hitbox)
        { box.Fire(hit_duration); }
        fighter.SetTrigger(anim_name);
        //instantiate one gem 
        int rand = Random.Range(0,4);
        tempForGem = Instantiate(gems[rand], RightCannon.position, Quaternion.identity);
        hitbox.Add(tempForGem.GetComponent<Hitbox>());
        //      transform.gameObject.GetComponent<SoundBox>().MissSFX();
    }

    public override void Stop() {}
    public override void Pause() 
    {
        foreach (Hitbox box in hitbox)
        { box.Pause(); }
    }
    public override void Resume() 
    {
        foreach (Hitbox box in hitbox)
        { box.Resume(); }
    }
    
    public override bool IsDone()
    {
        return hitbox.TrueForAll(isActive);
    }
    private static bool isActive(Hitbox h)
    {
        return !h.active;
    }
}
