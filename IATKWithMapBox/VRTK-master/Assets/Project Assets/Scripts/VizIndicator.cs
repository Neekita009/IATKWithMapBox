using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IATK;
using System.Linq;
using Mapbox.Utils;
using Mapbox.Unity.Map;
using Mapbox.Unity.MeshGeneration.Factories;
using Mapbox.Unity.Utilities;
using VRTK;
using UnityEngine.UI;

public class VizIndicator : MonoBehaviour {
    public CSVDataSource mySourceData;
    public AbstractMap _map;
    public GameObject Cube;
    public Visualisation _viz;
    List<string> LocalString = new List<string>();

    Vector3[] generateGeoData(CSVDataSource csvds, string dimensionX, string dimensionY, string dimensionZ)
    {
        LocalString.Clear();
        float[] originalX = new float[csvds[dimensionX].Data.Length];
        float[] stringx = new float[csvds[dimensionX].Data.Length];
        Dictionary<float, string> originalx_dict = new Dictionary<float, string>();
        for (int i = 0; i < csvds[dimensionX].Data.Length; i++)
            originalX[i] = (float)csvds.getOriginalValue(csvds[dimensionX].Data[i], dimensionX);

        float[] originalY = new float[csvds[dimensionY].Data.Length];

        for (int i = 0; i < csvds[dimensionY].Data.Length; i++)
            originalY[i] = (float)csvds.getOriginalValue(csvds[dimensionY].Data[i], dimensionY);


        Vector2d[] dataProjected = new Vector2d[csvds[dimensionY].Data.Length];


        for (int i = 0; i < csvds[dimensionY].Data.Length; i++)
        {
            string newString = originalX[i] + "," + originalY[i];
            LocalString.Add(newString);
        }



        for (int i = 0; i < LocalString.Count; i++)
        {
            dataProjected[i] = Conversions.StringToLatLon(LocalString[i]);
        }

        Vector3[] LocationVector = new Vector3[dataProjected.Count()];

        for (int i = 0; i <= dataProjected.Count() - 1; i++)
        {
            LocationVector[i] = _map.GeoToWorldPosition(dataProjected[i], true);

        }

        return LocationVector;
    }
    // Use this for initialization
    void Start () {
     //   _map.Initialize(vector ,10);
        Vector3[] location =   generateGeoData(mySourceData, "Latitude", "Longitude", "");
       for(int i =0; i<= location.Length-1 ;i++)
        {
            GameObject cube = Instantiate(Cube) as GameObject;
            cube.transform.position = location[i];
           
        }

        _map.OnUpdated += delegate
        {
            UpdateMap();

        };

    }

    public void UpdateMap()
    {
      

    }
        // Update is called once per frame
        void Update () {
		
	}
}
