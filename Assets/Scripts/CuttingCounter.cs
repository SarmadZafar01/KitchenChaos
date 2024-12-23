using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounter : BaseCounter, IHasProgress
{

    public static event EventHandler OnAnyCut;


    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    

    public event EventHandler OnCut;



    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSoArray;

    private int cuttingProgress;
    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            //player is carrying sonmething
            if (player.HasKitchenObject())
            {
                if (HasRecipWithInput(player.GetKitchenObject().GetKitchenObjectSO()))
                {
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                    cuttingProgress = 0;

                    CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalized= (float)cuttingProgress/ cuttingRecipeSO.cuttingProgressMax
                    });
                }

            }
            //player is not carrying someting
            else
            {

            }
        }
        //there is kitchenobject
        else
        {
            //player carring something
            if (player.HasKitchenObject())
            {
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {

                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {

                        GetKitchenObject().DestroySelf();
                    }
                }


            }
            //player is not carring anyting
            else
            {
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }



    }

    public override void InteractAlternate(Player player)
    {
        if (HasKitchenObject() && HasRecipWithInput(GetKitchenObject().GetKitchenObjectSO()))
        {

            cuttingProgress++;
            OnCut?.Invoke(this, EventArgs.Empty);
            OnAnyCut?.Invoke(this, EventArgs.Empty);

            CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());

            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
            {
                progressNormalized = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax
            });


            if (cuttingProgress >= cuttingRecipeSO.cuttingProgressMax)
            {
                KitchenObjectSo outputKitchenObjectso = getoutputforInput(GetKitchenObject().GetKitchenObjectSO());

                GetKitchenObject().DestroySelf();

                KitchenObject.SpawnkitchenObject(outputKitchenObjectso, this);
            }
           
        }
    }

    private bool HasRecipWithInput(KitchenObjectSo inputkitchenObjectSO)
    {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputkitchenObjectSO);
        return cuttingRecipeSO != null;
       
    }


    private KitchenObjectSo getoutputforInput(KitchenObjectSo inputKitchenObjectSO)
    {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputKitchenObjectSO);
        if (cuttingRecipeSO != null)
        {
            return cuttingRecipeSO.output;
        }
        else
        {
            return null;
        }
       
    }


    private CuttingRecipeSO GetCuttingRecipeSOWithInput(KitchenObjectSo inputKitchenObjectSO)
    {
        foreach (CuttingRecipeSO cuttingRecipeSO in cuttingRecipeSoArray)
        {
            if (cuttingRecipeSO.input == inputKitchenObjectSO)
            {
                return cuttingRecipeSO;
            }
        }
        return null;
    }
}
        