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

    private void OnGUI()
    {
        GUIStyle fontStyle = new GUIStyle();
        fontStyle.normal.textColor = Color.white;
        fontStyle.fontSize = 40;

        GUI.Label(new Rect(0, 0, 300, 300), "��O����ȡд����ͼ, ��I���Ƚ���ͼ����", fontStyle);
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
        if (oldTex == null || newTex == null)
            return;

        var oldAssetPath = AssetDatabase.GetAssetPath(oldTex);
        var absOldAssetPath = AssetsPath2ABSPath(oldAssetPath);

        var newAssetPath = AssetDatabase.GetAssetPath(newTex);
        var absNewAssetPath = AssetsPath2ABSPath(newAssetPath);


        PngTexture oldPngTex = new PngTexture();
        PngTexture newPngTex = new PngTexture();

        oldPngTex.Load(absOldAssetPath);
        newPngTex.Load(absNewAssetPath);

        var oldColors = oldPngTex.GetPixels32();
        var newColors = newPngTex.GetPixels32();
        if (oldColors == null || newColors == null)
            return;

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
                Debug.LogError("�޸���ɫ�в��� : " + i);
            }
        }

        if (!find)
        {
            Debug.LogError("��ͼһ�� ");
        }
    }

    void ModifyTexture()
    {
        if (oldTex == null || newTex == null)
            return;

        var oldAssetPath = AssetDatabase.GetAssetPath(oldTex);
        var absOldAssetPath = AssetsPath2ABSPath(oldAssetPath);

        var newAssetPath = AssetDatabase.GetAssetPath(newTex);
        var absNewAssetPath = AssetsPath2ABSPath(newAssetPath);

        PngTexture pngTex = new PngTexture();
        pngTex.Load(absOldAssetPath);
        pngTex.SaveAs(absNewAssetPath);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
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
