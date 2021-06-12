using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    TrailRenderer trail;
    bool canHit;
    string progenitorTag;
    float bDamage;
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

    public void Fire(Vector3 positionToShoot, Vector3 shootFrom, float accuracy, float speed, string spawnerTagName, Color tint, float damage) {
        progenitorTag = spawnerTagName;
        bDamage = damage;
        this.transform.position = shootFrom;
        var target = new Vector3(positionToShoot.x, positionToShoot.y, positionToShoot.z) - shootFrom;
        var angle = Vector3.Angle(target, this.transform.position) * Mathf.Deg2Rad;
        Debug.DrawRay(shootFrom, target, Color.red);
        target += (1 - accuracy/100) * new Vector3(Mathf.Cos(angle) * Random.Range(-800/accuracy, 800/accuracy), Mathf.Sin(angle) * Random.Range(-800/accuracy, 800/accuracy ));
        Debug.DrawRay(shootFrom, target);
        target.Normalize();
        this.GetComponent<Rigidbody2D>().AddForce(target * speed);
        GetComponent<TrailRenderer>().material.SetColor("_Color", tint);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (canHit && progenitorTag != other.gameObject.tag) {
            canHit = false;
            if (other.gameObject.tag == "Player")
            {
                other.gameObject.GetComponent<PlayerController>().RegisterBulletHit(bDamage);
            }
            if (other.gameObject.tag == "Enemy") {
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().dTimer += 0.5f;
                Destroy(other.gameObject);
            }
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            GetComponent<Collider2D>().enabled = false;
        }
    }
}
