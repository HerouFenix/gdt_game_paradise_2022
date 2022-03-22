using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class JournalManager : MonoBehaviour
{
    public List <News> Daily_list;

    private Dictionary<int, bool> DailyClientNews = new Dictionary<int, bool>();

    public List <News> Accidents_list;

    public List <News> Clients_list;

    public List <News> Death_list;

    public List <News> Final_News;

    public List <News> Backup;

    public List <News> FirstDay_list;

    public int currentWantedLevel;

    public bool WantedLevelChanged;

    public List <News> Cops_list;

    public Image News_image;

    private int day = 1;

    private GameManager _gameManager;

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

    #region Singleton

    private static JournalManager _instance;
    public static JournalManager Instance { get { return _instance; } }
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    #endregion

    void Start()
    {
        _gameManager = GameManager.Instance;
        _gameManager.SetJournalManager();
        _gameManager.EndDay += this.ResetManager;

        DailyNews(FirstDay_list, Backup, true);
    }

    void ChooseNews()
    {
        Final_News.Clear();
        Backup.Clear();
        Clients_list.Clear();

        foreach(KeyValuePair<int, bool> entry in DailyClientNews)
        {
            if (entry.Value)
            {
                Clients_list.Add(Death_list[entry.Key]);
            }

            else
            {
                Clients_list.Add(Accidents_list[entry.Key]);
            }
        }

        Clients();
        WBNews();
        DailyNews(Final_News, Backup, false);
    }

    void Clients()
    {
        int i;
        Final_News.Add(Clients_list[0]);

        
        for (i = 0; i < Clients_list.Count; i++)
        {
            
            if (Clients_list[i].weight > Final_News[0].weight && Clients_list[i] != Final_News[0])
            {
                Backup.Add(Final_News[0]);
                Final_News[0] = Clients_list[i];
            }

            else if (Clients_list[i] != Final_News[0])
                Backup.Add(Clients_list[i]);
        }
    }

    void WBNews()
    {
        Final_News.Add(Daily_list[day]);
    }

    void DailyNews(List<News> Final, List<News> Backup, bool custom)
    {
        int i;


        if (WantedLevelChanged && !custom)
            Final.Add(Cops_list[Mathf.Clamp(currentWantedLevel, 0, 5) /*- 1*/]);

        else if (!custom)
        {
            Final.Add(Backup[0]);

            for (i = 0; i < Backup.Count; i++)
            {
                if (Final[2].weight < Backup[i].weight)
                    Final[2] = Backup[i];
            }
        }

        News news1 = Final[0];
        News news2 = Final[1];
        News news3 = Final[2];

        if (!custom)
        {

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
        }

        News_image.sprite = news1.sprite;
        _News1_text.text = news1.text;
        _News2_text.text = news2.text;
        _News3_text.text = news3.text;

    }

    public void ReceivedClientNews(int clientId, bool killed, int increase)
    {
        DailyClientNews.Add(clientId, killed);


        if (!WantedLevelChanged)
        {
            WantedLevelChanged = increase > 0;
        }

        currentWantedLevel += increase;

    }

    void ResetManager()
    {
        ChooseNews();
        DailyClientNews = new Dictionary<int, bool>();
        day++;
        WantedLevelChanged = false;
    }

}
