using System.Linq;
using UnityEngine;



public class Doc : ScriptableObject
{
    public string Name;
    public Type Type;
    public string Description;


    public override string ToString()
    {
        switch (this)
        {
            case DocArray array:
            {
                string[] result = array.ElementDocs.Select((element) => element.ToString()).ToArray();
                string parameters = result.Aggregate("", (acc, s) => $"{acc}- {s}.\n");
                string desc = array.Description;
                string name = array.Name;
                parameters = parameters == "" ? "Any size" : parameters;
                return $"Array {name}\n Elements: {parameters}\n Description: {desc}";
            }

            case Any primitive:
            {
                string name = primitive.Name;
                string description = primitive.Description;
                string type = primitive.Type.ToString();
                return $"Name: {name}, Description: {description}, Type: {type}";
                break;
            }

            case DocObject hashmap:
            {
                break;
            }

            case DocKeyPair keyPair:
            {
                break;
            }

            case DocMethod method:
            {
                string[] result = method.Parameters.Select((element) => element.ToString()).ToArray();
                string parameters = result.Aggregate("", (acc, s) => $"{acc}- {s}.\n");
                string returns = $"{method.Return}";
                string name = method.Name;
                string desc = method.Description;
                string parametersComma = method.Parameters.Select((element) => element.Name)
                    .Aggregate("", (acc, s) => $"{acc}{s}, ");
                parametersComma = parametersComma.Length > 2 ? parametersComma.Substring(0, parametersComma.Length - 2) : parametersComma;
                string use = $"{Name}({parametersComma})";
                return $"Method Name: {name}\nUse:\n{use}\nMethod Description: {desc}\nReturns:\n {returns}\nParameters: {parameters}";
            }

            case DocPrimitive primitive:
            {
                string name = primitive.Name;
                string description = primitive.Description;
                string type = primitive.Type.ToString();
                return $"Name: {name}, Description: {description}, Type: {type}";
                break;
            }
        }
        return "";
    }
}


