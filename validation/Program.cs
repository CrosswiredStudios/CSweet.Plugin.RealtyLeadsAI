using CSweet.Agent.SDK;
await AgentManifestLoader.LoadAsync(Path.GetFullPath(args.Length == 1 ? args[0] : "csweet-plugin.json"), CancellationToken.None);
Console.WriteLine("Reviewed RealtyLeadsAI manifest validation passed.");
