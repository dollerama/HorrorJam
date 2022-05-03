using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public Mesh UnlockedMesh;
    public string UnlockID;
    public string Unlock(List<string> items)
    {
        if (!items.Contains(UnlockID)) return "";
        this.GetComponent<MeshFilter>().mesh = UnlockedMesh;
        this.GetComponent<MeshCollider>().sharedMesh = UnlockedMesh;
        return UnlockID;
    }

    public bool TryUnlock(List<string> items) => items.Contains(UnlockID);
}
