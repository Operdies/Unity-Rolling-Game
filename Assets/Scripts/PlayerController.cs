using System;
using System.Collections;
using DefaultNamespace;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


public class PlayerController : MonoBehaviour
{
    public PauseMenu pauseScreen;
    
    private Rigidbody rb;
    private float movementX;
    private float movementY;
    public float Speed = 0;
    public float JumpHeight = 200;
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
        Objective.OnRegister += UpdateCountText;
        Objective.OnCollected += OnCollected;

        UpdateCountText();
    }

    private void OnCollected()
    {
        power += 1;
        if (this.ParticleSystem.isPlaying)
        {
            StartCoroutine(nameof(EaseScale));
        }
        else
            this.ParticleSystem.Play();

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
    

    void FixedUpdate()
    {
        var movement = new Vector3(movementX, 0, movementY) * Speed;
        movement.y = this.didJump ? JumpHeight : 0.0f;
        
        rb.AddForce(movement);
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

    void OnPause(InputValue v)
    {
        pauseScreen.OnPause();
    }

    private void OnCollisionStay(Collision other)
    {
        ContactPoint[] contacts = new ContactPoint[10];
        int nContacts = other.GetContacts(contacts);

        var playerBottom = transform.position.y - (transform.lossyScale.y / 2);

        for (int i = 0; i < nContacts; i++)
        {
            var contact = contacts[i];
            if (Mathf.Approximately(contact.point.y, playerBottom))
            {
                IsGrounded = true;
                break;
            }
        }
    }

    private void OnCollisionExit(Collision other)
    {
        IsGrounded = false;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Deadly"))
        {
            // Expend a power (gained from donuts) to kill the enemy
            if (power > 0)
            {
                other.gameObject.SetActive(false);
                power--;
            }
            // Game over
            else
            { 
                SceneManager.LoadScene("Stage2");
                Objective.Reset();
            }
        }
    }

    private bool IsGrounded { get; set; }
    public int power = 3;

    void UpdateCountText()
    {
        countText.text = $"{Objective.Collected} / {Objective.TotalPickups}";
    }
}
