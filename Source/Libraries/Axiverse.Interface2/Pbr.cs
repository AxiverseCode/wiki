﻿using SharpDX.Direct3D11;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Buffer11 = SharpDX.Direct3D11.Buffer;

namespace Axiverse.Interface2
{
    public class Pbr
    {
        public struct VsData
        {
            public Matrix4 proj;
            public Matrix4 view;
            public Matrix4 world;
            public Vector3 camera;
            public float unused;
        }

        public struct PsData
        {
            public Vector4 color;
            public Vector3 position;
            public float p0;
            public Vector3 direction;
            public float p1;
            public Vector3 lightVector;
            public float intensity;
        }

        public struct PsData2
        {
            Vector4 test;
        }

        public Device device;
        public Shader shader;
        public Buffer11 bufferVs1;
        public Buffer11 bufferPs1;
        public Buffer11 bufferPs2;
        public VsData vsData1;
        public PsData psData1;
        public PsData2 psData2;
        ShaderResourceView albedo;
        ShaderResourceView normal;
        ShaderResourceView height;
        ShaderResourceView roughness;
        ShaderResourceView metallic;
        ShaderResourceView alpha;
        ShaderResourceView ao;

        public Pbr(Device device)
        {
            this.device = device;
            bufferVs1 = device.CreateBuffer<VsData>();
            bufferPs1 = device.CreateBuffer<PsData>();
            bufferPs1 = device.CreateBuffer<PsData2>();

            albedo = Texture.CreateTextureFromBitmap(device, "../../pbr/albedo.jpg");
            normal = Texture.CreateTextureFromBitmap(device, "../../pbr/normal.jpg");
            height = Texture.CreateTextureFromBitmap(device, "../../pbr/height.jpg");
            roughness = Texture.CreateTextureFromBitmap(device, "../../pbr/roughness.jpg");
            metallic = Texture.CreateTextureFromBitmap(device, "../../pbr/metallic.jpg");
            alpha = Texture.CreateTextureFromBitmap(device, "../../pbr/alpha.jpg");
            ao = Texture.CreateTextureFromBitmap(device, "../../pbr/ambientocclusion.jpg");
            shader = new Shader(device, "../../Pbr.hlsl", "VS", "PS", Mesh.ColoredTexturedVertex.Elements);
        }

        public void Setup(Matrix4 world, Matrix4 view, Matrix4 proj, Vector3 camera)
        {
            shader.Apply();

            vsData1.world = world;
            vsData1.view = view;
            vsData1.proj = proj;
            vsData1.camera = camera;
            //vsData1.proj = world * view * proj;
            device.UpdateData(bufferVs1, vsData1);

            psData1.direction = new Vector3(0, 0.2f, -1).Normal();
            device.UpdateData(bufferPs1, psData1);

            device.NativeDeviceContext.VertexShader.SetConstantBuffer(0, bufferVs1);

            device.NativeDeviceContext.PixelShader.SetConstantBuffer(0, bufferPs1);
            device.NativeDeviceContext.PixelShader.SetConstantBuffer(1, bufferPs2);

            device.NativeDeviceContext.PixelShader.SetShaderResource(0, albedo);
        }
    }
}
