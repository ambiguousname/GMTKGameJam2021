using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyCounter : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        var eCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
        if (transform.GetChild(0).gameObject.activeInHierarchy)
        {
            GetComponentInChildren<Text>().text = "" + eCount;
        }
    }
}
