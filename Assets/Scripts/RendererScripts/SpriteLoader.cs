using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SpriteLoader : MonoBehaviour
{
    [SerializeField] private Sprite[] importSprites;
    private Dictionary<string, Sprite> Sprites = new Dictionary<string, Sprite>();

    protected void LoadDictionary()
    {
        Sprites = new Dictionary<string, Sprite>();

        for (int i = 0; i < importSprites.Length; i++)
        {
            Debug.Log("SpriteLoader: Loaded sprite " + importSprites[i].name);
            Sprites.Add(importSprites[i].name, importSprites[i]);
        }
    }

    public int GetSpriteCount(string name_pattern)
    {
        // count the number of sprites that match the name pattern
        int count = 0;
        foreach (KeyValuePair<string, Sprite> sprite in Sprites)
        {
            if (sprite.Key.Contains(name_pattern))
            {
                count++;
            }
        }
        return count;
    }

    public Sprite GetSpriteByName(string name)
    {
        if (Sprites.ContainsKey(name))
            return Sprites[name];
        else
            return null;
    }
}