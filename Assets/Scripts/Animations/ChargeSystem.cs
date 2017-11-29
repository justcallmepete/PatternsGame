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
            StartAnimation();
            startCharge = false;
        }
        if (endCharge)
        {
            EndAnimation();
            endCharge = false;
        }
        if (isCharging)
        {
            currentChargeTime += Time.deltaTime;
            HandleChargeValues();
        }
	}

    void StartAnimation()
    {
        isCharging = true;
        currentChargeTime = 0;
        ps_chargeParticles.Play();
        ps_chargeCenter_2.Play();
        ps_chargeCenter.Play();
    }

    void EndAnimation()
    {
        isCharging = false;
    }

    void HandleChargeValues()
    {
        var main_chargeParticles = ps_chargeParticles.main;
        var main_chargeCenter = ps_chargeCenter_2.main;
        var main_chargeCenter_2 = ps_chargeCenter.main;
        var emission_chargeParticles = ps_chargeParticles.emission;
        main_chargeParticles.simulationSpeed = Mathf.Lerp(startSimulateSpeed, endSimulateSpeed, currentChargeTime/chargeTime);
        main_chargeCenter.simulationSpeed = Mathf.Lerp(startSimulateSpeed, endSimulateSpeed, currentChargeTime / chargeTime);
        main_chargeCenter_2.simulationSpeed = Mathf.Lerp(startSimulateSpeed, endSimulateSpeed, currentChargeTime / chargeTime);
        emission_chargeParticles.rateOverTime = Mathf.Lerp(startRateOverTime, endRateOverTime, currentChargeTime/chargeTime);
    }
}
