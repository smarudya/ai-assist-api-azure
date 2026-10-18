# Safety Notes

## PII handling
- Do not send personal, customer, or regulated data in prompts.
- This sample does not persist prompts/responses; add retention controls before production use.

## Prompt injection awareness
- Treat LLM output as untrusted.
- Never execute returned code or commands automatically.
- Add downstream allow-lists and human review for high-impact actions.

## Cost controls
- Mock provider is the default to avoid accidental spend.
- Azure requests use low `max_tokens` and conservative temperature.
- Keep rate limits enabled and monitor token usage in Azure metrics.
