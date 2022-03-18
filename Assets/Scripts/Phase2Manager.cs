using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Phase2Manager : MonoBehaviour
{
    [SerializeField] private List<GameObject> _clientSpecificCards;
    [SerializeField] private List<GameObject> _clientAgnosticCards;

    private List<GameObject> _currentToolCards = new List<GameObject>();

    [SerializeField] private Vector3 _spawnPosition = new Vector3(0, 0, 0);
    [SerializeField] public List<Vector3> cardPositions = new List<Vector3>();

    private Client client;
    public ClientCard _clientCard;
    private GameManager _GameManager;

    [HideInInspector] public List<GameObject> todaysClients = new List<GameObject>();

    //Actions
    public event Action PickCard;

    private bool _cardsDrawn = false;

    private List<int> currentResults;


    #region Singleton

    private static Phase2Manager _instance;
    public static Phase2Manager Instance { get { return _instance; } }
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

    public void Start()
    {
        _GameManager = GameManager.Instance;
        _GameManager.StartPhase2 += DrawToolCards;
        _GameManager.StartPhase2 += SetCurrentResults;
    }

    public void SetTodaysClients(List<GameObject> clients)
    {
        todaysClients = clients;
    }

    public void DrawToolCards(List<int> l)
    {
        StartCoroutine(DrawToolCardCor());
    }

    public void SetCurrentResults(List<int> results)
    {
        currentResults = results;
    }

    public void ChooseCurrentToolCards(int amount)
    {
        for (int i = 0; i < todaysClients.Count; i++)
        {
            Client thisClient = todaysClients[i].GetComponent<Client>();
            for (int j = 0; j < _clientSpecificCards.Count; j++)
            {
                if (_clientSpecificCards[j].GetComponent<ToolCards>().ClientID == thisClient.ClientID)
                {
                    _currentToolCards.Add(_clientSpecificCards[j]);
                    _clientSpecificCards.RemoveAt(j);
                    break;
                }
            }
        }

        /* Add random cards */
        for (int i = _currentToolCards.Count; i < amount; i++)
        {
            int cardIndex = UnityEngine.Random.Range(0, _clientAgnosticCards.Count);

            _currentToolCards.Add(_clientAgnosticCards[cardIndex]);
        }

        /* Shuffle */
        for (int i = 0; i < _currentToolCards.Count; i++)
        {
            GameObject temp = _currentToolCards[i];
            int randomIndex = UnityEngine.Random.Range(i, _currentToolCards.Count);
            _currentToolCards[i] = _currentToolCards[randomIndex];
            _currentToolCards[randomIndex] = temp;
        }
    }


    #region DrawCard

    private IEnumerator DrawToolCardCor()
    {
        WaitForSeconds wait = new WaitForSeconds(.85f);

        for (int i = 0; i < _currentToolCards.Count; i++)
        {
            ToolCards newCard = Instantiate(_currentToolCards[i], _spawnPosition, Quaternion.identity).GetComponent<ToolCards>();
            newCard.SetIndex(i, true);
            yield return wait;
        }
    }

    #endregion

    #region RemoveCard

    public void RemoveToolCard(GameObject card)
    {

    }

    public IEnumerator RemoveToolCardCor(GameObject card)
    {
        WaitForSeconds wait = new WaitForSeconds(.05f);

        yield return wait;
    }

    #endregion

    public IEnumerator HideCards()
    {
        WaitForSeconds wait = new WaitForSeconds(.08f);

        foreach (GameObject obj in this._currentToolCards)
        {
            InvestigationCard card = obj.GetComponent<InvestigationCard>();
            card.HideCard();

            yield return wait;
        }
    }

    public IEnumerator ShowCards()
    {
        WaitForSeconds wait = new WaitForSeconds(.08f);

        foreach (GameObject obj in this._currentToolCards)
        {
            InvestigationCard card = obj.GetComponent<InvestigationCard>();
            card.ShowCard();

            yield return wait;
        }
    }

}
