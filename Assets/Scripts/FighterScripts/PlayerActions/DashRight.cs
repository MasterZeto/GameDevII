using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashRight : DashAction
{
    Vector3 move;
    [SerializeField] GameObject landing;
    public override void StartAction(FighterController fighter)
    {
        running = true;
        this.fighter = fighter;
        fighter.SetTrigger("DashRight");
    /*  if (gameObject.tag == "Player")
        {
            Vector3 position = transform.position + Predictor(transform.right);
            Instantiate(landing, position, Quaternion.identity);
        }*/
        StartCoroutine(DashRightRoutine());        
    }

    public override void Stop() { running = false; }
    public override void Pause() {}
    public override void Resume() {}

    private IEnumerator DashRightRoutine()
    {
        gameObject.GetComponent<SoundBox>().ExtraSFX();
        for (float t = 0f; t < dash_duration && running; t += Time.deltaTime)
        {
            fighter.UnsafeMove(fighter.transform.right * dash_speed);
            yield return null;
        }
        running = false;
    }
    public override Vector3 Predictor(FighterController fighter, ref Vector3 currentForward, Vector3 currentPosition)
    {  //here right should be fighter.transform.right 
        //10f is the move speed in unsafe move
        move = Vector3.zero;
        Vector3 originalForward = fighter.transform.forward;
        Vector3 originalPosition = fighter.transform.position;
        fighter.transform.forward = currentForward;
        fighter.transform.position = currentPosition;
        Vector3 forward = Vector3.ProjectOnPlane(
            fighter.GetOpponent().transform.position - transform.position, 
            Vector3.up);
        for(int i = 0; i<10;i++){
            forward = Vector3.ProjectOnPlane(fighter.GetOpponent().transform.position - fighter.transform.position, Vector3.up);
            fighter.transform.forward = forward;
            Debug.Log(fighter.transform.forward);
            move += fighter.transform.right * dash_duration * dash_speed;
            fighter.transform.position += move;
        }
        Debug.Log(currentForward);
        currentForward = fighter.transform.forward;
        Debug.Log(currentForward);
        fighter.transform.forward = originalForward;
        fighter.transform.position = originalPosition;
        return move;
    }
}
