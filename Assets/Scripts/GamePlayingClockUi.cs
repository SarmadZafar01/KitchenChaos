using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayingClockUi : MonoBehaviour
{
    [SerializeField] private Image timerImage;


    private void Update()
    {
        timerImage.fillAmount = KitchenGamemanger.Instance.GetGamePlayingTimerNormalized();
    }
}



