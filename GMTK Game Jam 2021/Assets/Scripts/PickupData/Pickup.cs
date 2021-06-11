using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Pickup", menuName ="PickupData/Pickup")]
public class Pickup : ScriptableObject
{
    public virtual void OnPickup(PlayerController player) { 
    
    }
}
