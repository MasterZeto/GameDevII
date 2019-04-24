using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxForSaraWind : MonoBehaviour
{
    [SerializeField] float _damage;
    public Collider _collider;
    FighterController playerFighter;
    FighterController player;
    bool knock_back = false;

    public bool active { get; private set; }
    public float cooldown;
    Vector3 impact = Vector3.zero;

    float timescale = 1f;

    void Start() { active = false; cooldown = -1f;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<FighterController>();
    }
    //should not cause any damage, but should knock back player 
    //c should be the collider for the hurtbox
    void OnTriggerStay(Collider c)
    {
       // if (active && c.gameObject.layer == LayerMask.NameToLayer("Hurtbox"))
            if (active)
            {
            Debug.Log("hey I am active");
            //c should be player hurtbox here 
          
             Hurtbox h = c.gameObject.GetComponent<Hurtbox>(); 
           playerFighter = c.gameObject.GetComponentInParent<FighterController>();
            if (playerFighter != null&&playerFighter.gameObject.tag=="Player")
            {
               // playerFighter.SetTrigger("Stunned");
                knock_back = true;
            
       /*         impact = -playerFighter.gameObject.transform.forward * 100f;
                if (impact.magnitude > 0.2f)
                { playerFighter.Move(impact * Time.deltaTime); }
                impact = Vector3.Lerp(impact, Vector3.zero, 1* Time.deltaTime);*/
                // playerFighter.Move(-playerFighter.gameObject.transform.forward * 20f );
                Debug.Log("should play stunned anim here");
            }
            //need to add knock back here
            if (h != null)
            {
      
            }
            else
            {
            }
        }
    }
    void OnTriggerExit(Collider c)
    {
       
        
            playerFighter = c.gameObject.GetComponentInParent<FighterController>();
            if (playerFighter != null && playerFighter.gameObject.tag == "Player")
            {
                //playerFighter.SetTrigger("Stunned");
                knock_back = false;

            }
        
    }
    void Update()
    {

        if (knock_back)
        {
            impact = -player.gameObject.transform.forward * 300f;
            if (impact.magnitude > 0.2f)
            { player.Move(impact * Time.deltaTime); }
            impact = Vector3.Lerp(impact, Vector3.zero, 1 * Time.deltaTime);
        }
    }
    public void Fire(float duration)
    {
        if (!active && cooldown <= 0f)
        {
            Debug.Log("fire fire!!");
            active = true;
            StartCoroutine(FireRoutine(duration));
        }
    }

    public void Pause()
    {
        timescale = 0f;
        _collider.enabled = false;
    }

    public void Resume()
    {
        timescale = 1f;
        _collider.enabled = true;
    }
    //basically it should be active for "duration" amount of time
    //do we need coroutine here?
    IEnumerator FireRoutine(float duration)
    {
        cooldown = duration;

       
      
           while (cooldown >= 0f)
           {
               cooldown -= Time.deltaTime * timescale;
               yield return null;
           }
           
         active = false;
        // gameObject.SetActive(false);
    }

}
