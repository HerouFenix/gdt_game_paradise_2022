using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Client : MonoBehaviour
{
    [SerializeField] private ClientCard card;
    [SerializeField] private Sprite sprite;
    [SerializeField] private List<string> lines = new List<string>();

    [SerializeField] private GameObject _textBubble;
    [SerializeField] private TextMeshPro _textBubbleText;


    private bool _clickable;
    private bool _hovered;

    [HideInInspector] public DailyManager manager;

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
                switch (manager.GetPhase())
                {
                    case -1:
                        manager.SwapPhase(0);
                        break;
                    case 0:
                        manager.SwapPhase(1);
                        break;

                }
            }
        }
    }

    public void ToggleTextBubble()
    {
        _textBubble.SetActive(!_textBubble.activeSelf);
    }

    IEnumerator TypeSentence(string sentence)
    {
        _textBubbleText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            _textBubbleText.text += letter;
            yield return new WaitForSeconds(0.02f);
        }
    }

    private void OnMouseEnter()
    {
        _hovered = true;
    }
    private void OnMouseExit()
    {
        _hovered = false;
    }
}
