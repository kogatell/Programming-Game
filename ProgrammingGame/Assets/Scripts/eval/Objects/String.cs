using System;
using Newtonsoft.Json.Linq;

public class String : Object, IEquatable<String>, Hashable
{
    private string value = "";
    
    public const string Name = "String";
    public string Value => value;

    public String(string value)
    {
        this.value = value;
    }
    
    public override string Type()
    {
        return "String";
    }

    public override string ToString()
    {
        return $"\"{value}\"";
    }

    public bool Equals(String other)
    {
        if (other == null)
        {
            return false;
        }
        return string.Equals(value, other.value);
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((String) obj);
    }

    public override int GetHashCode()
    {
        return Value != null ? Value.GetHashCode() : 0;
    }

    public override Object FromJson(JToken token)
    {
        value = token.Value<string>();
        return this;
    }

    public override bool EqualDeep(Object target)
    {
        return Equals(target);
    }
}
