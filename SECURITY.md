# Security Guidelines

## Sensitive Information

This project contains sensitive information that should never be committed to version control.

### Files to Never Commit

- `appsettings.Production.json`
- `appsettings.Staging.json`
- `appsettings.QA.json`
- `firebase-service-account.json`
- `*.key` files
- `*.pem` files
- `.env` files
- Database backup files (`*.bak`, `*.sql`)
- SSL certificates
- Private keys

### Setup Instructions

1. **Copy example files:**
   ```bash
   cp appsettings.Production.example.json appsettings.Production.json
   cp firebase-service-account.example.json firebase-service-account.json
   ```

2. **Fill in your actual values:**
   - Replace `YOUR_SERVER` with your database server
   - Replace `YOUR_DATABASE` with your database name
   - Replace `YOUR_USER` and `YOUR_PASSWORD` with your database credentials
   - Replace Firebase configuration with your actual Firebase project details

3. **Verify files are ignored:**
   ```bash
   git status
   ```
   The sensitive files should not appear in the output.

### Environment Variables

For additional security, consider using environment variables:

```bash
export ConnectionStrings__DefaultConnection="your-connection-string"
export Firebase__ProjectId="your-project-id"
export Jwt__Secret="your-jwt-secret"
```

### Database Security

- Use strong passwords for database accounts
- Limit database user permissions to minimum required
- Use connection pooling
- Enable SSL/TLS for database connections
- Regularly rotate credentials

### Firebase Security

- Keep Firebase service account keys secure
- Use Firebase Security Rules
- Enable authentication methods only when needed
- Monitor Firebase usage and costs

### JWT Security

- Use strong, random secrets (at least 32 characters)
- Set appropriate expiration times
- Validate tokens on every request
- Use HTTPS in production

### Logging Security

- Never log sensitive information (passwords, tokens, etc.)
- Use structured logging
- Implement log rotation
- Monitor logs for security events

### Deployment Security

- Use different configurations for different environments
- Use Azure Key Vault or similar for production secrets
- Enable HTTPS in production
- Use proper firewall rules
- Regular security updates

## Reporting Security Issues

If you discover a security vulnerability, please report it privately to the project maintainers. 