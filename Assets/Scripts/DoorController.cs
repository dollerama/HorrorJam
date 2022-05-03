using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public Mesh UnlockedMesh;
    public string UnlockID;
    public void Unlock(List<string> items)
    {
        if (!items.Contains(UnlockID)) return;
        this.GetComponent<MeshFilter>().mesh = UnlockedMesh;
        this.GetComponent<MeshCollider>().sharedMesh = UnlockedMesh;
    }

    public bool TryUnlock(List<string> items) => items.Contains(UnlockID);
}
