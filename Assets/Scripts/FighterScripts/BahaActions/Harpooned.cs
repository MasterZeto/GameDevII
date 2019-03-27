using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harpooned : MonoBehaviour
{
    [SerializeField] float stunTime = 1f;
    public bool playerAttached = false;
    [SerializeField] FighterController bahaCon = null;
    GameObject player = null;
    float playerY;
    Vector3 playerLoc;
    FighterController playerCon;
    public bool isStunned = false;
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
        Debug.Log(other.gameObject.tag);
        if(other.gameObject.tag == "Player"){
            playerY = other.gameObject.transform.position.y;
            other.gameObject.transform.parent = this.gameObject.transform;
            player = other.gameObject;
            playerCon = player.GetComponent<FighterController>();
            playerCon.Stun();
            playerAttached = true;
        }
        else if(other.gameObject.tag == "Border"&&!isStunned){
            Debug.Log("Is now stunned");
            isStunned = true;
            if(bahaCon!=null){
                bahaCon.Stun();
                StartCoroutine(Resume(stunTime));
            }
        }
    }
    public void UnparentPlayer(){
        playerCon.Resume();
        player.transform.parent = null;
        playerAttached = false;
        player = null;
    }
    private IEnumerator Resume(float stunned){
        yield return new WaitForSeconds(stunned);
        bahaCon.Resume();
    }
}
