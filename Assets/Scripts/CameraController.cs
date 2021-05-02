using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Vector3 PlayerOffset;
    public GameObject Player;

    // Start is called before the first frame update
    void Start()
    {
        // Camera = GetComponent<Camera>();
        PlayerOffset = transform.position - Player.transform.position;
    }
    
    private void LateUpdate()
    {
        transform.position = Player.transform.position + PlayerOffset;
    }
}
