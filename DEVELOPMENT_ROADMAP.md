# Adventure Island 2 - Development Roadmap

## 1. מטרת המסמך

מסמך זה מתרגם את דרישות ה־Final Project לתכנית פיתוח מדורגת. הוא משמש כ־source of truth להתקדמות, כאשר בכל שלב מגדירים מערכת שלמה, בודקים אותה, ורק לאחר מכן עוברים לשלב הבא.

הסדר המוביל הוא:

1. קודם תשתית וקוד יציב.
2. אחר כך מערכות gameplay מחוברות.
3. לאחר מכן בניית שלבים, UI, אמנות, איזון ותיעוד.

אין להתחיל בבניית כל התוכן לפני שקיימת תשתית שמאפשרת להוסיף נשקים, חיות, אויבים וחפצים בלי לשנות את הלוגיקה המרכזית.

## 2. תמונת מצב נוכחית

### מה כבר קיים בפרויקט

- פרויקט Unity 2D עם סצנה פעילה: `Assets/Scenes/Scene_Physics.unity`.
- תנועת שחקן וקפיצה: `PlayerMovement.cs`, `PlayerJump.cs`.
- בסיס של איסוף וחפצים: `PickUp.cs`, `SC_Coin.cs`, `StarController.cs`.
- בסיס של אויבים: `Enemy.cs`, `Goomba.cs`, `KoopaTroopa.cs`, `EnemySpawner.cs`.
- בסיס של נשקים וקליעים: `AxeWeapon.cs`, `FireballWeapon.cs`, `ProjectileAxe.cs`, `ProjectileFireball.cs`.
- שימוש חלקי ב־Factory, Strategy, Builder ו־Pooling.
- הפרדת Model/View/Controller קיימת חלקית במערכת המטבעות.
- יש כלי Editor לבניית/הצבת תוכן: `BuildLevel.cs`, `PrefabSpawnerWindow.cs`.

### פערים מרכזיים מול הדרישות

- התוכן הנוכחי עדיין מבוסס בחלקו על Mario ולא על Adventure Island 2.
- אין מערכת מלאה לניהול כוח, פסילות, איפוס שלב ואיפוס משחק.
- אין מערכת של שני שלבים עם מעבר אוטומטי ביניהם.
- אין מערכת חיות רכיבות עם שלוש חיות והחלפה ביניהן.
- אין את כל סוגי הנשקים וההתנהגות הנדרשת לפטיש ולבומרנג.
- אין את כל שבעת סוגי האויבים והחוקים המיוחדים שלהם.
- אין מערכת ביצים שמייצרת חיה, נשק או פייה.
- אין מערכת טיימר לחזרת אויבים לאחר השמדה.
- אין מערכת פייה של 10 שניות עם החריג של תהום.
- אין בדיקות מקיפות, מסכי משחק מלאים, ואימות מלא של דרישות ההגשה.

הסטטוסים במסמך הם הערכת תכנון על בסיס בדיקה סטטית של הקבצים. לא נטען כאן שהמערכות הקיימות עובדות runtime עד שתבוצע בדיקה בתוך Unity.

## 3. עקרונות ארכיטקטורה מחייבים

### חלוקת שכבות

```text
Core / Domain
  חוקים טהורים של המשחק, נתונים, interfaces ו-events

Application / Services
  תזמור use-cases: התחלת שלב, פגיעה, איסוף, respawn, מעבר שלב

Infrastructure / Unity
  MonoBehaviours, Physics2D, Animator, Prefabs, Scene loading, Audio

Presentation
  Views, HUD, animations, effects and feedback
```

כלל חשוב: חוק משחק לא יישב בתוך View, Prefab או `OnCollisionEnter2D` בלבד. ה־MonoBehaviour יקבל את אירוע Unity, יעביר אותו ל־service, וה־service יפעיל את חוק המשחק.

### SOLID בפועל

- Single Responsibility: שחקן, כוח, פסילות, נשק, קליע, אויב, UI ו־level flow יהיו רכיבים נפרדים.
- Open/Closed: הוספת חיה או אויב חדש תיעשה דרך implementation חדש ו־Factory/Registry, בלי לשנות switch מרכזי ארוך.
- Liskov Substitution: כל weapon, enemy או mount יכבד את החוזה המשותף שלו.
- Interface Segregation: interfaces קטנים כמו `IDamageable`, `ICollectible`, `IAttacker`, `IRespawnable`.
- Dependency Inversion: services יתבססו על interfaces ויקבלו dependencies דרך Composition Root/DI.

