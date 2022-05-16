using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class FoodDispenser : MonoBehaviour
{
    [SerializeField] GameObject foodFolder, SpawningLocation, Door;
    [SerializeField] XMLManager xmlManager;
    private List<XMLManager.XMLFood> xmlFoodList;
    private List<GameObject> foodObjects;
    private DoorManager doorManager;
    private int currOrder;
    private bool spawning = false;
    [SerializeField] float timeBetweenFoodSpawn = 2f; //seconds
    private List<XMLManager.XMLFood> currFoods;
    private int spawned = 0;
    private float dt = 0;
    void Start()
    {
        doorManager = Door.GetComponent<DoorManager>();
        foodObjects = new List<GameObject>();
        currFoods = new List<XMLManager.XMLFood>();
        this.xmlFoodList = xmlManager.foodList;
        for (int i = 0; i < foodFolder.transform.childCount; i++)
        {
            foodObjects.Add(foodFolder.transform.GetChild(i).gameObject);
        }
        currOrder = 1;
    }
    public void SpawnOneFood()
    {
        foreach (var food in xmlFoodList)
        {
            if (food.Order == currOrder)
            {
                GameObject foodMesh = foodObjects.Where(obj => obj.name == food.MeshName).First();
                var newSpawn = Instantiate(foodMesh, SpawningLocation.transform.position, Quaternion.identity);
                newSpawn.name = $"{food.Id}-{food.MeshName}-{food.SurveyName}-{food.Quantity}-{food.Order}";
                newSpawn.SetActive(true);
                newSpawn.GetComponent<Floating>().SetOffset(0, 0.3f, 0);             
            }
        }
        currOrder++;
    }
    void Update()
    {
        if(spawning)
        {
            if (currFoods.Count > 0)
            {
                dt += Time.deltaTime;
                print(dt);
                if (dt > timeBetweenFoodSpawn)
                {
                    SpawnFoodWithoutChangingOrder(currFoods[0]);
                    spawned++;
                    dt = 0;
                }
                if (spawned == currFoods[0].Quantity)
                {
                    currFoods.RemoveAt(0);
                    dt = 0;
                    spawned=0;
                }
            }
            else
            {
                doorManager.InvokeControlDoor(timeBetweenFoodSpawn);
                spawning = false;
                dt = 0;
                spawned = 0;
                currOrder++;
            }
        }
    }

    private void SpawnFoodWithoutChangingOrder(XMLManager.XMLFood currFood)
    {
        if (currFood != null)
        {
            GameObject foodMesh = foodObjects.Where(obj => obj.name == currFood.MeshName).First();
            var newSpawn = Instantiate(foodMesh, SpawningLocation.transform.position, Quaternion.identity);
            newSpawn.name = $"{currFood.Id}-{currFood.MeshName}-{currFood.SurveyName}-{currFood.Quantity}-{currFood.Order}";
            newSpawn.SetActive(true);
            newSpawn.GetComponent<Floating>().SetOffset(0, 0.5f, 0);
        }
    }


    public void Dispense()
    {
        doorManager.ControlDoor();

        foreach (var food in xmlFoodList)
        {
            if (food.Order == currOrder)
            {
                currFoods.Add(food);
            }
        }
        this.spawning = true;
    }
}

