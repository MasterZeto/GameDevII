using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaraMoveLeft :DashAction
{
    [SerializeField] string anim_name;
    bool paused;
    public override void StartAction(FighterController fighter)
    {
        running = true;
        this.fighter = fighter;
   
        StartCoroutine(DashLeftRoutine());
    }

    public override void Stop() { running = false; }
    public override void Pause() { paused = true; }
    public override void Resume() { paused = false; }

    private IEnumerator DashLeftRoutine()
    {   //basically make it to the left
        Vector3 moveDirection = Vector3.Lerp(-transform.right, transform.forward, 0.1f);
        for (float t = 0f; t < dash_duration && running; t += Time.deltaTime)
        {
            while(paused){
                yield return null;
            }
            fighter.Move(moveDirection * dash_speed);
            yield return null;
        }
        running = false;
    }
}
