using System;
using DefaultNamespace;
using TMPro;
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
    
    void Start()
    {
        WinTextObj.SetActive(false);
        rb = GetComponent<Rigidbody>();

        Objective.OnWin += OnWin;
        Objective.OnCollected += () =>
        {
            Speed *= 1.15f;
            UpdateCountText();
        };
        
        UpdateCountText();
    }

    private void OnWin()
    {
        WinTextObj.SetActive(true);
        Speed *= 2f;
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
