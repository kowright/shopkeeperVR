using System;
using System.Collections.Generic;
using System.Text;

namespace Assets.Scripts.Items
{
    using UnityEngine;
    using UnityEngine.XR.Interaction.Toolkit;
    using UnityEngine.XR.Interaction.Toolkit.Interactables;

    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(XRGrabInteractable))]
    public class EnableGravityOnGrab : MonoBehaviour
    {
        private Rigidbody rb;
        private XRGrabInteractable grabInteractable;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            grabInteractable = GetComponent<XRGrabInteractable>();

            // Start with gravity off
            rb.useGravity = false;
        }

        private void OnEnable()
        {
            grabInteractable.selectEntered.AddListener(OnGrab);
            grabInteractable.selectExited.AddListener(OnRelease); // optional
        }

        private void OnDisable()
        {
            grabInteractable.selectEntered.RemoveListener(OnGrab);
            grabInteractable.selectExited.RemoveListener(OnRelease);
        }

        private void OnGrab(SelectEnterEventArgs args)
        {
            rb.useGravity = true;
        }

        private void OnRelease(SelectExitEventArgs args)
        {
            // Optional behavior:
            // keep gravity ON after letting go (most realistic)
            rb.useGravity = true;

            // OR if you want it to float again after release:
            // rb.useGravity = false;
        }
    }
}
