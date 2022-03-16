using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CardManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> _possibleClientCards = new List<GameObject>();

    //Tunes
    [SerializeField] private List<InvestigationCard> _deckCards;
    private List<InvestigationCard> _handCards = new List<InvestigationCard>();
    private List<InvestigationCard> _reserveCards = new List<InvestigationCard>();
    private List<InvestigationCard> _playedCards = new List<InvestigationCard>();

    private List<GameObject> _currentClientCards = new List<GameObject>();
    private List<GameObject> _currentInteractionCards = new List<GameObject>();

    [SerializeField] private Vector3 _spawnPosition = new Vector3(0,0,0);
    [SerializeField] public List<Vector3> cardPositions = new List<Vector3>();

    //TempFix
    [SerializeField] public int clientNumber;

    public void Start()
    {
        DrawReserveCards();
    }

    //Vai buscar as cartas de investigação diárias, para a mão e para a reserva
    public void DrawReserveCards()
    {
        for (int i = 17; i > 0; i--)
        {
            int cardIndex = UnityEngine.Random.Range(0, _deckCards.Count);
            //Debug.Log(_deckCards[cardIndex].number);
            //Debug.Log("Saiu carta de reserva" + _deckCards[cardIndex].number);

            _reserveCards.Add(_deckCards[cardIndex]);
            _deckCards.Remove(_deckCards[cardIndex]);
            //Debug.Log("Restam" + _deckCards.Count + "cartas");
        }
    }


    /* Client Card Methods */
    public void DrawClientCards(int amount)
    {
        StartCoroutine(DrawClientCardsCor(amount));
    }

    private IEnumerator DrawClientCardsCor(int amount)
    {
        WaitForSeconds wait = new WaitForSeconds(.85f);

        if (amount < _reserveCards.Count)
        {
            for (int i = 0; i < amount; i++)
            {
                DrawClientCard();
                yield return wait;
            }
        }
        else
        {
            for (int i = 0; i < _reserveCards.Count; i++)
            {
                Debug.Log(i);
                DrawClientCard();
                yield return wait;
            }
        }
        

    }

    public void DrawClientCard()
    {
        int randomCard = UnityEngine.Random.Range(0, _reserveCards.Count);

        _possibleClientCards[0].GetComponent<IClientCard>()._invCard = _reserveCards[randomCard];

        GameObject newCard = Instantiate(_possibleClientCards[0], _spawnPosition, Quaternion.identity);

        _handCards.Add(newCard.GetComponent<IClientCard>()._invCard);
        _reserveCards.Remove(newCard.GetComponent<IClientCard>()._invCard);

        IClientCard newCardScript = newCard.GetComponent<IClientCard>();

        newCardScript.SetIndex(_currentClientCards.Count, true);
        newCardScript.Played += RemoveClientCard;

        _currentClientCards.Add(newCard);
    }

    public void RemoveClientCard(GameObject card)
    {
        InvestigationCard _card = card.GetComponent<IClientCard>()._invCard;
        ApplyEffect(_card);
        _playedCards.Add(_card);
        _handCards.Remove(_card);
        StartCoroutine(RemoveClientCardI(card));
    }

    public IEnumerator RemoveClientCardI(GameObject card)
    {
        int cardIndex = card.GetComponent<IClientCard>().GetIndex();
        this._currentClientCards.Remove(card);


        WaitForSeconds wait = new WaitForSeconds(.05f);
        for (int i = cardIndex; i < this._currentClientCards.Count; i++)
        {
            IClientCard clientCard = this._currentClientCards[i].GetComponent<IClientCard>();

            clientCard.SetIndex(i);
            clientCard.MoveCard(this.cardPositions[i]);
            yield return wait;
        }

        Destroy(card);
    }

    public void ApplyEffect(InvestigationCard card)
    {
        if(card.type == InvestigationCard.Type.Number)
        {
            if(card.number < clientNumber)
            {
                Debug.Log("Numero Abaixo");
            }
            else if(card.number > clientNumber)
            {
                Debug.Log("Numero Acima");
            }
            else
            {
                Debug.Log("Numero Correto");
            }
        }
    }

    #region Singleton

    private static CardManager _instance;

    public static CardManager Instance
    {
        get
        {
            if (_instance == null) _instance = FindObjectOfType<CardManager>();
            return _instance;
        }
    }

    #endregion

    /* ************************ */
}
