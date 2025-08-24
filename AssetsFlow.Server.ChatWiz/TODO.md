# ai-chat-backend TODO

- [ ] Integrate Groq API call with prompt engineering for trade/element creation
- [ ] Implement response post-processing to enforce framework compliance
- [ ] Create API contract for frontend to send current trade and elements
- [ ] Add basic error handling and logging

# testing

- Mocha, Jest, supertest

## Advanced

- [ ] Integrate persistent database (e.g., MongoDB, PostgreSQL) for saving chat history per user
- [ ] Implement authentication (e.g., JWT, OAuth) for user identification and secure API access
- [ ] Link chat history to authenticated users in the database
- [ ] Add endpoints to retrieve past chat history for a user
- [ ] Add admin endpoint to review or moderate chat logs

## Infrastructure

nginx:

limit_req_zone $binary_remote_addr zone=chat_api:10m rate=5r/s;

server {
location /chat-api/ {
limit_req zone=chat_api burst=10 nodelay;
proxy_pass http://localhost:3000;
}
}

Use other low-cost defenses:
✅ Validation: reject malformed/invalid queries early
✅ Circuit breaker or fail-fast logic on backend
