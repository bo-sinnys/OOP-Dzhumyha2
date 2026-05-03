# JSON vs XML: практичне порівняння
---

## Вступ

Вибір формату серіалізації даних — одне з перших архітектурних рішень при проєктуванні будь-якої системи. JSON та XML є двома найпоширенішими форматами, кожен із яких має свої переваги та сфери застосування. У цьому есе я порівняю обидва формати на конкретному прикладі та розгляну реальні сценарії їх використання.

---

## 1. Клас для серіалізації

Для порівняння створимо клас `Employee` з 5 властивостями, що описує співробітника компанії:

```csharp
using System.Text.Json.Serialization;
using System.Xml.Serialization;

[XmlRoot("Employee")]
public class Employee
{
    [JsonPropertyName("id")]
    [XmlElement("ID")]
    public int Id { get; set; }

    [JsonPropertyName("full_name")]
    [XmlElement("FullName")]
    public string FullName { get; set; } = string.Empty;

    [JsonPropertyName("department")]
    [XmlElement("Department")]
    public string Department { get; set; } = string.Empty;

    [JsonPropertyName("salary")]
    [XmlElement("Salary")]
    public decimal Salary { get; set; }

    [JsonPropertyName("hired_at")]
    [XmlElement("HiredAt")]
    public DateTime HiredAt { get; set; }

    // Поле ігнорується в обох форматах
    [JsonIgnore]
    [XmlIgnore]
    public string InternalCode { get; set; } = string.Empty;
}
```

Створимо об'єкт для серіалізації:

```csharp
var employee = new Employee
{
    Id         = 42,
    FullName   = "Марія Коваленко",
    Department = "Розробка ПЗ",
    Salary     = 85000.00m,
    HiredAt    = new DateTime(2021, 3, 15),
    InternalCode = "INT-007" // це поле не потрапить у файл
};
```

---

## 2. Серіалізація у JSON

```csharp
using System.Text.Json;

var options = new JsonSerializerOptions { WriteIndented = true };
string json = JsonSerializer.Serialize(employee, options);
await File.WriteAllTextAsync("employee.json", json);
```

**Результат (файл `employee.json`):**

```json
{
  "id": 42,
  "full_name": "Марія Коваленко",
  "department": "Розробка ПЗ",
  "salary": 85000.00,
  "hired_at": "2021-03-15T00:00:00"
}
```

**Розмір файлу:** ~120 байт

---

## 3. Серіалізація у XML

```csharp
using System.Xml.Serialization;

var serializer = new XmlSerializer(typeof(Employee));
using (StreamWriter writer = new StreamWriter("employee.xml"))
{
    serializer.Serialize(writer, employee);
}
```

**Результат (файл `employee.xml`):**

```xml
<?xml version="1.0" encoding="utf-8"?>
<Employee xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
          xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <ID>42</ID>
  <FullName>Марія Коваленко</FullName>
  <Department>Розробка ПЗ</Department>
  <Salary>85000.00</Salary>
  <HiredAt>2021-03-15T00:00:00</HiredAt>
</Employee>
```

**Розмір файлу:** ~330 байт

---

## 4. Порівняння результатів

| Критерій | JSON | XML |
|---|---|---|
| Розмір файлу | ~120 байт | ~330 байт (+175%) |
| Кількість рядків | 8 | 12 |
| Зайвий синтаксис | Дужки `{}`, коми | Теги, `<?xml ...?>`, `xmlns` |
| Читабельність | Висока | Середня |
| Швидкість парсингу | Швидше | Повільніше |
| Підтримка коментарів | ❌ | ✅ |
| Валідація схеми | JSON Schema (опційно) | XSD (вбудовано) |

Різниця у розмірі на одному об'єкті виглядає незначною, але при серіалізації масиву з 10 000 записів XML-файл буде більшим на **сотні кілобайт** — це суттєво при передачі по мережі.

---

## 5. Реальні сценарії використання

