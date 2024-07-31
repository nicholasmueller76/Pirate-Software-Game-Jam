using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrownProjectile : MonoBehaviour
{
    [SerializeField]
    float damage;

    [SerializeField]
    Rigidbody rb;

    [SerializeField]
    bool sticksInEntities;

    [SerializeField]
    Vector3 pickupRotation;

    [SerializeField]
    float pickupYOffset;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.eulerAngles = pickupRotation;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("NPC"))
        {
            if(sticksInEntities)
            {
                rb.isKinematic = true;
                this.transform.parent = other.gameObject.transform;
            }
            else
            {
                rb.velocity = Vector3.zero;
            }

            other.gameObject.GetComponent<NPCHealth>().TakeDamage(damage);
        }
        else if(other.gameObject.CompareTag("Ground"))
        {
            transform.eulerAngles = pickupRotation;
            transform.position = new Vector3(transform.position.x, other.gameObject.transform.position.y + pickupYOffset, transform.position.z);

            rb.velocity = Vector3.zero;
            rb.isKinematic = true;
            this.gameObject.tag = "Pickup";
            Destroy(this);
        }
    }
}
