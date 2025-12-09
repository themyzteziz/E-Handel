📦 E-Handel – Konsolapplikation med Entity Framework Core
Ett enkelt e-handelssystem. Systemet använder Entity Framework Core, SQLite, migrationer, seed-data och en grundläggande kryptering för att hantera kunder, produkter, kategorier, ordrar och orderrader.
Projektet demonstrerar databasmodellering i 3NF, relationshantering och hur CRUD-flöden kan implementeras i ett konsolbaserat system.

🗂️ Datamodell & ER-Diagram
Systemet består av fem centrala entiteter: Customer, Category, Product, Order, OrderRow.

🔗 Relationer
En Category har många Products.
En Product kan förekomma i flera OrderRows.
En Customer har många Orders.
En Order består av flera OrderRows.

🧩 Normalisering (3NF)
Datamodellen följer 3NF genom: Inga redundanta fält. Alla fält beror direkt på primärnyckeln. TotalAmount beräknas via OrderRows istället för att dupliceras

🛠️ Entity Framework Core & Migrationer
Projektet använder EF Core för att: skapa databasen via migrationer, lägga in seed-data automatiskt, hantera relationer, navigation properties och foreign keys.

Vanliga hinder som löstes: Table already exists. No such table. Foreign key constraint failed. De flesta problemen berodde på gamla versioner av SQLite-filen. Genom att ta bort databasen och köra om migrationerna synkades modellen korrekt.

🔧 Funktionalitet (CRUD)
E-handeln har ett komplett CRUD-flöde:
👤 Customers: listcustomer, addcustomer, editcustomer, deletecustomer
🏷️ Categories: listcategory, addcategory, editcategory, deletecategory
📦 Products: listproduct, addproduct, editproduct, deleteproduct, productpages (paginering)
🧾 Orders: listorders, orderdetails, addorder

🔒 Kryptering av känslig data
Uppgiften krävde en enkel form av kryptering.
Jag implementerade lärarens XOR-baserade metod: E-postadresser krypteras före lagring. De dekrypteras vid utskrift.

✅ Slutsats
Det här projektet gav praktisk erfarenhet av: Databasdesign och relationsmodellering, Normalisering (3NF), EF Core, migrationer och seed-data, CRUD-flöden, Enkel kryptering av känslig information.
De största utmaningarna var relaterade till migrationer och foreign keys, men det gav också en tydligare förståelse för hur EF Core fungerar.

<img width="1920" height="1080" alt="E-Shopping" src="https://github.com/user-attachments/assets/d5e1eec5-b65a-4367-9a0c-ca4b48237895" />
