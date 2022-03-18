using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class JournalManager : MonoBehaviour
{
    public List <News> Daily_list;

    public List <News> DeadCharacters_list;

    public List <News> Final_News;

    public List <News> Backup;

    public int currentWantedLevel;

    public bool WantedLevelChanged;

    public List <News> Cops_list;

    public Image News_image;

    [SerializeField] private TextMeshProUGUI _News1_text;

    [SerializeField] private TextMeshProUGUI _News2_text;

    [SerializeField] private TextMeshProUGUI _News3_text;
    enum DeathNewsList
    {
        Postman = 0,

        Baker = 1,

        Politician = 2,

        Pirate = 3,

        OldLady = 4
    }

    private int WB_index;

    void Start()
    {
        Clients();
        WBNews();
        DailyNews(Final_News, Backup, true, 1);
    }

    void Clients()
    {
        int i;
        Final_News.Add(DeadCharacters_list[0]);

        
        for (i = 0; i < DeadCharacters_list.Count; i++)
        {
            
            if (DeadCharacters_list[i].weight > Final_News[0].weight && DeadCharacters_list[i] != Final_News[0])
            {
                Backup.Add(Final_News[0]);
                Final_News[0] = DeadCharacters_list[i];
            }

            else if (DeadCharacters_list[i] != Final_News[0])
                Backup.Add(DeadCharacters_list[i]);

        }
    }

    void WBNews()
    {
        int i;
        Final_News.Add(Daily_list[0]);

        
        for (i = 0; i < Daily_list.Count; i++)
        {
            
            if (Daily_list[i].weight > Final_News[1].weight && Daily_list[i] != Final_News[1])
            {
                Backup.Add(Final_News[1]);
                Final_News[1] = Daily_list[i];
            }

            else if (Daily_list[i] != Final_News[1])
                Backup.Add(Daily_list[i]);

        }
    }

    void DailyNews(List<News> Final, List<News> Backup,  bool WantedLevelChanged, int WantedLevel)
    {
        int i;

        if (WantedLevelChanged)
            Final.Add(Cops_list[WantedLevel - 1]);

        else
        {
            Final[3] = Backup[0];

            for (i = 0; i < Backup.Count; i++)
            {
                if (Final[3].weight < Backup[i].weight)
                    Final[3] = Backup[i];
            }
        }

        News news1 = Final[0];
        News news2 = Final[1];
        News news3 = Final[2];  

        for (i = 0; i < Final.Count; i++)
        {
            if (news1.weight <= Final[i].weight)
            {
                news1 = Final[i];
            }
        }

        Final.Remove(news1);

        for (i = 0; i < Final.Count; i++)
        {
            if (news2.weight <= Final[i].weight)
                news2 = Final[i];
        }

        Final.Remove(news2);

        news3 = Final[0];

        News_image.sprite = news1.sprite;
        _News1_text.text = news1.text;
        _News2_text.text = news2.text;
        _News3_text.text = news3.text;

    }
}
