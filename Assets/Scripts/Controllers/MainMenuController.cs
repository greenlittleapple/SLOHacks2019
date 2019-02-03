using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour {

    public static MainMenuController ins;
    public GameObject title, instructions, playBtn, scores, scoreText, gameOver;

    private void Awake()
    {
        ins = this;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PressedPlay()
    {
        title.SetActive(false);
        instructions.SetActive(true);
    }

    public void StartedGame()
    {
        instructions.SetActive(false);
        scores.SetActive(true);
        PlayerController.ins.gameStarted = true;
        PlayerController.ins.StartCoroutine("SpawnEnemy");
        PlayerController.ins.StartCoroutine("SpawnCoin");
    }

    public void RestartGame()
    {
        GameRestartController.ins.StartCoroutine("Restart");
    }

    public void MainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main");
    }
}
