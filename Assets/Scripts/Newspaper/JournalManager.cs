using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class JournalManager : MonoBehaviour
{
    public List <News> WB_list;

    public List <News> Deaths_list;

    public List <News> Cops_list;

    public Image Deaths_image;

    [SerializeField] private TextMeshProUGUI _Deaths_text;

    [SerializeField] private TextMeshProUGUI _Cops_text;

    [SerializeField] private TextMeshProUGUI _WB_text;

    private int WB_index;

    void Start()
    {
        WorldBuilding();
        CharacterDeath();
        WantedLevel();
    }

    void WorldBuilding()
    {
        WB_index = Random.Range(0, WB_list.Count);

        _WB_text.text = WB_list[WB_index].text;
    }

    void CharacterDeath()
    {
        Deaths_image.sprite = Deaths_list[0].sprite;

        _Deaths_text.text = Deaths_list[0].text;
    }

    void WantedLevel()
    {
        _Cops_text.text = Cops_list[0].text;
    }
}
