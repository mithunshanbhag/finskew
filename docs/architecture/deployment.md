# Deployment

> **Document Purpose**: Detailed documentation of FinSkew's deployment process, infrastructure, and CI/CD pipeline.

## Table of Contents

- [Overview](#overview)
- [Infrastructure Overview](#infrastructure-overview)
- [Azure Static Web Apps](#azure-static-web-apps)
- [Infrastructure as Code (Bicep)](#infrastructure-as-code-bicep)
- [CI/CD Pipeline](#cicd-pipeline)
- [Build Process](#build-process)
- [Deployment Process](#deployment-process)
- [Environment Configuration](#environment-configuration)
- [Monitoring and Diagnostics](#monitoring-and-diagnostics)
- [Rollback Strategy](#rollback-strategy)

## Overview

FinSkew uses a modern, automated deployment pipeline that emphasizes:

- **Infrastructure as Code**: All infrastructure defined in Bicep templates
- **Continuous Deployment**: Automatic deployment on every push to `main`
- **Serverless Hosting**: Azure Static Web Apps for cost-effective, scalable hosting
- **Zero-Downtime Deployments**: Blue-green deployment pattern built into Azure Static Web Apps
- **Version Control**: All deployment configuration tracked in Git

### Deployment Flow

```
Developer
    │
    └─── Push to main branch
            │
            ▼
    GitHub Actions Triggered
            │
            ├─── Checkout code
            ├─── Build .NET project
            ├─── Run tests
            ├─── Publish WASM bundle
            │
            ▼
    Deploy to Azure Static Web Apps
            │
            ├─── Upload static assets to CDN
            ├─── Configure routing
            ├─── Validate deployment
            │
            ▼
    Production (live at URL)
```

## Infrastructure Overview

### Architecture Diagram

```
                    Internet
                        │
                        ▼
            ┌───────────────────────┐
            │  Azure Static Web App │
            │   (Global CDN/Edge)   │
            └───────────────────────┘
                        │
                        ├─── Static Files (HTML, CSS, JS, WASM)
                        ├─── Routing Configuration
                        └─── HTTPS/SSL Termination
                                │
                                ▼
                        User's Browser
                        (Blazor WASM App)
```

### Components

1. **Azure Static Web Apps**: Hosts the static files and serves the application globally via CDN
2. **GitHub Actions**: CI/CD automation platform
3. **Azure Resource Groups**: Logical containers for Azure resources
4. **Bicep Templates**: Infrastructure definitions

### Current Environment

- **Environment**: Production only (no staging/dev environments)
- **Region**: Determined by Azure Static Web Apps automatic region selection
- **Hosting Plan**: Azure Static Web Apps Free or Standard tier
- **Domain**: `*.azurestaticapps.net` (custom domain support available)

## Azure Static Web Apps

### What is Azure Static Web Apps?

Azure Static Web Apps is a service that automatically builds and deploys full-stack web apps from a code repository. For FinSkew, it provides:

- **Global CDN**: Content distributed across Azure's edge network for low latency
- **Automatic HTTPS**: Free SSL/TLS certificates with auto-renewal
- **Custom Domains**: Support for branded domain names (e.g., `finskew.com`)
- **Integrated CI/CD**: Direct integration with GitHub Actions
- **Preview Environments**: Automatic staging deployments for pull requests (future)
- **Free Tier**: Generous free tier suitable for small/medium projects

### Key Features Used

**Static Content Hosting**:

- HTML, CSS, JavaScript, WebAssembly files
- Efficient caching and compression (Brotli/Gzip)
- Immutable assets with content hashing

**Routing Configuration**:

- SPA fallback routing (all routes serve `index.html`)
- Custom headers and redirects (if needed)
- MIME type configuration for WebAssembly files

**Performance Optimizations**:

- CDN edge caching for global distribution
- HTTP/2 and HTTP/3 support
- Brotli compression for WASM files

### Service Limits (Free Tier)

- **Bandwidth**: 100 GB/month
- **Storage**: 0.5 GB
- **Custom Domains**: 2
- **Staging Environments**: 3

For FinSkew's current scale, the free tier is sufficient.

## Infrastructure as Code (Bicep)

### What is Bicep?

Bicep is a domain-specific language (DSL) for deploying Azure resources declaratively. It's:

- Simpler than ARM JSON templates
- Strongly typed with IntelliSense support
- Compiled to ARM templates for deployment
- Version-controlled alongside application code

### Bicep Template Structure

```
infra/
├── createResourceGroups.bicep      # Resource group creation
├── createResources.bicep           # Main infrastructure (Static Web App)
└── createExtensionResources.bicep  # Additional resources/extensions
```

#### createResourceGroups.bicep

**Purpose**: Define Azure Resource Groups for logical organization.

**Contents**:

- Resource group names
- Region/location
- Tags (e.g., environment, project)

**Example Structure**:

```bicep
targetScope = 'subscription'

resource resourceGroup 'Microsoft.Resources/resourceGroups@2021-04-01' = {
  name: 'rg-finskew-prod'
  location: 'eastus'
  tags: {
    project: 'finskew'
    environment: 'production'
  }
}
```

#### createResources.bicep

**Purpose**: Provision the Azure Static Web App resource.

**Contents**:

- Static Web App definition
- SKU (pricing tier)
- Build configuration
- Custom domain settings (if applicable)

**Example Structure**:

```bicep
resource staticWebApp 'Microsoft.Web/staticSites@2022-03-01' = {
  name: 'swa-finskew'
  location: 'East US 2'
  sku: {
    name: 'Free'
    tier: 'Free'
  }
  properties: {
    repositoryUrl: 'https://github.com/mithunshanbhag/finskew'
    branch: 'main'
    buildProperties: {
      appLocation: 'src/FinSkew.Ui'
      outputLocation: 'wwwroot'
    }
  }
}
```

#### createExtensionResources.bicep

**Purpose**: Define additional resources or extensions (e.g., Application Insights, custom domains).

**Current State**: Minimal or placeholder configuration.

**Future Use Cases**:

- Application Insights for monitoring
- Custom domain bindings
- Azure Front Door integration (for advanced routing)

### Deployment of Bicep Templates

**Deployment Command** (manual):

```powershell
# Deploy resource groups
az deployment sub create `
  --location eastus `
  --template-file infra/createResourceGroups.bicep

# Deploy resources
az deployment group create `
  --resource-group rg-finskew-prod `
  --template-file infra/createResources.bicep
```

**Typical Flow**:

1. Bicep files are edited locally or via PRs
2. Changes committed to Git
3. Infrastructure changes deployed via Azure CLI or GitHub Actions
4. Infrastructure changes are versioned and auditable

### Benefits of IaC with Bicep

- **Reproducibility**: Entire infrastructure recreated from code
- **Version Control**: Infrastructure changes tracked in Git
- **Auditability**: Clear history of infrastructure changes
- **Collaboration**: Infrastructure changes reviewed via PRs
- **Documentation**: Bicep serves as living documentation

## CI/CD Pipeline

### GitHub Actions Workflow

**Location**: `.github/workflows/deploy.yml`

**Trigger**: Push to `main` branch

**Workflow Structure**:

```yaml
name: finskew-deploy

on:
  push:
    branches:
      - main

jobs:
  deploy:
    uses: ./.github/workflows/deploy-template.yml
    secrets: inherit
```

The main workflow delegates to a reusable template workflow (`deploy-template.yml`) to keep configuration DRY.

### Workflow Steps

1. **Checkout Code**

   ```yaml
   - uses: actions/checkout@v4.1.1
   ```

   Clones the repository to the GitHub Actions runner.

2. **Setup .NET**

   ```yaml
   - uses: actions/setup-dotnet@v3
     with:
       dotnet-version: '10.0.x'
   ```

   Installs .NET 10 SDK on the runner.

3. **Restore Dependencies**

   ```bash
   dotnet restore
   ```

   Downloads NuGet packages.

4. **Build Application**

   ```bash
   dotnet build --configuration Release --no-restore
   ```

   Compiles the Blazor WASM project in Release mode.

5. **Run Tests**

   ```bash
   dotnet test --no-build --verbosity normal
   ```

   Executes unit tests (E2E tests skipped in CI currently).

6. **Publish WASM Application**

   ```bash
   dotnet publish src/FinSkew.Ui/FinSkew.Ui.csproj `
     --configuration Release `
     --output ./publish
   ```

   Produces optimized WASM bundle in `./publish/wwwroot/`.

7. **Deploy to Azure Static Web Apps**

   ```yaml
   - uses: Azure/static-web-apps-deploy@v1
     with:
       azure_static_web_apps_api_token: ${{ secrets.AZURE_STATIC_WEB_APPS_API_TOKEN }}
       repo_token: ${{ secrets.GITHUB_TOKEN }}
       action: 'upload'
       app_location: 'publish/wwwroot'
   ```

   Uploads static files to Azure Static Web Apps CDN.

### Secrets Configuration

**Required Secrets** (stored in GitHub repository settings):

- **`AZURE_STATIC_WEB_APPS_API_TOKEN`**: Deployment token generated by Azure Static Web Apps
  - Obtained from Azure portal after creating Static Web App
  - Grants permission to deploy to the specific Static Web App resource

**How to Set Secrets**:

1. Navigate to GitHub repository → Settings → Secrets and variables → Actions
2. Add new repository secret
3. Paste the API token from Azure portal

### Deployment Strategy

**Blue-Green Deployment** (Automatic):

- Azure Static Web Apps manages staging and production environments internally
- New deployment uploaded to staging slot
- Health checks performed automatically
- Traffic switched to new version atomically
- Old version kept for instant rollback

**Zero-Downtime**: Users never experience downtime during deployments.

## Build Process

### .NET Build Configuration

**Target Framework**: `net10.0`

**Build Configuration**: `Release`

- Optimizations enabled
- Debug symbols removed
- Trimming and AOT (Ahead-of-Time compilation) may be enabled in future

**Output**:

- `FinSkew.Ui.dll` (compiled assembly)
- `blazor.boot.json` (manifest of files to load)
- `dotnet.wasm` (.NET runtime as WebAssembly)
- `*.wasm` (additional runtime files)
- Static assets (CSS, images, fonts)

### Build Optimizations

**Current**:

- Release configuration (`-c Release`)
- Assembly trimming (unused code removed)
- Brotli compression of WASM files

**Future Enhancements**:

- AOT compilation for faster startup
- Lazy loading of calculator pages
- Tree shaking of unused libraries

### Build Artifacts

After `dotnet publish`, the `wwwroot/` folder contains:

```
wwwroot/
├── _framework/                 # Blazor runtime and app DLLs
│   ├── blazor.boot.json
│   ├── dotnet.*.wasm
│   ├── FinSkew.Ui.dll
│   └── ... (dependencies)
├── css/                        # Stylesheets
├── images/                     # Images
├── index.html                  # Entry point
├── favicon.svg
└── ...
```

This entire folder is deployed to Azure Static Web Apps.

## Deployment Process

### Deployment Stages

1. **Code Push**: Developer pushes to `main` branch
2. **Trigger**: GitHub Actions workflow starts automatically
3. **Build**: .NET project compiled and tests run
4. **Publish**: WASM bundle created in `publish/wwwroot/`
5. **Upload**: Static files uploaded to Azure Static Web Apps API
6. **CDN Distribution**: Files replicated across Azure CDN edge nodes
7. **Validation**: Health checks ensure app is responsive
8. **Traffic Switch**: New version goes live
9. **Notification**: GitHub status check updates (success/failure)

### Deployment Duration

**Typical Timeline**:

- Build: ~1-2 minutes
- Tests: ~10-20 seconds
- Publish: ~30 seconds
- Upload to Azure: ~1-2 minutes
- CDN propagation: ~1-5 minutes

**Total**: ~5-10 minutes from push to live.

### Deployment Validation

**Automatic Checks**:

- Build succeeds
- Tests pass
- Static Web App API responds with 200 OK
- `index.html` is accessible

**Manual Validation** (recommended after deployment):

- Navigate to production URL
- Test one calculator (e.g., Simple Interest)
- Verify calculations are correct
- Check browser console for errors

## Environment Configuration

### Application Configuration

**Current**: No environment-specific configuration needed (static client-side app).

**Future** (when backend is added):

- **Development**: `appsettings.Development.json` with local API URLs
- **Production**: `appsettings.Production.json` with production API URLs
- **Secrets**: Azure Key Vault or GitHub Secrets for sensitive data

### Static Web App Configuration

**Configuration File**: `staticwebapp.config.json` (optional, located in `wwwroot/`)

**Example Configuration**:

```json
{
  "navigationFallback": {
    "rewrite": "/index.html",
    "exclude": ["/_framework/*", "/css/*", "/images/*"]
  },
  "mimeTypes": {
    ".wasm": "application/wasm"
  },
  "globalHeaders": {
    "cache-control": "public, max-age=31536000, immutable"
  }
}
```

**Key Settings**:

- **navigationFallback**: SPA routing (all routes serve `index.html`)
- **mimeTypes**: Ensure WASM files have correct MIME type
- **globalHeaders**: Cache control for static assets

### Custom Domains

**To Add Custom Domain** (e.g., `finskew.com`):

1. Navigate to Azure Static Web App → Custom domains
2. Add custom domain
3. Configure DNS records (CNAME or A record)
4. Azure provisions SSL certificate automatically
5. Domain becomes accessible within 5-10 minutes

## Monitoring and Diagnostics

### Current Monitoring

**GitHub Actions**:

- Build/deployment logs available in Actions tab
- Email notifications on build failures
- Status badges for README (optional)

**Azure Portal**:

- Request metrics (requests/sec, bandwidth)
- Error rates (4xx, 5xx responses)
- Geographic distribution of users

### Future Monitoring (Recommended)

**Application Insights**:

- Client-side telemetry (page views, exceptions)
- Performance metrics (load time, time-to-interactive)
- Custom events (e.g., calculator usage)
- Real-time monitoring dashboard

**Setup**:

1. Provision Application Insights via Bicep
2. Add Application Insights SDK to Blazor app
3. Configure telemetry in `Program.cs`
4. View telemetry in Azure Portal

### Logging

**Client-Side Logging**:

- Browser console (development)
- Application Insights (production, future)

**Build Logs**:

- GitHub Actions logs (retained for 90 days)

## Rollback Strategy

### Automatic Rollback

Azure Static Web Apps **does not support automatic rollback** on failure. Deployments are atomic but don't auto-revert.

### Manual Rollback

**Option 1: Revert Git Commit**

```bash
# Revert the commit that caused issues
git revert <commit-sha>
git push origin main

# Triggers automatic redeployment of previous working version
```

**Option 2: Redeploy Previous Commit**

```bash
# Checkout previous working commit
git checkout <previous-commit-sha>
git push origin main --force

# Forces redeployment of older version
```

**Option 3: Azure Portal Revert** (if feature available):

- Navigate to Azure Static Web App → Deployments
- Select previous successful deployment
- Click "Activate" to promote to production

### Rollback Best Practices

- **Test thoroughly** before deploying to `main`
- **Monitor deployment** in first 10 minutes after going live
- **Keep commit messages descriptive** for easy identification
- **Tag releases** for easy rollback references

  ```bash
  git tag -a v1.0.0 -m "Release 1.0.0"
  git push origin v1.0.0
  ```

### Prevention Strategies

1. **Branch Protection**: Require PR reviews before merging to `main`
2. **Staging Environment**: Test in preview environment first (future)
3. **Automated Tests**: Comprehensive unit and E2E tests catch issues early
4. **Feature Flags**: Toggle features on/off without redeployment (future)

## Summary

FinSkew's deployment process emphasizes:

- **Automation**: Fully automated CI/CD via GitHub Actions
- **Infrastructure as Code**: Bicep templates for reproducible infrastructure
- **Serverless Hosting**: Azure Static Web Apps for cost-effective, scalable hosting
- **Fast Deployments**: ~5-10 minutes from commit to live
- **Zero Downtime**: Blue-green deployment ensures smooth transitions
- **Simplicity**: Minimal configuration and secrets management

This deployment strategy provides a solid foundation for continuous delivery and can scale as the application grows.
