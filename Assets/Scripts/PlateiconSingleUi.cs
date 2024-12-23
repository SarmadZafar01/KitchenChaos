using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlateiconSingleUi : MonoBehaviour
{

    [SerializeField] private Image image;


    public void SetKitchenObjectSO(KitchenObjectSo kitchenObjectSO)
    {
        image.sprite = kitchenObjectSO.sprite;
    }
}
