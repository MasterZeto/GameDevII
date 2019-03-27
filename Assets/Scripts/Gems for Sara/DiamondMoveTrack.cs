using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//should intantiate into three other pieces and destroy itself
public class DiamondMoveTrack : MonoBehaviour
{
    GameObject player;
    float speed = 17f;
    float distance;
    GameObject child;
    GameObject child1;
    GameObject child2;
    GameObject child3;
    bool canReborn = true;

    // Start is called before the first frame update
    void Start()
    {
        child = Resources.Load("DiamondChild") as GameObject;
        Debug.Log("diamond");
        player = GameObject.FindGameObjectWithTag("Player");
        transform.LookAt(player.transform);
        
        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(distance);
        distance = Vector3.Distance(player.transform.position, transform.position);
        transform.position += transform.forward * speed * Time.deltaTime;
        if (distance < 15f && canReborn)
        {
            Debug.Log("should reborn now");
            canReborn = false;
            Reborn();

        }

    }


    void Reborn() {
        //face the same direction toward player
      
        child1= Instantiate(child, transform.position, Quaternion.identity);
        //face to the left of player
        child2 = Instantiate(child, transform.position, Quaternion.identity);
        //should adjust the forward direction
        Vector3 forward2 = child2.transform.forward;
        forward2.z -= 70;
        child2.transform.forward = forward2;


    /*    Vector3 angle2 = child2.transform.eulerAngles;
        angle2.z += 60;
        child2.transform.eulerAngles = angle2;*/

        //face to the right of player
        child3 = Instantiate(child, transform.position, Quaternion.identity);
        Vector3 forward3 = child3.transform.forward;
        forward3.x -=70;
        child3.transform.forward = forward3;




        /*Vector3 angle3 = child3.transform.eulerAngles;
        angle3.z -= 60;
        child3.transform.eulerAngles = angle3;*/


        Destroy(gameObject);






    }
}
