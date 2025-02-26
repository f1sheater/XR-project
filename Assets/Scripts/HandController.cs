using UnityEngine;
using UnityEngine.InputSystem;

public class HandController : MonoBehaviour
{
    public InputActionReference grip;
    public InputActionReference trigger;
    public Hand hand;

    void Start()
    {
        grip.action.Enable();
        trigger.action.Enable();
    }

    void Update()
    {
        hand.SetGrip(grip.action.ReadValue<float>());
        hand.SetTrigger(trigger.action.ReadValue<float>());
    }
}
