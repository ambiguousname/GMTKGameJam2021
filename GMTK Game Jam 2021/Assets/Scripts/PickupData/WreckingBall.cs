using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Pickup", menuName = "PickupData/WreckingBall")]
public class WreckingBall : Pickup
{
    public GameObject bulletPrefab;
    public float bulletSpeed = 1000.0f;
    public float fireRate = 0.5f;
    public float rotateSpeed = 500;
    private float fireTimer;
    private float angle = 0.0f;
    public override void OnPickup(PlayerController player)
    {
        angle = 90.0f;
        fireTimer = 0.0f;
    }
    public override void PickupUpdate(PlayerController player, List<GameObject> visibleEnemiesList)
    {
        angle += rotateSpeed * Time.deltaTime;
        player.dTimer += 1;
        var rad = angle * Mathf.Deg2Rad;
        var newVector = new Vector3(Mathf.Cos(rad) * angle, Mathf.Sin(rad) * angle);
        player.transform.rotation = Quaternion.LookRotation(Vector3.forward, newVector - player.transform.position);
        if (fireTimer <= 0) {
            fireTimer = fireRate;
            var bullet = Instantiate(bulletPrefab);
            bullet.GetComponent<BulletController>().Fire(newVector, player.transform.position, 100, bulletSpeed, "Player", Color.blue, 1.0f);
        }
        if (fireTimer > 0) {
            fireTimer -= Time.deltaTime;
        }
    }

}
