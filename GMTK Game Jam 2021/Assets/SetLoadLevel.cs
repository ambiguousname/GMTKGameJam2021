using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetLoadLevel : MonoBehaviour
{
    public LevelsList levels;
    public GameObject levelSelectorPrefab;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < levels.levelsList.Length; i++) {
            if (levels.levelsList[i] != "YouWin")
            {
                var selector = Instantiate(levelSelectorPrefab, this.transform);
                selector.GetComponentInChildren<Text>().text = "" + i;
                var x = i;
                selector.GetComponent<Button>().onClick.AddListener(delegate { StartLevel(x); });
            }
        }
    }

    void StartLevel(int index) {
        GetComponent<AudioSource>().Play();
        TakeCount.currentScene = index + 1;
        TakeCount.currentTake = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
        UnityEngine.SceneManagement.SceneManager.LoadScene(levels.levelsList[index]);
    }
}
