using UnityEngine;

[CreateAssetMenu(fileName = "Documentation", menuName = "Documentation/DocMethod", order = 2)]
public class DocMethod : Doc
{
    public Doc[] Parameters;
    public Doc Return;
}