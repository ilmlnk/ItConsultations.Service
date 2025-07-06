# ItConsultations OpenApi

This is the public API interface for ItConsultations service.

## Overview

ItConsultations OpenApi provides RESTful endpoints for managing consultations, coaches, students, and articles.

## Getting Started

### Prerequisites

- .NET 8.0 or later
- Valid API key (contact us for access)

### Installation

1. Clone the repository
2. Navigate to `ItConsultations.OpenApi` directory
3. Run the application

```bash
cd ItConsultations.OpenApi
dotnet run
```

### Authentication

All API endpoints require authentication. Use your API key in the Authorization header:

```
Authorization: Bearer YOUR_API_KEY
```

## API Endpoints

### Consultations
- `GET /api/consultations` - Get all consultations
- `GET /api/consultations/{id}` - Get consultation by ID
- `POST /api/consultations` - Create new consultation

### Coaches
- `GET /api/coaches` - Get all coaches
- `GET /api/coaches/{id}` - Get coach by ID

### Students
- `GET /api/students` - Get all students
- `GET /api/students/{id}` - Get student by ID

### Articles
- `GET /api/articles` - Get all articles
- `GET /api/articles/{id}` - Get article by ID

## Rate Limits

- 1000 requests per hour per API key
- Maximum request size: 10MB
- Rate limit headers included in responses

## Support

For API access, support, or commercial licensing, contact: [ilmlnkcorp@gmail.com]

## License

This API is provided under the ItConsultations OpenApi License. See LICENSE.txt for details.

**Note: This is only the API interface. The full business logic and data access layers are proprietary and not included in this public repository.** 