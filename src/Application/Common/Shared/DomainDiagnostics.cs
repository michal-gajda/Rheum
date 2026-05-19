namespace Rheum.Application.Common.Shared;

using System.Diagnostics;

public static class DomainDiagnostics
{
    public const string SOURCE_NAME = "Rheum.Service";
    public static readonly ActivitySource Source = new(SOURCE_NAME, "1.0.0");
}
