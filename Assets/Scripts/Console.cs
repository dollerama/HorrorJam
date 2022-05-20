using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Console : Interactable
{
    private bool _hasSymbol;
    [SerializeField] Renderer _renderer;
    private Material _mat;
    private Player.PlayerLogicController _player;

    public PickupDetail Detail;
    [ColorUsage(true, true)]
    public Color Emissive;
    public Texture2D Symbol;
    public string SymbolID;
    public string SymbolIDKey;

    public UnityEvent onKeyAdded;
    public UnityEvent onKeyRemoved;

    private float _timer;
    private ParticleSystem _particles;

    private void Awake()
    {
        Init();
        _player = Camera.main.GetComponent<Player.PlayerLogicController>();
        _particles = GetComponent<ParticleSystem>();
    }

    public override void Init()
    {
        base.Init();
    }

    void Start()
    {
        _mat = _renderer.material;
        _timer = 0;
        _hasSymbol = true;

        if(SymbolID == "")
        {
            _hasSymbol = false;
        }
        else
        {
            _mat.SetTexture("_Icon", Symbol);
            FormatWithKeyWord(SymbolID);
            Detail.Process(Symbol);
        }

        _mat.SetColor("_Emissive", Emissive);

        AddAction(Action);
        AddLook(() => { SetActionTextMode(!_hasSymbol); });
        AddVisibility(() =>
        {
            SetVisible((Player.MainUILogic.Instance.GetHoldingDetail().Name != "" && !_hasSymbol) || _hasSymbol);
        });
    }

    private void DisableSymbol()
    {
        _hasSymbol = false;
        _mat.SetTexture("_Icon", null);
    }

    public void SetDetail(PickupDetail d) => Detail = d;

    private void Action()
    {
        if (_timer > 0) return;
        _timer = 0.25f;

        if (_hasSymbol)
        {
            DisableSymbol();
            onKeyRemoved?.Invoke();
            Player.MainUILogic.Instance.AddItem(Detail);
        }
        else if(!_hasSymbol && Player.MainUILogic.Instance.GetHoldingDetail().Name != "")
        {
            SetDetail(Player.MainUILogic.Instance.GetHoldingDetail());

            _hasSymbol = true;
            SymbolID = Detail.Name;
            _mat.SetTexture("_Icon", Detail.GetImg());
            Player.MainUILogic.Instance.RemoveItem(Detail.Name);
            FormatWithKeyWord(SymbolID);
            _particles.Play();

            if(SymbolID == SymbolIDKey)
            {
                onKeyAdded?.Invoke();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        _timer -= Time.deltaTime;
    }
}
