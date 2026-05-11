# NitKotin Backend MVP Backlog

## 1. Мета документа

Цей документ розбиває [docs/backend-mvp-spec.md](docs/backend-mvp-spec.md) на практичний backlog для агентної розробки.

Ціль:

- мати зрозумілий порядок реалізації;
- давати Copilot/Codex агентам вузькі технічні slices;
- зменшити ризик того, що агент почне будувати занадто широкий шматок системи за раз;
- спростити ручне керування, рев’ю та тестування.

Принцип:

- один slice = один чіткий результат;
- після кожного slice має бути вузька перевірка;
- великі фічі не віддаються агентам одним запитом.

## 2. Product constraints для backlog

Backlog побудований на таких обмеженнях:

- local free user не має персонального server state;
- premium user має акаунт, entitlement і sync;
- content endpoints мають працювати і для local free користувачів;
- backend MVP не включає повний AI runtime;
- початковий backend краще будувати як modular monolith.

## 3. Як працювати з backlog через агентів

Для кожної задачі бажано передавати агенту:

1. короткий функціональний намір;
2. конкретний технічний scope;
3. список файлів або шарів, які можна змінювати;
4. acceptance criteria;
5. очікуваний validation step.

Рекомендований розмір одного агентного завдання:

- 1 endpoint;
- або 1 schema slice;
- або 1 auth flow;
- або 1 content/admin slice;
- але не цілий модуль повністю, якщо там є кілька різних ризиків.

## 4. Backlog overview

Рекомендований порядок епіків:

1. Architecture and bootstrap;
2. Persistence and schema;
3. Public content delivery;
4. Premium identity and entitlement;
5. Quit profile sync;
6. Event ingestion and timeline;
7. User-owned premium content;
8. Admin content operations;
9. Observability and operational hardening;
10. Privacy, export, deletion;
11. Client integration and acceptance validation.

## 5. Epic 1. Architecture and bootstrap

### Ціль

Підготувати skeleton backend проєкту без бізнес-логіки, але з правильною структурою, конфігурацією та test harness.

### Задачі

#### B001. Створити backend solution skeleton

Результат:

- окремий backend project або solution folder;
- базова конфігурація для environments;
- health endpoint;
- dependency injection skeleton;
- модульна структура папок.

Acceptance criteria:

- проєкт збирається локально;
- є `health` endpoint;
- є базовий configuration loading;
- немає зайвих доменних рішень, які ще не затверджені.

Validation:

- focused build;
- smoke request до `health` endpoint.

#### B002. Додати test harness

Результат:

- unit test project;
- integration test project;
- базовий test fixture для API і DB.

Acceptance criteria:

- тести запускаються навіть якщо поки містять лише простий smoke;
- є ізольований шлях для майбутніх integration tests.

Validation:

- test run для обох test projects.

## 6. Epic 2. Persistence and schema

### Ціль

Закласти PostgreSQL schema і migrations для основних premium-сутностей.

### Задачі

#### B003. Додати базову DB integration і migrations pipeline

Результат:

- DB connection configuration;
- migration runner;
- локальна стратегія для dev database.

Acceptance criteria:

- перша migration створюється й застосовується;
- integration tests можуть піднімати схему.

Validation:

- migration apply;
- integration smoke test.

#### B004. Схема для Identity і Subscription

Сутності:

- User;
- Subscription;
- DeviceRegistration;
- Refresh token metadata або еквівалент.

Acceptance criteria:

- таблиці створюються migration-ом;
- є constraints, індекси та FK;
- статус premium можна обчислити без двозначності.

Validation:

- schema integration tests.

#### B005. Схема для QuitProfile і SmokingBaseline

Acceptance criteria:

- можна зберегти поточний quit profile;
- є `updatedAt` для sync logic;
- baseline дані не змішані з event log.

Validation:

- repository integration tests.

#### B006. Схема для EventEntry і related event types

Acceptance criteria:

- event data підтримує `clientGeneratedId`;
- є індекси для `userId + occurredAt`;
- payload можна розширювати без schema rewrite кожного разу.

Validation:

- insert/read integration tests;
- duplicate protection tests.

#### B007. Схема для content entities

Сутності:

- ProductCatalogItem;
- MotivationPhrase;
- RecoveryMilestoneDefinition;
- AdviceSnippet.

Acceptance criteria:

- кожна сутність має language, publish state, sort order, updatedAt;
- схема підтримує versioned content delivery.

Validation:

- content repository tests.

## 7. Epic 3. Public content delivery

### Ціль

