using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class PngTexTest : MonoBehaviour
{
    public Texture2D oldTex;

    public Texture2D newTex;

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

            if (keyCode == KeyCode.I)
            {
                CompareTexture();
            }

        }
    }

    void CompareTexture()
    {
        if (oldTex == null)
            return;

        if (newTex == null)
            return;

        var oldColors = oldTex.GetPixels32();
        var newColors = newTex.GetPixels32();

        bool find = false;

        for (int i = 0; i < newColors.Length; i++)
        {
            var oldColor = oldColors[i];
            var newColor = newColors[i];

            if (oldColor.r != newColor.r ||
                oldColor.g != newColor.g ||
                oldColor.b != newColor.b ||
                oldColor.a != newColor.a)
            {
                find = true;
                Debug.LogError("ÐÞ¸´ÑÕÉ«ÓÐ²îÒì : " + i);
            }
        }

        if (!find)
        {
            Debug.LogError("ÌùÍ¼Ò»ÖÂ ");
        }
    }

    void ModifyTexture()
    {
        if (oldTex == null || newTex == null)
            return;

        var oldAssetPath = AssetDatabase.GetAssetPath(oldTex);
        var absOldAssetPath = AssetsPath2ABSPath(oldAssetPath);
        var bytes = File.ReadAllBytes(absOldAssetPath);

        if (bytes != null)
        {
            Texture2D neighborTex = new Texture2D(oldTex.width, oldTex.height, TextureFormat.RGBA32, false);
            var isSuccess = neighborTex.LoadImage(bytes);
            if (!isSuccess)
            {
                Debug.LogError("RepairTextureMgr.AddTextureCommon Texture2D.LoadImage ´íÎó");
            }

            neighborTex.Apply();

            var newAssetPath = AssetDatabase.GetAssetPath(newTex);
            var absTexPath = AssetsPath2ABSPath(newAssetPath);
            File.WriteAllBytes(absTexPath, neighborTex.EncodeToPNG());
            AssetDatabase.SaveAssets();
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
