using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;
using VRproj.FatStacks;

namespace VRStandardAssets.Utils
{
    public class SimControl : MonoBehaviour
    {
        public EnergyTrackingScript Starter;
        public DayNightController rotate;
        public FatStacks simpad;
        public float DollarsPerKwh;
        public event Action OnBarFilled;                                    // This event is triggered when the bar finishes filling.
        [SerializeField]
        private GameObject FPSController;

        
        public float m_Duration;                     // The length of time it takes for the bar to fill.
        [SerializeField]
        private AudioSource m_Audio;                       // Reference to the audio source that will play effects when the user looks at it and when it fills.
        [SerializeField]
        private AudioClip m_OnOverClip;                    // The clip to play when the user looks at the bar.
        [SerializeField]
        private AudioClip m_OnFilledClip;                  // The clip to play when the bar finishes filling.
        [SerializeField]
        private Slider m_Slider;                           // Optional reference to the UI slider (unnecessary if using a standard Renderer).
        [SerializeField]
        private VRInteractiveItem m_InteractiveItem;       // Reference to the VRInteractiveItem to determine when to fill the bar.
        [SerializeField]
        private VRInput m_VRInput;                         // Reference to the VRInput to detect button presses.
        [SerializeField]
        private GameObject m_BarCanvas;                    // Optional reference to the GameObject that holds the slider (only necessary if DisappearOnBarFill is true).
        [SerializeField]
        private Renderer m_Renderer;                       // Optional reference to a renderer (unnecessary if using a UI slider).
        [SerializeField]
        private SelectionRadial m_SelectionRadial;         // Optional reference to the SelectionRadial, if non-null the duration of the SelectionRadial will be used instead.
        [SerializeField]
        private UIFader m_UIFader;                         // Optional reference to a UIFader, used if the SelectionSlider needs to fade out.
        [SerializeField]
        private Collider m_Collider;                       // Optional reference to the Collider used to detect the user's gaze, turned off when the UIFader is not visible.
        [SerializeField]
        private bool m_DisableOnBarFill;                   // Whether the bar should stop reacting once it's been filled (for single use bars).
        [SerializeField]
        private bool m_DisappearOnBarFill;                 // Whether the bar should disappear instantly once it's been filled.
        [SerializeField]
        private bool m_DisableOnClick;                     // Whether the bar should disable after the first click.
        [SerializeField]
        private bool m_LockMovementOnClick;                // Stop movement after button is clicked and until bar is filled.
        [SerializeField]
        private GameObject text;                           // Any text on the panel/slider
        [SerializeField]
        private SelectionSlider m_PairedSlider;
        public bool laundrydone = false;
        public bool dishesdone = false;


        /* Fields used for money dispenser */

        //[SerializeField] private bool m_IsComparorObject;
        //[SerializeField] private MoneyDispenser money;
        //[SerializeField] private PanelDispenserHandler dispenserHandler;

        [SerializeField]
        private Autowalk walkingScript;

        FirstPersonController fps;
        private bool inProgress;                                            // Whether the fill is currently in progress.
        private bool m_BarFilled;                                           // Whether the bar is currently filled.
        private bool m_GazeOver;                                            // Whether the user is currently looking at the bar.
        private float m_Timer;                                              // Used to determine how much of the bar should be filled.
        private Coroutine m_FillBarRoutine;                                 // Reference to the coroutine that controls the bar filling up, used to stop it if required.
        private Coroutine m_FillOtherBarRoutine;                                 // Reference to the coroutine that controls the bar filling up, used to stop it if required.
        private const string k_SliderMaterialPropertyName = "_SliderValue"; // The name of the property on the SlidingUV shader that needs to be changed in order for it to fill.
        private int fontSize;
        private float walkSpeed;
        private float runSpeed;

