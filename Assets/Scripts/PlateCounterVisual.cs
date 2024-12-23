using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateCounterVisual : MonoBehaviour
{
    [SerializeField] private Transform counterTopPoint;
    [SerializeField] private Transform plateVisualPrefab;
    [SerializeField] private PlatesCounter platesCounter;


    private List<GameObject> plateVisualObjectList;
    private void Awake()
    {
        plateVisualObjectList = new List<GameObject>();
    }


    private void Start()
    {
        platesCounter.OnPlateSpawned += PlatesCounter_OnPlateSpawned;
        platesCounter.OnPlateRemove += PlatesCounter_OnPlateRemove;
    }

    private void PlatesCounter_OnPlateRemove(object sender, System.EventArgs e)
    {
       GameObject plateGameObject = plateVisualObjectList[plateVisualObjectList.Count - 1];
        plateVisualObjectList.Remove(plateGameObject);
        Destroy(plateGameObject);
    }

    private void PlatesCounter_OnPlateSpawned(object sender, System.EventArgs e)
    {
        Transform plateViusalTransform = Instantiate(plateVisualPrefab,counterTopPoint);
        float platOffSetY = .1f;
        plateViusalTransform.localPosition=new Vector3(0,platOffSetY*plateVisualObjectList.Count,0);

        plateVisualObjectList.Add(plateViusalTransform.gameObject);


    }
}
