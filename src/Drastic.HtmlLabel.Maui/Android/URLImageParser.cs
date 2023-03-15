// <copyright file="URLImageParser.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.IO;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Text;
using Android.Widget;
using Java.Net;

namespace Drastic.HtmlLabel.Maui
{
    internal class UrlDrawable : BitmapDrawable
    {
        public Drawable Drawable { get; set; }

        public override void Draw(Canvas canvas)
        {
            if (this.Drawable != null)
            {
                this.Drawable.Draw(canvas);
            }
        }
    }

    internal class ImageGetterAsyncTask : AsyncTask<string, int, Drawable>
    {
        private readonly UrlDrawable urlDrawable;
        private readonly TextView container;

        public ImageGetterAsyncTask(UrlDrawable urlDrawable, TextView container)
        {
            this.urlDrawable = urlDrawable;
            this.container = container;
        }

        protected override Drawable RunInBackground(params string[] @params)
        {
            var source = @params[0];
            return this.FetchDrawable(source);
        }

        protected override void OnPostExecute(Drawable result)
        {
            if (result == null)
            {
                return;
            }

            // Set the correct bound according to the result from HTTP call
            this.urlDrawable.SetBounds(0, 0, 0 + result.IntrinsicWidth, 0 + result.IntrinsicHeight);

            // Change the reference of the current drawable to the result from the HTTP call
            this.urlDrawable.Drawable = result;

            // Redraw the image by invalidating the container
            this.container.Invalidate();

            // For ICS
            this.container.SetHeight(this.container.Height + result.IntrinsicHeight);

            // Pre ICS
            this.container.Ellipsize = null;
        }

        private static Stream Fetch(string urlString)
        {
            var url = new URL(urlString);
            var urlConnection = (HttpURLConnection)url.OpenConnection();
            Stream stream = urlConnection.InputStream;
            return stream;
        }

        private Drawable FetchDrawable(string urlString)
        {
            try
            {
                Stream stream = Fetch(urlString);
                var drawable = Drawable.CreateFromStream(stream, "src");
                drawable.SetBounds(0, 0, 0 + drawable.IntrinsicWidth, 0 + drawable.IntrinsicHeight);
                return drawable;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(@"            ERROR: ", ex.Message);
                return null;
            }
        }
    }

    internal class UrlImageParser : Java.Lang.Object, Html.IImageGetter
    {
        private readonly TextView container;

        public UrlImageParser(TextView container)
        {
            this.container = container;
        }

        public Drawable GetDrawable(string source)
        {
            var urlDrawable = new UrlDrawable();

            var asyncTask = new ImageGetterAsyncTask(urlDrawable, this.container);
            _ = asyncTask.Execute(source);

            return urlDrawable;
        }
    }
}
