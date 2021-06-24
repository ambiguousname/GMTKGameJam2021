using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    TrailRenderer trail;
    bool canHit = true;
    string progenitorTag;
    float bDamage;
    // Start is called before the first frame update
    void Start()
    {
        //canHit = true;
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
        GetComponent<AudioSource>().pitch = Random.Range(0.5f, 1.5f);
        var volume = PlayerPrefs.GetFloat("soundVolume");
        GetComponent<AudioSource>().volume = volume;
        GetComponent<AudioSource>().Play();
        progenitorTag = spawnerTagName;
        bDamage = damage;
        this.transform.position = shootFrom;
        // Let's just use hitscan.
        var random = Random.Range(0, 100);
        var target = new Vector3(positionToShoot.x, positionToShoot.y, positionToShoot.z) - shootFrom;
        if (Mathf.Abs(random) > accuracy) // So if we detect a "miss", our shots go flying.
        {
            Debug.Log("Miss " + accuracy);
            var angle = Vector3.Angle(target, this.transform.position) * Mathf.Deg2Rad;
            target += new Vector3(Random.Range(1, 3f) * Random.Range(-1, 1) * Mathf.Cos(angle), Random.Range(1f, 3f) * Random.Range(-1, 1) * Mathf.Sin(angle));
            canHit = false;
        }
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
                other.GetComponent<Enemy>().Damage(bDamage);
            }
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            GetComponent<Collider2D>().enabled = false;
        }
    }
}
