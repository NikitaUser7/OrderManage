<template>
  <q-page style="min-width: 600px">
    <div class="row">
      <q-toolbar class="text-primary col-12" style="outline: 1px solid #09598c;">
      <q-btn
        icon="arrow_back_ios"
        style="color: black"
        label="Назад"
        size="11px"
        @click="closeCurrentOrder()"
      />
      <q-toolbar-title class="q-pl-xl text-black">
        Подробнее о заказе
      </q-toolbar-title>
      <q-btn stretch flat label="Изменить" @click="editOrder" />
      <q-separator dark vertical />
      <q-btn stretch flat label="Удалить" @click="deleteOrder" />
    </q-toolbar>
    </div>
    <div class="q-pa-md">
      <div class="row items-start">
        <div class="col-xs-12 col-sm-6 col-md-4 col-lg-3 col-xl-3">
          <q-card class="q-ma-sm">
            <q-card-section>
              <div class="text-h6">Дата создания</div>
              <div class="text-subtitle1">{{ this.creationDate }}</div>
            </q-card-section>
          </q-card>
        </div>

        <div class="col-xs-12 col-sm-6 col-md-4 col-lg-3 col-xl-3">
          <q-card class="q-ma-sm">
            <q-card-section>
              <div class="text-h6">Номер заказа</div>
              <div class="text-subtitle1">{{ this.orderNumber }}</div>
            </q-card-section>
          </q-card>
        </div>

        <div class="col-xs-12 col-sm-6 col-md-4 col-lg-3 col-xl-3">
          <q-card class="q-ma-sm">
            <q-card-section>
              <div class="text-h6">Фио клиента</div>
              <div class="text-subtitle1">{{ this.customNSP }}</div>
            </q-card-section>
          </q-card>
        </div>

        <div class="col-xs-12 col-sm-6 col-md-4 col-lg-3 col-xl-3">
          <q-card class="q-ma-sm">
            <q-card-section>
              <div class="text-h6">Сумма</div>
              <div class="text-subtitle1">{{ this.sumMoney }}</div>
            </q-card-section>
          </q-card>
        </div>
      </div>
    </div>
    <q-form class="col-8 q-pl-lg" @submit="addPosition">
      <q-dialog v-model="toolpit">
      <q-card>
        <q-card-section>
          <div class="text-h6">Ошибка</div>
        </q-card-section>

        <q-card-section class="q-pt-none" style="color: red">
          Не возможно удалить все позиции заказа
        </q-card-section>

        <q-card-actions align="right">
          <q-btn flat label="Отменить" color="primary" v-close-popup />
        </q-card-actions>
      </q-card>
    </q-dialog>
      <div class="row wrap justify-between" style="min-width:500px">
        <q-toolbar class="col-xs-auto col-sm-auto col-md-auto">
          <div>
            <strong
              style="font-color: black; color:black"
              class="text-h6 q-pl-lg"
              >Позиции заказа</strong
            >
          </div>
        </q-toolbar>
        <q-toolbar class="col-xs-auto col-sm-auto col-md-auto bg-white text-black">
          <q-btn
            stretch
            flat
            :label="editOrsave === false ? 'Добавить позицию' : 'Сохранить'"
            type="submit"
          />
          <q-separator dark vertical />
          <q-btn
            stretch
            flat
            label="Изменить"
            :disable="this.selected.length === 1 ? false : true"
            :style="
              this.selected.length === 1
                ? 'color: blue; disable'
                : 'color: grey'
            "
            @click="editPosition"
          />
          <q-separator dark vertical />
          <q-btn
            stretch
            flat
            label="Удалить"
            :disable="this.selected.length === 0 ? true : false"
            :style="
              this.selected.length === 0
                ? 'color: grey; disable'
                : 'color: blue'
            "
            @click="deletePosition()"
          />
          <q-btn
            stretch
            flat
            :label="editOrsave === true? 'Отменить' :'Очистить выбор'"
            v-if="this.selected.length !== 0"
            @click="clearSelected"
          />
        </q-toolbar>
      </div>
        <div class="row">
          <div class="col q-pr-md">
            <q-input
              ref="field"
              v-model="productName"
              class="q-pt-md q-pb-md"
              filled
              label="Название товара"
              lazy-rules
              :rules="[val => (val && val.length > 0) || 'Заполните поле']"
            >
              <template v-slot:append>
                <q-icon
                  name="close"
                  @click="clearNameOrder"
                  class="cursor-pointer"
                />
              </template>
            </q-input>
          </div>
          <div class="col q-pr-md">
            <q-input
              v-model="productSku"
              class="q-pt-md"
              filled
              mask="#"
              fill-mask="0"
              reverse-fill-mask
              label="Единицы товара"
              lazy-rules
              :rules="[val => (val && val.length > 0) || 'Заполните поле']"
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
              lazy-rules
              :rules="[val => (val && val.length > 0) || 'Заполните поле']"
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
      <div class="q-pr-md">
        <q-table
          :class="this.positions.length === 0 ? '' : 'my-sticky-header-table'"
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
      </div>
    </q-form>
    <q-dialog v-model="editOrderDialog">
      <AddOrderDialog
        :orderInfo="opnOrder"
        :editOrSave="true"
        :orderPositionsInfo="positions"
        @addOrder="emitAddOrderDialog"
      />
    </q-dialog>
  </q-page>
