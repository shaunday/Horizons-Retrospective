
## Global Request Limit

- **Limit:** 10 total requests per second (global)
- Any endpoint not listed in the table below follows this limit.
- If the limit is exceeded:
  - The API returns a **`429 Too Many Requests`** exception.
  - Violator IP addresses are placed in a **penalty box for 15 minutes**.
  - After 15 minutes, the IP is removed until another violation occurs.
  - Repeat violators may be **permanently blocked** until the issue is resolved.

---

## Historical Market Data

https://www.interactivebrokers.com/campus/ibkr-api-page/cpapi-v1/#hist-md

- **Limit:** 5 concurrent requests
- **Max data points per request:** 1000
- Exceeding the limit returns **`429 Too Many Requests`**.

| Endpoint                  | Limit                     | Notes                                 |
|----------------------------|---------------------------|---------------------------------------|
| Global                     | 10 requests/sec           | Applies to all endpoints by default   |
| Historical Market Data     | 5 concurrent requests     | Max 1000 data points per request      |

> ⚠️ Tip: Always implement **throttling** in your client to respect these limits.
