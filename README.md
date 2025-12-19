# E-Commerce Microservices Sample (.NET 10)

This is a reference implementation of a cloud-native e-commerce platform built with **.NET 10**. The project demonstrates microservices patterns including API Gateway routing, rate limiting, and fault tolerance.



## 🏗 Architecture Overview

The system is composed of the following services:

| Service | Description | Technology |
| :--- | :--- | :--- |
| **API Gateway** | Entry point for all clients. Handles routing and rate limiting. | Ocelot |
| **Auth Service** | Identity provider for JWT issuance and validation. | .NET 10 Web API |
| **Product Service** | Catalog management and inventory. | .NET 10 Web API |
| **Order Service** | Checkout process and order management. | .NET 10 Web API |

---

## 🛠 Features

### 🚀 Resilience with Polly
To ensure the system remains stable during transient failures, we use **Polly** for:
* **Retry Policy:** Automatically retrying requests to downstream services.
* **Circuit Breaker:** Breaking the connection if a service is consistently failing to prevent cascading failures.

### 🚦 API Gateway & Rate Limiting
**Ocelot** is configured to protect our services from traffic spikes.
* **Rate Limiting:** Restricts the number of requests per client within a specific time window.
* **Routing:** Aggregates multiple service endpoints into a single public-facing URL.

---

## 🧪 Unit Testing

The **Product Service** includes unit tests using **xUnit** to ensure business logic reliability.

## 🐳 Docker Orchestration

The project uses **Docker Compose** to manage the lifecycle of all services. This ensures that the Gateway, Identity, Product, and Order services start with the correct network dependencies and environment variables.

### Service Network Map
All services communicate over an internal bridge network. Only the **API Gateway** is exposed to the host machine to ensure security.



### Running the Stack
To build and start the entire infrastructure, run:

```bash
docker-compose up -d --build
