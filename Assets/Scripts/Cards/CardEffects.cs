using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardEffects : MonoBehaviour
{
    private Phase1Manager _phase1Manager = Phase1Manager.Instance;

    [SerializeField] private SpriteRenderer _feedbackImage;
    [SerializeField] private Sprite[] _sprites;

    //Managers
    private AudioManager _audioManager;

    private void Start()
    {
        _audioManager = AudioManager.Instance;
    }

    public enum Sprites
    {
        Wrong = 0,
        Correct = 1,
        Up = 2,
        Down = 3
    }

    public void NumberCardEffect(InvestigationCardProperties invCard, ClientCard clientCard)
    {
        int invCardNumber = invCard.number;
        int clientNumber = clientCard.Number;

        if (invCardNumber < clientNumber)
        {
            ChangeImage(Sprites.Up);
        }
        else if (invCardNumber > clientNumber)
        {
            ChangeImage(Sprites.Down);
        }
        else
        {
            ChangeImage(Sprites.Correct);
            _audioManager.PlaySound(AudioManager.soundList.CorrectChoice);
        }
    }

    public void EvenCardEffect(ClientCard clientCard)
    {
        int clientNumber = clientCard.Number;

        if (clientNumber % 2 == 0)
        {
            ChangeImage(Sprites.Correct);
            _audioManager.PlaySound(AudioManager.soundList.CorrectChoice);
        }
        else
        {
            ChangeImage(Sprites.Wrong);
            _audioManager.PlaySound(AudioManager.soundList.WrongChoice);
        }
    }

    public void SuitCardEffect(InvestigationCardProperties invCard, ClientCard clientCard)
    {
        ESuit invCardSuit = invCard.suit;
        ESuit clienCardSuit = clientCard.Suit;

        if(invCardSuit == clienCardSuit)
        {
            ChangeImage(Sprites.Correct);
            _audioManager.PlaySound(AudioManager.soundList.CorrectChoice);
        }
        else
        {
            ChangeImage(Sprites.Wrong);
            _audioManager.PlaySound(AudioManager.soundList.WrongChoice);
        }
    }

    public void ColorCardEffect(InvestigationCardProperties invCard,  ClientCard clientCard)
    {
        EColor invCardColor = invCard.color;
        EColor clienCardColor = clientCard.Color;

        if (invCardColor == clienCardColor)
        {
            ChangeImage(Sprites.Correct);
            _audioManager.PlaySound(AudioManager.soundList.CorrectChoice);
        }
        else
        {
            ChangeImage(Sprites.Wrong);
            _audioManager.PlaySound(AudioManager.soundList.WrongChoice);
        }
    }

    public void DoubleDCardEffect(ClientCard clientCard)
    {
        int clientNumber = clientCard.Number;

        if (clientNumber > 9)
        {
            ChangeImage(Sprites.Correct);
            _audioManager.PlaySound(AudioManager.soundList.CorrectChoice);
        }
        else
        {
            ChangeImage(Sprites.Wrong);
            _audioManager.PlaySound(AudioManager.soundList.WrongChoice);
        }
    }

    public void ChangeImage(Sprites sprit)
    {
        var i = (int)sprit;
        _feedbackImage.sprite = _sprites[i];
    }

}
