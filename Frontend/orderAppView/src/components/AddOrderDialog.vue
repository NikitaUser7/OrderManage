<template>
  <q-layout
    container
    class="bg-white"
    style="width: 1500px; max-width: 150vw; max-height: 700px;"
  >
    <q-page class="q-pa-lg">
      <div class="text-h6 q-pt-md" style="color:darkblue;">
        OrderNote
      </div>
      <div class="text-h5 q-pt-sm" style="color:blak;">
        {{ !editOrSave ? "Добавить заказ" : "Изменить заказ" }}
      </div>
      <q-form
        @reset="reset()"
        @submit="editOrSave ? editOrder() : saveOrder()"
        style="min-width:800px;"
      >
        <div class="row">
          <q-form class="col-4 q-pt-md">
            <div class="row">
              <div class="col q-pr-md">
                <q-input
                  v-model="orderNumber"
                  class="q-pt-md"
                  filled
                  label="Номер заказа"
                  lazy-rules
                  :rules="[val => (val && val.length > 0) || 'Заполните поле']"
                >
                  <template v-slot:append>
                    <q-icon
                      v-if="orderNumber !== ''"
                      name="close"
                      @click="orderNumber = ''"
                      class="cursor-pointer"
                    />
                  </template>
                </q-input>
              </div>
              <div class="col">
                <q-input
                  v-model="customSurname"
                  class="q-pt-md"
                  filled
                  label="Фамилия"
                  lazy-rules
                  :rules="[val => (val && val.length > 0) || 'Заполните поле']"
                >
                  <template v-slot:append>
                    <q-icon
                      v-if="customSurname !== ''"
                      name="close"
                      @click="customSurname = ''"
                      class="cursor-pointer"
                    />
                  </template>
                </q-input>
              </div>
            </div>
            <div class="row">
              <div class="col q-pr-md">
                <q-input
                  v-model="customName"
                  class="q-pt-md"
                  filled
                  label="Имя"
                  lazy-rules
                  :rules="[val => (val && val.length > 0) || 'Заполните поле']"
                >
                  <template v-slot:append>
                    <q-icon
                      v-if="customName !== ''"
                      name="close"
                      @click="customName = ''"
                      class="cursor-pointer"
                    />
                  </template>
                </q-input>
              </div>
              <div class="col">
                <q-input
                  v-model="customPatronymic"
                  class="q-pt-md"
                  filled
                  label="Отчество"
                  lazy-rules
                  :rules="[val => (val && val.length > 0) || 'Заполните поле']"
                >
                  <template v-slot:append>
                    <q-icon
                      v-if="customPatronymic !== ''"
                      name="close"
                      @click="customPatronymic = ''"
                      class="cursor-pointer"
                    />
                  </template>
                </q-input>
              </div>
            </div>
            <q-input
              readonly
              v-model="sumMoney"
              class="q-pt-md"
              filled
              label="Сумма"
              prefix="$"
              lazy-rules
              :rules="[val => (val && val > 0) || 'Заполните поле']"
            >
            </q-input>
            <q-item-label style="color: grey" class="q-pt-md"
              >Дата создания</q-item-label
            >
            <q-input
              v-model="creationDate"
              class="q-pt-xs"
              type="date"
              filled
              lazy-rules
              :rules="[val => (val && val.length > 0) || 'Заполните поле']"
            >
              <template v-slot:append>
                <q-icon
                  v-if="creationDate !== ''"
                  name="close"
                  @click="creationDate = ''"
                  class="cursor-pointer"
                />
              </template>
            </q-input>
          </q-form>
          <q-form class="col-8 q-pl-lg" @submit="addPosition">
            <div class="row wrap" style="min-width:500px">
              <q-toolbar class="col-xs-3 col-sm-5 col-md-6">
                <div>
                  <strong
                    style="font-color: black; color:black"
                    class="text-h6 q-pl-lg"
                  >Позиции заказа</strong
                  >
                </div>
              </q-toolbar>
              <q-toolbar class="col-xs-3 col-sm-4 col-md-6 bg-white text-black">
                <q-btn
                  stretch
                  flat
                  :label="editOrsave === false? 'Добавить позицию' : 'Сохранить'"
                  type="submit"
                />
                <q-separator dark vertical />
                <q-btn
                  stretch
                  flat
                  label="Изменить"
                  @click="editPosition"
                  :disable="this.selected.length === 1 ? false : true"
                  :style="
                    this.selected.length === 1
                      ? 'color: blue; disable'
                      : 'color: grey'
                  "
                />
                <q-separator dark vertical />
                <q-btn
                  stretch
                  flat
                  label="Удалить"
                  @click="deletePosition"
                  :disable="this.selected.length === 0 ? true : false"
                  :style="
                    this.selected.length === 0
                      ? 'color: grey; disable'
                      : 'color: blue'
                  "
                />
                <q-btn
                  stretch
                  flat
                  label="Очистить выбор"
                  v-if="this.selected.length !== 0"
                  @click="clearSelected"
                />
              </q-toolbar>
            </div>
            <q-form>
              <div class="row" style="color:red">
                  {{warn}}
                </div>
              <div class="row">
                <div class="col q-pr-md">
                  <q-input
                    v-model="productName"
                    class="q-pt-md q-pb-md"
                    filled
                    label="Название товара"
                  >
                    <template v-slot:append>
                      <q-icon
                        v-if="productName !== ''"
                        name="close"
                        @click="productName = ''"
                        class="cursor-pointer"
                      />
                    </template>
                  </q-input>
                </div>
                <div class="col q-pr-md">
                  <q-input
                    v-model="productSku"
                    type="number"
                    class="q-pt-md"
                    filled
                    label="Единицы товара"
                  >
                    <template v-slot:append>
                      <q-icon
                        v-if="productSku !== ''"
                        name="close"
                        @click="productSku = ''"
                        class="cursor-pointer"
                      />
                    </template>
                  </q-input>
                </div>
                <div class="col q-pr-md">
                  <q-input
                    v-model="productPrice"
                    class="q-pt-md"
                    filled
                    mask="#.##"
                    prefix="$"
                    fill-mask="0"
                    reverse-fill-mask
                    label="Цена"
                  >
                    <template v-slot:append>
                      <q-icon
                        v-if="productPrice !== ''"
                        name="close"
                        @click="productPrice = ''"
                        class="cursor-pointer"
                      />
                    </template>
                  </q-input>
                </div>
                <div class="col q-pr-md">
                  <q-input
                    v-model="positionSumMoney"
                    class="q-pt-md"
                    filled
                    label="Сумма"
                    prefix="$"
                    readonly
                  >
                  </q-input>
                </div>
              </div>
            </q-form>
            <q-table
              :class="this.positions.length === 0? '':'my-sticky-header-table'"
              style="min-width:500px"
              selection="multiple"
              :selected.sync="selected"
              :data="positions"
              :columns="columns"
              hide-no-data
              row-key="productName"
            />
            <div
              style="border-radious: 5px; min-width:500px; z-index:2"
              v-if="positions.length == 0"
            >
              <q-bar
                style="height: 40px; border-radius: 5px; background-color: lightgrey; text-align: center;"
              >
                <div class="text-orange q-gutter-md" style="font-size: 2em">
                  <q-icon name="warning" style="color: black" />
                </div>
                <div class="text-weight-bold">
                  Данные не найдены
                </div>
              </q-bar>
            </div>
          </q-form>
        </div>
        <div class="q-pt-xl row">
          <div class="q-pr-xs col">
            <q-btn
              label="Очистить"
              type="reset"
              color="primary"
              flat
              class="q-ml-sm"
            />
          </div>
          <div class="col-md-3 offset-md-3 col-lg-2 offset-lg-2">
            <q-btn
              :label="editOrSave ? 'Изменить' : 'Добавить'"
              type="submit"
              color="primary"
            />
          </div>
        </div>
      </q-form>
    </q-page>
  </q-layout>
