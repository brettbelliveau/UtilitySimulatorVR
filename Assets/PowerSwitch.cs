using UnityEngine;
using System.Collections;
namespace VRStandardAssets.Utils
{
    public class PowerSwitch : MonoBehaviour
    {
        
        public EnergyTrackingScript other; //drag the main EnergyTracking script here;
        [SerializeField]private VRInteractiveItem m_InteractiveItem;
        public int objects; //how many objects does this switch control?
        public GameObject[] LightArr = new GameObject[10]; //the lights this switch controls in the scene;
        // Use this for initialization
        [SerializeField] private Autowalk autoScript;
        
        bool on = false;
        void Start()
        {

        }
        private void OnEnable()
        {
            m_InteractiveItem.OnClick += HandleClick;
            autoScript.clickedSomething = true;
        }
        private void HandleClick()
        {
            //turn on lights at a click;
            SetLights();
        }
        public void SetLights() //turn lights on (adding them to the wattage calculation.)
        {
            if (on)
            {
                on = false;
                other.bulbs -= objects;
            }
            else {
                on = true;
                other.bulbs += objects;
            }
            //write function to active lighting component of the GameObjects in the light array.
        }
    }
}
