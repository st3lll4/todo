## A simple to-do list application, which fulfills the requirements for a small app while still demonstrating best code practises
- Technologies used: 
  - .NET backend
  - Next.js frontend
    - Flowbite React UI components with custom Tailwind (because the library is built on Tailwind and even though it can be creatively limiting on its own, it is highly customizable)
  - Postgres DB in docker

### Some specifications 
- API functionality
  - Filtering
    - it can be tested in swagger at http://localhost:5066/swagger, and it is optimized to my best ability :)
    - applied to GET /api/taskLists endpoint and is an optional parameter
    - fetches all lists that have a part which matches the filter, or any of their items matches the filter (multiple filters look for an item that matches all, not one)
  - Can add todos under todos infinitely
- Frontend functionality 
  - View todos
  - Create them
  - Update them
  - Delete them
- Database design
  - TaskList entity
  - ListItem entity
  - A TaskList can have many ListItems 
  - A ListItem can have many sub-ListItems
---
## Development Setup

### Prerequisites
- Docker and Docker Compose installed

### Quick Start
1. Clone the repo
2. In backend root folder (ToDo) run: `dotnet ef migrations add --project DAL --startup-project WebApp --context AppDbContext initial`
3. Run: `docker-compose up --build`
4. **Access the app at: http://localhost:3000**
5. Be more productive thanks to this small app :)

### Services
- Frontend: http://localhost:3000
- Backend: http://localhost:5066
- Database: localhost:7890

_author: Stella Tukia_
