using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Phase2Manager : MonoBehaviour
{
    [SerializeField] private List<GameObject> _clientSpecificCards;
    [SerializeField] private List<GameObject> _clientAgnosticCards;

    private List<GameObject> _currentToolCards = new List<GameObject>();
    private List<GameObject> _currentToolCardInstances = new List<GameObject>();


    [SerializeField] private Vector3 _spawnPosition = new Vector3(0, 0, 0);
    [SerializeField] public List<Vector3> cardPositions = new List<Vector3>();

    [HideInInspector] public Client client;
    public ClientCard _clientCard;

    //Managers
    private AudioManager _audioManager;
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
        _audioManager = AudioManager.Instance;

        _GameManager.StartPhase2 += DrawToolCards;
        _GameManager.StartPhase2 += SetCurrentResults;
        _GameManager.EndDay += ResetManager;
    }

    public void ResetManager()
    {
        StopAllCoroutines();
        _cardsDrawn = false;

        // Put Non-played agnostic cards back into the deck
        for(int i = 0; i < _currentToolCards.Count; i++)
        {
            if(_currentToolCards[i].GetComponent<ToolCards>().ClientID == -1)
            {
                _clientAgnosticCards.Add(_currentToolCards[i]);
            }

            Destroy(_currentToolCardInstances[i]);
        }

        _currentToolCards = new List<GameObject>();
        _currentToolCardInstances = new List<GameObject>();
    }

    public void SetTodaysClients(List<GameObject> clients)
    {
        todaysClients = clients;
    }

    public void DrawToolCards(List<int> l)
    {
        if (!_cardsDrawn)
        {
            StartCoroutine(DrawToolCardCor());
            _cardsDrawn = true;
        }
        else
        {
            StartCoroutine(ShowCards());
        }
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
            _clientAgnosticCards.RemoveAt(cardIndex); 
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
            newCard.Played += RemoveToolCard;
            //_currentToolCards[i] = newCard.gameObject;
            _currentToolCardInstances.Add(newCard.gameObject);
            yield return wait;
        }
    }

    #endregion

    #region RemoveCard

    public void RemoveToolCard(GameObject card)
    {
        _audioManager.PlaySound(AudioManager.soundList.CashRegister);
        StartCoroutine(RemoveToolCardCor(card));
    }

    public IEnumerator RemoveToolCardCor(GameObject card)
    {
        int cardIndex = card.GetComponent<ToolCards>().GetIndex();
        this._currentToolCards.RemoveAt(cardIndex);
        this._currentToolCardInstances.RemoveAt(cardIndex);

        WaitForSeconds wait = new WaitForSeconds(.05f);
        for (int i = cardIndex; i < this._currentToolCardInstances.Count; i++)
        {
            ToolCards curCard = this._currentToolCardInstances[i].GetComponent<ToolCards>();

            curCard.SetIndex(i);
            curCard.MoveCard(this.cardPositions[i]);

            curCard = this._currentToolCards[i].GetComponent<ToolCards>();
            curCard.SetIndex(i);

            yield return wait;
        }

        StartCoroutine(HideCards());
        ToolCards toolCard = card.GetComponent<ToolCards>();

        int soul = 0;
        int police = 0;

        if (toolCard.ClientID == client.ClientID)
        {// Right tool
            soul = client._soul_value;
        }
        else
        {
            int number = UnityEngine.Random.Range(0, 100);
            if (number <= toolCard.probabilityOfKilling)
            {
                soul = client._soul_value;
            }
            else
            {
                police = client._police_value;
            }
        }

        _GameManager.IncrementValues(soul, police);
        _GameManager.SwapPhase(0);
        Destroy(card);
    }

    #endregion

    public IEnumerator HideCards()
    {
        WaitForSeconds wait = new WaitForSeconds(.08f);

        foreach (GameObject obj in this._currentToolCardInstances)
        {
            ToolCards card = obj.GetComponent<ToolCards>();
            card.HideCard();

            yield return wait;
        }
    }

    public IEnumerator ShowCards()
    {
        WaitForSeconds wait = new WaitForSeconds(.08f);

        foreach (GameObject obj in this._currentToolCardInstances)
        {
            ToolCards card = obj.GetComponent<ToolCards>();
            card.ShowCard();

            yield return wait;
        }
    }

}
