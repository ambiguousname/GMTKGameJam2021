using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Pickup", menuName = "PickupData/Pistol")]
public class Pistol : Pickup
{
    public float rateOfFire = 1.0f;
    public float range = 10.0f;
    public float bulletSpeed = 500.0f;
    private float fireTimer = 0.0f;
    public GameObject pistolPrefab;
    public GameObject bulletPrefab;

    private GameObject pistolSprite;

    public override void OnPickup(PlayerController player)
    {
        pistolSprite = Instantiate(pistolPrefab, player.transform.GetChild(0));
        pistolSprite.transform.position = player.transform.GetChild(0).position;
        return;
    }

    public override void PickupUpdate(PlayerController player, List<GameObject> visibleEnemiesList)
    {
        pistolSprite.transform.position = player.transform.GetChild(0).position;
        if (visibleEnemiesList.Count > 0) {
            var closestEnemy = visibleEnemiesList[0];
            var closestDist = Vector2.Distance(closestEnemy.transform.position, player.transform.position);
            foreach (GameObject enemy in visibleEnemiesList)
            {
                var newDist = Vector2.Distance(enemy.transform.position, player.transform.position);
                if (newDist < closestDist)
                {
                    closestDist = newDist;
                    closestEnemy = enemy;
                }
            }
            var dir = closestEnemy.transform.position - player.transform.position;
            var lookDir = Quaternion.LookRotation(Vector3.forward, dir);
            player.transform.rotation = Quaternion.Slerp(player.transform.rotation, lookDir, Time.deltaTime * 5);
            var bulletsOut = pistolSprite.transform.GetChild(0).GetChild(0).transform.position;
            var mask = LayerMask.GetMask("Enemies", "Obstacles");
            RaycastHit2D hit = Physics2D.Raycast(bulletsOut, closestEnemy.transform.position - bulletsOut, range, mask);
            if (hit && Quaternion.Angle(player.transform.rotation, lookDir) <= 5)
            {
                if (hit.transform.tag == "Enemy" && fireTimer <= 0)
                {
                    fireTimer = rateOfFire;
                    var bullet = Instantiate(bulletPrefab);
                    Debug.Log(player.currentAccuracy);
                    bullet.GetComponent<BulletController>().Fire(closestEnemy.transform.position, bulletsOut, player.currentAccuracy, bulletSpeed, "Player", Color.blue);
                }
            }
        }
        if (fireTimer > 0) {
            fireTimer -= Time.deltaTime;
        }
    }
}
