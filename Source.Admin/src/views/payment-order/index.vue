<template>
  <div class="app-container">
    <div class="filter-container" >
      <el-select v-model="listQuery.payMethod" placeholder="支付方式" clearable style="width: 120px" class="filter-item">
        <el-option v-for="item in payMethodOptions" :key="item.key" :label="item.label" :value="item.key" />
      </el-select>
          <el-select v-model="listQuery.orderState" placeholder="订单状态" clearable style="width: 120px" class="filter-item">
        <el-option v-for="item in orderStateOptions" :key="item.key" :label="item.label" :value="item.key" />
      </el-select>
      <el-button v-waves class="filter-item" type="primary" icon="el-icon-search" @click="handleFilter">
         查询
      </el-button>
    </div>

    <el-table :key="tableKey" v-loading="listLoading" :data="list" border fit highlight-current-row style="width: 100%;">
      <el-table-column label="序号" type="index" align="center" width="70" />
      <el-table-column label="下单时间" align="center">
        <template slot-scope="{row}">
          <span>{{ row.createdTime | parseTime('{y}-{m}-{d} {h}:{i}') }}</span>
        </template>
      </el-table-column>
      <el-table-column label="支付方式" align="center">
        <template slot-scope="{row}">
          <el-tag>{{ row.payMethod | payMethodFilter }}</el-tag>
        </template>
      </el-table-column>
      <el-table-column label="订单状态" align="center">
        <template slot-scope="{row}">
          <el-tag>{{ row.orderState | orderStateFilter }}</el-tag>
        </template>
      </el-table-column>
      <el-table-column label="金额" prop="amount" align="center" />
    </el-table>

     <pagination v-show="total>0" :total="total" :page.sync="listQuery.pageIndex" :limit.sync="listQuery.pageSize" @pagination="getList" />

  </div>
</template>

<script>
import { getPaymentOrders } from '@/api/payment-order'
import waves from '@/directive/waves'
import Pagination from '@/components/Pagination'
import { payMethodOptions,orderStateOptions, payMethodKeyValue, orderStateKeyValue} from '@/utils/dict'

export default {
  name: 'PaymentOrder',
  components: { Pagination },
  directives: { waves },
  filters: {
    payMethodFilter(method) {
      return payMethodKeyValue[method] || ''
    },
    orderStateFilter(state) {
      return orderStateKeyValue[state] || ''
    }
  },
  data() {
    return {
      tableKey: 0,
      list: null,
      total: 0,
      listLoading: true,
      listQuery: {
        key: undefined,
        orderType:undefined,
        payMethod:undefined,
        orderState:undefined,
        pageIndex: 1,
        pageSize: 10
      },
      payMethodOptions,
      orderStateOptions,
    }
  },
  created() {
    this.getList()
  },
  methods: {
    getList() {
      this.listLoading = true
      getPaymentOrders(this.listQuery).then(res => {
        this.list = res.data.data
        this.total = res.data.total
        this.listLoading = false
      })
    },
    handleFilter() {
      this.listQuery.page = 1
      this.getList()
    }
  }
}
</script>
<style scoped>
  .el-table,.el-button,.el-tag{
    font-size: 16px!important;
  }
</style>

