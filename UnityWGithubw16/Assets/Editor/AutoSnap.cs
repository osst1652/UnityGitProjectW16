using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AutoSnap : EditorWindow {

    private Vector3 previous_position;
    private bool doSnap = true;
    private float snapValue = 1;
    private bool initial = false;
    [MenuItem("Edit/Auto Snap%_1")]
    static void Init()
    {
        var window = (AutoSnap)EditorWindow.GetWindow(typeof(AutoSnap));
        window.maxSize = new Vector2(200, 100);
    }

	// Use this for initialization
	void Start () {
		
	}
    private float Round( float input)
    {
        return snapValue * Mathf.Round((input / snapValue));
    }
    private void Snap()
    {
        foreach( var transform in Selection.transforms)
        {
            var tr = transform.transform.position;

            tr.x = Round(tr.x);
            tr.y = Round(tr.y);
            tr.z = Round(tr.z);
            transform.transform.position = tr;
        }
    }

    public void SceneGUI(SceneView sceneView)
    {
        if(doSnap && !EditorApplication.isPlaying && Selection.transforms.Length >0 && Selection.transforms[0].position != previous_position)
        {
            Snap();
            previous_position = Selection.transforms[0].position;
        }
    }
    public void OnGUI()
    {
        if (!initial)
        {
            SceneView.onSceneGUIDelegate += SceneGUI;
        }
    }

}
