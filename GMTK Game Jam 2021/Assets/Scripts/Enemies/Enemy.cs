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
    public float health = 1.0f;
    bool dying = false;

    private float stunTimer;
    private float bulletTimer = 0;
    Sprite initialSprite;

    public Sprite hitSprite;

    public bool isVisible = false;
    public float currentAccuracy {
        get {
            if (stunTimer > 0)
            {
                return 0;
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
        initialSprite = GetComponent<SpriteRenderer>().sprite;
        player = GameObject.FindGameObjectWithTag("Player");
        accuracy = startAccuracy;
        var main = GetComponent<ParticleSystem>().main;
        main.startColor = transform.GetChild(1).GetComponent<SpriteRenderer>().color;
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
                // Make sure that we won't fire immediately as stunning ends
                fireTimer = fireTimerLength;
                transform.GetChild(0).gameObject.SetActive(false);
            }
        }
    }

    public void Damage(float healthAmount) {
        health -= healthAmount;
        GetComponent<SpriteRenderer>().sprite = hitSprite;
        StartCoroutine("Kill");
    }

    IEnumerator Kill() {
        Time.timeScale = 0;
        dying = true;
        yield return new WaitForSecondsRealtime(0.05f);
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Shake>().shakeAmount = 0.05f;
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Shake>().shake = 0.05f;
        if (!GameObject.Find("PauseManager").GetComponent<PauseManager>().isPaused)
        {
            Time.timeScale = 1;
        }
            GetComponent<SpriteRenderer>().sprite = initialSprite;
        if (health <= 0.0f)
        {
            GetComponent<SpriteRenderer>().sprite = null;
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<Rigidbody2D>().isKinematic = true;
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(false);
            this.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            GetComponent<ParticleSystem>().Play();
            yield return new WaitForSeconds(1f);
            if (GameObject.FindGameObjectsWithTag("Enemy").Length == 1)
            {
                GameObject.Find("PauseManager").GetComponent<PauseManager>().GetWin();
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
    }

    // Update is called once per frame
    void Update() {
        if (!(GameObject.Find("PauseManager").GetComponent<PauseManager>().isPaused || Time.timeScale == 0) && !dying)
        {
            var playerHit = GetPlayer();
            if (playerHit && fireTimer <= 0)
            {
                fireTimer = fireTimerLength;
                bulletTimer = bulletsPerSecond;
                Fire(player.transform.position);
            }
            if (fireTimer > 0)
            {
                fireTimer -= Time.deltaTime;
                if (playerHit && bulletTimer <= 0)
                {
                    bulletTimer = bulletsPerSecond;
                    Fire(player.transform.position);
                }
                else if (bulletTimer > 0)
                {
                    bulletTimer -= Time.deltaTime;
                }
            }
            UpdateStun();
        }
    }
}
