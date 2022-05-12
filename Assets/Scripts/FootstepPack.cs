using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SFX/FootstepPack", fileName ="stepPackDefault")]
public class FootstepPack : ScriptableObject
{
    public string tag;
    public AudioClip[] clips;
}