### Patterns שצריך להראות בפרויקט

- DI: הזרקת services, repositories, factories ו־configuration ב־composition root.
- Factory: יצירת נשקים, חיות, אויבים ו־egg rewards לפי הגדרה.
- Builder: בניית projectile/configuration מורכב עם מהירות, כיוון, lifetime ואפקטים.
- Pooling System: קליעים, אויבים, collectibles ואפקטים חוזרים.
- MVC: Model של state, Controller של input/use-cases, View של UI/visual feedback.
- Template Method: בסיס משותף ל־enemy lifecycle או ל־collectible lifecycle, עם שלבים שניתנים למימוש על ידי תתי־מחלקות.
- Strategy: התנהגות תקיפה ותנועה שניתנת להחלפה, במיוחד עבור חיות ואויבים.
- Async & Tasks: טעינת שלב, countdown/respawn timers ותהליכים שאינם חוסמים את ה־main flow. Unity API יישאר על ה־main thread.

## 4. מבנה קוד יעד

```text
Assets/Scripts/
  Core/
    Domain/
    Interfaces/
    Events/
    Configuration/
  Composition/
  Application/
    GameFlow/
    Player/
    Combat/
    Collectibles/
    Enemies/
    Animals/
  Infrastructure/
    Unity/
    Pooling/
    SceneLoading/
  Presentation/
    Player/
    HUD/
    Screens/
    Effects/
  Content/
    Weapons/
    Animals/
    Enemies/
    Obstacles/
    Rewards/
  Editor/
```

השמות הם יעד ארכיטקטוני, לא הוראה להזיז את כל הקבצים בבת אחת. כל refactor יבוצע רק כחלק משלב מוגדר ולאחר שההתנהגות הקיימת נבדקה.

## 5. Roadmap לפי שלבים

## Phase 0 - Audit, Definition and Safety Gate

### מטרה

להגדיר את החוזים, גבולות המערכות, מבנה הסצנות ו־acceptance criteria לפני כתיבת קוד חדש.

### משימות

- ליצור רשימת דרישות סופית מתוך מסמך המטלה.
- להחליט על naming אחיד: `Life` מול `Health`, `Power`, `Stage`, `Enemy` וכו'.
- להגדיר layers, tags, collision matrix ו־input actions.
- להגדיר את ה־scene flow: Boot/Main Menu/Stage 1/Stage 2/Result.
- לקבוע מה יישאר מהקוד הקיים ומה יוחלף לאחר audit של התנהגות.
- ליצור Definition of Done לכל מערכת: קוד, prefab, scene integration, test, playtest ותיעוד.

### Gate

השלב הזה לא דורש כתיבת כל ה־interfaces או כל התשתיות. הוא מסתיים כאשר ברור מה המערכת הראשונה שנבנה, מה הקלט שלה ומה התוצאה שלה.

## איך עובדים על כל שלב - Vertical Slice

כל שלב ב־roadmap הוא מערכת אחת שלמה, ולא רשימת interfaces כללית. בתוך מערכת עובדים בסדר הבא:

1. מגדירים את חוק המשחק במשפט אחד.
2. מגדירים רק את ה־interface או ה־contract שהמערכת הזאת באמת צריכה.
3. בונים את ה־Model/service של המערכת.
4. בונים Unity adapter: `MonoBehaviour`, collision, prefab או input.
5. מוסיפים pattern רק אם הוא פותר בעיה אמיתית במערכת.
6. מוסיפים test קטן ומבצעים playtest.
7. רק אחרי שהמערכת עובדת עוברים למערכת הבאה.

לדוגמה, אין ליצור עכשיו `IWeapon`, `IMount` ו־`IObjectPool` רק כי בעתיד נצטרך אותם. במערכת הראשונה ניצור `ICollectible` רק אם הוא נדרש לפרי, נממש פרי אחד מקצה לקצה, ורק אז נחליט איך להרחיב אותו לפירות, ביצים וחיות.

## Phase 1 - First Vertical Slice: Fruit Collection and Power

