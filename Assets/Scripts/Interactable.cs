using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public UnityEvent InteractAction;
    public UnityEvent LookAction;
    public string ActionText;
    public string AltActionText;
    private bool ActionTextMode;

    // Start is called before the first frame update
    void Awake()
    {
        ActionTextMode = false;
        this.tag = "Interactable";
    }

    public string GetActionText()
    {
        return (ActionTextMode) ? AltActionText : ActionText;
    }

    public bool SetActionTextMode(bool b) => ActionTextMode = b;

    public void AddAction(UnityAction _action)
    {
        InteractAction.AddListener(_action);
    }

    public void AddLook(UnityAction _action)
    {
        LookAction.AddListener(_action);
    }

    public void Trigger()
    {
        InteractAction?.Invoke();
    }

    public void TriggerLook()
    {
        LookAction?.Invoke();
    }
}
