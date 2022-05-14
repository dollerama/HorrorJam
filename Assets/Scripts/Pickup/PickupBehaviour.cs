using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum PickupType
{
    Key
}

[System.Serializable]
public struct PickupDetail
{
    public string Name;
    public string Details;
    public PickupType Type;
    public Sprite Img;

    public PickupDetail(string n, string d, PickupType t, Sprite i)
    {
        Name = n;
        Details = d;
        Type = t;
        Img = i;
    }
}

public class PickupBehaviour : MonoBehaviour
{
    private PickupI _pickup;
    private Interactable _interactable;

    public PickupDetail Detail;

    // Start is called before the first frame update
    void Awake()
    {
        if(Detail.Type == PickupType.Key)
        {
            _pickup = new KeyPickup(
                gameObject, 
                GetComponentInChildren<MeshRenderer>().gameObject, 
                GetComponentInChildren<PickupVFXcontroller>().gameObject
            );
        }
        _interactable = this.GetComponent<Interactable>();
        _interactable.AddAction(TriggerPickup);
        _interactable.FormatWithKeyWord(Detail.Name);
    }

    public void TriggerPickup()
    {
        Player.MainUILogic.Instance.AddItem(Detail);
        _pickup.Grab();
    }
}
