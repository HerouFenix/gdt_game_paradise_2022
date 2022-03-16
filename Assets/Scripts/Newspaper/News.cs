using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[CreateAssetMenu(fileName = "News", menuName = "ScriptableObjects/News", order = 2)]
public class News : ScriptableObject
{
    [SerializeField] private Sprite _sprite;
    [SerializeField] private string _text;

    public Sprite sprite{get => _sprite; set => _sprite = value; }
    public string text{get => _text; set => _text = value;}
}
