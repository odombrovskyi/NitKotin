# NitKotin Service Roadmap

## 1. Мета продукту

Перетворити поточний застосунок NitKotin з локального мотиватора на повноцінний сервіс підтримки відмови від куріння з такими властивостями:

- корисний без реєстрації для базового безкоштовного сценарію;
- достатньо цінний у платній версії, щоб користувач бачив пряму вигоду від підписки;
- доступний на Windows, Android, iOS;
- побудований навколо довготривалої зміни поведінки, а не лише одного таймера;
- готовий до розвитку в AI-first продукт з персональним ментором.

## 2. Поточна база

У репозиторії вже є:

- Windows desktop app на WinForms/.NET 8;
- Android app на Kotlin + Jetpack Compose;
- локальні дані для каталогу товарів, мотиваційних фраз і recovery timeline;
- базова логіка відстеження часу без куріння та заощаджених коштів;
- віджет на Android.

Це означає, що продукт уже має перевірений core loop:

1. користувач фіксує старт;
2. бачить час без сигарет;
3. бачить фінансовий прогрес;
4. отримує мотиваційний контент;
5. бачить конкретну вигоду через товари та майлстоуни.

Подальший розвиток має зберегти цей простий core loop, а не ускладнити його зайвими екранами.

## 3. Продуктове бачення

NitKotin має стати персональним сервісом для відмови від куріння, який поєднує:

- трекінг прогресу;
- журнал подій та тригерів;
- підтримку під час craving episode;
- контентну бібліотеку;
- персональні досягнення;
- синхронізацію між пристроями;
- AI-ментора, який знає контекст користувача.

Ключова різниця від простого таймера: сервіс має допомагати пройти перші складні тижні, підтримувати звички місяцями та повертати користувача після зривів без сорому й friction.

## 4. Детальний опис фіч

### 4.1 Акаунт і доступ

Ціль:
дати анонімний low-friction вхід у продукт і водночас мати платний cloud-backed режим.

Рівні доступу:

- anonymous/local mode: без реєстрації, всі дані зберігаються лише на пристрої;
- premium account: платний акаунт із серверним збереженням, синхронізацією, AI-ментором, розширеною аналітикою, повною історією подій і персоналізацією.

Сценарії:

- старт без реєстрації за 10-20 секунд;
- м’який upsell до premium у моменти, коли користувач уже відчув цінність локального продукту;
- обов’язковий акаунт лише для платної підписки.

Принцип монетизації доступу:

- безкоштовний користувач не створює акаунт і не займає персональне серверне сховище;
- платний користувач отримує акаунт і весь cloud-backed сценарій;
- для безкоштовного користувача варто мінімізувати server cost через bundled content, CDN-style public content delivery або рідкі анонімні оновлення без персонального state.

### 4.2 Базовий quit-tracking

Обов’язкові дані:

- дата і час відмови від куріння;
- попередня інтенсивність куріння: сигарет або пачок на день;
- ціна пачки;
- опційно бренд/тип сигарет для точнішої аналітики;
- опційно обрані НЗТ засоби.

Похідні метрики:

- час без куріння;
- збережені кошти;
- кількість не викурених сигарет;
- streak поточного smoke-free періоду;
- окремо lifetime metrics з урахуванням можливих зривів.

### 4.3 Recovery timeline

Має показувати:

- що вже пройдено;
- що актуально зараз;
- що буде далі;
- щоденний та тижневий прогрес;
- пояснення без медичних обіцянок.

Вимоги:

- контент зберігається на сервері та кешується локально;
- контент версіонується;
- є medical disclaimer;
- для кожного milestone бажано мати короткий текст, розгорнутий текст і optional source metadata для внутрішньої редактури.

### 4.4 Екстрена кнопка “дуже хочу палити”

Це одна з найцінніших функцій для retention.

Сценарій:

1. користувач натискає emergency button;
2. вказує рівень тяги за шкалою 1-10 або пропускає;
3. отримує миттєву пораду в 1-2 реченнях;
4. опційно запускає коротку вправу на 60-180 секунд;
5. опційно відкриває чат з AI-ментором.

