using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadSpriteResources : MonoBehaviour
{
    public static Dictionary<string, Sprite> sprites = new Dictionary<string, Sprite>();

    /// <summary>
    /// Load động
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static Sprite LoadSprite(string path)
    {
        if (sprites.TryGetValue(path, out var value))
        {
            return value;
        }

        Sprite sprite = Resources.Load<Sprite>(path);
        if (sprite != null) sprites.Add(path, sprite);
        return sprite;
    }
}
