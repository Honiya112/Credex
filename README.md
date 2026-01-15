**🚀 Key Features**

**📊 Financial History & Data Ingestion**

- Automated CSV Import: At startup, the CsvImportService parses large-scale financial datasets (e.g., Financial_Statements.csv), mapping columns for Revenue, Earnings, Assets, and Debt directly into the database.

- Dynamic Sorting: Users can organize financial reports by year, highest revenue, or highest earnings through the FinancialHistoryController.

- Detailed Analytics: Specific yearly views provide a deep dive into balance sheet details, including total debt and liabilities.

**💳 Transaction Tracking & Dashboard**

- Full CRUD Functionality: Admins can create, read, update, and delete spending and earning records with automated date-based tracking.

- Interactive Visualizations: Integrated Chart.js renders stacked bar charts to compare spending vs. earnings over time.

- Optimized Performance: Implements asynchronous pagination via TransactionService to handle large volumes of transaction data without compromising speed.

**🛡️ Security & Identity**

- Role-Based Access Control: Secured via ASP.NET Core Identity, restricting the dashboard and transaction management to authorized Admin users.

- Automated Seeding: The UserService automatically seeds an Admin role and default credentials upon initial deployment.

- Secure Routing: Features custom cookie paths and anti-forgery token validation to prevent unauthorized access and CSRF attacks.
  
----------------------------------------------------------
**🛠️ Tech Stack**

Framework: ASP.NET Core MVC (.NET 9)

Database: SQLite via Entity Framework Core

Identity: ASP.NET Core Identity (Role-based)

Frontend: Razor Views, Bootstrap, Chart.js

Data Handling: Asynchronous CSV Parsing & Scoped Services

------------------------------------------------------------
**🗝️ Access the Application:**

Admin Credentials: admin@credex.com | Admin123!
