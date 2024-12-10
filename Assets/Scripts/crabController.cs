using UnityEngine;
using UnityEngine.SceneManagement;

public class crabController : MonoBehaviour
{

    [SerializeField] int speed = 3;
    [SerializeField] Vector3 endPosition;
    [SerializeField] SpriteRenderer sprite;

    Vector3 startPosition;
    bool goingToEnd = true;
    float previousXPos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPosition = transform.position;
        previousXPos = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (goingToEnd)
        {
            transform.position = Vector3.MoveTowards(transform.position, endPosition, speed * Time.deltaTime);
            if (transform.position == endPosition)
            {
                goingToEnd = false;
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, startPosition, speed * Time.deltaTime);
            if (transform.position == startPosition)
            {
                goingToEnd = true;
            }
        }

        if (transform.position.x > previousXPos)
        {
            sprite.flipX = true;
        }
        else if (transform.position.x < previousXPos)
        {
            sprite.flipX = false;
        }

        previousXPos = transform.position.x;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && !GameManager.invulnerable)
        {
            other.gameObject.GetComponent<playerController>().takeDamage();
        }
    }
}
