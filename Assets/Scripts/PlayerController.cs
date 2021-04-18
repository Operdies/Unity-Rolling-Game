using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private float movementX;
    private float movementY;
    public float Speed = 0;
    public TextMeshProUGUI countText;
    public GameObject WinTextObj;

    private int pickupCount;
    private int PickupCount
    {
        get => pickupCount;
        set
        {
            pickupCount = value;
            countText.text = $"{PickupCount} / 13";
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        WinTextObj.SetActive(false);
        PickupCount = 0;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
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
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pickup"))
        {
            other.gameObject.SetActive(false);
            PickupCount++;
            
            if (PickupCount >= 13)
                WinTextObj.SetActive(true);
        }
    }
}
