using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static public class Utility
{
    static public bool IsLookingAtObject(Vector3 dir, Vector3 obj1, Vector3 obj2) => (Vector3.Dot(dir, (obj2-obj1).normalized) < 0) ? false : true;
}
