using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    public Material _thisMat;
    private float _Alpha;
    // Start is called before the first frame update
    void Start()
    {
        _Alpha = 1;
        _thisMat = this.GetComponent<Renderer>().materials[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (_thisMat)
        {
            _thisMat.SetFloat("_Alpha", _Alpha);
            _Alpha = Mathf.Lerp(_Alpha, 0, Time.deltaTime);
        }
    }
}
