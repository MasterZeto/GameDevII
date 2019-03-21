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
        transform.RotateAround(opponent.transform.position, player.transform.position - opponent.transform.position, 500 * Time.deltaTime);

    }
}