Види допомоги:

- короткі grounding prompts;
- breathing exercise;
- нагадування про причини кинути;
- персональні фрази користувача;
- рекомендація щодо НЗТ, якщо така логіка дозволена й безпечна в рамках legal review;
- escalation path: “якщо дуже важко, звернись до лікаря/гарячої лінії”.

### 4.5 Загальні поради та контентна бібліотека

Категорії:

- як пережити перші 24 години;
- як пережити перший тиждень;
- що робити з дратівливістю;
- як боротися з ритуалами після кави/їжі;
- як проходити соціальні тригери;
- як використовувати НЗТ;
- як не зірватися після однієї сигарети.

Контент має бути:

- короткий для швидкого споживання;
- глибший у вигляді статей або карток;
- персоналізований за стадією користувача;
- локалізований мінімум українською та англійською.

Важливо:

- не копіювати дослівно захищені авторським правом тексти з книг без ліцензії;
- для ідей з книги Алана Карра використовувати або власні оригінальні формулювання, або ліцензований контент після окремої legal перевірки.

### 4.6 Каталог товарів

Роль фічі:

- показувати матеріальний результат прогресу;
- мотивувати через конкретні покупки;
- створювати повторюваний engagement loop.

Можливості:

- серверний каталог товарів;
- категорії, ціни, картинки, теги;
- персоналізовані picks за бюджетом;
- обране або wishlist;
- підбірка “вже можеш купити”;
- підбірка “ще трохи і зможеш купити”.

Для MVP достатньо:

- ручне оновлення каталогу через admin panel;
- локальне кешування;
- 3-10 релевантних пропозицій на поточний бюджет.

### 4.7 Досягнення та бейджі

Бейджі мають працювати як behavioral reinforcement, а не просто як декорація.

Типи бейджів:

- за час: 1 доба, 3 доби, 1 тиждень, 1 місяць, 3 місяці, 1 рік;
- за гроші: перші 100 грн, 500 грн, 1000 грн, 5000 грн;
- за поведінку: записав першу тягу, пройшов 7 днів без зриву, додав персональну мотивацію, пройшов 10 emergency sessions без сигарети;
- за відновлення після relapse: повернувся до трекінгу після зриву.

Потрібні:

- server-side achievement rules;
- локальний optimistic display з подальшою синхронізацією;
- окремий progress screen;
- shareable badge cards як growth mechanic на майбутнє.

### 4.8 Мотиваційні фрази

Джерела:

- вбудовані редакційні фрази;
- персональні фрази користувача;
- AI-generated summaries або reminders, якщо це буде доречно та безпечно.

Функції:

- ротація на головному екрані;
- окремий список вибраних фраз;
- персональні фрази користувача з синхронізацією для premium users;
- контентні теги: здоров’я, гроші, сім’я, самоповага, енергія, relapse recovery.

### 4.9 НЗТ трекінг

НЗТ засоби:

- пластир;
- жувальна гумка;
- льодяники;
- аерозоль/спрей;
- інгалятор;
- інше.

Що фіксувати:

- які засоби використовує користувач;
- коли почав;
- частоту або події використання;
- суб’єктивну ефективність.

Практична цінність:

- дає кращий контекст AI-ментору;
- дозволяє будувати персональні підказки;
- допомагає зрозуміти зв’язок між cravings і coping mechanisms.

### 4.10 Події та журнал

Необхідні типи подій:

- викурив одну сигарету;
- мав сильну тягу;
- використав НЗТ;
- натиснув emergency button;
- отримав і виконав пораду;
- додав власну нотатку;
- змінив quit date;
- почав нову smoke-free спробу.

Ключовий принцип:

сервіс не має карати користувача за relapse. Він має фіксувати подію, коректно перераховувати метрики і повертати користувача в процес.

### 4.11 Трекінг тяги до сигарет

Мінімум:

