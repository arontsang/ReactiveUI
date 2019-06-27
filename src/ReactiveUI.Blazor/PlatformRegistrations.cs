﻿// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Reactive.PlatformServices;
using System.Text;

namespace ReactiveUI.Blazor
{
    /// <summary>
    /// Blazor Framework platform registrations.
    /// </summary>
    /// <seealso cref="ReactiveUI.IWantsToRegisterStuff" />
    public class PlatformRegistrations : IWantsToRegisterStuff
    {
        /// <inheritdoc/>
        public void Register(Action<Func<object>, Type> registerFunction)
        {
            registerFunction(() => new PlatformOperations(), typeof(IPlatformOperations));
            registerFunction(() => new ActivationForViewFetcher(), typeof(IActivationForViewFetcher));

#if NETSTANDARD
            if (WasmPlatformEnlightenmentProvider.IsWasm)
            {
                RxApp.TaskpoolScheduler = WasmScheduler.Default;
                RxApp.MainThreadScheduler = WasmScheduler.Default;
            }
            else
#endif
            {
                RxApp.TaskpoolScheduler = TaskPoolScheduler.Default;
                RxApp.MainThreadScheduler = new WaitForDispatcherScheduler(() => CurrentThreadScheduler.Instance);
            }
        }
    }
}
