using UnityEngine;

public class dashTrail : MonoBehaviour
{

    public float trailTime;
    public float startTrailTime;

    public GameObject trail;
    private playerController player;

    void Start()
    {
        player = GetComponent<playerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.isDashing)
        {
            if (trailTime <= 0)
            {
                GameObject instance = (GameObject)Instantiate(trail,
                                        new Vector3(transform.position.x,
                                        transform.position.y + 1.5f,
                                        0),
                            Quaternion.identity);
                if (!playerController.lookingRight)
                {
                    instance.transform.localScale = new Vector3(-1, 1, 1);
                }
                Destroy(instance, 1f);
                trailTime = startTrailTime;
            }
            else
            {
                trailTime -= Time.deltaTime;
            }
        }

    }
}
