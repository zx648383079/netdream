﻿using System;

namespace NetDream.Core.Interfaces
{
    public interface IOperationResult
    {
        public bool IsSuccess { get; }

        public int FailureReason { get; }

        public string Message { get; }

        public Exception? Error { get; }
    }
}