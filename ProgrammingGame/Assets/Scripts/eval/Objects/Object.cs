using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

public abstract class Object
{
    public abstract string Type();
    
    /// <summary>
    /// Returns if this object is an error
    /// </summary>
    /// <returns></returns>
    public bool IsError()
    {
        return Type() == Error.Name;
    }

    public Object TryCopy()
    {
        if (IsNumeric()) return (this as Number).Copy();
        return this;
    }
    
    public bool EqualsInLua(Object other)
    {
        if (other.Type() != Type())
        {
            return false;
        }

        if (other.IsNumeric() || other is String)
        {
            return other.Equals(this);
        }

        return ReferenceEquals(this, other);
    }

    public Object ToNumber()
    {
        if (IsError()) return this;
        if (IsNumeric()) return this;
        if (Type() == Boolean.Name)
        {
            return ToBool() == Boolean.True ? new Number(1) : new Number(0);
        }
        if (Type() != String.Name) return new Error($"can't convert object of type {Type()} to numeric");
        try
        {
            return new Number(int.Parse((this as String).Value));
        }
        catch (Exception)
        {
            return new Error($"error converting string {this} to number");
        }
    }

    public Boolean ToBool()
    {
        if (this is Boolean b) return b;
        return Boolean.FromObject(this);
    }

    public bool IsNumeric()
    {
        return Type() == Number.Name;
    }

    public static Object FromJsonString(string json)
    {
        JObject parsedJson = JObject.Parse(json);
        JToken data = parsedJson["data"];
        
        return ObjectJSON.ParseJsonToken(data);
    }

    public virtual Object FromJson(JToken token) { return Null.NULL; }

    public virtual bool EqualDeep(Object target)
    {
        return target == this;
    }

    /// <summary>
    /// Tries to make a copy, usually a number, else will just return itself
    /// </summary>
    /// <returns></returns>
    public virtual Object Clone()
    {
        return this;
    }

}


class ObjectJSON
{
    public static Object ParseJsonToken(JToken token)
    {
        switch (token.Type)
        {
            case JTokenType.Array:
            {
                return new ArrayObject().FromJson(token);
            }

            case JTokenType.String:
            {
                return new String("").FromJson(token);
            }

            case JTokenType.Boolean:
            {
                return Boolean.FromBool(token.Value<bool>());
            }

            case JTokenType.Integer:
            {
                return new Number(token.Value<double>());
            }

            case JTokenType.Null:
            {
                return Null.NULL;
            }

            case JTokenType.Object:
            {
                return new Table().FromJson(token);
            }
        }

        return null;
    }
    
}