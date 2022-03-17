using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Investigation Card", menuName = "ScriptableObjects/Investigation Card", order = 1)]
public class InvestigationCardProperties : ScriptableObject
{
    private bool onDeck;
    private bool onReserve;
    private bool onHand;

    public enum Type { Number, Suit, Color, DoubleD, Even };
    [SerializeField] private Type _type;
    [SerializeField] private int _number;
    [SerializeField] private Sprite _image;

    [SerializeField] private ESuit _suit;
    [SerializeField] private EColor _color;

    public int number { get => _number; set => _number = value; }

    public Type type { get => _type; set => _type = value; }

    public ESuit suit { get => _suit; set => _suit = value; }

    public EColor color { get => _color; set => _color = value; }

    public Sprite image { get => _image; set => _image = value; }
}