Дати local free і premium клієнтам серверний контент без акаунта.

### Задачі

#### B008. `GET /api/content/products`

Acceptance criteria:

- повертає published items тільки для обраної мови;
- має передбачуваний sort order;
- не вимагає auth.

Validation:

- API integration tests;
- caching headers test, якщо вже додані.

#### B009. `GET /api/content/phrases`

Acceptance criteria:

- повертає published motivational phrases;
- language filter працює;
- формат відповіді стабільний для mobile/desktop клієнтів.

Validation:

- API contract test.

#### B010. `GET /api/content/milestones`

Acceptance criteria:

- endpoint повертає впорядкований recovery content набір;
- можна додавати нові milestone items без API rewrite.

Validation:

- API integration test.

#### B011. `GET /api/content/advice`

Acceptance criteria:

- endpoint повертає базові advice snippets;
- підтримує language;
- може використовуватись для rule-based emergency help у future client work.

Validation:

- API integration test.

## 8. Epic 4. Premium identity and entitlement

### Ціль

Реалізувати paid account lifecycle і server-side entitlement model.

### Задачі

#### B012. `POST /api/auth/register-premium`

Acceptance criteria:

- створює premium user account;
- валідно обробляє duplicate account scenario;
- зберігає consent metadata.

Validation:

- auth integration tests.

#### B013. `POST /api/auth/login`, `refresh`, `logout`, `me`

Acceptance criteria:

- працює login lifecycle;
- refresh token flow безпечний і тестований;
- `me` endpoint повертає effective identity і entitlement context.

Validation:

- auth lifecycle tests.

#### B014. Subscription entitlement middleware / policy layer

Acceptance criteria:

- premium-only endpoints захищені;
- expired subscription блокує персональні premium operations;
- public content endpoints не ламаються.

Validation:

- permission tests;
- negative tests для expired premium.

#### B015. `GET/POST /api/subscription/*`

Acceptance criteria:

- backend може визначити active entitlement;
- purchase activation оновлює статус;
- restore flow не дублює entitlement records.

Validation:

- integration tests для activation/restore.

## 9. Epic 5. Quit profile sync

### Ціль

Дати premium користувачу надійний cross-device sync основного профілю.

### Задачі

#### B016. `GET /api/profile/quit`

Acceptance criteria:

- повертає поточний server-backed quit profile premium user;
- правильно працює при пустому профілі.

Validation:

- API integration tests.

#### B017. `PUT /api/profile/quit`

Acceptance criteria:

- оновлює quit profile;
- зберігає `updatedAt`;
- підтримує predicable merge rules.

Validation:

- integration tests;
- conflict-oriented tests, якщо merge logic уже додана.

#### B018. Quit attempts endpoints

Acceptance criteria:

- premium user може бачити й створювати quit attempts;
- relapse/restart моделі не ламають current profile.

Validation:

- integration tests для create/list attempts.

## 10. Epic 6. Event ingestion and timeline

### Ціль

Додати збереження подій, cravings, relapse і NRT usage з ідемпотентною синхронізацією.

### Задачі

#### B019. `POST /api/events/batch`

Acceptance criteria:

- приймає кілька подій за раз;
- дедуплікує по `clientGeneratedId`;
- частково валідні пакети обробляються передбачувано.

Validation:

- idempotency tests;
- malformed batch tests.

#### B020. `GET /api/events`

Acceptance criteria:

- повертає timeline користувача в стабільному порядку;
- підтримує pagination або cursor strategy.

Validation:

- integration tests з sorting/paging.

#### B021. Typed endpoints для craving, relapse, NRT usage, emergency session

Acceptance criteria:

- кожен endpoint мапиться в уніфікований EventEntry model;
- є domain validation на обов’язкові поля;
- немає розсинхрону між typed endpoints і batch ingestion rules.

Validation:

- endpoint-specific integration tests.

## 11. Epic 7. User-owned premium content

### Ціль

Дати premium користувачу власні motivational phrases із sync.

### Задачі

#### B022. CRUD для `user motivation phrases`

Endpoints:

- `GET /api/user/motivation-phrases`
- `POST /api/user/motivation-phrases`
- `PUT /api/user/motivation-phrases/{id}`
- `DELETE /api/user/motivation-phrases/{id}`

Acceptance criteria:

- працює лише для premium users;
- записи прив’язані до userId;
- є basic validation на довжину та пусті значення.

Validation:

- CRUD integration tests;
- permission tests.

## 12. Epic 8. Admin content operations

### Ціль

Дати редакційний контроль над контентом без повноцінного public admin UI.

