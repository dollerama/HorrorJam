using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForcefieldController : MonoBehaviour
{
    [SerializeField] Renderer _renderer;
    private Material _mat;
    public bool enabled;
    private float enabledVal;

    // Start is called before the first frame update
    void Start()
    {
        enabledVal = (!enabled) ? 0 : 1;
        _mat = _renderer.material;
    }

    // Update is called once per frame
    void Update()
    {
        if (!enabled)
            enabledVal = Mathf.Lerp(enabledVal, 0, Time.deltaTime * 2);
        else
            enabledVal = Mathf.Lerp(enabledVal, 1, Time.deltaTime * 2);
        _mat.SetFloat("_Enabled", enabledVal);
    }

    public void ToggleEnable(bool val)
    {
        GetComponent<BoxCollider>().isTrigger = !val;
        enabled = val;
    }
}
