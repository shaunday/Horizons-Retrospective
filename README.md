# Horizons Retrospective - Trading Journal & Portfolio Management System

**Demo:** [Trading Journal Demo](https://hsr.mywebthings.xyz/)

## Overview

**Horizons Retrospective** is a comprehensive trading journal and portfolio management system designed to help traders track, analyze, and optimize their trading performance. The system evolves through multiple stages, each building upon the previous to create a robust trading ecosystem.

### Development Stages

**Stage 1: Trading Journal (Current)**
- **Core Functionality**: Complete trading journal system allowing users to add, manage, and track trade ideas and ongoing positions
- **Features**: Trade entry, position management, performance tracking, and historical analysis
- **Status**: ✅ **Complete - Polishing**

**Stage 2: Portfolio Ledger & Analytics (Planned)**
- **Ledger Management**: Comprehensive broker account management with withdraw/deposit tracking and currency conversion
- **Position Snapshots**: Time-stamped portfolio snapshots for historical performance analysis
- **Visual Analytics**: Interactive pie charts and allocation breakdowns per timestamp
- **Status**: 🚧 **Planning Phase**

**Stage 3: Advanced Trading Tools (Future)**
- **Price Alerts**: Real-time price monitoring and notification system
- **Risk Management**: Advanced position sizing and risk calculation tools
- **Status**: 📋 **Conceptual**

## Part 1: Client Architecture

### Technology Stack
- **Frontend Framework**: React 18 with modern hooks and functional components
- **State Management**: React Query (TanStack Query) for server state management
- **Styling**: Mantine UI components + Tailwind CSS for responsive design
- **Build Tool**: Vite for fast development and optimized builds

### Data Flow & Cache Management Architecture

The client implements a sophisticated caching strategy to optimize performance and user experience:

#### 1. **Initial Data Prefetch Strategy** (`useFetchAndCacheTrades`)
- **Purpose**: Preloads all trade data on application startup or window refocus
- **Implementation**: Fetches complete trade dataset and populates React Query cache
- **Benefits**: Eliminates loading states for subsequent navigation, improves perceived performance

#### 2. **Hierarchical Data Access** (`useTrade` hook)
- **Location**: JournalContainer component (one level below App.js)
- **Function**: Retrieves trade data from cache or fetches if missing
- **Cache Strategy**: Intelligent cache updates with new data while maintaining existing entries
- **Performance**: Leverages prefetched data for instant access

#### 3. **Real-time Data Updates** (`useAddTrade`)
- **Functionality**: Handles new trade submissions with optimistic updates
- **Cache Management**: Updates both trade ID list and individual trade caches simultaneously
- **User Experience**: Immediate UI feedback while ensuring data consistency

### Key Benefits
- **Reduced API Calls**: Intelligent caching minimizes redundant network requests
- **Improved Performance**: Prefetching eliminates loading delays for core data
- **Better UX**: Smooth navigation and instant data access across the application

## Part 2: Server Architecture

### Technology Stack
- **Backend Framework**: ASP.NET Core 9.0 with modern .NET features
- **Database**: PostgreSQL with Entity Framework Core for data access
- **API Design**: RESTful API with comprehensive endpoint coverage
- **Authentication**: JWT-based authentication with role-based access control
- **Deployment**: Containerized deployment with Docker support

### Database Design & Entity Architecture

The system implements a well-structured entity model designed for scalability and performance:

![Database Entity Diagram](https://github.com/user-attachments/assets/37b0def3-7901-4748-b2e8-2acb93e9d59e)

#### Core Entities
- **TradingJournal**: Central entity for trade management and tracking
- **TradePositions**: Individual trade entries with comprehensive metadata
- **UserManagement**: Secure user authentication and authorization
- **PerformanceMetrics**: Calculated performance indicators and analytics

#### Key Features
- **Relational Integrity**: Proper foreign key relationships and constraints
- **Audit Trail**: Comprehensive logging and change tracking
- **Scalability**: Optimized database design for high-performance trading operations
- **Data Consistency**: ACID compliance with proper transaction management

### API Architecture
- **RESTful Design**: Standardized HTTP methods and status codes
- **Versioning**: API versioning for backward compatibility
- **Documentation**: Swagger/OpenAPI integration for developer experience
- **Rate Limiting**: Protection against abuse and overload
- **Caching**: Strategic caching at multiple levels for optimal performance

### Security & Performance
- **Authentication**: JWT tokens with secure refresh mechanisms
- **Authorization**: Role-based access control (RBAC) implementation
- **Data Protection**: Encrypted data transmission and storage
- **Performance**: Optimized database queries and connection pooling
- **Monitoring**: Comprehensive logging and performance metrics