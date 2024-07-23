using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActionController : MonoBehaviour
{
    private Camera cam;
    private GameObject player;
    private RaycastHit hit;
    private LayerMask mask;
    [SerializeField]
    InventoryManager inventory;
    private Item heldItem;
    private bool hoveringOnPickup;
    private bool hoveringOnMineable;

    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
        player = this.transform.parent.gameObject;
        mask = LayerMask.GetMask("Interactable");
    }

    private void OnEnable()
    {
        InventoryManager.OnHeldItemChanged += UpdateHeldItem;
    }

    private void OnDisable()
    {
        InventoryManager.OnHeldItemChanged -= UpdateHeldItem;
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(ray, out hit, 50, mask))
        {
            if(hit.collider.gameObject.CompareTag("Pickup"))
            {
                hoveringOnPickup = true;
            }
            else
            {
                hoveringOnPickup = false;
            }
            
            if(hit.collider.gameObject.CompareTag("Mineable"))
            {
                //Need check for if held item is correct tool
                hoveringOnMineable = true;
            }
            else
            {
                hoveringOnMineable = false;
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            //If mouse is hovered over an interactable, should prioritize pickups first
            if(hoveringOnPickup)
            {
                GameObject item = hit.collider.gameObject;
                inventory.AddItem(item.GetComponent<Pickup>().GetItemData());
                
                //May want to cache items later to improve load times
                Destroy(item);
                hoveringOnPickup = false;
            }
            else if(hoveringOnMineable)
            {
                //Stuff here for breaking rocks, trees
            }
            else if (heldItem != null)
            {
                if (heldItem.type == ItemType.Swingable)
                {
                    Debug.Log("Swinging!");
                }
                else if (heldItem.type == ItemType.Throwable)
                {
                    Debug.Log("Throwing!");
                }
                else if (heldItem.type == ItemType.Consumable)
                {
                    Debug.Log("Eating!");
                }
            }
        }
    }

    //Note: need some way to run this if the player drags an item into their selected hotbar
    //Want to avoid running every frame because GetComponentInChildren is costly
    void UpdateHeldItem()
    {
        heldItem = inventory.GetSelectedItem();
    }
}
