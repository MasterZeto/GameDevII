using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CommandPattern
{
    

public class AnimationEvents : MonoBehaviour
{
    public Fighter fighter;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void ArmDone(){
        fighter.waiting=false;
        //fighter.comQueue.RemoveAt(0);
    }
}
}