- шкала 1-10;
- причина або тригер;
- час доби;
- опційний коментар.

Цінність:

- побудова heatmap по часу та тригерах;
- персональні рекомендації;
- оцінка ефективності НЗТ та порад;
- кращий контекст для AI-ментора.

### 4.12 AI-ментор

Роль:

- спокійно підтримує;
- знає контекст користувача;
- не соромить у разі relapse;
- не видає себе за лікаря;
- допомагає пройти cravings, стрес, дратівливість, страх зриву.

Контекст, який модель повинна знати:

- скільки курив користувач;
- коли кинув;
- чи були зриви;
- які НЗТ засоби використовує;
- які події останнім часом логувались;
- які мотиватори та персональні фрази важливі для користувача;
- поточний streak, savings, recent cravings pattern.

Типи AI-сценаріїв:

- chat mentor;
- emergency response;
- щоденний check-in;
- weekly summary;
- adaptive motivational suggestions.

Обмеження та safety:

- AI не дає медичних діагнозів;
- медичні або кризові сценарії відправляються в safe fallback відповіді;
- потрібен prompt safety layer і moderation layer;
- кожну AI-feature треба оцінювати за якістю, безпекою і вартістю inference.

### 4.13 Backend і серверний контент

На сервері мають жити:

- акаунти;
- профілі користувачів;
- smoking journey data;
- події;
- cravings;
- НЗТ usage data;
- персональні мотиваційні фрази;
- badge definitions і achievement progress;
- контентний каталог товарів;
- recovery milestones;
- редакційні мотиваційні фрази;
- AI conversation summaries і memory objects;
- subscription status.

Окремо потрібна адмінська поверхня для керування:

- каталогом товарів;
- milestone-контентом;
- бібліотекою порад;
- мотиваційними фразами;
- feature flags;
- AI prompts/templates.

### 4.14 Віджети та клієнтські поверхні

Платформи в цілі:

- Windows;
- Android;
- iOS.

Мінімальний UX стандарт для всіх клієнтів:

- головний екран з savings/time/status;
- мотивуючий контент;
- emergency action;
- badge/progress view;
- журнал подій;
- settings/profile;
- локальне кешування та робота з нестабільною мережею.

Віджети:

- savings;
- smoke-free time;
- optional quick action в майбутньому.

## 5. Пропонована архітектура сервісу

### 5.1 Backend domains

Рекомендований поділ:

- Auth and Identity Service;
- User Profile and Subscription Service;
- Quit Journey Service;
- Event and Craving Log Service;
- Content Service;
- Achievement Service;
- AI Mentor Orchestration Service;
- Admin CMS or Admin API;
- Analytics and Experimentation Layer.

На старті це може бути один modular monolith, а не відразу мікросервіси. Це дешевше, швидше й краще для керованості одним керівником і AI-агентами.

### 5.2 Рекомендований технічний підхід для backend MVP

Прагматичний варіант:

- REST API як основний контракт для клієнтів;
- PostgreSQL як primary relational store;
- object storage для media assets каталогу;
- background jobs для badge recalculation, summaries, content sync;
- Redis опційно для cache/rate limiting;
- vector store або embeddings index тільки коли реально з’явиться AI memory/search потреба.

На ранньому етапі не потрібно ускладнювати систему event sourcing або надмірною service decomposition.

### 5.3 Дані та основні сутності

Ключові сутності:

- User;
- Subscription;
- QuitProfile;
- SmokingBaseline;
- QuitAttempt;
- RelapseEvent;
- CravingEvent;
- NrtProfile;
- NrtUsageEvent;
- MotivationPhrase;
- UserMotivationPhrase;
- RecoveryMilestoneDefinition;
- ProductCatalogItem;
- ProductSuggestionSnapshot;
- BadgeDefinition;
- UserBadge;
- AISession;
- AIConversationSummary.

### 5.4 AI memory model

Щоб AI-ментор “пам’ятав” користувача без нескінченного prompt growth, треба мати 3 рівні контексту:

