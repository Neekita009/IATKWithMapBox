# IATKWithMapBox
  ⦁	VRTK_SDKManager

  ⦁	VRTK_Script

  ⦁	EventSystem

  ⦁	DirectionalLight

  ⦁	HeadSet -Empty GameObject. Add Script
  
      ⦁	VRTK_SDKObjectAlias
              ⦁	SDK Object - Headset

  ⦁	MapInteraction - Empty GameObject. Add following Script to the object
  
     ⦁	AbstractMapInteraction
     ⦁	AbstractMapInteractionVRTK
                  ⦁	YAxis - ProjectAssets/Script/CreateCustomYAxis
                  ⦁	AbstractMap- Map form the scene
                  ⦁	Controller Event- RightControllerScriptAlias(VRTK_ControllerEvent)
                  ⦁	PointerEvent- RightControllerScriptAlias(VRTK_Pointer)	

  ⦁	SceneObjects - Empty GameObject

      ⦁	DataSource -IATK
                  ⦁	Data - - ProjectAssets/Dataset/BB_VR_Data01_update

      ⦁	MainVisualisation - IATK
                  ⦁	DataScource -DataSource form the scene
                  ⦁	X_Axis - Longitute_met
                  ⦁	Y_Axis - Some Other Attribute 
                  ⦁	Z_Axis-Latitude_met
                  ⦁	Geometry-bars
                  
      ⦁	Map - MapBox. Add following Script 
                  ⦁	SpawnOnMap 
                          ⦁	Map - Map form the scene
                          ⦁	Viz - MainVisulization - IATK form the scene
                          ⦁	MySourceData - DataSource form the scene
                  ⦁	CreateCustomeYAxis	
                          ⦁	NewAxis - - ProjectAssets/Prefab/CustomYAxis
                          ⦁	Viz- MainVisualization form the scene
                          ⦁	SceneObjects - Entire Gameobject (SceneObjects)
                          ⦁	HeadSet - HeadSet GameObject form the Scene
                          ⦁	Data - DataSource form the scene
                          ⦁	AxisEvent - Idle
                          
      ⦁	BaseFloor - (Name has been hard coded in the script). Component added
                  ⦁	cube
                  ⦁	Box Collider
                  ⦁	Mesh Renderer
                  
      ⦁	Brushing and linking - IATK
                  ⦁	BrushingVisualization - MainVisualization form the scene
                  ⦁	Input1 - VRTK_SDKManager/VRTK_SDKSetUP/StreamVR/CameraRig/Controller(right)
                  ⦁	Input2- VRTK_SDKManager/VRTK_SDKSetUP/StreamVR/CameraRig/Controller(right)
                  ⦁	Brush_type - SPHERE
                  
      ⦁	CreateClones- Empty Gameobject - Add following Script 
                  ⦁	CreatePrefabs
                          ⦁	MenuPrefab- - ProjectAssets/Prefab/VRMenuForGraph
                          ⦁	VIzPrefab - - ProjectAssets/Prefab/VisualizationPrefab
                          ⦁	Data - DataSource form the scene
                          ⦁	MainMapViz - MainVisualization form the scene
                          ⦁	Cube-  ProjectAssets/Prefab/CubeContainer
                          ⦁	NumberOfGraph- 2 default - user input
                          ⦁	BL- Brushing and Linking from the Scene 
