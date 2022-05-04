using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    private PlayerLogicController _player;
    public Mesh UnlockedMesh;
    public string UnlockID;
    private Interactable _interactable;

    private void Start()
    {
        _player = Camera.main.GetComponent<PlayerLogicController>();
        _interactable = this.GetComponent<Interactable>();
        _interactable.AddAction( () => TriggerAction() );
        _interactable.AddLook( () => { _interactable.ActionTextMode = (TryUnlock(_player.GetItemsHeld())); });
    }

    private void TriggerAction()
    {
        MainUILogic mUI = GameObject.FindGameObjectWithTag("MainUI").GetComponent<MainUILogic>();
        mUI.RemoveItem( Unlock(_player.GetItemsHeld()) );
    }

    public string Unlock(List<string> items)
    {
        if (!items.Contains(UnlockID)) return "";
        this.GetComponent<MeshFilter>().mesh = UnlockedMesh;
        this.GetComponent<MeshCollider>().sharedMesh = UnlockedMesh;
        return UnlockID;
    }

    public bool TryUnlock(List<string> items) => items.Contains(UnlockID);
}
