using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashLeft : DashAction 
{
    Vector3 move;
    [SerializeField] GameObject landing;
    public override void StartAction(FighterController fighter)
    {
        running = true;
        this.fighter = fighter;
        fighter.SetTrigger("DashLeft");
    /*   if (gameObject.tag == "Player")
        {
            Vector3 position = transform.position + Predictor(-transform.right);
            Instantiate(landing, position, Quaternion.identity);
        }*/
        StartCoroutine(DashLeftRoutine());        
    }

    public override void Stop() { running = false; }
    public override void Pause() {}
    public override void Resume() {}

    private IEnumerator DashLeftRoutine()
    {
        gameObject.GetComponent<SoundBox>().ExtraSFX();
        for (float t = 0f; t < dash_duration && running; t += Time.deltaTime)
        {
            fighter.UnsafeMove(-fighter.transform.right * dash_speed);
            yield return null;
        }
        running = false;
    }
    public override Vector3 Predictor(FighterController fighter,ref Vector3 currentForward, Vector3 currentPosition)
    {  //here left should be -fighter.transform.right 
        //10f is the move speed in unsafe move
        move = -fighter.transform.right * dash_duration * dash_speed * 10f;
        return move;
    }
}

