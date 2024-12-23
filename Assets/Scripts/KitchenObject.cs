using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    [SerializeField] private KitchenObjectSo kitchenObjectSO;
    private IKitchenObjectParent KitchenObjectparent;

    public KitchenObjectSo GetKitchenObjectSO() { return kitchenObjectSO; }

    public void SetKitchenObjectParent(IKitchenObjectParent kitchenObjectParent)
    {
        if (this.KitchenObjectparent != null){
            this.KitchenObjectparent.ClearKitchenObject();
        }

  
        this.KitchenObjectparent = kitchenObjectParent;
        if (kitchenObjectParent.HasKitchenObject()) {
            Debug.LogError("IkitchenObjectParent already has object");
        
        }
        kitchenObjectParent.SetKitchenObject(this);
        transform.parent = kitchenObjectParent.GetKitchenObjectFollowTransform();
        transform.localPosition = Vector3.zero;
    }

    public IKitchenObjectParent GetKitchenObjectParent() { return KitchenObjectparent; }

    public void DestroySelf()
    {
        KitchenObjectparent.ClearKitchenObject() ;

        Destroy(gameObject);
    }

    public bool TryGetPlate(out PlateKitchenObject plateKitchenObject)
    {
        if (this is PlateKitchenObject)
        {
            plateKitchenObject = this as PlateKitchenObject;
            return true;
        }
        else
        {
            plateKitchenObject = null;
            return false;
        }
    }





    public static KitchenObject SpawnkitchenObject (KitchenObjectSo kitchenObjectSO, IKitchenObjectParent kitchenObjectParent)
    {
        Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.prefab);
        KitchenObject kitchenObject = kitchenObjectTransform.GetComponent<KitchenObject>();
        
      
        kitchenObject.SetKitchenObjectParent(kitchenObjectParent);
       
        return kitchenObject;
    
    }
}
