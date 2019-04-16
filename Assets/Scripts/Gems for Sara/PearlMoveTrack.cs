using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class PearlMoveTrack : MonoBehaviour
{   //support for the renderer(trail prediction)
    LineRenderer lr;
    int resolution;
    bool visible = false;


    GameObject player;
    GameObject opponent;
    FighterController fighter;

    float speed = 0.005f;
    float rotateSpeed = 100f;
    float deathTimer;
    float timer;

    float time;
    float width;
    float height;

    //support for the prediction
    float futureTime;
    float futureX;
    float futureY;
    float futureZ;
 

    Vector3[] points;

 
    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
      

    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        opponent = GameObject.FindGameObjectWithTag("Opponent");
        fighter = opponent.GetComponent<FighterController>();
        deathTimer = 100f;
        timer = 0;
        time = 0;
        width = 50;
        height = 50;
        transform.LookAt(player.transform);
        resolution = 40;
        points = new Vector3[resolution + 1];
        lr.positionCount = resolution + 1;
        //  z = transform.position.z;


    }


    void Update()
    {

        if (timer > deathTimer)
        {
            Destroy(gameObject);
        }
        //only move the gem when unpaused
        if (fighter.pause != true)
        {
            timer += Time.deltaTime;

            time += 20 * Time.deltaTime;
            //  float z = transform.position.z - 17 * Time.deltaTime * speed;
            float z = transform.position.z - time * speed;
            float x = transform.position.x + Mathf.Cos(time) * 0.6f;
            float y = transform.position.y + Mathf.Sin(time) * 0.6f;
            transform.position = new Vector3(x, y, z);

        }
        // when time is paused should show the line renderer
        //should only generate line renderer once, should not keep updating 
        else {
         
            // lr.positionCount = resolution + 1;
            for (int i = 0; i <= resolution; i++)
            {
                futureTime = time + 100 * i;
                futureZ = transform.position.z - futureTime * speed;
                futureX = transform.position.x + Mathf.Cos(futureTime) * 0.6f;
                futureY = transform.position.y + Mathf.Sin(futureTime) * 0.6f;
                points[i] = new Vector3(futureX, futureY, futureZ);
            }


            lr.SetPositions(points);

            Debug.Log("Enter Pause here!!");



        }
        //  transform.position += transform.forward * speed * Time.deltaTime;
        //Rotates the transform about axis passing through point in world coordinates by angle degrees.
        //This modifies both the position and the rotation of the transform.
        //speed of rotation
        //transform.RotateAround(opponent.transform.position, player.transform.position - opponent.transform.position, 650 * Time.deltaTime);
    }





    void OnCollisionExit(Collision other)
    {
        if (other.gameObject.tag == "Player")
            Destroy(gameObject);
    }
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag != "Player")
            Destroy(gameObject);
    }
    void Rotate(Vector3 point, Vector3 axis, float angle)
    {
        transform.RotateAround(point, axis, angle);

    }
}


