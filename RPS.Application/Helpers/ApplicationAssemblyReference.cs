using System.Reflection;

namespace RPS.Application.Helpers;

public static class AplicationAssemblyReference
{
    public static readonly Assembly Assembly = typeof(AplicationAssemblyReference).Assembly;
}