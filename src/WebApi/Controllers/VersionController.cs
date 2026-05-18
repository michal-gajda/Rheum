namespace Rheum.WebApi.Controllers;

using System.Diagnostics;
using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[AllowAnonymous, ApiController, ApiExplorerSettings(IgnoreApi = true), Route("[controller]")]
public sealed class VersionController : ControllerBase
{
    [Route(""), HttpGet]
    public async Task<FileVersionInfo> Get(CancellationToken cancellationToken = default)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var result = FileVersionInfo.GetVersionInfo(assembly.Location);

        return await Task.FromResult(result);
    }
}
