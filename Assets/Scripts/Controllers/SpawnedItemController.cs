using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnedItemController : MonoBehaviour {

    protected int lane;
    protected Transform target;
    protected Vector2 initPos, initScale = new Vector2(.1f, .1f);
    protected float time, speed = 0f;
    protected Rigidbody2D rb;

    // Use this for initialization
    protected void Start () {
        rb = GetComponent<Rigidbody2D>();
        initPos = transform.position;
        lane = Random.value < .5f ? PlayerController.ins.side : Random.Range(0, 2);
        target = PlayerController.ins.destinations[lane];
        StartCoroutine("EaseIn");
    }
	
	// Update is called once per frame
	void Update () {
        
	}

    protected IEnumerator EaseIn()
    {
        rb.velocity = speed * ((Vector2) target.position - initPos);
        transform.localScale = speed * Vector2.one;
        print(rb.velocity);
        yield return new WaitForSeconds(1f / PlayerController.ins.scoreMult);
        while (speed < 1f)
        {
            speed += Time.deltaTime * .15f * PlayerController.ins.scoreMult;
            rb.velocity += .25f * speed * ((Vector2)target.position - initPos).normalized;
            if (transform.localScale.x < 1)
            {
                transform.localScale += Vector3.one * speed * .05f;
            }
            yield return new WaitForEndOfFrame();
        }
    }

    protected void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.StartsWith("Bus"))
        {
            if (name.StartsWith("Coin"))
            {
                PlayerController.ins.StopCoroutine("CoinBonus");
                PlayerController.ins.StartCoroutine("CoinBonus", (int)(100 * PlayerController.ins.scoreMult));
            }
            else
            {
                if (!PlayerController.ins.invincible)
                {
                    PlayerController.ins.StartCoroutine("HitInvincibility");
                    PlayerController.ins.lives--;
                }
            }
            Destroy(gameObject);
        } else if(collision.gameObject.name == "KILLZONE")
            Destroy(gameObject);

    }
}
