using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Sucker : MonoBehaviour
{
    [Tooltip("Increasing the mass increases the attraction force")]
    [Range(10, 10000)]
    public float Mass;
    
    GameObject Player { get; set; }
    Rigidbody rb { get; set; }
    private float snapped { get; set; }
    public Volume PostProcessingVolume { get; set; }
    public float ColliderRadius { get; set; }
    public float PlayerColliderRadius { get; set; }

    void Start()
    {
        Player = GameObject.Find("Player");
        rb = Player.GetComponent<Rigidbody>();
        PostProcessingVolume = GameObject.Find("PostProcessing").GetComponent<Volume>();
        ColliderRadius = GetComponent<SphereCollider>().radius;
        PlayerColliderRadius = Player.GetComponent<SphereCollider>().radius;
    }

    IEnumerator MovePlayerIntoHole(Vector3 holePosition)
    {
        Vector3 start = rb.position;
        var distance = Vector3.Distance(holePosition, rb.position);
        var initialDistance = distance;
 
        while (distance > 0.1f)
        {
            distance = Vector3.Distance(holePosition, rb.position);
            rb.MovePosition (Vector3.Lerp (start, holePosition, initialDistance / 40));
            start = rb.position;
 
            yield return null;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (PostProcessingVolume.profile.TryGet<MotionBlur>(out var mb))
            {
                var holePosition = transform.position;
                var distance = Vector3.Distance(other.ClosestPoint(holePosition), holePosition);
                float intensity = math.max(0.3f, 1.0f - math.log2(distance + 0.5f));
                mb.intensity.value = intensity;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (PostProcessingVolume.profile.TryGet<MotionBlur>(out var mb))
            {
                mb.intensity.value = 0.3f;
            }
        }
    }
    
    private float GetDistance(Vector3 playerPosition) => Vector3.Distance(transform.position, playerPosition);

    private float GetColliderDistance(Vector3 playerPosition) =>
        GetDistance(playerPosition) - PlayerColliderRadius;

    private void Update()
    {
        snapped += Time.deltaTime;
        var G = 200f * Time.deltaTime;
        var holePosition = transform.position;
        var playerPosition = Player.transform.position;

        var r = GetDistance(playerPosition);

        float playerWeight = 1;

        // F = (GMm)/(^2
        var F = (G * playerWeight * Mass) / (r*r);
        Vector3 targetDirection = (holePosition - playerPosition).normalized;
        // if (targetDirection.y < 0)
        //     targetDirection.y = -targetDirection.y * 0.1f;
        rb.AddForce(F * targetDirection);
    }
}
