<template>
  <div id="app">
    <OffCanvas />
    <TopNav />
    <router-view/>
  </div>
</template>

<script>
import OffCanvas from './components/Menu';
import TopNav from './components/Navbar';

export default {
    components: {
      OffCanvas, TopNav
    }
}
</script>

<style>
#works {
  background: rgba(185, 180, 190, 0.8)!important;
}

#application {
  min-height: 80vh;
}

body {
  min-height: 100vh;
  max-width: 100vw;
  overflow-x: hidden;
}

#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
}

#nav {
  padding: 30px;
}

#nav a {
  font-weight: bold;
  color: #2c3e50;
}

#nav a.router-link-exact-active {
  color: #42b983;
}
</style>
