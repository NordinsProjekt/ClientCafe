# Cozy Café - Klientapplikation

En fullstack café-hanteringsapplikation byggd med C# .NET 10 och vanilla JavaScript. Detta projekt innehåller både en RESTful API backend och en frontend webbapplikation för att hantera produkter, beställningar och kundvagnsfunktionalitet.

## Innehållsförteckning

- [Projektstruktur](#projektstruktur)
- [Teknologier](#teknologier)
- [Komma igång](#komma-igång)
- [Arkitektur](#arkitektur)
- [Kända problem](#kända-problem)
- [Testning](#testning)
- [Bidra](#bidra)

## Projektstruktur

```
ClientCafe/
??? ClientApi/              # Huvudsakligt API-projekt
??? Entities/               # Domänmodeller (Order, Product, OrderItem)
??? EntityFramework/        # Databaskontext och konfigurationer
??? Services/               # Affärslogiklager
??? Services.Tests/         # Enhetstester för services
??? Requests/              # Request DTOs
??? CafePublicHtml/        # Frontend HTML/CSS/JS-filer (om tillgänglig)
```

### Projekt

| Projekt | Beskrivning | Ramverk |
|---------|-------------|---------|
| **ClientApi** | ASP.NET Core Web API | .NET 10 |
| **Entities** | Domänentiteter och modeller | .NET 10 |
| **EntityFramework** | EF Core DbContext och migrationer | .NET 10 |
| **Services** | Affärslogiktjänster | .NET 10 |
| **Services.Tests** | Enhetstester med xUnit | .NET 10 |
| **Requests** | Request/Response DTOs | .NET 10 |

## Teknologier

### Backend
- **.NET 10** - Senaste .NET-ramverket
- **C# 14.0** - Senaste C#-språkfunktioner
- **Entity Framework Core** - ORM för databasåtkomst
- **ASP.NET Core Web API** - RESTful API-ramverk
- **xUnit** - Enhetstestramverk

### Frontend
- **HTML5**
- **CSS3** (Grid, Flexbox, Media Queries)
- **Vanilla JavaScript** - Inga ramverk krävs
- **Live Server** - Utvecklingsserver

## Komma igång

### Förutsättningar

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) eller [VS Code](https://code.visualstudio.com/)
- [Live Server Extension](https://marketplace.visualstudio.com/items?itemName=ritwickdey.LiveServer) för VS Code (för frontend)

### Backend-installation

1. **Klona repositoryt**
   ```bash
   git clone https://github.com/NordinsProjekt/ClientCafe.git
   cd ClientCafe
   ```

2. **Återställ beroenden**
   ```bash
   dotnet restore
   ```

3. **Kör API:et**
   ```bash
   cd ClientApi
   dotnet run
   ```
   API:et kommer vara tillgängligt på `https://localhost:5001`

4. **Verifiera att API:et körs**
   Öppna webbläsaren och navigera till: `https://localhost:5001/products`

**OBS:** Projektet använder InMemoryDatabase, så inga databasmigrationer behövs.

### Frontend-installation

1. **Installera Live Server Extension i VS Code**
   - Öppna VS Code
   - Gå till Extensions (Ctrl+Shift+X)
   - Sök efter "Live Server" av Ritwick Dey
   - Klicka på "Install"

2. **Öppna frontend**
   - Öppna mappen `CafePublicHtml` i VS Code
   - Högerklicka på `index.html`
   - Välj "Open with Live Server"
   - Webbplatsen öppnas på `http://127.0.0.1:5500`

**OBS:** Live Server krävs eftersom att öppna HTML-filer direkt (file:///) gör att webbläsare blockerar API-anrop av säkerhetsskäl.

## Arkitektur

### Backend-arkitektur

```
???????????????
?  ClientApi  ?  ? Controllers & Program.cs
???????????????
       ?
???????????????
?  Services   ?  ? Affärslogik (OrderService, ProductService)
???????????????
       ?
???????????????????
? EntityFramework ?  ? AppDbContext, Repositories
???????????????????
       ?
???????????????
?  Entities   ?  ? Domänmodeller (Order, Product, OrderItem)
???????????????
```

### Nyckelkomponenter

#### Entities
- `Product` - Produktinformation
- `Order` - Kundbeställningar
- `OrderItem` - Radartiklar i beställningar

#### Request DTOs
- `CreateOrderRequest` - Begäran för att skapa en ny beställning
- `CreateOrderItemRequest` - Produkt och kvantitet för beställningsartiklar

#### Services
- `ProductService` - Produkthanteringslogik
- `OrderService` - Orderbehandlingslogik

## Kända problem

Detta projekt innehåller avsiktliga problem för utbildningsändamål. Nedan är de problem som ska lösas:

### Kritiska Backend-buggar

#### Bug 1: Product Service Problem - KRITISK
- **Symptom**: GET `/products` endpoint returnerar felaktiga data eller kastar undantag
- **Plats**: ProductService eller Product Repository
- **Åtgärd krävs**: Granska databasanrop, null-hantering och LINQ-queries

#### Bug 2: Order Service Problem - KRITISK
- **Symptom**: POST `/orders` endpoint misslyckas eller skapar felaktiga beställningar
- **Plats**: OrderService eller Order Repository
- **Åtgärd krävs**: Granska validering, databasinsättningar och relationshantering

### Frontend-problem (HTML & CSS)

**VIKTIGT:** JavaScript-filerna (`products.js`, `product-detail.js`, `cart.js`, `api.js`) fungerar korrekt. Ändra INTE dem!

#### Produktsida (products.html)

1. **Dålig layout - HÖG PRIORITET
   - Problem: Produkter visas i en enda kolumn istället för rutnät
   - Lösning: Implementera CSS Grid/Flexbox med 3+ kolumner

2. **Saknad prisformatering - HÖG PRIORITET
   - Problem: Priser visas utan "kr" valuta
   - Lösning: Lägg till CSS `::after` pseudo-element

3. **Saknade produktbilder - MEDEL PRIORITET
   - Problem: Inga produktbilder visas
   - Lösning: Skapa CSS platshållare med `::before` pseudo-element

4. **Dålig knappstyling - MEDEL PRIORITET
   - Problem: "View Details" och "Add to Cart" knappar saknar styling
   - Lösning: Tillämpa `.btn` och `.btn-primary` stilar

5. **Ingen responsiv design - LÅG PRIORITET
   - Problem: Dålig mobilupplevelse
   - Lösning: Lägg till media queries för mobil (max-width: 768px)

#### Produktdetaljsida (product-detail.html)

6. **Ingen tvåkolumnslayout - HÖG PRIORITET
   - Problem: Innehåll staplas vertikalt
   - Lösning: Implementera 2-kolumners layout (bild till vänster, info till höger)

7. **Ful bildplatshållare - MEDEL PRIORITET
   - Problem: Oprofessionellt utseende på platshållare
   - Lösning: Förbättra med ordentlig storlek (400px+), färger, border-radius

8. **Saknad prisformatering - HÖG PRIORITET
   - Problem: Pris saknar "kr" och framträdande
   - Lösning: Lägg till `::after` med "kr", öka storlek (1.5rem+)

9. **Dålig knapp-/länkstyling - MEDEL PRIORITET
   - Problem: "Add to Cart" knapp och "Back" länk är ostilad
   - Lösning: Tillämpa `.btn .btn-primary` styling

10. **Dålig kvantitetsväljare - LÅG PRIORITET
    - Problem: Input-fält för litet och oformaterat
    - Lösning: Lägg till padding, border-radius, bättre styling

11. **Ingen responsiv design - LÅG PRIORITET
    - Problem: Dålig mobillayout
    - Lösning: Lägg till media query för enkolumn på mobil

## Testning

### Köra enhetstester

```bash
cd Services.Tests
dotnet test
```

### Manuell testchecklista

- [ ] Produktlistan visas i rutnätslayout
- [ ] Priser visas med "kr" suffix
- [ ] Produktbilder/platshållare synliga
- [ ] "View Details" navigerar korrekt
- [ ] Produktdetaljer visas i 2-kolumners layout
- [ ] "Add to Cart" fungerar och uppdaterar kundvagnsräknare
- [ ] Kundvagnssidan visar artiklar korrekt
- [ ] Orderskapande lyckas
- [ ] Responsiv design fungerar på mobil

### Testverktyg

1. **API-testning**: Använd `test-api.html` för att debugga API-problem
2. **Browser DevTools**: Tryck F12 för att inspektera element och testa CSS
3. **Mobilvy**: Växla Device Toolbar i DevTools för responsiv testning

## Hjälpresurser

### CSS-koncept som behövs

- **CSS Grid**: `display: grid`, `grid-template-columns`, `gap`
- **Flexbox**: `display: flex`, `justify-content`, `align-items`
- **Pseudo-element**: `::before`, `::after`, `content`
- **Media Queries**: `@media (max-width: 768px) { }`
- **Responsiv design**: Viewport-enheter, procentbredder, mobile-first

### Utvecklingstips

- Använd Developer Tools (F12) för att inspektera och testa CSS live
- Läs kommentarer i HTML/CSS-filer noggrant
- Jämför med fungerande sidor (`index.html`, `cart.html`) för referens
- Använd `test-api.html` för att debugga API-problem
- Börja med högprioriterade problem först
- Jobba i par och hjälp varandra!

## Bidra

Detta är ett utbildningsprojekt. När du fixar problem:

1. Skapa en feature-branch
2. Fixa ett problem i taget
3. Testa grundligt
4. Commit med tydliga meddelanden
5. Pusha till din branch

## Noteringar

- **CORS är redan konfigurerat** - API:et tillåter webbläsarförfrågningar
- **JavaScript är korrekt** - Ändra inte JS-filer
- Alla fixar ska vara i **HTML, CSS och C#-filer endast**

## Licens

Detta är ett utbildningsprojekt för skoländamål.

## Sammanfattning

**Totalt antal problem att fixa:**
- Kritisk: 2 backend-buggar
- Hög prioritet: 5 frontend-problem
- Medel prioritet: 5 frontend-problem  
- Låg prioritet: 4 frontend-problem

**Totalt: 13 problem + 2 kritiska buggar**

---

Lycka till!