### Сценарій 1 — REST API мобільного додатку (JSON ✅)

Мобільний застосунок для замовлення їжі обмінюється даними з сервером. Кожен запит/відповідь передається по мережі — кожен зайвий байт уповільнює роботу. JSON тут очевидний вибір: менший розмір, нативна підтримка у JavaScript/TypeScript на фронтенді, швидкий парсинг у браузері та мобільних SDK.

```json
{ "order_id": 101, "status": "delivered", "total": 320.50 }
```

### Сценарій 2 — Інтеграція з банківською системою (XML ✅)

Банки та фінансові установи часто використовують протокол **SOAP** для міжсистемної взаємодії. SOAP побудований на XML і вимагає строгої валідації через **XSD-схеми** — це гарантує, що жодне обов'язкове поле не буде пропущено. Тут XML незамінний: він підтримує простори імен (`xmlns`), що дозволяє об'єднувати схеми різних організацій без конфліктів імен.

```xml
<soap:Envelope xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/">
  <soap:Body>
    <PaymentRequest>
      <Amount currency="UAH">5000.00</Amount>
      <RecipientIBAN>UA213223130000026007233566001</RecipientIBAN>
    </PaymentRequest>
  </soap:Body>
</soap:Envelope>
```

### Сценарій 3 — Конфігураційні файли (обидва, залежно від платформи)

**.NET** традиційно використовував XML для конфігурації (`Web.config`, `App.config`), але сучасний .NET перейшов на JSON (`appsettings.json`). Однак XML залишається у **Maven** (Java), **MSBuild** (`.csproj`) та **Android** (`AndroidManifest.xml`) — там, де важлива підтримка коментарів для пояснення налаштувань та строга валідація схеми.

```json
// appsettings.json — сучасний .NET
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=mydb"
  },
  "Logging": { "LogLevel": { "Default": "Information" } }
}
```

---

## 6. Чому JSON домінує у веб, але XML залишається в enterprise

**JSON переміг у веб з трьох причин:**

По-перше, JSON є рідним форматом JavaScript. Оскільки весь фронтенд веб-додатків написаний на JS/TS, парсинг JSON відбувається без додаткових бібліотек — `JSON.parse()` є вбудованою функцією браузера. XML вимагає окремого XML-парсера.

По-друге, розвиток REST API витіснив SOAP. REST не накладає вимог до формату — і розробники обрали лаконічніший JSON. Порівняно з SOAP-конвертом, REST-відповідь у JSON у 3–5 разів менша за розміром.

По-третє, поширення мобільних додатків. На мобільних пристроях трафік і швидкість відповіді критичні — менший розмір JSON дає реальну перевагу.

**XML залишається важливим в enterprise з інших причин:**

Великі корпоративні системи (ERP, CRM, банківські платформи) будувалися у 2000-х роках на SOAP і XML. Міграція таких систем надзвичайно дорога та ризикована, тому XML продовжує використовуватися десятиліттями. Крім того, **XSD-схеми** дають формальну гарантію структури документа — це критично у фінансовій сфері та охороні здоров'я, де некоректні дані можуть мати юридичні наслідки. XML також підтримує **XSLT** — мову трансформації документів, яка дозволяє перетворювати XML в HTML, PDF або інший XML без написання коду.

---

## Висновок

JSON та XML — не конкуренти, а інструменти для різних задач. JSON виграє у швидкості, лаконічності та зручності для веб-розробки. XML незамінний там, де потрібна строга валідація, підтримка просторів імен та інтеграція з legacy-системами.

Практичне порівняння на прикладі класу `Employee` наочно показало: XML-файл займає у 2,75 рази більше місця, ніж JSON при ідентичному вмісті. Для одного об'єкта це несуттєво, але у масштабах мільйонів запитів на день — це реальна різниця у витратах на трафік та швидкості відповіді.

Сучасний розробник повинен вміти працювати з обома форматами та обирати відповідний залежно від контексту: характеру системи, вимог до валідації та середовища виконання.
