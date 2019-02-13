using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CommandPattern
{
    

public class AnimationEvents : MonoBehaviour
{
    public Fighter fighter;
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player=GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void ArmDone(){
        if(this.gameObject==player){
            fighter.waiting=false;
            if(fighter.comQueue.Count!=0){
                fighter.comQueue.RemoveAt(0);
                fighter.UpdateUI();
            }
        }
        
    }
}
}
