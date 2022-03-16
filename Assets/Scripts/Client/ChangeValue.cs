using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeValue : MonoBehaviour
{
    enum Type { Number, Suit, Color };

    [SerializeField] private Type type;
    private bool _hovered = false;
    private ClientCard card;

    private void Start()
    {
        card = transform.parent.GetComponent<ClientCard>();
    }

    private void Update()
    {
        if (_hovered)
        {
            if (Input.GetMouseButtonDown(0))
                ChangeCurrent();
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

    private void ChangeCurrent()
    {
        switch (type)
        {
            case Type.Number:
                card.ChangeCurrentNumber();
                break;

            case Type.Color:
                card.ChangeCurrentColor();
                break;

            case Type.Suit:
                card.ChangeCurrentSuit();
                break;
        }
    }
}
