<template>
  <q-page>
    <BasicOrderList :ordOps="ordOps" @OrderClick="openCurrentOrder" v-if="!show" />
    <CurrentOrder v-if="show" :opnOrder ="orderOpn" @closeCurrentOrder="closeCurrentOrderM" @deleteOrder="emitDeleteCurrentOrder"/>
  </q-page>
</template>
<script>
import OrdOps from '../components/OrderOperations.js'
import BasicOrderList from '../components/BasicOrderList.vue'
import CurrentOrder from '../components/CurrentOrder.vue'

export default {
  components: {
    BasicOrderList,
    CurrentOrder
  },
  data () {
    return {
      ordOps: new OrdOps(),
      orderOpn: {},
      show: false,
      bigTaskCardShow: false
    }
  },
  async mounted () {
    this.$nextTick(async () => {})
  },
  methods: {
    openCurrentOrder (order) {
      this.show = true
      this.orderOpn = order
    },
    closeCurrentOrderM () {
      this.show = false
    },
    emitDeleteCurrentOrder () {
      this.show = false
    }
  },
  watch: {
  }
}
</script>
<style lang="sass">
.thead-custom-sticky tr > *,
.tfoot-custom-sticky tr > *
  position: sticky
  opacity: 1
  z-index: 1
  background-color: black
  color: white

.thead-custom-sticky tr:last-child > *
  top: 0

.tfoot-custom-sticky tr:first-child > *
  bottom: 0
</style>
