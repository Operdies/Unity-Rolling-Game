using System;
using System.Collections;
using DefaultNamespace;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private float movementX;
    private float movementY;
    public float Speed = 0;
    public float JumpHeight = 0;
    public TextMeshProUGUI countText;
    public GameObject WinTextObj;
    private bool didJump;
    public ParticleSystem ParticleSystem { get; set; }
    public Quaternion StartRotation { get; set; }

    void Start()
    {
        WinTextObj.SetActive(false);
        rb = GetComponent<Rigidbody>();
        ParticleSystem = GetComponentInChildren<ParticleSystem>();
        StartRotation = ParticleSystem.transform.rotation;

        Objective.OnWin += OnWin;
        Objective.OnCollected += () =>
        {
            if (this.ParticleSystem.isPlaying)
            {
                StartCoroutine(nameof(EaseScale));
            }
            else
                this.ParticleSystem.Play();
            UpdateCountText();
        };
        
        UpdateCountText();
    }

    IEnumerator EaseScale()
    {
        var factor = 1.15f;
        var iterations = 10;
        var each = math.pow(factor, 1.0f / iterations);
        var easeDuration = 1.0f; // seconds
        var gravityIncrease = 2f;
        var gravityEach = math.pow(gravityIncrease, 1.0f / iterations);

        for (int i = 0; i < iterations; i++)
        {
            var p = this.ParticleSystem.main;

            Speed *= each;
            JumpHeight *= each;
            this.ParticleSystem.transform.localScale *= each;
            p.gravityModifierMultiplier *= gravityEach;
            p.simulationSpeed *= each;
            yield return new WaitForSeconds(easeDuration / iterations);
        }
    }
    
    private void OnWin()
    {
        WinTextObj.SetActive(true);
        Speed *= 2f;
    }

    private void LateUpdate()
    {
    }

    void FixedUpdate()
    {
        var movement = new Vector3(movementX, this.didJump ? 1.0f * JumpHeight : 0.0f, movementY);
        rb.AddForce(movement * Speed);
        this.didJump = false;
    }

    void OnMove(InputValue movementValue)
    {
        var movement = movementValue.Get<Vector2>();
        movementX = movement.x;
        movementY = movement.y;
    }

    void OnJump(InputValue v)
    {
        this.didJump = this.IsGrounded;
    }

    private void OnCollisionStay(Collision other)
    {
        if (other.collider.CompareTag("Ground"))
        {
            this.IsGrounded = true;
        }
    }

    private bool IsGrounded { get; set; }

    void UpdateCountText()
    {
        countText.text = $"{Objective.Collected} / {Objective.TotalPickups}";
    }
}