### למה מתחילים כאן

זו מערכת קטנה שמחברת חוק משחק, collectible, state של השחקן ו־HUD עתידי. היא נותנת בסיס אמיתי ל־SOLID ול־MVC בלי לבנות מראש ארכיטקטורה ריקה.

### חוק המערכת

כאשר השחקן אוסף פרי, הפרי נעלם והשחקן מקבל כוח לפי סוג הפרי. פרי מסוג 1 מוסיף 1 כוח ופרי מסוג 2 מוסיף 2 כוח.

### מה מממשים בפועל

1. `PowerModel` שמחזיק את ערך הכוח ומאפשר `AddPower(amount)`.
2. `ICollectible` קטן עם פעולה אחת שמתאימה לאיסוף.
3. `FruitCollectible` שמממש את ה־interface ומחזיק `powerValue`.
4. `FruitController` שמזהה את השחקן ומעביר את האיסוף ל־service.
5. View/feedback בסיסי: העלמת הפרי ועדכון טקסט זמני או HUD פשוט.
6. test ל־1 ו־2 כוח ו־playtest בתוך הסצנה.

### Pattern בשלב הזה

- MVC בצורה מצומצמת: `PowerModel`, controller של האיסוף ו־view של התצוגה.
- DI ידני רק עבור `PowerModel`/service אם החיבור מצדיק זאת.
- אין Pooling, Factory או Builder בשלב הזה, כי עדיין אין צורך אמיתי בהם.

### Acceptance criteria

- שני סוגי פירות עובדים במשחק.
- כל פרי נאסף פעם אחת בלבד.
- הערך מתעדכן ב־Model ולא מחושב בתוך ה־View.
- יש test ו־playtest מתועד.

## Phase 2 - Player Movement and Lives

### מטרה

להשלים את השליטה הבסיסית ואת מחזור החיים של השחקן, תוך חיבור למערכת הכוח שכבר נבנתה.

### מערכות

#### 1. Player Movement System

- תנועה שמאלה/ימינה.
- קפיצה בכפתור אחד.
- input abstraction נפרד מ־movement implementation.
- תמיכה ב־grounded check, death lock ו־respawn.

#### 2. Player Lives

- התחלה עם 3 פסילות.
- `LoseLife()` מחזיר את השחקן לתחילת השלב ומאפס את השלב.
- כאשר הפסילות מגיעות לאפס: reset מלא וחזרה להתחלה.
- מוות מכוח אפס מפעיל את אותה פעולה, ולא מסלול מיוחד בתוך ה־View.

### Acceptance criteria

- playtest מוכיח תנועה, קפיצה, פסילה ו־reset.
- מוות לא מפעיל כמה פסילות מאותו אירוע.
- ה־HUD רק מציג את מספר הפסילות.

## Phase 3 - Combat, Weapons and Projectiles

### מטרה

להקים מערכת נשק כללית שתומכת בפטיש ובומרנג, עם הרחבה עתידית בלי שינוי ב־PlayerController.

### מערכות

#### 1. Weapon Inventory and Equip

- player יכול להחזיק נשק אחד פעיל.
- collect/equip/unequip דרך `WeaponService`.
- `IWeapon` מגדיר פעולה כללית; לכל נשק behavior עצמאי.

#### 2. Hammer

- איסוף, החזקה וזריקה.
- projectile עם כיוון, מהירות, lifetime ו־collision policy.
- פגיעה באבן ובאויבים לפי rules.

#### 3. Boomerang

- איסוף וזריקה.
- תנועה החוצה וחזרה לשחקן.
- אפשרות לאסוף מחדש אם חזר.
- pooling כדי למנוע instantiate/destroy חוזר.

#### 4. Projectile Builder and Factory

- Builder להרכבת projectile runtime configuration.
- Factory ליצירת projectile לפי weapon definition.
- Pool מחזיר projectile למצב נקי.

### Acceptance criteria

- שתי נשקים ניתנים להחלפה בזמן משחק.
- נשק יכול להשמיד אבן; כללי הפגיעה אינם מקודדים ב־View.
- קיימים tests עבור direction, lifetime, hit ו־release.

## Phase 4 - Animals, Mounting and Animal Attacks

### מטרה

לבנות את מערכת איסוף ורכיבת החיות, כולל החלפה בזמן שלב.

