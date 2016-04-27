using UnityEngine;
using System.Collections;

public class EnergyTrackingScript : MonoBehaviour {
    public float time;
    public float kWh;
    public float jouUsed; //joules of energy consumed;
    public float bulbs = 0;   //how many bulbs are on
    public float bulbconstant; //wattage rating of the installed bulbs
    public int tv = 0; //is the tv on?
    public int dryer = 0; //is the dryer on?
    public int washer = 0; //is the washer on?
    public bool run; //is the simulation on or stopped?
    
    // Use this for initialization
	void Start () {
        InvokeRepeating("UpdateEnergy",0,1);
	}
    public void Stop()
    {
        CancelInvoke();
    }
	
	// Update is called once per frame
	void Update () {
	
	}
    void UpdateEnergy()
    {
        if (run)
        {
            time += 1; //timer from beginning of sim;
            jouUsed += bulbs * bulbconstant;
            jouUsed += 213 * tv; //wattage of a powered TV.
        }
    }
    public float kWhcalc()
    {
        kWh = (jouUsed / 3600000); //kWh calculation for the course of the simulation;
        return kWh;
    }
}
