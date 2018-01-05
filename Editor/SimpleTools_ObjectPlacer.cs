using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace SimpleTools {
	public class SimpleTools_ObjectPlacer : EditorWindow {
		ObjectPlacementPrefab objectUsing;
		ObjectPlacementPrefab lastObjectUsing;
		bool placing = false;
		public List<GameObject> objectsPlaced = new List<GameObject>();
		// Use this for initialization
		void Start () {
			tex = new Texture2D (10,10);

		}
		void setFields(){
			toolRadius = objectUsing.toolRadius;
			placementDensity = objectUsing.placementDensity;
			placementAddition = objectUsing.placementAddition;
			normalOffset = objectUsing.normalOffset;
			randomizeNormalRotation = objectUsing.randomizeNormalAxisRotation;
			rotationOffset = objectUsing.rotationOffset;
			rotationOffset.x = objectUsing.rotationOffset.x;
			rotationOffset.y = objectUsing.rotationOffset.y;
			rotationOffset.z = objectUsing.rotationOffset.z;

		}

		void updatePrefab(){
			objectUsing.toolRadius = toolRadius;
			objectUsing.placementDensity = placementDensity;
			objectUsing.placementAddition = placementAddition;
			objectUsing.normalOffset = normalOffset;
			objectUsing.randomizeNormalAxisRotation =randomizeNormalRotation;
			objectUsing.rotationOffset = rotationOffset;
		}
	
		void OnFocus(){
			SceneView.onSceneGUIDelegate -= this.OnSceneGUI;
			SceneView.onSceneGUIDelegate += this.OnSceneGUI;
			placing = true;
		}


		[MenuItem( "Simple Tools/Place Objects", false, 1000 )]
		public static void openCustomTools(){
			var window = EditorWindow.GetWindow<SimpleTools_ObjectPlacer>();
			window.titleContent = new GUIContent("Place Objects");
		}
		int toolSelected = 0;
		float toolRadius = .5f;
		float placementDensity = 0;
		float placementAddition = 0;
		float normalOffset = 0;
		bool randomizeNormalRotation;
		Vector3 rotationOffset;
		Texture2D tex;
		float rx,ry,rz, no = 0;
		void OnGUI(){
			GUILayout.Label ("Using Object Placement");
			placing = GUILayout.Toggle (placing, tex);
			if (!placing)
				return;
			GUILayout.Label ("Radius: " + ((float)((int)(toolRadius * 100))/100).ToString());
			toolRadius = GUILayout.HorizontalSlider (toolRadius, .5f, 10);
			GUILayout.Space (20);

			string[] toolSelection = { "Draw", "Erase" };
			toolSelected = GUILayout.Toolbar(toolSelected,toolSelection);
			if (objectUsing) {
				EditorUtility.SetDirty (objectUsing);
			}
			if (toolSelected == 0) {
				GUILayout.Space (4);
				objectUsing = (ObjectPlacementPrefab)EditorGUILayout.ObjectField (objectUsing, typeof(ObjectPlacementPrefab), false);
				if (lastObjectUsing != objectUsing) {
					lastObjectUsing = objectUsing;
					setFields ();
				}
				GUILayout.Space (10);
				GUILayout.Label ("Placement Density: " + placementDensity.ToString ());
				placementDensity = GUILayout.HorizontalSlider (placementDensity, 0, 1);
				GUILayout.Space (10);

				GUILayout.Label ("Placement Addition: " + ((int)placementAddition).ToString ());
				placementAddition = GUILayout.HorizontalSlider (placementAddition, 0, 100);

				GUILayout.Space (10);
				GUILayout.BeginHorizontal ();
				GUILayout.Label ("Normal Offset");
				normalOffset = EditorGUILayout.FloatField (normalOffset);


				GUILayout.EndHorizontal ();

				GUILayout.Space (10);
				GUILayout.BeginHorizontal ();
				GUILayout.Label ("Rotation Offset");
				rotationOffset.x = EditorGUILayout.FloatField (rotationOffset.x);
				rotationOffset.y = EditorGUILayout.FloatField (rotationOffset.y);
				rotationOffset.z = EditorGUILayout.FloatField (rotationOffset.z);

					
				GUILayout.EndHorizontal ();

				GUILayout.Space (10);
				GUILayout.Label ("Randomize Normal-Axis Rotation");
				randomizeNormalRotation = GUILayout.Toggle (randomizeNormalRotation, tex);
			}

			if (toolSelected == 1) {
				GUILayout.Label ("Click or Drag to erase instantiated objects");
			}
			if (objectUsing) {
				updatePrefab ();
			}
		}

		void Update(){
			placementTime -= Time.deltaTime;
		}

		RaycastHit hitInfo;
		float placementTime;
		void OnSceneGUI(SceneView sceneView){
			if (placing) {
				HandleUtility.AddDefaultControl (GUIUtility.GetControlID (FocusType.Passive));
			} else {
				HandleUtility.Repaint ();
				return;
			}

			hitInfo = new RaycastHit ();
			if (Physics.Raycast (HandleUtility.GUIPointToWorldRay (Event.current.mousePosition),out hitInfo, 1000000)) {
				if (toolSelected == 0) {
					Handles.color = new Color (0, 1, 0, .3f);

				} else {
					Handles.color = new Color (1, 0, 0, .3f);
				}
				Handles.DrawSolidArc (hitInfo.point + hitInfo.normal / 100, hitInfo.normal,Vector3.Cross(hitInfo.normal,Vector3.right), 360, toolRadius);
			}

			if (Event.current.type == EventType.MouseDown && Event.current.button == 0) {
				if (toolSelected == 0) {
					placeObject ();
				}
				if (toolSelected == 1) {
					eraseObject ();
				}
			}

			if (Event.current.type == EventType.mouseDrag && Event.current.button == 0) {
				if (toolSelected == 1) {
					
					eraseObject ();
				}
				if (toolSelected == 0) {
					if (placementTime < 0) {
						placeObject ();
					}
				}
			}

		}
		GameObject temp;
		void eraseObject(){
			for (int i = 0; i < objectsPlaced.Count; i++) {
				if (objectsPlaced [i] != null) {
					if (Vector3.Distance (objectsPlaced [i].transform.position, hitInfo.point) <= toolRadius) {
						temp = objectsPlaced [i];
						objectsPlaced.RemoveAt (i);
						DestroyImmediate (temp);
					}
				}
			}


		}
		void placeObject(){
			placementTime = .3f;
			if (placementDensity == 0) {
				createObj (hitInfo.point, Quaternion.LookRotation (hitInfo.normal), hitInfo.normal);

			} else {
				for (int i = 0; i < Mathf.Pow (placementDensity * toolRadius, 2) + (int)placementAddition; i++) {
					Vector3 center = hitInfo.point;
					Vector3 normal = hitInfo.normal;

					Vector3 rand = Random.insideUnitSphere * toolRadius;
					RaycastHit newHit = new RaycastHit ();
				
					if (Physics.Raycast (new Ray(center + rand + normal * toolRadius * 1.01f,-normal), out newHit, toolRadius)) {
						
						createObj (newHit.point, Quaternion.LookRotation (newHit.normal), newHit.normal);
					}
				}
			}
		}
		void createObj(Vector3 pos, Quaternion rot, Vector3 normal){
			GameObject newObject = (GameObject)PrefabUtility.InstantiatePrefab (objectUsing.prefab, UnityEngine.SceneManagement.SceneManager.GetActiveScene ());
			newObject.transform.position = pos + normal * normalOffset;
			newObject.transform.rotation = rot * Quaternion.Euler(rotationOffset);
			if (randomizeNormalRotation) {
				newObject.transform.RotateAround (newObject.transform.position, normal, Random.Range (0, 360));
			}
			objectsPlaced.Add (newObject);	
		}
	}
}
