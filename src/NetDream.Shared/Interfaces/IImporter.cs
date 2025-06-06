﻿using System;
using System.Collections.Generic;
using System.IO;

namespace NetDream.Shared.Interfaces
{
    public interface IImporter : IDisposable
    {
        public bool IsMatch(Stream input, string fileName);

        public void Read(Stream input);
    }
    public interface IImporter<T> : IDisposable
    {
        public bool IsMatch(Stream input, string fileName);

        public IEnumerable<T> Read(Stream input);
    }
}
