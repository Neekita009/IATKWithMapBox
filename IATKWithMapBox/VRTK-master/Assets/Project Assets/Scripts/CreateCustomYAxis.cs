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
   
    private float ZeroYPosition;
    public bool IsChangePosition = false;
    public bool IsChangeConePosition = false;
    GameObject Tiks;
    float CurrentNormalisedValue;
    float yPosition;
    float SideConeYPosition;
    float TopConeYPosition;
  
    // Use this for initialization

   public enum AxisEvent
    {
        Idle,
        SideConveMove,
        TopConeMove,
        DragAround,
    }

    public AxisEvent axisEvent = AxisEvent.Idle;

    void Start () {

        Axis = Instantiate(NewAxis); // create copy of prefab Axis 
        SetUpMenu();
        SetUpAxis();
       
       
        SceneObjects.transform.position =new Vector3(SceneObjects.transform.position.x, Axis.transform.Find("LowPos").position.y +.009f, SceneObjects.transform.position.z) ;
        Axis.transform.Find("SideCone").position = new Vector3(Axis.transform.Find("SideCone").position.x, ZeroYPosition, Axis.transform.Find("SideCone").position.z);
        
        //CreateTik();
        MenuControl();
    }

    private void MenuControl()
    {
        Toggle MenuToggle = Axis.transform.Find("VR_Menu/CanvasMenu/Toggle").GetComponent<Toggle>();
        Toggle ClickToggle = Axis.transform.Find("VR_Menu/CanvasClickPosition/Toggle").GetComponent<Toggle>();
        //Toggle DragToggle = Axis.transform.Find("CanvasAllMenu/CanvasDragPosition/Toggle").GetComponent<Toggle>();
        MenuToggle.onValueChanged.AddListener(value =>
        {

            if (value)
            {
                Axis.transform.Find("VR_Menu/CanvasMenu/Toggle/Background").GetComponent<Image>().color = Color.green;
            }
            else
            {
                Axis.transform.Find("VR_Menu/CanvasMenu/Toggle/Background").GetComponent<Image>().color = Color.white;
            }
        });



      
    }



    //private void CreateTik()
    //{
    //    Tiks = Axis.transform.Find("Tik").gameObject;

    //    Tiks.transform.position = new Vector3(Tiks.transform.position.x, ZeroYPosition, Tiks.transform.position.z);
    //}


    private void SetUpAxis()
    {
        CurrentConePosY = Axis.transform.Find("TopCone").position.y;
        Axis.transform.parent = SceneObjects.transform;
        Axis.transform.Find("Label").GetComponent<TextMesh>().text = _viz.yDimension.Attribute;
        Axis.transform.Find("Cylinder").localScale = new Vector3(Axis.transform.Find("Cylinder").localScale.x, _viz.yDimension.maxScale, Axis.transform.Find("Cylinder").localScale.z);
        Axis.transform.Find("SideCone").GetComponent<VRTK_InteractableObject>().InteractableObjectGrabbed += new InteractableObjectEventHandler(ObjectGrabbedSideCone);
        Axis.transform.Find("SideCone").GetComponent<VRTK_InteractableObject>().InteractableObjectUngrabbed += new InteractableObjectEventHandler(ObjectUnGrabbedSideCone);
        Axis.transform.Find("TopCone").GetComponent<VRTK_InteractableObject>().InteractableObjectGrabbed += new InteractableObjectEventHandler(ObjectGrabbedTopCone);
        Axis.transform.Find("TopCone").GetComponent<VRTK_InteractableObject>().InteractableObjectUngrabbed += new InteractableObjectEventHandler(ObjectUnGrabbedTopCone);
        Axis.transform.Find("VR_Menu/CanvasClickPosition").GetComponent<VRTK_InteractableObject>().InteractableObjectGrabbed += new InteractableObjectEventHandler(ObjectGrabbedMenu);
        Axis.transform.Find("VR_Menu/CanvasClickPosition").GetComponent<VRTK_InteractableObject>().InteractableObjectUngrabbed += new InteractableObjectEventHandler(ObjectUnGrabbedMenu);
    }
    private void SetUpMenu()
    {
        Axis.transform.Find("VR_Menu").GetComponent<VRMenuInteractor>().visualisation = _viz;
    //    floorSlider = Axis.transform.Find("Canvas/Panel/Container/GroundLevelSlider").GetComponent<Slider>();
   //     floorSlider.value = -0.5f;
 
    }


    // Change position of the axis on click 
    public void ChangePosition(Vector3 positionCurrent)
    {
     
            // providing position to y-axies 
    
      
            positionCurrent.y = positionCurrent.y + .85f;
            positionCurrent.x = positionCurrent.x + .3f;
           // positionCurrent.z = positionCurrent.z + -.3f;
            Axis.transform.position = positionCurrent;
            Axis.transform.rotation = Quaternion.Euler(new Vector3(0, headset.transform.eulerAngles.y, 0));
            axisEvent = AxisEvent.Idle;

    }
    // event listner when objects are grabbed   
    private void ObjectGrabbedTopCone(object sender, InteractableObjectEventArgs e)
    {
    //    IsChangeConePosition = true;
        axisEvent = AxisEvent.TopConeMove;
        TopConeYPosition = Axis.transform.Find("TopCone").position.y;

    }

    // event listner when objects are grabbed   
    private void ObjectUnGrabbedTopCone(object sender, InteractableObjectEventArgs e)
    {

        float CurrentTopConeYPosition = Axis.transform.Find("TopCone").position.y;
        float NewSideConePosition = ((CurrentTopConeYPosition* SideConeYPosition) / TopConeYPosition);
        Axis.transform.Find("SideCone").position = new Vector3(Axis.transform.Find("SideCone").position.x, NewSideConePosition, Axis.transform.Find("SideCone").position.z);

    }

       // event listner when objects are grabbed   
    private void ObjectGrabbedSideCone(object sender, InteractableObjectEventArgs e)
    {
        axisEvent = AxisEvent.SideConveMove;
    }

    // event listner when objects are grabbed   
    private void ObjectUnGrabbedSideCone(object sender, InteractableObjectEventArgs e)
    {

        SideConeYPosition = Axis.transform.Find("SideCone").position.y;
       
        IsChangeConePosition = false;

        axisEvent = AxisEvent.Idle;
    }

    // event listner when objects are grabbed   
    private void ObjectGrabbedMenu(object sender, InteractableObjectEventArgs e)
    {
     //   Axis.transform.Find("VR_Menu/CanvasClickPosition/Toggle/Background").GetComponent<Image>().color = Color.green;

        axisEvent = AxisEvent.DragAround;

    }

    // event listner when objects are grabbed   
    private void ObjectUnGrabbedMenu(object sender, InteractableObjectEventArgs e)
    {
     //    Axis.transform.Find("VR_Menu/CanvasClickPosition/Toggle/Background").GetComponent<Image>().color = Color.white;
    }


    void SideConveMove()
    {
        // get the Label on the custom y axis
        float MaxDataValue = Int32.Parse(_viz.dataSource.getOriginalValue(Data[_viz.yDimension.Attribute].Data.Max(), _viz.yDimension.Attribute).ToString());
        TopConeYPosition = Axis.transform.Find("TopCone").position.y;
        float SideConeYPosition = Axis.transform.Find("SideCone").position.y;
        float NormalisedValue = ((SideConeYPosition) / (TopConeYPosition - .13f));
        CurrentNormalisedValue = NormalisedValue * MaxDataValue;

        Axis.transform.Find("SideCone/Value").GetComponent<TextMesh>().text = Math.Round(CurrentNormalisedValue, 0).ToString();
        _viz.transform.Find("View/BigMesh").GetComponent<Renderer>().material.SetFloat("_cutoffHeight", NormalisedValue); // change the level of the shader by the smaller cone 
        Axis.transform.Find("Label").GetComponent<TextMesh>().text = _viz.yDimension.Attribute;// label name for custom y axis 
        // saving the position of zero value 
        if (CurrentNormalisedValue == 0f)
        {
            ZeroYPosition = Axis.transform.Find("SideCone").position.y;
        }
    }

    void TopConeMove()
    {


        // Extention of top cone 
        Vector3 LineVector = Axis.transform.Find("Cylinder").position - Axis.transform.Find("TopCone").position;
        if (LineVector.magnitude <= 0.6700001f)
        {
            if (CurrentConePosY != 0f)
            {
                Axis.transform.Find("TopCone").position = new Vector3(Axis.transform.Find("Cylinder").position.x, Axis.transform.Find("Cylinder").position.y + .67f, Axis.transform.Find("Cylinder").position.z);
            }
            _viz.height = Axis.transform.Find("TopCone").position.y;
        }
        else
        {
            Vector3 value = new Vector3(Axis.transform.Find("Cylinder").localScale.x, LineVector.magnitude, Axis.transform.Find("Cylinder").localScale.z);
            Axis.transform.Find("Cylinder").localScale = value;
            _viz.height = Axis.transform.Find("TopCone").position.y;
        }

    }

    void Idel()
    {
        Vector3 LineVector = Axis.transform.Find("Cylinder").position - Axis.transform.Find("TopCone").position;
        if (LineVector.magnitude <= 0.6700001f)
        {
            Axis.transform.Find("TopCone").position = new Vector3(Axis.transform.Find("Cylinder").position.x, Axis.transform.Find("Cylinder").position.y + .67f, Axis.transform.Find("Cylinder").position.z);
            Vector3 value = new Vector3(Axis.transform.Find("Cylinder").localScale.x, LineVector.magnitude, Axis.transform.Find("Cylinder").localScale.z);
            Axis.transform.Find("Cylinder").localScale = value;
        }
        _viz.height = Axis.transform.Find("TopCone").position.y;

    }

   private void DragYAxis()
    {
  //      Axis.transform.Find("VR_Menu/CanvasClickPosition/Toggle/Background").GetComponent<Image>().color = Color.green;


    }


    // Update is called once per frame
    void Update () {

        switch (axisEvent)
        {
            case AxisEvent.Idle:
                Idel();
                break;
            case AxisEvent.SideConveMove:
                SideConveMove();
                break;
            case AxisEvent.TopConeMove:
                TopConeMove();
                break;
            case AxisEvent.DragAround:
                DragYAxis();
                break;

        }
    }
}

   
