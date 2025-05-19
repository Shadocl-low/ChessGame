### Пилявський Олександр Іванович
### vt231_poi@student.ztu.edu.ua

![Row's of code](C:\Users\1\Downloads\SD\ChessGame\Images\code.png)

# Гра шахи

### Мета проєкту
Розробити настільну гру "Шахи" у вигляді WPF-додатку з графічним інтерфейсом користувача. Програма дозволяє двом гравцям грати у класичні шахи на одному пристрої, з можливістю зберігати та завантажувати партії.

## Основні функціональності
1. Графічний інтерфейс (UI)
+ Візуальне шахове поле 8x8, побудоване з використанням Grid.
+ Фігури зображені як зображення або стилізовані кнопки.
+ Панель управління з кнопками:
    + Нова гра
    + Зберегти гру
    + Завантажити гру
    + Завершити гру
2. Ігрова логіка
+ Реалізація всіх шахових фігур та їх правил руху:
    + Пішак, тура, кінь, слон, ферзь, король
+ Чергування ходів між двома гравцями (білими та чорними).
+ Валідація ходів: неможливість робити неправильні ходи.
+ Виявлення:
    + Шаху
    + Мату
    + Патового положення
+ Основні спецправила:
    + Рокіровка
    + Перетворення пішака
3. Збереження та завантаження гри
+ Збереження поточного стану гри (позиція фігур, черга ходу, список ходів) у файл (наприклад, .json).
+ Завантаження гри з файлу для продовження збереженої партії.
4. Повідомлення та статус гри
+ Інформація про поточний хід (який гравець ходить).
+ Відображення повідомлень: "Шах", "Мат", "Пат", "Нічия", "Перемога гравця".

## Формат збереження гри
Використання JSON або XML для зберігання:
+ Позиції всіх фігур
+ Поточного гравця

## Додаткові можливості (опціонально)
+ Вибір тем оформлення (кольори шахівниці, стилі фігур) +
+ Автосейв гри після кожного ходу
+ Підказка можливих ходів при натисканні на фігуру +
+ Таймер для кожного гравця
+ Статистика зіграних ігор +

# Опис програми
### Цей додаток є повноцінною шаховою грою з графічним інтерфейсом, яка підтримує:
+ Класичні шахові правила (включаючи спеціальні ходи як рокірування)
+ Систему збереження/завантаження гри
+ Візуальні підказки для можливих ходів
+ Вибір теми та стилю фігур
+ Вікно статистики перемог
+ Два режими відображення (повноекранний та віконний)

## Запуск програми
1. Клонуйте репозиторій
2. Відкрийте рішення у Visual Studio
3. Зберіть рішення
4. Запустіть проект

# Programming Principles
1. SOLID Principles
    1.1 Single Responsibility Principle (SRP): Кожен клас відповідає за одну конкретну функціональність 
        + GameManager - керує логікою гри, оброблює стани.
        + Board - менеджить поведінку та розташування фігур на дошці. Дошка представляє собою двовимірний масив.
        + WindowManager - керує логікою перемикань між вікнами.
        + [SettingsJsonOperator](https://github.com/Shadocl-low/ChessGame/blob/test-version/ChessGameApplication/SettingsJsonOperator.cs) - оперує лише файлом, що зберігає поточні налаштування користувача, зберігає та отримує з нього дані.
    1.2 Liskov Substitution Principle (LSP): Похідні класи фігур можуть заміняти базовий клас [Piece](https://github.com/Shadocl-low/ChessGame/blob/a0d37fee2435d01a82c0a52f5cdefe46609d7f8f/ChessGameApplication/Game/Figures/Piece.cs) та [IPieceImageStrategy](https://github.com/Shadocl-low/ChessGame/blob/test-version/ChessGameApplication/Game/PieceImageStrategies/IPieceImageStrategy.cs)
        + [Piece](https://github.com/Shadocl-low/ChessGame/blob/a0d37fee2435d01a82c0a52f5cdefe46609d7f8f/ChessGameApplication/Game/Figures/Piece.cs) та наприклад [Queen](https://github.com/Shadocl-low/ChessGame/blob/a0d37fee2435d01a82c0a52f5cdefe46609d7f8f/ChessGameApplication/Game/Figures/Queen.cs).
        + [IWindowManager](https://github.com/Shadocl-low/ChessGame/blob/a0d37fee2435d01a82c0a52f5cdefe46609d7f8f/ChessGameApplication/Windows/Manager/IWindowManager.cs) та [WindowManager](https://github.com/Shadocl-low/ChessGame/blob/a0d37fee2435d01a82c0a52f5cdefe46609d7f8f/ChessGameApplication/Windows/Manager/WindowManager.cs)
    1.3 Interface Segregation Principle (ISP): Інтерфейси розділені за функціональністю
        + [IChangeWindowMode](https://github.com/Shadocl-low/ChessGame/blob/a0d37fee2435d01a82c0a52f5cdefe46609d7f8f/ChessGameApplication/Windows/IChangeWindowMode.cs) - для об'єктів, що будуть змінювати стан вікна.
+ [IWindowManager](https://github.com/Shadocl-low/ChessGame/blob/a0d37fee2435d01a82c0a52f5cdefe46609d7f8f/ChessGameApplication/Windows/Manager/IWindowManager.cs) - містить метод Notify, що буде сповіщати про виклик певним об'єктом певної дії.
2. DRY (Don't Repeat Yourself)
+ Повторюваний код винесено у методи
+ Дії у методі [MovePiece](https://github.com/Shadocl-low/ChessGame/blob/a0d37fee2435d01a82c0a52f5cdefe46609d7f8f/ChessGameApplication/Game/Board.cs#L86-L101) часто використовувались, тому були винесені у метод.
+ [GetAllowedMoves](https://github.com/Shadocl-low/ChessGame/blob/a0d37fee2435d01a82c0a52f5cdefe46609d7f8f/ChessGameApplication/Game/GameManager.cs#L159-L176) - дії у методі використовуються для кожного ходу, тому були винесені у метод.
+ [ApplyTheme](https://github.com/Shadocl-low/ChessGame/blob/a0d37fee2435d01a82c0a52f5cdefe46609d7f8f/ChessGameApplication/Windows/SettingsWindow.xaml.cs;#L43-L67) використовує switch лише один раз, і задає Action, замість того щоб двома switch окремо задавати необхідні параметри.
3. KISS (Keep It Simple, Stupid)
+ Проста архітектура без зайвої складності
+ Для збереження стану гри, налаштувань або статистики використовуються [прості моделі](https://github.com/Shadocl-low/ChessGame/tree/a0d37fee2435d01a82c0a52f5cdefe46609d7f8f/ChessGameApplication/JsonModels), лише з необхідними параметрами.
+ Керуванням збереження та завантаження налаштувань з файлу займається [один клас](https://github.com/Shadocl-low/ChessGame/blob/a0d37fee2435d01a82c0a52f5cdefe46609d7f8f/ChessGameApplication/GameJsonOperator.cs), що парсить спеціально створену model. Для інших налаштувань використовуються інші класи, щоб розділити їх обов'язки та сфери використання.
+ Для простішої роботи з координатами створений був клас [Position](https://github.com/Shadocl-low/ChessGame/blob/a0d37fee2435d01a82c0a52f5cdefe46609d7f8f/ChessGameApplication/Game/Position.cs), що зберігає спрощений вигляд координат та забезпечує просту роботу з ними.
4. YAGNI (You Aren't Gonna Need It)
+ Реалізовано лише необхідний функціонал без "на випадок". Всі методи та класи у проекті використовуються.
5. Law of Demeter
+ Мінімальна залежність між класами
+ [Piece](https://github.com/Shadocl-low/ChessGame/blob/a0d37fee2435d01a82c0a52f5cdefe46609d7f8f/ChessGameApplication/Game/Figures/Piece.cs) класи не знають про [GameWindow](https://github.com/Shadocl-low/ChessGame/blob/a0d37fee2435d01a82c0a52f5cdefe46609d7f8f/ChessGameApplication/Windows/GameWindow.xaml.cs)
+ [Board](https://github.com/Shadocl-low/ChessGame/blob/a0d37fee2435d01a82c0a52f5cdefe46609d7f8f/ChessGameApplication/Game/Board.cs) не знає про [GameManager](https://github.com/Shadocl-low/ChessGame/blob/a0d37fee2435d01a82c0a52f5cdefe46609d7f8f/ChessGameApplication/Game/GameManager.cs).

# Design Patterns
1. Singleton
+ Використано для [GameJsonOperator](https://github.com/Shadocl-low/ChessGame/blob/a0d37fee2435d01a82c0a52f5cdefe46609d7f8f/ChessGameApplication/GameJsonOperator.cs), [SettingsJsonOperator](https://github.com/Shadocl-low/ChessGame/blob/a0d37fee2435d01a82c0a52f5cdefe46609d7f8f/ChessGameApplication/SettingsJsonOperator.cs) та [StatsJsonOperator](https://github.com/Shadocl-low/ChessGame/blob/a0d37fee2435d01a82c0a52f5cdefe46609d7f8f/ChessGameApplication/StatsJsonOperator.cs) - класи, які мають бути в єдиному екземплярі. Доступні з будь якої точки програми та дають доступ до одного і того ж об'єкту(файлу), тому однакові параметри з файлу можуть бути застосовані до різних об'єктів.
+ Реалізовано через статичну властивість Instance з ленивою ініціалізацією
2. Strategy
+ Паттерн застосовано для різних стилів відображення фігур, де [IPieceImageStrategy](https://github.com/Shadocl-low/ChessGame/blob/a0d37fee2435d01a82c0a52f5cdefe46609d7f8f/ChessGameApplication/Game/PieceImageStrategies/CutePieceImageStrategy.cs) - загальний інтерфейс для стратегії вибору зображення, а [ClassicPieceImageStrategy](https://github.com/Shadocl-low/ChessGame/blob/a0d37fee2435d01a82c0a52f5cdefe46609d7f8f/ChessGameApplication/Game/PieceImageStrategies/ClassicPieceImageStrategy.cs) та [CutePieceImageStrategy](https://github.com/Shadocl-low/ChessGame/blob/a0d37fee2435d01a82c0a52f5cdefe46609d7f8f/ChessGameApplication/Game/PieceImageStrategies/CutePieceImageStrategy.cs) - конкретні реалізації стратегій.
+ Дозволяє легко додавати нові стилі без змін коду гри
+ Конкретні стратегії передаються у необхідний Piece, де викликається [метод стратегії](https://github.com/Shadocl-low/ChessGame/blob/a0d37fee2435d01a82c0a52f5cdefe46609d7f8f/ChessGameApplication/Game/Figures/Piece.cs#L32-L35), у якому реалізований алгоритм вибору необхідного зображення. Стратегія може бути змінена під час роботи додатку.
3. Observer
+ Використано у [SettingsJsonOperator](https://github.com/Shadocl-low/ChessGame/blob/a0d37fee2435d01a82c0a52f5cdefe46609d7f8f/ChessGameApplication/SettingsJsonOperator.cs) для сповіщення про події
+ Реалізовано через event-и у C#
+ У SettingsJsonOperator [оголошуються події](https://github.com/Shadocl-low/ChessGame/blob/a0d37fee2435d01a82c0a52f5cdefe46609d7f8f/ChessGameApplication/SettingsJsonOperator.cs#L18-L19), на які можуть [підписуватись інші об'єкти](https://github.com/Shadocl-low/ChessGame/blob/a0d37fee2435d01a82c0a52f5cdefe46609d7f8f/ChessGameApplication/Windows/GameWindow.xaml.cs#L35-L36), наприклад GameWindow.
+ При зміні у налаштуваннях параметру, [викликається подія](https://github.com/Shadocl-low/ChessGame/blob/a0d37fee2435d01a82c0a52f5cdefe46609d7f8f/ChessGameApplication/SettingsJsonOperator.cs#L57), і підписані на подію об'єкти [викликають свої методи](https://github.com/Shadocl-low/ChessGame/blob/a0d37fee2435d01a82c0a52f5cdefe46609d7f8f/ChessGameApplication/Windows/GameWindow.xaml.cs#L193-L212).
4. Mediator
+ Дозволяє не зв'язувати напряму низку об'єктів, а передавати обробку їх взаємодій іншому класу, такому як [WindowManager](https://github.com/Shadocl-low/ChessGame/blob/a0d37fee2435d01a82c0a52f5cdefe46609d7f8f/ChessGameApplication/Windows/Manager/WindowManager.cs).
+ Об'єкти, які будуть пов'язані через WindowManager, [будуть містити його інтерфейс](https://github.com/Shadocl-low/ChessGame/blob/a0d37fee2435d01a82c0a52f5cdefe46609d7f8f/ChessGameApplication/Windows/MainMenuWindow.xaml.cs#L22-L27), та викликати його [метод](https://github.com/Shadocl-low/ChessGame/blob/a0d37fee2435d01a82c0a52f5cdefe46609d7f8f/ChessGameApplication/Windows/MainMenuWindow.xaml.cs#L31-L56), який буде сповіщувати mediator про дію, [яку необхідно виконати](https://github.com/Shadocl-low/ChessGame/blob/a0d37fee2435d01a82c0a52f5cdefe46609d7f8f/ChessGameApplication/Windows/Manager/WindowManager.cs#L29-L57).

# Refactoring Techniques
1. Extract Method
+ Винесення повторюваного коду в окремі методи 
+ Винесення [анімації](https://github.com/Shadocl-low/ChessGame/blob/a0d37fee2435d01a82c0a52f5cdefe46609d7f8f/ChessGameApplication/Windows/GameWindow.xaml.cs#L238-L261) у GameWindow.
2. Replace Magic Numbers with Constants
+ Заміна "магічних чисел" на константи 
+ Сталі [змінні для клітинок](https://github.com/Shadocl-low/ChessGame/blob/a0d37fee2435d01a82c0a52f5cdefe46609d7f8f/ChessGameApplication/Windows/GameWindow.xaml.cs#L) у GameWindow.
3. Introduce Parameter Object
+ Використання [Position](https://github.com/Shadocl-low/ChessGame/blob/a0d37fee2435d01a82c0a52f5cdefe46609d7f8f/ChessGameApplication/Game/GameManager.cs#L96-L122) замість окремих параметрів row/column
4. Replace Conditional with Polymorphism
+ Обробка різних типів фігур через поліморфізм 
+ [Piece та похідні класи](https://github.com/Shadocl-low/ChessGame/blob/a0d37fee2435d01a82c0a52f5cdefe46609d7f8f/ChessGameApplication/Game/GameManager.cs#L146-L154)
5. Decompose Conditional
+ Розбиття складних умов на методи
+ Перевірка рокірування у [King](https://github.com/Shadocl-low/ChessGame/blob/a0d37fee2435d01a82c0a52f5cdefe46609d7f8f/ChessGameApplication/Game/Figures/King.cs#L38-L75).