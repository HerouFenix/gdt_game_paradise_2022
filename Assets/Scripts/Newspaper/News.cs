using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[CreateAssetMenu(fileName = "News", menuName = "ScriptableObjects/News", order = 2)]
public class News : ScriptableObject
{
    [SerializeField] private Image _image;
    [SerializeField] private string _text;

    public Image image{get => _image; set => _image = value; }
    public string text{get => _text; set => _text = value;}
}
