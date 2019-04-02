using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxForSaraWind : MonoBehaviour
{
    [SerializeField] float _damage;
    public Collider _collider;
    FighterController playerFighter;

    public bool active { get; private set; }
    public float cooldown;

    float timescale = 1f;

    void Start() { active = false; cooldown = -1f; }
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
                playerFighter.SetTrigger("Stunned");
                playerFighter.Move(-playerFighter.gameObject.transform.forward * 200f*Time.deltaTime );
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
