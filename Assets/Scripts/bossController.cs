using UnityEngine;
using UnityEngine.UI;

public class bossController : MonoBehaviour
{
    public float hitPoints = 5;
    public float maxHP = 100;
    public Image healthBar;

    [SerializeField] float speed = 10f;
    [SerializeField] GameObject laser;
    [SerializeField] float originalFireRate;
    [SerializeField] float fireRate = 10f;
    [SerializeField] float moveChangeInterval = 2f;
    [SerializeField] float time = 0f;

    [SerializeField] GameObject finalItem;
    [SerializeField] GameObject audioPlayer;

    [SerializeField] Vector2 targetPosition;
    [SerializeField] float cooldown;


    AudioSource audioSrc;
    [SerializeField]
    AudioClip sndDeath;

    void Start()
    {
        setRandomTargetPosition();
        InvokeRepeating("SetRandomTargetPosition", moveChangeInterval, moveChangeInterval);
        originalFireRate = fireRate;
        audioSrc = GetComponent<AudioSource>();
    }


    void Update()
    {
        healthBar.fillAmount = Mathf.Clamp(hitPoints / maxHP, 0, 1);

        time += Time.deltaTime;

        // fireRate = Mathf.Min(originalFireRate + (time * 0.5f),100f);

        if (hitPoints < 50)
        {
            fireRate = 0.75f;
        }
        if (hitPoints < 25)
        {
            fireRate = 0.5f;
        }

        moveTowardsTarget();
        shoot();
        if (hitPoints <= 0)
        {
            death();
        }
    }

    void setRandomTargetPosition()
    {
        float randomX = Random.Range(32f, 72f);
        float randomY = Random.Range(36f, 20f);
        targetPosition = new Vector2(randomX, randomY);
    }

    void moveTowardsTarget()
    {

        transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
        {
            setRandomTargetPosition();
        }
    }


    void shoot()
    {
        if (Time.time >= cooldown)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                Vector2 direction = (player.transform.position - transform.position).normalized;
                shootProjectile(direction);
            }
            cooldown = Time.time + fireRate; 
        }
    }

    void shootProjectile(Vector2 direction)
    {

        GameObject shot = Instantiate(laser,
                                      new Vector3(transform.position.x,
                                                  transform.position.y,
                                                0),
                                      Quaternion.identity);
        Rigidbody2D rb = shot.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = direction * 10f;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "shot")
        {
            hitPoints -= 1;
            Destroy(other.gameObject);
        }
    }

    

    void death()
    {
        Instantiate(finalItem, new Vector3(50f, 24f, 0f), Quaternion.identity);
        AudioSource audioSource = audioPlayer.GetComponent<AudioSource>();
        audioSource.Stop();
        audioSrc.PlayOneShot(sndDeath);
        gameObject.SetActive(false);
    }
}
