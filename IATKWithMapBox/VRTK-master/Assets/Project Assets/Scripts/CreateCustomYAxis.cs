using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IATK;
using VRTK;
using UnityEngine.UI;
using System;
using System.Linq;


public class CreateCustomYAxis : MonoBehaviour {
    //public variables 
    public GameObject NewAxis;
    public Visualisation _viz;
    public GameObject SceneObjects;
    public GameObject headset;
    public CSVDataSource Data;

    //private and default variables 
    private float CurrentConePosY;
    private GameObject Axis;
    private Vector3 BaseFloor = new Vector3(0, -.5f, 0);
    private Slider floorSlider;
    private Vector3 TopHeight = new Vector3(0, 1f, 0);
    private float height;
    private float ZeroYPosition;
    // Use this for initialization
    void Start () {

        Axis = Instantiate(NewAxis); // create copy of prefab Axis 
        SetUpMenu();
        SetUpAxis();
        //  _viz.height = _viz.yDimension.maxScale;
        SceneObjects.transform.position =new Vector3(SceneObjects.transform.position.x, ZeroYPosition-.07f, SceneObjects.transform.position.z) ;
    }
    private void SetUpAxis()
    {
        CurrentConePosY = Axis.transform.Find("TopCone").position.y;
        Axis.transform.parent = SceneObjects.transform;
        Axis.transform.Find("Label").GetComponent<TextMesh>().text = _viz.yDimension.Attribute;
        Axis.transform.Find("Cylinder").localScale = new Vector3(Axis.transform.Find("Cylinder").localScale.x, _viz.yDimension.maxScale, Axis.transform.Find("Cylinder").localScale.z);
        Axis.transform.Find("TopCone").GetComponent<VRTK_InteractableObject>().InteractableObjectGrabbed += new InteractableObjectEventHandler(ObjectGrabbed);
        Axis.transform.Find("TopCone").GetComponent<VRTK_InteractableObject>().InteractableObjectUngrabbed += new InteractableObjectEventHandler(ObjectUnGrabbed);
        
    }
    private void SetUpMenu()
    {
        Axis.transform.Find("VR_Menu").GetComponent<VRMenuInteractor>().visualisation = _viz;
        floorSlider = Axis.transform.Find("VR_Menu/Canvas/Panel/Container/GroundLevelSlider").GetComponent<Slider>();
        floorSlider.value = -0.5f;
        floorSlider.onValueChanged.AddListener(value =>
        {
            ValidateGroundLevel();
        });
    }
  
    // Change position of the axis on click 
    public void ChangePosition(Vector3 positionCurrent)
    {

        // providing position to y-axies 
        positionCurrent.y = positionCurrent.y + .85f;
        positionCurrent.x = positionCurrent.x + .3f;
        positionCurrent.z = positionCurrent.z + -.3f;
        Axis.transform.position = positionCurrent;
        Axis.transform.rotation = Quaternion.Euler(new Vector3(0, headset.transform.eulerAngles.y, 0));
    }

    // event listner when objects are grabbed   
    private void ObjectGrabbed(object sender, InteractableObjectEventArgs e)
    {
       
    }

    // event listner when objects are grabbed   
    private void ObjectUnGrabbed(object sender, InteractableObjectEventArgs e)
    {
  
    }

    //Change the ground level by slider on the menu 
    private void ValidateGroundLevel()
    {
        Vector3 temp = SceneObjects.transform.position;
        temp.y = floorSlider.value;
    //    SceneObjects.transform.localPosition = temp;
      
    }
  
    // Update is called once per frame
    void Update () {

        height = Vector3.Distance(BaseFloor, TopHeight);
        float MaxDataValue = Int32.Parse(_viz.dataSource.getOriginalValue(Data[_viz.yDimension.Attribute].Data.Max(), _viz.yDimension.Attribute).ToString());
        float TopConeYPosition = Axis.transform.Find("TopCone").position.y;
        float SideConeYPosition = Axis.transform.Find("SideCone").position.y;
        float NormalisedValue =((SideConeYPosition) / (TopConeYPosition-.13f));
        float CurrentDataValue = NormalisedValue * MaxDataValue;
        Axis.transform.Find("SideCone/Value").GetComponent<TextMesh>().text = Math.Round(CurrentDataValue, 0).ToString();
        _viz.transform.Find("View/BigMesh").GetComponent<Renderer>().material.SetFloat("_cutoffHeight", NormalisedValue); // change the level of the shader by the smaller cone 
        Axis.transform.Find("Label").GetComponent<TextMesh>().text = _viz.yDimension.Attribute;// label name for custom y axis 
        
        if(CurrentDataValue == 0f)
        {
             ZeroYPosition = Axis.transform.Find("SideCone").position.y;
        }

        //if(Axis.transform.Find("SideCone").position.y <= ZeroYPosition)
        //{
        //    Axis.transform.Find("SideCone").position = new Vector3(Axis.transform.Find("SideCone").position.x, ZeroYPosition, Axis.transform.Find("SideCone").position.z);
        //}

        Vector3 LineVector = Axis.transform.Find("Cylinder").position - Axis.transform.Find("TopCone").position;
        if (LineVector.magnitude <= 0.6700001f)
        {
            if (CurrentConePosY != 0f)
            {
                Axis.transform.Find("Cylinder").localScale = new Vector3(Axis.transform.Find("Cylinder").localScale.x, _viz.yDimension.maxScale, Axis.transform.Find("Cylinder").localScale.z);
                Axis.transform.Find("TopCone").position = new Vector3(Axis.transform.Find("Cylinder").position.x, Axis.transform.Find("Cylinder").position.y + .78f, Axis.transform.Find("Cylinder").position.z);
            }
            _viz.height = _viz.yDimension.maxScale;
        }
        else
        {
            Vector3 value = new Vector3(Axis.transform.Find("Cylinder").localScale.x, LineVector.magnitude, Axis.transform.Find("Cylinder").localScale.z);
            Axis.transform.Find("Cylinder").localScale = value;
            _viz.height = Axis.transform.Find("TopCone").position.y;
        }
    }
}

   
