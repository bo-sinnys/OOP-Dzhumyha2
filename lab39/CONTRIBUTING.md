# Contributing to BankingSystem

## Branching Strategy (GitHub Flow)

`main` завжди стабільний і готовий до деплою. Будь-яка зміна йде через feature branch → PR → review → merge.

### Іменування гілок

| Тип | Шаблон | Приклад |
|-----|--------|---------|
| Нова функція | `feature/short-description` | `feature/add-csv-export` |
| Виправлення | `fix/short-description` | `fix/overdraft-fee-calculation` |
| Рефакторинг | `refactor/short-description` | `refactor/extract-validator` |
| Документація | `docs/short-description` | `docs/update-readme` |

```bash
git checkout main
git pull origin main
git checkout -b feature/your-feature
```

---

## Commit Conventions

Формат: `<type>: <short description>`

| Тип | Коли використовувати |
|-----|----------------------|
| `feat:` | нова функціональність |
| `fix:` | виправлення помилки |
| `refactor:` | зміна коду без зміни поведінки |
| `docs:` | оновлення документації |
| `test:` | додавання або виправлення тестів |
| `chore:` | оновлення залежностей, конфігурації |

**Приклади:**
```
feat: add CSV export for transaction history
fix: correct overdraft fee applied twice on boundary amount
refactor: extract IValidator interface from AccountService
docs: add Docker setup instructions to README
test: add unit tests for SavingsAccount minimum balance rule
```

---

## Як створити Pull Request

1. Переконайтесь що гілка актуальна відносно `main`:
   ```bash
   git fetch origin
   git rebase origin/main
   ```
2. Запушити гілку:
   ```bash
   git push -u origin feature/your-feature
   ```
3. Відкрити PR на GitHub: **Compare & pull request**
4. Заповнити шаблон: Summary, Related Issue (`Closes #N`), Changes, Checklist
5. Додати мітки та призначити рецензента
6. Дочекатися щонайменше 1 approval перед merge

---

## Як проводити Code Review

Фокус перевірки:
- логічні помилки та edge cases
- читабельність і іменування
- дотримання конвенцій проєкту
- покриття тестами

Типи коментарів:
- **suggestion** — конкретна пропозиція з кодом
- **question** — запитання щодо рішення
- **nitpick** — дрібне зауваження (не блокує merge)

Використовуйте GitHub Suggestions для конкретних змін:
````
```suggestion
public ValidationResult Validate(Order order) { ... }
```
````

Фінальне рішення: **Approve**, **Request Changes** або **Comment**.

---

## Як вирішувати конфлікти

```bash
# 1. Оновити main
git fetch origin
git checkout main
git pull origin main

# 2. Перейти на вашу гілку і змержити main
git checkout feature/your-feature
git merge main

# 3. Відкрити конфліктні файли, вирішити вручну
# Шукати маркери: <<<<<<<, =======, >>>>>>>

# 4. Після вирішення
git add .
git commit -m "fix: resolve merge conflict in AccountService"
git push
```

Правило: **не видаляйте чужі зміни** без узгодження з автором.
