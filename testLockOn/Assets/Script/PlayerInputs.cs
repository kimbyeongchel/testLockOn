using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputs : MonoBehaviour
{
    public static PlayerInputs instance;

    public Vector3 moveDir;
    public Vector3 cameraDir;
    public bool sprint;
    public bool jump;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void OnMove(InputValue value)
    {
        MoveInput(value.Get<Vector2>());
    }

    void MoveInput(Vector2 newInput)
    {
        moveDir = new Vector3(newInput.x, 0, newInput.y);
    }

    void OnLook(InputValue value)
    {
        cameraDir = value.Get<Vector2>();
    }

    void OnSprint(InputValue value)
    {
        sprint = value.isPressed;
    }

    void OnJump(InputValue value)
    {
        jump = value.isPressed;
    }
}
