# UnityDemo_PngTexReadWrite

Png贴图没有打开可读写开关下的读写


```C#

// 根据绝对路径读取二进制
var bytes = File.ReadAllBytes(absOldAssetPath);

Texture2D copyTex = new Texture2D(oldTex.width, oldTex.height, TextureFormat.RGBA32, false);

newTex.LoadImage(bytes);

if (bytes != null)
{
    Texture2D copyTex = new Texture2D(oldTex.width, oldTex.height, TextureFormat.RGBA32, false);
    var isSuccess = newTex.LoadImage(bytes);
    if (!isSuccess)
    {
        Debug.LogError("Texture2D.LoadImage 错误");
    }

    copyTex.Apply();

    var newAssetPath = AssetDatabase.GetAssetPath(newTex);
    var absTexPath = AssetsPath2ABSPath(newAssetPath);
    File.WriteAllBytes(absTexPath, copyTex.EncodeToPNG());
    AssetDatabase.SaveAssets();
    AssetDatabase.Refresh();
}

``` 

