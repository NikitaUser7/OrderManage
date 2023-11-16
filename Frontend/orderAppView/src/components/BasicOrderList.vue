<template>
  <q-page style="min-width: 800px">
    <div class="row wrap shadow-1">
      <q-toolbar class="col-4">
        <div>
          <strong style="font-color: black; color:black" class="text-h4 q-pl-lg"
            >Учет заказов</strong
          >
        </div>
      </q-toolbar>
      <q-toolbar class="col-xs-auto col-sm-auto col-md-8 bg-white text-black">
        <q-space />
        <q-input
          v-model="search.string"
          :label="'Поиск по: ' + searchByColumn"
          style="width:400px"
          v-if="
            this.searchByColumn == 'orderNumber' ||
              this.searchByColumn == 'customSurname' ||
              this.searchByColumn == 'orderNumber' ||
              this.searchByColumn == 'positionName'
          "
        >
          <template v-slot:append>
            <q-icon
              name="close"
              @click="search.string = ''"
              class="cursor-pointer"
            />
          </template>
        </q-input>
        <q-input
          v-model="search.date"
          type="date"
          style="width:400px"
          v-if="this.searchByColumn == 'creationDate'"
          ><template v-slot:append>
            <q-icon
              name="close"
              @click.stop="search.date = null"
              class="cursor-pointer"
            />
          </template>
        </q-input>
        <q-btn-dropdown
          stretch
          flat
          label="Установить диапазон дат"
          v-if="this.searchByColumn == 'dateRange'"
        >
              <q-item-label>
                  <q-date
                  v-model="dateDiapazone"
                  range
                >
                <div class="column items-center">
                  <div class="col">
                    <q-icon
                      name="close"
                      @click="dateDiapazone = null"
                      class="cursor-pointer"
                      size="sm"
                      v-if="this.searchByColumn == 'dateRange'"
                      style="position:center"
                    />
                  </div>
                  </div>
                </q-date>
              </q-item-label>
        </q-btn-dropdown>
        <q-btn-dropdown stretch flat>
          <q-list>
            <q-item
              clickable
              v-close-popup
              @click="selectSearchBy('creationDate')"
            >
              <q-item-section>
                <q-item-label>Дате создания</q-item-label>
              </q-item-section>
            </q-item>

            <q-item
              clickable
              v-close-popup
              @click="selectSearchBy('orderNumber')"
            >
              <q-item-section>
                <q-item-label>Номеру заказа</q-item-label>
              </q-item-section>
            </q-item>

            <q-item
              clickable
              v-close-popup
              @click="selectSearchBy('customSurname')"
            >
              <q-item-section>
                <q-item-label>Фамилии клиента</q-item-label>
              </q-item-section>
            </q-item>

            <q-item
              clickable
              v-close-popup
              @click="selectSearchBy('positionName')"
            >
              <q-item-section>
                <q-item-label>Названию товара</q-item-label>
              </q-item-section>
            </q-item>

            <q-item
              clickable
              v-close-popup
              @click="selectSearchBy('dateRange')"
            >
              <q-item-section>
                <q-item-label>Диапазону дат</q-item-label>
              </q-item-section>
            </q-item>
          </q-list>
        </q-btn-dropdown>
        <q-btn stretch flat label="Добавить заказ" @click="openAddOrder()" />
        <q-separator dark vertical />
        <q-btn
          stretch
          flat
          label="Изменить"
          :disable="this.selected.length === 1 ? false : true"
          :style="
            this.selected.length === 1 ? 'color: blue; disable' : 'color: grey'
          "
          @click="editOrder()"
        />
        <q-separator dark vertical />
        <q-btn
          stretch
          flat
          label="Удалить"
          :disable="this.selected.length === 0 ? true : false"
          :style="
            this.selected.length === 0 ? 'color: grey; disable' : 'color: blue'
          "
          @click="deleteOrder()"
        />
        <q-btn
          stretch
          flat
          label="Очистить выбор"
          v-if="this.selected.length !== 0"
          @click="resetOrder()"
        />
      </q-toolbar>
    </div>
    <q-table
      selection="multiple"
      :selected.sync="selected"
      :data="orders"
      :columns="columns"
      :pagination.sync="pagination"
      @request="getOrdersPagination"
      hide-no-data
      @row-dblclick="openCurrentOrder"
    />

    <div style="border-radious: 5px" v-if="orders.length == 0">
      <q-bar
        style="height: 40px; border-radius: 5px; background-color: lightgrey; text-align: center"
      >
        <div class="text-orange q-gutter-md" style="font-size: 2em">
          <q-icon name="warning" style="color: black" />
        </div>
        <div class="text-weight-bold">
          Данные не найдены
        </div>
      </q-bar>
    </div>
    <q-dialog v-model="AddDialog">
      <AddOrderDialog
        :orderInfo="editOrderInfo"
        :editOrSave="editOrSave"
        @addOrder="emitAddOrderDialog"
      />
    </q-dialog>
  </q-page>
