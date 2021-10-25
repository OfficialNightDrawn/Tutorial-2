using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody2D rd2d;
    public float speed;
    public float jumpForce;
    public Text score;
    public Text lives;
    public GameObject winTextObject;
    public GameObject loseTextObject;
    private int scoreValue = 0;
    private int livesValue = 3;
    private bool facingRight = true;
    private bool isOnGround;
    public Transform groundcheck;
    public float checkRadius;
    public LayerMask allGround;
    Animator anim;
    public AudioSource musicSource;
    public AudioClip musicClip;
    public AudioClip musicClip2;

void Start()
{
    rd2d = GetComponent<Rigidbody2D>();
    anim = GetComponent<Animator>();
    musicSource.clip = musicClip;
    musicSource.Play();
    score.text = "Score: " + scoreValue.ToString();
    lives.text = "Lives: " + livesValue.ToString();
    winTextObject.SetActive(false);
    loseTextObject.SetActive(false);
}

void FixedUpdate()
{
    float hozMovement = Input.GetAxis("Horizontal");
    float vertMovement = Input.GetAxis("Vertical");
    rd2d.AddForce(new Vector2(hozMovement * speed, vertMovement * speed));
    isOnGround = Physics2D.OverlapCircle(groundcheck.position, checkRadius, allGround);

    if (facingRight == false && hozMovement > 0)
    {
        Flip();
    }

    else if (facingRight == true && hozMovement < 0)
    {
        Flip();
    }

    if (hozMovement > 0)
    {
        anim.SetInteger("State", 1);
    }

    if (hozMovement > 0 && facingRight == false)
    {
        anim.SetInteger("State", 1);
    }

    if (hozMovement == 0 && isOnGround == true)
    {
        anim.SetInteger("State", 0);
    }

    else if (isOnGround == false)
    {
        anim.SetInteger("State", 2);
    }

    if(scoreValue == 8)
        {
            Destroy(GameObject.FindWithTag("Enemy"));
            winTextObject.SetActive(true);
        }

    if(livesValue == 0)
        {
            Destroy(GameObject.FindWithTag("Player"));
            loseTextObject.SetActive(true);
        }
}

private void OnCollisionEnter2D(Collision2D collision)
{
    if (collision.collider.tag == "Coin")
    {
        scoreValue += 1;
        score.text = "Score: " + scoreValue.ToString();
        Destroy(collision.collider.gameObject);
        if (scoreValue == 4)
        {
            transform.position = new Vector3(40.0f, -0.1242f, 0.0f);
            livesValue = 3;
            lives.text = "Lives: " + livesValue.ToString();
        }
        if (scoreValue == 8)
        {
            musicSource.clip = musicClip2;
            musicSource.Play();
        }
    }

    if (collision.collider.tag == "Enemy")
    {
        livesValue -= 1;
        lives.text = "Lives: " + livesValue.ToString();
        Destroy(collision.collider.gameObject);
    }
}

private void OnCollisionStay2D(Collision2D collision)
{
    if (collision.collider.tag == "Ground" && isOnGround)
    {
        if (Input.GetKey(KeyCode.W))
        {
            rd2d.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        }
    }
}

void Flip()
{
    facingRight = !facingRight;
    Vector2 Scaler = transform.localScale;
    Scaler.x = Scaler.x * -1;
    transform.localScale = Scaler;
}
}
