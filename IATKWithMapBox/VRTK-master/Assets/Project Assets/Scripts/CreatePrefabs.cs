using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using IATK;
using VRTK;
using UnityEngine.UI;


public class CreatePrefabs : MonoBehaviour
{
    // Use this for initialization
    public GameObject prefab;
    //  public Visualisation Viz;
    public Visualisation VizCloneOne;
    public CSVDataSource Data;
    public BrushingAndLinking BL;
    public Visualisation mainMapViz;
    public GameObject Cube;
    public int NumberOfGraph =2;
   
    //public GameObject Viz_GameObject;
    //public GameObject Cube;
    //public CSVDataSource Data;
    //;
    //public BrushingAndLinking BL;
    int CloneCount = 0;
    int CloneName = 0;
    public void Start()
    {
        CloneCount = 0;
        CloneName = 0;
      
    }

   public  void InstantiatePrefab()
    {
      
        if (CloneCount < NumberOfGraph)
        {
            //position of the cube 
            float countCloneDist = CloneCount * 1.5f; // adjusting the position as per the number of graph 
            Vector3 pos = new Vector3(-0.869097f + countCloneDist, 2.297714f, 1.17764f);
           
          

            var gameObject = new GameObject(); // creating parent GameObject 
            
            // initializing cube(for dragging), Menu, Visualisation 
            GameObject cube = (GameObject)Instantiate(Cube, pos, Quaternion.Euler(new Vector3(0, 0, 0)));
            GameObject Menu = (GameObject)Instantiate(prefab);
            Visualisation viz = Instantiate(VizCloneOne, pos, Quaternion.Euler(new Vector3(0, 0, 0)));

            // assigning GameObject to Follow for Visulization and Menu 
            viz.GetComponent<VRTK_TransformFollow>().gameObjectToFollow = cube.transform.Find("Cube").gameObject;
            Menu.transform.GetComponent<VRTK_TransformFollow>().gameObjectToFollow = cube.transform.Find("Canvas/Dummy").gameObject;

            // Assigning values to x,y,z parameter of Visualisation 
            viz.dataSource = Data;
            viz.xDimension.Attribute = "Undefined";
            viz.yDimension.Attribute = "Undefined";
            viz.zDimension.Attribute = "Undefined";
            viz.geometry = AbstractVisualisation.GeometryType.Bars;
            viz.colorPaletteDimension = "Undefined";

            // Assigning Visualisation to brushing and linking  
            BL.brushedVisualisations.Add(viz);
            Menu.GetComponent<VRMenuInteractor>().visualisation = viz;

            // for Toggle event 
            Toggle MenuToggle = cube.transform.Find("Canvas/MenuToggle").GetComponent<Toggle>();
            Toggle DestroyToggle = cube.transform.Find("Canvas/DestroyToggle").GetComponent<Toggle>();
            DestroyToggle.name = CloneName.ToString();

           

            //CreateAxisDropDown()



            // making Gameobject as parent to all the clone object 
            gameObject.name = CloneName.ToString();
            viz.transform.parent = gameObject.transform;
            cube.transform.parent = gameObject.transform;
            Menu.transform.parent = gameObject.transform;
           
            // toggle function for menu enable and disable 
            MenuToggle.onValueChanged.AddListener(value =>
            {
                Menu.SetActive(value);
            });

            // toggle function for Destroying the graph 
            DestroyToggle.onValueChanged.AddListener(value =>
            {
                if (!value)
                {
                    Destroy(GameObject.Find(DestroyToggle.name));
                    CloneCount--; // when object is destroyed the count of the clone decreases 

                }
               
            });
            // count of graph 
            CloneCount++;
            //name for the graph 
            CloneName++;
        }
    }

    //private List<string> GetAttributesList()
    //{
    //    List<string> dimensions = new List<string>();
    //    dimensions.Add("Undefined");
    //    for (int i = 0; i < visualisation.dataSource.DimensionCount; ++i)
    //    {
    //        dimensions.Add(visualisation.dataSource[i].Identifier);
    //    }
    //    return dimensions;
    //}
    // Update is called once per frame
    void Update()
    {
       
    }
}