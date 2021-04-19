using Unity.Mathematics;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    [Range(-180, 180)] public float x = 0f;
    [Range(-180, 180)] public float y = 0f;
    [Range(-180, 180)] public float z = 0f;

    void Start()
    {
        // Avoid calling update if the rotator isn't active
        if (math.abs(x + y + z) < 0.01f)
        {
            enabled = false;
        }
    }
    void Update()
    {
        transform.Rotate(new Vector3(x, y, z) * Time.deltaTime );
    }
}
