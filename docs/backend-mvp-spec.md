# NitKotin Backend MVP Spec

## 1. Мета

Побудувати першу серверну версію NitKotin, яка:

- підтримує платні акаунти;
- синхронізує прогрес між пристроями для premium users;
- зберігає основні дані journey і журнал подій;
- віддає серверний контент для каталогу товарів, motivational phrases і recovery milestones;
- створює основу для майбутнього AI-ментора без передчасного ускладнення архітектури.

Це має бути backend-first етап, а не повна enterprise-платформа з першого дня.

## 2. Product model

Модель доступу в MVP:

- local free user: без акаунта, без персонального серверного сховища;
- premium user: платний користувач з акаунтом, серверним профілем і синхронізацією між пристроями.

Наслідки для backend:

- backend не повинен вимагатися для базового локального сценарію;
- персональні API та state існують лише для premium users;
- загальний контент може віддаватися анонімно або через легкий public-content механізм;
- server cost має концентруватися на тих користувачах, які реально платять.

## 3. MVP goals

Backend MVP має покрити 5 ключових задач:

1. premium account creation and sign-in;
2. cloud sync основного quit profile;
3. sync журналу подій для premium users;
4. серверний delivery контенту;
5. базову subscription-aware авторизацію клієнтів.

## 4. Non-goals for MVP

Що свідомо не входить у першу серверну ітерацію:

- повноцінний AI chat runtime;
- складна recommendation engine;
- мікросервісна архітектура;
- складний billing orchestration layer всередині backend;
- social features;
- family accounts;
- web admin portal з повним UI, якщо достатньо admin API + simple internal tooling.

## 5. Recommended architecture

### 5.1 High-level approach

Рекомендовано почати з modular monolith:

- один deployable backend service;
- чіткий поділ на модулі всередині коду;
- одна основна реляційна БД;
- background job runner;
- object storage для картинок або asset payloads.

Це дешевше і простіше для agent-driven delivery, ніж старт із багатьох сервісів.

### 5.2 Suggested modules

- Identity module;
- Subscription access module;
- Quit Journey module;
- Event Log module;
- Content module;
- Sync module;
- Admin Content module;
- Analytics hooks module;
- AI readiness module.

### 5.3 Suggested stack direction

Точний стек можна затвердити окремо, але для MVP бажані такі властивості:

- швидка розробка REST API;
- хороша інтеграція з PostgreSQL;
- проста міграційна модель;
- нормальна підтримка background jobs;
- хороша тестованість;
- зручність для Copilot/Codex агентів.

Практично підійдуть:

- ASP.NET Core + PostgreSQL;
- або Node.js/NestJS + PostgreSQL.

З огляду на поточний WinForms/.NET бекграунд проєкту, старт на ASP.NET Core виглядає прагматично, але це не жорстка вимога.

## 6. Core domains and data

### 6.1 Identity and subscription

Основні сутності:

- User;
- UserCredential або external auth binding;
- Subscription;
- DeviceRegistration;
- SessionToken / RefreshToken metadata.

Мінімальні поля User:

- id;
- email або інший login identifier;
- createdAt;
- status;
- locale;
- marketingConsent;
- privacyPolicyAcceptedAt.

Мінімальні поля Subscription:

- id;
- userId;
- tier;
- status;
- startedAt;
- expiresAt;
- platformSource;
- externalPurchaseReference.

### 6.2 Quit journey

Основні сутності:

- QuitProfile;
- SmokingBaseline;
- QuitAttempt.

QuitProfile:

- userId;
- currentQuitStartedAt;
- packsPerDay або cigarettesPerDay;
- packPrice;
- currency;
- hasActiveAttempt;
- updatedAt.

SmokingBaseline:

- userId;
- cigarettesPerDay;
- packsPerDay;
- preferredInputMode;
- averagePackPrice;
- optionalBrandName.

QuitAttempt:

- id;
- userId;
- startedAt;
- endedAt;
- endReason;
- notes;
- createdAt.

### 6.3 Event log

Основні сутності:

- EventEntry;
- CravingEvent;
- RelapseEvent;
- NrtUsageEvent;
- EmergencySession.

EventEntry має бути узагальненим контейнером для timeline користувача.

Мінімальні поля:

- id;
- userId;
- type;
- occurredAt;
- intensity;
- payloadJson;
- clientGeneratedId;
- createdAt;
- updatedAt.

### 6.4 NRT data

Сутності:

- NrtProfile;
- NrtProductReference;
- NrtUsageEvent.

MVP може підтримати просту модель:

- тип НЗТ;
- дата початку;
- active/inactive status;
- log використання по подіях.

### 6.5 Content

Сутності:

- ProductCatalogItem;
- MotivationPhrase;
- RecoveryMilestoneDefinition;
- AdviceSnippet.

Вимоги до content data:

- language;
- published flag;
- sort order;
- effective version;
- updatedAt;
- optional tags.

### 6.6 AI readiness

Навіть якщо AI chat ще не входить в MVP, бажано закласти:

