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

    // Start is called before the first frame update
    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        distanceTimer = distanceTimerInit;
        accuracy = startAccuracy;
        timerBaseColor = timerSlider.color;
    }

    public void SetPickup(Pickup newPickup) {
        currentPickup = newPickup;
        currentPickup.OnPickup(this);
        pickupUpdate = true;
    }

    public bool ObjectInFrame(Vector3 objectPosition) {
        return gameCamera.GetInFrame(objectPosition);
    }

    public void RegisterBulletHit() {
        distanceTimer -= 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        if (ObjectInFrame(this.transform.position))
        {
            currentAccuracy = startAccuracy + (gameCamera.sizeAccuracy * (1 / gameCamera.currentSize));
        }
        else {
            currentAccuracy = startAccuracy;
        }
            var cameraPos = gameCamera.GetCameraPos();
        var target = new Vector3(cameraPos.x, cameraPos.y) - this.transform.position;
        target.Normalize();
        var initTimer = distanceTimer;
        if (Vector2.Distance(this.transform.position, new Vector3(cameraPos.x, cameraPos.y)) > 5/2 * GetComponent<Rigidbody2D>().drag)
        {
            if (distanceTimer > 0 && Vector2.Distance(this.transform.position, new Vector3(cameraPos.x, cameraPos.y)) > gameCamera.currentSize)
            {
                distanceTimer -= Time.deltaTime;
                timerSlider.transform.localScale = new Vector2(distanceTimer / distanceTimerInit, timerSlider.transform.localScale.y);
            }
            if (!Input.GetMouseButton(1) && playerRigidbody.velocity.magnitude < maxSpeed)
            {
                playerRigidbody.AddForce(target * playerSpeed);
            }
        }
        if (Vector2.Distance(this.transform.position, new Vector3(cameraPos.x, cameraPos.y)) <= gameCamera.currentSize && distanceTimer > 0 && distanceTimer < distanceTimerInit) {
            distanceTimer += Time.deltaTime;
            timerSlider.transform.localScale = new Vector2(distanceTimer / distanceTimerInit, timerSlider.transform.localScale.y);
        }
        if (initTimer != distanceTimer && distanceTimer > 0) {
            timerSlider.color = new Color(timerBaseColor.r, timerBaseColor.g - (distanceTimerInit/distanceTimer) * 0.1f, timerBaseColor.b - (distanceTimerInit/distanceTimer) * 0.1f);
        }

        if (distanceTimer < 0 && !GameObject.Find("PauseManager").GetComponent<PauseManager>().isPaused) {
            GameObject.Find("PauseManager").GetComponent<PauseManager>().PauseGame();
            GameObject.Find("PauseManager").GetComponent<PauseManager>().canUnpause = false;
            loseMenu.SetActive(true);
        }
        var enemies = GameObject.FindGameObjectsWithTag("Enemy");
        var visibleEnemies = new List<GameObject>();
        foreach (GameObject enemy in enemies)
        {
            if (ObjectInFrame(enemy.transform.position))
            {
                visibleEnemies.Add(enemy);
                enemy.GetComponent<Enemy>().isVisible = true;
                var eComponent = enemy.GetComponent<Enemy>();
                eComponent.currentAccuracy = eComponent.startAccuracy - (gameCamera.sizeAccuracy * 1 / gameCamera.currentSize);
            }
            else if (enemy.GetComponent<Enemy>().currentAccuracy != enemy.GetComponent<Enemy>().startAccuracy)
            {
                enemy.GetComponent<Enemy>().currentAccuracy = enemy.GetComponent<Enemy>().startAccuracy;
            }
        }
        if (pickupUpdate) {
            currentPickup.PickupUpdate(this, visibleEnemies);
        }
    }
}
