using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public UnityEvent InteractAction;
    public UnityEvent LookAction;
    public UnityEvent VisibilityAction;
    public string ActionText;
    public string AltActionText;
    private bool ActionTextMode;
    private bool visible;
    private string _keyword;
    // Start is called before the first frame update
    void Awake()
    {
        visible = true;
        ActionTextMode = false;
        this.tag = "Interactable";
        _keyword = "Object";
    }
    public void FormatWithKeyWord(string word) => _keyword = word;

    public string GetActionText()
    {
        string text = (ActionTextMode) ? AltActionText.Replace("@", _keyword) : ActionText.Replace("@", _keyword);
        return (visible) ? text : "";
    }

    public bool SetActionTextMode(bool b) => ActionTextMode = b;
    public bool SetVisible(bool b) => visible = b;

    public void AddAction(UnityAction _action)
    {
        InteractAction.AddListener(_action);
    }

    public void AddLook(UnityAction _action)
    {
        LookAction.AddListener(_action);
    }

    public void AddVisibility(UnityAction _action)
    {
        VisibilityAction.AddListener(_action);
    }

    public void Trigger()
    {
        InteractAction?.Invoke();
    }

    public void TriggerLook()
    {
        VisibilityAction?.Invoke();
        LookAction?.Invoke();
    }
}
