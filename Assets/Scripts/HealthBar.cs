using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HealthBar : MonoBehaviour
{


    public Image currentHealth;
    float hitPoint = 100;
    float maxHitPoint = 100;

    private void Start()
    {
        UpdateHealthBar();
        



    }

    private void UpdateHealthBar()
    {
        float ratio = hitPoint / maxHitPoint;
        currentHealth.rectTransform.localScale = new Vector3(ratio,1,1);
    }

    void TakeDamage(float damage)
    {
        hitPoint -= damage;
        if(hitPoint<0)
        {
            hitPoint = 0;
            Debug.Log("Die");
        
        }
        UpdateHealthBar();


    }

    void HealDamage(float heal)
    {
        hitPoint += heal;
        if (hitPoint >maxHitPoint)
        {
            hitPoint = maxHitPoint;

        }
        UpdateHealthBar();
    }



}
