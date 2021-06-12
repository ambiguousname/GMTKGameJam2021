using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    TrailRenderer trail;
    bool canHit;
    string progenitorTag;
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

    public void Fire(Vector3 positionToShoot, Vector3 shootFrom, float accuracy, float speed, string spawnerTagName, Color tint) {
        progenitorTag = spawnerTagName;
        this.transform.position = shootFrom;
        var target = new Vector3(positionToShoot.x, positionToShoot.y, positionToShoot.z) - shootFrom;
        target.Normalize();
        target += new Vector3(Random.Range(-25.0f, 25.0f) / accuracy,  Random.Range(-25.0f, 25.0f)/ accuracy);
        this.GetComponent<Rigidbody2D>().AddForce(target * speed);
        GetComponent<TrailRenderer>().material.SetColor("_Color", tint);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (canHit && progenitorTag != other.gameObject.tag) {
            canHit = false;
            if (other.gameObject.tag == "Player")
            {
                other.gameObject.GetComponent<PlayerController>().RegisterBulletHit();
            }
            if (other.gameObject.tag == "Enemy") {
                Destroy(other.gameObject);
            }
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            GetComponent<Collider2D>().enabled = false;
        }
    }
}
