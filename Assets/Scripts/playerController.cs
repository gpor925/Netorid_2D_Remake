using System;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class playerController : MonoBehaviour
{

    Rigidbody2D rb;
    public int speed = 4;
    public int jump = 5;
    
    [SerializeField] SpriteRenderer sprite;
    [SerializeField] Animator anim;
    [SerializeField] GameObject shot;
    [SerializeField]
    GameObject txtWin,
                                txtLose;

    [SerializeField] bool hasJumped;

    [SerializeField] int lives = 3;
    [SerializeField] int items = 0;
    [SerializeField] float time = 180;
    [SerializeField] float canJump = 0.5f;

    public static bool right = true;

    [SerializeField]
    TMP_Text txtLives,
                              txtItems,
                              txtTime;

    bool endGame = false;

    AudioSource audioSrc;

    [SerializeField] AudioClip sndJump,
                               sndItem,
                               sndShoot,
                               sndDamage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        GameManager.invulnerable = false;
        txtLives.text = "Lives: " + lives;

        txtItems.text = "Items: " + items;

        txtTime.text = time.ToString();

        txtLose.SetActive(false);
        txtWin.SetActive(false);

        audioSrc = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!endGame)
        {
            float inputX = Input.GetAxis("Horizontal");
            rb.linearVelocity = new Vector2(inputX * speed, rb.linearVelocity.y);

            if (inputX > 0)
            {               //Derecha
                sprite.flipX = false;
                right = true;
            }
            else if (inputX < 0)
            {                //Izquierda
                sprite.flipX = true;
                right = false;
            }

            if (Input.GetKeyDown(KeyCode.Space) && canJump > 0)
            {
                rb.transform.position = new Vector2(rb.position.x, rb.position.y + 0.3f);
                rb.AddForce(Vector2.up * jump, ForceMode2D.Impulse);
                hasJumped = true;
                canJump = 0;
                audioSrc.PlayOneShot(sndJump);
            }

            if (canJump > 0.0f)
            {
                hasJumped = false;
            }

            //Animaciones
            if (Input.GetKey(KeyCode.A) ||
                Input.GetKey(KeyCode.D) ||
                Input.GetKey(KeyCode.LeftArrow) ||
                Input.GetKey(KeyCode.RightArrow))
            {
                anim.SetBool("isRunning", true);
            }
            else
            {
                anim.SetBool("isRunning", false);
            }

            if (hasJumped)
            {
                anim.SetBool("isJumping", true);
            }
            else
            {
                anim.SetBool("isJumping", false);
                hasJumped = false;
            }

            //DISPARO
            if (Input.GetMouseButtonDown(0))
            {
                Instantiate(shot,
                            new Vector3(transform.position.x,
                                        transform.position.y + 1.7f,
                                        0),
                            Quaternion.identity);
                anim.SetBool("isShooting", true);
                audioSrc.PlayOneShot(sndShoot);
            }

            time = time - Time.deltaTime;
            if (time < 0)
            {
                time = 0;
                txtLose.SetActive(true);
                endGame = true;
                Invoke("goToMenu", 3);
            }

            float min, sec;
            min = Mathf.Floor(time / 60);
            sec = Mathf.Floor(time % 60);

            txtTime.text = min.ToString("00") + ":" + sec.ToString("00");
        } else
        {
            sprite.gameObject.SetActive(false);
        }
    }


    void grounded()
    {
        RaycastHit2D touch = Physics2D.Raycast(transform.position, Vector2.down, 0.1f);

        if (touch.collider == null)
        {   
            canJump -= 0.1f * Time.deltaTime;
        } else if (touch.collider)
        {
            canJump = 0.5f;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "PowerUp")
        {
            Destroy(other.gameObject);
            sprite.color = Color.yellow;
            GameManager.invulnerable = true;
            Invoke("becomeVulnerable", 5);
        }

        if (other.gameObject.tag == "Item")
        {
            Destroy(other.gameObject);
            items++;
            txtItems.text = "Items: " + items;
            audioSrc.PlayOneShot(sndItem);
            if (items == 4)
            {
                txtWin.SetActive(true);
                endGame = true;
                Invoke("goToCredits", 3);
            }
        }
    }

    void becomeVulnerable()
    {
        sprite.color = Color.white;
        GameManager.invulnerable = false;
    }

    public void takeDamage()
    {
        lives--;
        txtLives.text = "Lives: " + lives;
        sprite.color = Color.red;
        GameManager.invulnerable = true;
        Invoke("becomeVulnerable", 1);
        audioSrc.PlayOneShot(sndDamage);
        if (lives == 0)
        {
            txtLose.SetActive(true);
            endGame = true;
            Invoke("goToMenu", 3);
        }
    }

    void goToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    void goToCredits()
    {
        SceneManager.LoadScene("Credits");
    }
}
