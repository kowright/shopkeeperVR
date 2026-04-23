using Assets.Scripts.Items;
using UnityEngine;

public class ItemColorChart : MonoBehaviour
{
    [SerializeField] private MeshRenderer firstMesh;
    [SerializeField] private MeshRenderer secondMesh;
    [SerializeField] private MeshRenderer thirdMesh;
    [SerializeField] private MeshRenderer fourthMesh;
    [SerializeField] private MeshRenderer fifthMesh;
    private ItemOutlineColorManager outlineColorManager = new ItemOutlineColorManager();


    void Start()
    {
        ColorBandForQuality(firstMesh, ItemQuality.Top);
        ColorBandForQuality(secondMesh, ItemQuality.Great);
        ColorBandForQuality(thirdMesh, ItemQuality.Good);
        ColorBandForQuality(fourthMesh, ItemQuality.Low);
        ColorBandForQuality(fifthMesh, ItemQuality.Bad);


        //Color topColor = outlineColorManager.GetOutlineColorForQuality(ItemQuality.Top);
        //Material mat = new Material(firstMesh.sharedMaterials[0]);
        //mat.color = topColor;
        //Color color = mat.color;
        //mat.color = color;
        //firstMesh.material = mat;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ColorBandForQuality(MeshRenderer mesh, ItemQuality quality)
    {
        Color matColor = outlineColorManager.GetOutlineColorForQuality(quality);
        Material mat = new Material(mesh.sharedMaterials[0]);
        mat.color = matColor;
        Color color = mat.color;
        mat.color = color;
        mesh.material = mat;
    }
}
