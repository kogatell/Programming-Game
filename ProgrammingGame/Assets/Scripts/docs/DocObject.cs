using UnityEngine;

[CreateAssetMenu(fileName = "Documentation", menuName = "Documentation/DocObject", order = 2)]
public class DocObject : Doc
{
    public Type Type = Type.HashMap;
    public DocKeyPair[] PairsDocumentation;
}