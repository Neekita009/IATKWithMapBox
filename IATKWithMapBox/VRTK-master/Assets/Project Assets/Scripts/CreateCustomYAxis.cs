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
    private float ZeroYPosition;
    private float CurrentNormalisedValue;
    private float PrevSideConeYPosition;
    private float PrevTopConeYPosition;
      
  


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
        
        
    }

    private void CreateTick()
    {
       
        // calculating the distance from the ground to the top cone 
        float DistanceGroundToTopCone = Vector3.Distance(Axis.transform.Find("LowPos").position, Axis.transform.Find("TopCone").position);
       
        GameObject TicksMin = Axis.transform.Find("TicksMin").gameObject;

        //Assign value of TickMax to the Sidecone position of 0
        TicksMin.transform.position = new Vector3(TicksMin.transform.position.x, ZeroYPosition, TicksMin.transform.position.z);
        GameObject TicksMax = Axis.transform.Find("TicksMax").gameObject;

        TicksMax.transform.position = new Vector3(TicksMax.transform.position.x, DistanceGroundToTopCone -.075f, TicksMax.transform.position.z);
        float mid = ((DistanceGroundToTopCone - .075f) + ZeroYPosition) / 2;
        GameObject TicksMid = Axis.transform.Find("TicksMid").gameObject;
        TicksMid.transform.position = new Vector3(TicksMid.transform.position.x, mid, TicksMid.transform.position.z);
       
        _viz.height = TicksMax.transform.position.y + 0.055f;

        //half of half 

        

        // bottom
        float BottomHalf = (mid + ZeroYPosition) / 2;
        float BottomHalfFirst = (BottomHalf + ZeroYPosition) / 2;
        float BottomHalfSecond = (BottomHalf + mid) / 2;

        GameObject BottomHalfGO = Axis.transform.Find("TicksBottomHalf").gameObject;
        BottomHalfGO.transform.position = new Vector3(BottomHalfGO.transform.position.x, BottomHalf, BottomHalfGO.transform.position.z);

        GameObject BottomHalfFirstGO = Axis.transform.Find("TicksBottomHalfFirst").gameObject;
        BottomHalfFirstGO.transform.position = new Vector3(BottomHalfFirstGO.transform.position.x, BottomHalfFirst, BottomHalfFirstGO.transform.position.z);

        GameObject BottomHalfSecondGO = Axis.transform.Find("TicksBottomHalfSecond").gameObject;
        BottomHalfSecondGO.transform.position = new Vector3(BottomHalfSecondGO.transform.position.x, BottomHalfSecond, BottomHalfSecondGO.transform.position.z);

        // Top
        float TopHalf = (mid + (DistanceGroundToTopCone - .075f)) / 2;
        float TopHalfFirst = (TopHalf + (DistanceGroundToTopCone - .075f)) / 2;
        float TopHalfSecond = (TopHalf + mid) / 2;
        GameObject TopHalfGO = Axis.transform.Find("TicksTopHalf").gameObject;
        TopHalfGO.transform.position = new Vector3(TopHalfGO.transform.position.x, TopHalf, TopHalfGO.transform.position.z);

        GameObject TopHalfFirstGO = Axis.transform.Find("TicksTopHalfFirst").gameObject;
        TopHalfFirstGO.transform.position = new Vector3(TopHalfFirstGO.transform.position.x, TopHalfFirst, TopHalfFirstGO.transform.position.z);
        GameObject TicksTopHalfSecondGO = Axis.transform.Find("TicksTopHalfSecond").gameObject;
        TicksTopHalfSecondGO.transform.position = new Vector3(TicksTopHalfSecondGO.transform.position.x, TopHalfSecond, TicksTopHalfSecondGO.transform.position.z);
    }


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
        Axis.transform.rotation = Quaternion.Euler(new Vector3(0, headset.transform.eulerAngles.y, 0));
    }
    private void SetUpMenu()
    {
        Axis.transform.Find("VR_Menu").GetComponent<VRMenuInteractor>().visualisation = _viz;
        Axis.transform.Find("VR_Menu/CanvasClickPosition").GetComponent<VRTK_InteractableObject>().InteractableObjectGrabbed += new InteractableObjectEventHandler(ObjectGrabbedMenu);
        Axis.transform.Find("VR_Menu/CanvasLabel").GetComponent<VRTK_InteractableObject>().InteractableObjectGrabbed += new InteractableObjectEventHandler(ObjectGrabbedMenu);
        
        Toggle MenuToggle = Axis.transform.Find("VR_Menu/CanvasMenu/Toggle").GetComponent<Toggle>();

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


    // Change position of the axis on click 
    public void ChangePosition(Vector3 positionCurrent)
    {
     
            // providing position to y-axies 
            positionCurrent.y = positionCurrent.y + .85f;
            positionCurrent.x = positionCurrent.x + .3f;
            Axis.transform.position = positionCurrent;
            Axis.transform.rotation = Quaternion.Euler(new Vector3(0, headset.transform.eulerAngles.y, 0));
        if (Axis.transform.Find("SideCone").position.y < ZeroYPosition)
        {
            Axis.transform.Find("SideCone").position = new Vector3(Axis.transform.Find("SideCone").position.x, ZeroYPosition, Axis.transform.Find("SideCone").position.z);
        }
    

    }
    // event listner when objects are grabbed   
    private void ObjectGrabbedTopCone(object sender, InteractableObjectEventArgs e)
    {
        axisEvent = AxisEvent.TopConeMove;
        PrevTopConeYPosition = Axis.transform.Find("TopCone").position.y;

    }

    // event listner when objects are grabbed   
    private void ObjectUnGrabbedTopCone(object sender, InteractableObjectEventArgs e)
    {
        SidConeRePosition();
        
    }


    private void SidConeRePosition()
    {
        float CurrentTopConeYPosition = Axis.transform.Find("TopCone").position.y;
        float NewSideConePosition = (((CurrentTopConeYPosition - .13f) * PrevSideConeYPosition) / (PrevTopConeYPosition - .13f));
        if (NewSideConePosition < CurrentTopConeYPosition)
        {

            Axis.transform.Find("SideCone").position = new Vector3(Axis.transform.Find("SideCone").position.x, NewSideConePosition, Axis.transform.Find("SideCone").position.z);
            PrevSideConeYPosition = NewSideConePosition;

        }

    }
       // event listner when objects are grabbed   
    private void ObjectGrabbedSideCone(object sender, InteractableObjectEventArgs e)
    {
        axisEvent = AxisEvent.SideConveMove;
    }

    // event listner when objects are grabbed   
    private void ObjectUnGrabbedSideCone(object sender, InteractableObjectEventArgs e)
    {

        PrevSideConeYPosition = Axis.transform.Find("SideCone").position.y;
       
         axisEvent = AxisEvent.Idle;
    }

    // event listner when objects are grabbed   
    private void ObjectGrabbedMenu(object sender, InteractableObjectEventArgs e)
    {
        axisEvent = AxisEvent.DragAround;

    }



    void SideConveMove()
    {
        // get the Label on the custom y axis
        CreateAxisLabel();
        // saving the position of zero value 
        if (CurrentNormalisedValue == 0f)
        {
            ZeroYPosition = Axis.transform.Find("SideCone").position.y;
        }




    }

    private void CreateAxisLabel()
    {

        float MaxDataValue = Int32.Parse(_viz.dataSource.getOriginalValue(Data[_viz.yDimension.Attribute].Data.Max(), _viz.yDimension.Attribute).ToString());
        PrevTopConeYPosition = Axis.transform.Find("TopCone").position.y;
        float SideConeYPosition = Axis.transform.Find("SideCone").position.y;
        float NormalisedValue = ((SideConeYPosition) / (PrevTopConeYPosition - .13f));
        CurrentNormalisedValue = NormalisedValue * MaxDataValue;

        Axis.transform.Find("SideCone/Value").GetComponent<TextMesh>().text = Math.Round(CurrentNormalisedValue, 0).ToString();
        _viz.transform.Find("View/BigMesh").GetComponent<Renderer>().sharedMaterial.SetFloat("_cutoffHeight", NormalisedValue); // change the level of the shader by the smaller cone 
        Axis.transform.Find("Label").GetComponent<TextMesh>().text = _viz.yDimension.Attribute;// label name for custom y axis 
       
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
        
        }
        else
        {
            Vector3 value = new Vector3(Axis.transform.Find("Cylinder").localScale.x, LineVector.magnitude, Axis.transform.Find("Cylinder").localScale.z);
            Axis.transform.Find("Cylinder").localScale = value;
       
        }
        CreateTick();
        SidConeRePosition();
        PrevTopConeYPosition = Axis.transform.Find("TopCone").position.y;
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

        CreateTick();
        CreateAxisLabel();
    }




    // Update is called once per frame
    void Update () {
        float DistanceGroundToTopCone = Vector3.Distance(Axis.transform.Find("LowPos").position, Axis.transform.Find("TopCone").position);
        if (Axis.transform.Find("SideCone").position.y >= (DistanceGroundToTopCone - .08f))
        {
            Axis.transform.Find("SideCone").position = new Vector3(Axis.transform.Find("SideCone").position.x, DistanceGroundToTopCone - .08f, Axis.transform.Find("SideCone").position.z);
        }
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
           

        }
    }
}

   
