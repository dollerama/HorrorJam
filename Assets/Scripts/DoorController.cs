using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : Interactable
{
    private Player.PlayerLogicController _player;
    public Mesh UnlockedMesh;
    public string UnlockID;
    private bool _locked;

    private void Start()
    {
        Init();
        _locked = true;
        _player = Camera.main.GetComponent<Player.PlayerLogicController>();
        AddAction( () => TriggerAction() );
        AddLook( () => { SetActionTextMode(TryUnlock(_player.HoldingItem(UnlockID))); });
        AddVisibility( () => { SetVisible(_locked); });
        FormatWithKeyWord(UnlockID);
    }

    private void TriggerAction()
    {
        if (_player.HoldingItem(UnlockID))
        {
            Player.MainUILogic.Instance.RemoveItem(Unlock(_player.HoldingItem(UnlockID)));
        }
    }

    public string Unlock(bool k)
    {
        if (!k || !_locked) return "";
        this.GetComponent<MeshFilter>().mesh = UnlockedMesh;
        this.GetComponent<MeshCollider>().sharedMesh = UnlockedMesh;
        _locked = false;
        return UnlockID;
    }

    public bool TryUnlock(bool item) => item && _locked;
}
