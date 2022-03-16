using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CardManager : MonoBehaviour
{
    [SerializeField] private GameObject _baseInvestigationCard;

    //Tunes
    [SerializeField] private List<InvestigationCardProperties> _deckCards;
    private List<InvestigationCardProperties> _handCards = new List<InvestigationCardProperties>();
    private List<InvestigationCardProperties> _reserveCards = new List<InvestigationCardProperties>();
    private List<InvestigationCardProperties> _playedCards = new List<InvestigationCardProperties>();

    private List<GameObject> _currentInvestigationCards = new List<GameObject>();

    [SerializeField] private Vector3 _spawnPosition = new Vector3(0, 0, 0);
    [SerializeField] public List<Vector3> cardPositions = new List<Vector3>();

    //TempFix
    [SerializeField] public int clientNumber;

    public void Start()
    {
        DrawReserveCards();
    }

    //Vai buscar as cartas de investiga��o di�rias, para a m�o e para a reserva
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


    /* Investigation Card Methods */
    public void DrawInvestigationCard(int amount)
    {
        StartCoroutine(DrawInvestigationCardCor(amount));
    }

    private IEnumerator DrawInvestigationCardCor(int amount)
    {
        WaitForSeconds wait = new WaitForSeconds(.85f);

        if (amount > _reserveCards.Count)
        {
            amount = _reserveCards.Count;
        }

        for (int i = 0; i < amount; i++)
        {
            DrawInvestigationCard();
            yield return wait;
        }


    }

    public void DrawInvestigationCard()
    {
        int randomCard = UnityEngine.Random.Range(0, _reserveCards.Count);

        _baseInvestigationCard.GetComponent<InvestigationCard>()._invCard = _reserveCards[randomCard];

        GameObject newCard = Instantiate(_baseInvestigationCard, _spawnPosition, Quaternion.identity);

        _handCards.Add(newCard.GetComponent<InvestigationCard>()._invCard);
        _reserveCards.Remove(newCard.GetComponent<InvestigationCard>()._invCard);

        InvestigationCard newCardScript = newCard.GetComponent<InvestigationCard>();

        newCardScript.SetIndex(_currentInvestigationCards.Count, true);
        newCardScript.Played += RemoveInvestigationCard;

        _currentInvestigationCards.Add(newCard);
    }

    public void RemoveInvestigationCard(GameObject card)
    {
        InvestigationCardProperties _card = card.GetComponent<InvestigationCard>()._invCard;
        ApplyEffect(_card);
        _playedCards.Add(_card);
        _handCards.Remove(_card);
        StartCoroutine(RemoveInvestigationCardCor(card));
    }

    public IEnumerator RemoveInvestigationCardCor(GameObject card)
    {
        int cardIndex = card.GetComponent<InvestigationCard>().GetIndex();
        this._currentInvestigationCards.Remove(card);


        WaitForSeconds wait = new WaitForSeconds(.05f);
        for (int i = cardIndex; i < this._currentInvestigationCards.Count; i++)
        {
            InvestigationCard clientCard = this._currentInvestigationCards[i].GetComponent<InvestigationCard>();

            clientCard.SetIndex(i);
            clientCard.MoveCard(this.cardPositions[i]);
            yield return wait;
        }

        Destroy(card);
    }

    public void ApplyEffect(InvestigationCardProperties card)
    {
        if (card.type == InvestigationCardProperties.Type.Number)
        {
            if (card.number < clientNumber)
            {
                Debug.Log("Numero Abaixo");
            }
            else if (card.number > clientNumber)
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
