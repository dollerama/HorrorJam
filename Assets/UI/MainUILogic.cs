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
    private List<System.Action> _slotsDetailData;
    private List<System.Action> _slotsHoldData;
    private PickupDetail HoldID;

    public string GetHoldingID() => HoldID.Name;
    public PickupDetail GetHoldingDetail() => HoldID;

    // Start is called before the first frame update
    void Start()
    {
        HoldID = new PickupDetail();
        HoldID.Name = "";

        _slots = new List<TemplateContainer>();
        _slotsDetailData = new List<System.Action>();
        _slotsHoldData = new List<System.Action>();
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
            temp.Q<VisualElement>("ButtonContainer").style.opacity = 0f;
            _slots.Add(temp);
            _slotsDetailData.Add(()=> { });
            _slotsHoldData.Add(() => { });
            rootV.Add(temp);
        }

        _defaultSB = _slots[0].Q<VisualElement>("Icon").style.backgroundImage;
    }

    public void AddItem(PickupDetail detail)
    {
        for (int i = 0; i < 9; i++)
        {
            if (_slots[i].Q<Label>("Name").text == "")
            {
                //add item to players log
                _player.AddItem(detail.Name);

                //set UI
                _slots[i].Q<Label>("Name").text = detail.Name;
                _slots[i].Q<VisualElement>("Icon").style.backgroundImage = new StyleBackground(detail.Img);
                _slots[i].Q<VisualElement>("ButtonContainer").style.opacity = 1f;

                //detail button action
                _slotsDetailData[i] = () =>
                {
                    _document.rootVisualElement.Q<VisualElement>("Inventory").visible = false;
                    _document.rootVisualElement.Q<VisualElement>("Description").visible = true;
                    _document.rootVisualElement.Q<VisualElement>("Description").Q<Label>("Title").text = detail.Name;
                    _document.rootVisualElement.Q<VisualElement>("Description").Q<Label>("Content").text = detail.Details;
                };

                _slotsHoldData[i] = () =>
                {
                    if (_slots[i].Q<Button>("Hold").style.opacity != 0f)
                    {
                        VisualElement CornerSlot = _document.rootVisualElement.Q<VisualElement>("CornerSlot");
                        CornerSlot.Q<VisualElement>("Icon").style.backgroundImage = new StyleBackground(detail.Img);
                        CornerSlot.Q<Label>("Name").text = detail.Name;
                        _slots[i].Q<Button>("Hold").style.opacity = 0f;
                        HoldID = detail;
                    }
                };

                //register callbacks
                _slots[i].Q<Button>("Details").clicked += _slotsDetailData[i];
                _slots[i].Q<Button>("Hold").clicked += _slotsHoldData[i];
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
                //update GUI
                _slots[i].Q<Label>("Name").text = "";
                _slots[i].Q<VisualElement>("Icon").style.backgroundImage = _defaultSB;

                VisualElement CornerSlot = _document.rootVisualElement.Q<VisualElement>("CornerSlot");

                if (HoldID.Name == CornerSlot.Q<Label>("Name").text)
                {
                    HoldID = new PickupDetail();
                }

                CornerSlot.Q<VisualElement>("Icon").style.backgroundImage = null;
                CornerSlot.Q<Label>("Name").text = "";
                _slots[i].Q<Button>("Hold").style.opacity = 1f;
                _slots[i].Q<VisualElement>("ButtonContainer").style.opacity = 0f;

                //deregister callbacks :thumbs-up
                _slots[i].Q<Button>("Details").clicked -= _slotsDetailData[i];
                _slots[i].Q<Button>("Hold").clicked -= _slotsHoldData[i];

                //remove item from player log
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
