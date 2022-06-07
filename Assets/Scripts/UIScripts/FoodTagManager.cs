using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class FoodTagManager : MonoBehaviour
{
    private List<FoodTag> foodTagList;
    private CameraSwitch cameraSwitch;
    private float yOffset = 0.2f;

    // Start is called before the first frame update
    void Awake()
    {
        cameraSwitch = GetComponent<CameraSwitch>();
        foodTagList = new List<FoodTag>();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (FoodTag ft in foodTagList)
        {
            if (ft.food == null)
            {
                Destroy(ft.tag);
                foodTagList.Remove(ft);
                break;
            }
            try
            {
                ft.tag.transform.position = ft.food.transform.position + Vector3.up * yOffset;
                var lookAtPos = new Vector3(cameraSwitch.currentCamera.transform.position.x, cameraSwitch.currentCamera.transform.position.y, cameraSwitch.currentCamera.transform.position.z);
                ft.tag.transform.LookAt(lookAtPos, cameraSwitch.currentCamera.transform.up);
            }
            catch 
            {

            }
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
