using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * The main player class to control the state of the player and other behaviours. It searches all 
 * Player Component Interfaces and controls it by updating it in a specific order.
 */

[RequireComponent(typeof(InputComponent))]
[RequireComponent(typeof(MovementComponent))]
[RequireComponent(typeof(Inventory))]
[RequireComponent(typeof(WhistleComponent))]
[RequireComponent(typeof(TeleportComponent))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(DetectionComponent))]

public class MainPlayer : MonoBehaviour
{
    public List<GameObject> lightStandingIn;

    public enum PlayerIndex
    {
        P1, P2
    }

    [Header("Player settings")]
    public PlayerIndex playerIndex;

    [Header("General settings")]
    [Tooltip("The speed of changing the alpha value.")]
    public float alphaSpeed = 1f;    // How fast alpha value decreases.
    private Color color;            // Used to store color reference.
    private Material material;

    private Inventory inventory;

    // Used for channelling
    private float channelTimeRatio = 0;
    public float ChannelTimeRatio { get { return channelTimeRatio; } set { channelTimeRatio = value; } }
    [HideInInspector]
    public Vector3 teleportTarget;

    public enum State
    {
        Idle,
        Busy,
        Teleported,
        Channelling,
        Dead
    }

    /*
     * 0 - X
     * 1 - A
     * 2 - B 
     * 3 - Y
     * 4 - L1
     * 5 - R1
     * 6 - L2
     * 7 - R2
     * 8 - Select
     * 9 - Start
     */
     
    [HideInInspector]
    public bool[] buttonDownList = new bool[10];
    [HideInInspector]
    public bool[] buttonList = new bool[10];
    [HideInInspector]
    public Vector3 axisDirection = Vector3.zero;
    [HideInInspector]
    public PlayerComponentInterface[] components;
    [HideInInspector]
    public Rigidbody rigidBody;
    
    private State currentState = State.Idle;
    public State CurrentState { get { return currentState; } set { currentState = value; } }

    private void Awake()
    {
        // Cache stuff for later
        components = gameObject.GetComponents<PlayerComponentInterface>();
        inventory = gameObject.GetComponent<Inventory>();
        rigidBody = gameObject.GetComponent<Rigidbody>();
        material = gameObject.GetComponent<MeshRenderer>().material;

        // Save current color
        color = material.color;

        // Initialize
        foreach (PlayerComponentInterface component in components)
        {
            component.MainPlayer = this;
            component.AwakeComponent();
        }

        // Sort array on ID
        Array.Sort(components, delegate (PlayerComponentInterface a, PlayerComponentInterface b)
        {
            return (a.Id).CompareTo(b.Id);
        });
    }

    private void Update()
    {
        // Update all components
        foreach (PlayerComponentInterface component in components)
        {
            component.UpdateComponent();
        }
    }

    private void FixedUpdate()
    {
        // Fixed update all components
        foreach (PlayerComponentInterface component in components)
        {
            component.FixedUpdateComponent();
        }
    }

    private void LateUpdate()
    {
        // Fixed update all components
        foreach (PlayerComponentInterface component in components)
        {
            component.LateUpdateComponent();
        }
    }

    // Check current states
    public bool IsBusy()
    {
        return CurrentState == State.Teleported || CurrentState == State.Dead;
    }

    public bool IsFree()
    {
        return CurrentState == State.Idle;
    }

    public bool IsChannelling()
    {
        return CurrentState == State.Channelling;
    }

    public bool IsTeleported()
    {
        return CurrentState == State.Teleported;
    }

    public IEnumerator AlphaFade(float pAlpha = 0)
    {
        // Alpha start value.
        float currentAlpha = material.color.a;

        if (pAlpha < currentAlpha) {
            // Loop until aplha is below zero (completely invisalbe)
            while (currentAlpha > pAlpha)
            {
                // Reduce alpha by fadeSpeed amount.
                currentAlpha -= alphaSpeed * Time.deltaTime;
                // Create a new color using original color RGB values combined with new alpha value
                material.color = new Color(color.r, color.g, color.b, currentAlpha);

                yield return null;
            }
        }
        else
        {
            while (currentAlpha < pAlpha)
            {
                // Reduce alpha by fadeSpeed amount.
                currentAlpha += alphaSpeed * Time.deltaTime;
                // Create a new color using original color RGB values combined with new alpha value
                material.color = new Color(color.r, color.g, color.b, currentAlpha);

                yield return null;
            }
        }
    }
}
