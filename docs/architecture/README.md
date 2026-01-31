# FinSkew Architecture Documentation

> **Version**: 1.0  
> **Last Updated**: February 2026  
> **Status**: Current State Documentation

## Table of Contents

- [Overview](#overview)
- [System Context](#system-context)
- [Architecture Overview](#architecture-overview)
- [Technology Stack](#technology-stack)
- [Deployment Model](#deployment-model)
- [Key Architectural Principles](#key-architectural-principles)
- [Additional Documentation](#additional-documentation)

## Overview

FinSkew is a web-based financial calculator application designed as an educational tool for individuals to understand and manage their finances better. The application is primarily targeted at Indian users, featuring INR currency, Indian numbering system, and India-centric financial defaults.

### Purpose

The application provides various financial calculators to help users:

- Calculate simple and compound interest
- Plan systematic investments (SIP)
- Model systematic withdrawals (SWP)
- Evaluate systematic transfers (STP)
- Analyze lump sum investments

### Key Characteristics

- **Client-Side Application**: Runs entirely in the browser with no backend dependencies
- **Real-Time Calculations**: Instant results with debounced updates as users type
- **Responsive Design**: Optimized for desktop, tablet, and mobile devices
- **Accessibility-First**: WCAG AA compliant with comprehensive keyboard navigation
- **Educational Focus**: Clear visualizations and explanations for financial concepts

## System Context

```
┌─────────────────────────────────────────────────────────────┐
│                        User's Browser                        │
│                                                              │
│  ┌────────────────────────────────────────────────────────┐ │
│  │           FinSkew Blazor WASM Application              │ │
│  │                                                         │ │
│  │  ┌──────────────┐  ┌──────────────┐  ┌─────────────┐ │ │
│  │  │  Calculator  │  │  Calculator  │  │ Calculator  │ │ │
│  │  │    Pages     │  │   Services   │  │  ViewModels │ │ │
│  │  └──────────────┘  └──────────────┘  └─────────────┘ │ │
│  │                                                         │ │
│  │  ┌──────────────┐  ┌──────────────┐                  │ │
│  │  │   MudBlazor  │  │    Blazor    │                  │ │
│  │  │      UI      │  │  Framework   │                  │ │
│  │  └──────────────┘  └──────────────┘                  │ │
│  └────────────────────────────────────────────────────────┘ │
└─────────────────────────────────────────────────────────────┘
                            │
                            │ HTTPS
                            ▼
┌─────────────────────────────────────────────────────────────┐
│              Azure Static Web Apps (CDN)                     │
│                                                              │
│  - Static file hosting                                       │
│  - Global CDN distribution                                   │
│  - Automatic HTTPS                                           │
│  - Custom domain support                                     │
└─────────────────────────────────────────────────────────────┘
```

### Current Architecture Scope

**In Scope (Implemented)**:

- Blazor WebAssembly frontend application
- MudBlazor UI component library
- Client-side calculation logic
- Azure Static Web Apps deployment
- Infrastructure as Code (Bicep templates)

**Out of Scope (Future)**:

- Backend API services (planned: Azure Functions)
- Database layer (planned: Azure Cosmos DB)
- User authentication and authorization
- Data persistence and user profiles
- Server-side calculations or validations

## Architecture Overview

FinSkew follows a **client-side single-page application (SPA)** architecture built with Blazor WebAssembly. The application is compiled to WebAssembly and runs entirely in the user's browser, requiring no backend infrastructure for its core functionality.

### High-Level Architecture

```
User Browser
    │
    ├─── [Blazor WASM Runtime]
    │         │
    │         ├─── Components (Razor Pages)
    │         │       ├─── Layout (MainLayout, NavMenu)
    │         │       ├─── Pages (Calculator Pages)
    │         │       └─── Shared (Reusable Components)
    │         │
    │         ├─── Models
    │         │       └─── ViewModels
    │         │               ├─── InputModels (with validation)
    │         │               └─── ResultModels (calculations)
    │         │
    │         ├─── Services
    │         │       ├─── Interfaces (contracts)
    │         │       └─── Implementations (business logic)
    │         │
    │         └─── Constants (configuration values)
    │
    └─── [MudBlazor Component Library]
```

### Architecture Layers

1. **Presentation Layer**: Razor components for UI rendering and user interaction
2. **Business Logic Layer**: Service classes containing calculation algorithms
3. **Data Layer**: ViewModels for input validation and result presentation
4. **Infrastructure Layer**: MudBlazor components, Blazor runtime, .NET libraries

## Technology Stack

### Core Framework

- **.NET 10.0**: Latest .NET version with performance and language improvements
- **Blazor WebAssembly**: Client-side framework for building interactive web UIs with C#
- **C# 13**: Modern language features with nullable reference types enabled

### UI Framework

- **MudBlazor 8.15.0**: Material Design component library for Blazor
  - Provides comprehensive UI components (forms, charts, navigation)
  - Built-in responsive design support
  - Accessibility features out of the box
  - Consistent theming and styling

### Key Libraries

- **MithunShanbhag.Nucleus 1.0.0-alpha.4**: Custom utility library (author's personal package)
- **Microsoft.AspNetCore.Components.WebAssembly**: Core Blazor WASM runtime

### Development Tools

- **Visual Studio / VS Code**: Primary IDEs
- **dotnet CLI**: Build, test, and deployment tooling
- **.NET formatting tools**: Code style enforcement

### Infrastructure

- **Azure Static Web Apps**: Hosting platform for static web content
- **Azure Bicep**: Infrastructure as Code for Azure resource provisioning
- **GitHub Actions**: CI/CD pipeline for automated deployments

### Testing Stack

- **xUnit**: Unit testing framework
- **FluentAssertions**: Assertion library for readable test assertions
- **Bogus**: Test data generation library
- **Playwright**: End-to-end browser testing

## Deployment Model

### Infrastructure

FinSkew is deployed as an **Azure Static Web App**, which provides:

- **Static Content Hosting**: Efficient delivery of HTML, CSS, JavaScript, and WebAssembly files
- **Global CDN**: Distributed caching for low-latency access worldwide
- **Automatic HTTPS**: Built-in SSL/TLS certificate management
- **Custom Domains**: Support for branded domain names
- **Preview Environments**: Automatic staging deployments for pull requests

### Deployment Pipeline

```
Developer
    │
    ├─── Push to 'main' branch
    │
    ▼
GitHub Actions Workflow
    │
    ├─── Checkout code
    ├─── Build .NET project (dotnet build)
    ├─── Run tests (dotnet test)
    ├─── Publish WASM app (dotnet publish)
    │
    ▼
Azure Static Web Apps
    │
    ├─── Deploy static assets to CDN
    ├─── Update routing configuration
    ├─── Invalidate cache
    │
    ▼
Production (finskew.azurestaticapps.net)
```

### Infrastructure as Code

The infrastructure is defined using **Azure Bicep** templates located in the `infra/` directory:

- **createResourceGroups.bicep**: Creates Azure resource groups
- **createResources.bicep**: Provisions the Static Web App resource
- **createExtensionResources.bicep**: Configures additional resources or extensions

This approach ensures:

- **Reproducibility**: Infrastructure can be recreated from code
- **Version Control**: Infrastructure changes are tracked in Git
- **Documentation**: Bicep files serve as living documentation
- **Automation**: Deployments can be fully automated

### Deployment Configuration

- **Trigger**: Automatic deployment on push to `main` branch
- **Build**: .NET 10.0 build with WebAssembly optimization
- **Output**: Static files deployed to Azure CDN
- **Environment**: Production environment only (no staging/dev environments currently)

## Key Architectural Principles

### 1. Client-Side First

All computation and rendering happens in the browser. This eliminates server costs, reduces latency, and enables offline functionality (with future service worker implementation).

**Trade-offs**:

- ✅ Zero server costs for compute
- ✅ Instant user interactions
- ✅ Simplified deployment
- ❌ Initial load time (WebAssembly download)
- ❌ No data persistence without future backend

### 2. Component-Based Design

The application is built using reusable Razor components following the single responsibility principle. Each calculator is an isolated component with its own state and logic.

**Benefits**:

- Easy to add new calculators
- Consistent UI patterns
- Testable in isolation
- Maintainable and modular

### 3. Input/Result Separation

Each calculator separates concerns using distinct view models:

- **InputViewModel**: User inputs with validation attributes
- **ResultViewModel**: Calculated outputs and chart data

This separation enables:

- Clear data flow
- Independent validation and calculation logic
- Easier testing
- Type-safe contracts

### 4. Real-Time Calculation with Debouncing

Calculations update automatically as users type, with debouncing to prevent excessive computation during rapid input changes.

**User Experience**:

- No "Calculate" button needed
- Immediate feedback
- Smooth, non-blocking updates

### 5. Multiple Route Aliases

Each calculator registers multiple URL routes for better SEO and user convenience:

- Primary route: `/simple-interest-calculator`
- Short form: `/sic`
- Variations: `/simpleinterestcalculator`, `/simple-interest-calc`

This improves:

- Search engine discoverability
- User memorability
- Link shareability

### 6. Accessibility by Design

Every component includes:

- ARIA labels and roles
- Keyboard navigation support
- WCAG AA color contrast
- Semantic HTML structure
- Screen reader compatibility

### 7. Indian Market Localization

The application is tailored for Indian users with:

- INR (₹) currency throughout
- Indian numbering system (commas every 2 digits after first 3)
- India-centric interest rate defaults
- Culturally relevant examples and language

## Additional Documentation

For detailed information on specific architectural aspects, see:

- **[Frontend Architecture](./frontend-architecture.md)**: Blazor WASM structure, components, routing, and state management
- **[Design Patterns](./design-patterns.md)**: Common patterns used throughout the application
- **[Project Structure](./project-structure.md)**: Directory organization and file conventions
- **[Deployment](./deployment.md)**: Azure Static Web Apps deployment and CI/CD pipeline

For functional specifications of individual calculators, see:

- **[Calculator Specifications](../specs/README.md)**: Detailed requirements for each calculator type

## Architecture Evolution

This documentation reflects the **current state** of the FinSkew architecture as a purely client-side application. Future architectural enhancements may include:

- Backend API layer using Azure Functions
- Data persistence using Azure Cosmos DB
- User authentication and profile management
- Advanced features requiring server-side processing

When these enhancements are implemented, this documentation will be updated to reflect the evolved architecture.
