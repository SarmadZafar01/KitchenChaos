using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateKitchenObject : KitchenObject


{

    public event EventHandler<OnIngredientAddedEventArgs> OnIngredientAdded;
    public class OnIngredientAddedEventArgs : EventArgs
    {
        public KitchenObjectSo KitchenObjectSO;
    }


    [SerializeField] private List<KitchenObjectSo> validKitchenObjectSOList;


    private List<KitchenObjectSo> kitchenObjectSOList;


    private void Awake()
    {
        kitchenObjectSOList = new List<KitchenObjectSo>();
    }

    public bool TryAddIngredient(KitchenObjectSo kitchenObjectSO)
    {
        if (!validKitchenObjectSOList.Contains(kitchenObjectSO))
        {
            // Not a valid ingredient
            return false;
        }
        if (kitchenObjectSOList.Contains(kitchenObjectSO))
        {
            // Already has this type
            return false;
        }
        else
        {
            kitchenObjectSOList.Add(kitchenObjectSO);

            OnIngredientAdded?.Invoke(this, new OnIngredientAddedEventArgs
            {
                KitchenObjectSO = kitchenObjectSO
            });
            return true;
        }
    }

    public List<KitchenObjectSo> GetKitchenObjectSOList()
    {
        return kitchenObjectSOList;
    }
}
