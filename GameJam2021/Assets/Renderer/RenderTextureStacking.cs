using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class RenderTextureStacking : MonoBehaviour
{
    public Texture[] CameraRenders;
    public RenderTexture[] FinalRenders;

    void Update()
    {
        for (int i = 0; i < CameraRenders.Length; i++)
        {
            Graphics.CopyTexture(CameraRenders[i], FinalRenders[i]);
        }
    }
}

