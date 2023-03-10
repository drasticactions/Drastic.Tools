// <copyright file="ImageExtensions.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using SkiaSharp;
using Svg.Skia;

#nullable disable

namespace Drastic.Markdown.Extensions
{
    public static class ImageExtensions
    {
        public static void RenderSvg(this Image view, string uri)
        {
            try
            {
                var req = (HttpWebRequest)WebRequest.Create(uri);

                var svg = new SKSvg();
                req.BeginGetResponse(
                    (ar) =>
                {
                    var res = (ar.AsyncState as HttpWebRequest).EndGetResponse(ar) as HttpWebResponse;
                    using (var stream = res.GetResponseStream())
                    {
                        if (stream != null)
                        {
                            var picture = svg.Load(stream);

                            using (var image = SKImage.FromPicture(picture, picture.CullRect.Size.ToSizeI()))
                            using (var data = image.Encode(SKEncodedImageFormat.Jpeg, 80))
                            {
                                var ms = new MemoryStream();

                                if (data != null && !data.IsEmpty)
                                {
                                    data.SaveTo(ms);
                                    ms.Seek(0, SeekOrigin.Begin);
                                    ms.Position = 0;
                                    view.Source = ImageSource.FromStream(() => ms);
                                }
                            }
                        }
                    }
                }, req);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Failed to render svg: {ex}");
            }
        }
    }
}
