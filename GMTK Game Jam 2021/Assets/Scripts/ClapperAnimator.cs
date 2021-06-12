using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClapperAnimator : MonoBehaviour
{
    bool isRunning = true;
    string stage = "raise";
    // Start is called before the first frame update
    void Start()
    {
        TakeCount.currentTake += 1;
        this.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "" + TakeCount.currentScene;
        this.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = "" + TakeCount.currentTake;
    }

    // Update is called once per frame
    void Update()
    {
        if (isRunning) {
            if (!GameObject.Find("PauseManager").GetComponent<PauseManager>().isPaused && stage != "disappear")
            {
                GameObject.Find("PauseManager").GetComponent<PauseManager>().PauseGame();
                GameObject.Find("PauseManager").GetComponent<PauseManager>().canUnpause = false;
            }
            if (stage == "raise")
            {
                var currRotation = this.transform.GetChild(1).rotation;
                this.transform.GetChild(1).rotation = Quaternion.Euler(new Vector3(currRotation.eulerAngles.x, currRotation.eulerAngles.y, currRotation.eulerAngles.z + 0.3f));
                if (currRotation.eulerAngles.z > 50)
                {
                    stage = "lower";
                }
            }
            else if (stage == "lower")
            {
                var currRotation = this.transform.GetChild(1).rotation;
                this.transform.GetChild(1).rotation = Quaternion.Euler(new Vector3(currRotation.eulerAngles.x, currRotation.eulerAngles.y, currRotation.eulerAngles.z - 0.3f));
                if (currRotation.eulerAngles.z <= 10)
                {
                    stage = "disappear";
                    GameObject.Find("PauseManager").GetComponent<PauseManager>().canUnpause = true;
                    GameObject.Find("PauseManager").GetComponent<PauseManager>().PauseGame();
                }
            }
            else if (stage == "disappear") {
                var currRotation = this.transform.GetChild(1).rotation;
                if (currRotation.eulerAngles.z > 1)
                {
                    this.transform.GetChild(1).rotation = Quaternion.Euler(new Vector3(currRotation.eulerAngles.x, currRotation.eulerAngles.y, currRotation.eulerAngles.z - 0.5f));
                }
                this.transform.position -= new Vector3(0, 1.0f, 0);
                if (this.transform.position.y < -Screen.height) {
                    isRunning = false;
                }
            }
        }
    }
}
