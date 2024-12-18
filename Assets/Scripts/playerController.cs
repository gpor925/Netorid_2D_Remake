using System.Collections;
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

    [SerializeField] int lives = 3;
    [SerializeField] int items = 0;
    [SerializeField] float time = 180;

    public float coyoteTime = 0.2f;
    private float coyoteTimeCounter;

    public float jumpBufferTime = 0.2f;
    private float jumpBufferCounter;

    public float shotCooldown = 1f;
    private float shotCooldownCounter;

    private bool canDash = true;
    public bool isDashing;
    private float dashingPower = 24f;
    private float dashingTime = 0.3f;
    private float dashingCooldown = 0.5f;

    public static bool lookingRight = true;
    public static bool lookingUp = false;
    private bool wasGrounded;
    private bool hasJumped;

    [SerializeField] bool isInWater;

    [SerializeField]
    TMP_Text txtLives,
                              txtItems,
                              txtTime;

    bool endGame = false;

    AudioSource audioSrc;

    [SerializeField]
    AudioClip sndJump,
                               sndItem,
                               sndShoot,
                               sndDamage,
                               sndDash,
                               sndHealth;

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
            if (isDashing)
            {
                return;
            }

            float inputX = Input.GetAxis("Horizontal");
            rb.linearVelocity = new Vector2(inputX * speed, rb.linearVelocity.y);

            if (inputX > 0)
            {               //Derecha
                sprite.flipX = false;
                lookingRight = true;
            }
            else if (inputX < 0)
            {                //Izquierda
                sprite.flipX = true;
                lookingRight = false;
            }

            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                lookingUp = true;
            }
            else
            {
                lookingUp = false;
            }

            //Coyote time

            if (isGrounded())
            {
                coyoteTimeCounter = coyoteTime;
            }
            else
            {
                coyoteTimeCounter -= Time.deltaTime;
            }

            //Jump buffer

            if (Input.GetKeyDown(KeyCode.Space))
            {
                jumpBufferCounter = jumpBufferTime;
            }
            else
            {
                jumpBufferCounter -= Time.deltaTime;
            }

            if (jumpBufferCounter < -0.5f)
                jumpBufferCounter = -0.1f;

            //Landing detection
            if (isGrounded() && !wasGrounded)
            {
                OnLand();
            }
            void OnLand()
            {
                hasJumped = false;
            }

            wasGrounded = isGrounded();

            //Salto

            if (jumpBufferCounter > 0f && coyoteTimeCounter > 0f)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocityX, jump);
                jumpBufferCounter = 0f;
                audioSrc.PlayOneShot(sndJump);
                hasJumped = true;
            }
            if (Input.GetKeyUp(KeyCode.Space) && rb.linearVelocity.y > 0f)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocityX, rb.linearVelocityY * 0.5f);
                coyoteTimeCounter = 0f;
            }

            if (isInWater)
            {
                rb.gravityScale = 0.5f;
            }
            else
            {
                rb.gravityScale = 1.5f;
            }

            //Dash

            if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
            {
                StartCoroutine(Dash());
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

            if (!isGrounded() && hasJumped == true)
            {
                anim.SetBool("isJumping", true);
            }
            else
            {
                anim.SetBool("isJumping", false);
            }

            //DISPARO

            shotCooldownCounter -= Time.deltaTime;

            if (shotCooldownCounter < -1f)
                shotCooldownCounter = -0.5f;

            if (Input.GetMouseButtonDown(0))
            {
                shoot();
            }

            void shoot()
            {
                if (shotCooldownCounter > 0)
                {
                    return;
                }

                shotCooldownCounter = shotCooldown;

                Instantiate(shot,
                            new Vector3(transform.position.x,
                                        transform.position.y + 1.7f,
                                        0),
                            Quaternion.identity);
                audioSrc.PlayOneShot(sndShoot);
                anim.SetBool("isShooting", true);
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
        }
        else
        {
            sprite.gameObject.SetActive(false);
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        GameManager.invulnerable = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        audioSrc.PlayOneShot(sndDash);
        if (lookingRight)
        {
            rb.linearVelocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        }
        else
        {
            rb.linearVelocity = new Vector2(transform.localScale.x * -1 * dashingPower, 0f);
        }
        yield return new WaitForSeconds(dashingTime);
        rb.gravityScale = originalGravity;
        isDashing = false;
        GameManager.invulnerable = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;

    }

    bool isGrounded()
    {
        RaycastHit2D touch = Physics2D.Raycast(transform.position, Vector2.down, 0.2f);

        if (touch.collider == null)
        {
            return false;
        }
        else
        {
            return true;
        }

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "PowerUp")
        {
            lives++;
            txtLives.text = "Lives: " + lives;
            audioSrc.PlayOneShot(sndHealth);
            Destroy(other.gameObject);
        }

        if (other.gameObject.tag == "Item")
        {
            Destroy(other.gameObject);
            items++;
            txtItems.text = "Items: " + items;
            audioSrc.PlayOneShot(sndItem);
            if (SceneManager.GetActiveScene().name == "Level1")
            {
                if (items == 10)
                {
                    txtWin.SetActive(true);
                    endGame = true;
                    Invoke("goToLevel2", 3);
                }
            }
            else if (SceneManager.GetActiveScene().name == "Level2")
            {
                if (items == 1)
                {
                    txtWin.SetActive(true);
                    endGame = true;
                    Invoke("goToCredits", 3);
                }
            }
        }

        if (other.gameObject.tag == "Water")
        {
            isInWater = true;
        }

        if (other.gameObject.tag == "FinalBoss")
        {
            takeDamage();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Water")
        {
            isInWater = false;
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
        Invoke("becomeVulnerable", 1.5f);
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

    void goToLevel2()
    {
        SceneManager.LoadScene("Level2");
    }

    void goToCredits()
    {
        SceneManager.LoadScene("Credits");
    }
}
