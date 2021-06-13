using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraMove : MonoBehaviour
{
    public float maxSize = 2;
    public float minSize = 0.4f;
    public float currentSize = 1;
    /// <summary>
    /// How much the camera's size buffs player accuracy and debuffs enemy accuracy. (Calculation is accuracy - sizeAccuracy * (1/currentSize)))
    /// </summary>
    public float sizeAccuracy = 80.0f;
    public Camera MainCamera;
    private Vector3 baseScale;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
        baseScale = this.transform.localScale;
    }

    public Vector3 GetCameraPos() {
        return MainCamera.ScreenToWorldPoint(this.transform.position);
    }

    public bool GetInFrame(Vector3 position) {
        var localPos = MainCamera.WorldToScreenPoint(position);
        var testPos = new Vector3(localPos.x, localPos.y, -1.0f);
        var rTransform = GetComponent<RectTransform>().rect;
        var newRect = new Rect(this.transform.position.x - (rTransform.width * currentSize/2), this.transform.position.y - (rTransform.height * currentSize / 2), rTransform.width * currentSize, rTransform.height * currentSize);
        return newRect.Contains(testPos);
    }

    public void MoveCamera(Vector3 nextPosition) {
        var backgrounds = GameObject.FindGameObjectsWithTag("Background");
        foreach (GameObject background in backgrounds)
        {
            if (background.GetComponent<SpriteRenderer>().bounds.Contains(new Vector3(MainCamera.transform.position.x + nextPosition.x, MainCamera.transform.position.y + nextPosition.y, background.transform.position.z)))
            {
                MainCamera.transform.position = Vector3.Lerp(MainCamera.transform.position, MainCamera.transform.position + nextPosition, 5 * Time.deltaTime);
                break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameObject.Find("PauseManager").GetComponent<PauseManager>().isPaused)
        {
            var img = GetComponent<Image>();
            if (Input.GetMouseButton(0))
            {
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = false;
            }
            if (Input.GetMouseButtonDown(0) && img.color.a <= 0.38f)
            {
                GetComponent<AudioSource>().pitch = Random.Range(0.8f, 1.2f);
                GetComponent<AudioSource>().Play();
                GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Shake>().shakeAmount = 0.1f;
                GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Shake>().shake = 0.1f;
                img.color = new Color(img.color.r + 0.7f, img.color.g + 0.7f, img.color.b + 0.7f, img.color.a + 0.7f);
                var enemies = GameObject.FindGameObjectsWithTag("Enemy");
                foreach (GameObject enemy in enemies)
                {
                    if (GetInFrame(enemy.transform.position))
                    {
                        enemy.GetComponent<Enemy>().SetStunned();
                    }
                }
            }
            if (img.color.a > 0.38f)
            {
                img.color = new Color(img.color.r - 0.7f * Time.deltaTime, img.color.g - 0.7f * Time.deltaTime, img.color.b - 0.7f * Time.deltaTime, img.color.a - 0.7f * Time.deltaTime);
            }
            this.transform.position = Input.mousePosition + new Vector3(0, 0, -1);
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
            this.transform.localScale = baseScale * currentSize;
            if (this.transform.localPosition.y >= Screen.height / 2 - Screen.height / 3)
            {
                MoveCamera(new Vector3(0, 3));
            }
            if (this.transform.localPosition.y <= -Screen.height / 2 + Screen.height / 3)
            {
                MoveCamera(new Vector3(0, -3));
            }
            if (this.transform.localPosition.x >= Screen.width / 2 - Screen.width / 3)
            {
                MoveCamera(new Vector3(3, 0));
            }
            if (this.transform.localPosition.x <= -Screen.width / 2 + Screen.width / 3)
            {
                MoveCamera(new Vector3(-3, 0));
            }
        }
    }
}
