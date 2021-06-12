using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyCounter : MonoBehaviour
{
    public string NextScene;
    // Update is called once per frame
    void Update()
    {
        if (!GameObject.Find("PauseManager").GetComponent<PauseManager>().isPaused)
        {
            var eCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
            GetComponentInChildren<Text>().text = "" + eCount;
            if (eCount <= 0)
            {
                TakeCount.currentScene += 1;
                UnityEngine.SceneManagement.SceneManager.LoadScene(NextScene);
            }
        }
    }
}
