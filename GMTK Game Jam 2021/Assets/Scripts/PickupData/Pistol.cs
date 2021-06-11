using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Pickup", menuName = "PickupData/Pistol")]
public class Pistol : Pickup
{
    public float rateOfFire = 1.0f;
    public GameObject pistolPrefab;

    private GameObject pistolSprite;

    public override void OnPickup(PlayerController player)
    {
        pistolSprite = Instantiate(pistolPrefab);
        pistolSprite.transform.position = player.transform.GetChild(0).position;
        return;
    }

    public override void PickupUpdate(PlayerController player, List<GameObject> visibleEnemiesList)
    {
        pistolSprite.transform.position = player.transform.GetChild(0).position;
    }
}
