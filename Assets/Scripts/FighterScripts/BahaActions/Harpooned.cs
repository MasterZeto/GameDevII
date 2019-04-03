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
    [SerializeField] FighterController playerCon;
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
    public void ParentPlayer(Collider other)
    {
        if(!playerAttached&&!isStunned){
            Debug.Log("Player should now be attached");
            playerY = playerCon.gameObject.transform.position.y;
            playerCon.gameObject.transform.parent = this.gameObject.transform;
            player = playerCon.gameObject;
            playerCon = player.GetComponent<FighterController>();
            playerCon.Stun();
            playerAttached = true;
        }
    }
    public void StunBaha(Collider other)  {
        if(!isStunned&&!playerAttached){
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
        player = null;
    }
    private IEnumerator Resume(float stunned){
        yield return new WaitForSeconds(stunned);
        bahaCon.Resume();
    }
}
