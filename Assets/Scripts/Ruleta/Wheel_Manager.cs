using System.Collections.Generic;
using UnityEngine;

public class Wheel_Manager : MonoBehaviour
{

    bool _isGenerating;

    [SerializeField]Material _material;

    
    [ContextMenu("Generate")]
    public void Generate(int _numeroDeSegmentos, (float radioInterior, float radioExterior) Radios, UnityEngine.Color[] colores, int _arcSubdivisions)
    {
        if (_isGenerating)
            return;

        _isGenerating = true;

        ClearSegments();

        float angleStep =
            360f / _numeroDeSegmentos;

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
                colores,
                Radios,
                _arcSubdivisions
            );
        }

        _isGenerating = false;
    }

    public void Generate(int _numeroDeSegmentos, (float radioInterior, float radioExterior) Radios, Texture2D sprite, int _arcSubdivisions)
    {
        if (_isGenerating)
            return;

        _isGenerating = true;

        ClearSegments();

        float angleStep =
            360f / _numeroDeSegmentos;

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
                sprite,
                Radios,
                _arcSubdivisions
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
        int _arcSubdivisions)
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
               colores,
               Radios,
               _arcSubdivisions
            );

        meshfilter.sharedMesh =
            mesh;
    }

    void CreateSegment(
        int index,
        float startAngle,
        float endAngle,
        Texture2D sprite,
        (float radioInterior, float radioExterior) Radios,
        int _arcSubdivisions)
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

        meshfilter.sharedMesh =
            mesh;
        meshRenderer.material.SetTexture("_MainTex", sprite);
    }

    Mesh BuildSlice(float start, float end, UnityEngine.Color[] colores, (float radioInterior,float radioExterior) Radios, int _arcSubdivisions)
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
    Mesh BuildSlice(float start, float end, (float radioInterior, float radioExterior) Radios, int _arcSubdivisions)
    {
        Mesh mesh =
            new Mesh();

        List<Vector3> verts =
            new();

        List<int> tris =
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

        

        return mesh;
    }

    Vector2 Polar(
        float radius,
        float angle)
    {
        float rad =
            angle *
            Mathf.Deg2Rad;

        return new Vector2(
            Mathf.Cos(rad)
                * radius,
            Mathf.Sin(rad)
                * radius
        );
    }

    void ClearSegments()
    {
        List<GameObject> children =
            new();

        foreach (
            Transform child
            in transform
        )
        {
            children.Add(
                child.gameObject
            );
        }

        foreach (
            var obj
            in children
        )
        {
            Destroy(obj);
        }
    }
}