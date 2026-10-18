# ai-assist-api-azure

Thin ASP.NET Core 8 portfolio project: a small AI assist API that demonstrates practical AI integration patterns without becoming the "hero project."

## What this includes
- `POST /api/assist` endpoint for prompt -> assist response
- `IAiClient` abstraction with:
  - `MockAiClient` (default, no key required)
  - `AzureOpenAiClient` (activated only when Azure config is present)
- Built-in .NET rate limiting (`20 req/min` + queue)
- Input validation (`Prompt` required, 3-2000 chars)
- Health endpoints:
  - `GET /api/health`
  - `GET /health/live`
  - `GET /health/ready`
- Unit tests for controller/service behavior using mock clients
- Dockerfile and GitHub Actions CI
- Safety guidance in `docs/SAFETY.md`

## Run locally (mock mode, no Azure key)
1. Install .NET 8 SDK.
2. From repo root:
   - `dotnet restore AiAssistApiAzure.sln`
   - `dotnet run --project AiAssist.Api/AiAssist.Api.csproj`
3. Call:
   - `POST http://localhost:5000/api/assist`
   - body: `{ "prompt": "Help me design a retry strategy for transient cloud failures." }`

## Optional Azure OpenAI configuration
Set placeholders in `AiAssist.Api/appsettings.json` or user-secrets/environment variables:

```json
"AzureOpenAI": {
  "Endpoint": "https://<your-resource>.openai.azure.com",
  "ApiKey": "<your-key>",
  "DeploymentName": "<your-deployment>",
  "ApiVersion": "2024-02-15-preview"
}
```

If any required value is missing, the app automatically stays on `MockAiClient`.

## Cost warning
- Do not commit keys.
- Keep token limits/rate limits enabled.
- Use mock mode for local development unless Azure validation is needed.
