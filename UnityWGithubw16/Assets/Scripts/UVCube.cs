using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UVCube : MonoBehaviour {
    private MeshFilter meshF;
    public float tileSize = 0.25f;

	// Use this for initialization
	void Start () {

        ApplyTexture();
		
	}
	
	// Update is called once per frame
	public void ApplyTexture() {
        meshF = gameObject.GetComponent<MeshFilter>();
        if (meshF)
        {

            Mesh mesh = meshF.sharedMesh;
            if (mesh)
            {
                Vector2[] uvs = mesh.uv;

                //Frontside
                uvs[0] = new Vector2(0f, 0f);       //bottom left
                uvs[1] = new Vector2(tileSize, 0f); //bottom right
                uvs[2] = new Vector2(0f, 1f);       //top left
                uvs[3] = new Vector2(tileSize, 1f); //top right

                //Right side
                uvs[20] = new Vector2(tileSize * 1.001f, 0f);       //bottom left
                uvs[22] = new Vector2(tileSize * 2.001f, 0f);       //bottom right
                uvs[23] = new Vector2(tileSize * 1.001f, 1f);       //top left
                uvs[21] = new Vector2(tileSize * 2.001f, 1f);       //top right


                //Backside
                uvs[10] = new Vector2(tileSize * 2.001f, 1f);       //bottom left
                uvs[11] = new Vector2(tileSize * 3.001f, 1f);       //bottom right
                uvs[6] = new Vector2(tileSize * 2.001f, 1f);       //top left
                uvs[7] = new Vector2(tileSize * 3.001f, 1f);       //top right

                //Left side
                uvs[16] = new Vector2(tileSize * 3.001f, 0f);       //bottom left
                uvs[18] = new Vector2(tileSize * 4.001f, 0f);       //bottom right
                uvs[19] = new Vector2(tileSize * 3.001f, 1f);       //top left
                uvs[17] = new Vector2(tileSize * 4.001f, 1f);       //top right

                //Up
                uvs[8] = new Vector2(tileSize * 4.001f, 0f);       //bottom left
                uvs[9] = new Vector2(tileSize * 5.001f, 0f);       //bottom right
                uvs[4] = new Vector2(tileSize * 4.001f, 1f);       //top left
                uvs[5] = new Vector2(tileSize * 5.001f, 1f);       //top right

                //down
                uvs[12] = new Vector2(tileSize * 5.001f, 0f);       //bottom left
                uvs[14] = new Vector2(tileSize * 6.001f, 0f);       //bottom right
                uvs[15] = new Vector2(tileSize * 5.001f, 1f);       //top left
                uvs[13] = new Vector2(tileSize * 6.001f, 1f);       //top right

                mesh.uv = uvs;
            }
        }
        else
        {
            Debug.Log("No mesh filter found or attached");
        }
	}
}