- persistent profile memory: baseline smoking data, quit goals, NRT preferences, tone preferences;
- rolling behavioral summary: останні cravings, relapse events, progress, активні тригери;
- session context: останні 10-20 повідомлень та поточний емоційний стан.

Це має бути окремою прикладною логікою, а не лише “довгим промптом”.

### 5.5 Privacy, security, legal

Обов’язково перед production:

- політика приватності;
- явна згода на обробку персональних даних;
- encryption in transit і at rest;
- account deletion flow;
- export user data;
- audit для admin actions;
- rate limiting і abuse protection для AI endpoints;
- окремий disclaimer: сервіс не є медичним діагностичним інструментом.

## 6. Що залишити безкоштовним, а що монетизувати

### 6.1 Безкоштовна версія

Безкоштовно має бути достатньо корисно, щоб продукт реально допомагав. Інакше не буде органічного росту.

Рекомендовано лишити free:

- локальний quit-tracking;
- базовий savings calculator;
- базовий recovery timeline;
- базовий каталог товарів;
- базові мотиваційні фрази;
- базові бейджі за час і гроші;
- локальний event log з обмеженою історією або без cloud sync;
- віджети;
- emergency button з rule-based порадами;
- локальна підтримка Windows/Android/iOS клієнтів без акаунта там, де це можливо.

### 6.2 Premium / paid

Платними мають бути фічі, що створюють регулярну довготривалу цінність, а не базову людську допомогу.

Рекомендовано monetizable:

- акаунт і серверне збереження даних;
- синхронізація між пристроями;
- cloud backup і відновлення після зміни девайса;
- AI-ментор з пам’яттю про користувача;
- персональні daily/weekly AI summaries;
- розширена аналітика cravings, relapse patterns, NRT effectiveness;
- необмежена історія подій і детальні графіки;
- розширені бейджі, journeys, challenge programs;
- розумні рекомендації контенту;
- premium content packs;
- сімейний режим або shared accountability у майбутньому;
- пріоритетні експериментальні AI features.

Не рекомендовано ховати за paywall:

- базовий лічильник часу без куріння;
- базовий лічильник заощаджених коштів;
- базову emergency допомогу;
- базовий recovery timeline.

### 6.3 Як показувати цінність premium локальному безкоштовному користувачу

Мета не в тому, щоб агресивно заважати, а в тому, щоб користувач побачив чітку й доречну вигоду.

Рекомендовані точки upsell:

- після 3-7 днів використання, коли користувач уже вклався в трекінг;
- після досягнення сильного milestone, наприклад 1 доба або 1 тиждень;
- при спробі додати другу платформу або перенести прогрес на інший пристрій;
- при відкритті AI-ментора або advanced analytics;
- після кількох зафіксованих cravings, коли персональна підтримка вже виглядає доречно.

Що саме показувати free local user:

- preview premium dashboard з прикладами інсайтів;
- пояснення, що premium зберігає прогрес у хмарі й синхронізує між пристроями;
- приклади AI-ментора: короткі демо-відповіді без повного доступу до чату;
- екран “що ти втратиш без backup”, коли користувач міняє або скидає пристрій;
- порівняльну таблицю local free vs premium.

Що не варто робити:

- блокувати базовий quit-tracking нав’язливими paywall screens;
- показувати upsell до того, як користувач взагалі зрозумів core value;
- змушувати реєстрацію для базових сценаріїв.

Практичний paywall copy напрям:

- “Збережи свій прогрес назавжди й синхронізуй його між пристроями”;
- “Отримай персонального AI-ментора, який знає твій шлях”;
- “Побач, які тригери найчастіше ведуть до зриву, і як їх обійти”.

### 6.4 Таблиця Free vs Premium

