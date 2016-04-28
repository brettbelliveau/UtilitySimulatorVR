using UnityEngine;
using System.Collections;
using VRStandardAssets.Utils;

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

    [SerializeField]
    private SelectionSlider dishesSlider;
    [SerializeField]
    private SelectionSlider laundrySlider;

    private float dishesCost = 10800*2;
    private float laundryCost = 10800*2;
    private bool addedDishes;
    private bool addedLaundry;
            
    // Use this for initialization
	void Start () {
        addedDishes = false;
        addedLaundry = false;
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
            if (!addedDishes && !dishesSlider.GetHandWashed())
            {
                addedDishes = true;
                jouUsed += dishesCost;
            }
            if (!addedLaundry && !laundrySlider.GetHandWashed())
            {
                addedLaundry = true;
                jouUsed += laundryCost;
            }
            time += 1; //timer from beginning of sim;
            jouUsed += bulbs * bulbconstant * 4;
            jouUsed += 213 * tv; //wattage of a powered TV.
        }
    }
    public float kWhcalc()
    {
        kWh = (jouUsed / 3600000); //kWh calculation for the course of the simulation;
        return kWh;
    }
}
