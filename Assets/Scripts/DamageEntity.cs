using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageEntity : MonoBehaviour
{
    [SerializeField]
    bool damagePlayer;
    [SerializeField]
    bool damageNPCs;
    [SerializeField]
    bool damageMineables;

    [SerializeField]
    float damage;

    private void OnTriggerEnter(Collider other)
    {
        if(damagePlayer && other.gameObject.CompareTag("Player"))
        {

        }
        else if(damageNPCs && other.gameObject.CompareTag("NPC"))
        {
            other.gameObject.GetComponent<NPCHealth>().TakeDamage(damage);
        }
        else if(damageMineables && other.gameObject.CompareTag("Mineable"))
        {

        }
    }

    public void SetDamage(float damage)
    {
        this.damage = damage;
    }
}