- AiUserProfileSummary;
- AiConversationSummary;
- AiSafetyFlag.

У MVP ці таблиці можуть бути порожніми або мінімально використовуватись, але схема має передбачати майбутній розвиток.

## 7. API scope for MVP

### 7.1 Public or lightweight anonymous content endpoints

- `GET /api/content/products?lang=uk`
- `GET /api/content/phrases?lang=uk`
- `GET /api/content/milestones?lang=uk`
- `GET /api/content/advice?lang=uk`

Ці endpoints можна віддавати без персонального акаунта, з aggressive caching.

### 7.2 Premium auth endpoints

- `POST /api/auth/register-premium`
- `POST /api/auth/login`
- `POST /api/auth/refresh`
- `POST /api/auth/logout`
- `GET /api/auth/me`

### 7.3 Subscription-aware endpoints

- `GET /api/subscription/status`
- `POST /api/subscription/activate`
- `POST /api/subscription/restore`

Примітка:

billing verification може частково виконуватись через app store ecosystems, але backend повинен мати власне уявлення про effective entitlement.

### 7.4 Quit journey endpoints

- `GET /api/profile/quit`
- `PUT /api/profile/quit`
- `POST /api/profile/quit-attempts`
- `GET /api/profile/quit-attempts`

### 7.5 Event log endpoints

- `GET /api/events`
- `POST /api/events/batch`
- `POST /api/events/craving`
- `POST /api/events/relapse`
- `POST /api/events/nrt-usage`
- `POST /api/events/emergency-session`

### 7.6 User content endpoints

- `GET /api/user/motivation-phrases`
- `POST /api/user/motivation-phrases`
- `PUT /api/user/motivation-phrases/{id}`
- `DELETE /api/user/motivation-phrases/{id}`

## 8. Sync model

### 8.1 Local free users

- працюють повністю локально;
- можуть тягнути public content updates;
- не мають персонального sync state на сервері.

### 8.2 Premium users

- мають server-backed profile;
- sync виконують після login;
- конфлікти вирішуються предиктовано і явно.

### 8.3 Conflict resolution MVP

Для MVP достатньо такої політики:

- `updatedAt` + server-authoritative merge для профілю;
- idempotent batch import для подій;
- `clientGeneratedId` для дедуплікації;
- явне логування rejected records.

## 9. Security and privacy requirements

Обов'язково в MVP:

- TLS only;
- password hashing або external auth best practices;
- encrypted secrets management;
- мінімізація PII;
- audit trail для admin content changes;
- rate limiting на auth endpoints;
- account deletion flow;
- export user data flow;
- consent tracking.

## 10. Admin and operations

### 10.1 Admin content capabilities

MVP admin surface має дозволяти:

- додавати й публікувати товари;
- редагувати motivational phrases;
- редагувати milestones;
- керувати advice snippets;
- включати/вимикати content entries;
- міняти порядок відображення.

### 10.2 Observability

Потрібні:

- structured logs;
- request tracing;
- error tracking;
- health endpoints;
- dashboard по auth failures, sync failures, content publish issues.

## 11. Testing plan for backend MVP

### 11.1 Automated tests

- unit tests для calculators, entitlement rules, merge rules, validators;
- integration tests для DB, auth, event ingestion, content publishing;
- API contract tests;
- migration tests;
- permission tests для premium-only endpoints;
- idempotency tests для batch event ingestion.

### 11.2 Critical acceptance scenarios

- premium user register/login/logout;
- purchase activation updates entitlement;
- premium user updates quit profile on device A and sees changes on device B;
- relapse event не ламає історію;
- duplicate event batch не створює дублікати;
- local free user може оновити public content без акаунта;
- deleted or expired premium subscription блокує premium-only sync endpoints;
- user data export and deletion complete successfully.

## 12. Delivery plan

### Phase 1

- backend project bootstrap;
- PostgreSQL schema migrations;
- auth and subscription skeleton;
- public content endpoints;
- admin content CRUD API.

### Phase 2

- quit profile sync;
- event ingestion endpoints;
- device registration;
- deduplication and merge rules;
- telemetry and health checks.

### Phase 3

- premium activation hardening;
- export/delete flows;
- AI readiness tables and summary jobs;
- rollout support and operational dashboards.

## 13. Suggested first backlog slices

Найближчі технічні slices, які зручно давати агентам:

1. створити backend solution skeleton і базову конфігурацію;
2. додати PostgreSQL schema для User, Subscription, QuitProfile;
3. реалізувати `GET/PUT /api/profile/quit`;
4. реалізувати public content endpoints;
5. реалізувати `POST /api/events/batch` з idempotency;
6. реалізувати premium entitlement middleware;
7. додати integration tests для auth, sync і content.

## 14. Definition of Done

Backend MVP вважається готовим, якщо:

- premium account lifecycle працює end-to-end;
- content endpoints віддають версіонований контент клієнтам;
- quit profile sync працює стабільно;
- event ingestion і deduplication працюють;
- є базова observability;
- пройдені integration і contract tests;
- є manual verification notes для Android і Windows клієнтів.