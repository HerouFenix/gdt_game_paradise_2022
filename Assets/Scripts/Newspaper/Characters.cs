using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[CreateAssetMenu(fileName = "Characters", menuName = "ScriptableObjects/Characters", order = 2)]
public class Characters : ScriptableObject
{
    [SerializeField] private Sprite _sprite;

    [SerializeField] private string _death_text;

    [SerializeField] private int _soul_level;

    public Sprite sprite{get => _sprite; set => _sprite = value; }
    public string death_text{get => _death_text; set => _death_text = value;}

    public int soul_level{get => _soul_level; set => _soul_level = value;}
}
