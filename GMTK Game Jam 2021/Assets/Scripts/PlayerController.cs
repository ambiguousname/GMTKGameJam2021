using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public CameraMove gameCamera;
    public GameObject loseMenu;
    public Image timerSlider;
    [Header("Gameplay Stuff")]
    public float playerSpeed = 1;
    public float maxSpeed = 5;
    public float minAccuracy = 20.0f;
    public float maxAccuracy = 100.0f;
    public float startAccuracy = 40.0f;

    Sprite initSprite;
    public Sprite hitSprite;
    public Sprite grumpySprite;
    public Sprite happySprite;
    public float currentAccuracy
    {
        get
        {
            return accuracy;
        }
        set
        {
            accuracy = Mathf.Clamp(value, minAccuracy, maxAccuracy);
        }
    }
    public float accuracy;

    Rigidbody2D playerRigidbody;

    // Stuff for the timer:
    private float distanceTimer;
    public float distanceTimerInit = 5.0f;
    public float dTimer
    {
        get {
            return distanceTimer;
        }
        set {
            distanceTimer = Mathf.Clamp(value, -5, distanceTimerInit);
        }
    }
    Color timerBaseColor;

    // To handle weapons:
    private Pickup currentPickup;
    [HideInInspector]
    public bool pickupUpdate = false;

    Vector3 initScale;

    // Start is called before the first frame update
    void Start()
    {
        initSprite = GetComponent<SpriteRenderer>().sprite;
        playerRigidbody = GetComponent<Rigidbody2D>();
        distanceTimer = distanceTimerInit;
        accuracy = startAccuracy;
        timerBaseColor = timerSlider.color;
        initScale = this.transform.localScale;
        if (Application.isEditor) {
            Application.targetFrameRate = 60;
        }
    }

    public void SetPickup(Pickup newPickup) {
        for (int i = 0; i < transform.GetChild(0).childCount; i++) {
            Destroy(transform.GetChild(0).GetChild(i).gameObject);
        }
        currentPickup = newPickup;
        currentPickup.OnPickup(this);
        pickupUpdate = true;
    }

    public bool ObjectInFrame(Vector3 objectPosition, Vector2 scale) {
        return gameCamera.GetInFrame(objectPosition, scale);
    }

    public void RegisterBulletHit(float damage) {
        distanceTimer -= damage;
        GetComponent<AudioSource>().pitch = Random.Range(0.8f, 1.2f);
        GetComponent<AudioSource>().Play();
        StartCoroutine("HitTimer");
    }

    IEnumerator HitTimer() {
        Time.timeScale = 0;
        GetComponent<SpriteRenderer>().sprite = hitSprite;
        yield return new WaitForSecondsRealtime(0.05f);
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Shake>().shakeAmount = 0.1f;
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Shake>().shake = 0.1f;
        if (!GameObject.Find("PauseManager").GetComponent<PauseManager>().isPaused)
        {
            Time.timeScale = 1;
        }
        GetComponent<SpriteRenderer>().sprite = initSprite;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameObject.Find("PauseManager").GetComponent<PauseManager>().isPaused && Time.timeScale != 0)
        {
            if (gameCamera.flashing) {
                GetComponent<SpriteRenderer>().sprite = happySprite;
            } else if (ObjectInFrame(this.transform.position, Vector2.one))
            {
                GetComponent<SpriteRenderer>().sprite = grumpySprite;
            } else {
                GetComponent<SpriteRenderer>().sprite = initSprite;
            }
            currentAccuracy = startAccuracy + (gameCamera.sizeAccuracy * (1 / (gameCamera.effectiveSize + 0.01f)));
            var cameraPos = gameCamera.GetCameraPos();
            var target = new Vector3(cameraPos.x, cameraPos.y) - this.transform.position;
            target.Normalize();
            var initTimer = distanceTimer;
            if (!ObjectInFrame(this.transform.position, Vector2.one))
            {
                if (distanceTimer > 0)
                {
                    distanceTimer -= Time.deltaTime;
                    timerSlider.transform.localScale = new Vector2(distanceTimer / distanceTimerInit, timerSlider.transform.localScale.y);
                }
            } else if (distanceTimer > 0 && distanceTimer < distanceTimerInit)
            {
                distanceTimer += Time.deltaTime;
                timerSlider.transform.localScale = new Vector2(distanceTimer / distanceTimerInit, timerSlider.transform.localScale.y);
            }
            if (!ObjectInFrame(this.transform.position, new Vector2(0.3f, 0.3f)) && !Input.GetMouseButton(1) && playerRigidbody.velocity.magnitude < maxSpeed)
            {
                playerRigidbody.AddForce(target * playerSpeed);
            }
            if (initTimer != distanceTimer && distanceTimer > 0)
            {
                timerSlider.color = new Color(timerBaseColor.r, timerBaseColor.g - (distanceTimerInit / distanceTimer) * 0.1f, timerBaseColor.b - (distanceTimerInit / distanceTimer) * 0.1f);
            }

            if (distanceTimer < 0 && !GameObject.Find("PauseManager").GetComponent<PauseManager>().isPaused)
            {
                GameObject.Find("PauseManager").GetComponent<PauseManager>().PauseGame();
                GameObject.Find("PauseManager").GetComponent<PauseManager>().canUnpause = false;
                loseMenu.SetActive(true);
            }
            var enemies = GameObject.FindGameObjectsWithTag("Enemy");
            var visibleEnemies = new List<GameObject>();
            foreach (GameObject enemy in enemies)
            {
                if (ObjectInFrame(enemy.transform.position, Vector2.one))
                {
                    visibleEnemies.Add(enemy);
                    enemy.GetComponent<Enemy>().isVisible = true;
                    var eComponent = enemy.GetComponent<Enemy>();
                    eComponent.currentAccuracy = eComponent.startAccuracy - (gameCamera.sizeAccuracy * 1 / (gameCamera.effectiveSize + 0.01f));
                }
                else if (enemy.GetComponent<Enemy>().currentAccuracy != enemy.GetComponent<Enemy>().startAccuracy)
                {
                    enemy.GetComponent<Enemy>().currentAccuracy = enemy.GetComponent<Enemy>().startAccuracy;
                }
            }
            if (pickupUpdate)
            {
                currentPickup.PickupUpdate(this, visibleEnemies);
            }
        }
    }
}
