using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaraDashBack : DashAction
{
    [SerializeField] string anim_name;
    [SerializeField] GameObject player;
    FighterController Sara;
    CharacterController controller;
    bool paused = false;
    public override void StartAction(FighterController fighter)
    {
        running = true;
        this.fighter = fighter;
        Sara = GetComponent<FighterController>();
        controller = GetComponent<CharacterController>();

        StartCoroutine(DashBackwardRoutine());
    }

    public override void Stop() { running = false; }
    public override void Pause() { paused = true; }
    public override void Resume() { paused = false; }

    private IEnumerator DashBackwardRoutine()
    {
        Debug.Log("coroutine check");
        //if stuck here, check the motion 
        if (gameObject.GetComponent<CharacterController>().velocity==Vector3.zero)
        {
            Debug.Log("I rotate here to avoid corner");
          //maybe here can use the air jump to the center stage in the future
            for (float s = 0; s < 1f; s += Time.deltaTime)
            {
                controller.SimpleMove(-fighter.transform.right * dash_speed);
             //   fighter.UnsafeMove(-fighter.transform.right * dash_speed);

                // gameObject.transform.RotateAround(player.transform.position, Vector3.up, 90f);
            }
            yield return null;
        }

        for (float t = 0f; t < dash_duration && running; t += Time.deltaTime)
        {
            while (paused)
            {
                yield return null;
            }
    
        
            fighter.UnsafeMove(-fighter.transform.forward * dash_speed);
            
            yield return null;
        }
        running = false;
    }
}

