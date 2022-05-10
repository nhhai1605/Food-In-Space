using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodDispenser : MonoBehaviour
{
    [SerializeField] List<GameObject> foods = new List<GameObject>();
    [SerializeField] GameObject SpawningLocation, Door;
    private DoorManager doorManager;
    private int idx = 0;

    void Start()
    {
        doorManager = Door.GetComponent<DoorManager>();
    }

    void Update()
    {
        
    }

    private void Spawn()
    {
        Instantiate(foods[idx], SpawningLocation.transform.position, Quaternion.identity);
        idx++;
        if (idx >= foods.Count)
        {
            idx = 0;
        }
    }
    public void Dispense()
    {

        if (doorManager.IsDoorOpen())
        {
            Invoke("Spawn", 1f);
        }
    }
}
