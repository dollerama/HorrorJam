using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static public class Utility
{
    static public bool IsLookingAtObject(Vector3 dir1, Vector3 dir2) => (Vector3.Dot(dir1, dir2) < 0) ? false : true;
}
