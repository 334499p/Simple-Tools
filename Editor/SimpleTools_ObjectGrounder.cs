using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace SimpleTools {
	public class SimpleTools_ObjectGrounder : EditorWindow {



		int toolSelected = 0;
		bool rotateTowardsNormal;
		float normalOffset = 0;
		bool randomizeNormalRotation;
		Vector3 rotationOffset;
		Texture2D tex;
		int layerMask;
		public static List<GameObject> originalObjects = new List<GameObject>();

		public static List<Vector3> originalPositions = new List<Vector3>();
		public static List<Quaternion> originalRotations = new List<Quaternion>();
		public GroundObjectsData groundObjectsData;


		[MenuItem( "Simple Tools/Ground Objects", false, 1001 )]
		public static void openCustomTools(){
			var window = EditorWindow.GetWindow<SimpleTools_ObjectGrounder>();
			window.titleContent = new GUIContent("Ground Objects");
	
		}

		void OnFocus(){
			tex = new Texture2D (10,10);
			groundObjectsData = (GroundObjectsData)AssetDatabase.LoadMainAssetAtPath ("Assets/Simple Tools/Ground Objects/GroundObjectsData.asset");
			normalOffset = groundObjectsData.offset;
			rotationOffset = groundObjectsData.rotationOffset;
			randomizeNormalRotation = groundObjectsData.randomizeRotation;
			rotateTowardsNormal = groundObjectsData.rotateTowardsNormal;
		}
		void OnGUI(){
			if (groundObjectsData != null) {
				EditorUtility.SetDirty (groundObjectsData);
			}
			GUILayout.Space (10);
			GUILayout.Label ("Move all selected objects down to the ground");
				GUILayout.Space (10);
				GUILayout.BeginHorizontal ();
			if (rotateTowardsNormal) {
				GUILayout.Label ("Normal Offset");
			} else {
				GUILayout.Label ("Vector3.up Offset");

			}
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
			if (rotateTowardsNormal) {
				GUILayout.Label ("Randomize Rotation Around Normal");
			} else {
				GUILayout.Label ("Randomize Rotation Around Vector3.up");

			}				
			randomizeNormalRotation = GUILayout.Toggle (randomizeNormalRotation, tex);

			GUILayout.Space (10);
			GUILayout.Label ("Rotate Towards Normal");
			rotateTowardsNormal = GUILayout.Toggle (rotateTowardsNormal, tex);

			GUILayout.Space (10);

			GUILayout.Space (5);
			if(GUILayout.Button("Ground "+Selection.gameObjects.Length.ToString() + " Objects")){
				groundObjects (Selection.gameObjects,rotateTowardsNormal,normalOffset,rotationOffset,randomizeNormalRotation);

			}
			if (originalObjects.Count != 0) {
				if (GUILayout.Button ("Undo")) {
					int i = 0;
					foreach (GameObject g in originalObjects) {
						g.transform.position = originalPositions [i];
						g.transform.rotation = originalRotations [i];
						i++;
					}
					originalPositions.Clear ();
					originalRotations.Clear ();
					originalObjects.Clear ();
				}
			}
			groundObjectsData.offset = normalOffset;
			groundObjectsData.rotationOffset = rotationOffset;
			groundObjectsData.randomizeRotation = randomizeNormalRotation;
			groundObjectsData.rotateTowardsNormal = rotateTowardsNormal;
		}

		public static void groundObjects(GameObject[] objects, bool rotateToNormal, float normOffset, Vector3 rotOffset,bool randNormalRotation){
			foreach (GameObject g in objects) {
				originalPositions.Add (g.transform.position);
				originalRotations.Add (g.transform.rotation);
				originalObjects.Add (g);

				Collider[] cols = g.GetComponentsInChildren<Collider> ();
				foreach (Collider c in cols) {
					c.enabled = false;
				}
				RaycastHit hitInfo = new RaycastHit ();

				if (Physics.Raycast (new Ray (g.transform.position, Vector3.down), out hitInfo, 10000)) {
					g.transform.position = hitInfo.point;
					Vector3 n = rotateToNormal ? hitInfo.normal : Vector3.up;
					g.transform.position += n * normOffset;
					if (rotateToNormal) {
						g.transform.rotation = Quaternion.LookRotation (n);
					}
					g.transform.rotation *= Quaternion.Euler (rotOffset);
					if (randNormalRotation) {
						g.transform.RotateAround (g.transform.position, n, Random.Range (0, 360));
					}
				}

				foreach (Collider c in cols) {
					c.enabled = true;
				}
			}
		}
	}


}

