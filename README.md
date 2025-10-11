# VerticalCleanModularMicroservices

### Making Sense of Modern Architectures

> How are you supposed to keep up — and make the right choice — when your boss just wants you to ship features?

This repository accompanies the conference session **“Vertical Clean Modular Microservices?! Making Sense of Modern Architectures”** by [Steve "Ardalis" Smith](https://ardalis.com).  
It demonstrates how a single .NET application can evolve through four common architectural styles — from simple to sophisticated — while keeping the *same domain* and *consistent business logic*.

As your team or app evolves, you can move **from slices to services** with increasing complexity as well as capabilities.

---

## 🧩 Overview

| Stage | Architecture Style | Focus | Project Count |
|--------|--------------------|--------|----------------|
| **01** | 🟢 Vertical Slice | Feature-focused design with Mediator + EF Core + Aspire | 1 project |
| **02** | 🟡 Clean Architecture | Separation of concerns and testable boundaries | 4 projects |
| **03** | 🟠 Modular Monolith | Independent in-process modules and internal events | 4–5 projects |
| **04** | 🔴 Microservices | Independently deployable services orchestrated with Aspire | 3–4 services |

Each stage has its own self-contained **solution and Aspire AppHost**, allowing you to run and compare them independently.

---

## 🧱 Repository Layout

```markdown
VerticalCleanModularMicroservices/
├── 01-VerticalSlice/
│   ├── aspire/OrderDemo.AppHost/
│   └── src/OrderDemo.Api/
│
├── 02-CleanArchitecture/
│   ├── aspire/OrderDemo.AppHost/
│   └── src/(Api, Application, Domain, Infrastructure)/
│
├── 03-ModularMonolith/
│   ├── aspire/OrderDemo.AppHost/
│   └── src/(Api, Modules.Orders, Modules.Products, Modules.Payments)/
│
└── 04-Microservices/
    ├── aspire/OrderDemo.AppHost/
    └── src/(OrderService, ProductService, PaymentService)/
```

Each folder is a **complete, runnable example** of the same domain implemented in a different architecture.

---

## ⚙️ Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0) or later  
- [Docker Desktop](https://www.docker.com/products/docker-desktop/)  
- [Visual Studio 2022 17.10+](https://visualstudio.microsoft.com/vs/) or [Rider / VS Code](https://code.visualstudio.com)  
- [Aspire 9.5+](https://learn.microsoft.com/en-us/dotnet/aspire/overview) for local container orchestration  

Optionally you may find the Aspire App Templates helpful. Install them with:

```bash
dotnet new install Aspire.ProjectTemplates@9.5.1
```

---

## 🚀 Running a Stage

1. Navigate to the desired stage folder:

```bash
cd 01-VerticalSlice/aspire/OrderDemo.AppHost
```

2. Run Aspire:

```bash
dotnet run
```

3. Open the Aspire dashboard in your browser to see:
   - Running SQL Server and API containers  
   - Live logs  
   - Connection strings and health checks  

4. Explore endpoints via Swagger (default `/swagger`).

---

## 🧠 Learning Goals

- Understand the **trade-offs** between common architecture styles.  
- See how **complexity grows intentionally** with project needs.  
- Learn how to **evolve architecture incrementally** without rewriting the system.  
- Observe how **.NET Aspire** can unify local environments across monoliths and microservices.

---

## 🧰 Technologies Used

- **.NET 9 / 10 Preview**
- **EF Core 9** for data access
- **Mediator** (Martin Costello’s library) for in-process messaging
- **Aspire** for local orchestration and observability
- **Swagger / Minimal APIs**
- **OpenTelemetry (OTEL)** (in later stages)
- **Docker + SQL Server**

---

## 📚 Architecture Evolution Summary

| Focus Area | Vertical Slice | Clean Architecture | Modular Monolith | Microservices |
|-------------|----------------|--------------------|------------------|----------------|
| **Code Organization** | By Feature | By Layer | By Module | By Service |
| **Coupling** | Tight | Controlled | Explicit | Loosely coupled |
| **Deployability** | Single Unit | Single Unit | Single Unit | Independent |
| **Testability** | High (feature-level) | High (unit-level) | High (module-level) | Complex (integration-heavy) |
| **When to Use** | Small teams, fast iteration | Medium teams, maintainability | Growing orgs with domain boundaries | Large scale, independent releases |

---

## 🧭 Suggested Presentation Flow

Each stage can be demoed in isolation, or you can use Git branches/tags to move between stages:

| Branch | Description |
|---------|-------------|
| `stage-01` | Vertical Slice Architecture |
| `stage-02` | Clean Architecture |
| `stage-03` | Modular Monolith |
| `stage-04` | Microservices |

```bash
git checkout stage-02
```

---

## 🧩 Credits

Created and maintained by **[Steve “Ardalis” Smith](https://ardalis.com)**  
Microsoft MVP • Software Architect • Trainer at [NimblePros](https://nimblepros.com)
