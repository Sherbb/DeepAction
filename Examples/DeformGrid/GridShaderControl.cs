using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;

public class GridShaderControl : MonoBehaviour
{
    public static GridShaderControl instance;


    public ComputeShader shader;
    public RenderTexture dispTex;
    public RenderTexture velTex;

    public float gridSize = 400f;

    public VisualEffect vfx;

    public float springForce = 500f;
    public float force;
    public float dampening;
    public float offset;

    public int texSize;

    public float test = 1f;

    private void Awake()
    {

        instance = this;

        RunShader();
        vfx.SetTexture("_Disp", dispTex);
    }


    void RunShader()
    {
        int kernelHandle = shader.FindKernel("UpdateTexture");

        dispTex = new RenderTexture(texSize, texSize, 24, RenderTextureFormat.ARGBFloat);
        dispTex.enableRandomWrite = true;
        dispTex.filterMode = FilterMode.Point;
        dispTex.Create();

        velTex = new RenderTexture(texSize, texSize, 24, RenderTextureFormat.ARGBFloat);
        velTex.enableRandomWrite = true;
        velTex.filterMode = FilterMode.Point;
        velTex.Create();

        shader.SetTexture(kernelHandle, "Displacement", dispTex);
        shader.SetTexture(kernelHandle, "Velocity", velTex);
        shader.SetFloat("deltaTime", Time.deltaTime);
        shader.Dispatch(kernelHandle, texSize / 8, texSize / 8, 1);


        //d.texture = dispTex;
        //i.texture = velTex;
    }

    void UpdateShader()
    {

        int kernelHandle = shader.FindKernel("UpdateTexture");
        shader.SetFloat("deltaTime", Time.fixedDeltaTime);
        shader.SetFloat("dampening", dampening);
        shader.SetFloat("springForce", springForce);
        shader.SetTexture(kernelHandle, "Displacement", dispTex);
        shader.SetTexture(kernelHandle, "Velocity", velTex);
        shader.Dispatch(kernelHandle, texSize / 8, texSize / 8, 1);
    }

    private void FixedUpdate()
    {
        UpdateShader();
    }

    public void AddForceAtLocation(Vector3 location, float force, float falloffExponent = 1f)
    {
        int kernelHandle = shader.FindKernel("AddForce");

        location.x += gridSize * .5f;
        location.y += gridSize * .5f;

        shader.SetVector("forcePosition", new UnityEngine.Vector4(((location.x / gridSize) * (float)texSize) - .5f, ((location.y / gridSize) * (float)texSize) - .5f, offset + location.z));
        shader.SetFloat("deltaTime", Time.fixedDeltaTime);
        shader.SetFloat("forceStrength", force);
        shader.SetFloat("falloffExponent", falloffExponent);
        shader.SetTexture(kernelHandle, "Displacement", dispTex);
        shader.SetTexture(kernelHandle, "Velocity", velTex);
        shader.Dispatch(kernelHandle, texSize / 8, texSize / 8, 1);
    }



    //these are all just presets.
    //could do enum + switch, but i think this is cleaner.
    public void LargePop(Vector3 location)
    {
        AddForceAtLocation(location, 25000f, 4f);
    }
    public void MediumPop(Vector3 location)
    {
        AddForceAtLocation(location, 17000f, 4f);
    }
    public void SmallPop(Vector3 location)
    {
        AddForceAtLocation(location, 8500f, 4f);
    }

    public void SmallSuck(Vector3 location)
    {
        AddForceAtLocation(location, -6500f, 4f);
    }

    public void BigSuck(Vector3 location)
    {
        AddForceAtLocation(location, -10000f, 4f);
    }

    public void MediumSuck(Vector3 location)
    {
        AddForceAtLocation(location, -8500f, 4f);
    }

}

