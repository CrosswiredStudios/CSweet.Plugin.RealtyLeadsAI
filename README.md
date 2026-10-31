# RealtyLeadsAI plugin

Version 0.2.0. This is a declarative, host-executed, read-only plugin;
there is no plugin process that receives the user's API key.

`csweet-plugin.json` declares `kind: remote-mcp` and protocol 2.6. The C-Sweet
host builds its whole broker policy from this reviewed manifest: the endpoint
(`https://realtyleads.ai/mcp`), the `X-Api-Key` header, stateless POST with MCP
2025-03-26, and the `reviewedRemoteMcp.tools` map:

| Capability | Remote tool |
|---|---|
| `realtyleads.inheritance.search.v1` | `search_inheritance_leads` |
| `realtyleads.inheritance.lookup.v1` | `get_inheritance_lead_by_id` |

Input and output schemas are closed and bounded (SDK `ClosedJsonSchema`); the host
enforces them on every request and result and releases only the eleven safe lead
fields. The key is never a manifest value, configuration field or business-tool argument.
Adding a RealtyLeads read tool is now a manifest change plus the server tool — no
C-Sweet host code change.

Requires SDK 5.8.0 and a C-Sweet host with the generic reviewed remote MCP broker.
Older hosts reject protocol 2.6. Install from an immutable repository import; ZIP
source archives and executable bundles are rejected. The install review shows the
endpoint that will receive connected keys.

Connection creation stores an unverified key with no grants. RealtyLeadsAI checks
key validity and company membership when the agent reads; C-Sweet does not discover
or verify the provider company identity. Human connection owners must be active
business Managers or above. Never paste a real key into chat or agent configuration.

## Validation

```sh
dotnet run --project validation -- csweet-plugin.json validation/remote-tools.json
```

This validates the manifest with the SDK and checks each mapped tool against
`validation/remote-tools.json`, a recorded `tools/list` shape from
`RealtyLeads.McpServer`. Update that file whenever a server tool signature changes.
