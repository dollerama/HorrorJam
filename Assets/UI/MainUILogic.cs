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

    private StyleBackground _defaultSB;
    private List<TemplateContainer> _slots;
    private List<System.Action> _slotsData;

    // Start is called before the first frame update
    void Start()
    {
        _slots = new List<TemplateContainer>();
        _slotsData = new List<System.Action>();
        _player = Camera.main.GetComponent<PlayerLogicController>();
        _document = this.GetComponent<UIDocument>();
        _document.rootVisualElement.Q<Button>("InventoryBackBtn").clicked += () =>
        {
            _document.rootVisualElement.Q<VisualElement>("Inventory").visible = false;
            _document.rootVisualElement.Q<VisualElement>("Reticule").visible = true;
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        };

        _document.rootVisualElement.Q<Button>("DescriptionBackBtn").clicked += () =>
        {
            _document.rootVisualElement.Q<VisualElement>("Inventory").visible = true;
            _document.rootVisualElement.Q<VisualElement>("Description").visible = false;
        };

        VisualElement rootV = _document.rootVisualElement.Q<VisualElement>("Inventory");
        for (int i = 0; i < 9; i++)
        {
            TemplateContainer temp = SlotTemplate.Instantiate();
            temp.Q<Label>("Name").text = "";
            _slots.Add(temp);
            _slotsData.Add(()=> { });
            rootV.Add(temp);
        }

        _defaultSB = _slots[0].Q<VisualElement>("Icon").style.backgroundImage;
    }

    public void AddItem(string Name, string Details, Sprite Spr)
    {
        for (int i = 0; i < 9; i++)
        {
            if (_slots[i].Q<Label>("Name").text == "")
            {
                _slots[i].Q<Label>("Name").text = Name;
                _slots[i].Q<VisualElement>("Icon").style.backgroundImage = new StyleBackground(Spr);
                _player.AddItem(Name);
                _slotsData[i] = () =>
                {
                    _document.rootVisualElement.Q<VisualElement>("Inventory").visible = false;
                    _document.rootVisualElement.Q<VisualElement>("Description").visible = true;
                    _document.rootVisualElement.Q<VisualElement>("Description").Q<Label>("Title").text = Name;
                    _document.rootVisualElement.Q<VisualElement>("Description").Q<Label>("Content").text = Details;
                };

                _slots[i].Q<Button>("Details").clicked += _slotsData[i];

                break;
            }
        }
    }

    public void RemoveItem(string Name)
    { 
        for (int i = 0; i < 9; i++)
        {
            if (_slots[i].Q<Label>("Name").text == Name)
            {
                _slots[i].Q<Label>("Name").text = "";
                _slots[i].Q<VisualElement>("Icon").style.backgroundImage = _defaultSB;
                _slots[i].Q<Button>("Details").clicked -= _slotsData[i];

                _player.RemoveItem(Name);
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
            _document.rootVisualElement.Q<VisualElement>("Reticule").visible = false;
        }
    }
}
