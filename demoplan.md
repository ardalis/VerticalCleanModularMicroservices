# Demo Plan for Conference Talk

This document outlines the high-level plan for building the demos for the conference talk. The demos will progress through the following architectural styles:

1. [x] Vertical Slices
2. [ ] Clean Architecture
3. [ ] Modular Monoliths
4. [ ] Microservices

Each demo will be placed in a numbered subfolder (e.g., `01-VerticalSlice`).

## Common Demo Requirements

- [ ] **Development Environment**: Ensure all demos are built using .NET 9 or later.
- [ ] **Aspire**: All demo apps will use Aspire 9.5.1 or later, with Aspire's `AspireAppHostBuilderExtensions` package installed.
- [ ] **Database**: Use a SQL Server database in an Aspire-managed docker container.
- [ ] **API Design**: All demos should expose RESTful APIs for core functionality, and support Scalar for API documentation. APIs will always return DTOs, not entities or domain types.
- [ ] **Testing**: Include unit tests and integration tests for key features.
- [ ] **Documentation**: Provide a `README.md` in each demo folder explaining the architecture and how to run the demo.
- [ ] **Domain**: The domain will be for an ecommerce site. The key operations that the demo will need to model include the basic steps a user would perform to make a purchase: ListProducts, GetProductById, AddToCart, ViewCart, Checkout, ConfirmPurchase, GetOrderById, and ListOrders. The domain will support guest checkouts and will not include any admin capabilities; identity/authn/authz is out of scope. Cart and Order should be modeled independently from one another. An Order can be created from a Cart, but a Cart is not an Order. Carts should be marked Deleted once an Order has been associated with them.
- [ ] **Orders**: Should have a properties for DatePaid and PaymentReference which should be populated by the ConfirmPurchase endpoint. Endpoints for viewing orders should indicate whether they are Paid and if so list the date and reference. The payment reference should be populated from the response back from the mock payment service.

## Steps for Creating Each Demo

### 1. Vertical Slices
- [x] Create a new folder `01-VerticalSlice`.
- [x] Implement a simple application using the vertical slice pattern.
- [x] Focus on feature-based organizational design.
- [x] Use a single ASP.NET project for all APIs.
- [x] Include minimal dependencies and keep the architecture straightforward.
- [x] Do not use interfaces for data access - use EF Core directly where needed.
- [x] Use minimal APIs and extension methods to separate them from Program.cs.
- [x] Put all request/response/dto types that are not shared in the same file as the endpoint that uses/calls them.
- [x] Use a root level folder for each feature. Name each folder after the entity it works with plus "Feature" suffix (e.g. "CartFeature")

### 2. Clean Architecture
- [x] Create a new folder `02-CleanArchitecture`.
- [x] Copy aspire folder and ServiceDefaults project from vertical slice demo.
- [x] Create solution file `OrderDemo.CleanArch.sln`.
- [x] Set up Directory.Build.props and Directory.Packages.props with central package management.
- [x] Introduce projects: Core, Use Cases, Infrastructure, and Web.
- [x] Add project references following dependency inversion (Core has no deps, UseCases->Core, Infrastructure->Core+UseCases, Web->UseCases+Infrastructure+ServiceDefaults).
- [x] Update AppHost to reference OrderDemo.Web project.
- [x] Verify solution builds successfully.
- [ ] Refactor the vertical slice demo to follow clean architecture principles. Use the sample folder in github.com/ardalis/CleanArchitecture and on local disk at C:\dev\github-ardalis\CleanArchitecture\sample as a guide.
- [ ] Use Repository and Specification patterns for data access. Use Ardalis.Specification.EntityFrameworkCore package.

### 3. Modular Monoliths
- [ ] Create a new folder `03-ModularMonolith`.
- [ ] Refactor the clean architecture demo to introduce modular boundaries.
- [ ] Define modules for distinct business capabilities (e.g., `Order`, `Inventory`).
- [ ] Ensure modules communicate through well-defined interfaces.

### 4. Microservices
- [ ] Create a new folder `04-Microservices`.
- [ ] Split the modular monolith into independent microservices.
- [ ] Use a lightweight message broker (e.g., RabbitMQ) for inter-service communication.
- [ ] Implement service discovery and API gateways as needed.

---

This plan will evolve as the demos are developed. Each step builds upon the previous one, demonstrating the progression from simple to complex architectures.