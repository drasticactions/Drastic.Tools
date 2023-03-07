// <copyright file="DebugGridWrapper.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

#nullable disable

namespace Drastic.Maui.DebugRainbows
{
    public class DebugGridWrapper : ContentView
    {
        public DebugGridWrapper()
        {
            this.InputTransparent = true;
        }

        public double HorizontalItemSize { get; set; }

        public double VerticalItemSize { get; set; }

        public int MajorGridLineInterval { get; set; }

        public Color MajorGridLineColor { get; set; }

        public Color GridLineColor { get; set; }

        public double MajorGridLineOpacity { get; set; }

        public double GridLineOpacity { get; set; }

        public double MajorGridLineWidth { get; set; }

        public double GridLineWidth { get; set; }

        public bool Inverse { get; set; }

        public bool MakeGridRainbows { get; set; }

        public DebugGridOrigin GridOrigin { get; set; }
    }
}
