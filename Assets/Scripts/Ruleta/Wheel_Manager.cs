using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Wheel_Manager : MonoBehaviour
{

    bool _isGenerating;
    [SerializeField] float radioInterior;
    [SerializeField]  float radioExterior;
    [SerializeField]  int _arcSubdivisions;
    [SerializeField] Material _material;
    [SerializeField] private TMPro.TMP_FontAsset _fontPersonalizada;
    IInventoryService inventoryService;
    IRouletteService rouletteService;
    [SerializeField] private bool isreward = false;

    protected virtual void Start()
    {
        inventoryService = AppContainer.Get<IInventoryService>();
        List<Color[]> colores = new List<Color[]>();
        //textos = new List<string>();
        List<Sprite> sprites = new List<Sprite>();

        rouletteService = AppContainer.Get<IRouletteService>();
        if(!isreward)
        GenerateRoulette();
        

    }

    public void GenerateRoulette() {
        inventoryService.ramdomizeList();

        List<Ficha> listaFichas = inventoryService.getListaFichas();
        int numeroSegmentos = listaFichas.Count;
        rouletteService.ToogleStatus(false);
        //para cada ficha sacamos su color y color secundario

        Generate(numeroSegmentos, listaFichas);
    }
    [ContextMenu("Generate")]
    public void Generate(int _numeroDeSegmentos,  List<Ficha> fichas)
    {
        if (_isGenerating) return;

        _isGenerating = true;

        ClearSegments();

        (float radioInterior, float radioExterior) Radios = (radioInterior,radioExterior);

        float angleStep = 360f / _numeroDeSegmentos; //Separamos el circulo en los segmentos que tenemos


        for (int i = 0; i < _numeroDeSegmentos; i++)
        {
            float start = i * angleStep;

            float end = start + angleStep;

            CreateSegment(
                i,
                start,
                end,
                fichas[i].colorPrincipal,
                fichas[i].colorSecundario,
                Radios,
                _arcSubdivisions,
                fichas[i].sprite,
                fichas[i]
            );
        }

        _isGenerating = false;
    }

    void CreateSegment(
        int index,
        float startAngle,
        float endAngle,
        UnityEngine.Color colorPrincipal, 
        UnityEngine.Color colorSecundario,
        (float radioInterior, float radioExterior) Radios,
        int _arcSubdivisions,
        Sprite spriteObjeto,
        Ficha ficha)
    {
        GameObject segment = new($"Segment_{index}");

        segment.transform.SetParent(
            transform,
            false
        );

        MeshFilter meshfilter = segment.AddComponent<MeshFilter>();

        MeshRenderer meshRenderer = segment.AddComponent<MeshRenderer>();

        if (_material != null) meshRenderer.sharedMaterial = _material;   

        Mesh mesh =
            BuildSlice(
                startAngle,
                endAngle,
               colorPrincipal,
               colorSecundario,
               Radios,
               _arcSubdivisions
            );

        meshfilter.sharedMesh = mesh;


        //Mesh Collider
        PolygonCollider2D Polygoncollider = segment.AddComponent<PolygonCollider2D>();
        Vector2[] colliderPoints = mesh.vertices.Select(v=>(Vector2)v).Distinct().ToArray();
        Polygoncollider.SetPath(0, colliderPoints);
        //Game objects de texto y de sprite
        float midAngle = (startAngle + endAngle) * 0.5f;
        float midRadius = (Radios.radioInterior + Radios.radioExterior) * 0.5f;

        float midAngleRad = midAngle * Mathf.Deg2Rad;
        Vector3 midPos = new Vector3(
            Mathf.Cos(midAngleRad),
            Mathf.Sin(midAngleRad),
            0f
        ) * midRadius;

        GameObject textGO = new($"Text_{index}");
        textGO.transform.SetParent(segment.transform, false);
        textGO.transform.localPosition = midPos;

        TMPro.TextMeshPro tmp = textGO.AddComponent<TMPro.TextMeshPro>();

        tmp.text = ficha.segmentData; 

        tmp.alignment = TMPro.TextAlignmentOptions.Center;
        tmp.fontSize = 4f; 
        tmp.sortingOrder = 1;
        

        if (_fontPersonalizada != null)
        {
            tmp.font = _fontPersonalizada;
            tmp.ForceMeshUpdate();
        }
        
        textGO.transform.localRotation = Quaternion.identity;

        // --- Sprite ---
        GameObject spriteGO = new($"Sprite_{index}");
        spriteGO.transform.SetParent(segment.transform, false);

        float spriteRadius = midRadius * 1.5f;
        Vector3 spritePos = new Vector3(
            Mathf.Cos(midAngleRad),
            Mathf.Sin(midAngleRad),
            0f
        ) * spriteRadius;

        spriteGO.transform.localPosition = spritePos;

        SpriteRenderer spriteRenderer = spriteGO.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = spriteObjeto;
        spriteRenderer.sortingOrder = 1;


        segment.AddComponent<SegmentController>();
        segment.GetComponent<SegmentController>().addFicha(ficha);

        segment.layer = LayerMask.NameToLayer("Segment");
    }

    Mesh BuildSlice(float start, float end, UnityEngine.Color colorPrincipal, UnityEngine.Color colorSecundario, (float radioInterior, float radioExterior) Radios, int _arcSubdivisions)
    {
        Mesh mesh = new Mesh();

        List<Vector3> verts = new();

        List<int> tris = new();

        List<UnityEngine.Color> colors = new();

        float step = (end - start) / _arcSubdivisions;

        for (int i = 0; i <= _arcSubdivisions; i++)
        {
            float angle = start + step * i;

            verts.Add(
                Polar(
                    Radios.radioInterior,
                    angle
                ));

            verts.Add(
                Polar(
                    Radios.radioExterior,
                    angle
                ));


            colors.Add(colorPrincipal);
            colors.Add(colorSecundario);
        }

        for (int i = 0; i < _arcSubdivisions; i++)
        {
            int a = i * 2;

            tris.Add(a);
            tris.Add(a + 1);
            tris.Add(a + 2);

            tris.Add(a + 2);
            tris.Add(a + 1);
            tris.Add(a + 3);
        }

        mesh.SetVertices(verts);
        mesh.SetTriangles( tris, 0 );

        mesh.SetColors( colors );

        return mesh;
    }
    Mesh BuildSlice(
    float start,
    float end,
    (float radioInterior, float radioExterior) Radios,
    int _arcSubdivisions)
    {
        Mesh mesh = new Mesh();

        List<Vector3> verts =
            new();

        List<int> tris =
            new();

        List<Vector2> uvs =
            new();

        float step =
            (end - start)
            / _arcSubdivisions;

        for (int i = 0; i <= _arcSubdivisions; i++)
        {
            float angle =  start + step * i;

            verts.Add(
                Polar(
                    Radios.radioInterior,
                    angle
                ));

            verts.Add(
                Polar(
                    Radios.radioExterior,
                    angle
                ));

            float u = (float)i / _arcSubdivisions;

            uvs.Add(
                new Vector2(
                    u,
                    0
                )
            );

            uvs.Add(
                new Vector2(
                    u,
                    1
                )
            );
        }

        for (int i = 0; i < _arcSubdivisions; i++)
        {
            int a =
                i * 2;

            tris.Add(a);
            tris.Add(a + 1);
            tris.Add(a + 2);

            tris.Add(a + 2);
            tris.Add(a + 1);
            tris.Add(a + 3);
        }

        mesh.SetVertices(
            verts
        );

        mesh.SetTriangles(
            tris,
            0
        );

        mesh.SetUVs(
            0,
            uvs
        );

        return mesh;
    }
    Vector2 Polar(
        float radius,
        float angle)
    {
        float rad = angle * Mathf.Deg2Rad;

        return new Vector2(
            Mathf.Cos(rad)
                * radius,
            Mathf.Sin(rad)
                * radius
        );
    }

    void ClearSegments()
    {
        List<GameObject> children = new();

        foreach (Transform child in transform)
        {
            children.Add( child.gameObject );
        }

        foreach ( var obj in children)
        {
            Destroy(obj);
        }
    }
}