| Можливість | Local free | Premium |
| --- | --- | --- |
| Старт без реєстрації | Так | Так |
| Локальне збереження прогресу | Так | Так |
| Лічильник часу без куріння | Так | Так |
| Лічильник заощаджених коштів | Так | Так |
| Базовий recovery timeline | Так | Так |
| Базовий каталог товарів | Так | Так |
| Базові мотиваційні фрази | Так | Так |
| Базові бейджі | Так | Так |
| Віджети | Так | Так |
| Rule-based emergency допомога | Так | Так |
| Акаунт | Ні | Так |
| Хмарне збереження даних | Ні | Так |
| Синхронізація між пристроями | Ні | Так |
| Backup і відновлення прогресу | Ні | Так |
| Необмежена історія подій | Ні | Так |
| Персональні фрази з sync | Ні | Так |
| Розширена аналітика cravings / relapse / NRT | Ні | Так |
| AI-ментор з пам'яттю про користувача | Ні | Так |
| AI daily/weekly summaries | Ні | Так |
| Преміум challenge programs | Ні | Так |

Практична логіка таблиці:

- free дає повноцінний core quit-tracking і реальну щоденну користь;
- premium не “ремонтує зламаний free”, а додає continuity, insight і персоналізацію;
- головні тригери купівлі premium: страх втратити прогрес, потреба в sync, бажання мати AI-підтримку й глибшу аналітику.

## 7. Маркетингова стратегія

### 7.1 Позиціонування

Позиціонування не як “ще один таймер”, а як:

персональний сервіс підтримки відмови від куріння з прогресом, діями, контекстом і AI-підтримкою.

Ключові меседжі:

- бачиш реальний прогрес у часі, грошах і здоров’ї;
- не починаєш з нуля після кожної помилки;
- завжди маєш швидку допомогу в складний момент;
- сервіс росте разом з тобою від першого дня до року без куріння.

### 7.2 Цільові сегменти

- люди, які вже кілька разів пробували кинути;
- люди, яким важливий фінансовий ефект;
- користувачі, які люблять трекери, streaks, badges;
- користувачі, яким потрібна емоційна підтримка;
- люди, які вже використовують НЗТ і хочуть відслідковувати ефективність.

### 7.3 Канали росту

- ASO/SEO по ключах “кинути палити”, “quit smoking”, “smoke free”, “craving help”;
- короткі social content clips про savings milestones;
- shareable cards із бейджами та savings achievements;
- історії прогресу користувачів;
- контент-маркетинг навколо relapse recovery і cravings management;
- referral program після появи акаунтів та cloud sync.

### 7.4 Growth loops

- badge sharing;
- wishlist товарів і досягнутих покупок;
- weekly summary, який хочеться відкрити;
- AI mentor re-engagement nudges;
- recovery milestones reminders;
- “ти вже зекономив enough to buy ...” повідомлення всередині app surfaces.

### 7.5 Pricing ideas

Варто тестувати:

- місячна підписка;
- річна підписка з відчутною знижкою;
- trial 7-14 днів для AI mentor;
- win-back offer для churned users.

## 8. План розробки

Нижче наведений план для сценарію, де GitHub Copilot і Codex агенти виконують більшість реалізації, а власник продукту формує задачі, приймає рішення й тестує.

### Етап 0. Product foundation

Цілі:

- зафіксувати product scope;
- описати data model;
- затвердити API contracts;
- затвердити free vs premium boundaries;
- прийняти рішення по platform strategy.

Результати:

- product spec;
- backlog v1;
- architecture decision records;
- test strategy;
- privacy and legal checklist.

### Етап 1. Backend MVP foundation

Цілі:

- paid user accounts;
- profile storage;
- quit profile sync;
- basic content service;
- product catalog from server;
- motivational phrases from server;
- milestone definitions from server.

Результати:

- backend MVP;
- admin API;
- client sync for Android and Windows;
- observability базового рівня.

### Етап 2. Cross-device client integration

Цілі:

- onboarding in local-first mode;
- premium purchase and account activation flow;
- sync and offline cache for premium users;
- migration path from local-only users to paid cloud-backed users.

Результати:

- Android + Windows apps працюють з backend;
- локальний режим не ламається;
- дані контенту оновлюються з сервера.

