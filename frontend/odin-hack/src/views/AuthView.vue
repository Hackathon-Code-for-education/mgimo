<template>
  <div>
    <h1>Авторизация</h1>
    <div class="auth-wrapper">
      <form @submit.prevent="submitForm">
        <div class="slider">
          <a href="#" @click.prevent="changeForStd">Для студентов</a>
          <a href="#" @click.prevent="changeForUni">Для ВУЗа</a>
        </div>
        <input
          type="text"
          id="username"
          placeholder="Логин"
          v-model="login"
          required
        />

        <input
          type="email"
          id="email"
          placeholder="Email"
          v-model="email"
          required
        />

        <input
          type="password"
          id="password"
          placeholder="Пароль"
          v-model="password"
          required
        />

        <button type="submit">Войти</button>
      </form>
    </div>
  </div>
</template>

<script>
import router from "/src/router/index.js";

export default {
  data() {
    return {
      isAuthenticated: false,
      login: "",
      email: "",
      password: "",
      tokenJWT: "",
    };
  },
  methods: {
    changeForStd() {
      console.log("std");
    },
    changeForUni() {
      console.log("uni");
    },
    async submitForm() {
      const formdata = new FormData();
      formdata.append("Login", this.login);
      formdata.append("Password", this.password);

      fetch("https://api.mgimoapp.ru/applicant/login", {
        method: "POST",
        body: formdata,
        redirect: "follow",
      })
        .then((response) => response.json())
        .then((result) => {
          const token = result.token;
          sessionStorage.setItem("token", token);
          console.log(sessionStorage.getItem("token"));
        })
        .catch((error) => {
          console.error(error);
          //   this.isAuthenticated = false;
        });

      const myHeaders = new Headers();
      myHeaders.append(
        "Authorization",
        "Bearer " + sessionStorage.getItem("token")
      );

      fetch("https://api.mgimoapp.ru/applicant/me", {
        method: "GET",
        headers: myHeaders,
        redirect: "follow",
      })
        .then((response) => response.text())
        .then((result) => {
          const user = result.user;
          console.log(result);
        })
        .catch((error) => console.error(error));
      sessionStorage.clear();
    },
  },
};
</script>

<style lang="scss" scoped>
.slider {
  width: 80%;
  display: flex;
  justify-content: space-between;
}

h1 {
  margin-left: 1vw;
  margin-bottom: 3vw;
  font-size: 5vw;
}
.auth-wrapper {
  display: flex;
  justify-content: center; /* Выравнивание по горизонтали */
  align-items: center;
}

form {
  width: 50%;
  display: flex;
  flex-direction: column;
  gap: 1vw;
  align-items: center;

  input {
    width: 96%;
    background-color: #f4f4f4;
    border: none;
    padding: 1vw;
    font-size: 1.5vw;
    border-radius: 2vw;
  }
}

button {
  margin-top: 1.5vw;
  font-family: "Fira sans", sans-serif;
  font-size: 1.3vw;
  background-color: #6558ff;
  color: white;
  border-radius: 2vw;
  padding: 1vw;
  cursor: pointer;
  text-transform: uppercase;
  width: 100%;
}
</style>
