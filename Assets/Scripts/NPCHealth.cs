using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCHealth : MonoBehaviour
{
    [SerializeField]
    float maxHealth;

    [SerializeField]
    float currentHealth;

    
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        //Some hit effect here

        if(currentHealth <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
