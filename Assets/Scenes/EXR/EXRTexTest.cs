using gtmEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR

[ExecuteInEditMode]
public class EXRTexTest : MonoBehaviour
{
    Texture tex;

    // Start is called before the first frame update
    void OnEnable()
    {
        SceneView.duringSceneGui -= OnSceneGui;
        SceneView.duringSceneGui += OnSceneGui;
    }

    void OnSceneGui(SceneView sceneView)
    {
        bool isKeyUp = Event.current.type == EventType.KeyUp;
        if (isKeyUp)
        {
            var evt = Event.current;
            KeyCode keyCode = evt.keyCode;
            if (keyCode == KeyCode.O)
            {
                ModifyTexture();
            }
        }
    }

    void SetTexture(Texture tex)
    {
        var render = gameObject.GetComponentInChildren<MeshRenderer>();
        if (render != null)
        {
            var mat = render.sharedMaterial;
            if (mat != null)
            {
                mat.mainTexture = tex;
            }
        }
    }

    Texture GetTexture()
    {
        Texture tex = null;

        var render = gameObject.GetComponentInChildren<MeshRenderer>();
        if (render != null)
        {
            var mat = render.sharedMaterial;
            if (mat != null)
            {
                tex = mat.mainTexture;
            }
        }

        return tex;
    }

    void ModifyTexture()
    {
        tex = GetTexture();

        var assetPath = AssetDatabase.GetAssetPath(tex);
        var absAssetPath = AssetsPath2ABSPath(assetPath);
        int width = tex.width;
        int height = tex.height;

        EXRTexture2D exrtex = new EXRTexture2D(width, height);
        exrtex.Load(absAssetPath);

        for (int i = 0; i < 30; i++)
        {
            for (int j = 0; j < height; j++)
            {
                exrtex.SetPixel(i, j, Color.black);
            }
        }

        exrtex.Save(absAssetPath);

        AssetDatabase.Refresh();

        var newtex = AssetDatabase.LoadAssetAtPath<Texture>(assetPath);
        if (newtex != null)
        {
            SetTexture(newtex);
        }
    }

    public string AssetsPath2ABSPath(string assetsPath)
    {
        string assetRootPath = System.IO.Path.GetFullPath(Application.dataPath);
        return assetRootPath.Substring(0, assetRootPath.Length - 6) + assetsPath;
    }

    public string ABSPath2AssetsPath(string absPath)
    {
        string assetRootPath = System.IO.Path.GetFullPath(Application.dataPath);
        return "Assets" + System.IO.Path.GetFullPath(absPath).Substring(assetRootPath.Length).Replace("\\", "/");
    }
}

#endif