### מערכות

#### 1. Animal Contract

- `IMount`/`IAnimal` עבור חיה שניתן לאסוף ולרכב עליה.
- mount/unmount, movement integration ו־damage behavior.
- כאשר אוספים חיה חדשה בזמן רכיבה, החיה החדשה מחליפה את הקודמת.

#### 2. Three Animal Strategies

- כחולה: התקפת זנב; איסוף באמצעות לב.
- אדומה: יריקת אש; איסוף באמצעות עלה.
- ירוקה: סיבוב במקום; איסוף באמצעות כוכב.
- Strategy נפרדת לכל attack, כדי שה־mount controller לא יכיר את פרטי החיות.

#### 3. Animal Interaction Rules

- התקפת חיה הורסת אבנים אך לא מדורות.
- התקפת חיה הורגת אויבים, חוץ מרוח רפאים.
- פגיעה באבן בזמן רכיבה מעלימה גם את האבן וגם את החיה.
- פגיעה במדורה בזמן רכיבה מעלימה גם את המדורה וגם את החיה.

### Acceptance criteria

- כל שלוש החיות ניתנות לאיסוף במהלך שלב.
- כל חיה מציגה attack שונה ו־feedback ברור.
- החלפת חיה בזמן רכיבה עובדת בלי לשכפל mounts או להשאיר subscriptions.

## Phase 5 - Obstacles, Collectibles and Eggs

### מטרה

ליישם את כל החפצים והאינטראקציות שלהם באמצעות contracts משותפים וחוקים מפורשים.

### מערכות

#### 1. Stone

- collision עם שחקן מוריד 3 כוח.
- hammer/boomerang/animal attack יכולים להשמיד.
- destroyed state ואפקט visual דרך event.

#### 2. Bonfire

- collision עם שחקן גורם פסילה.
- רק פייה יכולה להשמיד.
- animal attack ונשקים רגילים אינם משמידים.

#### 3. Fruits

- שני סוגים עם values של 1 ו־2 כוח.
- pickup חד־פעמי, feedback ו־pool release.
- 30 פירות מפעילים פסילה לפי דרישת המטלה.

#### 4. Eggs

- פתיחת ביצה יוצרת reward אחד: animal, weapon או fairy.
- Reward Factory מבודד את בחירת התוכן מה־Egg View.
- egg state מונע פתיחה כפולה.

#### 5. Fairy

- נשארת עם השחקן 10 שניות.
- השחקן בלתי מנוצח ומשמיד כל דבר במגע, חוץ מתהום.
- timer ניתן לביטול/איפוס בצורה בטוחה.

### Acceptance criteria

- לכל חפץ יש interaction matrix מתועד.
- חוקי האבן, המדורה והפייה נבדקים בנפרד.
- ביצה מייצרת rewards שונים דרך factory ללא שינוי ב־egg logic.

## Phase 6 - Enemy Framework and Enemy Content

### מטרה

להחליף את בסיס האויבים הקיים במערכת שניתנת להרחבה ומכסה את כל דרישות המטלה.

### מערכות

#### 1. Enemy Lifecycle

- Template Method עבור initialize, patrol/decision, attack, damage, death ו־respawn.
- `IDamageable` ו־death event אחידים.
- כל אויב מושמד מפיל חיה לאיסוף לפי configuration.
- לאחר השמדה מתחיל timer; בסיום האויב חוזר לאותו מקום.

#### 2. Enemy Behaviours

- עכביש: תנועה למעלה/למטה או מצב סטטי באוויר.
- ציפור: תנועה שמאלה עם ירידה ועלייה.
- נחש קופץ: קפיצה קדימה.
- נחש יורה: ירי כדור אש.
- צפרדע: קפיצה גבוהה ורחוקה כאשר השחקן מתקרב.
- רוח רפאים: אינה ניתנת להשמדה על ידי נשק/חיה; רק פייה יכולה להשמיד.

#### 3. Enemy Factory and Pool

- Factory מייצר enemy לפי `EnemyDefinition`.
- Pool מנהל spawn/death/respawn ומנקה state.
- Async timer לרגע החזרה, עם cancellation כאשר עוברים שלב או מאפסים משחק.

### Acceptance criteria

