using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charger : Enemy
{
    public float speed = 2.0f;
    public GameObject slicer;
    // Update is called once per frame
    void Update()
    {
        if (GetPlayer()) {
            var dir = player.transform.position - this.transform.position;
            var rot = Quaternion.LookRotation(Vector3.forward, dir);
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, rot, Time.deltaTime * 2);
            if (Quaternion.Angle(this.transform.rotation, rot) < 10)
            {
                GetComponent<Rigidbody2D>().AddForce(dir.normalized * speed);
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.transform.tag == "Player" && fireTimer <= 0) {
            if (Random.Range(0, 100) <= currentAccuracy)
            {
                fireTimer = fireTimerLength;
                player.GetComponent<PlayerController>().dTimer -= 0.5f;
                slicer.GetComponent<Animator>().Play("Slicing");
                slicer.SetActive(true);
            }
        }
        if (fireTimer > 0) {
            fireTimer -= Time.deltaTime;
        }
        UpdateStun();
    }
}
