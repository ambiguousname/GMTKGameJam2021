using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public CameraMove gameCamera;
    public Image timerSlider;
    public float playerSpeed = 1;
    Rigidbody2D playerRigidbody;

    // Stuff for the timer:
    private float distanceTimer;
    public float distanceTimerInit = 5.0f;

    // To handle weapons:
    private Pickup currentPickup;
    [HideInInspector]
    public bool pickupUpdate = false;

    // Start is called before the first frame update
    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        distanceTimer = distanceTimerInit;
    }

    public void SetPickup(Pickup newPickup) {
        currentPickup = newPickup;
        currentPickup.OnPickup(this);
        pickupUpdate = true;
    }

    // Update is called once per frame
    void Update()
    {
        var cameraPos = gameCamera.GetCameraPos();
        var target = new Vector3(cameraPos.x, cameraPos.y) - this.transform.position;
        target.Normalize();
        if (Vector2.Distance(this.transform.position, new Vector3(cameraPos.x, cameraPos.y)) > gameCamera.currentSize)
        {
            if (distanceTimer > 0)
            {
                distanceTimer -= Time.deltaTime;
                timerSlider.transform.localScale = new Vector2(distanceTimer / distanceTimerInit, timerSlider.transform.localScale.y);
                if (distanceTimer < distanceTimerInit && timerSlider.color.g > 0)
                {
                    timerSlider.color = new Color(timerSlider.color.r, timerSlider.color.g - 0.2f * Time.deltaTime, timerSlider.color.b - 0.2f * Time.deltaTime);
                }
            }
            playerRigidbody.AddForce(target * playerSpeed);
        }
        else if (distanceTimer > 0 && distanceTimer < distanceTimerInit) {
            distanceTimer += Time.deltaTime;
            if (timerSlider.color.g < 1)
            {
                timerSlider.color = new Color(timerSlider.color.r, timerSlider.color.g + 0.2f * Time.deltaTime, timerSlider.color.b + 0.2f * Time.deltaTime);
            }
            timerSlider.transform.localScale = new Vector2(distanceTimer / distanceTimerInit, timerSlider.transform.localScale.y);
        }

        if (distanceTimer < 0) {
            UnityEngine.SceneManagement.SceneManager.LoadScene("GameOver");
        }
        if (pickupUpdate) {
            currentPickup.PickupUpdate(this);
        }
    }
}
