using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientCard : MonoBehaviour
{
    // TODO: Remove serialize field //
    [SerializeField] private int _number;
    [SerializeField] private ESuit _suit;
    [SerializeField] private EColor _color;
    public int Number { get => _number; set => _number = value; }

    public ESuit Suit { get => _suit; set => _suit = value; }

    public EColor Color { get => _color; set => _color = value; }

    private int _currentNumber = 0;
    private ESuit _currentSuit = ESuit.None;
    private EColor _currentColor = EColor.None;

    private SpriteRenderer _numberSprite;
    private SpriteRenderer _suitSprite;
    private SpriteRenderer _colorSprite;

    private void Start()
    {
        _numberSprite = this.gameObject.transform.Find("number").GetComponent<SpriteRenderer>();
        _suitSprite = this.gameObject.transform.Find("suit").GetComponent<SpriteRenderer>();
        _colorSprite = this.gameObject.transform.Find("color").GetComponent<SpriteRenderer>();

    }

    public void ResetClientCard()
    {
        /* Pick random values */
        this._number = Random.Range(0, 21);
        this._suit = (ESuit)Random.Range(0, 5);
        this._color = (EColor)Random.Range(0, 4);

        _currentNumber = 0;
        _currentSuit = ESuit.None;
        _currentColor = EColor.None;
    }

    /* Change Current Values */
    public void ChangeCurrentNumber()
    {
        _currentNumber += 1;
        _currentNumber = _currentNumber % 21;
        _numberSprite.sprite = Resources.Load<Sprite>("ClientCard/Numbers/" + _currentNumber);
    }

    public void ChangeCurrentSuit()
    {
        switch (_currentSuit)
        {
            case ESuit.None:
                _currentSuit = ESuit.Work;
                break;
            case ESuit.Work:
                _currentSuit = ESuit.Love;
                break;
            case ESuit.Love:
                _currentSuit = ESuit.Money;
                break;
            case ESuit.Money:
                _currentSuit = ESuit.Fame;
                break;
            case ESuit.Fame:
                _currentSuit = ESuit.None;
                break;
        }

        _suitSprite.sprite = Resources.Load<Sprite>("ClientCard/Suits/" + _currentSuit);
    }

    public void ChangeCurrentColor()
    {
        switch (_currentColor)
        {
            case EColor.None:
                _currentColor = EColor.Red;
                break;
            case EColor.Red:
                _currentColor = EColor.White;
                break;
            case EColor.White:
                _currentColor = EColor.Black;
                break;
            case EColor.Black:
                _currentColor = EColor.None;
                break;
        }

        _colorSprite.sprite = Resources.Load<Sprite>("ClientCard/Colors/" + _currentColor);
    }

    /* Reveal Real Values */
    public List<int> RevealAll()
    {
        var _list = new List<int> { 0, 0, 0 };

        if (_currentColor == _color)
        {
            Debug.Log("Acertou na cor");
            _list[0] = 1;
        }
        else
        {
            Debug.Log("Cor errada");
        }
        if (_currentSuit == _suit)
        {
            Debug.Log("Acertou no naipe");
            _list[1] = 1;
        }
        else
        {
            Debug.Log("Naipe errado");
        }

        int difNumber = Mathf.Abs(_currentNumber - _number);
        _list[2] = difNumber;

        Debug.Log("falhou por " + difNumber);
        return _list;

        /*
        this.RevealNumber();
        this.RevealSuit();
        this.RevealColor();
        */
    }

    public void RevealNumber()
    {
        _numberSprite.sprite = Resources.Load<Sprite>("ClientCard/Numbers/" + _number);
    }

    public void RevealSuit()
    {
        _suitSprite.sprite = Resources.Load<Sprite>("ClientCard/Suits/" + _suit);
    }

    public void RevealColor()
    {
        _colorSprite.sprite = Resources.Load<Sprite>("ClientCard/Colors/" + _color);
    }

}
