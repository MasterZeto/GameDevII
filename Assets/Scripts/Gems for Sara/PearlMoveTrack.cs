using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PearlMoveTrack : MonoBehaviour
{

    GameObject player;
    GameObject opponent;
    
    float speed = 17f;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        opponent = GameObject.FindGameObjectWithTag("Opponent");
       
        transform.LookAt(player.transform);
    }


    void Update()
    {   
        transform.position += transform.forward * speed * Time.deltaTime;
        //Rotates the transform about axis passing through point in world coordinates by angle degrees.
        //This modifies both the position and the rotation of the transform.
        //speed of rotation
        transform.RotateAround(opponent.transform.position, player.transform.position - opponent.transform.position, 650 * Time.deltaTime);

    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.tag == "Player")
            Destroy(gameObject);
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag != "Player")
            Destroy(gameObject);
    }
}
