﻿using System;
using System.Threading;
using System.Threading.Tasks;

namespace InvocationContext
{
    public interface IInvocationContext
    {
        bool Working { get; }

        Task InvokeAsync(Action<BaseInvocationContextOptions>? optionsAction,
            Func<IServiceProvider, Task> action, CancellationToken cancellation = default);
    }
}