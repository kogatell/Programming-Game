using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

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
            print += $" {key}: {hashables[key]} ";
        }
        foreach (Object key in nonHashables.Keys)
        {
            print += $" {key}: {nonHashables[key]} ";
        }

        print += " }";
        return print;
    }

    public ArrayObject Keys()
    {
        List<Object> array = new List<Object>(hashables.Keys.Count + nonHashables.Keys.Count);

        foreach (Object key in hashables.Keys)
        {
            array.Add(key.Clone());
        }
        
        foreach (Object key in nonHashables.Keys)
        {
            array.Add(key);
        }
        
        return new ArrayObject(array);
    }
    
    public ArrayObject Values()
    {
        List<Object> array = new List<Object>(hashables.Values.Count + nonHashables.Values.Count);

        foreach (Object value in hashables.Values)
        {
            array.Add(value.Clone());
        }
        
        foreach (Object value in nonHashables.Values)
        {
            array.Add(value.Clone());
        }
        
        return new ArrayObject(array);
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

    public bool Contains(Object key)
    {
        if (key is Hashable)
        {
            if (!hashables.ContainsKey(key)) return false;
            return true;
        }

        if (!nonHashables.ContainsKey(key))
        {
            return false;
        }

        return true;
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

    public Object Delete(Object key)
    {
        if (key is Hashable)
        {
            return Boolean.FromBool(hashables.Remove(key));
        }
        
        return Boolean.FromBool(nonHashables.Remove(key));
    }

    public override bool EqualDeep(Object target)
    {
        if (!(target is Table t)) return false;
        foreach (Object key in t.hashables.Keys)
        {
            if (!hashables.ContainsKey(key)) return false;
            if (hashables[key] != t.hashables[key]) return false;
        }

        return true;
    }

    public override Object FromJson(JToken token)
    {
        JObject tokenDict = token.Value<JObject>();
        foreach (KeyValuePair<string, JToken> pair in tokenDict)
        {
            Set(new String(pair.Key), ObjectJSON.ParseJsonToken(pair.Value));
        }

        return this;
    }
}
