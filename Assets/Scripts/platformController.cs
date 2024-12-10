using UnityEngine;
using UnityEngine.SceneManagement;

public class platformController : MonoBehaviour
{

    [SerializeField] int speed = 4;
    [SerializeField] Vector3 endPosition;
    Vector3 startPosition;
    bool goingToEnd = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (goingToEnd) {
            transform.position = Vector3.MoveTowards(transform.position, endPosition, speed * Time.deltaTime);
            if (transform.position == endPosition) {
                goingToEnd = false;
            }
        } else {
            transform.position = Vector3.MoveTowards(transform.position, startPosition, speed * Time.deltaTime);
            if (transform.position == startPosition) {
                goingToEnd = true;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D other){
        if (other.gameObject.tag == "Player") {
            other.gameObject.transform.SetParent(transform);
        }
    }

    void OnCollisionExit2D(Collision2D other){
        if (other.gameObject.tag == "Player") {
            other.gameObject.transform.SetParent(null);
        }
    }
}
