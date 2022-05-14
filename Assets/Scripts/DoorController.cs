using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    private Player.PlayerLogicController _player;
    public Mesh UnlockedMesh;
    public string UnlockID;
    private Interactable _interactable;
    private bool _locked;

    private void Start()
    {
        _locked = true;
        _player = Camera.main.GetComponent<Player.PlayerLogicController>();
        _interactable = this.GetComponent<Interactable>();
        _interactable.AddAction( () => TriggerAction() );
        _interactable.AddLook( () => { _interactable.SetActionTextMode(TryUnlock(_player.HoldingItem(UnlockID))); });
        _interactable.AddVisibility( () => { _interactable.SetVisible(_locked); });
        _interactable.FormatWithKeyWord(UnlockID);
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
