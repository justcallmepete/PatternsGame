using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/** This class represents the guard charge animation
 * This class is part of the Guard prefab and the methods are being called from the gaurd methods.
 * 
 * */
public class ChargeSystem : MonoBehaviour
{

    public UnityEvent methods;
    [SerializeField]
    float chargeTime;
    float currentChargeTime;
    [SerializeField]
    float startSimulateSpeed, endSimulateSpeed;
    [SerializeField]
    float startRateOverTime, endRateOverTime;

    [Range(0.01f, 1f)]
    [SerializeField]
    float laserMoveSpeed = 0.2f;
    float currentDistanceToTarget = 0f;
    [SerializeField]
    ParticleSystem ps_chargeParticles, ps_chargeCenter, ps_chargeCenter_2;

    GuardStateMachine myGuard;
    GameObject target;

    LineRenderer laserRenderer;
    enum ChargeState { start, charging, stopCharging, shooting, done };
    ChargeState currentState = ChargeState.start;

    bool isCharging;
    // Use this for initialization
    void Start()
    {
        methods.Invoke();
        myGuard = GetComponentInParent<GuardStateMachine>();
        laserRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case ChargeState.charging:
                HandleCharging();
                break;
            case ChargeState.stopCharging:
                HandleStopCharging();
                break;
            case ChargeState.shooting:
                HandleShoot();
                break;
            default:
                break;
        }
    }

    public void BeginCharge()
    {
        StartAnimation();
        currentState = ChargeState.charging;
    }

    public void StopCharge()
    {
        currentState = ChargeState.stopCharging;
    }

    void StartAnimation()
    {
        isCharging = true;
        currentChargeTime = 0;
        ps_chargeParticles.Play();
        ps_chargeCenter_2.Play();
        ps_chargeCenter.Play();
    }

    void StopAnimation()
    {
        isCharging = false;
        currentChargeTime = 0;
        ps_chargeParticles.Stop();
        ps_chargeCenter_2.Stop();
        ps_chargeCenter.Stop();
    }

    void EndAnimation()
    {
        isCharging = false;
    }

    void HandleChargeValues()
    {
        if (!isCharging) return;

        var main_chargeParticles = ps_chargeParticles.main;
        var main_chargeCenter = ps_chargeCenter_2.main;
        var main_chargeCenter_2 = ps_chargeCenter.main;
        var emission_chargeParticles = ps_chargeParticles.emission;
        main_chargeParticles.simulationSpeed = Mathf.Lerp(startSimulateSpeed, endSimulateSpeed, currentChargeTime / chargeTime);
        main_chargeCenter.simulationSpeed = Mathf.Lerp(startSimulateSpeed, endSimulateSpeed, currentChargeTime / chargeTime);
        main_chargeCenter_2.simulationSpeed = Mathf.Lerp(startSimulateSpeed, endSimulateSpeed, currentChargeTime / chargeTime);
        emission_chargeParticles.rateOverTime = Mathf.Lerp(startRateOverTime, endRateOverTime, currentChargeTime / chargeTime);
    }

    void HandleCharging()
    {
        currentChargeTime += Time.deltaTime;
        if (currentChargeTime >= chargeTime)
        {
            Shoot();
            currentState = ChargeState.shooting;
        }
        HandleChargeValues();
    }

    void HandleStopCharging()
    {
        currentChargeTime -= Time.deltaTime;
        if (currentChargeTime <= 0)
        {
            StopAnimation();
            currentState = ChargeState.start;
            return;
        }
        HandleChargeValues();
    }

    void Shoot()
    {
        GameManager.Instance.SlowMotion(0.2f, 3);
        target = myGuard.TargetPlayer.gameObject;
        currentDistanceToTarget = 0f;
    }

    void HandleShoot()
    {
        target = myGuard.TargetPlayer.gameObject;
        Vector3 targetPos = target.transform.position;
        targetPos.y = transform.position.y;
        float distanceToTarget = Vector3.Distance(transform.position, targetPos);
        currentDistanceToTarget += laserMoveSpeed;
        Vector3 dirToTarget = (targetPos - transform.position).normalized;
        float distanceRatio = Mathf.Clamp(currentDistanceToTarget / distanceToTarget, 0, 1);
        laserRenderer.SetPosition(1, transform.InverseTransformPoint(transform.position + (targetPos - transform.position) * distanceRatio));

        if (distanceRatio >= 1)//Reached target
        {
            PlayerHit();
            StartCoroutine(RemoveLaser(1));
        }
    }

    void PlayerHit()
    {
        MainPlayer player = target.GetComponent<MainPlayer>();
        player.GetHit();
        currentState = ChargeState.done;
        StartCoroutine(DelayBeforeReload(3f));
        GameManager.Instance.SlowMotion(0.2f, 3);
    }

    IEnumerator DelayBeforeReload(float sec)
    {
        yield return new WaitForSeconds(sec);

        // Should be in, doesnt work atm
        GameManager.Instance.ReloadCheckpoint();
    }

    IEnumerator RemoveLaser(float sec)
    {
        yield return new WaitForSecondsRealtime(sec);
        laserRenderer.SetPosition(1, transform.InverseTransformPoint(transform.position));
    }
}
