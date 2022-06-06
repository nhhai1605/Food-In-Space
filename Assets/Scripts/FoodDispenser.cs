using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
public class FoodDispenser : MonoBehaviour
{
    [SerializeField] private GameObject foodFolder, SpawningLocation, Door;
    [SerializeField] private XMLManager xmlManager;
    private List<XMLManager.XMLFood> xmlFoodList;
    private List<GameObject> foodObjects = new List<GameObject>();
    private DoorManager doorManager;
    private bool spawning = false;
    private float timeBetweenFoodSpawn = 2f; //seconds
    private List<XMLManager.XMLFood> currFoods = new List<XMLManager.XMLFood>();
    private int spawned = 0;
    private float dt = 0;
    private float yOffset = 0.5f;
    private float doorOpenTime = 2f;
    private int currIdx = 0;
    [SerializeField] private GameObject foodTag;
    [SerializeField] private FoodTagManager foodTagManager;
    List<int> foodOrderList = new List<int>();
    [SerializeField] private Transform spawnFoodFolder, foodTagFolder;
    [SerializeField] private GameObject smokeEffect;
    void Start()
    {
        doorManager = Door.GetComponent<DoorManager>();
        this.xmlFoodList = xmlManager.foodList;
        for (int i = 0; i < foodFolder.transform.childCount; i++)
        {
            foodObjects.Add(foodFolder.transform.GetChild(i).gameObject);
        }
        foreach (XMLManager.XMLFood food in xmlFoodList)
        {
            foodOrderList.Add(food.Order);
        }
        foodOrderList = foodOrderList.Distinct().ToList();
    }
    public void SpawnOneFood()
    {
        foreach (var food in xmlFoodList)
        {
            if (food.Order == foodOrderList[currIdx])
            {
                GameObject foodMesh = foodObjects.Where(obj => obj.name == food.MeshName).First();
                var newSpawn = Instantiate(foodMesh, SpawningLocation.transform.position, Quaternion.identity);
                newSpawn.name = $"{food.Id}-{food.MeshName}-{food.Color}-{food.SurveyName}-{food.Quantity}-{food.Order}";
                newSpawn.SetActive(true);
                newSpawn.GetComponent<Floating>().SetOffset(0, yOffset, 0);
                if (food.Color != "Default")
                {
                    newSpawn.GetComponentsInChildren<MeshRenderer>().Where(c => c.name.Split(' ')[0] == "Chroma").FirstOrDefault().material = xmlManager.GetMaterialList().Where(m => m.name == food.Color).FirstOrDefault();
                }
                var foodTagSpawn = Instantiate(foodTag, newSpawn.transform.position, Quaternion.identity);
                foodTagSpawn.name = $"Food ID: {food.Id}";
                foodTagSpawn.GetComponentInChildren<Text>().text = $"Food ID: {food.Id}";

                newSpawn.transform.SetParent(spawnFoodFolder);
                foodTagSpawn.transform.SetParent(foodTagFolder);

                foodTagManager.AddNewTag(new FoodTagManager.FoodTag(newSpawn, foodTagSpawn));
            }
        }
        currIdx++;
    }
    void Update()
    {
        if (spawning)
        {
            if (currFoods.Count > 0)
            {
                //smokeEffect.SetActive(true);
                dt += Time.deltaTime;
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
                    spawned = 0;
                }
            }
            else
            {
                //Invoke(nameof(TurnOffSmoke), 4f);
                smokeEffect.SetActive(false);
                spawning = false;
                dt = 0;
                spawned = 0;
                currIdx++;
            }
        }
    }

    private void TurnOffSmoke()
    {
        smokeEffect.SetActive(false);
    }
    private void SpawnFoodWithoutChangingOrder(XMLManager.XMLFood currFood)
    {
        if (currFood != null)
        {
            GameObject foodMesh = foodObjects.Where(obj => obj.name == currFood.MeshName).First();
            var newSpawn = Instantiate(foodMesh, SpawningLocation.transform.position, Quaternion.identity);
            newSpawn.name = $"{currFood.Id}-{currFood.MeshName}-{currFood.Color}-{currFood.SurveyName}-{currFood.Quantity}-{currFood.Order}";
            newSpawn.SetActive(true);
            newSpawn.GetComponent<Floating>().SetOffset(0, yOffset, 0);
            if (currFood.Color != "Default")
            {
                newSpawn.GetComponentsInChildren<MeshRenderer>().Where(c => c.name.Split(' ')[0] == "Chroma").FirstOrDefault().material = xmlManager.GetMaterialList().Where(m => m.name == currFood.Color).FirstOrDefault();
            }
            var foodTagSpawn = Instantiate(foodTag, newSpawn.transform.position, Quaternion.identity);
            foodTagSpawn.name = $"Food ID: {currFood.Id}";
            foodTagSpawn.GetComponentInChildren<Text>().text = $"Food ID: {currFood.Id}";

            newSpawn.transform.SetParent(spawnFoodFolder);
            foodTagSpawn.transform.SetParent(foodTagFolder);

            foodTagManager.AddNewTag(new FoodTagManager.FoodTag(newSpawn, foodTagSpawn));
        }
    }


    public void Dispense()
    {
        int quantity = 0;
        while (true)
        {
            if (currIdx >= foodOrderList.Count)
            {
                return;
            }
            foreach (var food in xmlFoodList)
            {
                if (food.Order == foodOrderList[currIdx])
                {
                    currFoods.Add(food);
                    quantity += food.Quantity;
                }
            }
            if (quantity == 0)
            {
                currIdx++;
            }
            else
            {
                break;
            }
        }
        doorManager.SetOpenTime(quantity * doorOpenTime);
        doorManager.ControlDoor();
        this.spawning = true;
    }
}

