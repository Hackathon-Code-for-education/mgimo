<template>
    <div>
      <button @click="getData">Загрузить данные</button>
      <p v-if="loaded">Данные: {{ fetchedData }}</p>
      <p v-if="error">Произошла ошибка при загрузке данных.</p>
      <button @click="submitData">Отправить данные</button>
      <p>{{ postData }}</p>
    </div>
  </template>
  
  <script>
  export default {
    data() {
      return {
        fetchedData: null,
        loaded: false,
        error: false,
        postData: null
      };
    },
    methods: {
      async getData() {
        try {
          const response = await fetch('https://api.mgimoapp.ru/home');
          if (!response.ok) {
            throw new Error('Ошибка при загрузке данных');
          }
          this.fetchedData = await response.json();
          console.log(this.fetchedData);
          this.loaded = true;
        } catch (error) {
          console.error(error);
          this.error = true;
        }
      },

      async submitData() {
      try {
        const data = { Username: 'd' , Name: 'f'};
        const response = await fetch('https://api.mgimoapp.ru/home', {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json'
          },
          redirect: "follow",
          body: JSON.stringify(data)
        });

        if (!response.ok) {
          throw new Error('Ошибка при отправке данных');
        }
        this.postData = await response.json();
        this.submitted = true;
      } catch (error) {
        console.error(error);
        this.submitError = true;
      }
    }
    }
  };
  </script>