using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField]
    float maxHealth;

    [SerializeField]
    float currentHealth;

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        //Some hit effect here

        if (currentHealth <= 0)
        {
            Debug.Log("You died!");
        }
    }

    public void RecoverHealth(float heal)
    {
        Mathf.Clamp(currentHealth += heal, 0, maxHealth);
    }
}
