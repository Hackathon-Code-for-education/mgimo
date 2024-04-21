import telebot
import sqlite3
import random
from telebot import types

# Укажите токен вашего бота
TOKEN = 'YOUR_TOKEN'

# Создание базы данных SQLite и таблиц в ней
conn = sqlite3.connect('universities.db')
cursor = conn.cursor()
cursor.execute('''
    CREATE TABLE IF NOT EXISTS students (
        user_id INTEGER PRIMARY KEY,
        status TEXT,
        university TEXT
    )
''')
cursor.execute('''
    CREATE TABLE IF NOT EXISTS abiturients (
        user_id INTEGER PRIMARY KEY,
        university TEXT
    )
''')
cursor.execute('''
    CREATE TABLE IF NOT EXISTS pairs (
        abitur_id INTEGER PRIMARY KEY,
        student_id INTEGER
    )
''')
conn.commit()

keyboard = types.ReplyKeyboardMarkup(resize_keyboard=True, one_time_keyboard=True)
keyboard.add(types.KeyboardButton('МГУ'), types.KeyboardButton('СПбГУ'))
keyboard.add(types.KeyboardButton('ВШЭ'), types.KeyboardButton('МГИМО'))

user_status = {}

status_keyboard = types.ReplyKeyboardMarkup(resize_keyboard=True, one_time_keyboard=True)
status_keyboard.add(types.KeyboardButton('Студент'), types.KeyboardButton('Не студент'))

# Создаем экземпляр бота
bot = telebot.TeleBot(TOKEN)

# Обработчик команды /start
@bot.message_handler(commands=['start'])
def handle_start(message): 
    user_id = message.from_user.id
    bot.send_message(user_id, "Выберите ваш статус:", reply_markup=status_keyboard)

@bot.message_handler(commands=['exit'])
def handle_exit(message):
    conn = sqlite3.connect('universities.db')
    cursor = conn.cursor()
    user_id = message.from_user.id
    try:
        cursor.execute(f"SELECT * FROM pairs WHERE abitur_id = {user_id}")
        rows = cursor.fetchall()

        if rows == []:
            cursor.execute(f"SELECT * FROM pairs WHERE student_id = {user_id}")
            rows = cursor.fetchall()

            if rows != []:
                bot.send_message(rows[0][0], "Ваш собеседник разорвал пару. Вы можете создать новый диалог со студентом. Удалите данные через /del")
        else:
            bot.send_message(rows[0][1], "Ваш собеседник разорвал пару. Вам напишет другой абитуриент")

    except sqlite3.Error as e:
        print(e)
    
    # Удаление пары
    try:
        cursor.execute(f'''
            DELETE FROM pairs
            WHERE abitur_id='{user_id}' OR student_id='{user_id}';
        ''')
        conn.commit()

        bot.send_message(user_id, f"Вы закрыли диалог. Вы можете создать новый диалог со студентом. Удалите данные через /del")
    except sqlite3.Error as e:
        print(e)


@bot.message_handler(commands=['del'])
def handle_delete(message):
    conn = sqlite3.connect('universities.db')
    cursor = conn.cursor()
    user_id = message.from_user.id
    try:
        cursor.execute(f'''
            DELETE FROM abiturients
            WHERE user_id='{user_id}';
        ''')
        conn.commit()
        # bot.send_message(user_id, f"Ваши данные были удалены, можно заново зарегестрироваться")
    except sqlite3.Error as e:
        print(e)

    try:
        cursor.execute(f'''
            DELETE FROM students
            WHERE user_id='{user_id}';
        ''')
        conn.commit()
        # bot.send_message(user_id, f"Ваши данные были удалены, можно заново зарегестрироваться")
    except sqlite3.Error as e:
        print(e)

    # Уведомление о разрыве
    try:
        cursor.execute(f"SELECT * FROM pairs WHERE abitur_id = {user_id}")
        rows = cursor.fetchall()

        if rows == []:
            cursor.execute(f"SELECT * FROM pairs WHERE student_id = {user_id}")
            rows = cursor.fetchall()

            if rows != []:
                bot.send_message(rows[0][0], "Ваш собеседник удален. Вы можете создать новый диалог со студентом. Удалите данные через /del")
        else:
            bot.send_message(rows[0][1], "Ваш собеседник удален. Вам напишет другой абитуриент")

    except sqlite3.Error as e:
        print(e)
    
    # Удалиение пары
    try:
        cursor.execute(f'''
            DELETE FROM pairs
            WHERE abitur_id='{user_id}' OR student_id='{user_id}';
        ''')
        conn.commit()

        bot.send_message(user_id, f"Ваши данные были удалены, можно заново создать диалог. /start")
    except sqlite3.Error as e:
        print(e)

