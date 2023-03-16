using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PngTexture
{
    /// <summary>
    /// ÌùÍ¼
    /// </summary>
    Texture2D m_Tex;

    /// <summary>
    /// ÌùÍ¼Â·¾¶
    /// </summary>
    string m_Path;

    public PngTexture()
    {

    }

    public Color32[] GetPixels32()
    {
        if (m_Tex == null)
            return null;

        return m_Tex.GetPixels32();
    }

    public void Load(string path)
    {
        m_Path = path;

        if (!File.Exists(path))
            return;

        byte[] bytes = File.ReadAllBytes(path);
        if (bytes == null)
            return;

        m_Tex = new Texture2D(0, 0, TextureFormat.RGBA32, false);
        if (m_Tex == null)
            return;

        Debug.LogError($"PngTexture.Load {path} ´íÎó");

        bool isSuccess = m_Tex.LoadImage(bytes);
        if (!isSuccess)
        {
            Debug.LogError($"PngTexture.Load {path} ´íÎó");
        }
    }

    public void SaveAs(string path)
    {
        if (m_Tex == null)
            return;

        if (!File.Exists(path))
            return;

        m_Tex.Apply();

        File.WriteAllBytes(path, m_Tex.EncodeToPNG());
    }

    public void Save()
    {
        SaveAs(m_Path);
    }
}
