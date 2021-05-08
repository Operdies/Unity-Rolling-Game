using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using DefaultNamespace;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
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

        PlayerJumped += () =>
        {
            rb.AddForce(JumpHeight * Vector3.up);
        };

        UpdateCountText();
    }

    private void OnCollected()
    {
        power += 1;
        if (this.ParticleSystem.isPlaying)
        {
            PowerUp(1.08f);
        }
        else
            this.ParticleSystem.Play();

        UpdateCountText();
    }

    public void PowerUp(float factor)
    {
        StartCoroutine(nameof(EaseScale), factor);
    }

    IEnumerator EaseScale(float factor)
    {
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
        if (transform.position.y < -50f)
        {
            rb.velocity *= 0;
            rb.MovePosition(new Vector3(2, -2, 2) * -1);
        }

        var movement = new Vector3(movementX, 0, movementY) * Speed;
        var torque = new Vector3(movementY, 0, -movementX) * Speed;
        
        if (this.didJump)
        {
            PlayerJumped?.Invoke();
            this.didJump = false;
        }

        rb.AddForce(movement);
        rb.AddTorque(torque);
    }

    private Action PlayerJumped;

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

        for (int i = 0; i < nContacts; i++)
        {
            var contact = contacts[i];
            // if (math.distance((float) contact.point.y, playerBottom) < 0.01f)
            {
                if (other.transform.gameObject.CompareTag("TerrainWithGrass"))
                    GrassIncident(other.transform.gameObject);
                IsGrounded = true;
                break;
            }
        }
    }

    private void GrassIncident(GameObject go)
    {
        var playerPos = transform.position;
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

    private void Launch()
    {
        rb.velocity *= 0;
        rb.AddForce(LaunchHeight * LaunchDirection);
        PlayerJumped -= Launch;
        IsGrounded = false;
        
    }


    [Range(100, 8000)]
    public float LaunchHeight;

    private bool LaunchStored =>
        PlayerJumped?.GetInvocationList().Any(i => i.GetMethodInfo().Name == nameof(Launch)) == true;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("BlackHole"))
        {
            LaunchDirection = (transform.position - other.gameObject.transform.position).normalized;
            IsGrounded = true;
            if (LaunchStored == false)
                PlayerJumped += Launch;
        }
    }

    private Vector3 LaunchDirection { get; set; }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("BlackHole"))
        {
            IsGrounded = false;
            if (LaunchStored)
                PlayerJumped -= Launch;
        }
    }

    private bool IsGrounded { get; set; }
    public int power = 3;

    void UpdateCountText()
    {
        countText.text = $"{Objective.Collected} / {Objective.TotalPickups}";
    }
}