- כל אויב קיים כ־prefab עם behavior עצמאי.
- כללי ההשמדה והרוח מתועדים ונבדקים.
- אין `switch` אחד שמכיל את כל לוגיקת האויבים.

## Phase 7 - Level System and Two Stages

### מטרה

לבנות שני שלבים מלאים עם flow ברור ומעבר אוטומטי.

### מערכות

#### 1. Level Definition and Builder

- `StageConfig` מגדיר theme, spawn point, power, enemy placements, goal ו־music.
- Builder או level loader בונה את הסביבה מתוך data/prefabs.
- scene-specific content נשאר מופרד מה־gameplay services.

#### 2. Stage 1 - Horizontal Adventure

- סגנון הליכה עד סוף השלב ימינה.
- שילוב אבנים, מדורות, פירות, ביצים, נשקים, חיות ואויבים.
- goal שמפעיל מעבר ישיר לשלב השני.

#### 3. Stage 2 - Maze Platforming

- סגנון מבוך עם קפיצות.
- מסלול ברור אך מאתגר עד סוף השלב.
- שימוש שונה בתנועה, אויבים ומכשולים לעומת שלב 1.

#### 4. Scene Loading

- מעבר שלב עם loading state.
- Async/Tasks לטעינת scene או הכנת content, בלי להקפיא את המשחק.
- שמירת lives/score לפי החלטת design מתועדת; reset מלא כאשר נגמרות הפסילות.

### Acceptance criteria

- אפשר להשלים Stage 1 ולעבור אוטומטית ל־Stage 2.
- אפשר להשלים Stage 2 ולהגיע למסך סיום.
- reset, pause ומעבר scene אינם משאירים timers, enemies או pooled objects מהשלב הקודם.

## Phase 8 - MVC HUD, Screens and Feedback

### מטרה

להציג את מצב המשחק באופן ברור ולהפריד UI מהלוגיקה.

### מערכות

- HUD: כוח, פסילות, סוג נשק, חיה רכובה, מספר פירות וטיימר פייה.
- Main Menu, Pause, Stage Complete, Game Over ו־Final Complete.
- Model מחזיק display state; Controller מתרגם events לפעולות UI; View מצייר בלבד.
- feedback לאיסוף, פגיעה, השמדת אויב, איבוד כוח ומעבר שלב.
- עדכון ה־README והוראות controls בהתאם לגרסה הסופית.

### Acceptance criteria

- שינוי state מתעדכן ב־HUD דרך event/binding ולא דרך polling מיותר.
- כל מסך ניתן לפתיחה וסגירה במצב מבוקר.
- ה־UI לא מכיל חוקי collision או חישוב damage.

## Phase 9 - Art, Audio, Balance and Polish

### מטרה

להחליף placeholder content בתוכן עקבי ולהכין build שניתן להציג.

### משימות

- יצירת sprites/animations לכל player, animal, enemy, weapon, obstacle ו־reward.
- התאמת sorting layers, colliders, pivots ו־physics materials.
- מוזיקה ואפקטי קול לאיסוף, זריקה, פגיעה, מוות, פייה ומעבר שלב.
- איזון כוח, מרחקים, spawn points, timers וקושי.
- בדיקת readability: השחקן צריך לזהות מה ניתן לאסוף, מה פוגע ומה ניתן להשמיד.

## Phase 10 - Testing, Playtest and Submission Video

### מטרה

להוכיח שכל דרישה מתקיימת ולבנות סרטון שמסביר את הפיתוח, לא רק מציג gameplay.

### בדיקות חובה

- Unit tests ל־Power, Lives, Fruit values, Egg rewards ו־interaction rules.
- Tests ל־weapon/projectile direction, boomerang return ו־pool reset.
- Tests ל־enemy respawn timer ולחריגים של ghost/fairy.
- Integration test ל־Stage 1 -> Stage 2 -> completion.
- Playtest מלא: game over, reset, mount replacement, obstacle destruction, all enemy types.
- בדיקה על build נקי עם scenes ב־Build Settings.

### מבנה סרטון ההסבר

1. הצגת המשחק והדרישות.
2. מבנה התיקיות וה־architecture.
3. SOLID ודוגמאות ל־DI.
4. Factory, Builder, Strategy, Template Method ו־Pooling.
5. Async/Tasks ומעבר בין שלבים.
6. הדגמת player, weapons, animals, enemies, obstacles, eggs and fairy.
7. gameplay מלא של שני השלבים.
8. בדיקות, limitations ו־build סופי.

