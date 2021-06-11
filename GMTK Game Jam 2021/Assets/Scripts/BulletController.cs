using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    TrailRenderer trail;
    bool canHit;
    // Start is called before the first frame update
    void Start()
    {
        canHit = true;
        trail = GetComponent<TrailRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        var color = trail.material.GetColor("_Color");
        trail.material.SetColor("_Color",  new Color(color.r, color.g, color.b, color.a - 0.3f * Time.deltaTime));
        if (color.a <= 0) {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (canHit && other.gameObject.tag != "Enemy") {
            canHit = false;
            if (other.gameObject.tag == "Player")
            {
                other.gameObject.GetComponent<PlayerController>().RegisterBulletHit();
            }
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            GetComponent<Collider2D>().enabled = false;
        }
    }
}
