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
        if (distance < 20f && canReborn)
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
        child2.transform.eulerAngles +=new Vector3(0, 0, 45);

        //face to the right of player
       child3 = Instantiate(child, transform.position, Quaternion.identity);
        child3.transform.eulerAngles += new Vector3(0, 0, -45);
     
        Destroy(gameObject);






    }
}
