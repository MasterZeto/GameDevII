using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PearlMoveTrack : MonoBehaviour
{

    GameObject player;
    GameObject opponent;
    
    float speed = 0.2f;
    float rotateSpeed = 100f;

    float time;
    float width;
    float height;
    float z;


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        opponent = GameObject.FindGameObjectWithTag("Opponent");
        time = 0;
        width = 50;
        height = 50;
        z = transform.position.z;
       
      transform.LookAt(player.transform);
    }


    void Update()
    {
        time += 10*Time.deltaTime;
        z += 10* Time.deltaTime *speed;
        float x =transform.position.x+Mathf.Cos(time)*0.1f ;
        float y = transform.position.y+Mathf.Sin(time)*0.1f ;
        transform.position = new Vector3(x, y, z);

      //  transform.position += transform.forward * speed * Time.deltaTime;
        //Rotates the transform about axis passing through point in world coordinates by angle degrees.
        //This modifies both the position and the rotation of the transform.
        //speed of rotation
        //transform.RotateAround(opponent.transform.position, player.transform.position - opponent.transform.position, 650 * Time.deltaTime);

      
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
    void Rotate(Vector3 point, Vector3 axis, float angle)
    {
        transform.RotateAround(point, axis, angle);

    }
}


