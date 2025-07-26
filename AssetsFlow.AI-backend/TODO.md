# ai-chat-backend TODO

- [ ] Initialize Node.js project in ai-chat-backend folder (npm init)
- [ ] Set up Express server with a /chat endpoint
- [ ] Implement per-user workflow state management (in-memory for now)
- [ ] Integrate Groq API call with prompt engineering for trade/element creation
- [ ] Implement response post-processing to enforce framework compliance
- [ ] Create API contract for frontend to send current trade and elements
- [ ] Add basic error handling and logging

## Advanced
- [ ] Integrate persistent database (e.g., MongoDB, PostgreSQL) for saving chat history per user
- [ ] Implement authentication (e.g., JWT, OAuth) for user identification and secure API access
- [ ] Link chat history to authenticated users in the database
- [ ] Add endpoints to retrieve past chat history for a user
- [ ] Add admin endpoint to review or moderate chat logs
