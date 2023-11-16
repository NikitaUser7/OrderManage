import Vue from 'vue'
import Vuex from 'vuex'

Vue.use(Vuex)

export default new Vuex.Store({
  state: {
    user: null,
    sortCheck: false
  },
  mutations: {
    changeUser (state, u) {
      state.user = u
    },
    changeCheck (state, u) {
      state.sortCheck = u
    }
  }
})
