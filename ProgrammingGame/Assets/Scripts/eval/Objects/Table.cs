using System.Collections;
using System.Collections.Generic;

interface Hashable
{
    int GetHashCode();
    bool Equals(object obj);
}

public class Table : Object
{
    private Dictionary<Object, Object> hashables;
    private Dictionary<Object, Object>   nonHashables;
    
    public const string Name = "Table";

    public Table()
    {
        hashables = new Dictionary<Object, Object>();
        nonHashables = new Dictionary<Object, Object>();
    }
    
    public override string Type()
    {
        return Name;
    }

    public override string ToString()
    {
        string print = "{ ";
        foreach (Object key in hashables.Keys)
        {
            print += $" {key.ToString()}: {hashables[key]} ";
        }

        print += " }";
        return print;
    }

    public Object Get(Object key)
    {
        if (key is Hashable)
        {
            if (!hashables.ContainsKey(key)) return Null.NULL;
            return hashables[key];
        }

        if (!nonHashables.ContainsKey(key))
        {
            return Null.NULL;
        }

        return nonHashables[key];
    }

    public void Set(Object key, Object value)
    {
        if (key is Hashable)
        {
            hashables[key] = value;
        }
        else
        {
            nonHashables[key] = value;
        }
    }
}
