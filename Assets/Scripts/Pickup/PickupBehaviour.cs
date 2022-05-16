using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public struct PickupDetail
{
    public string Name;
    public string Details;
    public Sprite Img;

    public PickupDetail(string n, string d, Sprite i)
    {
        Name = n;
        Details = d;
        Img = i;
    }
}

public abstract class PickupBehaviour : Interactable
{
    //private PickupI _pickup;
    //public Interactable interact;
    public PickupDetail Detail;

    public override void Init()
    {
        base.Init();
        AddAction(Grab);
        FormatWithKeyWord(Detail.Name);
    }

    public override void Grab()
    {
        Player.MainUILogic.Instance.AddItem(Detail);
    }
}