### Definition of Done סופי

- שני שלבים ניתנים לסיום.
- כל הדרישות במסמך המטלה קיימות ומתועדות.
- כל ה־patterns שנדרשו ניתנים להצגה בקוד אמיתי.
- אין compile errors או missing references.
- Git מכיל source, scenes, prefabs, assets ו־metadata הנדרשים; generated folders נשארים ב־`.gitignore`.
- יש build/recording וה־README מסביר איך להריץ.

## 6. סדר עבודה מומלץ בתוך כל מערכת

לכל מערכת יש לעבוד תמיד בסדר הבא:

1. Contract: interface, data model ו־acceptance criteria.
2. Domain rule: לוגיקה שאפשר לבדוק בלי Unity כאשר אפשר.
3. Application service: תזמור הפעולה והאירועים.
4. Unity adapter: collision, input, prefab ו־animation.
5. Tests: unit ואז integration.
6. Scene/prefab integration.
7. Playtest קצר.
8. Commit קטן עם הודעה המתארת feature אחד.

## 7. סדר עדיפויות מעשי

### Must Have

- תנועה, קפיצה, התקפה ונשק.
- כוח, 3 פסילות, פירות ו־reset.
- אבנים, מדורות, ביצים ופייה.
- שלוש חיות ומערכת רכיבה.
- שבעה סוגי אויבים וכללי ההשמדה שלהם.
- שני שלבים ומעבר ביניהם.
- HUD, מסכי סיום, בדיקות וסרטון.

### Should Have

- Pooling לכל אובייקט חוזר.
- Async scene loading מלא.
- animations/audio עשירים.
- כלי Editor ליצירת שלבים מתוך data.

### לא להרחיב לפני הסיום

- multiplayer.
- מערכת שמירה מורכבת.
- חנות/כלכלה.
- procedural generation.
- תוכן שאינו מופיע בדרישות המטלה.

## 8. כללי עבודה והתקדמות

- כל שלב יסתיים ב־playable checkpoint.
- לא לערבב refactor רחב עם feature חדש באותו commit.
- לא להוסיף מערכת חדשה לפני שמזהים את ה־owner שלה ואת ה־interface שלה.
- כל שינוי ב־collision או ב־scene ייבדק ב־Unity, לא רק בקוד.
- כל claim על feature ייחשב מאומת רק לאחר playtest או test מתאים.
- לאחר כל milestone יש לעדכן את המסמך בסטטוס: `Not started`, `In progress`, `Verified`.

## 9. סטטוס התחלתי

| Phase | נושא | סטטוס |
|---|---|---|
| 0 | Audit and definition | In progress |
| 1 | First vertical slice: fruit and power | Not started |
| 2 | Player movement and lives | Partial foundation exists |
| 3 | Weapons and projectiles | Partial foundation exists |
| 4 | Animals and mounting | Not started |
| 5 | Obstacles, collectibles and eggs | Partial collectibles foundation exists |
| 6 | Enemy framework and all enemies | Partial unrelated enemy foundation exists |
| 7 | Two stages and scene flow | Not started |
| 8 | MVC HUD and screens | Partial foundation exists |
| 9 | Art, audio and balance | Not started |
| 10 | Tests, playtest and video | Not started |

## 10. מה מממשים עכשיו

המשימה הראשונה היא **Phase 0 בלבד**: audit קצר והגדרת חוקי המערכת הראשונה.

מיד אחריה מממשים את **Phase 1 - Fruit Collection and Power**:

1. `PowerModel` עם ערך כוח.
2. `ICollectible` יחיד, רק אם הוא נדרש לחיבור הפרי.
3. פרי אחד שנותן 1 כוח.
4. בדיקה ו־playtest.
5. רק לאחר שהפרי הראשון עובד, מוסיפים את פרי 2 ואת ההרחבה הנדרשת.

בשלב הזה לא מממשים `IWeapon`, `IMount`, `IEnemyFactory`, `IObjectPool`, את כל האירועים או את כל ה־DI. כל אחד מהם ייכנס ל־roadmap רק כאשר נגיע למערכת שבאמת צריכה אותו.
