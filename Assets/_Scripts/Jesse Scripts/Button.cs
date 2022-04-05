﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace BNG {
    /// <summary>
    /// Physical button helper with 
    /// </summary>
    /// 

    //Added custom unityevent class
    [System.Serializable]
    public class MyCrisisTypeEvent : UnityEvent<CrisisSubType>
    {
    }

    public class Button : MonoBehaviour {

        [Tooltip("The Local Y position of the button when it is pushed all the way down. Local Y position will never be less than this.")]
        public float MinLocalY = 0.25f;

        [Tooltip("The Local Y position of the button when it is not being pushed. Local Y position will never be greater than this.")]
        public float MaxLocalY = 0.55f;

        [Tooltip("How far away from MinLocalY / MaxLocalY to be considered a click")]
        public float ClickTolerance = 0.01f;

        [Tooltip("If true the button can be pressed by physical object by utiizing a Spring Joint. Set to false if you don't need / want physics interactions, or are using this on a moving platform.")]
        public bool AllowPhysicsForces = true;

        List<Grabber> grabbers = new List<Grabber>(); // Grabbers in our trigger
        List<UITrigger> uiTriggers = new List<UITrigger>(); // UITriggers in our trigger
        SpringJoint joint;

        bool clickingDown = false;
        public AudioClip ButtonClick;
        public AudioClip ButtonClickUp;

        //Commented this out -Jesse
        //public Tutka SolverGameObject;


        [Header("Regular Unity event")]
        public UnityEvent onButtonDown;
        public UnityEvent onButtonUp;

        AudioSource audioSource;
        Rigidbody rigid;


        [Space(3)]

        // My own code starts -Jesse

        [Header("Select crisis type the button will solve")]
        public CrisisSubType crisisSubType;

        [Space (3)]

        [Header("Custom crisisType event")]
        public MyCrisisTypeEvent onButtonDownCrisisType;
        public MyCrisisTypeEvent onButtonUpCrisisType;

        [Space(3)]

        [Header("IS BUTTON ACTIVE")]
        [Space(2)]
        public bool buttonActive;

        // My own code ends -Jesse

        void Start() {
            joint = GetComponent<SpringJoint>();
            rigid = GetComponent<Rigidbody>();

            // Set to kinematic so we are not affected by outside forces
            if(!AllowPhysicsForces) {
                rigid.isKinematic = true;
            }

            // Start with button up top / popped up
            transform.localPosition = new Vector3(transform.localPosition.x, MaxLocalY, transform.localPosition.z);

            audioSource = GetComponent<AudioSource>();


            if (onButtonDown == null)
                onButtonDown = new UnityEvent();

            if (onButtonDown == null)
                onButtonUp = new UnityEvent();

            onButtonDown.AddListener(Ping);

            // Add buttonclicks to my custom event -Jesse           

            if (onButtonDownCrisisType == null)
                    onButtonDownCrisisType = new MyCrisisTypeEvent();

            if (onButtonDownCrisisType == null)
                    onButtonUpCrisisType = new MyCrisisTypeEvent();

            // Add listener for custom event - Jesse
            onButtonDownCrisisType.AddListener(PingCrisis);

        }

        //My listener for custom event
        void PingCrisis(CrisisSubType crisisSubType)
        {
            Debug.Log("Pressed " + crisisSubType + " button");
        }

        void Ping()
        {
            Debug.Log("Pressed button");
        }


        // These have been hard coded for hand speed
        float ButtonSpeed = 15f;
        float SpringForce = 1500f;
        Vector3 buttonDownPosition;
        Vector3 buttonUpPosition;


        void Update() {

            buttonDownPosition = GetButtonDownPosition();
            buttonUpPosition = GetButtonUpPosition();
            bool grabberInButton = false;
            bool UITriggerInButton = uiTriggers != null && uiTriggers.Count > 0;

            // Find a valid grabber to push down
            for (int x = 0; x < grabbers.Count; x++) {
                if (!grabbers[x].HoldingItem) {
                    grabberInButton = true;
                    break;
                }
            }
            // push button down
            if (grabberInButton || UITriggerInButton) {
                float speed = ButtonSpeed; 
                transform.localPosition = Vector3.Lerp(transform.localPosition, buttonDownPosition, speed * Time.deltaTime);

                if(joint) {
                    joint.spring = 0;
                }
            }
            else {
                // Let the spring push the button up if physics forces are enabled
                if (AllowPhysicsForces) {
                    if(joint) {
                        joint.spring = SpringForce;
                    }
                }
                // Need to lerp back into position if spring won't do it for us
                else {
                    float speed = ButtonSpeed;
                    transform.localPosition = Vector3.Lerp(transform.localPosition, buttonUpPosition, speed * Time.deltaTime);
                    if(joint) {
                        joint.spring = 0;
                    }
                }
            }

            // Cap values
            if (transform.localPosition.y < MinLocalY) {
                transform.localPosition = buttonDownPosition;
            }
            else if (transform.localPosition.y > MaxLocalY) {
                transform.localPosition = buttonUpPosition;
            }

            // Click Down?
            float buttonDownDistance = transform.localPosition.y - buttonDownPosition.y;
            if (buttonDownDistance <= ClickTolerance && !clickingDown) {
                clickingDown = true;
                OnButtonDown();
            }
            // Click Up?
            float buttonUpDistance = buttonUpPosition.y - transform.localPosition.y;
            if (buttonUpDistance <= ClickTolerance && clickingDown) {
                clickingDown = false;
                OnButtonUp();
            }
        }

        public virtual Vector3 GetButtonUpPosition() {
            return new Vector3(transform.localPosition.x, MaxLocalY, transform.localPosition.z);
        }

        public virtual Vector3 GetButtonDownPosition() {
            return new Vector3(transform.localPosition.x, MinLocalY, transform.localPosition.z);
        }

        // Callback for ButtonDown
        public virtual void OnButtonDown() {

            // Play sound
            if (audioSource && ButtonClick) {
                audioSource.clip = ButtonClick;
                audioSource.Play();
            }

            // Call event if button is active!
            if (onButtonDown != null && buttonActive == true)
            {
                onButtonDown.Invoke();
            }

            // Call CrisisType event if button is active!
            if (onButtonDownCrisisType != null && buttonActive == true) {  
                onButtonDownCrisisType.Invoke(crisisSubType);                
            }
        }

        // Callback for ButtonDown
        public virtual void OnButtonUp() {
            // Play sound
            if (audioSource && ButtonClickUp) {
                audioSource.clip = ButtonClickUp;
                audioSource.Play();
            }

            // Call event if button is active!
            if (onButtonUp != null && buttonActive == true)
            {
                onButtonUp.Invoke();
            }

            // Call crisistype event if button is active!
            if (onButtonUpCrisisType != null && buttonActive == true) {
                onButtonUpCrisisType.Invoke(crisisSubType);
            }
        }

        //My activate/deactivate functions -Jesse
        public void ActivateButton()
        {
            buttonActive = true;
        }

        public void DeactivateButton()
        {
            buttonActive = false;
        }
        //My activate/deactivate functions -Jesse


        void OnTriggerEnter(Collider other) {
            // Check Grabber
            Grabber grab = other.GetComponent<Grabber>();
            if (grab != null) {
                if (grabbers == null) {
                    grabbers = new List<Grabber>();
                }

                if (!grabbers.Contains(grab)) {
                    grabbers.Add(grab);
                }
            }

            // Check UITrigger, which is another type of activator
            UITrigger trigger = other.GetComponent<UITrigger>();
            if (trigger != null) {
                if (uiTriggers == null) {
                    uiTriggers = new List<UITrigger>();
                }

                if (!uiTriggers.Contains(trigger)) {
                    uiTriggers.Add(trigger);
                }
            }
        }

        void OnTriggerExit(Collider other) {
            Grabber grab = other.GetComponent<Grabber>();
            if (grab != null) {
                if (grabbers.Contains(grab)) {
                    grabbers.Remove(grab);
                }
            }

            UITrigger trigger = other.GetComponent<UITrigger>();
            if (trigger != null) {
                if (uiTriggers.Contains(trigger)) {
                    uiTriggers.Remove(trigger);
                }
            }
        }

        void OnDrawGizmosSelected() {
            // Show Grip Point
            Gizmos.color = Color.blue;

            Vector3 upPosition = transform.TransformPoint(new Vector3(transform.localPosition.x, MaxLocalY, transform.localPosition.z));
            Vector3 downPosition = transform.TransformPoint(new Vector3(transform.localPosition.x, MinLocalY, transform.localPosition.z));

            Vector3 size = new Vector3(0.005f, 0.005f, 0.005f);

            Gizmos.DrawCube(upPosition, size);

            Gizmos.color = Color.yellow;
            Gizmos.DrawCube(downPosition, size);

            Gizmos.color = Color.red;
            Gizmos.DrawLine(upPosition, downPosition);
        }
    }
}