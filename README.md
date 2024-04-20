# Проект "Университеты России"

## Описание проекта
Проект хакатона "Код образования" от команды ODIN из МГИМО МИД РОССИИ.

Проект fullstack написан на ASP.NET MVC Web API c реляционной базой данных MySQL.


## Сборка Backend сервера

ВНИМАНИЕ: Сервер Backend в данный момент запущен на домене:

   ```
   https://api.mgimoapp.ru/swagger/index.html
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

2. Установить DotNet SDK , выполнив команду:
   ```
   sudo apt-get update && \
   sudo apt-get install -y dotnet-sdk-7.0
   ```


3. Запустить сервер, введя команду:
   ```
   cd ./fullstack/UniWeb/UniWeb
   dotnet run
   ```

4. После этого проект будет доступен по адресу `http://localhost:5000`.
