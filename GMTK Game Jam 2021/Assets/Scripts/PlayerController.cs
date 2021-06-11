using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public CameraMove gameCamera;
    public float playerSpeed = 1;
    Rigidbody2D playerRigidbody;
    Vector3 prevPos;
    // Start is called before the first frame update
    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        var cameraPos = gameCamera.GetCameraPos();
        var target = new Vector3(cameraPos.x, cameraPos.y) - this.transform.position;
        target.Normalize();
        if (Vector2.Distance(this.transform.position, new Vector3(cameraPos.x, cameraPos.y)) > 1.0f)
        {
            playerRigidbody.AddForce(target * playerSpeed);
        }
    }
}
