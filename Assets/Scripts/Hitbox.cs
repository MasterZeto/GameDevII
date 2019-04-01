using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CommandPattern;

// make sure this is on the same layer as only other hurt boxes.
// so the players hitboxes are gonna be on the enemy hurtbox layer
// and the enemy hitboxes are gonna be on the player hurtbox layer
public class Hitbox : MonoBehaviour
{
    [SerializeField] float _damage;
    public Collider _collider;

    public bool active { get; private set; }
    public float cooldown;
    
    float timescale = 1f;
  
    void Start() { active = false; cooldown = -1f; }

    void OnTriggerStay(Collider c)
    {
        if (active)
        {
            Debug.Log("entered and active for player hitbox");
            Debug.Log(c.gameObject.layer);

            Hurtbox h = c.gameObject.GetComponent<Hurtbox>();

            if (h != null)
            {
                Debug.Log("opponent hurtbox get");
                active = false;
                h.TakeDamage(_damage);

                GameObject.Find("CameraShaker").GetComponent<CameraShaker>().Shake();

            //  transform.root.gameObject.GetComponent<SoundBox>().HitSFX();

                GameObject.Find("Flash").GetComponent<ScreenFlash>().Flash();
            } else {
           //    transform.root.gameObject.GetComponent<SoundBox>().MissSFX();
            }
        }
    }

    public void Fire(float duration)
    {
        if (!active && cooldown <= 0f)
        {
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

    IEnumerator FireRoutine(float duration)
    {
        cooldown = duration;
        while (cooldown >= 0f)
        {
            cooldown -= Time.deltaTime * timescale;
            yield return null;
        }
        active = false;
    }

}
