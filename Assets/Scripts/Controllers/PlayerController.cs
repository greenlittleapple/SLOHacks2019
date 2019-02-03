using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public static PlayerController ins;
    public int lives = 3, activeHearts = 3;
    public int side = 1;
    public bool gameStarted = false, invincible;
    public GameObject[] buses, hearts;
    public Transform[] destinations;
    public int score = 0;
    public float scoreMult = 1;
    public UnityEngine.UI.Text scoreObj, coinBonus;
    public Animator road;
    public Transform spawnPoint;
    GameObject activeBus;

    private void Awake()
    {
        ins = this;
        Screen.SetResolution(768, 1024, false);
    }

    // Use this for initialization
    void Start()
    {
        activeBus = buses[side];
        SwipeDetector.OnSwipe += ((x) =>
        {
            if(gameStarted && (x.Direction == SwipeDirection.Left || x.Direction == SwipeDirection.Right))
                StartCoroutine("DelaySide", x.Direction == SwipeDirection.Left ? -1 : 1);
        });
    }

    // Update is called once per frame
    void Update()
    {
        if (gameStarted)
        {
            scoreMult += .04f * Time.deltaTime;
            score += (int)scoreMult;
            scoreObj.text = score.ToString();
            road.speed += .025f * Time.deltaTime;
            if ((Input.GetKeyDown(KeyCode.LeftArrow)))
                StartCoroutine("DelaySide", -1);
            else if (Input.GetKeyDown(KeyCode.RightArrow))
                StartCoroutine("DelaySide", 1);
            if (activeBus != buses[side])
            {
                activeBus.SetActive(false);
                activeBus = buses[side];
                activeBus.SetActive(true);
            }
            if (lives != activeHearts)
            {
                hearts[--activeHearts].GetComponent<Animator>().SetBool("kill", true);
                if (lives == 0)
                {
                    gameStarted = false;
                    road.speed = 0;
                    spawnPoint.gameObject.SetActive(false);
                    activeBus.GetComponent<Animator>().speed = 0;
                    MainMenuController.ins.gameOver.SetActive(true);
                    MainMenuController.ins.scoreText.GetComponent<UnityEngine.UI.Text>().text = score.ToString();
                }
            }
        }
    }

    public IEnumerator SpawnEnemy()
    {
        Instantiate(Resources.Load<GameObject>("Prefabs/Obstacle"), spawnPoint.position, spawnPoint.rotation, spawnPoint);
        yield return new WaitForSeconds(3f / scoreMult);
        if (gameStarted)
            StartCoroutine("SpawnEnemy");
    }

    public IEnumerator SpawnCoin()
    {
        if (Random.value > 0.6f)
        {
            Instantiate(Resources.Load<GameObject>("Prefabs/Coin"), spawnPoint.position, spawnPoint.rotation, spawnPoint);
        }
        yield return new WaitForSeconds(.5f / scoreMult);
        if (gameStarted)
            StartCoroutine("SpawnCoin");
    }

    public IEnumerator CoinBonus(int bonus)
    {
        coinBonus.text = "+" + bonus.ToString();
        score += bonus;
        yield return new WaitForSeconds(2f);
        coinBonus.text = "";
    }

    IEnumerator DelaySide(int i)
    {
        yield return new WaitForSeconds(.1f);
        side += i;
        side = Mathf.Clamp(side, 0, 2);
    }

    IEnumerator HitInvincibility()
    {
        SpriteRenderer x = activeBus.GetComponent<SpriteRenderer>();
        invincible = true;
        Color orig = x.color, a = new Color(1, .5f, .5f, .5f);
        x.color = a;
        yield return new WaitForSeconds(.5f);
        x.color = orig;
        yield return new WaitForSeconds(.5f);
        x.color = a;
        yield return new WaitForSeconds(.5f);
        x.color = orig;
        invincible = false;
    }
}
