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

    // Update is called once per frame
    void Update()
    {
        var img = GetComponent<Image>();
        if (Input.GetMouseButtonDown(0) && img.color.a <= 0.38f) {
            img.color = new Color(img.color.r + 0.7f, img.color.g + 0.7f, img.color.b + 0.7f, img.color.a + 0.7f);
            var enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemy in enemies) {
                if (GetInFrame(enemy.transform.position)) {
                    enemy.GetComponent<Enemy>().SetStunned();
                }
            }
        }
        if (img.color.a > 0.38f) {
            img.color = new Color(img.color.r - 0.7f * Time.deltaTime, img.color.g - 0.7f * Time.deltaTime, img.color.b - 0.7f * Time.deltaTime, img.color.a - 0.7f * Time.deltaTime);
        }
        this.transform.position = Input.mousePosition + new Vector3(0, 0, -1);
        var scaleChange = Input.GetAxis("Mouse ScrollWheel");
        currentSize += scaleChange;
        if (Vector3.Magnitude(baseScale * currentSize) > Vector3.Magnitude(baseScale * maxSize))
        {
            currentSize -= scaleChange;
        }
        else if (Vector3.Magnitude(baseScale * currentSize) < Vector3.Magnitude(baseScale * minSize)) {
            currentSize -= scaleChange;
        }
        this.transform.localScale = baseScale * currentSize;
    }
}
