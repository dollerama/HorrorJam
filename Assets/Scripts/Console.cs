using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Console : Interactable
{
    public PickupDetail Detail;
    [ColorUsage(true, true)]
    public Color Emissive;
    private bool _hasSymbol;
    [SerializeField] Renderer _renderer;
    private Material _mat;
    private Player.PlayerLogicController _player;
    public Texture2D Symbol;
    public string SymbolID;
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
        _mat.SetTexture("_Icon", Symbol);

        _timer = 0;
        _hasSymbol = true;

        _mat.SetColor("_Emissive", Emissive);

        FormatWithKeyWord(SymbolID);

        AddAction(Action);
        AddLook(() => { SetActionTextMode(!_hasSymbol); });
        AddVisibility(() =>
        {
            SetVisible((Player.MainUILogic.Instance.GetHoldingDetail().Name != "" && !_hasSymbol) || _hasSymbol);
        });

        Detail.Img = Sprite.Create(Symbol, new Rect(Vector2.zero, new Vector2(Symbol.width, Symbol.height)), Vector2.zero);
    }

    public void SetDetail(PickupDetail d) => Detail = d;

    private void Action()
    {
        if (_timer > 0) return;
        _timer = 0.25f;

        if (_hasSymbol)
        {
            _hasSymbol = false;
            _mat.SetTexture("_Icon", null);
            Player.MainUILogic.Instance.AddItem(Detail);
        }
        else if(!_hasSymbol && Player.MainUILogic.Instance.GetHoldingDetail().Name != "")
        {
            SetDetail(Player.MainUILogic.Instance.GetHoldingDetail());

            _hasSymbol = true;
            SymbolID = Detail.Name;
            _mat.SetTexture("_Icon", Detail.Img.texture);
            Player.MainUILogic.Instance.RemoveItem(Detail.Name);
            FormatWithKeyWord(SymbolID);
            _particles.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        _timer -= Time.deltaTime;
    }
}