### Етап 3. Journaling and behavior layer

Цілі:

- event log;
- cravings tracking;
- NRT tracking;
- relapse handling;
- improved achievements.

Результати:

- behavioral dataset для персоналізації;
- багатший продукт even without AI.

### Етап 4. AI mentor MVP

Цілі:

- AI chat;
- emergency AI response;
- session summaries;
- contextual memory;
- safety policies.

Результати:

- premium-ready differentiator;
- контрольована модель витрат на inference;
- eval suite для AI quality.

### Етап 5. iOS and platform expansion

Цілі:

- iOS client;
- unified product design system;
- optional desktop modernization.

Результати:

- повний platform coverage;
- менше platform drift.

### Етап 6. Monetization and scale

Цілі:

- subscriptions;
- experiments;
- pricing tests;
- lifecycle campaigns;
- referral and retention features.

## 9. Розподіл роботи між людиною та AI-агентами

### 9.1 Що робить власник продукту

- формує backlog і пріоритети;
- приймає продукті рішення;
- перевіряє UX на реальних пристроях;
- проводить acceptance testing;
- вирішує, що йде в реліз;
- стежить за legal, privacy, billing, content risks.

### 9.2 Що роблять Copilot/Codex агенти

- реалізують невеликі та середні технічні slices;
- оновлюють тести;
- пишуть документацію;
- пропонують рефакторинг;
- допомагають з backend contracts, migrations, admin tools, CI/CD.

### 9.3 Рекомендований режим роботи

Для кожної фічі:

1. створити короткий spec з acceptance criteria;
2. визначити affected files / services;
3. дати агенту вузьку задачу;
4. вимагати focused validation після кожного edit slice;
5. приймати зміни лише після реальної перевірки на пристрої або в тестах.

### 9.4 Правило для великих фіч

Будь-яку велику фічу розбивати щонайменше на:

- product spec;
- backend contract;
- storage/data changes;
- client UI;
- analytics;
- tests;
- release note.

## 10. План тестування

### 10.1 Рівні тестування

- unit tests для calculators, rules, badge logic, craving classification, NRT mapping;
- integration tests для API, auth, sync, background jobs;
- contract tests між backend і клієнтами;
- UI tests для головних флоу на Android/iOS/Windows;
- end-to-end tests для onboarding, sync, relapse logging, achievement unlock;
- AI eval tests для mentor scenarios;
- manual exploratory testing на реальних девайсах.

### 10.2 Критичні сценарії для обов’язкового покриття

- старт без реєстрації;
- купівля premium і створення акаунта;
- міграція з local-only у paid cloud mode;
- sync між двома пристроями;
- журнал relapse без втрати історії;
- emergency button response under poor network;
- додавання персональної фрази;
- оновлення каталогу товарів із сервера;
- badge unlock;
- віджет після зміни даних;
- subscription gating;
- видалення акаунта і даних.

### 10.3 AI-специфічне тестування

Треба окремо оцінювати:

- контекстуальність відповіді;
- tone safety;
- відсутність shame language;
- коректність у relapse scenarios;
- відповідь на high-craving cases;
- fallback behavior при відсутності контексту;
- prompt injection resistance у межах practical risk profile.

AI quality має мати власний eval dataset і release gate.

### 10.4 Release criteria

Перед кожним release:

- green CI;
- пройдені regression tests по ключових флоу;
- manual smoke на реальних пристроях;
- перевірка telemetry і crash-free startup;
- перевірка offline and sync behavior;
- перевірка widget surfaces;
- перевірка premium gating;
- оновлені docs і changelog.

## 11. Варіанти спільного фреймворку для desktop + mobile

### Варіант A. Залишити поточні native apps і будувати спільний backend

Плюси:

- найшвидший шлях до backend-enabled продукту;
- не втрачається вже зроблена робота на Android і Windows;
- найменший ризик повного rewrite;
- можна окремо додати iOS пізніше.

Мінуси:

- більше platform-specific коду;
- вища вартість підтримки в довгу.

