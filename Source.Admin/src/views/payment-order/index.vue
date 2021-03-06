<template>
  <div class="app-container">
    <div class="filter-container">
      <el-input v-model="listQuery.key" placeholder="产品" style="width: 200px;" class="filter-item" @keyup.enter.native="handleFilter" />
      <el-select v-model="listQuery.orderType" placeholder="订单类型" clearable style="width: 120px" class="filter-item">
        <el-option v-for="item in orderTypeOptions" :key="item.key" :label="item.label" :value="item.key" />
      </el-select>
      <el-select v-model="listQuery.orderState" placeholder="订单状态" clearable style="width: 120px" class="filter-item">
        <el-option v-for="item in productOrderStateOptions" :key="item.key" :label="item.label" :value="item.key" />
      </el-select>
      <el-button v-waves class="filter-item" type="primary" icon="el-icon-search" @click="handleFilter">
        查询
      </el-button>
    </div>

    <el-table :key="tableKey" v-loading="listLoading" :data="list" border fit highlight-current-row style="width: 100%;">
      <el-table-column label="序号" type="index" align="center" width="70" />
      <el-table-column label="下单时间" align="center" width="150">
        <template slot-scope="{row}">
          <span>{{ row.createdTime | parseTime('{y}-{m}-{d} {h}:{i}') }}</span>
        </template>
      </el-table-column>
      <el-table-column label="产品" prop="content" align="center" min-width="150" />
      <el-table-column label="订单类型" align="center" width="150">
        <template slot-scope="{row}">
          <el-tag>{{ row.orderType | orderTypeFilter }}</el-tag>
        </template>
      </el-table-column>
      <el-table-column label="订单状态" align="center" width="150">
        <template slot-scope="{row}">
          <el-tag>{{ row.orderState | orderStateFilter }}</el-tag>
        </template>
      </el-table-column>
      <el-table-column label="数量" prop="quantity" align="center" width="150" />
      <el-table-column label="金额" prop="amount" align="center" width="150" />
    </el-table>

    <pagination v-show="total>0" :total="total" :page.sync="listQuery.pageIndex" :limit.sync="listQuery.pageSize" @pagination="getList" />

  </div>
</template>

<script>
import { getPaymentOrders } from '@/api/payment-order'
import waves from '@/directive/waves'
import Pagination from '@/components/Pagination'
import { orderTypeOptions, payMethodOptions, productOrderStateOptions, orderTypeKeyValue, payMethodKeyValue, productOrderStateKeyValue } from '@/utils/dict'

export default {
  name: 'PaymentOrder',
  components: { Pagination },
  directives: { waves },
  filters: {
    orderTypeFilter(type) {
      return orderTypeKeyValue[type] || ''
    },
    payMethodFilter(method) {
      return payMethodKeyValue[method] || ''
    },
    orderStateFilter(state) {
      return productOrderStateKeyValue[state] || ''
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
        orderType: undefined,
        payMethod: undefined,
        orderState: undefined,
        pageIndex: 1,
        pageSize: 10
      },
      orderTypeOptions,
      payMethodOptions,
      productOrderStateOptions
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

