using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR

namespace gtmEngine
{
    /// <summary>
    /// exr贴图格式
    /// </summary>
    public class EXRTexture2D
    {
        /// <summary>
        /// 贴图宽度
        /// </summary>
        int m_Width;

        /// <summary>
        /// 贴图高度
        /// </summary>
        int m_Height;

        /// <summary>
        /// 数据列表
        /// </summary>
        Color[] m_DataArray;

        /// <summary>
        /// 贴图宽度
        /// </summary>
        public int width
        {
            get { return m_Width; }
        }

        /// <summary>
        /// 贴图高度
        /// </summary>
        public int height
        {
            get { return m_Height; }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public EXRTexture2D(int width, int height)
        {
            m_Width = width;
            m_Height = height;
            m_DataArray = new Color[width * height];
        }

        /// <summary>
        /// 清理
        /// </summary>
        public void Clear()
        {
            m_DataArray = null;
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="path"></param>
        public unsafe void Save(string path)
        {
            fixed (Color* pixelData = m_DataArray)
            {
                AresEditor.NativeAPI.SaveEXR((IntPtr)pixelData, m_Width, m_Height, 4, 0, path);
            }
        }

        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="path"></param>
        public unsafe void Load(string path)
        {
            IntPtr outData = IntPtr.Zero;
            IntPtr err = IntPtr.Zero;
            int width = 0;
            int height = 0;

            if (AresEditor.NativeAPI.LoadEXR(ref outData, out width, out height, path, ref err) < 0 ||
                           m_Width != width || m_Height != height)
            {
                string format = string.Format("Error Read LightMap Error: {0} OK", path);
                Debug.LogError(format);
                return;
            }

            var src = (Color*)outData;
            for (int j = 0; j < m_DataArray.Length; ++j)
            {
                m_DataArray[j] = *src++;
            }

            AresEditor.NativeAPI.crt_free(outData);
        }

        /// <summary>
        /// 获取像素
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public Color GetPixel(int x, int y, bool isYFlip = false)
        {
            int index = y * m_Width + x;
            if (isYFlip)
            {
                index = (m_Height - y - 1) * m_Width + x;
            }

            if (m_DataArray == null ||
                index < 0 || 
                index >= m_DataArray.Length)
                return Color.black;

            Color color = m_DataArray[index];
            return color;
        }

        /// <summary>
        /// 设置像素
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void SetPixel(int x, int y, Color color, bool isYFlip = false)
        {
            if (x < 0 || x >= m_Width)
                return;

            if (y < 0 || y >= m_Height)
                return;

            int index = y * m_Width + x;
            if (isYFlip)
            {
                index = (m_Height - y - 1) * m_Width + x;
            }

            if (m_DataArray == null ||
                index < 0 ||
                index >= m_DataArray.Length)
                return;

            m_DataArray[index] = color;
        }

        /// <summary>
        /// 获取像素范围
        /// </summary>
        /// <param name="sx"></param>
        /// <param name="sy"></param>
        /// <param name="w"></param>
        /// <param name="h"></param>
        /// <returns></returns>
        public Color[] GetPixels(int sx, int sy, int w, int h)
        {
            sy = m_Height - sy - h;

            if (sx < 0 || sx >= m_Width ||
                sy < 0 || sy >= m_Height ||
                w < 0 || w > m_Width ||
                h < 0 || h > m_Height ||
                (sx + w) > m_Width || (sy + h) > m_Height ||
                m_DataArray == null || m_DataArray.Length != m_Width * m_Height)
            {
                return null;
            }

            Color[] result = new Color[w * h];

            for (int y = 0; y < h; ++y)
            {
                for (int x = 0; x < w; ++x)
                {
                    Color color = m_DataArray[(sy + y) * m_Width + (sx + x)];
                    result[y * w + x] = color;
                }
            }

            return result;
        }

        /// <summary>
        /// 设置像素范围
        /// </summary>
        /// <param name="sx"></param>
        /// <param name="sy"></param>
        /// <param name="w"></param>
        /// <param name="h"></param>
        /// <param name="result"></param>
        public void SetPixels(int sx, int sy, int w, int h, Color[] result)
        {
            sy = m_Height - sy - h;

            if (sx < 0 || sx >= m_Width ||
                sy < 0 || sy >= m_Height ||
                w < 0 || w > m_Width ||
                h < 0 || h > m_Height ||
                (sx + w) > m_Width || (sy + h) > m_Height ||
                result == null || result.Length != w * h ||
                m_DataArray == null || m_DataArray.Length != m_Width * m_Height)
            {
                return;
            }

            for (int y = 0; y < h; ++y)
            {
                for (int x = 0; x < w; ++x)
                {
                    m_DataArray[(sy + y) * m_Width + (sx + x)] = result[y * w + x];
                }
            }
        }
    }
}

#endif
