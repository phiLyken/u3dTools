using UnityEngine;
using System.Collections;
using UnityEditor;

 

public class AnyTools : EditorWindow{
	bool loaded;
    bool keepRotation = false;
    
	string assignName = "newName";
    string parenName = "parentName";
	bool numerate;
    Vector2 ScrollPos;
	GameObject rotateTo = null;
	Axis selectedAxis = Axis.y;
	string prefix = "enter prefix";
	string suffix = "enter suffix";
	
	public enum Axis {
		x, y, z	
	}
	
	[MenuItem ("WOLGame/AnyTools")]
		static void Init () {
			AnyTools window = (AnyTools)EditorWindow.GetWindow(typeof(AnyTools));
		}
	void OnGUI () {
      
        ScrollPos = EditorGUILayout.BeginScrollView(ScrollPos, false, true);
        Transform[] all = Selection.transforms;

        RotateToward(all);		
	
        AssignPrefix(all);		

        AssignSuffix(all);
	
        BatchRename(all);		
	
        NameForPlatform(all);

        CreateParentAtPosition(all);

        EditorGUILayout.EndScrollView();    
	}

    private void RotateToward(Transform[] all)
    {
        GUILayout.Label("Rotate Towards", EditorStyles.boldLabel);

        GUILayout.Label("Selected: " + all.Length);

        GUILayout.Label("\n Target");

        rotateTo = (GameObject)EditorGUILayout.ObjectField(rotateTo, typeof(GameObject), true);


        GUILayout.Label("\n");
        selectedAxis = (Axis)EditorGUILayout.EnumPopup("Axis up", selectedAxis);

        GUILayout.Label("\n");
        if (GUILayout.Button("Rotate to" + ((rotateTo == null) ? "Root" : " Selected Object") + "Normal"))
        {
            for (int i = 0; i < all.Length; i++)
            {
                RotateObjectToWheelNormal(all[i], rotateTo != null ? rotateTo.transform.position : all[i].root.position, selectedAxis);
            }
        }
    }

    private void AssignPrefix(Transform[] all)
    {
        GUILayout.Label("\n");
        GUILayout.Label("Give Prefix", EditorStyles.boldLabel);
        prefix = EditorGUILayout.TextField("prefix (without _ )", prefix);

        if (GUILayout.Button("Assign"))
        {

            for (int i = 0; i < all.Length; i++)
            {

                if (all[i].name.IndexOf('_') < 0)
                {
                    all[i].name = prefix + "_" + all[i].name;
                    Debug.Log(all[i].name);

                }
                else
                {

                    string[] n = all[i].name.Split('_');
                    string newName = prefix + "_";
                    Debug.Log(n.Length + "   " + newName);
                    for (int j = 1; j < n.Length; j++)
                    {
                        newName += n[j];

                    }
                    all[i].name = newName;

                }
            }
        }
    }

    private void AssignSuffix(Transform[] all)
    {
        GUILayout.Label("\n");
        GUILayout.Label("Give Suffix", EditorStyles.boldLabel);
        prefix = EditorGUILayout.TextField("suffix (without _ )", suffix);

        if (GUILayout.Button("Assign"))
        {

            for (int i = 0; i < all.Length; i++)
            {

                if (all[i].name.IndexOf('_') < 0)
                {
                    all[i].name = all[i].name + "_" + suffix;
                    Debug.Log(all[i].name);

                }
                else
                {

                    string[] n = all[i].name.Split('_');
                    string newName = "";
                    Debug.Log(n.Length + "   " + newName);
                    for (int j = 0; j < n.Length - 1; j++)
                    {
                        newName += n[j];

                    }
                    all[i].name = newName + "_" + suffix;

                }
            }
        }


    }

    private void BatchRename(Transform[] all)
    {
        GUILayout.Label("\n");
        GUILayout.Label("Assign Name", EditorStyles.boldLabel);
        assignName = EditorGUILayout.TextField("New Name", assignName);
        numerate = EditorGUILayout.Toggle("Numerate", numerate);

        if (GUILayout.Button("Give New Name"))
        {
            for (int i = 0; i < all.Length; i++)
            {
                all[i].name = assignName;
                if (numerate) all[i].name += i.ToString();
            }
        }
    }

    void NameForPlatform(Transform[] gos)
    {
        if (GUILayout.Button("Name for Platform"))
        {
            for (int i = 0; i < gos.Length; i++)
            {
                gos[i].name = gos[i].name + "_" + WheelPartEditor.platformIdentifier;
            }
        }
    }

    void CreateParentAtPosition(Transform[] gos)
    {
        GUILayout.Label("Create Parent At Position", EditorStyles.boldLabel);
        parenName = EditorGUILayout.TextField("ParentName", parenName);
        keepRotation = EditorGUILayout.Toggle("Keep Rotation", keepRotation);

        if (GUILayout.Button("Create"))
        {
            for (int i = 0; i < gos.Length; i++)
            {
                GameObject parent = new GameObject(parenName);
                parent.transform.position = gos[i].position;
                if (keepRotation)
                {
                    parent.transform.rotation = gos[i].rotation;
                    parent.transform.rotation = Quaternion.identity;
                }

                gos[i].parent = parent.transform;
            }
        }
       
    }

	public static void RotateObjectToWheelNormal(Transform tr, Vector3 wheelPartPos, Axis a){
			
			Vector3 wheelNormal = (wheelPartPos- tr.position).normalized;
			Quaternion rot = Quaternion.FromToRotation(a == Axis.y ? tr.up : a == Axis.x ? tr.right : tr.forward,wheelNormal);
			tr.rotation = rot * tr.rotation;
		
		
	}
	
	
}
