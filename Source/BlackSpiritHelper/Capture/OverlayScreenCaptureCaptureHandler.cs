//  ---------------------------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
// 
//  The MIT License (MIT)
// 
//  Permission is hereby granted, free of charge, to any person obtaining a copy
//  of this software and associated documentation files (the "Software"), to deal
//  in the Software without restriction, including without limitation the rights
//  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//  copies of the Software, and to permit persons to whom the Software is
//  furnished to do so, subject to the following conditions:
// 
//  The above copyright notice and this permission notice shall be included in
//  all copies or substantial portions of the Software.
// 
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//  THE SOFTWARE.
//  ---------------------------------------------------------------------------------

using Composition.WindowsRuntimeHelpers;
using System;
using System.Numerics;
using Windows.Graphics.Capture;
using Windows.Graphics.DirectX.Direct3D11;
using Windows.UI.Composition;

namespace BlackSpiritHelper
{
    public class OverlayScreenCaptureCaptureHandler : IDisposable
    {
        private Compositor __compositor;
        private readonly ContainerVisual __root;

        private readonly SpriteVisual __content;
        private readonly CompositionSurfaceBrush __brush;

        private readonly IDirect3DDevice __device;
        private BasicCapture __capture;

        public OverlayScreenCaptureCaptureHandler(Compositor c)
        {
            __compositor = c;
            __device = Direct3D11Helper.CreateDevice();

            // Setup the root.
            __root = __compositor.CreateContainerVisual();
            __root.RelativeSizeAdjustment = Vector2.One;

            // Setup the content.
            __brush = __compositor.CreateSurfaceBrush();
            __brush.HorizontalAlignmentRatio = 0.5f;
            __brush.VerticalAlignmentRatio = 0.5f;
            __brush.Stretch = CompositionStretch.Uniform;

            var shadow = __compositor.CreateDropShadow();
            shadow.Mask = __brush;

            __content = __compositor.CreateSpriteVisual();
            __content.AnchorPoint = new Vector2(0.5f);
            __content.RelativeOffsetAdjustment = new Vector3(0.5f, 0.5f, 0);
            __content.RelativeSizeAdjustment = Vector2.One;
            __content.Brush = __brush;
            __content.Shadow = shadow;
            __root.Children.InsertAtTop(__content);
        }

        public Visual Visual => __root;

        public void Dispose()
        {
            StopCapture();
            __compositor = null;
            __root.Dispose();
            __content.Dispose();
            __brush.Dispose();
            __device.Dispose();
        }

        public void StartCaptureFromItem(GraphicsCaptureItem item)
        {
            StopCapture();
            __capture = new BasicCapture(__device, item);

            var surface = __capture.CreateSurface(__compositor);
            __brush.Surface = surface;

            __capture.StartCapture();
        }

        public void StopCapture()
        {
            __capture?.Dispose();
            __brush.Surface = null;
        }
    }
}
