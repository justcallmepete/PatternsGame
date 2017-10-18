using System.Collections;
using UnityEngine;

/*
 * The main player class to control the state of the player and other behaviours. Alpha Fading, 
 * Pulling and Teleporting are the other behaviours.
 */
 
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Controlable))]
[RequireComponent(typeof(Teleportation))]
[RequireComponent(typeof(PlayerLight))]
[RequireComponent(typeof(Pull))]
[RequireComponent(typeof(Inventory))]
[RequireComponent(typeof(Whistle))]
public class MainPlayer : MonoBehaviour
{
    [Header("General settings")]
    // Animation curve for pulling
    public AnimationCurve pullEasing;

    [SerializeField]
    private float outerRadius = 1.3f;
    [SerializeField]
    private float pullSpeed = 1;
    [SerializeField]
    private float teleportDelay = 1;

    private Controlable controlable;
    private Material material;
    public float alphaSpeed = 1f;    // How fast alpha value decreases.
    private Color color;            // Used to store color reference.

    public enum State
    {
        Free,
        Busy,
        Channelling
    }

    
    private State currentState = State.Free;
    public State CurrentState { get { return currentState; } set { currentState = value; } }

    private void Start()
    {
        // Cache components for later use
        controlable = gameObject.GetComponent<Controlable>();
        material = gameObject.GetComponent<MeshRenderer>().material;
        color = material.color;
    }

    #region Pull Mechanic
    public void BePulled(GameObject obj, float maxDistance)
    {
        currentState = State.Busy;

        StartCoroutine(PullPlayer(obj, maxDistance));
    }

    private IEnumerator PullPlayer(GameObject obj, float maxDistance)
    {
        // Wait for pulling
        yield return new WaitForSeconds(teleportDelay);

        float curveTime = 0f;
        float curveAmount = pullEasing.Evaluate(curveTime);

        // Calculate the path
        Vector3 path = gameObject.transform.position - obj.transform.position - 
            (gameObject.transform.position - obj.transform.position).normalized * outerRadius;

        // Set begin position
        Vector3 beginPosition = gameObject.transform.position;

        while (curveTime < pullEasing[pullEasing.length - 1].time)
        {
            // Update easing
            curveTime += Time.deltaTime * pullSpeed * maxDistance / path.magnitude;
            curveAmount = pullEasing.Evaluate(curveTime);

            // Update transform position
            gameObject.transform.position = beginPosition - curveAmount * path;
            yield return new WaitForEndOfFrame();
        }
        
        // Set state to free when target is reached
        currentState = State.Free;
    }

    public void PullPlayer(GameObject obj)
    {
        currentState = State.Busy;

        StartCoroutine(WaitForPlayer(obj));
    }

    #endregion

    public IEnumerator StartTeleport(GameObject pPlayer)
    {
        currentState = State.Busy;
        StartCoroutine(AlphaFade());
        // Wait for teleport
        yield return new WaitForSeconds(teleportDelay);

        Vector3 path = pPlayer.transform.position + (gameObject.transform.position - pPlayer.transform.position).normalized * outerRadius;
        gameObject.transform.position = path;

        StartCoroutine(AlphaFade(1));
        currentState = State.Free;
    }
    
    private IEnumerator WaitForPlayer(GameObject obj)
    {

        while (obj.GetComponent<MainPlayer>().IsBusy())
        {
            yield return new WaitForEndOfFrame();
        }

        currentState = State.Free;
    }

    public bool IsBusy()
    {
        return CurrentState == State.Busy;
    }

    public bool IsFree()
    {
        return CurrentState == State.Free;
    }

    public bool IsChannelling()
    {
        return CurrentState == State.Channelling;
    }

    private IEnumerator AlphaFade(float pAlpha = 0)
    {
        // Alpha start value.
        float currentAlpha = material.color.a;

        if (pAlpha < currentAlpha) {
            // Loop until aplha is below zero (completely invisalbe)
            while (currentAlpha > pAlpha)
            {
                // Reduce alpha by fadeSpeed amount.
                currentAlpha -= alphaSpeed * Time.deltaTime;
                Debug.Log(currentAlpha);
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
                Debug.Log(currentAlpha);
                // Create a new color using original color RGB values combined with new alpha value
                material.color = new Color(color.r, color.g, color.b, currentAlpha);

                yield return null;
            }
        }
    }
}
