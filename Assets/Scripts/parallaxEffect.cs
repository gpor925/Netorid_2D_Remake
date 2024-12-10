using UnityEngine;

public class parallaxEffect : MonoBehaviour
{

    [SerializeField] float effect;
    GameObject mainCamera;
    Vector3 lastCamPosition;

    void Start()
    {
        mainCamera = Camera.main.gameObject;
        lastCamPosition = mainCamera.transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 cameraMovement = mainCamera.transform.position - lastCamPosition;

        transform.position += new Vector3(
            cameraMovement.x * effect,  //X
            cameraMovement.y * effect,  //Y
            0                           //Z
        );

        lastCamPosition = mainCamera.transform.position;
    }
}
