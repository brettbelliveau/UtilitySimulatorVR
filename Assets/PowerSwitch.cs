using UnityEngine;
using System.Collections;
namespace VRStandardAssets.Utils
{
    public class PowerSwitch : MonoBehaviour
    {
        
        [SerializeField] private VRInteractiveItem m_InteractiveItem;
        [SerializeField] private VRInput m_VRInput;                         // Reference to the VRInput to detect button presses.
        [SerializeField] private SelectionRadial m_SelectionRadial;         // Optional reference to the SelectionRadial, if non-null the duration of the SelectionRadial will be used instead.
        [SerializeField] private Collider m_Collider;                       // Optional reference to the Collider used to detect the user's gaze, turned off when the UIFader is not visible.
        
        
        
        public EnergyTrackingScript other; //drag the main EnergyTracking script here;
        public int objects; //how many objects does this switch control?
        public GameObject[] LightArr = new GameObject[10]; //the lights this switch controls in the scene;
        private bool m_GazeOver;                                            // Whether the user is currently looking at the bar.

        // Use this for initialization
        [SerializeField] private Autowalk autoScript;
        
        bool on = false;
        void Start()
        {

        }
        
        private void OnEnable()
        {
            m_VRInput.OnDown += HandleDown;
            m_VRInput.OnUp += HandleUp;

            m_InteractiveItem.OnOver += HandleOver;
            m_InteractiveItem.OnOut += HandleOut;
        }

        private void OnDisable()
        {
            m_VRInput.OnDown -= HandleDown;
            m_VRInput.OnUp -= HandleUp;

            m_InteractiveItem.OnOver -= HandleOver;
            m_InteractiveItem.OnOut -= HandleOut;
        }

        private void HandleDown()
        {
            if (m_GazeOver)
            {
                SetLights();
                autoScript.clickedSomething = true;
            }
            //turn on lights at a click;
        }

        private void HandleUp()
        { }

        private void HandleOver()
        {
            // The user is now looking at the bar.
            m_GazeOver = true;
            if (autoScript != null)
            {
                autoScript.setCanWalk(false);
            }
        }


        private void HandleOut()
        {
            m_GazeOver = false;
            if (autoScript != null)
            {
                autoScript.setCanWalk(true);
            }
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
