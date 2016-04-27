using UnityEngine;
using System.Collections;

namespace VRproj.FatStacks
{
    public class FatStacks : MonoBehaviour
    {
      
        Transform stack;
        public int test;
        public float fillrate;
        public GameObject stackz;
        public GameObject pedestal;
        public float xo, x, oy, y, oz, z;
        public float moneylimit;
        public float pmoney;
        public float CashConstant; //how much does each block represent?
      

        // Use this for initialization
        void Start()
        {
            
                xo = (pedestal.transform.position.x) - 2f;
                x = xo;
                oy = pedestal.transform.position.y;
                y = oy;
                oz = pedestal.transform.position.z - 2f;
                z = oz;
            //
        }
        // Update is called once per frame
        void Update()
        {

        }
        public void StartSim()
        {
            InvokeRepeating("Place", 0, fillrate*2);

        }
        public void StopSim()
        {
            CancelInvoke();
        }
        public void Place()
        {
           if (pmoney <= moneylimit)
            {
                if (x > xo + 2f)
                {
                    x = xo;
                    z += 0.08f;
                }
                if (z > oz + 2f)
                {
                    z = oz;
                    x = xo;
                    y += 0.08f;
                }
                Instantiate(stackz, new Vector3(x, y, z), Quaternion.identity);
                pmoney += CashConstant;
                x += 0.1f;
            }

        }
    }
}
