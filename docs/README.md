# PongTron Documentation

Welcome to the documentation for **PongTron**.

## Architecture & Codebase Design

- [**Dependency Web & Presentation Coupling Analysis**](DEPENDENCY_WEB.md)
  - Complete dependency web diagram of all components.
  - In-depth answer to whether core logic is connected to presentation (**Yes, tightly coupled**).
  - Detailed inspection of coupling points (`GameManager`, `Ball`, `ScoringZone`, `ComputerPaddle`).
  - Sequence diagrams of the scoring and game-over flows.
  - Refactoring blueprint & code examples for decoupling core logic from UI and audio using the Observer / Event-driven pattern.
