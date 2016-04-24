using UnityEngine;
using UnityEngine.UI;
using System.Collections;
namespace VRStandardAssets.Utils
{
    public class ApplianceSelector : MonoBehaviour
    {
        public EnergyTrackingScript other;
        [SerializeField] private VRInteractiveItem m_InteractiveItem;
        public int type; //wattage for the appliance;
        // Use this for initialization
        void Start()
        {

        }
        private void OnEnable()
        {
            m_InteractiveItem.OnClick += HandleClick;
        }
        private void HandleClick()
        {
            //Set the appliance to it's type's wattage
            SetAppliance(type);
        }
        // Update is called once per frame
        void Update()
        {

        }
        public void SetAppliance(int select)
        {
            //continue adding types for other appliance constants
            if (select == 1)
            {
                other.bulbconstant = 60; //incandescent
             
            }
            if (select == 2)
            {
                other.bulbconstant = 15; //CFL
                
            }
            if (select == 3)
            {
                other.bulbconstant = 12; //LED
                
            }
        }
    }
}
