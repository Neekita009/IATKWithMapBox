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
using Mapbox.Examples;

public class mycustompositions : MonoBehaviour {


    public VRTK_Pointer pointerEvents;
    public SpawnOnMap Spwan;




    // generate GeoLocation For the Map form the Latitude and Longitute 

    public  void Start () {
     

              if (pointerEvents)
             {
                pointerEvents.DestinationMarkerHover += new DestinationMarkerEventHandler(DoPointing);
              }
       
    }
   
 public void DoPointing(object sender, DestinationMarkerEventArgs e)
    {
       Vector3 currentPosition = e.destinationPosition;
     
    }

  
   
    void Update()
    {
     
    }
}

