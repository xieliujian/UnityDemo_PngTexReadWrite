# UnityDemo_PngTexReadWrite

> 这个Demo用来展示Png贴图没有打开可读写开关下的读写， 为什么有这个需求，有的时候不想修改TextureImporter的可读写模式, 编辑器刷新Unity会有耗时。 可以用直接读取文件的形式操作

```C#

// 根据绝对路径读取二进制
var bytes = File.ReadAllBytes(absSrcAssetPath);

// 贴图可读写保存，格式设置为RGBA32，宽度高度默认0就可以，会从数据中直接赋值
Texture2D pngTex = new Texture2D(0, 0, TextureFormat.RGBA32, false);

// 加载数据
pngTex.LoadImage(bytes);

// 修改数据，保存数据
pngTex.Apply();

// 保存贴图
File.WriteAllBytes(absDstAssetPath, pngTex.EncodeToPNG());

```





