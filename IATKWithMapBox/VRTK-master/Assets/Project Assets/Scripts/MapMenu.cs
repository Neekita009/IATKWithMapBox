using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using IATK;
using UnityEngine.UI;
public class MapMenu : MonoBehaviour {

    public VRMenuInteractor Menu;
    public GameObject floor;
    public Slider floorSlider;
   
	// Use this for initialization
	void Start () {
        Menu.X_AxisDropDown.enabled = false;
        Menu.Z_AxisDropDown.enabled = false;
        floorSlider = GetComponent<Slider>();
        floorSlider.value = floor.transform.position.y;
        //  Viz.geometry = AbstractVisualisation.GeometryType.Bars;
    }

    // Update is called once per frame

    public void decreaseSize()
    {
 
     }
  
	void Update () {
        Vector3 temp = floor.transform.position;
        temp.y = floorSlider.value;
        floor.transform.localPosition = temp;
    }
}
