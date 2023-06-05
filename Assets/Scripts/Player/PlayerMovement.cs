using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;
    private IInput _input;
    private Transform myTransform;

    [SerializeField] private float maxSpeed;
    [SerializeField] private float maxAccel;

    [SerializeField] private float rotateVel;

    private bool isGrounded;
    [SerializeField] private float gravityScale;
    private float globalGravity = -9.81f;

    private float holdJump;
    [SerializeField] private float jumpForce;
    [SerializeField] private float holdJumpTime;

    [SerializeField] private float groundCheckRadius;
    [SerializeField] private LayerMask whatIsGround;

    [SerializeField] private float KnockbackForce;
    private Vector3 lastDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        _input = GetComponent<IInput>();
        TryGetComponent(out myTransform);
    }

    void FixedUpdate()
    {
        HorizontalMove();
        RotateToDirection();
        Vector3 gravity = globalGravity * gravityScale * Vector3.up;
        rb.AddForce(gravity, ForceMode.Acceleration);
    }

    private void Update()
    {
        Jump();
    }

    void HorizontalMove()
    {
        Vector3 goalVel = new Vector3(_input.direction.normalized.x, 0, _input.direction.normalized.y) * maxSpeed;
        Vector3 neededAccel = goalVel - rb.velocity;
        neededAccel -= Vector3.up * neededAccel.y;
        neededAccel = Vector3.ClampMagnitude(neededAccel, maxAccel);
        rb.AddForce(neededAccel, ForceMode.Impulse);
    }

    void RotateToDirection()
    {
        if (_input.direction.magnitude != 0)
        {
            myTransform.rotation = Quaternion.Slerp(myTransform.rotation, Quaternion.LookRotation(new Vector3(_input.direction.x, 0, _input.direction.y)), Time.deltaTime * rotateVel);
            lastDirection = new Vector3(_input.direction.x, 0, _input.direction.y);
        }
    }

    void Jump()
    {
        Collider[] grounds = Physics.OverlapSphere(transform.position, groundCheckRadius, whatIsGround);
        isGrounded = grounds.Length > 0;

        if (_input.jump)
        {
            if (isGrounded)
            {
                rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
                holdJump = holdJumpTime;
            }
            else
            {
                if (holdJump > 0)
                {
                    rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
                    holdJump -= Time.deltaTime;
                }
            }
        }
    }

    public void Knockback()
    {
        BeThrown(new Vector3(0, 1, 0), KnockbackForce);
    }

    public void BeThrown(Vector3 dir, float force)
    {
        rb.velocity = new Vector3(0, 0, 0);
        rb.AddForce(dir * force, ForceMode.Impulse);
    }
}
