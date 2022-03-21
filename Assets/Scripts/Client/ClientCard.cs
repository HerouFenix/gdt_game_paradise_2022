using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    private int _currentNumber = 1;
    private ESuit _currentSuit = ESuit.Work;
    private EColor _currentColor = EColor.Black;

    [SerializeField] private SpriteRenderer _numberSprite;
    [SerializeField] private SpriteRenderer _suitSprite;
    [SerializeField] private SpriteRenderer _colorSprite;

    [SerializeField] private GameObject results;

    private void Start()
    {
        /*_numberSprite = this.gameObject.transform.Find("number").GetComponent<SpriteRenderer>();
        _suitSprite = this.gameObject.transform.Find("suit").GetComponent<SpriteRenderer>();
        _colorSprite = this.gameObject.transform.Find("color").GetComponent<SpriteRenderer>();*/

    }

    private void OnEnable()
    {
        //gameObject.GetComponent<Animator>().SetTrigger();
    }

    public void ResetClientCard()
    {
        /* Pick random values */
        this._number = Random.Range(1, 21);
        this._suit = (ESuit)Random.Range(0, 4);
        this._color = (EColor)Random.Range(0, 3);

        _currentNumber = 1;
        _numberSprite.sprite = Resources.Load<Sprite>("ClientCard/Numbers/" + _currentNumber);
        _currentSuit = ESuit.Work;
        _suitSprite.sprite = Resources.Load<Sprite>("ClientCard/Suits/" + _currentSuit);
        _currentColor = EColor.Black;
        _colorSprite.sprite = Resources.Load<Sprite>("ClientCard/Colors/" + _currentColor);
    }

    /* Change Current Values */
    public void ChangeCurrentNumber()
    {
        _currentNumber += 1;
        _currentNumber = _currentNumber % 21;
        if (_currentNumber == 0)
            _currentNumber++;

        _numberSprite.sprite = Resources.Load<Sprite>("ClientCard/Numbers/" + _currentNumber);
    }

    public void ChangeCurrentSuit()
    {
        switch (_currentSuit)
        {
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
                _currentSuit = ESuit.Work;
                break;
        }

        _suitSprite.sprite = Resources.Load<Sprite>("ClientCard/Suits/" + _currentSuit);
    }

    public void ChangeCurrentColor()
    {
        switch (_currentColor)
        {
            case EColor.Red:
                _currentColor = EColor.White;
                break;
            case EColor.White:
                _currentColor = EColor.Black;
                break;
            case EColor.Black:
                _currentColor = EColor.Red;
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
            _list[0] = 1;
        }
        if (_currentSuit == _suit)
        {
            _list[1] = 1;
        }

        int difNumber = Mathf.Abs(_currentNumber - _number);
        _list[2] = difNumber;

        return _list;
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

    public void RevealResults(List<string> hints, int police, int souls)
    {
        foreach (Transform child in results.transform)
        {
            switch (child.name)
            {
                case "hint1":
                    if(hints.Count > 0)
                    {
                        child.gameObject.SetActive(true);
                        child.GetComponent<TextMeshPro>().text = hints[0];
                    }
                    else
                    {
                        child.gameObject.SetActive(false);
                    }
                    break;
                case "hint1_hidden":
                    if (hints.Count == 0)
                    {
                        child.gameObject.SetActive(true);
                    }
                    else
                    {
                        child.gameObject.SetActive(false);
                    }
                    break;
                case "hint2":
                    if (hints.Count > 1)
                    {
                        child.gameObject.SetActive(true);
                        child.GetComponent<TextMeshPro>().text = hints[1];
                    }
                    else
                    {
                        child.gameObject.SetActive(false);
                    }
                    break;
                case "hint2_hidden":
                    if (hints.Count <= 1)
                    {
                        child.gameObject.SetActive(true);
                    }
                    else
                    {
                        child.gameObject.SetActive(false);
                    }
                    break;
                case "hint3":
                    if (hints.Count > 2)
                    {
                        child.gameObject.SetActive(true);
                        child.GetComponent<TextMeshPro>().text = hints[2];
                    }
                    else
                    {
                        child.gameObject.SetActive(false);
                    }
                    break;
                case "hint3_hidden":
                    if (hints.Count <= 2)
                    {
                        child.gameObject.SetActive(true);
                    }
                    else
                    {
                        child.gameObject.SetActive(false);
                    }
                    break;

                case "souls":
                    if (souls != -1)
                    {
                        child.gameObject.SetActive(true);
                        child.GetComponent<TextMeshPro>().text = "Souls: " + souls;
                    }
                    else
                    {
                        child.gameObject.SetActive(false);
                    }
                    break;
                case "souls_hidden":
                    if (souls == -1)
                    {
                        child.gameObject.SetActive(true);
                    }
                    else
                    {
                        child.gameObject.SetActive(false);
                    }
                    break;

                case "police":
                    if (police != -1)
                    {
                        child.gameObject.SetActive(true);
                        child.GetComponent<TextMeshPro>().text = "Police: " + police;
                    }
                    else
                    {
                        child.gameObject.SetActive(false);
                    }
                    break;
                case "police_hidden":
                    if (police == -1)
                    {
                        child.gameObject.SetActive(true);
                    }
                    else
                    {
                        child.gameObject.SetActive(false);
                    }
                    break;
            }
        }
    }

}
