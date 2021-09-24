using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagList : MonoBehaviour
{
    [SerializeField]
    private List<string> tags = new List<string>();

    void Add(string newTag)
    {
        tags.Add(newTag);
    }

    bool HasTag(string checkTag)
    {
        return tags.Contains(checkTag);
    }

    public int Count()
    {
        return tags.Count;
    }
}
