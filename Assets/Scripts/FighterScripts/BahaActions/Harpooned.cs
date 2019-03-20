using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harpooned : MonoBehaviour
{
    public bool playerAttached = false;
    GameObject player = null;
    float playerY;
    Vector3 playerLoc;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(player!=null){
            playerLoc = transform.position;
            playerLoc.y=playerY;
            player.transform.position=playerLoc;
        }
        
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player"){
            playerY = other.gameObject.transform.position.y;
            other.gameObject.transform.parent = this.gameObject.transform;
            player = other.gameObject;
            playerAttached = true;
        }
    }
    public void UnparentPlayer(){
        player.transform.parent = null;
        playerAttached = false;
        player = null;
    }
}
