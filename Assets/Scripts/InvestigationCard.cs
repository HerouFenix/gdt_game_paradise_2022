using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[CreateAssetMenu(fileName = "Investigation Card", menuName = "ScriptableObjects/Investigation Card", order = 1)]
public class InvestigationCard : ScriptableObject
{
    private bool onDeck;
    private bool onReserve;
    private bool onHand;

    enum Type { Number, Suit, Color, DoubleD, Even };
    enum Suit { Work, Love, Money, Fame, None };
    enum Color { Red, White, Black, None };

    [SerializeField] private Type _type;
    [SerializeField] private int _number;
    [SerializeField] private Suit _suit;
    [SerializeField] private Color _color;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
