using UnityEngine;
using System.Collections;

public class FatStacks : MonoBehaviour {
    Transform stack;
    public int test;
    public float fillrate;
    public GameObject stackz;
    public GameObject pedestal;
    public float xo,x,oy,y,oz,z;
    // Use this for initialization
    void Start()
    {
        xo = (pedestal.transform.position.x) - 2f ;
        x = xo;
        oy = pedestal.transform.position.y;
        y = oy;
        oz = pedestal.transform.position.z - 2f;
        z = oz;
        InvokeRepeating("Place", 0, fillrate);
    }
	// Update is called once per frame
	void Update () {
	
	}
    public void Place(){
        
        if (x > xo + 4f)
        {
            x = xo;
            z += 0.5f;
        }
        if(z > oz + 4f)
        {
            z = oz;
            x = xo;
            y += 0.5f;
        }
        Instantiate(stackz,new Vector3(x,y,z), Quaternion.identity);
        x += 0.5f;


    }
}
