# QuestBoard Documentation Assets

This folder collects visual artifacts that make the QuestBoard API easier to showcase.

- `swagger-ui.png` - capture the Swagger UI once the API is running (`https://localhost:5001/swagger`).
- `architecture.mmd` - Mermaid source of the layered architecture diagram (export to `architecture.png`).
- `architecture.png` - exported architecture diagram (optional once rendered).
- `sequence-diagram.mmd` - Mermaid sequence diagram describing a representative workflow.

## Generating Diagrams
```bash
# Install mermaid CLI if you don't already have it
npm install -g @mermaid-js/mermaid-cli

# Export the architecture diagram
mmdc -i docs/architecture.mmd -o docs/architecture.png

# Export the sequence diagram
mmdc -i docs/sequence-diagram.mmd -o docs/sequence-diagram.png
```

> Tip: keep source files (e.g., `.drawio`, `.fig`) beside the rendered images so future updates stay simple.
