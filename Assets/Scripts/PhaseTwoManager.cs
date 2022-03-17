using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseTwoManager : MonoBehaviour
{
    public List<ToolCards> allToolCards;

    private List<GameObject> _possibleToolCards = new List<GameObject>();
    private List<GameObject> _currentToolCards = new List<GameObject>();

    [SerializeField] private Vector3 _spawnPosition = new Vector3(0, 0, 0);
    [SerializeField] public List<Vector3> cardPositions = new List<Vector3>();

    [HideInInspector] public List<Client> daysClients;
    [HideInInspector] public Client client;

    [HideInInspector] public int phaseIndex = -1;

    private bool _cardsDrawn = false;

    private void Start()
    {
        this.DrawPossibleToolCards();
    }

    private void Update()
    {
        if (phaseIndex == 0)
        {
            if (_cardsDrawn)
                this.HideToolCards();

            return;
        }
        else if (phaseIndex == 1)
        {
            client.ToggleTextBubble();

            if (!_cardsDrawn)
                DrawToolCards(1);
            else
                ShowToolCards();

            phaseIndex = 1;

            client.ToggleShowClientCard();

            phaseIndex = 2;
        }
    }

    public void HideToolCards()
    {
        for (int i = 0; i < _currentToolCards.Count; i++)
        {
            _currentToolCards[i].SetActive(false);
        }
    }

    public void ShowToolCards()
    {
        for (int i = 0; i < _currentToolCards.Count; i++)
        {
            _currentToolCards[i].SetActive(true);
        }
    }

    //Build today's tool deck by getting each of the client's items + 3 others
    public void DrawPossibleToolCards()
    {
        for (int i = 0; i < daysClients.Count ; i++)
        {
            for(int j = 0; j < allToolCards.Count; j++)
            {
                if(allToolCards[j].ClientID == daysClients[i].ClientID)
                {
                    _possibleToolCards.Add(allToolCards[j].gameObject);
                    allToolCards.RemoveAt(j);
                    break;
                }
            }
        }


        for(int i = daysClients.Count; i < 1; i++)
        {
            int randomCard = UnityEngine.Random.Range(0, allToolCards.Count);
            _possibleToolCards.Add(allToolCards[randomCard].gameObject);
            allToolCards.RemoveAt(randomCard);
        }
    }

    /* Tool Card Methods */
    public void DrawToolCards(int amount)
    {
        _cardsDrawn = true;
        StartCoroutine(DrawToolCardCor(amount));
    }

    private IEnumerator DrawToolCardCor(int amount)
    {
        client.SetClickable(false);

        WaitForSeconds wait = new WaitForSeconds(.85f);

        for (int i = 0; i < amount; i++)
        {
            DrawToolCard();
            yield return wait;
        }

        client.SetClickable(true);
    }

    public void DrawToolCard()
    {
        int randomCard = UnityEngine.Random.Range(0, _possibleToolCards.Count);

        GameObject newCard = Instantiate(_possibleToolCards[randomCard], _spawnPosition, Quaternion.identity);

        newCard.GetComponent<ToolCards>().SetIndex(_currentToolCards.Count, true);

        _currentToolCards.Add(newCard);
        _possibleToolCards.RemoveAt(randomCard);
    }


    #region Singleton

    private static PhaseTwoManager _instance;

    public static PhaseTwoManager Instance
    {
        get
        {
            if (_instance == null) _instance = FindObjectOfType<PhaseTwoManager>();
            return _instance;
        }
    }

    #endregion

}
