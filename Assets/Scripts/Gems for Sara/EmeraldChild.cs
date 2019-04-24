using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmeraldChild : MonoBehaviour
{
    GameObject player;
    float speed = 25f;
    float deathTimer;
    float timer;
    GameObject opponent;
    FighterController fighter;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        deathTimer = 3f;
        timer = 0;
        transform.LookAt(player.transform.position + new Vector3(0, 4.0f, 0));
        opponent = GameObject.FindGameObjectWithTag("Opponent");
        fighter = opponent.GetComponent<FighterController>();
    }


    void Update()
    {

        if (fighter.pause != true)
        {
            timer += Time.deltaTime;
            transform.position += transform.forward * speed * Time.deltaTime;
        }
        if (timer > deathTimer)
        {
            Destroy(gameObject);
        }
    }

}
