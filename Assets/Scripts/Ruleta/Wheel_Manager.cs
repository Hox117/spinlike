using System;
using System.Collections.Generic;
using UnityEngine;

public class Wheel_Manager : MonoBehaviour
{

    bool _isGenerating;

    [SerializeField] Material _material;


    [ContextMenu("Generate")]
    public void Generate(int _numeroDeSegmentos, (float radioInterior, float radioExterior) Radios, List<UnityEngine.Color[]> colores, int _arcSubdivisions, List<string> texto, List<Sprite> sprite, List<Ficha> fichas)
    {
        if (_isGenerating)
            return;

        _isGenerating = true;

        ClearSegments();

        float angleStep = 360f / _numeroDeSegmentos; //Separamos el circulo en los segmentos que tenemos


        for (int i = 0; i < _numeroDeSegmentos; i++)
        {
            float start =
                i * angleStep;

            float end =
                start + angleStep;

            CreateSegment(
                i,
                start,
                end,
                colores[i],
                Radios,
                _arcSubdivisions,
                texto[i],
                sprite[i],
                fichas[i]
            );
        }

        _isGenerating = false;
    }

    void CreateSegment(
        int index,
        float startAngle,
        float endAngle,
        UnityEngine.Color[] colores,
        (float radioInterior, float radioExterior) Radios,
        int _arcSubdivisions,
        string textoSegmento,
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
               colores,
               Radios,
               _arcSubdivisions
            );

        meshfilter.sharedMesh = mesh;


        //Mesh Collider
        PolygonCollider2D Polygoncollider = segment.AddComponent<PolygonCollider2D>();

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
        tmp.text = textoSegmento;
        tmp.alignment = TMPro.TextAlignmentOptions.Center;
        tmp.fontSize = 4f; // ajusta según tu escala
        tmp.sortingOrder = 1;

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

    }

    void CreateSegment(
        int index,
        float startAngle,
        float endAngle,
        Texture2D sprite,
        (float radioInterior, float radioExterior) Radios,
        int _arcSubdivisions,Ficha ficha)
        
    {
        GameObject segment =
            new($"Segment_{index}");

        segment.transform.SetParent(
            transform,
            false
        );

        MeshFilter meshfilter =
            segment.AddComponent<MeshFilter>();

        MeshRenderer meshRenderer =
            segment.AddComponent<MeshRenderer>();

        if (_material != null)
            meshRenderer.sharedMaterial =
                _material;

        Mesh mesh =
            BuildSlice(
                startAngle,
                endAngle,
               Radios,
               _arcSubdivisions
            );

        meshfilter.sharedMesh = mesh;
        meshRenderer.material.SetTexture("_MainTex", sprite);

        segment.AddComponent<SegmentController>();
        segment.GetComponent<SegmentController>().addFicha(ficha);

    }

    Mesh BuildSlice(float start, float end, UnityEngine.Color[] colores, (float radioInterior, float radioExterior) Radios, int _arcSubdivisions)
    {
        Mesh mesh =
            new Mesh();

        List<Vector3> verts =
            new();

        List<int> tris =
            new();

        List<UnityEngine.Color> colors =
            new();

        float step =
            (end - start)
            / _arcSubdivisions;

        for (int i = 0; i <= _arcSubdivisions; i++)
        {
            float angle =
                start + step * i;

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


            colors.Add(colores[0]);
            colors.Add(colores[1]);
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

        mesh.SetVertices(verts);
        mesh.SetTriangles(
            tris,
            0
        );

        mesh.SetColors(
            colors
        );

        return mesh;
    }
    Mesh BuildSlice(
    float start,
    float end,
    (float radioInterior, float radioExterior) Radios,
    int _arcSubdivisions)
    {
        Mesh mesh =
            new Mesh();

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