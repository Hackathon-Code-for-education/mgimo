# Проект "Университеты России"

## Описание проекта
Проект хакатона "Код образования" от команды ODIN из МГИМО МИД РОССИИ.

Проект fullstack написан на ASP.NET MVC Web API c реляционной базой данных MySQL.

ВНИМАНИЕ: Основной проект находится в папке fullstack
К открытию предпочтительна pptx версия презентации

## Сборка Backend сервера

ВНИМАНИЕ: Сервер в данный момент запущен на домене:

   ```
   https://api.mgimoapp.ru/
   ```
Туда можно отправлять запросы для тестирования

ВНИМАНИЕ: Сервер использует базу данных MySQL с пользовательскими настройсками:

- UserName => api
- Password => JLgHe5Vs
- Database => uni


Для сборки проекта необходимо выполнить следующие шаги:

1. Клонировать репозиторий с проектом с помощью команды:
   ```
   git clone https://github.com/Hackathon-Code-for-education/mgimo.git
   ```

Для запуска необходимо установить Dotnet SDK 7-ой версии. Вы этом можете сделать по данноый ссылке:
https://dotnet.microsoft.com/en-us/download/dotnet/7.0


2. Установить DotNet SDK , выполнив команду:
   ```
   sudo apt-get update && \
   sudo apt-get install -y dotnet-sdk-7.0
   ```


3. Запустить сервер, введя команду, находясь в папке проекта:
   ```
   cd ./fullstack/UniWeb/UniWeb
   dotnet run
   ```

4. После этого проект будет доступен по адресу `http://localhost:5051`.

## Сборка Telegram Bot для анонимного чата студентов и абитуриентов

ВНИМАНИЕ: Бот сейчас запущен и доступен по ссылке:

   ```
   https://t.me/dumb_pashas_bot
   ```
Для сборки проекта необходимо выполнить следующие шаги:

1. Клонировать репозиторий с проектом с помощью команды:
   ```
   git clone https://github.com/Hackathon-Code-for-education/mgimo.git
   ```
2. Установите python и pip
```
   2.1. Перейдите на официальный сайт Python по ссылке: https://www.python.org/downloads/
   2.2. Скачайте последнюю версию Python для вашей операционной системы (Windows, macOS, Linux) и запустите установщик.
   2.3. Убедитесь, что при установке выбран пункт "Add Python to PATH", чтобы Python был доступен из командной строки.
   2.4. Пройдите процесс установки, следуя инструкциям установщика.

   После установки Python, убедитесь, что инструмент управления пакетами pip установлен. В новых версиях Python pip устанавливается автоматически. Однако, если pip не установлен, выполните следующие действия:

   1. Откройте командную строку (на Windows) или терминал (на macOS и Linux).
   2. Установите pip, введя следующую команду:

   python -m ensurepip
```
3. Перейти к директории бота, выполнив команду из папки проекта:
   ```
   cd ./bot
   ```
4. Выполните установку необходимых библиотек
   ```
   pip install telebot
   pip install sqlite3
   pip install random
   ```
5. Добавьте свой токен
    ```
   Для работы с ботом вам понадобится токен, который можно получить, зарегистрировав бота через официального бота BotFather в Telegram.
   Замените 'YOUR_TOKEN' на токен вашего бота
    ```
6. Запустить бота, введя команду:
   ```
   python main.py
   ```