        private void Start()
        {
            fontSize = text.GetComponent<Text>().fontSize;
            inProgress = false;

            fps = FPSController.GetComponent<FirstPersonController>();
            walkSpeed = fps.m_WalkSpeed;
            runSpeed = fps.m_RunSpeed;
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


        private void Update()
        {
            if (!m_UIFader)
                return;

            // If this bar is using a UIFader turn off the collider when it's invisible.
            m_Collider.enabled = m_UIFader.Visible;
        }


        public IEnumerator WaitForBarToFill()
        {
            // Disable the bar so user cannot repeatedly press it
            // This has been done with the inProgress variable
            //            if (m_DisableOnClick)
            //                enabled = false;

            // If the bar should disappear when it's filled, it needs to be visible now.
            if (m_BarCanvas && m_DisappearOnBarFill)
                m_BarCanvas.SetActive(true);

            // Currently the bar is unfilled.
            m_BarFilled = false;

            // Reset the timer and set the slider value as such.
            m_Timer = 0f;
            SetSliderValue(0f);

            // Keep coming back each frame until the bar is filled.
            while (!m_BarFilled)
            {
                //fps.m_WalkSpeed = 0;
                yield return null;
            }

            // If the bar should disappear once it's filled, turn it off.
            if (m_BarCanvas && m_DisappearOnBarFill)
                m_BarCanvas.SetActive(false);
        }


        public IEnumerator FillBar()
        {
            simpad.StartSim();
            inProgress = true;
            // Disable the bar so user cannot repeatedly press it
            if (m_DisableOnClick)
            {
                //enabled = false;
            }

            // Disable movement until bar has been filled
            if (m_LockMovementOnClick)
            {
                fps.m_WalkSpeed = fps.m_RunSpeed = 0;
            }
           

            m_Audio.clip = m_OnFilledClip;
            m_Audio.Play();

            // When the bar starts to fill, reset the timer.
            m_Timer = 0f;
            var newText = "\n Activity \n Progress: ";

            // The amount of time it takes to fill is either the duration set in the inspector, or the duration of the radial.
            float fillTime = m_SelectionRadial != null ? m_SelectionRadial.SelectionDuration : m_Duration;

            // Until the timer is greater than the fill time...
            while (m_Timer < fillTime)
            {
                // ... add to the timer the difference between frames.
                m_Timer += Time.deltaTime;

                text.GetComponent<Text>().text = newText + (int)((m_Timer / fillTime) * 100) + "%";

                // Set the value of the slider or the UV based on the normalised time.
                SetSliderValue(m_Timer / fillTime);

                // Wait until next frame.
                yield return null;

                // We want to continue the loop ever if the user looks away.
                if (true)
                    continue;

                // If the user is no longer looking at the bar, reset the timer and bar and leave the function.
                m_Timer = 0f;
                SetSliderValue(0f);
                yield break;
            }

            // Play the clip for when the bar is filled.
            m_Audio.clip = m_OnFilledClip;
            m_Audio.Play();
            //StopSimulation
            rotate.run = false;
            text.GetComponent<Text>().text = "\n Activity \n Complete";
            simpad.StopSim();
            //Set speeds back to normal
            fps.m_WalkSpeed = walkSpeed;
            fps.m_RunSpeed = runSpeed;
        }


        private void SetSliderValue(float sliderValue)
        {
            // If there is a slider component set it's value to the given slider value.
            if (m_Slider)
                m_Slider.value = sliderValue;

            // If there is a renderer set the shader's property to the given slider value.
            if (m_Renderer)
                m_Renderer.sharedMaterial.SetFloat(k_SliderMaterialPropertyName, sliderValue);
        }

        //To be called only by the paired slider
        public void InstantFill()
        {
            inProgress = true;
            SetSliderValue(1);
            text.GetComponent<Text>().text = "\n Activity \n Complete";
        }

        private void HandleDown()
        {
            // If the user is looking at the bar start the FillBar coroutine and store a reference to it.
            if (m_GazeOver && !inProgress && laundrydone && dishesdone)
            {
                Starter.Stop();
                simpad.moneylimit = ((Starter.kWhcalc() * DollarsPerKwh) / Starter.time) * 409968000f; //dollars spent over the time of th simulation * 13years in seconds.
                simpad.fillrate = (m_Duration/(simpad.moneylimit / simpad.CashConstant))*2.5f;
                m_FillBarRoutine = StartCoroutine(FillBar());
                Debug.Log("Starting");
                
                rotate.run = true;
                simpad.StartSim();
                if (m_PairedSlider != null)
                    m_PairedSlider.InstantFill();
            }
        }


        private void HandleUp()
        { }


        private void HandleOver()
        {
            // The user is now looking at the bar.
            m_GazeOver = true;
            if (walkingScript != null)
            {
                walkingScript.setCanWalk(false);
            }

            // Play the clip appropriate for when the user starts looking at the bar.
            m_Audio.clip = m_OnOverClip;
            m_Audio.Play();
        }


        private void HandleOut()
        {
            m_GazeOver = false;
            if (walkingScript != null)
            {
                walkingScript.setCanWalk(true);
            }
        }

        /* Below functions were built for toggle functionality, left for reference */
        /*
		private void ToggleSelection() {
			Debug.Log("Toggle Button");

			ToggleSliderValue();
			if (m_IsComparorObject) {
				Debug.Log("Toggle Comparater Dispenser");
				dispenserHandler.toggleComparatorDispensing(gameObject, money);
			}
			else {
				Debug.Log("Toggle Regular Dispenser");
				dispenserHandler.toggleDispensing(money);
			}

			// Play the clip for when the bar is filled.
			m_Audio.clip = m_OnFilledClip;
			m_Audio.Play();
		}

		public void ToggleSliderValue() {
			if (m_Slider.value == m_Slider.maxValue) {
				m_Slider.value = m_Slider.minValue;
			}
			else {
				m_Slider.value = m_Slider.maxValue;
			}
		}
        */
    }
}