</template>
<script>
import AddOrderDialog from './AddOrderDialog.vue'
export default {
  props: {
    ordOps: {
      type: Object,
      required: true
    }
  },
  components: {
    AddOrderDialog
  },
  data () {
    return {
      selected: [],
      searchByColumn: 'orderNumber',
      loading: false,
      AddDialog: false,
      pagination: {
        sortBy: 'creationDate',
        descending: false,
        page: 0,
        rowsPerPage: 4,
        rowsNumber: 10
      },
      dateDiapazone: {},
      search: {
        string: '',
        date: null,
        dateA: null,
        dateZ: null
      },
      orders: [],
      editOrderInfo: {},
      editOrSave: false,
      test: '',

      columns: [
        {
          name: 'creationDate',
          align: 'center',
          label: 'Дата создания',
          sortable: true,
          field: 'creationDate'
        },
        {
          name: 'orderNumber',
          align: 'center',
          sortable: false,
          label: 'Номер заказа',
          field: 'orderNumber'
        },
        {
          name: 'customNSP',
          align: 'center',
          label: 'ФИО клиента',
          field: 'customNSP',
          sortable: true
        },
        {
          name: 'sumMoney',
          align: 'center',
          label: 'Сумма',
          field: 'sumMoney',
          sortable: true
        }
      ]
    }
  },
  async mounted () {
    this.$nextTick(async () => {
      await this.getOrders()
    })
  },
  methods: {
    async emitAddOrderDialog () {
      this.AddDialog = false
      this.editOrSave = false
      await this.getOrders()
      this.selected = []
    },
    async getOrdersPagination (props) {
      this.pagination = props.pagination
      await this.getOrders()
    },
    async getOrders () {
      debugger
      this.pagination.rowsNumber = Number.parseInt(
        await this.ordOps.getOrdersNumber(
          this.pagination.sortBy,
          this.pagination.descending,
          this.searchByColumn,
          this.search,
          this.pagination.page - 1,
          this.pagination.rowsPerPage
        )
      )

      this.orders = JSON.parse(
        await this.ordOps.getOrders(
          this.pagination.sortBy,
          this.pagination.descending,
          this.searchByColumn,
          this.search,
          this.pagination.page - 1,
          this.pagination.rowsPerPage
        )
      )
      debugger
    },
    async editOrder () {
      const ids = this.selected.map(d => d.id)
      this.editOrderInfo = JSON.parse(await this.ordOps.editOrderInfo(ids[0]))
      this.editOrSave = true
      this.AddDialog = true
    },
    async deleteOrder () {
      const ids = this.selected.map(d => d.id)
      await this.ordOps.removeOrder(ids)
      this.pagination.page = 1
      await this.getOrders()
      this.selected = []
    },
    openAddOrder () {
      this.editOrderInfo = {}
      this.editOrSave = false
      this.AddDialog = true
    },
    resetOrder () {
      this.selected = []
    },
    selectSearchBy (colnam) {
      this.searchByColumn = colnam
      this.dateDiapazone = null
      this.search.string = ''
      this.search.date = null
      this.search.dateA = null
      this.search.dateZ = null
    },
    openCurrentOrder (evt, row, index) {
      this.$emit('OrderClick', this.orders[index])
    }
  },
  watch: {
    'search.string': async function () {
      this.pagination.page = 1
      await this.getOrders()
    },
    'search.date': async function () {
      this.pagination.page = 1
      await this.getOrders()
    },
    dateDiapazone: async function () {
      if (this.dateDiapazone === null) {
        this.search.dateA = null
        this.search.dateZ = null
        this.pagination.page = 1
        await this.getOrders()
      }
      this.search.dateA = this.dateDiapazone.from
      this.search.dateZ = this.dateDiapazone.to
      this.pagination.page = 1
      await this.getOrders()
      this.search.dateA = null
      this.search.dateZ = null
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
