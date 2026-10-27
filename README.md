# RealtyLeadsAI plugin

Version 0.1.0. This is a declarative, host-executed, read-only plugin;
there is no plugin process that receives the user's API key.

`csweet-plugin.json` declares a distinct `remote-mcp` kind and protocol 2.5. The
reviewed profile uses only `https://realtyleads.ai/mcp`, stateless POST, MCP
2025-03-26, and `X-Api-Key`. It exposes inheritance search and lookup only, with
closed input schemas and a projection containing the server's eleven safe fields.
The key is never a manifest value, configuration field or business-tool argument.

SDK 5.7.0 validates this profile. Older hosts reject it. The companion C-Sweet
feature branch implements native key entry, owner/business/installation scope,
explicit consumer consent, trusted dispatch and rotation/revocation fencing. Its
synthetic browser acceptance passes connect → grant → actual agent read → revoke.
Install only after the companion host and SDK 5.7.0 rollout. Publication alone does not establish a connection or grant.
This first release requires immutable repository import; ZIP
source archives and executable bundles are rejected. See `C-Sweet/docs/implementation/realtyleads-remote-mcp.md`.

Connection creation stores an unverified key with no grants. RealtyLeadsAI checks
key validity and company membership when the agent reads; C-Sweet does not discover
or verify the provider company identity. Human connection owners must be active
business Managers or above. Never paste a real key into chat or agent configuration.

Contract source inspected locally: RealtyLeadsAI's `InheritanceLeadTools`,
`McpAccess`, `Program` and MCP security tests. No deployment was modified.
