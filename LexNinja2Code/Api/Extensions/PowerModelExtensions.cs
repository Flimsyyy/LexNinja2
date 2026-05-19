using System.Reflection;
using MegaCrit.Sts2.Core.Models;

namespace LexNinja2.LexNinja2Code.Api.Extensions;

public static class PowerModelExtensions
{
    private static readonly FieldInfo? InternalDataField = typeof(PowerModel).GetField(
        "_internalData",
        BindingFlags.NonPublic | BindingFlags.Instance
    );

    public static object? GetInternalData(this PowerModel model)
    {
        return InternalDataField?.GetValue(model);
    }

    public static void SetInternalData(this PowerModel model, object value)
    {
        InternalDataField?.SetValue(model, value);
    }
}
