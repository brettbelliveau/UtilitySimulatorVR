using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;
namespace VRStandardAssets.Utils
{
    public class ClothingPickup : MonoBehaviour
    {
        [SerializeField] private VRInteractiveItem m_InteractiveItem;
        [SerializeField] private GameObject FPSControllerObject;
        [SerializeField] private VRInput m_VRInput;                         // Reference to the VRInput to detect button presses.
        [SerializeField] private Autowalk walkingScript;
        [SerializeField] private SelectionRadial m_SelectionRadial;         // Optional reference to the SelectionRadial, if non-null the duration of the SelectionRadial will be used instead.
        [SerializeField] private Collider m_Collider;                       // Optional reference to the Collider used to detect the user's gaze, turned off when the UIFader is not visible.
        [SerializeField] private Autowalk walkScript;

        private FirstPersonController fps;
        private bool m_GazeOver;                                            // Whether the user is currently looking at the bar.

        // Use this for initialization
        void Start()
        {
            fps = FPSControllerObject.GetComponent<FirstPersonController>();  
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
            // If the user is looking at the clothing  pick it up!
            if (m_GazeOver)
            {
                fps.PickUpClothing();
                gameObject.SetActive(false);
                walkScript.clickedSomething = true;
            }

        }

        private void HandleUp()
        { }

//        private void HandleClick()
//        {
//            fps.PickUpClothing();
//            enabled = false;
//        }

        private void HandleOver()
        {
            // The user is now looking at the bar.
            m_GazeOver = true;
            if (walkScript != null)
            {
                walkScript.setCanWalk(false);
            }
        }


        private void HandleOut()
        {
            m_GazeOver = false;
            if (walkScript != null)
            {
                walkScript.setCanWalk(true);
            }
        }

    }
}
