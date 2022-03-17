using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientCard : MonoBehaviour
{
    enum Suit { None, Work, Love, Money, Fame };
    enum Color { None, Red, White, Black};

    [SerializeField] private int _number;
    public int Number { get => _number; }
    [SerializeField] private Suit _suit;
    [SerializeField] private Color _color;

    private int _currentNumber = 0;
    private Suit _currentSuit = Suit.None;
    private Color _currentColor = Color.None;

    private SpriteRenderer _numberSprite;
    private SpriteRenderer _suitSprite;
    private SpriteRenderer _colorSprite;

    private void Start()
    {
        _numberSprite = this.gameObject.transform.Find("number").GetComponent<SpriteRenderer>();
        _suitSprite = this.gameObject.transform.Find("suit").GetComponent<SpriteRenderer>();
        _colorSprite = this.gameObject.transform.Find("color").GetComponent<SpriteRenderer>();

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
            case Suit.None:
                _currentSuit = Suit.Work;
                break;
            case Suit.Work:
                _currentSuit = Suit.Love;
                break;
            case Suit.Love:
                _currentSuit = Suit.Money;
                break;
            case Suit.Money:
                _currentSuit = Suit.Fame;
                break;
            case Suit.Fame:
                _currentSuit = Suit.None;
                break;
        }

        _suitSprite.sprite = Resources.Load<Sprite>("ClientCard/Suits/" + _currentSuit);
    }

    public void ChangeCurrentColor()
    {
        switch (_currentColor)
        {
            case Color.None:
                _currentColor = Color.Red;
                break;
            case Color.Red:
                _currentColor = Color.White;
                break;
            case Color.White:
                _currentColor = Color.Black;
                break;
            case Color.Black:
                _currentColor = Color.None;
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
