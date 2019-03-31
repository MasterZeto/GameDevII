using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] float maxHitPoints;
    [SerializeField] Hurtbox[] hurtboxes;
    [SerializeField] Image uiHealthBar;

    float hitPoints;

    void Start()
    {
        hitPoints = maxHitPoints;
        UpdateHealthBar();
        for (int i = 0; i < hurtboxes.Length; i++)
        {
            hurtboxes[i].Initialize(TakeDamage);
        }
    }

    void UpdateHealthBar()
    {
        float ratio = hitPoints / maxHitPoints;
        uiHealthBar.rectTransform.localScale = new Vector3(ratio,1,1);
    }

    public void TakeDamage(float damage)
    {
        hitPoints -= damage;
        if (hitPoints < 0.001f)
        {
            hitPoints = 0; 
            Debug.Log("DEAD");
            /* TODO: fix this, we want to do specific things per character
                when they die (player, should have a restart option, for the
                opponent, should proceed to the next scene)
             */
            SceneManager.LoadScene("EndScene"); 
            GameObject.Destroy(this.gameObject); 
        }
        UpdateHealthBar();
    }

    public void HealDamage(float damage)
    {
        hitPoints += damage;
        if (hitPoints > maxHitPoints)
        {
            hitPoints = maxHitPoints;
        }
        UpdateHealthBar();
    }

    public float GetHealthPercent() { return (hitPoints / maxHitPoints); }
}
