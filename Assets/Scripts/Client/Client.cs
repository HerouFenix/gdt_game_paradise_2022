using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Client : MonoBehaviour
{
    public int ClientID = 0;
    public List<string> _hints = new List<string>();
    public int _police_value;
    public int _soul_value;

    private ClientCard clientCard;
    [SerializeField] private Sprite sprite;
    [SerializeField] private List<string> lines = new List<string>();

    [SerializeField] private GameObject _textBubble;
    [SerializeField] private TextMeshPro _textBubbleText;

    [HideInInspector] public bool _canGuess = false;

    private bool _clickable;
    private bool _hovered;

    [HideInInspector] public GameManager manager;
    [HideInInspector] public Phase1Manager manager1;
    [HideInInspector] public Phase2Manager manager2;


    private bool _textRunning = false;

    private void Start()
    {
        _clickable = true;
        this.gameObject.GetComponent<SpriteRenderer>().sprite = sprite;

        StartCoroutine(TypeSentence("Hello!"));
    }

    private void Update()
    {
        if (_clickable && _hovered)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (!_canGuess)
                {
                    manager1.DrawCards();
                    ToggleTextBubble();

                }
                else
                {
                    manager1.GuessCard();
                }

            }

        }
    }

    public void ToggleTextBubble()
    {
        _textBubble.SetActive(!_textBubble.activeSelf);
    }

    public void ToggleShowClientCard()
    {
        clientCard.gameObject.SetActive(!clientCard.gameObject.activeSelf);
    }

    IEnumerator TypeSentence(string sentence)
    {
        if (!_textBubble.activeSelf)
            this.ToggleTextBubble();

        _textBubbleText.text = "";

        _textRunning = true;
        foreach (char letter in sentence.ToCharArray())
        {
            _textBubbleText.text += letter;
            yield return new WaitForSeconds(0.02f);
        }
        _textRunning = false;
    }

    private void OnMouseEnter()
    {
        if (_clickable)
        {
            _hovered = true;

            if (_canGuess)
            {
                StopAllCoroutines();
                StartCoroutine(TypeSentence("Are you done with the reading?"));
            }
        }
    }

    private void OnMouseExit()
    {
        _hovered = false;

        if (_canGuess)
        {
            if (_textBubble.activeSelf)
                this.ToggleTextBubble();
        }

    }

    public void SetClickable(bool clickable)
    {
        this._clickable = clickable;
        if (!this._clickable)
            this._hovered = false;
    }
}
