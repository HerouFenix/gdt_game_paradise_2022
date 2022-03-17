using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Client : MonoBehaviour
{
    public int ClientID = 0;
    [SerializeField] private List<string> _hints = new List<string>();
    [SerializeField] private int _police_value;
    [SerializeField] private int _soul_value;

    [SerializeField] private GameObject cardObject;
    private ClientCard clientCard;
    [SerializeField] private Sprite sprite;
    [SerializeField] private List<string> lines = new List<string>();

    [SerializeField] private GameObject _textBubble;
    [SerializeField] private TextMeshPro _textBubbleText;

    private bool _clickable;
    private bool _hovered;

    [HideInInspector] public GameManager manager;

    private bool _textRunning = false;

    private void Start()
    {
        _clickable = true;
        this.gameObject.GetComponent<SpriteRenderer>().sprite = sprite;

        StartCoroutine(TypeSentence("Hello!"));
    }

    private void Update()
    {
        /*if (_clickable && _hovered)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log(manager.GetPhase());
                switch (manager.GetPhase())
                {
                    case 0:
                        manager.SwapPhase(1);
                        break;
                    case 1:
                        manager.SwapPhase(2);
                        break;

                }
            }
            
        }*/
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

            switch (manager.GetPhase())
            {
                case 1:
                    StopAllCoroutines();
                    StartCoroutine(TypeSentence("Are you done with the reading?"));
                    break;
            }
        }
    }

    private void OnMouseExit()
    {
        _hovered = false;
        switch (manager.GetPhase())
        {
            case 1:
                if (_textBubble.activeSelf)
                    this.ToggleTextBubble();
                break;
        }
    }

    public void SetClickable(bool clickable)
    {
        this._clickable = clickable;
        if (!this._clickable)
            this._hovered = false;
    }
}
