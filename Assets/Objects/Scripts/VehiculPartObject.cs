using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[CreateAssetMenu(fileName = "New Empty", menuName = "Parts/Empty")]
public class VehiculPartObject : ScriptableObject
{
    public VehiculParts part;
    public GameObject prefab;
    public Texture2D texture;

    public Sprite Sprite
    {
        get
        {
            return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        }
    }

#if UNITY_EDITOR
    [ContextMenu("Generate Icon")]
    public void GetIcon()
    {
        string pathAbs = Application.dataPath + "/Objects/Icons/";
        string path = "Assets/Objects/Icons/";
        Texture2D t = UnityEditor.AssetPreview.GetAssetPreview(prefab);
        if (t != null)
        {
            byte[] itemBGBytes = t.EncodeToPNG();
            File.WriteAllBytes(pathAbs + name + "_icon.png", itemBGBytes);
            UnityEditor.AssetDatabase.Refresh();
            texture = (Texture2D)UnityEditor.AssetDatabase.LoadAssetAtPath(path + name + "_icon.png", typeof(Texture2D)); 
        }
    }
#endif
}

public enum VehiculParts
{
    None,
    Engine,
    Wheel,
    Seat,
    Roof,
    Armature,
    SteeringWheel,
    Back,
    Carrige,
    Spoiler,
    Sidebar,
    Bumper,
    Exhaust,
    Bonnet,
    Door,
    Count
}
