using Unity.VisualScripting;
using UnityEngine;

public class laserController : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] float speed = 10;

    void Start()
    {

        rb = GetComponent<Rigidbody2D>();

        GameObject player = GameObject.FindWithTag("Player");

        Vector2 direction = (player.transform.position - transform.position).normalized;

        rb.linearVelocity = direction * speed;

        //Rotation
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.gameObject.GetComponent<playerController>().takeDamage();
            Destroy(gameObject);
        }
        if (other.CompareTag("Stage"))
        {
            Destroy(gameObject);
        }

    }
}
