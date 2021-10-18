﻿using Microsoft.AspNetCore.SignalR.Client;
using SKPLager.Shared.IHubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SKPLager.Shared.Helpers
{
    public static class HubConnectionBindExtensions
    {
        public static IDisposable BindOnInterface<T>(this HubConnection connection, Expression<Func<IInventoryPushHub, Func<T, Task>>> boundMethod, Action<T> handler)
            => connection.On<T>(_GetMethodName(boundMethod), handler);

        public static IDisposable BindOnInterface<T1, T2>(this HubConnection connection, Expression<Func<IInventoryPushHub, Func<T1, T2, Task>>> boundMethod, Action<T1, T2> handler)
            => connection.On<T1, T2>(_GetMethodName(boundMethod), handler);

        public static IDisposable BindOnInterface<T1, T2, T3>(this HubConnection connection, Expression<Func<IInventoryPushHub, Func<T1, T2, T3, Task>>> boundMethod, Action<T1, T2, T3> handler)
            => connection.On<T1, T2, T3>(_GetMethodName(boundMethod), handler);

        private static string _GetMethodName<T>(Expression<T> boundMethod)
        {
            var unaryExpression = (UnaryExpression)boundMethod.Body;
            var methodCallExpression = (MethodCallExpression)unaryExpression.Operand;
            var methodInfoExpression = (ConstantExpression)methodCallExpression.Object;
            var methodInfo = (MethodInfo)methodInfoExpression.Value;
            return methodInfo.Name;
        }
    }

}
