using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraMove : MonoBehaviour
{
    public float maxSize = 2;
    public float minSize = 0.4f;
    public float currentSize = 1;

    public float effectiveSize
    {
        get {
            return (currentSize - minSize) / (maxSize - minSize);
        }
    }
    /// <summary>
    /// How much the camera's size buffs player accuracy and debuffs enemy accuracy. (Calculation is accuracy - sizeAccuracy * (1/currentSize)))
    /// </summary>
    public float sizeAccuracy = 80.0f;

    public float cameraSpeed = 3.0f;

    public Camera MainCamera;
    private Vector2 baseScale;
    public bool flashing;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
        var rect = GetComponent<RectTransform>();
        baseScale = rect.sizeDelta;
    }

    public Vector3 GetCameraPos() {
        return MainCamera.ScreenToWorldPoint(this.transform.position);
    }

    public bool GetInFrame(Vector3 position, Vector2 scale) {
        scale *= this.transform.parent.GetComponent<Canvas>().scaleFactor;
        var localPos = MainCamera.WorldToScreenPoint(position);
        var rTransform = GetComponent<RectTransform>();
        var rect = rTransform.rect;
        var newRect = new Rect(this.transform.position.x - (scale.x * rect.width/2), this.transform.position.y - (scale.y * rect.height/2), rect.width * scale.x, rect.height * scale.y);
        return newRect.Contains(localPos);
    }

    public void MoveCamera(Vector3 nextPosition) {
        var backgrounds = GameObject.FindGameObjectsWithTag("Background");
        foreach (GameObject background in backgrounds)
        {
            if (background.GetComponent<SpriteRenderer>().bounds.Contains(new Vector3(MainCamera.transform.position.x + nextPosition.x, MainCamera.transform.position.y + nextPosition.y, background.transform.position.z)))
            {
                MainCamera.GetComponent<Rigidbody2D>().AddForce(nextPosition * cameraSpeed * Screen.width/1000);
                //MainCamera.transform.position = Vector3.Lerp(MainCamera.transform.position, MainCamera.transform.position + nextPosition, cameraSpeed * Time.deltaTime);
                break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!(GameObject.Find("PauseManager").GetComponent<PauseManager>().isPaused || Time.deltaTime == 0))
        {
            var img = GetComponent<Image>();
            if (Input.GetMouseButton(0))
            {
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = false;
            }
            if (Input.GetMouseButtonDown(0) && img.color.a <= 0.38f)
            {
                flashing = true;
                GetComponent<AudioSource>().pitch = Random.Range(0.8f, 1.2f);
                var volume = PlayerPrefs.GetFloat("soundVolume");
                GetComponent<AudioSource>().volume = volume;
                GetComponent<AudioSource>().Play();
                GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Shake>().shakeAmount = 0.1f;
                GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Shake>().shake = 0.1f;
                img.color = new Color(img.color.r + 0.7f, img.color.g + 0.7f, img.color.b + 0.7f, img.color.a + 0.7f);
                var enemies = GameObject.FindGameObjectsWithTag("Enemy");
                foreach (GameObject enemy in enemies)
                {
                    if (GetInFrame(enemy.transform.position, Vector2.one))
                    {
                        enemy.GetComponent<Enemy>().SetStunned();
                    }
                }
            }
            if (img.color.a > 0.38f)
            {
                img.color = new Color(img.color.r - 0.7f * Time.deltaTime, img.color.g - 0.7f * Time.deltaTime, img.color.b - 0.7f * Time.deltaTime, img.color.a - 0.7f * Time.deltaTime);
                if (img.color.a <= 0.38f) {
                    flashing = false;
                }
            }
            var pos = Input.mousePosition;
            this.transform.position = new Vector3(pos.x, pos.y, 1.0f);
            var scaleChange = Input.GetAxis("Mouse ScrollWheel");
            currentSize -= scaleChange;
            if (Vector3.Magnitude(baseScale * currentSize) > Vector3.Magnitude(baseScale * maxSize))
            {
                currentSize += scaleChange;
            }
            else if (Vector3.Magnitude(baseScale * currentSize) < Vector3.Magnitude(baseScale * minSize))
            {
                currentSize += scaleChange;
            }
            var rect = GetComponent<RectTransform>();
            rect.sizeDelta = baseScale * currentSize;
            if (this.transform.localPosition.y >= Screen.height / 7)
            {
                MoveCamera(new Vector3(0, 1));
            }
            if (this.transform.localPosition.y <= -Screen.height / 7)
            {
                MoveCamera(new Vector3(0, -1));
            }
            if (this.transform.localPosition.x >= Screen.width / 7)
            {
                MoveCamera(new Vector3(1, 0));
            }
            if (this.transform.localPosition.x <= -Screen.width / 7)
            {
                MoveCamera(new Vector3(-1, 0));
            }
        }
    }
}
