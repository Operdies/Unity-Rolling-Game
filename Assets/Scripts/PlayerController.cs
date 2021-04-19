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
    public TextMeshProUGUI countText;
    public GameObject WinTextObj;
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
            Speed *= 1.15f;
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
        var factor = 2f;
        var iterations = 10;
        var each = math.pow(factor, 1.0f / iterations);
        var easeDuration = 1.0f; // seconds
        var gravityIncrease = 2f;
        var gravityEach = math.pow(gravityIncrease, 1.0f / iterations);

        for (int i = 0; i < iterations; i++)
        {
            var p = this.ParticleSystem.main;
            
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
        this.ParticleSystem.transform.rotation = StartRotation;
    }

    void FixedUpdate()
    {
        var movement = new Vector3(movementX, 0.0f, movementY);
        rb.AddForce(movement * Speed);
    }

    void OnMove(InputValue movementValue)
    {
        var movement = movementValue.Get<Vector2>();
        movementX = movement.x;
        movementY = movement.y;
    }

    void UpdateCountText()
    {
        countText.text = $"{Objective.Collected} / {Objective.TotalPickups}";
    }
}
