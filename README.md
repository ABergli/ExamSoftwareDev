# ExamSoftwareDev

# Refrigerator + Cookbook (PG3302 Software Design Exam)

A simple **C# /.NET 9 console app** that lets you track fridge inventory and store recipes in one place.  It was built by five students for the December 2024 Software Design final at Kristiania University College.

---

## What it does

* **Ingredient manager** – add, update, list or delete items in a local **SQLite** DB.
* **Cookbook** – save recipes (title, steps, link) in a JSON file and search them by title or free‑text.
* **Fridge check** – warns when a recipe needs more of an ingredient than you have.
* **CLI menus** (see `Program.cs`) keep everything keyboard‑only.
* **Unit tests** – NUnit + in‑memory SQLite cover CRUD paths.

---

## Run it

```bash
# build + run (requires .NET 9 SDK)
dotnet restore
dotnet run --project ExamSoftwareDev
```

The first launch seeds `ingredients.db` with sample items and loads starter recipes from `recipes_bbc.json`.

---

## Folder map

```
├─ ExamSoftwareDev
│   ├── Logic/                    
│   │  ├─ Ingredient/                  
│   │  └─ Recipe/  
│   ├─ Storage/                   ← EF Core context + and repositories
│   │   └─ Data/                  ← SQLite + JSON
│   ├─ UI/                        ← console handlers / menus
│   └─ Program.cs                 ← entry point 
└─ Tests/                     ← unit tests (NUnit)
```

---

## Tech stack

* **.NET 9 / C# 13**
* **Entity Framework Core 9** + **SQLite** (code‑first migrations)
* **NUnit 4** (via `NUnit3TestAdapter`) for tests

Design patterns & principles: MVC layering, Repository + Service layers, dependency injection, SOLID mindset.

---

## Known quirks

* Units must match exactly (no automatic g→kg etc.).
* Ingredient update flow lacks some input validation.
* Recipe JSON is single‑threaded; simultaneous writers aren’t handled.

---

## Credits

Course: **PG3302 Software Design** · Lecturer: Thomas H. Johansen · Kristiania University College, Oslo.
README.md
3 KB
