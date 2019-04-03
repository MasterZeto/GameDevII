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
    GameObject child4;
    GameObject child5;
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
        if (distance < 13f && canReborn)
        {
            Debug.Log("should reborn now");
            canReborn = false;
            Reborn();

        }

    }


    void Reborn() {
        //face the same direction toward player
      
        child1= Instantiate(child, transform.position, Quaternion.identity);
        child1.transform.LookAt(player.transform);

        //face to the left of player
        child2 = Instantiate(child, transform.position, Quaternion.identity);
        //should adjust the forward direction
        Vector3 forward2 = child2.transform.forward;
        forward2.z -= 70;
        child2.transform.forward = forward2;

        child4= Instantiate(child, transform.position, Quaternion.identity);
        Vector3 forward4 = child4.transform.forward;
        forward4.z+= 50;
        child4.transform.forward = forward4;




        //face to the right of player
        child3 = Instantiate(child, transform.position, Quaternion.identity);
        Vector3 forward3 = child3.transform.forward;
        forward3.x -=20;
        child3.transform.forward = forward3;


        child5 = Instantiate(child, transform.position, Quaternion.identity);
        Vector3 forward5 = child5.transform.forward;
        forward5.x +=50;
        child5.transform.forward = forward5;


        Destroy(gameObject);






    }
}
