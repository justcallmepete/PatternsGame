using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ChargeSystem : MonoBehaviour {

    public UnityEvent methods;
    [SerializeField]
    float chargeTime;
    float currentChargeTime;
    [SerializeField]
    float startSimulateSpeed, endSimulateSpeed;
    [SerializeField]
    float startRateOverTime, endRateOverTime;


    [SerializeField]
    ParticleSystem ps_chargeParticles, ps_chargeCenter, ps_chargeCenter_2;


    enum ChargeState { start, charging, stopCharging, shooting, done };
    ChargeState currentState = ChargeState.start;

    bool isCharging;
    public bool startCharge;
    public bool endCharge;
	// Use this for initialization
	void Start () {
        methods.Invoke();
    }
	
	// Update is called once per frame
	void Update () {
        if (startCharge)
        {
            BeginCharge();
            startCharge = false;
        }
        if (endCharge)
        {
            StopCharge();
            endCharge = false;
        }

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
        Debug.Log("begin charge");
        currentState = ChargeState.charging;
    }

    public void StopCharge()
    {
        currentState = ChargeState.stopCharging;
        Debug.Log("stop charge");

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
        main_chargeParticles.simulationSpeed = Mathf.Lerp(startSimulateSpeed, endSimulateSpeed, currentChargeTime/chargeTime);
        main_chargeCenter.simulationSpeed = Mathf.Lerp(startSimulateSpeed, endSimulateSpeed, currentChargeTime / chargeTime);
        main_chargeCenter_2.simulationSpeed = Mathf.Lerp(startSimulateSpeed, endSimulateSpeed, currentChargeTime / chargeTime);
        emission_chargeParticles.rateOverTime = Mathf.Lerp(startRateOverTime, endRateOverTime, currentChargeTime/chargeTime);
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
        Debug.Log("Shoot");
    }

    void HandleShoot()
    {
        //Stop charge animations
        //Handle line rederer distance
    }
}
