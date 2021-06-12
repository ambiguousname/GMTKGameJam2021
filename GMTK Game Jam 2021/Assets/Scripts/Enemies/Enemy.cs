using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Let's call the default enemy the "Stand Your Ground" type.
    // Start is called before the first frame update
    public GameObject bulletPrefab;

    public GameObject player;

    public float fireTimerLength = 5.0f;
    public float bulletsPerSecond = 1.0f;
    public float weaponRange = 10.0f;
    public float weaponSpeed = 10.0f;
    public float minAccuracy = 5.0f;
    public float maxAccuracy = 90.0f;
    public float startAccuracy = 90.0f;
    public float enemyDamage = .5f;
    public float stunDuration = 3.0f;

    private float stunTimer;
    private float bulletTimer = 0;

    public bool isVisible = false;
    public float currentAccuracy {
        get {
            if (stunTimer > 0)
            {
                return 1;
            }
            else
            {
                return accuracy;
            }
        }
        set {
            accuracy = Mathf.Clamp(value, minAccuracy, maxAccuracy);
        }
    }
    private float accuracy;

    [HideInInspector]
    public float fireTimer;
    void Start()
    {
        fireTimer = 0;
        player = GameObject.FindGameObjectWithTag("Player");
        accuracy = startAccuracy;
    }

    public void Fire(Vector3 positionToShoot) {
        var bullet = Instantiate(bulletPrefab);
        bullet.GetComponent<BulletController>().Fire(positionToShoot, this.transform.position, currentAccuracy, weaponSpeed, "Enemy", Color.yellow, enemyDamage);
    }

    public void SetStunned() {
        stunTimer = stunDuration;
        transform.GetChild(0).gameObject.SetActive(true);
    }

    public GameObject GetPlayer() {
        var mask = LayerMask.GetMask("Player", "Obstacles");
        RaycastHit2D hit = Physics2D.Raycast(this.transform.position, player.transform.position - this.transform.position, weaponRange, mask);
        if (hit && hit.transform.tag == "Player")
        {
            return hit.transform.gameObject;
        }
        else {
            return null;
        }
    }

    public void UpdateStun() {
        if (stunTimer > 0)
        {
            stunTimer -= Time.deltaTime;
            if (stunTimer <= 0)
            {
                transform.GetChild(0).gameObject.SetActive(false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        var playerHit = GetPlayer();
        if (playerHit && fireTimer <= 0) {
            fireTimer = fireTimerLength;
            bulletTimer = bulletsPerSecond;
            Fire(player.transform.position);
        }
        if (fireTimer > 0) {
            fireTimer -= Time.deltaTime;
            if (playerHit && bulletTimer <= 0)
            {
                bulletTimer = bulletsPerSecond;
                Fire(player.transform.position);
            }
            else if (bulletTimer > 0) {
                bulletTimer -= Time.deltaTime;
            }
        }
        UpdateStun();
    }
}