</template>
<script>
import OrdOps from '../components/OrderOperations.js'
export default {
  props: {
    orderInfo: {
      type: Object,
      required: true
    },
    orderPositionsInfo: {
      type: Array,
      required: true
    },
    editOrSave: {
      type: Boolean,
      required: true
    }
  },
  data () {
    return {
      ordOps: new OrdOps(),
      orderNumber: '',
      creationDate: '',
      customName: '',
      customSurname: '',
      customPatronymic: '',
      sumMoney: 0,
      selected: [],
      productName: '',
      productPrice: '',
      productSku: '',
      positionSumMoney: 0,
      positions: [],
      editOrsave: false,
      warn: '',
      columns: [
        {
          name: 'productName',
          align: 'center',
          label: 'Название товара',
          field: 'productName'
        },
        {
          name: 'productPrice',
          align: 'center',
          label: 'Цена',
          field: 'productPrice'
        },
        {
          name: 'productSku',
          align: 'center',
          label: 'Кол-во',
          field: 'productSku'
        },
        {
          name: 'sumMoney',
          align: 'center',
          label: 'Сумма',
          field: 'sumMoney'
        }
      ]
    }
  },
  async mounted () {
    this.$nextTick(async () => {
      if (this.$props.editOrSave === true) {
        this.orderNumber = this.$props.orderInfo.orderNumber
        this.creationDate = this.$props.orderInfo.creationDate
        this.customName = this.$props.orderInfo.customName
        this.customSurname = this.$props.orderInfo.customSurname
        this.customPatronymic = this.$props.orderInfo.customPatronymic
        this.positions = this.$props.orderPositionsInfo === undefined ? JSON.parse(await this.ordOps.getOrderPositions(this.$props.orderInfo.orderID)) : this.$props.orderPositionsInfo
        this.sumMoney = this.$props.orderInfo.sumMoney
      }
    })
  },
  methods: {
    clearSelected () {
      this.selected = []
      this.positionSumMoney = 0
      this.productSku = ''
      this.productName = ''
      this.productPrice = ''
      this.editOrsave = false
    },
    addPosition () {
      if (this.editOrsave === false) {
        if (this.productName === '' || this.positionSumMoney === 0) {
          this.warn = 'Заполните поля'
        } else {
          this.positions.push({
            productName: this.productName,
            productPrice: this.productPrice,
            productSku: this.productSku,
            sumMoney: this.productSku * this.productPrice
          })
          this.positionSumMoney = 0
          this.productSku = ''
          this.productName = ''
          this.productPrice = ''
          this.editOrsave = false
        }
      } else {
        if (this.productName === '' || this.positionSumMoney === 0) {
          this.warn = 'Заполните поля'
        } else {
          if (this.productName === '' || this.positionSumMoney === 0) {
            this.warn = 'Заполните поля'
          }
          this.positions.splice(this.selected[0], this.selected.length)
          this.positions.push({
            productName: this.productName,
            productPrice: this.productPrice,
            productSku: this.productSku,
            sumMoney: this.productSku * this.productPrice
          })
          this.positionSumMoney = 0
          this.productSku = ' '
          this.productName = ' '
          this.productPrice = ' '
          this.editOrsave = false
          this.selected = []
        }
      }
    },
    reset () {
      this.orderNumber = ''
      this.creationDate = null
      this.customName = ''
      this.customSurname = ''
      this.customPatronymic = ''
      this.positions = []
      this.positionSumMoney = ''
      this.productSku = ''
      this.productName = ''
      this.productPrice = ''
      this.sumMoney = 0
      this.selected = []
      this.editOrsave = false
      this.warn = ''
    },
    async saveOrder () {
      debugger
      await this.ordOps.saveOrder(
        this.orderNumber,
        this.creationDate,
        this.customName,
        this.customSurname,
        this.customPatronymic,
        this.positions
      )
      this.$emit('addOrder')
      this.editOrsave = false
      this.$props.editOrSave = false
    },
    async editOrder () {
      debugger
      await this.ordOps.editOrder(
        this.$props.orderInfo.orderID === 0 || this.$props.orderInfo.orderID === null || this.$props.orderInfo.orderID === undefined ? this.$props.orderInfo.id : this.$props.orderInfo.orderID,
        this.orderNumber,
        this.creationDate,
        this.customName,
        this.customSurname,
        this.customPatronymic,
        this.sumMoney,
        this.positions
      )
      debugger
      this.$emit('addOrder')
      this.$props.editOrSave = false
    },
    editPosition () {
      debugger
      this.editOrsave = true
      this.positionSumMoney = this.selected[0].sumMoney
      this.productSku = this.selected[0].productSku
      this.productName = this.selected[0].productName
      this.productPrice = this.selected[0].productPrice
      this.editOrsave = true
    },
    deletePosition () {
      this.positions.splice(this.selected[0], this.selected.length)
      this.selected = []
      this.editOrsave = false
    }
  },
  watch: {
    positions: async function () {
      this.sumMoney = 0
      for (var i = 0; i < this.positions.length; i++) {
        this.sumMoney =
         Number(this.sumMoney + Number(this.positions[i].sumMoney))
      }
    },
    productPrice: async function () {
      this.positionSumMoney = this.productPrice * this.productSku
    },
    productSku: async function () {
      this.positionSumMoney = this.productPrice * this.productSku
    },

    productName: async function () {
      this.warn = ''
    },
    positionSumMoney: async function () {
      this.warn = ''
    }
  }
}
</script>
<style lang="sass">
.my-sticky-header-table
  /* height or max-height is important */
  height: 310px

  .q-table__top,
  .q-table__bottom,
  thead tr:first-child th
    /* bg color is important for th; just specify one */
    background-color: #c1f4cd

  thead tr th
    position: sticky
    z-index: 1
  thead tr:first-child th
    top: 0

  /* this is when the loading indicator appears */
  &.q-table--loading thead tr:last-child th
    /* height of all previous header rows */
    top: 48px
</style>
