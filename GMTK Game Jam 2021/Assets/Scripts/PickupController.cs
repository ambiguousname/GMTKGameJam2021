using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupController : MonoBehaviour
{
    public Pickup pickupType;
    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.tag == "Player");
        if (collision.gameObject.tag == "Player") {
            collision.gameObject.GetComponent<PlayerController>().SetPickup(pickupType);
            Destroy(gameObject);
        }
    }
}
