using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.WitAi;
using UnityEngine.InputSystem;

public class ActivateVoice : MonoBehaviour
{
    [SerializeField]
    private Wit wit;

    // Update is called once per frame
    private void Update()
    {
        if(wit == null) wit = GetComponent<Wit>();
        // wit.Activate();
    }

    public void TriggerPressed(InputAction.CallbackContext context)
    {
        if(context.performed)
            WitActivate();
    }

    public void WitActivate()
    {
        wit.Activate();
    }
}
