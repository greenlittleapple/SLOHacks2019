using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRestartController : MonoBehaviour {

    public static GameRestartController ins;

    private void Awake()
    {
        if (ins == null)
        {
            ins = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            DestroyImmediate(gameObject);
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public IEnumerator Restart()
    {
        AsyncOperation asyncLoadLevel = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("Main", UnityEngine.SceneManagement.LoadSceneMode.Single);
        while (!asyncLoadLevel.isDone)
        {
            yield return null;
        }
        MainMenuController.ins.PressedPlay();
        MainMenuController.ins.StartedGame();
    }
}
