using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;

[System.Serializable]
public abstract class Interactable : MonoBehaviour
{
    public UnityEvent InteractAction;
    public UnityEvent LookAction;
    public UnityEvent VisibilityAction;
    public string ActionText;
    public string AltActionText;

    public AudioClip ActionSfx;
    public AudioClip AltActionSfx;
    private AudioSource Source;
    public AudioMixerGroup Group;

    private bool ActionTextMode;
    private bool visible;
    private string _keyword;
    private GameObject _instance;
    // Start is called before the first frame update
    public virtual void Init()
    {
        _instance = gameObject;

        visible = true;
        ActionTextMode = false;
        _instance.tag = "Interactable";
        _keyword = "Object";
        Source = _instance.AddComponent<AudioSource>();
        Source.playOnAwake = false;
        Source.outputAudioMixerGroup = Group;
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
        if (ActionTextMode)
            Source.clip = ActionSfx;
        else
            Source.clip = AltActionSfx;

        Source.Play();
        InteractAction?.Invoke();
    }

    public void TriggerLook()
    {
        VisibilityAction?.Invoke();
        LookAction?.Invoke();
    }

    public virtual void Grab()
    {

    }
}
