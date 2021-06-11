using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Let's call the default enemy the "Stand Your Ground" type.
    // Start is called before the first frame update
    public GameObject bulletPrefab;

    GameObject player;

    public float fireTimerLength = 5.0f;
    public float weaponRange = 10.0f;
    public float weaponSpeed = 10.0f;
    public float minAccuracy = 5.0f;
    public float maxAccuracy = 90.0f;
    public float startAccuracy = 90.0f;
    public float currentAccuracy {
        get {
            return accuracy;
        }
        set {
            accuracy = Mathf.Clamp(value, minAccuracy, maxAccuracy);
        }
    }
    private float accuracy;

    private float fireTimer;
    void Start()
    {
        fireTimer = 0;
        player = GameObject.FindGameObjectWithTag("Player");
        accuracy = startAccuracy;
    }

    public void Fire(Vector3 positionToShoot) {
        var bullet = Instantiate(bulletPrefab);
        bullet.transform.position = this.transform.position;
        var target = new Vector3(positionToShoot.x + Random.Range(-25.0f, 25.0f)/accuracy, positionToShoot.y + Random.Range(-25.0f, 25.0f)/accuracy, positionToShoot.z) - this.transform.position;
        target.Normalize();
        bullet.GetComponent<Rigidbody2D>().AddForce(target * weaponSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(this.transform.position, player.transform.position - this.transform.position, weaponRange);
        if (hit) {
            if (hit.transform.tag == "Player" && fireTimer <= 0) {
                fireTimer = fireTimerLength;
                Fire(player.transform.position);
            }
        }
        if (fireTimer > 0) {
            fireTimer -= Time.deltaTime;
        }
    }
}
