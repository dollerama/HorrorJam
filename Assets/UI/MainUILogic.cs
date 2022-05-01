using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MainUILogic : MonoBehaviour
{
    private PlayerLogicController _player;
    private UIDocument _document;
    // Start is called before the first frame update
    void Start()
    {
        _player = Camera.main.GetComponent<PlayerLogicController>();
        _document = this.GetComponent<UIDocument>();
    }

    // Update is called once per frame
    void Update()
    {
        _document.rootVisualElement.Q<Label>("Pickup").visible = (_player.CanPickUp) ? true : false;
    }
}
