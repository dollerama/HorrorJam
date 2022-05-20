using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class PickupDetail
{
    public string Name;
    public string Details;
    public Sprite Img;
    public Texture2D ImgTex;

    private bool processed;
    public PickupDetail(string n, string d, Sprite i)
    {
        Name = n;
        Details = d;
        Img = i;
    }

    public PickupDetail(string n)
    {
        Name = n;
    }

    public void Process(Texture2D i = null)
    {
        processed = true;

        if (i == null)
        {
            Img = Sprite.Create(ImgTex, new Rect(Vector2.zero, new Vector2(ImgTex.width, ImgTex.height)), Vector2.zero);
        }
        else
        {
            Img = Sprite.Create(i, new Rect(Vector2.zero, new Vector2(i.width, i.height)), Vector2.zero);
        }
    }

    public Texture2D GetImg()
    {
        return Img.texture;
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
        Detail.Process();
        AddAction(Grab);
        FormatWithKeyWord(Detail.Name);
    }

    public override void Grab()
    {
        Player.MainUILogic.Instance.AddItem(Detail);
    }
}
