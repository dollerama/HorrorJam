using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;

public class MainUILogic : MonoBehaviour
{
    private PlayerLogicController _player;
    private UIDocument _document;
    public VisualTreeAsset SlotTemplate;

    private List<TemplateContainer> _slots;

    // Start is called before the first frame update
    void Start()
    {
        _slots = new List<TemplateContainer>();
        _player = Camera.main.GetComponent<PlayerLogicController>();
        _document = this.GetComponent<UIDocument>();
        _document.rootVisualElement.Q<Button>("BackBtn").clicked += () =>
        {
            _document.rootVisualElement.Q<VisualElement>("Inventory").visible = false;
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        };

        VisualElement rootV = _document.rootVisualElement.Q<VisualElement>("Inventory");
        for (int i = 0; i < 9; i++)
        {
            TemplateContainer temp = SlotTemplate.Instantiate();
            temp.Q<Label>("Name").text = "";
            _slots.Add(temp);
            rootV.Add(temp);
        }
    }

    public void AddItem(string Name, Sprite Spr)
    {

        for (int i = 0; i < 9; i++)
        {
            if (_slots[i].Q<Label>("Name").text == "")
            {
                _slots[i].Q<Label>("Name").text = Name;
                _slots[i].Q<VisualElement>("Icon").style.backgroundImage = new StyleBackground(Spr);
                _player.AddItem(Name);
                break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        _document.rootVisualElement.Q<Label>("Pickup").visible = (_player.CanPickUp) ? true : false;
        _document.rootVisualElement.Q<Label>("Pickup").text = _player.PickUpAction;

        if (_player.TriggerMenu())
        {
            UnityEngine.Cursor.lockState = CursorLockMode.None;
            _document.rootVisualElement.Q<VisualElement>("Inventory").visible = true;
        }
    }
}
