<script setup>
// import { ref } from 'vue';
/*
const formdata = new FormData();
formdata.append("Username", "s");
formdata.append("Name", "d");

const requestOptions = {
  method: "POST",
  body: formdata,
  redirect: "follow"
};

fetch("https://api.mgimoapp.ru/home", requestOptions)
  .then((response) => response.text())
  .then((result) => console.log(result))
  .catch((error) => console.error(error));
const formdata = new FormData();

const requestOptions = {
  method: "GET",
  redirect: "follow"
};

fetch("https://api.mgimoapp.ru/home", requestOptions)
  .then((response) => response.text())
  .then((result) => this.product = result)
  .catch((error) => console.error(error));
*/

import Test from "@/components/Test.vue";
</script>

<template>
  <header>
    
  </header>

  <main>
    <div class="wrapper">
      <Test />
    </div>
  </main>
</template>

<style scoped>
</style>