Практичний висновок:

це найкращий шлях на найближчі етапи, якщо ціль номер один зараз не “єдиний UI codebase”, а “випустити сервіс швидше”.

### Варіант B. React Native без Expo, з native builds

Плюси:

- один мобільний codebase для Android та iOS;
- велика екосистема;
- TypeScript зручний для агентної розробки;
- є React Native Windows для потенційного desktop-напрямку.

Мінуси:

- поточний Android app доведеться переписувати;
- Windows desktop теж фактично потребуватиме окремого rewrite, якщо йти в React Native Windows;
- складні native modules, widgets, platform integration все одно залишаються platform-specific.

Коли обирати:

- якщо готові до керованого rewrite клієнтів;
- якщо пріоритет номер один це швидка розробка Android+iOS з одним UI стеком;
- якщо desktop можна лишити окремим або відкласти.

### Варіант C. Kotlin Multiplatform + Compose Multiplatform

Плюси:

- краще перевикористання поточного Android Kotlin коду;
- можна винести спільні domain/data layers;
- потенційно хороша база для Android+iOS;
- нижча когнітивна вартість для існуючого Android напряму.

Мінуси:

- Windows WinForms код напряму не перевикористовується;
- desktop migration все одно означає новий UI шар;
- tooling і cross-platform polish можуть вимагати більше часу, ніж очікується.

Коли обирати:

- якщо хочеться максимізувати reuse Android-коду;
- якщо mobile-first важливіший за desktop unification;
- якщо команда готова жити в Kotlin-centric стеку.

### Варіант D. .NET MAUI

Плюси:

- сильніший зв’язок з поточним C# світом;
- потенційно один стек для Windows + Android + iOS;
- native builds.

Мінуси:

- Android застосунок теж доведеться переписувати;
- існуючий Kotlin/Compose код не реюзається;
- UX і ecosystem fit треба окремо перевіряти на вашому продукті.

### Рекомендація

Рекомендований порядок рішень:

1. не переписувати на єдиний framework прямо зараз;
2. спочатку побудувати backend, sync, content service, event log і AI-ready data model;
3. залишити Windows і Android поточними ще на 1-2 великих етапи;
4. перед стартом iOS окремо прийняти platform decision.

Якщо потрібен саме shared mobile stack, найбільш прагматичний вибір для rewrite-кандидата:

- React Native bare workflow, якщо пріоритет це швидкість Android+iOS розробки одним UI codebase;
- Kotlin Multiplatform, якщо пріоритет це reuse поточного Android Kotlin коду і поступова еволюція без різкого технологічного розвороту.

Для цього проєкту на поточному етапі більш прагматично виглядає не повний rewrite, а backend-first розвиток з відкладеним platform consolidation decision.

## 12. Рекомендований пріоритет backlog v1

Найближчий practical backlog:

1. описати backend entities і API contracts;
2. реалізувати paid accounts і premium activation flow;
3. винести каталог товарів, milestones і motivational phrases на сервер;
4. додати sync для quit profile;
5. додати журнал подій: relapse, craving, NRT usage;
6. додати базові badge rules;
7. додати персональні мотиваційні фрази;
8. лише після цього брати AI mentor MVP.

## 13. Definition of Done для кожної нової фічі

Фіча вважається завершеною, якщо:

- є короткий spec;
- є acceptance criteria;
- є tests на відповідному рівні;
- є manual verification notes;
- оновлена документація;
- немає відомих критичних regressions;
- є зрозумілий rollout plan, якщо зміна ризикована.

## 14. Підсумок

Найкраща стратегія для NitKotin зараз:

- не починати з тотального rewrite клієнтів;
- спершу побудувати backend і дані для сервісної моделі;
- зберегти сильні сторони поточних застосунків;
- робити premium навколо AI, аналітики, sync і персоналізації;
- залишити базову допомогу безкоштовною;
- використовувати AI-агентів для delivery невеликих, чітко окреслених slices під ручним керуванням і тестуванням.