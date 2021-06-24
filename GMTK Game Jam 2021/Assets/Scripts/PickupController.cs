using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupController : MonoBehaviour
{
    public Pickup pickupType;
    bool isDestroyed = false;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && !isDestroyed) {
            var volume = PlayerPrefs.GetFloat("soundVolume");
            GetComponent<AudioSource>().volume = volume;
            GetComponent<AudioSource>().Play();
            collision.gameObject.GetComponent<PlayerController>().SetPickup(pickupType);
            isDestroyed = true;
            StartCoroutine("KillPickup");
        }
    }
    IEnumerator KillPickup() {
        GetComponent<SpriteRenderer>().sprite = null;
        GetComponent<BoxCollider2D>().enabled = false;
        yield return new WaitForSeconds(1.0f);
        Destroy(gameObject);
    }
}
