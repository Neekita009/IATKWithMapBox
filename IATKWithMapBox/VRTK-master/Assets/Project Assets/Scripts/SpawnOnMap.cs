namespace Mapbox.Examples
{
    using UnityEngine;
    using Mapbox.Utils;
    using Mapbox.Unity.Map;
    using Mapbox.Unity.MeshGeneration.Factories;
    using Mapbox.Unity.Utilities;
    using System.Collections.Generic;
    using IATK;
    using System.Linq;
    using VRTK;
    using UnityEngine.UI;
    using System;

    public class SpawnOnMap : MonoBehaviour
    {
        [SerializeField]
        AbstractMap _map;
        public Visualisation _viz;
     //   public Visualisation HospitalViz;
        public CSVDataSource mySourceData;


        //private  variables 
        private string xExtremeAxis;
        private string zExtremeAxis;
        private string center;
        private float smoothTime = 0.003F;
        private Vector3 velocity = Vector3.zero;


        void Start()
        {
            // obtaining the  maximum and minimum latitiude and longitude from the graph 
            object maxlongitute = _viz.dataSource.getOriginalValue(mySourceData["X"].Data.Max(), "X");
            object maxlatitude = _viz.dataSource.getOriginalValue(mySourceData["Y"].Data.Max(), "Y");
            object minlongitute = _viz.dataSource.getOriginalValue(mySourceData["X"].Data.Min(), "X");
            object minlatitude = _viz.dataSource.getOriginalValue(mySourceData["Y"].Data.Min(), "Y");

            // calculating the coordinates for the x-axis end point, z-axis end point  and the center
            xExtremeAxis = maxlatitude + "," + minlongitute;
            zExtremeAxis = minlatitude + "," + maxlongitute;
            center = minlatitude + "," + minlongitute;

            // coverting the coordinates into geolocation 
            Vector3 xExtremeAxisGeo = _map.GeoToWorldPosition(Conversions.StringToLatLon(xExtremeAxis), true);
            Vector3 zExtremeAxisGeo = _map.GeoToWorldPosition(Conversions.StringToLatLon(zExtremeAxis), true);
            Vector3 centerGeo = _map.GeoToWorldPosition(Conversions.StringToLatLon(center), true);

            // Assigning the position to the visulization by making center of the visulization fixed  
            _viz.transform.position = centerGeo;
        
            // calculating the length of x and z axis for width and depth of the graph respectively  
            var width = (centerGeo - zExtremeAxisGeo).magnitude;
            var Depth = (centerGeo - xExtremeAxisGeo).magnitude;
            _viz.width = width;
            _viz.depth = Depth;
         
          //  height of the graph
            // when z-axis is not defined 
            if (_viz.zDimension.Attribute == "Undefined")
            {
                _viz.height = _map.Options.locationOptions.zoom / 5;
              
            }
            else // when z-axis is defined 
            {
                _viz.height = _viz.zDimension.maxScale;
             
            }

            // function for map update 
            _map.OnUpdated += delegate
            {
                UpdateMap();

            };

        }


        public void UpdateMap()
        {

            // update the geolocation of the graph according to change in the Map
            Vector3 xExtremeAxisGeo = _map.GeoToWorldPosition(Conversions.StringToLatLon(xExtremeAxis), true);
            Vector3 zExtremeAxisGeo = _map.GeoToWorldPosition(Conversions.StringToLatLon(zExtremeAxis), true);
            Vector3 centerGeo = _map.GeoToWorldPosition(Conversions.StringToLatLon(center), true);

            var width = (centerGeo - zExtremeAxisGeo).magnitude;
            var Depth = (centerGeo - xExtremeAxisGeo).magnitude;
        
            _viz.width = width;
            _viz.depth = Depth;
            _viz.transform.position = centerGeo;
         


        }


        private void Update()
        {
          
        }
    }
}