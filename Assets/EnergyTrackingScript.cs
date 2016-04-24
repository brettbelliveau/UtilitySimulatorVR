using UnityEngine;
using System.Collections;

public class EnergyTrackingScript : MonoBehaviour {
    public double time;
    public double kWh;
    public double jouUsed; //joules of energy consumed;
    public double bulbs = 0;   //how many bulbs are on
    public double bulbconstant; //wattage rating of the installed bulbs
    public int tv = 0; //is the tv on?
    public int dryer = 0; //is the dryer on?
    public int washer = 0; //is the washer on?
    public bool run; //is the simulation on or stopped?
    
    // Use this for initialization
	void Start () {
        InvokeRepeating("UpdateEnergy",0,1);
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
    double kWhcalc()
    {
        kWh = (jouUsed / time) * 3.6; //kWh calculation for the course of the simulation;
        return kWh;
    }
}
