  docker compose up -d

dotnet ef migrations add Initial
dotnet ef database update

-- cap tables
\connect Test
set schema 'cap';
select * from received
select * from published

-- project tables
\connect Test
set schema 'public';
select * from "Wallet"

-- nservicebus tables
select * from "Wallet_Sender_OutboxData"

SELECT *
FROM pg_catalog.pg_tables
WHERE schemaname != 'pg_catalog' AND
    schemaname != 'information_schema';