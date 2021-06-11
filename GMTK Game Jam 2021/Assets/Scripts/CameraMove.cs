using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        // We subtract our rect transform's position because the rectangle's boundaries itself are static, meaning we need a way
        // to get the testPosition relative to our rect.
        return GetComponent<RectTransform>().rect.Contains(testPos - GetComponent<RectTransform>().position);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1")) {
            Cursor.visible = false;
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
