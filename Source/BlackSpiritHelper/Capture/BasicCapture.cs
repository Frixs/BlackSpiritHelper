//  Followed example: https://github.com/microsoft/Windows.UI.Composition-Win32-Samples/tree/master/dotnet/WPF/ScreenCapture
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
using Windows.Graphics;
using Windows.Graphics.Capture;
using Windows.Graphics.DirectX;
using Windows.Graphics.DirectX.Direct3D11;
using Windows.UI.Composition;

namespace BlackSpiritHelper
{
    public class BasicCapture : IDisposable
    {
        private readonly GraphicsCaptureItem __item;
        private readonly Direct3D11CaptureFramePool __framePool;
        private readonly GraphicsCaptureSession __session;
        private SizeInt32 __lastSize;

        private IDirect3DDevice __device;
        private SharpDX.Direct3D11.Device __d3dDevice;
        private SharpDX.DXGI.SwapChain1 __swapChain;

        public BasicCapture(IDirect3DDevice d, GraphicsCaptureItem i)
        {
            __item = i;
            __device = d;
            __d3dDevice = Direct3D11Helper.CreateSharpDXDevice(__device);

            var dxgiFactory = new SharpDX.DXGI.Factory2();
            var description = new SharpDX.DXGI.SwapChainDescription1()
            {
                Width = __item.Size.Width,
                Height = __item.Size.Height,
                Format = SharpDX.DXGI.Format.B8G8R8A8_UNorm,
                Stereo = false,
                SampleDescription = new SharpDX.DXGI.SampleDescription()
                {
                    Count = 1,
                    Quality = 0
                },
                Usage = SharpDX.DXGI.Usage.RenderTargetOutput,
                BufferCount = 2,
                Scaling = SharpDX.DXGI.Scaling.Stretch,
                SwapEffect = SharpDX.DXGI.SwapEffect.FlipSequential,
                AlphaMode = SharpDX.DXGI.AlphaMode.Premultiplied,
                Flags = SharpDX.DXGI.SwapChainFlags.None
            };
            __swapChain = new SharpDX.DXGI.SwapChain1(dxgiFactory, __d3dDevice, ref description);

            __framePool = Direct3D11CaptureFramePool.Create(
                __device,
                DirectXPixelFormat.B8G8R8A8UIntNormalized,
                2,
                i.Size);
            __session = __framePool.CreateCaptureSession(i);
            __lastSize = i.Size;

            // Disable mouse cursor to be visible
            __session.IsCursorCaptureEnabled = false;

            __framePool.FrameArrived += OnFrameArrived;
        }

        public void Dispose()
        {
            __session?.Dispose();
            __framePool?.Dispose();
            __swapChain?.Dispose();
            __d3dDevice?.Dispose();
        }

        public void StartCapture()
        {
            __session.StartCapture();
        }

        public ICompositionSurface CreateSurface(Compositor compositor)
        {
            return compositor.CreateCompositionSurfaceForSwapChain(__swapChain);
        }

        private void OnFrameArrived(Direct3D11CaptureFramePool sender, object args)
        {
            var newSize = false;

            using (var frame = sender.TryGetNextFrame())
            {
                if (frame.ContentSize.Width != __lastSize.Width ||
                    frame.ContentSize.Height != __lastSize.Height)
                {
                    // The thing we have been capturing has changed size.
                    // We need to resize the swap chain first, then blit the pixels.
                    // After we do that, retire the frame and then recreate the frame pool.
                    newSize = true;
                    __lastSize = frame.ContentSize;
                    __swapChain.ResizeBuffers(
                        2, 
                        __lastSize.Width, 
                        __lastSize.Height, 
                        SharpDX.DXGI.Format.B8G8R8A8_UNorm, 
                        SharpDX.DXGI.SwapChainFlags.None);
                }

                using (var backBuffer = __swapChain.GetBackBuffer<SharpDX.Direct3D11.Texture2D>(0))
                using (var bitmap = Direct3D11Helper.CreateSharpDXTexture2D(frame.Surface))
                {
                    __d3dDevice.ImmediateContext.CopyResource(bitmap, backBuffer);
                }

            } // Retire the frame.

            __swapChain.Present(0, SharpDX.DXGI.PresentFlags.None);

            if (newSize)
            {
                __framePool.Recreate(
                    __device,
                    DirectXPixelFormat.B8G8R8A8UIntNormalized,
                    2,
                    __lastSize);
            }
        }
    }
}
