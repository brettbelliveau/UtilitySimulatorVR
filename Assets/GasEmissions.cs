using UnityEngine;
using System.Collections;

public class GasEmissions : MonoBehaviour {
    public float totalkwh;
    float co2;
	// Use this for initialization
	void Start () {
        co2 = totalkwh * 1.27f;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