### Задачі

#### B023. Admin CRUD для ProductCatalogItem

Acceptance criteria:

- admin може створювати, публікувати, приховувати записи;
- publish state впливає на public endpoints.

Validation:

- admin API tests.

#### B024. Admin CRUD для MotivationPhrase, RecoveryMilestoneDefinition, AdviceSnippet

Acceptance criteria:

- редакційний workflow однаковий між content types;
- sort order і language реально впливають на public responses.

Validation:

- content publish/unpublish tests.

## 13. Epic 9. Observability and operational hardening

### Ціль

Побачити, що відбувається в системі, ще до production rollout.

### Задачі

#### B025. Structured logs and request tracing

Acceptance criteria:

- запити можна трасувати по correlation id;
- помилки в auth/sync/content видно в логах.

Validation:

- local smoke з логами;
- integration test, якщо є підтримка trace context.

#### B026. Error tracking and health probes

Acceptance criteria:

- є health/readiness endpoints;
- критичні помилки потрапляють у monitoring sink.

Validation:

- health endpoint smoke;
- simulated failure test там, де можливо.

#### B027. Operational dashboard minimum

Acceptance criteria:

- можна побачити auth failures, sync failures, content publish issues;
- є базова інструкція для ручного triage.

Validation:

- manual verification notes.

## 14. Epic 10. Privacy, export, deletion

### Ціль

Закрити мінімальні legal/privacy вимоги для paid accounts.

### Задачі

#### B028. User data export flow

Acceptance criteria:

- premium user може експортувати основні дані;
- формат експорту задокументований;
- експорт не губить critical profile/event data.

Validation:

- export integration test.

#### B029. Account deletion flow

Acceptance criteria:

- користувач може видалити акаунт;
- premium-only personal data видаляється або анонімізується за політикою;
- deletion не ламає audit requirements.

Validation:

- deletion flow integration tests.

#### B030. Consent and policy tracking

Acceptance criteria:

- backend зберігає факт прийняття політик;
- це видно в user/account metadata.

Validation:

- auth/account tests.

## 15. Epic 11. Client integration and acceptance validation

### Ціль

Підтвердити, що backend реально готовий до підключення Windows і Android клієнтів.

### Задачі

#### B031. Android integration slice

Acceptance criteria:

- Android client може тягнути public content;
- premium login і quit profile sync працюють мінімально end-to-end;
- є manual notes по happy path і основних failure cases.

Validation:

- ручний smoke на Android;
- focused client integration checks.

#### B032. Windows integration slice

Acceptance criteria:

- Windows client може тягнути public content;
- premium login і quit profile sync працюють мінімально end-to-end;
- є manual notes по sync behavior.

Validation:

- ручний smoke на Windows.

## 16. Dependency graph

Критичні залежності:

- B001 -> майже все інше;
- B003 -> B004-B007;
- B004 -> B012-B015;
- B005 -> B016-B018;
- B006 -> B019-B021;
- B007 -> B008-B011, B023-B024;
- B014-B015 -> всі premium-only endpoints;
- B025-B026 бажано до початку клієнтської інтеграції.

## 17. Recommended delivery waves

### Wave 1

- B001-B004;
- B008-B011.

Результат:

- skeleton backend;
- DB basics;
- public content delivery для local free клієнтів.

### Wave 2

- B012-B018.

Результат:

- premium identity;
- entitlement;
- quit profile sync.

### Wave 3

- B019-B024.

Результат:

- event timeline;
- premium user phrases;
- admin content control.

### Wave 4

- B025-B032.

Результат:

- observability;
- privacy flows;
- client integration readiness.

## 18. What to ask agents to produce

Для кожного slice варто просити агента повертати:

- короткий список змінених файлів;
- що саме реалізовано;
- який validation запущений;
- які відкриті ризики лишилися;
- які наступні 1-2 slices логічні після цього.

## 19. Definition of Ready для агентного slice

Задача готова до передачі агенту, якщо:

- зрозуміло, який endpoint / schema / flow змінюється;
- визначені межі змін;
- визначений expected validation step;
- немає неоднозначності в monetization model;
- є зрозуміло, чи це public endpoint, чи premium-only logic.

## 20. Definition of Done для backlog item

Backlog item вважається завершеним, якщо:

- реалізація зібрана і проходить відповідну вузьку перевірку;
- є автоматичний тест або обґрунтована причина, чому його нема;
- немає зламаного auth/public boundary;
- документація або API notes оновлені, якщо контракт змінився;
- є короткий manual verification note там, де це важливо.