using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardEffects : MonoBehaviour
{
    private Phase1Manager _phase1Manager = Phase1Manager.Instance;

    public int NumberCardEffect(InvestigationCardProperties invCard, ClientCard clientCard)
    {
        int invCardNumber = invCard.number;
        int clientNumber = clientCard.Number;

        if (invCardNumber < clientNumber)
        {
            Debug.Log("Numero Abaixo");
            return -1;
        }
        else if (invCardNumber > clientNumber)
        {
            Debug.Log("Numero Acima");
            return 1;
        }
        else
        {
            Debug.Log("Numero Correto");
            return 0;
        }
    }

    public int EvenCardEffect(ClientCard clientCard)
    {
        int clientNumber = clientCard.Number;

        if (clientNumber % 2 == 0)
        {
            Debug.Log("É par");
            return -1;
        }
        else
        {
            Debug.Log("Não é par");
            return 0;
        }
    }

    public void SuitCardEffect(InvestigationCardProperties invCard, ClientCard clientCard)
    {
        ESuit invCardSuit = invCard.suit;
        ESuit clienCardSuit = clientCard.Suit;

        if(invCardSuit == clienCardSuit)
        {
            Debug.Log("Naipe Correto");
        }
        else
        {
            Debug.Log("Naipe Errado");
        }
    }

    public void ColorCardEffect(InvestigationCardProperties invCard,  ClientCard clientCard)
    {
        EColor invCardColor = invCard.color;
        EColor clienCardColor = clientCard.Color;

        if (invCardColor == clienCardColor)
        {
            Debug.Log("Cor Correta");
        }
        else
        {
            Debug.Log("Cor Errada");
        }
    }

    public void DoubleDCardEffect(ClientCard clientCard)
    {

    }

}
