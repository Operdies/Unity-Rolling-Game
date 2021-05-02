using UnityEngine;

public class FixedRotation : MonoBehaviour
{
    public Quaternion TransformRotation { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        TransformRotation = transform.rotation;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.rotation = TransformRotation;
    }
}
