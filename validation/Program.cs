using System.Text.Json;
using CSweet.Agent.Contracts.Packaging;
using CSweet.Agent.SDK;

// 1. The manifest must pass the SDK's reviewed remote MCP validation (protocol 2.6).
var manifestPath = Path.GetFullPath(args.Length >= 1 ? args[0] : "csweet-plugin.json");
var manifest = await AgentManifestLoader.LoadAsync(manifestPath, CancellationToken.None);

// 2. Every mapped remote tool must be compatible with the server's recorded tools/list advertisement.
//    Update remote-tools.json whenever RealtyLeads.McpServer changes a tool signature; drift then fails here.
var listingPath = Path.GetFullPath(args.Length >= 2 ? args[1] : Path.Combine("validation", "remote-tools.json"));
using var listing = JsonDocument.Parse(await File.ReadAllTextAsync(listingPath));
var advertised = listing.RootElement.GetProperty("tools").EnumerateArray()
    .ToDictionary(t => t.GetProperty("name").GetString()!, t => t.GetProperty("inputSchema").Clone(), StringComparer.Ordinal);
var failures = new List<string>();
foreach (var tool in manifest.ReviewedRemoteMcp!.Tools)
{
    var declared = manifest.Provides.Single(p => p.Name == tool.Capability);
    if (!advertised.TryGetValue(tool.RemoteTool, out var remote))
        failures.Add($"{tool.RemoteTool} is not advertised by the recorded server listing.");
    else if (!ClosedJsonSchema.IsCompatibleRemoteInput(declared.InputSchema, remote))
        failures.Add($"{tool.RemoteTool} input schema is incompatible with {tool.Capability}.");
}
if (failures.Count > 0)
{
    foreach (var failure in failures) Console.Error.WriteLine(failure);
    return 1;
}
Console.WriteLine($"Reviewed {manifest.Name} {manifest.Version} manifest validation passed ({manifest.ReviewedRemoteMcp.Tools.Count} tools, endpoint {ReviewedRemoteMcpValidator.EndpointHost(manifest.ReviewedRemoteMcp.Endpoint)}).");
return 0;
