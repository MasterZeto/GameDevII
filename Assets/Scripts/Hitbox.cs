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
    [SerializeField] Collider _collider;

    public bool active { get; private set; }
    public float cooldown;

    void Start() { active = false; cooldown = -1f; }

    void OnTriggerStay(Collider c)
    {
        if (active)
        {
            //Debug.Log("entered and active");
            Hurtbox h = c.gameObject.GetComponent<Hurtbox>();
            if (h != null)
            {
                //Debug.Log("oof");
                active = false;
                h.TakeDamage(_damage);

                GameObject.Find("CameraShaker").GetComponent<CameraShaker>().Shake();

                GameObject.FindWithTag("Player").GetComponent<SoundBox>().HitSFX();

                GameObject.Find("Flash").GetComponent<ScreenFlash>().Flash();
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

    IEnumerator FireRoutine(float duration)
    {
        cooldown = duration;
        while (cooldown >= 0f)
        {
            cooldown -= Time.deltaTime;
            yield return null;
        }
        active = false;
    }

}
