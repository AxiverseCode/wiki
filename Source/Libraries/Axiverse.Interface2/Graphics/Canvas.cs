﻿using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;
using SharpDX.DXGI;
using SharpDX.Mathematics.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Texture2D11 = SharpDX.Direct3D11.Texture2D;
using Device2D = SharpDX.Direct2D1.Device;
using Factory2D = SharpDX.Direct2D1.Factory1;
using FactoryWrite = SharpDX.DirectWrite.Factory;
using DeviceGI = SharpDX.DXGI.Device;
using Font = Axiverse.Interface2.Interface.Font;

namespace Axiverse.Interface2
{
    public class Canvas : IDisposable
    {
        public Device Device { get; }
        public Device2D NativeDevice { get; set; }
        public Factory2D NativeFactory { get; set; }
        public DeviceContext NativeDeviceContext { get; set; }
        public FactoryWrite NativeFactoryWrite { get; set; }

        public Dictionary<Font, TextFormat> TextFormats { get; } = new Dictionary<Font, TextFormat>();
        public SolidColorBrush Brush { get; set; }

        public Bitmap1 Target { get; set; }
        public TextFormat TextFormat { get; set; }

        private string fontName = "Calibri";
        private int fontSize = 14;
        private Color fontColor = Color.White;

        /// <summary>
        /// Create a batch manager for drawing text and sprite
        /// </summary>
        /// <param name="device">Device pointer</param>
        internal Canvas(Device device)
        {
            Device = device;

            NativeFactory = new SharpDX.Direct2D1.Factory1(SharpDX.Direct2D1.FactoryType.SingleThreaded, DebugLevel.Information);

            using (var dxgiDevice = device.NativeDevice.QueryInterface<DeviceGI>())
                NativeDevice = new Device2D(NativeFactory, dxgiDevice);

            NativeDeviceContext = new DeviceContext(NativeDevice, DeviceContextOptions.None);

            InitializeFont();
            Brush = new SolidColorBrush(NativeDeviceContext, default(Color4));
        }

        public TextFormat GetTextFormat(Font font)
        {
            if (!TextFormats.TryGetValue(font, out var format))
            {
                format = new TextFormat(NativeFactoryWrite, font.FontFamily, font.Size);
                TextFormats.Add(font, format);
            }
            return format;
        }

        public SolidColorBrush GetBrush(Color4 color)
        {
            Brush.Color = color;
            return Brush;
        }

        public SolidColorBrush GetBrush(Axiverse.Mathematics.Drawing.Color color)
        {
            Brush.Color = color.ToRawColor4();
            return Brush;
        }

        /// <summary>
        /// Begin a 2D drawing session
        /// </summary>
        public void Begin()
        {
            NativeDeviceContext.BeginDraw();
        }

        /// <summary>
        /// End drawing session
        /// </summary>
        public void End()
        {
            NativeDeviceContext.EndDraw();
        }

        /// <summary>
        /// Update all resources
        /// </summary>
        /// <param name="backBuffer">BackBuffer</param>
        internal void UpdateResources(Texture2D11 backBuffer)
        {
            using (var surface2D = backBuffer.QueryInterface<Surface>())
            {
                var dpi = NativeFactory.DesktopDpi;
                Target = new Bitmap1(NativeDeviceContext, surface2D,
                    new BitmapProperties1(new PixelFormat(
                        Format.Unknown, SharpDX.Direct2D1.AlphaMode.Premultiplied), dpi.Height, dpi.Width, BitmapOptions.CannotDraw | BitmapOptions.Target));
                NativeDeviceContext.Target = Target;
            }

        }

        internal void Release()
        {
            NativeDeviceContext.Target = null;
            Target?.Dispose();
            Target = null;
        }

        private void InitializeFont()
        {
            NativeFactoryWrite = new FactoryWrite();
            TextFormat = new TextFormat(NativeFactoryWrite, fontName, fontSize) {
                TextAlignment = TextAlignment.Leading,
                ParagraphAlignment = ParagraphAlignment.Near
            };

            Brush = new SolidColorBrush(NativeDeviceContext, fontColor);
        }

        /// <summary>
        /// Draw text
        /// </summary>
        /// <param name="text">Text</param>
        /// <param name="x">Left position</param>
        /// <param name="y">Top position</param>
        /// <param name="width">Max width</param>
        /// <param name="height">Max heigh</param>
        public void DrawString(string text, float x, float y, float width = 800, float height = 600)
        {
            var brush = GetBrush(Color.White);
            NativeDeviceContext.DrawText(text, TextFormat, new RawRectangleF(x, y, width, height), brush);
        }

        public void DrawImage(Image2D image, Vector2 location)
        {
            var previous = NativeDeviceContext.Transform;
            NativeDeviceContext.Transform = Matrix3x2.Multiply(NativeDeviceContext.Transform, Matrix3x2.Translation(new SharpDX.Vector2(location.X, location.Y)));
            NativeDeviceContext.DrawBitmap(image.nativeBitmap, 1.0f, BitmapInterpolationMode.Linear);
            NativeDeviceContext.Transform = previous;
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            foreach (var textFormat in TextFormats.Values)
            {
                textFormat.Dispose();
            }
            TextFormats.Clear();

            Brush?.Dispose();

            TextFormat?.Dispose();
            TextFormat = null;
            Brush?.Dispose();
            Brush = null;
            Target?.Dispose();
            Target = null;

            NativeDeviceContext.Target = null;
            NativeDeviceContext?.Dispose();
            NativeDeviceContext = null;
            NativeDevice?.Dispose();
            NativeDevice = null;
            NativeFactory?.Dispose();
            NativeFactory = null;
            NativeFactoryWrite?.Dispose();
            NativeFactoryWrite = null;
        }
    }
}
