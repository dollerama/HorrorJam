using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Console : MonoBehaviour
{
    public PickupDetail Detail;
    [ColorUsage(true, true)]
    public Color Emissive;
    private Interactable _interactable;
    private bool _hasSymbol;
    private Material _mat;
    private PlayerLogicController _player;
    private MainUILogic _uiLogic;
    public Texture2D Symbol;
    public string SymbolID;
    private float _timer;
    private ParticleSystem _particles;

    // Start is called before the first frame update
    void Start()
    {
        _timer = 0;
        _uiLogic = GameObject.FindGameObjectWithTag("MainUI").GetComponent<MainUILogic>();
        _hasSymbol = true;
        Renderer[] _mats = this.GetComponentsInChildren<Renderer>();
        foreach(Renderer _r in _mats)
        {
            if(_r.transform.parent != transform.parent)
            {
                _mat = _r.material;
            }
        }

        _player = Camera.main.GetComponent<PlayerLogicController>();
        _particles = GetComponent<ParticleSystem>();

        _mat.SetTexture("_Icon", Symbol);
        _mat.SetColor("_Emissive", Emissive);

        _interactable = this.GetComponent<Interactable>();
        _interactable.FormatWithKeyWord(SymbolID);

        _interactable.AddAction(Action);
        _interactable.AddLook(() => { _interactable.SetActionTextMode(!_hasSymbol); });
        _interactable.AddVisibility(() => 
        {
            _interactable.SetVisible((_uiLogic.GetHoldingDetail().Name != "" && !_hasSymbol) || _hasSymbol);
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
            _uiLogic.AddItem(Detail);
        }
        else if(!_hasSymbol && _uiLogic.GetHoldingDetail().Name != "")
        {
            SetDetail(_uiLogic.GetHoldingDetail());

            _hasSymbol = true;
            _mat.SetTexture("_Icon", Detail.Img.texture);
            _uiLogic.RemoveItem(Detail.Name);

            _particles.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        _timer -= Time.deltaTime;
    }
}
