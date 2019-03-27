using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmethystMoveTrack : MonoBehaviour
{
    GameObject player;

    float speed = 17f;

 
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
      //Vector3 playerPos= new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
        transform.LookAt(player.transform);
        // Aim gem in player's direction.
     // transform.rotation = Quaternion.LookRotation(playerPos);
    }
    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
            Destroy(gameObject);
    }
}
