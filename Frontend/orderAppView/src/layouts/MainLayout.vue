<template>
  <q-layout view="hHh lpR fFf">
    <q-header class="bg-primary text-white" height-hint="98" elevated >
      <q-toolbar>
        <q-btn dense flat round icon="menu" @click="left = !left" />

        <q-toolbar-title class="q-pb-sm">
          <q-avatar
            color="white"
            size="100px"
            text-color="purple-9"
            class="q-pb-lg"
          > <div class="text-h1">
            🛏
          </div>
          </q-avatar>

          <strong class="q-pl-md text-h4" style="color: white">OrderNote</strong>
        </q-toolbar-title>
      </q-toolbar>
    </q-header>
    <q-drawer
      side="left"
      behavior="desktop"
      v-model="left"
      :width="200"
      :breakpoint="500"
      bordered
      content-class="bg-grey-3"
    >
      <q-scroll-area class="fit">
        <q-list>
          <template v-for="(menuItem, index) in menuList">
            <q-item
              :key="index"
              clickable
              v-ripple
              @click="open(menuItem.pach)"
            >
              <q-item-section avatar>
                <q-icon :name="menuItem.icon" />
              </q-item-section>
              <q-item-section>
                {{ menuItem.label }}
              </q-item-section>
            </q-item>
            <q-separator :key="'sep' + index" v-if="menuItem.separator" />
          </template>
        </q-list>
      </q-scroll-area>
    </q-drawer>

    <q-page-container>
      <router-view />
    </q-page-container>
  </q-layout>
</template>

<script>
const menuList = [
  {
    icon: 'subject',
    label: 'Заказы',
    separator: false,
    pach: '/orders'
  }
]
export default {
  data () {
    return {
      drawer: false,
      menuList,
      left: true
    }
  },
  async mounted () {
    this.$nextTick(async () => {
      this.$router.push('/orders')
    })
  },
  methods: {
    open (pach) {
      this.$router.push(pach)
    }
  }
}
</script>
