using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaraLeftAttack : Action
{
    public Hitbox hitbox;
    [SerializeField] float hit_duration;
    [SerializeField] string anim_name;
    [SerializeField] GameObject[] gems = new GameObject[4];
    [SerializeField] Transform LeftCannon;

    public override void StartAction(FighterController fighter)
    {
        this.fighter = fighter;
        hitbox.Fire(hit_duration);
        fighter.SetTrigger(anim_name);
        int rand = Random.Range(0, 4);
        Instantiate(gems[rand],LeftCannon.position,Quaternion.identity);

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
