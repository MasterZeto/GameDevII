using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaraCoolDown:DashAction
{
    [SerializeField] string anim_name;
    bool paused;
    float cdTime = 2f;
    public override void StartAction(FighterController fighter)
    {
        running = true;
        this.fighter = fighter;
   
        StartCoroutine(CoolDown());
      
    }

    public override void Stop() { running = false; }
    public override void Pause() { paused = true; }
    public override void Resume() { paused = false; }
    //cool down and at the same time move toward player
    private IEnumerator CoolDown()
    {

        for (float t = 0f; t < cdTime && running; t += Time.deltaTime)
        {
            while (paused)
            {
                yield return null;
            }
            fighter.UnsafeMove(fighter.transform.forward * 0.2f*dash_speed);

            yield return null;
        }
        running = false;



    }
}
