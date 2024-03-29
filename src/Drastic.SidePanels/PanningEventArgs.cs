﻿using System;

namespace SidePanels
{
    public class PanningEventArgs
    {
        public PanningEventArgs(nfloat delta, nfloat start, nfloat current)
        {
            Delta = delta;
            Start = start;
            Current = current;
        }

        public nfloat Delta { get; private set; }

        public nfloat Start { get; private set; }

        public nfloat Current { get; private set; }
    }
}
