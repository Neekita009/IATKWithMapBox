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
public class CustomPosition : MonoBehaviour {
   
    public CSVDataSource mySourceData;
    public AbstractMap _map;
    public VRTK_Pointer pointerEvents;
    public GameObject newPosition;

    Vector3 Newposition = new Vector3();
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
    void Start () {
        /*
        _map.OnUpdated += delegate
        {
            PlotUpdate();

        }; */
        /*
        if (pointerEvents)
        {
            pointerEvents.DestinationMarkerHover += new DestinationMarkerEventHandler(DoPointing);
        }*/
    }

    public void DoPointing(object sender, DestinationMarkerEventArgs e)
    {
        Vector3 currentPosition = e.destinationPosition;
        Vector3[] LocationVector = new Vector3[mySourceData.DataCount];
        LocationVector = generateGeoData(mySourceData, "Latitude", "Longitude", "");

        if (currentPosition.x > 0 || currentPosition.y > 0 || currentPosition.z > 0)
        {
            
            foreach (Vector3 j in LocationVector)
            {

                if (System.Math.Round(currentPosition.x,1).Equals(System.Math.Round(j.x, 1)) && System.Math.Round(currentPosition.y, 1).Equals(System.Math.Round(j.y, 1)) && System.Math.Round(currentPosition.z, 1).Equals(System.Math.Round(j.z, 1)))
                {
                    Debug.Log(currentPosition + " My Position" + "  Data Position is:" + j);
                    getToolTip(j);
                }

            }
        }
    }

    void getToolTip(Vector3 GeoPosition)
    {


        Newposition = GeoPosition;
        newPosition.transform.position = Newposition;
        newPosition.GetComponent<Renderer>().material.color = Color.yellow;
       
    

    }

    System.Action PlotUpdate()
    {


     newPosition.transform.position = Newposition;
     return null;
    }



    // Update is called once per frame
    void Update () {

    }
}
