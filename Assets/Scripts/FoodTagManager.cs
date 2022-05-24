using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class FoodTagManager : MonoBehaviour
{
    private List<FoodTag> foodTagList;
    private float yOffset = 0.2f;
    [SerializeField] private GameObject cameraFolder;
    [SerializeField] private List<Camera> spectatorCameras;
    private Camera activeCamera = null;
    // Start is called before the first frame update
    void Awake()
    {
        foodTagList = new List<FoodTag>();
        for (int i = 0; i < cameraFolder.transform.childCount; i++)
        {
            spectatorCameras.Add(cameraFolder.transform.GetChild(i).GetComponent<Camera>());
            cameraFolder.transform.GetChild(i).GetComponent<Camera>().enabled = false;
        }
        spectatorCameras[0].enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        activeCamera = spectatorCameras.Where(cam => cam.enabled == true).SingleOrDefault();
        foreach (FoodTag ft in foodTagList)
        { 
            if(ft.food == null)
            {
                Destroy(ft.tag);
                foodTagList.Remove(ft);
                break;
            }
            ft.tag.transform.position = ft.food.transform.position + Vector3.up * yOffset;
            var lookAtPos = new Vector3(activeCamera.transform.position.x, activeCamera.transform.position.y, activeCamera.transform.position.z);
            ft.tag.transform.LookAt(lookAtPos, activeCamera.transform.up);
        }
    }
    public void AddNewTag(FoodTag newTag)
    {
        foodTagList.Add(newTag);
    }
    public class FoodTag
    {
        public GameObject food, tag;

        public FoodTag(GameObject food, GameObject tag)
        {
            this.food = food;
            this.tag = tag;
        }
    }
}
