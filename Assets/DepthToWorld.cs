using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepthToWorld : MonoBehaviour {

    public Material depthToWorld;

	// Use this for initialization
	void Start () {
        GetComponent<Camera>().depthTextureMode = DepthTextureMode.Depth;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, depthToWorld);
    }
}
