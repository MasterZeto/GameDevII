using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    [SerializeField]
    GameObject Sawyer;
    public bool FoundSawyer = true;
    public float BackAngle = 180f;

    // Update is called once per frame
    void Update()
    {
        CheckView();
    }


    bool CheckView ()
    {
        Vector3 direction = Sawyer.transform.position - transform.position;
        float angle = Vector3.Angle(direction, -transform.forward);
        if(angle<BackAngle*0.5)
        {
            FoundSawyer = false;
        }
        else
        { FoundSawyer = true; }

        return FoundSawyer;
    }

}
