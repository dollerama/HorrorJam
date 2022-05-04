using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum PickupType
{
    Key
}

public class PickupBehaviour : MonoBehaviour
{
    private PickupI _pickup;
    private Interactable _interactable;

    public string Name;
    public PickupType _type;
    public Sprite Img;

    // Start is called before the first frame update
    void Start()
    {
        if(_type == PickupType.Key)
        {
            _pickup = new KeyPickup(this.gameObject, this.GetComponentInChildren<MeshRenderer>().gameObject, this.GetComponentInChildren<PickupVFXcontroller>().gameObject);
        }
        _interactable = this.GetComponent<Interactable>();
        _interactable.AddAction(() => TriggerPickup() );
    }

    public void TriggerPickup()
    {
        MainUILogic mUI = GameObject.FindGameObjectWithTag("MainUI").GetComponent<MainUILogic>();
        mUI.AddItem(Name, Img);
        _pickup.Grab();
    }
}
