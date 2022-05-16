using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class SpawningManager : MonoBehaviour
{
    [SerializeField] GameObject foodFolder, SpawningLocation;
    [SerializeField] XMLManager xmlManager;
    private List<XMLManager.XMLFood> xmlFoodList;
    private List<GameObject> foodObjects;
    private int currOrder;
    // Start is called before the first frame update
    void Start()
    {
        foodObjects = new List<GameObject> ();
        this.xmlFoodList = xmlManager.foodList;
        for (int i = 0; i < foodFolder.transform.childCount; i++)
        {
            foodObjects.Add(foodFolder.transform.GetChild(i).gameObject);
        }
        currOrder = 0;
        SpawnFoods();
    }

    void SpawnFoods()
    {
        foreach (var food in xmlFoodList)
        {
            //if(food.Order == currOrder)
            //{
                for (int i = 0; i < food.Quantity; i++)
                {
                    GameObject foodMesh = foodObjects.Where(obj => obj.name == food.MeshName).First();
                    var newSpawn = Instantiate(foodMesh, SpawningLocation.transform.position, Quaternion.identity);
                    newSpawn.name = $"{food.Id}-{food.MeshName}-{food.SurveyName}-{food.Quantity}-{food.Order}";
                    newSpawn.SetActive(true);
                }
            //}
        }
        currOrder++;
    }
}
