using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class VirtualHandPresence : MonoBehaviour
{
    // public InputDeviceCharacteristics controllerCharacteristics;    
    // private InputDevice targetDevice;
    public Animator HandAnimator;

    void Start()
    {
        // TryInitialize();
    }

    // void TryInitialize()
    // {
    //     List<InputDevice> devices = new List<InputDevice>();

    //     InputDevices.GetDevicesWithCharacteristics(controllerCharacteristics, devices);
    //     if (devices.Count > 0)
    //     {
    //         targetDevice = devices[0];
    //     }
    // }

    // public bool GetHandGrip()
    // {
    //     return targetDevice.TryGetFeatureValue(CommonUsages.grip, out float gripValue);
    // }

    // Update is called once per frame
    void Update()
    {
    //     if(!targetDevice.isValid)
    //     {
    //         TryInitialize();
    //     }
    //     else
    //     {
    //         UpdateHandAnimation();
    //     }
    }

    public void setGrip(float gripValue, float triggerValue)
    {
        HandAnimator.SetFloat("Grip", gripValue);
        HandAnimator.SetFloat("Trigger", triggerValue);
        Debug.Log(HandAnimator.GetFloat("Grip"));
    }
}
