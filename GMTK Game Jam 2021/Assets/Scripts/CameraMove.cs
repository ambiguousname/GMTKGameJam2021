using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public float maxSize = 2;
    public float minSize = 0.4f;
    private float currentScale = 1;
    private Vector3 baseScale;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        baseScale = this.transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = Input.mousePosition + new Vector3(0, 0, -1);
        var scaleChange = Input.GetAxis("Mouse ScrollWheel");
        currentScale += scaleChange;
        if (Vector3.Magnitude(baseScale * currentScale) > Vector3.Magnitude(baseScale * maxSize))
        {
            currentScale -= scaleChange;
        }
        else if (Vector3.Magnitude(baseScale * currentScale) < Vector3.Magnitude(baseScale * minSize)) {
            currentScale -= scaleChange;
        }
        this.transform.localScale = baseScale * currentScale;
    }
}
