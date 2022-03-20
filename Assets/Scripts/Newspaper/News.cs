using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[CreateAssetMenu(fileName = "News", menuName = "ScriptableObjects/News", order = 2)]
public class News : ScriptableObject
{
    enum news_type {WB, Characters, Police};

    [SerializeField] private int _characterID;

    [SerializeField] private int _police;

    [SerializeField] private int _weight;

    [SerializeField] private news_type _type;
    [SerializeField] private Sprite _sprite;
    [SerializeField] private string _text;

    public Sprite sprite{get => _sprite; set => _sprite = value; }
    public string text{get => _text; set => _text = value;}
    public int weight{get => _weight; set => _weight = value;}
    public int id{get => _characterID; set => _characterID = value;}
}
