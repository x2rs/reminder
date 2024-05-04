using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public class Item
{
    public string name;
    public Color color;
    public Lesson lesson;
    public Item(string name, Color? color = null, Lesson? lesson = null)
    {
        this.name = name;
        this.color = color ?? Color.black;
        this.lesson = lesson ?? new Lesson();
    }
    public string ToText()
    {
        return ReminderLib.ToColorText(name, color);
    }
}


[Serializable]
public struct Lesson
{

}

public class Inventory
{
    public List<Item> items = new List<Item>();
    
    public void AddItem(string name, Color? color = null)
    {
        items.Add(new Item(name,color));
    }
}