@bot.message_handler(func=lambda message: message.text in ['Студент', 'Не студент'])
def handle_student_status(message):
    user_id = message.from_user.id
    global status
    status = message.text
    user_id = message.from_user.id
    bot.send_message(user_id, "Выберите ваш ВУЗ из списка:", reply_markup=keyboard)
    
@bot.message_handler(func=lambda message: message.text in ['МГУ', 'СПбГУ', 'ВШЭ', 'МГИМО'])
def handle_university_choice(message):
    user_id = message.from_user.id
    university = message.text
    global status

    conn = sqlite3.connect('universities.db')
    cursor = conn.cursor()

    if status=="Студент":
        try:
            cursor.execute('''
                INSERT INTO students (user_id, status, university)
                VALUES (?, ?, ?)
            ''', (user_id, status, university))
            conn.commit()
            bot.send_message(user_id, f"Вы выбрали {university}. Информация сохранена в базе данных. Ожидайте сообщения от абитуриента")
        except sqlite3.Error as e:
            bot.send_message(user_id, "Вы уже находитесь в базе данных. Удалите информацию через /del")
    else:
        try:
            cursor.execute('''
                INSERT INTO abiturients (user_id, university)
                VALUES (?, ?)
            ''', (user_id, university))
            conn.commit()
            bot.send_message(user_id, f"Вы выбрали {university}. Информация сохранена в базе данных. Можете спросить анонимного студента")
        except sqlite3.Error as e:
            bot.send_message(user_id, "Вы уже находитесь в базе данных. Удалите информацию через /del")
        
        cursor.execute(f"SELECT user_id FROM students WHERE university = '{university}'")
        rows = cursor.fetchall()
        if rows:
            random_student_id = random.choice(rows)[0]
            # print(f"ID рандомного студента из МГИМО: {random_student_id}")
            global random_chatter
            random_chatter = random_student_id
            try:
                cursor.execute('''
                    INSERT INTO pairs (abitur_id, student_id)
                    VALUES (?, ?)
                ''', (user_id, random_chatter))
                conn.commit()
            except sqlite3.Error as e:
                print("нельзя создать новую пару")
        else:
            print("Нет студентов этого вуза в базе данных")
    conn.close()

# Обработчик текстовых сообщений
@bot.message_handler(func=lambda message: True)
def handle_text(message):
    global random_chatter
    user_id = message.from_user.id
    conn = sqlite3.connect('universities.db')
    cursor = conn.cursor()
    cursor.execute(f"SELECT * FROM pairs WHERE abitur_id = {user_id}")
    rows = cursor.fetchall()

    try:
        cursor.execute(f"SELECT * FROM students WHERE user_id = {user_id}")
        uni = cursor.fetchall()
        if uni == []:
            cursor.execute(f"SELECT * FROM abiturients WHERE user_id = {user_id}")
            uni = cursor.fetchall()
    except sqlite3.Error as e:
        print(e)


    if rows != []:
        for chat_id in rows:
            print(chat_id[1])
            chat_to = chat_id[1]
        bot.send_message(chat_to, f"Вы рандомный студент из {uni[0][1]}. Ваш собеседник говорит: {message.text}")
    else:
        cursor.execute(f"SELECT * FROM pairs WHERE student_id = {user_id}")
        rows = cursor.fetchall()
        if rows != []:
            for chat_id in rows:
                print(chat_id[0])
                chat_to = chat_id[0]
            bot.send_message(chat_to, f"Случайный студент {uni[0][2]} говорит: {message.text}")

# Запуск бота
bot.polling(none_stop=True)