using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class NewCameraController : MonoBehaviour
{
    public GameObject Player;
    private Rigidbody rb { get; set; }
    [Range(1, 10)]
    public float FollowDistance = 2;

    void Start()
    {
        rb = Player.GetComponent<Rigidbody>();
    }


    private void LateUpdate()
    {
        var velocity = rb.velocity;
        var direction = velocity;
        var distanceMult = 4f;//math.clamp(velocity.magnitude / 10f * FollowDistance, 3f, 10f);
        transform.position = Player.transform.position - direction.normalized * distanceMult;
        Vector3 targetDirection = (Player.transform.position - transform.position).normalized;
        var targetRotation = Quaternion.LookRotation(targetDirection);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime * 180);
        
    }
}