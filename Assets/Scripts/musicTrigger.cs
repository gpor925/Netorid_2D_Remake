using UnityEngine;

public class musicTrigger : MonoBehaviour
{
    [SerializeField] private GameObject audioPlayer;
    [SerializeField] private GameObject bossBar;

    void Start()
    {
        bossBar.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            AudioSource audioSource = audioPlayer.GetComponent<AudioSource>();
            audioSource.Play();
            bossBar.SetActive(true);
            Destroy(gameObject);
        }
    }
}
