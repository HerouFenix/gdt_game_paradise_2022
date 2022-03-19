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
    [SerializeField] private List<string> startLines = new List<string>();
    private int nextLine = 0;
    [SerializeField] private string endLine = "Goodbye";

    [SerializeField] private GameObject _textBubble;
    [SerializeField] private TextMeshPro _textBubbleText;

    [HideInInspector] public bool _canGuess = false;

    private bool _clickable;
    private bool _hovered;

    public GameObject outline;

    [HideInInspector] public GameManager manager;
    [HideInInspector] public Phase1Manager manager1;
    [HideInInspector] public Phase2Manager manager2;

    Animator _animator;


    private bool _textRunning = false;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _clickable = true;
        this.gameObject.GetComponent<SpriteRenderer>().sprite = sprite;
    }

    private void Update()
    {
        if (_clickable && _hovered)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if(nextLine < startLines.Count)
                {
                    StopCoroutine(TypeSentence(""));
                    StartCoroutine(TypeSentence(startLines[nextLine]));
                    nextLine++;
                }
                else if (!_canGuess && manager.GetPhase() == 1)
                {
                    manager1.DrawCards();
                    ToggleTextBubble();

                }
                else if(manager.GetPhase() == 1)
                {
                    manager1.GuessCard();
                    _clickable = false;
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

    IEnumerator TypeSentence(string sentence, float initialDelay = 0)
    {
        yield return new WaitForSeconds(initialDelay);

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

    public IEnumerator FadeAway()
    {
        string sentence = endLine;

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

        yield return new WaitForSeconds(1f);

        if(_animator == null)
            _animator = GetComponent<Animator>();

        _animator.SetTrigger("fadeAway");
    }

    private void DestroySelf()
    {
        Destroy(this.gameObject);
    }

    private void OnMouseEnter()
    {
        if (_clickable)
        {
            outline.SetActive(true);
            _hovered = true;

            if (_canGuess)
            {
                StopCoroutine(TypeSentence(""));
                StartCoroutine(TypeSentence("Are you done with the reading?"));
            }
        }
    }

    private void OnMouseExit()
    {
        _hovered = false;
        outline.SetActive(false);
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
        {
            this._hovered = false;
            outline.SetActive(false);
        }
    }
}
