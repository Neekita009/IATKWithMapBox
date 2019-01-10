using Mapbox.Unity.Map;
using Mapbox.Unity.Utilities;
using Mapbox.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbstractMapInteraction : MonoBehaviour {
    public AbstractMap _map;
    [Range(0, 10)]
    public float _panSpeed;
    [Range(0, 10)]
    public float _zoomSpeed;
    /*
       * <summary>
       * Copied from QuadCameraMovement.cs
       * </summary>
       * */
    public  void Pan(float x, float y)
    {
        float factor = _panSpeed * (Conversions.GetTileScaleInDegrees((float) _map.CenterLatitudeLongitude.x, _map.AbsoluteZoom));
        var latitudeLongitude = new Vector2d(_map.CenterLatitudeLongitude.x + y * factor * 2.0f, _map.CenterLatitudeLongitude.y + x * factor * 4.0f);
        _map.UpdateMap(latitudeLongitude, _map.Zoom);
    }

    private void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        if(x != 0 || y != 0)
        {
            Debug.Log(x + "," + y);
            Pan(x, y);
        }
    }


    /*
     * <summary>
     * Zoom to point on Map instead of zooming to center
     * </summary>
     * */
    public  void Zoom(Vector3 target, float value)
    {
        Vector3 mapCenter = _map.GeoToWorldPosition(_map.CenterLatitudeLongitude);

        target -= mapCenter;
        var zoom = Mathf.Max(0.0f, Mathf.Min(_map.Zoom + value * _zoomSpeed, 21.0f));
        var change = zoom - _map.Zoom;
     
        //0.7f is a constant
        var offsetX = target.x * change * 0.7f;
        var offsetY = target.z * change * 0.7f;

        Vector3 newCenterUnity = new Vector3(offsetX, 0, offsetY) + mapCenter;
        Vector2d newCenter = _map.WorldToGeoPosition(newCenterUnity);

        _map.UpdateMap(newCenter, zoom);
    }

    public void Zoom(Vector2d target, float value)
    {
        Vector3 targetUnity = _map.GeoToWorldPosition(target);
        Zoom(targetUnity, value);
    }

    public  void Zoom(float value)
    {
        var zoom = Mathf.Max(0.0f, Mathf.Min(_map.Zoom + value * _zoomSpeed, 21.0f));
        _map.UpdateMap(_map.CenterLatitudeLongitude, zoom);
    }

}
