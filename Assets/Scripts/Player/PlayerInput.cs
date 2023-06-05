using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class PlayerInput : MonoBehaviour, IInput
{
    [SerializeField] private GameObjectVariable player;
    [SerializeField] private GameObjectVariable camera;
    private bool canControl = true;

    private void Start()
    {
        player.Value = gameObject;
    }

    public Vector2 direction
    {
        get
        {
            Vector2 gamepadMove = Vector2.zero;
            if (Gamepad.current != null)
            {
                StickControl stick = Gamepad.current.leftStick;
                gamepadMove = new Vector2(stick.right.value - stick.left.value, stick.up.value - stick.down.value);
                if (gamepadMove.magnitude < 0.9f) 
                {
                    gamepadMove = Vector2.zero;
                }
            }
            Vector2 keyboardMove = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            Vector2 move = keyboardMove + gamepadMove;

            float headAngle = Mathf.Deg2Rad * (360 - camera.Value.transform.rotation.eulerAngles.y);

            Vector2 a = new Vector2(Mathf.Cos(headAngle), Mathf.Sin(headAngle));
            Vector2 b = new Vector2(-Mathf.Sin(headAngle), Mathf.Cos(headAngle));

            Vector2 rotatedMove = move.x * a + move.y * b;

            if (canControl)
            {
                return rotatedMove;
            }
            else
            {
                return Vector2.zero;
            }
        }
    }

    public Vector2 look
    {
        get
        {
            if (!canControl)
            {
                return Vector2.zero;
            }
            Vector2 gamepadLook = Vector2.zero;
            if (Gamepad.current != null)
            {
                StickControl stick = Gamepad.current.rightStick;
                gamepadLook = new Vector2(stick.up.value - stick.down.value, stick.right.value - stick.left.value);
            }
            Vector2 mouseLook = new Vector2(Mouse.current.delta.value.y, Mouse.current.delta.value.x);
            return mouseLook + gamepadLook;
        }
    }

    public bool jump
    {
        get
        {
            if (canControl)
            {
                bool gamepadJump = false;
                if (Gamepad.current != null)
                {
                    gamepadJump = Gamepad.current.buttonSouth.isPressed;
                }
                return Input.GetKey(KeyCode.Space) || gamepadJump;
            }
            else
            {
                return false;
            }
        }
    }

    public bool dash
    {
        get
        {
            if (canControl)
            {
                bool gamepadDash = false;
                if (Gamepad.current != null)
                {
                    gamepadDash = Gamepad.current.rightTrigger.wasPressedThisFrame || Gamepad.current.buttonEast.wasPressedThisFrame;
                }
                return Input.GetKeyDown(KeyCode.LeftShift) || gamepadDash;
            }
            else
            {
                return false;
            }
        }
    }

    public void SetCanControl(bool state)
    {
        canControl = state;
    }
}
