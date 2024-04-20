<template>
  <div>
    <h1>Поиск ВУЗа</h1>
    <iframe
      width="100%"
      height="600"
      :src="panoramaUrl"
      frameborder="0"
      allowfullscreen
    ></iframe>
    <Test />
  </div>
</template>

<script>
import Test from "@/components/Test.vue";
export default {
  components: {
    Test,
  },
  data() {
    return {
      panoramaUrl: "@/src/pro/3D tour.html",
    };
  },
  props: {},
  methods: {},
};
</script>

<style lang="scss" scoped>
h1 {
  margin-left: 1vw;
  margin-bottom: 3vw;
  font-size: 5vw;
}
</style>
