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
    void OnTriggerEnter(Collider c)
    {
        if (active)
        { Debug.Log("hey I am active"); }

        if (active && c.gameObject.layer == LayerMask.NameToLayer("Hurtbox"))
        {
            Debug.Log("layer correct!");
            //c should be player hurtbox here 
            Hurtbox h = c.gameObject.GetComponent<Hurtbox>();
            playerFighter = c.gameObject.GetComponentInParent<FighterController>();
            if (playerFighter != null)
            {
                playerFighter.SetTrigger("Stunned");
                Debug.Log("should play stunned anim here");
            }
            if (h != null)
            {
                
                //Debug.Log("oof");
              //  active = false;
              //  h.TakeDamage(_damage);

             //   GameObject.Find("CameraShaker").GetComponent<CameraShaker>().Shake();

                //  transform.root.gameObject.GetComponent<SoundBox>().HitSFX();

              //  GameObject.Find("Flash").GetComponent<ScreenFlash>().Flash();
            }
            else
            {
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
    //basically it should be active for "duration" amount of time
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
