using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActionController : MonoBehaviour
{
    [SerializeField]
    Camera cam;
    private GameObject player;
    private RaycastHit hit;
    private RaycastHit hitGround;
    private LayerMask interactableMask;
    private LayerMask groundMask;

    [SerializeField]
    InventoryManager inventory;
    private Item heldItem;
    private bool hoveringOnPickup;
    private bool hoveringOnMineable;

    [SerializeField]
    GameObject hand;

    [SerializeField]
    GameObject handBack;

    [SerializeField]
    float maxPickupDistance;

    private Vector3 clickDir;
    private float clickAngle;
    private Vector2Int clickDirInt;

    [SerializeField]
    Animator anim;

    [SerializeField]
    bool inAction;

    [SerializeField]
    GameObject swingArc;

    public delegate void PlayerAction();

    public static event PlayerAction OnActionStart;

    public static event PlayerAction OnActionEnd;

    // Start is called before the first frame update
    void Start()
    {
        player = this.transform.parent.gameObject;
        interactableMask = LayerMask.GetMask("Interactable");
        groundMask = LayerMask.GetMask("Ground");
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

        if(Physics.Raycast(ray, out hit, 50, interactableMask))
        {
            if(hit.collider.gameObject.CompareTag("Pickup") && Vector3.Distance(player.transform.position, hit.collider.gameObject.transform.position) <= maxPickupDistance)
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

        if (Input.GetMouseButtonDown(0) && !inAction)
        {
            if (Physics.Raycast(ray, out hitGround, 100))
            {
                clickDir = hitGround.point - player.transform.position;
                clickAngle = Vector3.SignedAngle(clickDir, player.transform.forward, player.transform.up);

                if (clickAngle >= -45 && clickAngle < 45)
                {
                    //Clicked above the player
                    //Debug.Log("Clicked above the player");
                    clickDirInt = new Vector2Int(0, 1);
                }
                else if (clickAngle >= 45 && clickAngle < 135)
                {
                    //Debug.Log("Clicked to the left of the player");
                    //Clicked to the right of the player
                    clickDirInt = new Vector2Int(-1, 0);
                }
                else if (clickAngle >= 135 || clickAngle < -135)
                {
                    //Clicked below the player
                    //Debug.Log("Clicked below the player");
                    clickDirInt = new Vector2Int(0, -1);
                }
                else
                {
                    //Clicked to the right of the player
                    //Debug.Log("Clicked to the right of the player");
                    clickDirInt = new Vector2Int(1, 0);
                }
            }

            //If mouse is hovered over an interactable, should prioritize pickups first
            if (hoveringOnPickup && hit.collider != null)
            {
                anim.SetInteger("Horiz MoveDir", clickDirInt.x);
                anim.SetInteger("Vert MoveDir", clickDirInt.y);
                anim.SetBool("Facing Right", clickDirInt.x > 0);
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
                    ActionStart();
                    anim.SetTrigger("Swing");
                    //Debug.Log("Swinging!");
                }
                else if (heldItem.type == ItemType.Throwable)
                {
                    ActionStart();
                    anim.SetTrigger("Throw");
                    //Debug.Log("Throwing!");
                }
                else if (heldItem.type == ItemType.Consumable)
                {
                    GetComponent<PlayerHealth>().RecoverHealth(heldItem.heal);
                    //Debug.Log("Eating!");
                }
            }
        }
    }

    //Note: need some way to run this if the player drags an item into their selected hotbar
    //Want to avoid running every frame because GetComponentInChildren is costly
    void UpdateHeldItem()
    {
        heldItem = inventory.GetSelectedItem();
        if (heldItem != null)
        {
            hand.GetComponent<SpriteRenderer>().sprite = heldItem.image;
            handBack.GetComponent<SpriteRenderer>().sprite = heldItem.image;
            if (heldItem.type == ItemType.Swingable)
            {
                swingArc.GetComponent<DamageEntity>().SetDamage(heldItem.damage);
            }
        }
        else
        {
            hand.GetComponent<SpriteRenderer>().sprite = null;
            handBack.GetComponent<SpriteRenderer>().sprite = null;
        }
    }

    public void ActionStart()
    {
        OnActionStart();
        inAction = true;
        anim.SetBool("InAction", true);
        anim.SetInteger("Horiz MoveDir", clickDirInt.x);
        anim.SetInteger("Vert MoveDir", clickDirInt.y);
        anim.SetBool("Facing Right", clickDirInt.x > 0);
    }

    public void ActionEnd()
    {
        OnActionEnd();
        inAction = false;
        anim.SetBool("InAction", false);
    }

    public void ThrowHeldItem()
    {
        GameObject thrownItem = Instantiate(heldItem.throwableItemPrefab, hand.transform.position, hand.transform.rotation);
        thrownItem.GetComponent<SpriteRenderer>().flipX = hand.GetComponent<SpriteRenderer>().flipX;

        thrownItem.GetComponent<Rigidbody>().AddForce(Vector3.ClampMagnitude(clickDir, heldItem.throwForce), ForceMode.Impulse);
    }
}