</template>
<script>
import AddOrderDialog from './AddOrderDialog.vue'
import OrdOps from '../components/OrderOperations.js'
export default {
  props: {
    opnOrder: {
      type: Object,
      required: true
    }
  },
  components: {
    AddOrderDialog
  },
  data () {
    return {
      ordOps: new OrdOps(),
      orderNumber: '',
      creationDate: '',
      customNSP: '',
      sumMoney: 0,
      selected: [],
      productName: '',
      productPrice: '',
      productSku: '',
      positionSumMoney: 0,
      positions: [],
      editOrsave: false,
      toolpit: false,
      editOrderDialog: false,
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
      this.orderNumber = this.$props.opnOrder.orderNumber
      this.creationDate = this.$props.opnOrder.creationDate
      this.customNSP = this.$props.opnOrder.customNSP
      this.sumMoney = this.$props.opnOrder.sumMoney
      await this.getPositions()
    })
  },
  methods: {
    closeCurrentOrder () {
      this.$emit('closeCurrentOrder')
    },
    clearSelected () {
      this.selected = []
      this.positionSumMoney = 0
      this.productSku = ''
      this.productName = ''
      this.productPrice = ''
      this.editOrsave = false
      this.$refs.field.resetValidation()
    },
    async getPositions () {
      this.positions = JSON.parse(
        await this.ordOps.getOrderPositions(this.$props.opnOrder.id)
      )
    },
    async addPosition () {
      if (this.editOrsave === false) {
        await this.ordOps.saveOrderPosition(
          this.$props.opnOrder.id,
          this.productName,
          this.productPrice,
          this.productSku
        )
        this.positionSumMoney = 0
        this.productSku = ''
        this.productName = ''
        this.productPrice = ''
        this.editOrsave = false
        this.$refs.field.resetValidation()
      } else {
        const ids = this.selected.map(d => d.id)
        await this.ordOps.editOrderPosition(ids[0], this.productName, this.productPrice, this.productSku, this.positionSumMoney)
        this.positionSumMoney = 0
        this.productSku = ''
        this.productName = ''
        this.productPrice = ''
        this.editOrsave = false
        this.selected = []
        this.$refs.field.resetValidation()
      }
      this.getPositions()
    },
    async editOrder () {
      this.editOrderDialog = true
    },
    async emitAddOrderDialog () {
      this.editOrderDialog = false
      this.orderNumber = this.$props.opnOrder.orderNumber
      this.creationDate = this.$props.opnOrder.creationDate
      this.customNSP = this.$props.opnOrder.customNSP
      this.sumMoney = this.$props.opnOrder.sumMoney
      await this.getPositions()
    },
    editPosition () {
      this.editOrsave = true
      this.positionSumMoney = this.selected[0].sumMoney
      this.productSku = this.selected[0].productSku
      this.productName = this.selected[0].productName
      this.productPrice = this.selected[0].productPrice
    },
    async deletePosition () {
      if (this.positions.length === this.selected.length) {
        this.toolpit = true
      } else {
        const ids = this.selected.map(d => d.id)
        await this.ordOps.removeOrderPosition(ids)
        this.selected = []
        this.editOrsave = false
        await this.getPositions()
      }
    },
    clearNameOrder () {
      this.productName = ''
      this.$refs.field.resetValidation()
    },
    async deleteOrder () {
      const ar = []
      ar.push(this.$props.opnOrder.id)
      await this.ordOps.removeOrder(ar)
      this.$emit('deleteOrder')
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
    }
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
