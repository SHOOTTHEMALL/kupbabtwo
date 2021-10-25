using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IItem
{
    public void Use();
    public void Drop(Vector2 pos);

    public bool Save(bool getItem);
}
