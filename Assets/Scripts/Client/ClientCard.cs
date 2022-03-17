using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientCard : MonoBehaviour
{
    private int _number;
    public int Number { get => _number; set => _number = value; }

    private ESuit _suit;
    private EColor _color;

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
    public void SetClientValues(int number, ESuit suit, EColor color)
    {
        this._number = number;
        this._suit = suit;
        this._color = color;

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
    public void RevealAll()
    {
        this.RevealNumber();
        this.RevealSuit();
        this.RevealColor();
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
