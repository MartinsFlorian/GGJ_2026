using UnityEngine;
using System;

[Serializable]
public class S_ClassPlayer
{
    [Header("Movement")]
    public float moveSpeed = 6f;
    public float rotationSpeed = 12f;

    [Header("Look")]
    public float lookSensitivity = 1.5f;
    public float minPitch = -40f;
    public float maxPitch = 70f;

    [Header("Jump")]
    public float jumpForce = 6f;
    public float groundCheckDistance = 0.35f;
    public LayerMask groundLayer;

    [Header("Coyote Time")]
    public float coyoteTime = 0.15f;
    public float jumpBufferTime = 0.15f;

    [Header("Dash")]
    public float dashForce = 12f;
    public float dashCooldown = 1f;

    [Header("FOV Dash")]
    public float normalFOV = 60f;
    public float dashFOV = 75f;
    public float fovTransitionSpeed = 10f;
    public float fovHoldTime = 0.2f;

    [Header("Headbob")]
    public float bobAmplitude = 0.04f;
    public float bobFrequency = 6f;
    public float bobSmooth = 8f;
}